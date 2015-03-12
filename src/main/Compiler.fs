namespace FunScript

open AST
open Applications
open Constructors
open InternalCompiler
open System
open System.IO
open System.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Reflection

type internal CompiledType(cons: JSStatement, meths: Map<string, JSStatement>, infcs: Map<string, CompiledType>) =
   member __.Methods                   = meths
   member __.Interfaces                = infcs
   member __.WithMethod(name, meth)    = CompiledType(cons, meths.Add(name, meth), infcs)
   member __.WithInterface(name, infc) = CompiledType(cons, meths, infcs.Add(name, infc))
   member __.Print(stream: TextWriter) =
      stream.Write (cons.PrintGlobal())
      meths |> Seq.iter (fun x -> x.Value.PrintGlobal() |> stream.Write)
      infcs |> Seq.iter (fun x -> x.Value.Print(stream))

type internal TypeBag private(ctypes: Map<string, CompiledType>, unchkdStments: JSStatement list, boot: JSStatement) =
   // NOTE: In Coerce expressions FullName property of type becomes null. Why?
   let nameof (t: Type) =
      let rec parentName (t: Type) =
         if t.IsNested then (parentName t.DeclaringType) + t.DeclaringType.Name + "+" else ""
      let ns = if String.IsNullOrEmpty(t.Namespace) then "" else t.Namespace + "."
      ns + (parentName t) + t.Name

   static member Create (com: Compiler, expr: Expr) =
      let boot = com.CompileStatement ReturnFrom expr
      TypeBag(Map.empty, [boot], boot)

   member __.Types               = ctypes
   member __.UncheckedStatements = unchkdStments
   member __.ResetStatements()   = TypeBag(ctypes, [], boot)

   member self.AddType(com: Compiler, t: Type) =
      let tname = nameof t
      if self.Types.ContainsKey(tname) then self
      else
         let stment = compileConstructor com t
         let ctype = CompiledType(stment, Map.empty, Map.empty)
         TypeBag(self.Types.Add(tname, ctype), stment::unchkdStments, boot)

   member self.AddMethod(com: Compiler, t: Type, name: string, genStment: unit->JSStatement) =
      let self, tname = self.AddType(com, t), nameof t
      if self.Types.[tname].Methods.ContainsKey(name) then self
      else
         let stment = genStment()
         let ctype = self.Types.[tname].WithMethod(name, stment)
         TypeBag(self.Types.Add(tname, ctype), stment::unchkdStments, boot)
   
   member self.AddInterface(com: Compiler, impl: Type, infc: Type) =
      let self, implName, infcName = self.AddType(com, impl), nameof impl, nameof infc
      if self.Types.[implName].Interfaces.ContainsKey(infcName) then self
      else
         let map = impl.GetInterfaceMap(infc)
         let meths = map.InterfaceMethods |> Array.zip map.TargetMethods
                     |> Array.map (fun (implMeth, infcMeth) ->
                        string infcMeth, Applications.compileMethod(com, implMeth, Some infcMeth))
                     |> Array.toList
         let cons = AssignGlobal(PropertyGet(refType impl, refType infc), JSExpr.Object [])
         let ctype = self.Types.[implName].WithInterface(infcName, CompiledType(cons, Map(meths), Map.empty))
         TypeBag(self.Types.Add(implName, ctype), (List.map snd meths)@unchkdStments, boot)

   member __.Print(stream: TextWriter) =
      JSMapper.resetCache()
      stream.WriteLine "(function(){"
      stream.WriteLine "var ns={};"
      ctypes |> Seq.iter (fun ct -> ct.Value.Print(stream))
      stream.WriteLine (boot.PrintGlobal())
      stream.Write "}());"

type FSCompiler =
   static member private compile(expr: Expr, writer: TextWriter) =
      let compileGlobalRef (com: Compiler) (tbag: TypeBag) (e: JSExpr) =
         match e with
         | Reference ref ->
            match ref with
            | Method meth when (not meth.DeclaringType.IsInterface) ->
               tbag.AddMethod(com, meth.DeclaringType, string meth,
                              fun () -> compileMethod(com, meth, None))
            | Case uci ->
               tbag.AddMethod(com, uci.DeclaringType, uci.Name,
                              fun () -> compileUciConstructor uci)
            | Type t ->
               if t.IsGenericParameter then tbag
               elif t.IsInterface then tbag
               else tbag.AddType(com, t)
            | _ -> tbag
         | Coerce(impl, infc, _) ->
            let rec getInterfaces (infc: Type) =
               let subinfcs = infc.GetInterfaces() |> List.ofArray
               match subinfcs with
               | [] -> [infc]
               | _ -> infc::(subinfcs |> List.map getInterfaces |> List.concat)
            getInterfaces infc |> List.fold (fun (tbag: TypeBag) infc ->
               tbag.AddInterface(com, impl, infc)) tbag
         | _ -> tbag

      // TODO: Make this more type-safe?
      let rec traverseJsi (com: Compiler) (tbag: TypeBag) (jsi: obj) =
         let tbag, (_, items) =
            match jsi with
            | :? JSExpr as jse ->
               compileGlobalRef com tbag jse, FSharpValue.GetUnionFields(jse, typeof<JSExpr>, true)
            | :? JSStatement as jss ->
               tbag, FSharpValue.GetUnionFields(jss, typeof<JSStatement>, true)
            | _ -> failwith "Please report: Non-JS-instruction detected while traversing AST"
         items
         |> Array.fold (fun (tbag: TypeBag) jsi -> 
            match jsi with
            | :? JSExpr
            | :? JSStatement -> traverseJsi com tbag jsi
            | :? List<JSExpr> as jsis ->
                  jsis |> List.fold (fun (tbag: TypeBag) jsi ->traverseJsi com tbag jsi) tbag
            | :? List<string*JSExpr> as tups ->
                  tups |> List.fold (fun (tbag: TypeBag) (_,jsi) ->traverseJsi com tbag jsi) tbag
            | _ -> tbag) tbag

      let rec checkTypeBag (com: Compiler) (tbag: TypeBag) =
         let stments, tbag = tbag.UncheckedStatements, tbag.ResetStatements()
         stments
         |> List.fold (fun (tbag: TypeBag) stment -> traverseJsi com tbag stment) tbag
         |> fun tbag -> if tbag.UncheckedStatements.Length = 0
                        then tbag
                        else checkTypeBag com tbag

      try
         let com = Compiler [ yield! PrimitiveTypes.components
                              yield! GetSet.components
                              yield! Comparisons.components
                              yield! LetBindings.components
                              yield! ControlFlow.components
                              yield! Conversions.components
                              yield! Constructors.components
                              yield! Applications.components ]
         let typeBag = TypeBag.Create(com, expr) |> checkTypeBag com
         typeBag.Print(writer)
      with
      | ex -> printfn "ERROR: %s" ex.Message

   static member CompileTo(expr: Expr, filePath: string, ?baseDir: string) =
      let baseDir = defaultArg baseDir <| Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
      let filePath = Path.Combine(baseDir, filePath)
      let folder = Path.GetDirectoryName(filePath)
      if not (Directory.Exists folder) then Directory.CreateDirectory(folder) |> ignore
      use stream = File.OpenWrite(Path.Combine(baseDir, filePath))
      use writer = new StreamWriter(stream)
      FSCompiler.compile(expr, writer)

   static member Compile(expr: Expr) =
      use writer = new StringWriter()
      FSCompiler.compile(expr, writer)
      writer.ToString()

[<assembly:AutoOpen("FunScript.Core")>]
[<assembly:AutoOpen("FunScript.Core.Collections")>]
do()
