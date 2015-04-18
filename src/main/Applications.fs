module internal FunScript.Applications

open AST
open System.Reflection
open System.Text.RegularExpressions
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

// TODO TODO TODO
// Change TypeMappings: List
// Invoke method of delegates

// TODO: Cache?
let getGenMethodDef (meth: MethodBase) =
   let getGenericMethod' (t: System.Type) (meth: MethodBase): MethodBase =
      match meth with
      | :? ConstructorInfo as ci ->
         t.GetConstructors(BindingFlags.All)
         |> Array.pick (fun ci ->
            if ci.MetadataToken = meth.MetadataToken then Some (upcast ci) else None)
      | _ ->
         t.GetMethods(BindingFlags.All)
         |> Array.find (fun mi -> mi.MetadataToken = meth.MetadataToken)
         |> function
            | mi when mi.IsGenericMethod -> upcast(mi.GetGenericMethodDefinition())
            | mi -> upcast mi
   let t = if meth.DeclaringType.IsGenericType
           then meth.DeclaringType.GetGenericTypeDefinition()
           else meth.DeclaringType
   getGenericMethod' t meth
  
let getMethGenArgs (meth: MethodBase) =
   let genArgs = if meth.DeclaringType.IsGenericType
                 then meth.DeclaringType.GetGenericArguments()
                 else [||]
   List.ofArray <|
      if meth.IsGenericMethod
      then meth.GetGenericArguments() |> Array.append genArgs
      else genArgs

let compileArgs (com: ICompiler) = function
   | [DerivedPatterns.Unit _] -> []
   | _ as args -> List.map com.CompileExpr args

let compileMethod (com: ICompiler) (meth: MethodBase) (methRef: JSExpr) =
   let cons =
      match meth.TryGetAttribute<JSEmitAttribute>() with
      | Some att -> 
         let vars =
            meth.GetParameters()
            |> Seq.map (fun p -> Var(p.Name, typeof<obj>))
            |> Seq.toList
            |> fun vars -> if meth.IsStatic then vars else (Var("_this", typeof<obj>))::vars
         Lambda(vars, Do(DebugInfo.Empty, EmitExpr(att.Emit, List.map JSExpr.Var vars)))
      | None ->
         match Expr.TryGetReflectedDefinition meth with
         | Some e ->
            let genArgs =                                // Receive also generic arguments
               getMethGenArgs meth |> List.map (fun a -> Var("$" + a.Name, a))
            match e with 
            | DerivedPatterns.Lambdas(vars, body) ->
               vars
               |> List.concat
               |> List.filter (fun v -> v.Type <> typeof<unit>)
               |> fun vars -> Lambda(vars@genArgs, com.CompileStatement ReturnFrom body)
            | _ ->
               Lambda(genArgs, com.CompileStatement ReturnFrom e)
         | None ->
            failwithf "Cannot compile method %s of %s. %s"
                        meth.Name meth.DeclaringType.FullName
                        "Either JS attribute is missing or the method is not supported by FunScript."
   Assign(DebugInfo.Empty, methRef, cons)

let private pipes (com: ICompiler) _ expr =
   let replaceVars varExprs freeVars valExprs =
      let rec replaceVars varExprs valExprs acc =
         match varExprs with
         | [] -> 
            match valExprs with
            | [] -> List.rev acc
            | v::vs -> failwith "Remaining values after vars have been consumed"
         | var::vars ->
            let var, coerceType =
               match var with
               | Patterns.Coerce(var, t) -> var, Some t
               | _ -> var, None
            match var with
            | Var var  when not(Set.contains var freeVars) ->
               match valExprs with
               | [] -> failwith "Values consumed before it was expected"
               | v::vs -> match coerceType with
                          | Some t -> replaceVars vars vs (Expr.Coerce(v, t)::acc)
                          | None -> replaceVars vars vs (v::acc)
            | _ ->
               // If the Expr doesn't correspond to a non-free var, it means it's a value itself
               match coerceType with
               | Some t -> replaceVars vars valExprs (Expr.Coerce(var, t)::acc)
               | None -> replaceVars vars valExprs (var::acc)
      replaceVars varExprs valExprs []
   let rec call c target (mi: MethodInfo) (args: Expr list) =
      match c with
      | DerivedPatterns.SpecificCall <@ (|>) @> (None,_,[_;f]) ->
         compress args.Head f
      | DerivedPatterns.SpecificCall <@ (<|) @> (None,_,[f;_]) ->
         compress args.Head f
      | _ ->
         match target with 
         | Some target -> Expr.Call(target,mi,args)
         | None -> Expr.Call(mi,args)
   and compress x f =
      match x with
      | DerivedPatterns.SpecificCall <@ (|>) @> (None,_,[x';f']) ->
         compress (compress x' f') f
      | DerivedPatterns.SpecificCall <@ (<|) @> (None,_,[f';x']) ->
         compress (compress x' f') f
      | _ ->
         let freeVars = Set(f.GetFreeVars())
         match f with
         | Lambda(var1, (Call(target, mi, vars) as c)) ->
            call c target mi <| replaceVars vars freeVars [x]
         | Let(var1, val1, Lambda(var2, (Call(target, mi, vars) as c))) ->
            call c target mi <| replaceVars vars freeVars [val1; x]
         | Let(var1, val1, Let(var2, val2, Lambda(var3, (Call(target, mi, vars) as c)))) ->
            call c target mi <| replaceVars vars freeVars [val1; val2; x]
         | Let(var1, val1, Let(var2, val2, Let(var3, val3, Lambda(var4, (Call(target, mi, vars) as c))))) ->
            call c target mi <| replaceVars vars freeVars [val1; val2; val3; x]
         | _ -> Expr.Application(f, x)
   match expr with
   | DerivedPatterns.SpecificCall <@ (|>) @> (None,_,[x;f]) ->
         buildExpr <| com.CompileExpr(compress x f)
   | DerivedPatterns.SpecificCall <@ (<|) @> (None,_,[f;x]) ->
         buildExpr <| com.CompileExpr(compress x f)
   | _ -> None

