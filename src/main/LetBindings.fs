module internal FunScript.LetBindings

open AST
open InternalCompiler
open Microsoft.FSharp.Quotations

let private bindings (com: Compiler) ret = function
   | Patterns.LetRecursive(bindingExprs, CompileStatement com ret body) ->
      bindingExprs
      |> List.fold (fun (acc: JSStatement) (var, assignment) ->
         let compiledAssignment = com.CompileExpr assignment
         // TODO: Check if the source mapping works properly in this case
         Let(assignment.DebugInfo, var, compiledAssignment, acc)) body
      |> buildStatement
   | Patterns.Let(var, CompileExpr com assignment, CompileStatement com ret body) as e ->
      Let(e.DebugInfo, var, assignment, body)
      |> buildStatement
   | _ -> None

let private vars (com: Compiler) _ = function
   | Patterns.Var(var) ->
      buildExpr <| refVar var
   | Patterns.VarSet(var, CompileExpr com assignment) as e -> 
      buildStatement <| Assign(e.DebugInfo, refVar var, assignment)
   | _ -> None

let components: CompilerComponent list = [ 
   bindings
   vars
]