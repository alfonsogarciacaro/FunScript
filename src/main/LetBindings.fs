module internal FunScript.LetBindings

open AST
open Microsoft.FSharp.Compiler.SourceCodeServices.BasicPatterns

let private bindings (com: ICompiler) ret = function
   | LetRec(bindingExprs, CompileStatement com ret body) ->
      bindingExprs
      |> List.rev
      |> List.fold (fun (acc: JSStatement) (var, assignment) ->
         // TODO: Check if the source mapping works properly in this case
         Let(assignment.DebugInfo, var, com.CompileExpr assignment, acc)) body
      |> buildStatement
   | Let((var, CompileExpr com assignment), CompileStatement com ret body) as e ->
      Let(e.DebugInfo, var, assignment, body)
      |> buildStatement
   | _ -> None

let private vars (com: ICompiler) _ = function
   | Value(var) ->
      buildExpr <| JSExpr.Var var
   | ValueSet(var, CompileExpr com assignment) as e -> 
      buildStatement <| Assign(e.DebugInfo, JSExpr.Var var, assignment)
   | _ -> None

let components: CompilerComponent list = [ 
   bindings
   vars
]