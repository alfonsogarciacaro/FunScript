module internal FunScript.ControlFlow

open AST
open InternalCompiler
open Microsoft.FSharp.Quotations

let private controlFlow com ret = function
   // TODO: Problems with closures with JS for loops? See old FunScript version 
   | Patterns.ForIntegerRangeLoop(var, CompileExpr com beginning, CompileExpr com ending, CompileStatement com ret body) as e ->
      buildStatement <| ForLoop(e.DebugInfo, var, beginning, ending, body)

   | Patterns.WhileLoop(CompileExpr com cond, CompileStatement com ret body) as e ->
      buildStatement <| WhileLoop(e.DebugInfo, cond, body)

   // TODO: Check cases where ?: can be used
   | Patterns.IfThenElse(CompileExpr com cond, CompileStatement com ret trueBody, CompileStatement com ret falseBody) as e ->
      buildStatement <| IfThenElse(e.DebugInfo, cond, trueBody, falseBody)

   | Patterns.TryFinally
      (Patterns.TryWith(CompileStatement com ret body, _, _, catchVar, CompileStatement com ret catchBody),
       CompileStatement com Inplace finalBody) ->
      buildStatement <| TryCatchFinally(body, catchVar, catchBody, finalBody)

   | Patterns.TryFinally(CompileStatement com ret body, CompileStatement com Inplace finalBody) ->
      buildStatement <| TryFinally(body, finalBody)

   // TODO: Type testing... (?)
   | Patterns.TryWith(CompileStatement com ret body, _, _, catchVar, CompileStatement com ret catchBody) ->
      buildStatement <| TryCatch(body, catchVar, catchBody)

   | Patterns.Sequential(CompileStatement com Inplace block, Patterns.Value(_,t)) when t = typeof<unit> ->
      buildStatement block

   | Patterns.Sequential(CompileStatement com Inplace firstBlock, CompileStatement com ret secondBlock) ->
      buildStatement <| Sequential(firstBlock, secondBlock)

   | _ -> None

let components: CompilerComponent list = [ 
   controlFlow
]