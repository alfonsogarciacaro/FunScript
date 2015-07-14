module internal FunScript.PrimitiveTypes

open AST
open Microsoft.FSharp.Compiler.SourceCodeServices
open Microsoft.FSharp.Compiler.SourceCodeServices.BasicPatterns

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
   | Const(constValueObj, constType) ->
      buildExpr <|
         match constValueObj with
         | :? unit ->        JSExpr.Null        
         | :? bool   as x -> JSExpr.Boolean x        
         | :? char   as x -> JSExpr.String (string x)
         | :? string as x -> JSExpr.String x         
         | :? sbyte  as x -> JSExpr.Integer(int x)   
         | :? byte   as x -> JSExpr.Integer(int x)   
         | :? int16  as x -> JSExpr.Integer(int x)   
         | :? int32  as x -> JSExpr.Integer(x)       
         | :? int64  as x -> JSExpr.Float(float x)  
         | :? uint16 as x -> JSExpr.Float(float x)  
         | :? uint32 as x -> JSExpr.Float(float x)  
         | :? uint64 as x -> JSExpr.Float(float x)  
         | :? single as x -> JSExpr.Float(float x)  
         | :? double as x -> JSExpr.Float(x)        
         // TODO: our own decimal type?
         | _ ->
            if constType.TypeDefinition.IsEnum
            then JSExpr.Integer(unbox constValueObj)
            else failwith "Unexpected" // TODO TODO TODO
   | DefaultValue(constType) ->
      if constType.TypeDefinition.IsValueType then JSExpr.Integer 0 else JSExpr.Null
      |> buildExpr
   | _ -> None

let private isPrimitive (t: FSharpType) =
   t.TypeDefinition.IsValueType || t.TypeDefinition.FullName = "System.String"

let private unaryOp (com: ICompiler) op hsT hs =
// TODO TODO TODO: If not primitive, check if there's an implementation or default (generics?)
//   if isPrimitive hsT
//   then buildExpr <| UnaryOp(op, com.CompileExpr hs)
//   else None
   buildExpr <| UnaryOp(op, com.CompileExpr hs)

let private binaryOp (com: ICompiler) op lhsT rhsT lhs rhs =
// TODO TODO TODO: If not primitive, check if there's an implementation or default (generics?)
//   if isPrimitive lhsT && isPrimitive rhsT
//   then buildExpr <| BinaryOp(com.CompileExpr lhs, op, com.CompileExpr rhs)
//   else None
   buildExpr <| BinaryOp(com.CompileExpr lhs, op, com.CompileExpr rhs)

let private primitiveOps com _ = function
   // Arithmetic operators
   | Call(target, mi, typeGenArgs, methGenArgs, [hs]) ->
      let hsT = hs.Type
      match mi.FullName with
      | "Microsoft.FSharp.Core.Operators.( ~- )" -> unaryOp com "-" hsT hs
      | "Microsoft.FSharp.Core.Operators.not" -> unaryOp com "!" typeof<bool> hs
      | _ -> None
   | Call(target, mi, typeGenArgs, methGenArgs, [lhs;rhs]) ->
      let lhsT, rhsT = lhs.Type, rhs.Type
      match mi.FullName with
      | "Microsoft.FSharp.Core.Operators.( + )" -> binaryOp com "+" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.Operators.( - )" -> binaryOp com "-" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.Operators.( * )" -> binaryOp com "*" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.Operators.( / )" -> binaryOp com "/" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.Operators.( % )" -> binaryOp com "%" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.Operators.( &&& )" -> binaryOp com "&" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.Operators.( ||| )" -> binaryOp com "|" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.Operators.( >>> )" -> binaryOp com ">>" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.Operators.( <<< )" -> binaryOp com "<<" lhsT rhsT lhs rhs
      | "Microsoft.FSharp.Core.LanguagePrimitives.IntrinsicOperators.( || )" ->
         binaryOp com "||" typeof<bool> typeof<bool> lhs rhs
      | "Microsoft.FSharp.Core.LanguagePrimitives.IntrinsicOperators.( && )" ->
         binaryOp com "&&" typeof<bool> typeof<bool> lhs rhs
      | _ -> None
   | _ -> None

let private arrayCreation (com: ICompiler) ret = function
   | NewArray(_, exprs) 
   | NewTuple(_, exprs) ->
      let exprs = List.map (com.CompileExpr) exprs
      buildExpr <| JSExpr.Array exprs
   | _ -> None

open Microsoft.FSharp.Quotations.DerivedPatterns

let private seqs com _ = function
   | SpecificCall <@ seq @> (_,_,[CompileExpr com expr]) ->
      buildExpr expr
   | SpecificCall <@ Seq.delay @> (_,_,args) ->
      buildExpr <| com.CompileCall <@ Core.Seq.delay @> args
   | SpecificCall <@ Seq.map @> (_,_,args) ->
      buildExpr <| com.CompileCall <@ Core.Seq.map @> args
   | SpecificCall <@ op_Range @>   (_,_,args) ->
      buildExpr <| com.CompileCall <@ Core.Range.oneStep @> args
   | SpecificCall <@ op_RangeStep @>   (_,_,args) ->
      buildExpr <| com.CompileCall <@ Core.Range.customStep @> args
   | _ -> None

let private errors com _ expr =
   match expr with
   | SpecificCall <@ Core.Operators.raise @> (_,_,[CompileExpr com jse]) ->
      buildStatement <| Throw(expr.DebugInfo, jse)
   | _ -> None

let private printFormatToString (com: ICompiler) _ = function
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