let private fnDefinition com _ = function
   | NewDelegate(_, vars, CompileStatement com ReturnFrom body) ->
      buildExpr <| Lambda(vars, body)
   | Lambda(var, bodyExpr) ->
      buildExpr <|
         match bodyExpr with
         // See comment for `id` and `ignore` in FunScript.Core.Operators
         | Patterns.Call(None, mi, _) when mi.Name = "FSIgnore" || mi.Name = "FSIdentity" ->
            let var = Var("x", typeof<obj>) in Lambda([var], Return(DebugInfo.Empty, JSExpr.Var var))
         | _ ->
            let vars = if var.Type = typeof<unit> then [] else [var]
            Lambda(vars, com.CompileStatement ReturnFrom bodyExpr)
   | _ -> None

let rec private tryPicki i f xs =
   if i = (Array.length xs) then None
   else match f i xs.[i] with Some _ as u -> u | None -> tryPicki (i+1) f xs 

let private application (com: ICompiler) _ = function
   | DerivedPatterns.Applications(DerivedPatterns.Lambdas(vars, body), args) as expr ->
      let vars, args =
         List.toArray(List.concat vars), List.toArray(List.concat args)
      let body = body.Substitute(fun var ->
         tryPicki 0 (fun i (v: Var) ->
            if v = var then Some args.[i] else None) vars).With(expr.DebugInfo)
      buildExpr <| com.CompileExpr body
   | Application (CompileExpr com body, arg) ->
      buildExpr <| Apply(body, compileArgs com [arg])
   | Call(Some(CompileExpr com func), mi, args) when mi.Name = "Invoke" && typeof<System.Delegate>.IsAssignableFrom mi.DeclaringType ->
      buildExpr <| Apply(func, compileArgs com args)
// TODO: Is this necessary?
//   | Patterns.Call(_, mi, args) when typeof<System.Delegate>.IsAssignableFrom mi.DeclaringType ->
   | _ -> None

let private replaceMethod (m1: MethodBase) (t: System.Type): MethodBase =
   let m1Params = m1.GetParameters()
   t.GetMethods(BindingFlags.All)
   |> Array.tryFind (fun m2 -> 
      let m2Params = m2.GetParameters()
      m1.Name = m2.Name && m1Params.Length = m2Params.Length &&
         m1Params |> Array.zip m1Params
                  |> Array.fold (fun (b: bool) (p1, p2) ->
                     b && p1.ParameterType.Name = p2.ParameterType.Name) true)
   |> function
      | Some replacement -> upcast replacement
      | None -> failwithf "Couldn't find replacement for method %s of %s" m1.Name m1.DeclaringType.FullName

let private methodCalling (com: ICompiler) _ = function
   | Call(target, mi, args) as expr ->
      let genMi =
         let genMi = getGenMethodDef mi
         if com.TypeMappings.ContainsKey genMi.DeclaringType.Name              // Check if there are replacements
         then replaceMethod genMi com.TypeMappings.[genMi.DeclaringType.Name]
         else genMi
      let target = Option.map com.CompileExpr target
      // TODO: Even if the method accepts flexible types, force coertion to be sure the interface is compiled
      let args =                                      
         let args = compileArgs com args
         match target with None -> args | Some target -> target::args
      match genMi.TryGetAttribute<JSEmitInlineAttribute>() with
      | Some att ->
         let pars = genMi.GetParameters()
         if pars.Length > 0 && pars.[pars.Length-1].TryGetAttribute<System.ParamArrayAttribute>().IsSome then
            match List.rev args with
            | (JSExpr.Array paramArgs)::args ->
               buildExpr <| EmitExpr(att.Emit, (List.rev args.Tail)@paramArgs)
            | _ -> failwithf "Expected an array as last argument: %A" expr
         elif Regex(@"\bset_").IsMatch(mi.Name) then // TODO: Is there a better way to check whether it's a Set method?
            let lastArg = sprintf "{%i}" (args.Length-1)
            // If the attribute doesn't contain a placeholder for the last arg, assume the same
            // emitted expression is being used for both methods (get and set) of the property
            let emit = if att.Emit.Contains lastArg then att.Emit else att.Emit + "=" + lastArg
            buildStatement <| Do(DebugInfo.Empty, EmitExpr(emit, args))
         else
            buildExpr <| EmitExpr(att.Emit, args)
      | None ->
         let genArgs = getMethGenArgs mi |> List.map com.RefType
         buildExpr <| Apply(com.RefMethod(genMi, target), args@genArgs)    // Pass also generic arguments
   | _ -> None

let components: CompilerComponent list = [ 
   pipes
   fnDefinition
   application
   methodCalling
]