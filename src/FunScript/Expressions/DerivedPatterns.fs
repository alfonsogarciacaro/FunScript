module FunScript.DerivedPatterns


open Microsoft.FSharp.Compiler.SourceCodeServices
open Microsoft.FSharp.Compiler.SourceCodeServices.BasicPatterns
type JSE = AST.JSExpr
type JSI = AST.JSInstruction

let matchArgs (lambdaArgs: FSharpMemberOrFunctionOrValue list) (callArgs: FSharpExpr list) =
  if lambdaArgs.Length = callArgs.Length then
    lambdaArgs
    |> List.fold2 (fun (matchingSoFar: bool) (a: FSharpExpr) v ->
      if matchingSoFar
      then match a with Value v' -> v = v' | _ -> false
      else false) true callArgs
  else 
    false

let (|Lambdas|_|) e =
  match e with
  | Lambda _ ->
    match e with
    | Lambda(v1, Lambda(v2, Lambda(v3, Lambda(v4, Lambda(v5, Lambda(v6, e)))))) ->
      Some ([v1;v2;v3;v4;v5;v6], e)
    | Lambda(v1, Lambda(v2, Lambda(v3, Lambda(v4, Lambda(v5, e))))) ->
      Some ([v1;v2;v3;v4;v5], e)
    | Lambda(v1, Lambda(v2, Lambda(v3, Lambda(v4, e)))) ->
      Some ([v1;v2;v3;v4], e)
    | Lambda(v1, Lambda(v2, Lambda(v3, e))) ->
      Some ([v1;v2;v3], e)
    | Lambda(v1, Lambda(v2, e)) ->
      Some ([v1;v2], e)
    | Lambda(v1, e) ->
      Some ([v1], e)
    | _ -> None
  | _ -> None

let (|Lets|_|) e =
  match e with
  | Let _ ->
    match e with
    | Let(b1, Let(b2, Let(b3, Let(b4, Let(b5, Let(b6, e)))))) ->
      Some([b1;b2;b3;b4;b5;b6], e)
    | Let(b1, Let(b2, Let(b3, Let(b4, Let(b5, e))))) ->
      Some([b1;b2;b3;b4;b5], e)
    | Let(b1, Let(b2, Let(b3, Let(b4, e)))) ->
      Some([b1;b2;b3;b4], e)
    | Let(b1, Let(b2, Let(b3, e))) ->
      Some([b1;b2;b3], e)
    | Let(b1, Let(b2, e)) ->
      Some([b1;b2], e)
    | Let(b1, e) ->
      Some([b1], e)
    | _ -> None
  | _ -> None


  // TODO: Extend this to instance methods?
let (|WrappedMethod|_|) = function
  | Lambdas(lambdaArgs, Call(None, meth, _, _, callArgs)) when matchArgs lambdaArgs callArgs -> Some meth
  | _ -> None

let (|PartialApply|_|) (com: AST.ICompiler) (scope: AST.IScopeInfo) e =
  let com v meth args =
    Some <| JSE.Lambda(isGenerator=false, args=[v], body=JSI.Expr (com.CompileCall scope None meth args))
  match e with
  | Lets(bindings, Lambda(v, Call(None, meth, _, _, callArgs))) ->
    let lambdaArgs = bindings |> List.fold (fun acc (bindVar, _) -> bindVar::acc) [v]
    if matchArgs lambdaArgs callArgs
    then com v meth (bindings |> List.fold (fun acc (_, bindVal) -> bindVal::acc) [List.last callArgs])
    else None
  | Application(Lambdas(lambdaArgs, Call(None, meth, _, _, callArgs)), _, applyArgs) when matchArgs lambdaArgs callArgs ->
    com (List.last lambdaArgs) meth (applyArgs |> List.fold (fun acc arg -> arg::acc) [List.last callArgs])
  | _ -> None

let (|Unit|_|) = function
  | Const(:? unit,_) -> Some ()
  | _ -> None
