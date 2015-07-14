module FunScript.Applications

open AST
open Microsoft.FSharp
open Microsoft.FSharp.Compiler.SourceCodeServices
open Microsoft.FSharp.Compiler.SourceCodeServices.BasicPatterns

// TODO TODO TODO
// Invoke method of delegates

let private (|Lambdas|_|) = function
  | Lambda(v1, Lambda(v2, Lambda(v3, Lambda(v4, Lambda(v5, Lambda(v6, e)))))) ->
      Some (v1::v2::v3::v4::v5::v6::[], e)
  | Lambda(v1, Lambda(v2, Lambda(v3, Lambda(v4, Lambda(v5, e))))) ->
      Some (v1::v2::v3::v4::v5::[], e)
  | Lambda(v1, Lambda(v2, Lambda(v3, Lambda(v4, e)))) ->
      Some (v1::v2::v3::v4::[], e)
  | Lambda(v1, Lambda(v2, Lambda(v3, e))) ->
      Some (v1::v2::v3::[], e)
  | Lambda(v1, Lambda(v2, e)) ->
      Some (v1::v2::[], e)
  | Lambda(v1, e) ->
      Some (v1::[], e)
  | _ -> None

let compileArgs (com: ICompiler) = function
  | [Const(:? unit, _)] -> []
  | _ as args -> List.map com.CompileExpr args

(*
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
*)

let private pipes (com: ICompiler) (ret: ReturnStrategy) (expr: FSharpExpr) =
  let replaceVars varExprs (lambdaVars: FSRef list) valExprs =
    let isNonFreeVar var =
      List.exists (fun (v: FSRef) -> v.IsEffectivelySameAs var) lambdaVars
    let rec replaceVars varExprs valExprs acc =
      match varExprs with
      | [] -> 
        match valExprs with
        | [] -> List.rev acc
        | v::vs -> failwith "Remaining values after vars have been consumed"
      | var::vars ->
        match var with
        | Value var
        | Coerce(_, Value var) when isNonFreeVar var ->
          match valExprs with
          | [] -> failwith "Values consumed before it was expected"
          | value::values -> replaceVars vars values (value::acc)
        | _ ->
          // If the Expr doesn't correspond to a nonfree var, it means it's a value itself
          replaceVars vars valExprs (var::acc)
    replaceVars varExprs valExprs []
  let apply target mi args =
    Apply(com.RefMethod(mi, Option.map com.CompileExpr target), compileArgs com args)
  match expr with
  | SpecificCall <@ (|>) @> (None,_,_,[x;f])
  | SpecificCall <@ (<|) @> (None,_,_,[f;x]) ->
    buildExpr <|
      match f with
      | Value _ ->
        failwith "Not implemented"
      | Lambda(var1, Call(target,mi,_,_,vars)) ->
        apply target mi <| replaceVars vars [var1] [x]
      | Let((var1, val1), Lambda(var2, Call(target,mi,_,_,vars))) ->
        apply target mi <| replaceVars vars [var1;var2] [val1; x]
      | Let((var1, val1), Let((var2, val2), Lambda(var3, Call(target,mi,_,_,vars)))) ->
        apply target mi <| replaceVars vars [var1;var2;var3] [val1; val2; x]
      | Let((var1, val1), Let((var2, val2), Let((var3, val3), Lambda(var4, Call(target,mi,_,_,vars))))) ->
        apply target mi <| replaceVars vars [var1;var2;var3;var4] [val1; val2; val3; x]
      | _ ->
        Apply(com.CompileExpr f, compileArgs com [x])
  | _ -> None

let private fnDefinition com _ = function
  | NewDelegate(typ, Lambdas(vars, CompileStatement com Return body)) ->
      buildExpr <| Lambda(vars, body)
  | Lambda(var, CompileStatement com Return body) ->
      buildExpr <| Lambda([var], body)
  | _ -> None

let private application com _ = function
  | Application(f, _, args) ->
    let f = match f with
            | Lambdas(vars, CompileStatement com Return body) -> Lambda(vars, body)
            | CompileExpr com f -> f
    buildExpr <| Apply(f, compileArgs com args)
  | Call(Some(CompileExpr com func), mi, _, _, args) when mi.EnclosingEntity.IsDelegate ->
    buildExpr <| Apply(func, compileArgs com args)
  | _ -> None

let private methodCalling (com: ICompiler) _ = function
  | Call(target, mi, _, _, args) as expr ->
    let args = compileArgs com args
    match mi.TryGetAttribute<JSEmitInlineAttribute>() with
    | Some [:? string as emit] -> 
      let pars = mi.CurriedParameterGroups |> Seq.concat |> Seq.toArray // TODO TODO TODO
      if pars.Length > 0 && pars.[pars.Length-1].TryGetAttribute<System.ParamArrayAttribute>().IsSome then
        match List.rev args with
        | (JSExpr.Array paramArgs)::args ->
            buildExpr <| EmitExpr(emit, (List.rev args.Tail)@paramArgs)
        | _ -> failwithf "Expected an array as last argument: %A" expr
      elif mi.IsPropertySetterMethod then
        let lastArg = sprintf "{%i}" (args.Length-1)
        // If the attribute doesn't contain a placeholder for the last arg, assume the same
        // emitted expression is being used for both methods (get and set) of the property
        let emit = if emit.Contains lastArg then emit else emit + "=" + lastArg
        buildStatement <| Do(Range.Zero, EmitExpr(emit, args))
      else
        buildExpr <| EmitExpr(emit, args)
    | _ ->
      let target = Option.map com.CompileExpr target
      buildExpr <| Apply(com.RefMethod(mi, target), args)
  | _ -> None

let components: CompilerComponent list = [ 
   pipes
   fnDefinition
   application
   methodCalling
]