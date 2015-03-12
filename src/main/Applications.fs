module internal FunScript.Applications

open AST
open InternalCompiler
open System.Reflection
open Microsoft.FSharp.Quotations

// TODO TODO TODO
//let (|>) x f = f x
// Invoke method of delegates

let typeMappings =
   let assembly = Assembly.GetAssembly(typeof<Compiler>)
   Map [
      typeof<string>.Name, typeof<Core.String>
      "OptionModule", assembly.GetType("FunScript.Core.OptionModule")
      typeof<obj option>.Name, assembly.GetType("FunScript.Core.FSOption")
      typeof<obj list>.Name, assembly.GetType("FunScript.Core.Collections.ListModule")
   ]

let getGenTypeDef (t: System.Type) =
   if t.IsGenericType then t.GetGenericTypeDefinition() else t

let getGenMethodDef (meth: MethodBase) =
   let getGenericMethod' (t: System.Type) (meth: MethodBase): MethodBase =
      match meth with
      | :? ConstructorInfo as ci ->
         t.GetConstructors(BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance)
         |> Array.pick (fun ci ->
            if ci.MetadataToken = meth.MetadataToken then Some (upcast ci) else None)
      | _ ->
         t.GetMethods(BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Static ||| BindingFlags.Instance)
         |> Array.find (fun mi -> mi.MetadataToken = meth.MetadataToken)
         |> function
            | mi when mi.IsGenericMethod -> upcast(mi.GetGenericMethodDefinition())
            | mi -> upcast mi
   getGenericMethod' (getGenTypeDef meth.DeclaringType) meth
  
let getMethGenArgs (meth: MethodBase) =
   let typeGenArgs =
      if meth.DeclaringType.IsGenericType
      then meth.DeclaringType.GetGenericArguments()
      else [||]
   List.ofArray <|
      if meth.IsGenericMethod
      then meth.GetGenericArguments() |> Array.append typeGenArgs
      else typeGenArgs

let getMethGenArgDefinitions (meth: MethodBase) =
   let typeGenArgs =
      if meth.DeclaringType.IsGenericType
      then meth.DeclaringType.GetGenericTypeDefinition().GetGenericArguments()
      else [||]
   List.ofArray <|
      match meth with
      | :? MethodInfo as mi when mi.IsGenericMethod ->
         mi.GetGenericMethodDefinition().GetGenericArguments() |> Array.append typeGenArgs
      | _ -> typeGenArgs

let compileMethod (com: Compiler, meth: MethodBase, infcMeth: MethodInfo option) =
   let jse =
      match meth.TryGetAttribute<JSEmitAttribute>() with
      | Some att -> 
         let vars = meth.GetParameters() |> Seq.map (fun p -> Var(p.Name, typeof<obj>)) |> Seq.toList
         Lambda(vars, Do(DebugInfo.Empty, EmitExpr(att.Emit, List.map refVar vars)))
      | None ->
         match Expr.TryGetReflectedDefinition meth with
         | Some e ->
            match e with 
            | DerivedPatterns.Lambdas(vars, body) ->
               let vars =
                  let vars = vars |> List.concat |> List.filter (fun v -> v.Type <> typeof<unit>)
                  getMethGenArgDefinitions meth                         // Receive also generic arguments
                  |> List.map (fun a -> Var("$" + a.Name, typeof<obj>))
                  |> List.append <| (List.rev vars)                     // DerivedPatterns.Lambdas reverses the var list
               Lambda(vars, com.CompileStatement ReturnFrom body)
            | Patterns.PropertyGet(None, pi, args) as expr ->
               Lambda([], com.CompileStatement ReturnFrom <| Expr.Call(pi.GetMethod, args).With(expr.DebugInfo))
            | _ ->
               failwithf "Please report: DerivedPatterns.Lambdas doesn't match method %s of %s"
                         meth.Name meth.DeclaringType.Name
         | None ->
            failwithf "Cannot compile method %s of %s. %s"
                      meth.Name meth.DeclaringType.FullName
                      "Either JS attribute is missing or the method is not supported by FunScript."
   let methRef = match infcMeth with None -> meth | Some meth -> upcast meth
   AssignGlobal(refMethodDef(methRef, meth.DeclaringType), jse)

let private fnDefinition com _ = function
   | Patterns.Lambda(var, CompileStatement com ReturnFrom body) ->
      let vars = if var.Type = typeof<unit> then [] else [var]
      buildExpr <| Lambda(vars, body)
   | Patterns.NewDelegate(_, vars, CompileStatement com ReturnFrom body) ->
      buildExpr <| Lambda(vars, body)
   | _ -> None

let rec private tryPicki i f xs =
   if i = (Array.length xs) then None
   else match f i xs.[i] with Some _ as u -> u | None -> tryPicki (i+1) f xs 

let private application (com: Compiler) _ = function
   | DerivedPatterns.Applications(DerivedPatterns.Lambdas(vars, body), args) as expr ->
      let vars, args =
         List.toArray(List.concat vars), List.toArray(List.concat args)
      let body = body.Substitute(fun var ->
         tryPicki 0 (fun i (v: Var) ->
            if v = var then Some args.[i] else None) vars).With(expr.DebugInfo)
      buildExpr <| com.CompileExpr body
   | Patterns.Application (CompileExpr com body, arg) ->
      let args = if arg.Type = typeof<unit> then [] else [com.CompileExpr arg]
      buildExpr <| Apply(body, args)
// TODO: Is this necessary?
//   | Patterns.Call(_, mi, args) when typeof<System.Delegate>.IsAssignableFrom mi.DeclaringType ->
   | _ -> None

let private replaceMethod (m1: MethodBase) (t: System.Type): MethodBase =
   let m1Params = m1.GetParameters()
   t.GetMethods(BindingFlags.Static ||| BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.NonPublic)
   |> Array.tryFind (fun m2 -> 
      let m2Params = m2.GetParameters()
      m1.Name = m2.Name && m1Params.Length = m2Params.Length &&
         m1Params |> Array.zip m1Params |> Array.fold (fun (b: bool) (p1, p2) ->
                                                b && p1.Name = p2.Name) true)
   |> function
      | Some replacement -> upcast replacement
      | None -> failwithf "Couldn't find replacement for method %s of %s" m1.Name m1.DeclaringType.FullName

let private methodCalling (com: Compiler) _ = function
   | Patterns.Call(target, mi, args) ->
      let genMi =
         let genMi = getGenMethodDef mi
         if typeMappings.ContainsKey genMi.DeclaringType.Name              // Check if there are replacements
         then replaceMethod genMi typeMappings.[genMi.DeclaringType.Name]
         else genMi
      let target = Option.map com.CompileExpr target
      let args =
         let args = List.map com.CompileExpr args
         match target with None -> args | Some target -> target::args
      match genMi.TryGetAttribute<JSEmitInlineAttribute>() with
         | Some att ->
            let pars = genMi.GetParameters()
            if pars.Length < 2 then
               EmitExpr(att.Emit, args)
            else
               let args =
                  let args = List.rev args
                  match pars.[pars.Length - 1].TryGetAttribute<System.ParamArrayAttribute>(), args.Head with
                  | Some _, JSExpr.Array paramArgs -> (List.rev args.Tail)@paramArgs
                  | _ -> List.rev args
               EmitExpr(att.Emit, args)
         | None ->
            let genArgs = getMethGenArgs mi |> List.map refType
            Apply(refMethodCall(genMi, target), args@genArgs)           // Pass also generic arguments
      |> buildExpr
   | _ -> None

let components: CompilerComponent list = [ 
   fnDefinition
   application
   methodCalling
]