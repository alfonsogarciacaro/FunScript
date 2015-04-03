module internal FunScript.PrimitiveTypes

open AST
open InternalCompiler
open System
open Microsoft.FSharp.Quotations.Patterns
open Microsoft.FSharp.Quotations.DerivedPatterns

// TODO TODO TODO: Default values (see Options.fs in old FunScript version)

//[<JSEmitInline("/^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i.test({0})")>]
//let isGuid x = failwith "JavaScript only"

//[<JSEmit("""return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
//var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8); return v.toString(16)})""")>]
//let newGuid() : System.Guid = failwith "JavaScript only"

//// NOTE: A call replacer with System.Diagnostics.Debug will fail if FunScript is compiled in Release mode, so use this function instead
//let private debugComponents = createComponent <| fun (|Split|) compiler returnStrategy -> function
//      | Patterns.Call(None,mi,args) when mi.DeclaringType.Name = "Debug" ->
//        let compile = fun quote -> let mi, _ = Quote.toMethodInfoFromLambdas quote
//                                   compiler.Compile returnStrategy (ExpressionReplacer.buildCall mi args)
//        match mi.Name with
//        | "WriteLine" -> compile <@ Extensions.logFormat @>
//        | "WriteLineIf" -> compile <@ Extensions.logIf @>
//        | _ -> []
//      | _ -> []

let private primitiveValues _ _ = function
   | Unit x ->   buildExpr <| JSExpr.Undefined        
   | Bool x ->   buildExpr <| JSExpr.Boolean x        
   | Char x ->   buildExpr <| JSExpr.String (string x)
   | String x -> buildExpr <| JSExpr.String x         
   | SByte x ->  buildExpr <| JSExpr.Integer(int x)   
   | Byte x ->   buildExpr <| JSExpr.Integer(int x)   
   | Int16 x ->  buildExpr <| JSExpr.Integer(int x)   
   | Int32 x ->  buildExpr <| JSExpr.Integer(x)       
   | Int64 x ->  buildExpr <| JSExpr.Number(float x)  
   | UInt16 x -> buildExpr <| JSExpr.Number(float x)  
   | UInt32 x -> buildExpr <| JSExpr.Number(float x)  
   | UInt64 x -> buildExpr <| JSExpr.Number(float x)  
   | Single x -> buildExpr <| JSExpr.Number(float x)  
   | Double x -> buildExpr <| JSExpr.Number(x)        
   // TODO: our own decimal type?
   | Value(null, _) -> buildExpr JSExpr.Null
   | Value(x, t) when t.IsEnum -> buildExpr <|JSExpr.Integer(unbox x)
   | _ -> None

let private isPrimitive (t: Type) =
   t.IsPrimitive || t.IsEnum || t = typeof<string>

let private unaryOp (com: Compiler) op hsT hs =
// TODO TODO TODO: If not primitive, check if there's an implementation or default (generics?)
//   if isPrimitive hsT
//   then buildExpr <| UnaryOp(op, com.CompileExpr hs)
//   else None
   buildExpr <| UnaryOp(op, com.CompileExpr hs)

let private binaryOp (com: Compiler) op lhsT rhsT lhs rhs =
// TODO TODO TODO: If not primitive, check if there's an implementation or default (generics?)
//   if isPrimitive lhsT && isPrimitive rhsT
//   then buildExpr <| BinaryOp(com.CompileExpr lhs, op, com.CompileExpr rhs)
//   else None
   buildExpr <| BinaryOp(com.CompileExpr lhs, op, com.CompileExpr rhs)

let private primitiveOps com _ = function
   // Arithmetic operators
   | SpecificCall <@ (~-) @> (_,[hsT;_],[hs]) -> unaryOp com "-" hsT hs
   | SpecificCall <@ (+) @>   (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com "+" lhsT rhsT lhs rhs
   | SpecificCall <@ (-) @>   (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com "-" lhsT rhsT lhs rhs
   | SpecificCall <@ (*) @>   (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com "*" lhsT rhsT lhs rhs
   | SpecificCall <@ (/) @>   (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com "/" lhsT rhsT lhs rhs
   | SpecificCall <@ (%) @>   (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com "%" lhsT rhsT lhs rhs
   | SpecificCall <@ (&&&) @> (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com "&" lhsT rhsT lhs rhs
   | SpecificCall <@ (|||) @> (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com "|" lhsT rhsT lhs rhs
   | SpecificCall <@ (>>>) @> (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com ">>" lhsT rhsT lhs rhs
   | SpecificCall <@ (<<<) @> (_,[lhsT;rhsT;_],[lhs;rhs]) -> binaryOp com "<<" lhsT rhsT lhs rhs
   
   // Logic operators
   | SpecificCall <@ not @> (_,_,[hs]) -> unaryOp com "!" typeof<bool> hs
   | OrElse(lhs, rhs) ->  binaryOp com "||" typeof<bool> typeof<bool> lhs rhs
   | AndAlso(lhs, rhs) -> binaryOp com "&&" typeof<bool> typeof<bool> lhs rhs

   | _ -> None

let private arrayCreation (com: Compiler) ret = function
   | NewArray(_, exprs) 
   | NewTuple(exprs) ->
      let exprs = List.map (com.CompileExpr) exprs
      buildExpr <| JSExpr.Array exprs
   | _ -> None

let private seqs com _ = function
   | SpecificCall <@ seq @> (_,_,[CompileExpr com expr]) ->
      buildExpr expr
   | SpecificCall <@ Seq.delay @> (_,_,args) ->
      buildExpr <| com.CompileCall <@ Core.Collections.Seq.delay @> args
   | SpecificCall <@ Seq.map @> (_,_,args) ->
      buildExpr <| com.CompileCall <@ Core.Collections.Seq.map @> args
   | SpecificCall <@ op_Range @>   (_,_,args) ->
      buildExpr <| com.CompileCall <@ Core.Collections.Range.oneStep @> args
   | SpecificCall <@ op_RangeStep @>   (_,_,args) ->
      buildExpr <| com.CompileCall <@ Core.Collections.Range.customStep @> args
   | _ -> None

// TODO TODO TODO: raise, failwith, invalidOp, invalidArg
let private errors com _ expr =
   match expr with
   | SpecificCall <@ raise @> (_,_,[CompileExpr com jse])
   | SpecificCall <@ failwith @> (_,_,[CompileExpr com jse]) ->
      buildStatement <| Throw(expr.DebugInfo, jse)
   | _ -> None

let private printFormatToString (com: Compiler) _ = function
   | Call(_, mi, [Coerce(NewObject(_,args),_)]) when mi.Name = "PrintFormatToString" ->
      buildExpr <| com.CompileCall <@ Core.String.PrintFormatToString @> args
   | _ -> None

let components: CompilerComponent list = [ 
   primitiveValues
   primitiveOps
   arrayCreation
   seqs
   errors
   printFormatToString
]