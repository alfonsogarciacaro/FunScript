module internal FunScript.Comparisons

open AST
open System
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.DerivedPatterns


[<JSEmit("function structEq(a,b) {
  if (typeof b !== 'object') {
    return false;
  }
  for(var k in a) {
     var eq = typeof a[k] === 'object'
        ? structEq(a[k], b[k])
        : a[k] === b[k];
     if (!eq) {
        return false;
     }
     else {
       for(var k in b)
         if (a[k] === undefined)
           return fals;
     }
     return true;
  }
  return true;
}
return structEq({0},{1})")>]
let structuralEquality lhs rhs = failwith "never"

// TODO TODO TODO: Structural comparison for non primitives and IComparable checking
let private comparison (com: ICompiler) _ e =
   let comp = com.CompileExpr
   let binOp (op: string) lhs rhs = BinaryOp(comp lhs, op, comp rhs) |> buildExpr
   let emit (pattern: string) lhs rhs = EmitExpr(pattern, [comp lhs; comp rhs]) |> buildExpr
   let checkComp (lhsType: Type) (rhsType: Type) lhs rhs =
      if lhsType.IsPrimitive then
         binOp "===" lhs rhs
      else
         // TODO: Structural comparison
         failwith "Not implemented"

   match e with
   | SpecificCall <@ obj.ReferenceEquals @> (_,_,[lhs;rhs]) -> binOp "===" lhs rhs
   | SpecificCall <@ compare @> (_,_,[lhs;rhs]) -> emit "({0}<{1}?-1:({0}==={1}?0:1))" lhs rhs
   | SpecificCall <@ min @>  (_,_,[lhs;rhs]) -> emit "({0}>{1}?{1}:{0})" lhs rhs
   | SpecificCall <@ max @>  (_,_,[lhs;rhs]) -> emit "({0}<{1}?{1}:{0})" lhs rhs
   | SpecificCall <@ (=) @>  (None,[lhsType;rhsType],[lhs;rhs]) -> binOp "===" lhs rhs
   | SpecificCall <@ (<>) @> (_,_,[lhs;rhs]) -> binOp "!==" lhs rhs
   | SpecificCall <@ (<) @>  (_,_,[lhs;rhs]) -> binOp "<" lhs rhs
   | SpecificCall <@ (<=) @> (_,_,[lhs;rhs]) -> binOp "<=" lhs rhs
   | SpecificCall <@ (>) @>  (_,_,[lhs;rhs]) -> binOp ">" lhs rhs
   | SpecificCall <@ (>=) @> (_,_,[lhs;rhs]) -> binOp ">=" lhs rhs
   | _ -> None

let components: CompilerComponent list = [
   comparison
]