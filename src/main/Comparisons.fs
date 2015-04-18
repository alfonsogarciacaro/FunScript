module internal FunScript.Comparisons

open AST
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.DerivedPatterns

// TODO TODO TODO: Structural comparison for non primitives and IComparable checking
let private comparison (com: ICompiler) _ e =
   let comp = com.CompileExpr
   let binOp (op: string) lhs rhs = BinaryOp(comp lhs, op, comp rhs) |> buildExpr
   let emit (pattern: string) lhs rhs = EmitExpr(pattern, [comp lhs; comp rhs]) |> buildExpr
   match e with
   | SpecificCall <@ obj.ReferenceEquals @> (_,_,[lhs;rhs]) -> binOp "===" lhs rhs
   | SpecificCall <@ compare @> (_,_,[lhs;rhs]) -> emit "({0}<{1}?-1:({0}==={1}?0:1))" lhs rhs
   | SpecificCall <@ min @>  (_,_,[lhs;rhs]) -> emit "({0}>{1}?{1}:{0})" lhs rhs
   | SpecificCall <@ max @>  (_,_,[lhs;rhs]) -> emit "({0}<{1}?{1}:{0})" lhs rhs
   | SpecificCall <@ (=) @>  (_,_,[lhs;rhs]) -> binOp "===" lhs rhs
   | SpecificCall <@ (<>) @> (_,_,[lhs;rhs]) -> binOp "!==" lhs rhs
   | SpecificCall <@ (<) @>  (_,_,[lhs;rhs]) -> binOp "<" lhs rhs
   | SpecificCall <@ (<=) @> (_,_,[lhs;rhs]) -> binOp "<=" lhs rhs
   | SpecificCall <@ (>) @>  (_,_,[lhs;rhs]) -> binOp ">" lhs rhs
   | SpecificCall <@ (>=) @> (_,_,[lhs;rhs]) -> binOp ">=" lhs rhs
   | _ -> None

let components: CompilerComponent list = [
   comparison
]