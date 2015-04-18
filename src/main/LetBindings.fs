module internal FunScript.LetBindings

open AST
open Microsoft.FSharp.Quotations

let private bindings (com: ICompiler) ret = function
   | Patterns.LetRecursive(bindingExprs, CompileStatement com ret body) ->
      bindingExprs
      |> List.rev
      |> List.fold (fun (acc: JSStatement) (var, assignment) ->
         // TODO: Check if the source mapping works properly in this case
         Let(assignment.DebugInfo, var, com.CompileExpr assignment, acc)) body
      |> buildStatement
   | Patterns.Let(var, CompileExpr com assignment, CompileStatement com ret body) as e ->
      Let(e.DebugInfo, var, assignment, body)
      |> buildStatement
   | _ -> None

let private vars (com: ICompiler) _ = function
   | Patterns.Var(var) ->
      buildExpr <| JSExpr.Var var
   | Patterns.VarSet(var, CompileExpr com assignment) as e -> 
      buildStatement <| Assign(e.DebugInfo, JSExpr.Var var, assignment)
   | _ -> None

let components: CompilerComponent list = [ 
   bindings
   vars
]