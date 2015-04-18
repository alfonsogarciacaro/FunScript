module internal FunScript.Conversions

open AST
open System.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.DerivedPatterns
open Microsoft.FSharp.Reflection

let private emit (com: ICompiler) pattern target =
   buildExpr <| EmitExpr(pattern, [com.CompileExpr target])

let private typeTest com _ = function
   | Patterns.TypeTest(CompileExpr com jse, t) ->
      // Check if primary constructor has inline replacement (o instanceof attr.Emit)
      // Rest -> o instanceof <type/construcotr)
      buildExpr <|
         // For primitives, type reference is compiled as string
         if t.IsPrimitive || t = typeof<string> || t.IsEnum || t = typeof<unit> ||
            t.IsArray || (FSharpType.IsTuple t) ||
            FSharpType.IsFunction t then
            BinaryOp(UnaryOp("typeof", jse), "===", com.RefType t)
         elif t = typeof<obj> then
            Boolean true
         elif t.IsInterface then
            PropertyGet(jse, String "constructor")
            |> fun cons -> PropertyGet(cons, com.RefType t)
            |> fun infc -> BinaryOp(infc, "!==", Undefined)
         else
            BinaryOp(jse, "instanceof", com.RefType t)

   | Patterns.UnionCaseTest (CompileExpr com jse, uci) ->
      buildExpr <|
         if uci.DeclaringType.Name = typeof<_ option>.Name then
            let op = if uci.Name = "Some" then "!==" else "==="
            BinaryOp(jse, op, Null)
         else
            BinaryOp(PropertyGet(jse, String "Tag"), "===", Integer uci.Tag)
   | _ -> None

let private coerce (com: ICompiler) _ = function
   | Patterns.Coerce (sourceExpr, targetType) ->
      if targetType.IsInterface && (not sourceExpr.Type.IsInterface) then
         com.AddInterface(sourceExpr.Type, targetType)
      buildExpr <| com.CompileExpr sourceExpr
   | SpecificCall <@ ignore @>   (_,_,[x]) -> buildExpr <| com.CompileExpr x
   | SpecificCall <@ box @>      (_,_,[x]) -> buildExpr <| com.CompileExpr x
   | SpecificCall <@ unbox @>    (_,_,[x]) -> buildExpr <| com.CompileExpr x

   | Patterns.Call(None, mi, [x]) ->
      if mi.Name = "UnboxGeneric" || mi.Name = "UnboxFast"
      then buildExpr <| com.CompileExpr x
      else None

   | _ -> None

let private conversion (com: ICompiler) _ = function
  | SpecificCall <@ sbyte @>   (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ byte @>    (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ int16 @>   (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ uint16 @>  (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ int @>     (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ int32 @>   (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ uint32 @>  (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ int64 @>   (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ uint64 @>  (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ float @>   (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ single @>  (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ float32 @> (_,_,[x]) -> buildExpr <| com.CompileExpr x
  | SpecificCall <@ double @>  (_,_,[x]) -> buildExpr <| com.CompileExpr x

  | SpecificCall <@ string @> (_,_,[x]) ->
     emit com "{0}.toString()" x
  | Patterns.Call(Some x, mi, []) when mi.Name = "ToString" ->
     emit com "{0}.toString()" x
  | SpecificCall <@ char @>  (_,_,[x]) ->
     emit com "String.fromCharCode({0})" x
  | _ -> None

// TODO: opening the System namespace is not recommended
// Provide a different way to parse numbers?
let private parsing (com: ICompiler) _ = function
   | SpecificCall <@ System.Boolean.Parse @> (_,_,[x]) -> emit com "parseInt({0})" x   // TODO: Use Boolean(x) or !!x ?
   | SpecificCall <@ System.Byte.Parse @>    (_,_,[x]) -> emit com "parseInt({0})" x
   | SpecificCall <@ System.UInt16.Parse @>  (_,_,[x]) -> emit com "parseInt({0})" x
   | SpecificCall <@ System.UInt32.Parse @>  (_,_,[x]) -> emit com "parseInt({0})" x
   | SpecificCall <@ System.UInt64.Parse @>  (_,_,[x]) -> emit com "parseInt({0})" x
   | SpecificCall <@ System.SByte.Parse @>   (_,_,[x]) -> emit com "parseInt({0})" x
   | SpecificCall <@ System.Int16.Parse @>   (_,_,[x]) -> emit com "parseInt({0})" x
   | SpecificCall <@ System.Int32.Parse @>   (_,_,[x]) -> emit com "parseInt({0})" x
   | SpecificCall <@ System.Int64.Parse @>   (_,_,[x]) -> emit com "parseInt({0})" x
     
   | SpecificCall <@ System.Single.Parse @>  (_,_,[x]) -> emit com "parseFloat({0})" x
   | SpecificCall <@ System.Double.Parse @>  (_,_,[x]) -> emit com "parseFloat({0})" x

//TODO: Consider whether we should support this... We might need our own decimal type. 
// ExpressionReplacer.createUnsafe <@ Decimal.Parse @> <@ parseFloat @>
// ExpressionReplacer.create <@ Guid.Parse @> <@ parseGuid @>
   | _ -> None

let private toPropertyNameFromLambda (com: ICompiler) _ = function
   | Patterns.Quote(Patterns.Lambda(_,Patterns.PropertyGet(_, pi, _))) ->
      buildExpr <| com.CompileExpr (Expr.Value(pi.Name))
   | _ -> None
   
let components: CompilerComponent list = [
   conversion
   parsing
   typeTest
   coerce
   toPropertyNameFromLambda
]
