module internal FunScript.ControlFlow

open AST
open Microsoft.FSharp.Compiler.SourceCodeServices.BasicPatterns

let private decisionTree com _ = function
  | DecisionTree(e, branches) ->
    let rec reduce e branches =
        match branches with
        | [] -> failwith "Caught empty branches when pattern-matching a DecisionTree"
        | [(_, branch); (_, last)] -> failwith "never"
        | (_, branch)::branches ->
          match e with
          | IfThenElse(cond, DecisionTreeSuccess(i1, args), e) ->
              match args with
              | [] -> AdHoc(AdHocIf(imp cond, imp branch, reduce e branches))
              | _ -> failwith "Caught DecisionTreeSuccess with non-empty args"
          | _ -> failwith "Caught DecisionTree with condition different than IfThenElse"
    match reduce e branches with
    | AdHoc(AdHocIf(cond, trueBranch, falseBranch)) -> Some(cond, trueBranch, falseBranch)
    | _ -> failwith "Unexpected"
  | _ -> None


let private controlFlow com ret = function
  | Let((_, Call(Some (CompileExpr com iterable), getEnum,_,_,_)),
        TryFinally(WhileLoop(_, Let((var,_), CompileStatement com Inplace body)),_)) as e
    when getEnum.CompiledName = "GetEnumerator" ->
    buildStatement <| ForOfLoop(e.Range, var, iterable, body)

  | FastIntegerForLoop(CompileExpr com beginning,
                       CompileExpr com ending,
                       Lambda(var, CompileStatement com Inplace body), isUp) as e ->
    buildStatement <| ForIntegerLoop(e.Range, var, beginning, ending, body, isUp)

  | WhileLoop(CompileExpr com cond, CompileStatement com ret body) as e ->
    buildStatement <| WhileLoop(e.Range, cond, body)

  | IfThenElse(CompileExpr com cond, CompileStatement com ret trueBody, CompileStatement com ret falseBody) as e ->
    buildStatement <| IfThenElse(e.Range, cond, trueBody, falseBody)

  | TryFinally
    (TryWith(CompileStatement com ret body, _, _, catchVar, CompileStatement com ret catchBody),
      CompileStatement com Inplace finalBody) ->
    buildStatement <| TryCatchFinally(body, catchVar, catchBody, finalBody)

  | TryFinally(CompileStatement com ret body, CompileStatement com Inplace finalBody) ->
    buildStatement <| TryFinally(body, finalBody)

  // TODO: Type testing... (?)
  | TryWith(CompileStatement com ret body, _, _, catchVar, CompileStatement com ret catchBody) ->
    buildStatement <| TryCatch(body, catchVar, catchBody)

  | Sequential(CompileStatement com Inplace firstBlock, CompileStatement com ret secondBlock) ->
    buildStatement <| Sequential(firstBlock, secondBlock)

  | _ -> None

let components: CompilerComponent list = [ 
   controlFlow
]

