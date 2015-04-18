namespace FunScript

open AST
open Applications
open Constructors
open System
open System.IO
open System.Reflection
open System.Collections.Generic
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Reflection

type internal CompiledType (cons: JSStatement, meths: Dictionary<int, string*JSStatement>) =
   let infcs = Dictionary<Type, CompiledType>()
   new (cons) = CompiledType(cons, Dictionary<_,_>())
   member __.Methods                   = meths
   member __.Interfaces                = infcs
   member __.Print(stream: TextWriter) =
      stream.Write (cons.Print 0 Map.empty)
      __.Methods    |> Seq.iter (fun x -> (snd x.Value).Print 0 Map.empty |> stream.Write)
      __.Interfaces |> Seq.iter (fun x -> x.Value.Print(stream))

type CompileOptions() =
   member val noReturn = false with get, set
   member val enclose = true with get, set // TODO: User must be able to set the closure
   member val compress = false with get, set

type internal InternalCompiler(options: CompileOptions,
                               typeMappings: Map<string, Type>,
                               components: CompilerComponent list) as this =
   let ctypes = Dictionary<Type, CompiledType>()

   let compile ret expr =
      components |> List.tryPick (fun part -> part this ret expr) |> function
      | Some res -> res
      | None -> failwithf "%s%s%A"
                  "Cannot compile the quotation below. Check if FunScript.JS attribute is missing. "
                  "If that doesn't help, please report the issue." expr

   let compileStatement ret expr = 
      match compile ret expr with
      | Statement jsSt -> jsSt
      | Expr jsEx -> ret.Return(expr.DebugInfo, jsEx)

   let compileExpr expr = 
      match compile ReturnFrom expr with
      | Expr jsEx -> jsEx
      | Statement jsSt -> Apply(Lambda([], jsSt), [])

   let addType (t: System.Type) =
      if t.IsGenericParameter then
         t
      else
         let t =
            if typeMappings.ContainsKey t.Name
            then if typeMappings.[t.Name].GetConstructors().Length > 0 then typeMappings.[t.Name] else t
            else if t.IsGenericType then t.GetGenericTypeDefinition() else t
         if not(t.IsInterface) && not(ctypes.ContainsKey t) then
            ctypes.Add(t, Unchecked.defaultof<CompiledType>)
            ctypes.[t] <- CompiledType(compileConstructor this t)
         t

   let addMethod (t: Type) (id: int) (name: string) (genStment: Type->string->JSStatement) =
      let t = addType t
      let ctype = ctypes.[t]
      if ctype.Methods.ContainsKey id then
         t, fst ctype.Methods.[id]
      else
         let methName = JSMapper.preventConflicts (fun s ->
            ctype.Methods.Values |> Seq.exists (fun (s',_) -> s = s')) name
         ctype.Methods.Add(id, (methName, Unchecked.defaultof<JSStatement>))
         ctype.Methods.[id] <- (methName, genStment t methName)
         t, methName

   let refType t =
      if   t = typeof<unit> then JSExpr.String("undefined")
      elif t = typeof<bool> then JSExpr.String("boolean")
      elif t = typeof<string> || t = typeof<char> then JSExpr.String("string")
      elif t.IsPrimitive || t.IsEnum then JSExpr.String("number")
      elif t = typeof<obj> then JSExpr.String("object")
      elif t.IsArray || t.Name.StartsWith("Tuple") then JSExpr.String("Array") // TODO: Should `:? Tuple` match arrays? See Conversions.typeTest
      elif FSharpType.IsFunction t || typeof<System.Delegate>.IsAssignableFrom(t) then JSExpr.String("function")
      else JSExpr.Type(addType t)
      
   let refCase (uci: UnionCaseInfo) =
      // Use negative tags to prevent clashing with MethodBase.MetadataToken
      let t, name = addMethod uci.DeclaringType (-uci.Tag) uci.Name (fun t name ->
         let uci = FSharpType.GetUnionCases(t, true) |> Array.find (fun u -> u.Tag = uci.Tag)
         let uciRef = PropertyGet(JSExpr.Type t, JSExpr.String name)
         compileUciConstructor uci (JSExpr.Type t) uciRef)
      PropertyGet(JSExpr.Type t, JSExpr.String name)

   let refMethod (meth: MethodBase) target =
      if meth.DeclaringType.IsInterface then
         let infc = if meth.DeclaringType.IsGenericType
                    then meth.DeclaringType.GetGenericTypeDefinition()
                    else meth.DeclaringType
         match target with
         | None -> failwith "Please report: Interface method referred as static"
         | Some target -> PropertyGet(
                              PropertyGet(PropertyGet(target, JSExpr.String "constructor"), JSExpr.Type infc),
                              // TODO: Warning! An interface can also have overloaded methods (see also addInterface)
                              JSExpr.String meth.Name)
      else
         let t, name = addMethod meth.DeclaringType meth.MetadataToken meth.Name (fun t name ->
            let genMeth = Seq.cast<MethodBase> (t.GetMethods BindingFlags.All)
                          |> Seq.append (Seq.cast<MethodBase> (t.GetConstructors BindingFlags.All))
                          |> Seq.find (fun m -> m.MetadataToken = meth.MetadataToken)
            let methRef = PropertyGet(JSExpr.Type t, JSExpr.String name)
            compileMethod this genMeth methRef)
         PropertyGet(JSExpr.Type t, JSExpr.String name)

   // TODO: This only accepts static method calls at the moment,
   // extend to instance methods and properties. Add DebugInfo?
   let compileCall quote args =
      match quote with
      | DerivedPatterns.Lambdas(_, Patterns.Call(_,mi,_)) ->
         let args = List.map compileExpr args
         Apply(refMethod mi None, args)
      | _ -> failwith "Expecting a lambda with a static method call"

   let addInterface (impl: Type) (infc: Type) =
      let addInterface (impl, infc) =
         let ctype = ctypes.[impl]
         if not(ctype.Interfaces.ContainsKey infc) then
            ctype.Interfaces.Add(infc, Unchecked.defaultof<CompiledType>)
            let map = impl.GetInterfaceMap(infc)
            let infcRef = PropertyGet(JSExpr.Type impl, 
                                      JSExpr.Type(if infc.IsGenericType
                                                  then infc.GetGenericTypeDefinition()
                                                  else infc))
            let meths = map.TargetMethods
                        |> Array.mapi (fun i meth ->
                              // TODO: Warning! An interface can also have overloaded methods (see also refMethod)
                              let methName = map.InterfaceMethods.[i].Name
                              let methRef = PropertyGet(infcRef, JSExpr.String methName)
                              meth.MetadataToken, (methName, Applications.compileMethod this meth methRef))
                        |> Array.fold (fun (dic: Dictionary<_,_>) (methId, methInfo) ->
                              dic.Add(methId, methInfo); dic) (Dictionary<_,_>())
            let cons = Assign(DebugInfo.Empty, infcRef, JSExpr.Object [])
            ctype.Interfaces.[infc] <- CompiledType(cons, meths)

      let rec getInheritedInterfaces (infc: Type) =
         match infc.GetInterfaces() |> List.ofArray with
         | [] -> [infc]
         | baseInfcs -> infc::(baseInfcs |> List.map getInheritedInterfaces |> List.concat)

      let impl = addType impl
      let infc = impl.GetInterfaces() |> Array.find (fun i -> i.MetadataToken = infc.MetadataToken)
      getInheritedInterfaces infc |> List.iter (fun infc -> addInterface(impl, infc))

   member __.Print(expr: Expr, stream: TextWriter) =
      JSMapper.resetCache()
      let boot = compileStatement (if options.noReturn then Inplace else ReturnFrom) expr
      if options.enclose then stream.WriteLine "(function(){"
      stream.WriteLine "var ns={};"
      ctypes |> Seq.iter (fun ct -> ct.Value.Print(stream))
      stream.WriteLine (boot.Print 0 Map.empty)
      if options.enclose then stream.Write "}());"

   interface ICompiler with
      member __.TypeMappings = typeMappings
      member __.CompileStatement ret expr = compileStatement ret expr
      member __.CompileExpr expr = compileExpr expr
      member __.CompileCall quote args = compileCall quote args
      member __.RefType t = refType t
      member __.RefCase uci = refCase uci
      member __.RefMethod (meth, target) = refMethod meth target
      member __.AddInterface (impl, infc) = addInterface impl infc

type Compiler =
   static member private compile(expr: Expr, writer: TextWriter, options: CompileOptions option) =
      let options = defaultArg options (CompileOptions())
      let components = [ yield! PrimitiveTypes.components
                         yield! GetSet.components
                         yield! Comparisons.components
                         yield! LetBindings.components
                         yield! ControlFlow.components
                         yield! Conversions.components
                         yield! Constructors.components
                         yield! Applications.components ]
      let typeMappings =
         let assembly = Assembly.GetAssembly(typeof<JSExpr>)
         Map [
            typeof<string>.Name, typeof<Core.String>
            typeof<System.Exception>.Name, typeof<Core.Exception>
            // Some intrinsic functions use this module where it can't be hidden
            "OptionModule", assembly.GetType("FunScript.Core.OptionModule")
            typeof<obj option>.Name, assembly.GetType("FunScript.Core.FSOption")
            typeof<obj list>.Name, assembly.GetType("FunScript.Core.FSList")
         ]
      let compiler = InternalCompiler(options, typeMappings, components)
      compiler.Print(expr, writer)

   static member CompileTo(expr: Expr, filePath: string, ?baseDir: string, ?options: CompileOptions, ?browserify: bool) =
      let baseDir = defaultArg baseDir <| Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
      let filePath = Path.Combine(baseDir, filePath)
      let folder = Path.GetDirectoryName(filePath)
      if not (Directory.Exists folder) then Directory.CreateDirectory(folder) |> ignore
      use stream = File.Open(Path.Combine(baseDir, filePath), FileMode.Create, FileAccess.ReadWrite)
      use writer = new StreamWriter(stream)
      Compiler.compile(expr, writer, options)
      writer.Flush()

   static member Compile(expr: Expr, ?options: CompileOptions) =
      use writer = new StringWriter()
      Compiler.compile(expr, writer, options)
      writer.Flush();
      writer.ToString()
