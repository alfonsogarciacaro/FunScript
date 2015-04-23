module internal FunScript.ControlFlow

open AST
open Microsoft.FSharp.Compiler.SourceCodeServices.BasicPatterns

let private controlFlow com ret = function
   // TODO: Problems with closures with JS for loops? See old FunScript version 
   | FastIntegerForLoop(CompileExpr com beginning,
                        CompileExpr com ending,
                        Lambda(var, CompileStatement com Inplace body), isUp) as e ->
      buildStatement <| ForLoop(e.DebugInfo, var, beginning, ending, body, isUp)

   | WhileLoop(CompileExpr com cond, CompileStatement com ret body) as e ->
      buildStatement <| WhileLoop(e.DebugInfo, cond, body)

   // TODO: Check cases where ?: can be used
   | IfThenElse(CompileExpr com cond, CompileStatement com ret trueBody, CompileStatement com ret falseBody) as e ->
      buildStatement <| IfThenElse(e.DebugInfo, cond, trueBody, falseBody)

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