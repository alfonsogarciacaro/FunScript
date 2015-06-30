module FunScript.Applications

open AST
open Microsoft.FSharp.Compiler.Ast

let private compileSynExp com inf = function
  /// F# syntax: (expr)
  /// Paren(expr, leftParenRange, rightParenRange, wholeRangeIncludingParentheses)
  /// Parenthesized expressions. Kept in AST to distinguish A.M((x,y)) 
  /// from A.M(x,y), among other things.
  | SynExpr.Paren(CompileExpr com inf expr, _, _, _) ->
    buildExpr (Paren expr)

  /// F# syntax: <@ expr @>, <@@ expr @@>
  /// Quote(operator,isRaw,quotedSynExpr,isFromQueryExpression,m)
  | SynExpr.Quote _ -> failwith "Quotes are not supported yet"

  /// F# syntax: 1, 1.3, () etc.
  // TODO: Check the value typ
  | SynExpr.Const(cons, m) ->
    buildExpr <|
      match cons with
      /// F# syntax: ()
      | SynConst.Unit -> JSExpr.Null
      /// F# syntax: true, false
      | SynConst.Bool b -> JSExpr.Boolean b
      /// F# syntax: 13y, 0xFFy, 0o077y, 0b0111101y
      | SynConst.SByte x -> JSExpr.Integer(int x)
      /// F# syntax: 13uy, 0x40uy, 0oFFuy, 0b0111101uy
      | SynConst.Byte x -> JSExpr.Integer(int x)
      /// F# syntax: 13s, 0x4000s, 0o0777s, 0b0111101s
      | SynConst.Int16 x -> JSExpr.Integer(int x)
      /// F# syntax: 13us, 0x4000us, 0o0777us, 0b0111101us
      | SynConst.UInt16 x -> JSExpr.Integer(int x)
      /// F# syntax: 13, 0x4000, 0o0777
      | SynConst.Int32 x -> JSExpr.Integer(x)
      /// F# syntax: 13u, 0x4000u, 0o0777u
      | SynConst.UInt32 x -> JSExpr.Integer(int x)
      /// F# syntax: 13L
      | SynConst.Int64 x -> JSExpr.Number(float x)
      /// F# syntax: 13UL
      | SynConst.UInt64 x -> JSExpr.Number(float x)
      /// F# syntax: 13n
      | SynConst.IntPtr _ -> failwith "Pointers are not supported"
      /// F# syntax: 13un
      | SynConst.UIntPtr _ -> failwith "Pointers are not supported"
      /// F# syntax: 1.30f, 1.40e10f etc.
      | SynConst.Single x -> JSExpr.Number(float x)
      /// F# syntax: 1.30, 1.40e10 etc.
      | SynConst.Double x -> JSExpr.Number(x)
      /// F# syntax: 'a'
      | SynConst.Char c -> JSExpr.String (string c)
      /// F# syntax: 23.4M
      | SynConst.Decimal _ -> failwith "Decimals are not supported yet"
      /// UserNum(value, suffix)
      /// F# syntax: 1Q, 1Z, 1R, 1N, 1G
      | SynConst.UserNum _ -> failwith "Custom literals are not supported yet"
      /// F# syntax: verbatim or regular string, e.g. "abc"
      | SynConst.String(s, _) -> JSExpr.String s
      /// F# syntax: verbatim or regular byte string, e.g. "abc"B.
      /// Also used internally in the typechecker once an array of unit16 contants 
      /// is detected, to allow more efficient processing of large arrays of uint16 constants. 
      | SynConst.Bytes _ -> failwith "Not implemented"
      /// Used internally in the typechecker once an array of unit16 contants 
      /// is detected, to allow more efficient processing of large arrays of uint16 constants. 
      | SynConst.UInt16s _
      /// Old comment: "we never iterate, so the const here is not another SynConst.Measure"
      | SynConst.Measure _ -> failwith "Not expected"

  /// F# syntax: expr : type
  /// Typed(expr, typ, m)
  | SynExpr.Typed(CompileExpr com inf expr, _, _) ->
    buildExpr expr

  /// F# syntax: e1, ..., eN
  | SynExpr.Tuple(exprs, _, _) ->
    exprs |> List.map (com.CompileExpr inf) |> Array |> buildExpr

  /// F# syntax: { f1=e1; ...; fn=en }
  /// SynExpr.Record((baseType, baseCtorArgs, mBaseCtor, sepAfterBase, mInherits), (copyExpr, sepAfterCopyExpr), (recordFieldName, fieldValue, sepAfterField), mWholeExpr)
  /// inherit includes location of separator (for tooling) 
  /// copyOpt contains range of the following WITH part (for tooling)
  /// every field includes range of separator after the field (for tooling)
  | SynExpr.Record(_, _, fields, m) -> failwith "Not implemented"

  /// F# syntax: new C(...)
  /// The flag is true if known to be 'family' ('protected') scope
  /// Note: This is used only when new keyword appears explicitly
  ///       Other constructors are just applications (App)
  | SynExpr.New(flag, typ, expr, m) -> failwith "Not implemented"

  /// SynExpr.ObjExpr(objTy,argOpt,binds,extraImpls,mNewExpr,mWholeExpr)
  /// F# syntax: { new ... with ... }
  | SynExpr.ObjExpr(typ, ident, bindings, infcs, _, m) -> failwith "Not implemented"

  /// F# syntax: 'while ... do ...'
  | SynExpr.While(_, CompileExpr inf com cond, CompileStatement com inf body, m) ->
    buildStatement <| WhileLoop(m, cond, body)

  /// F# syntax: 'for i = ... to ... do ...'
  | SynExpr.For(seqInfo, ident, start, isUp, finish, body, m) -> failwith "Not implemented"

  /// SynExpr.ForEach (spBind, seqExprOnly, isFromSource, pat, enumExpr, bodyExpr, mWholeExpr).
  /// F# syntax: 'for ... in ... do ...'
  | SynExpr.ForEach(spBind, seqExprOnly, isFromSource, pat, enumExpr, bodyExpr, mWholeExpr) -> failwith "Not implemented"

  /// F# syntax: [ e1; ...; en ], [| e1; ...; en |]
  | SynExpr.ArrayOrList(isArray, exprs, _) ->
    if isArray
    then exprs |> List.map (com.CompileExpr inf) |> Array |> buildExpr
    else failwith "Not implemented"

  /// F# syntax: [ expr ], [| expr |]
  | SynExpr.ArrayOrListOfSeqExpr(isArray, CompileExpr com inf expr, m) ->
    if isArray then buildExpr (Array [expr]) else failwith "Not implemented"

  /// CompExpr(isArrayOrList, isNotNakedRefCell, expr)
  /// F# syntax: { expr }
  | SynExpr.CompExpr(isArrayOrList, isNotNakedRefCell, expr, m) -> failwith "Not implemented"

  /// First bool indicates if lambda originates from a method. Patterns here are always "simple" 
  /// Second bool indicates if this is a "later" part of an iterated sequence of lambdas
  /// F# syntax: fun pat -> expr
  | SynExpr.Lambda(isMeth, isSeq, pats, expr, m) -> failwith "Not implemented"

  /// F# syntax: function pat1 -> expr | ... | patN -> exprN
  | SynExpr.MatchLambda(flag, _, clauses, seqInfo, m) -> failwith "Not implemented"

  /// F# syntax: match expr with pat1 -> expr | ... | patN -> exprN
  | SynExpr.Match(seqInfo, expr, clauses, flag, m) -> failwith "Not implemented"

  /// F# syntax: do expr 
  | SynExpr.Do(CompileExpr com inf expr, m) -> buildStatement <| Do(m, expr)

  /// F# syntax: assert expr 
  | SynExpr.Assert(expr, m) -> failwith "Not implemented"

  /// App(exprAtomicFlag, isInfix, funcExpr, argExpr, m)
  ///  - exprAtomicFlag: indicates if the application is syntactically atomic, e.g. f.[1] is atomic, but 'f x' is not
  ///  - isInfix is true for the first app of an infix operator, e.g. 1+2 becomes App(App(+,1),2), where the inner node is marked isInfix 
  ///      (or more generally, for higher operator fixities, if App(x,y) is such that y comes before x in the source code, then the node is marked isInfix=true)
  /// F# syntax: f x
  | SynExpr.App(_, isInfix, funcExpr, argExpr, m) -> failwith "Not implemented"

  /// TypeApp(expr, mLessThan, types, mCommas, mGreaterThan, mTypeArgs, mWholeExpr)
  ///     "mCommas" are the ranges for interstitial commas, these only matter for parsing/design-time tooling, the typechecker may munge/discard them
  /// F# syntax: expr<type1,...,typeN>
  | SynExpr.TypeApp(expr, mLessThan, types, mCommas, mGreaterThan, mTypeArgs, mWholeExpr) -> failwith "Not implemented"

  /// LetOrUse(isRecursive, isUse, bindings, body, wholeRange)
  /// F# syntax: let pat = expr in expr 
  /// F# syntax: let f pat1 .. patN = expr in expr 
  /// F# syntax: let rec f pat1 .. patN = expr in expr 
  /// F# syntax: use pat = expr in expr 
  | SynExpr.LetOrUse(isRecursive, isUse, bindings, body, wholeRange) -> failwith "Not implemented"

//  | TryFinally(CompileStatement com ret body, CompileStatement com Inplace finalBody) ->
//    buildStatement <| TryFinally(body, finalBody)
//
//  // TODO: Type testing... (?)
//  | TryWith(CompileStatement com ret body, _, _, catchVar, CompileStatement com ret catchBody) ->
//    buildStatement <| TryCatch(body, catchVar, catchBody)

  /// F# syntax: try expr finally expr
  | SynExpr.TryFinally(SynExpr.TryWith(bodyExpr, _, catchClauses, _, _, _, _), finalExpr, _, _, _) ->
    let body = com.CompileStatement inf bodyExpr      // TODO: Pass return strategy
    let finalBody = com.CompileStatement inf finalExpr // TODO: InPlace
    // TODO: Convert catchClauses to IfElse chain
    failwith "Not implemented"
//    buildStatement <| TryCatchFinally(body, catchClauses, finalBody)

  | SynExpr.TryFinally(bodyExpr, finalExpr, _, _, _) ->
    let body = com.CompileStatement inf bodyExpr      // TODO: Pass return strategy
    let finalBody = com.CompileStatement inf finalExpr // TODO: InPlace
    buildStatement <| TryFinally(body, finalBody)

  /// F# syntax: try expr with pat -> expr
  | SynExpr.TryWith(bodyExpr, _, catchClauses, _, _, _, _) ->
    let body = com.CompileStatement inf bodyExpr      // TODO: Pass return strategy
    failwith "Not implemented"
//    buildStatement <| TryCatchFinally(body, catchClauses)

  /// F# syntax: lazy expr
  | SynExpr.Lazy(expr, m) -> failwith "Not implemented"

  /// Seq(seqPoint, isTrueSeq, e1, e2, m)
  ///  isTrueSeq: false indicates "let v = a in b; v"
  /// F# syntax: expr; expr
  | SynExpr.Sequential(seqPoint, isTrueSeq, e1, e2, m) -> failwith "Not implemented"

  ///  IfThenElse(exprGuard,exprThen,optionalExprElse,spIfToThen,isFromErrorRecovery,mIfToThen,mIfToEndOfLastBranch)
  /// F# syntax: if expr then expr
  /// F# syntax: if expr then expr else expr
  | SynExpr.IfThenElse(exprGuard,exprThen,optionalExprElse,spIfToThen,isFromErrorRecovery,mIfToThen,mIfToEndOfLastBranch) -> failwith "Not implemented"

  /// F# syntax: ident
  /// Optimized representation, = SynExpr.LongIdent(false,[id],id.idRange) 
  | SynExpr.Ident id -> failwith "Not implemented"

  /// F# syntax: ident.ident...ident
  /// LongIdent(isOptional, longIdent, altNameRefCell, m)
  ///   isOptional: true if preceded by a '?' for an optional named parameter 
  ///   altNameRefCell: Normally 'None' except for some compiler-generated variables in desugaring pattern matching. See SynSimplePat.Id
  | SynExpr.LongIdent(isOptional, longIdent, altNameRefCell, m) -> failwith "Not implemented"

  /// F# syntax: ident.ident...ident <- expr
  | SynExpr.LongIdentSet _ -> failwith "Not implemented"

  /// DotGet(expr, rangeOfDot, lid, wholeRange)
  /// F# syntax: expr.ident.ident
  | SynExpr.DotGet(expr, rangeOfDot, lid, wholeRange) -> failwith "Not implemented"

  /// F# syntax: expr.ident...ident <- expr
  | SynExpr.DotSet(expr, lid, value, m) -> failwith "Not implemented"

  /// F# syntax: expr.[expr,...,expr] 
  | SynExpr.DotIndexedGet(expr, args, _, m) -> failwith "Not implemented"

  /// DotIndexedSet (objectExpr, indexExprs, valueExpr, rangeOfLeftOfSet, rangeOfDot, rangeOfWholeExpr)
  /// F# syntax: expr.[expr,...,expr] <- expr
  | SynExpr.DotIndexedSet(objectExpr, indexExprs, valueExpr, rangeOfLeftOfSet, rangeOfDot, rangeOfWholeExpr) -> failwith "Not implemented"

  /// F# syntax: Type.Items(e1) <- e2 , rarely used named-property-setter notation, e.g. Foo.Bar.Chars(3) <- 'a'
  | SynExpr.NamedIndexedPropertySet(lid, idx, value, m) -> failwith "Not implemented"

  /// F# syntax: expr.Items(e1) <- e2 , rarely used named-property-setter notation, e.g. (stringExpr).Chars(3) <- 'a'
  | SynExpr.DotNamedIndexedPropertySet(expr, lid, idx, value, m)  -> failwith "Not implemented"

  /// F# syntax: expr :? type
  | SynExpr.TypeTest(expr, typ, m) -> failwith "Not implemented"

  /// F# syntax: expr :> type 
  | SynExpr.Upcast(expr, typ, m) -> failwith "Not implemented"

  /// F# syntax: expr :?> type 
  | SynExpr.Downcast(expr, typ, m) -> failwith "Not implemented"

  /// F# syntax: upcast expr
  | SynExpr.InferredUpcast(expr, m) -> failwith "Not implemented"

  /// F# syntax: downcast expr
  | SynExpr.InferredDowncast(expr, m) -> failwith "Not implemented"

  /// F# syntax: null
  | SynExpr.Null m -> failwith "Not implemented"

  /// F# syntax: &expr, &&expr
  | SynExpr.AddressOf(flag, expr, _, m) -> failwith "Not implemented"

  /// F# syntax: ((typar1 or ... or typarN): (member-dig) expr)
  | SynExpr.TraitCall(typars, sign, expr, m) -> failwith "Not implemented"

  /// F# syntax: ... in ... 
  /// Computation expressions only, based on JOIN_IN token from lex filter
  | SynExpr.JoinIn(lhs, _, rhs, m) -> failwith "Not implemented"

  /// F# syntax: <implicit>
  /// Computation expressions only, implied by final "do" or "do!"
  | SynExpr.ImplicitZero m -> failwith "Not implemented"

  /// F# syntax: yield expr 
  /// F# syntax: return expr 
  /// Computation expressions only
  | SynExpr.YieldOrReturn((flag1, flag2), expr, m) -> failwith "Not implemented"

  /// F# syntax: yield! expr 
  /// F# syntax: return! expr 
  /// Computation expressions only
  | SynExpr.YieldOrReturnFrom((flag1, flag2), expr, m) -> failwith "Not implemented"

  /// SynExpr.LetOrUseBang(spBind, isUse, isFromSource, pat, rhsExpr, bodyExpr, mWholeExpr).
  /// F# syntax: let! pat = expr in expr
  /// F# syntax: use! pat = expr in expr
  /// Computation expressions only
  | SynExpr.LetOrUseBang(spBind, isUse, isFromSource, pat, rhsExpr, bodyExpr, mWholeExpr) -> failwith "Not implemented"

  /// F# syntax: do! expr 
  /// Computation expressions only
  | SynExpr.DoBang(expr, m) -> failwith "Not implemented"

  /// Only used in FSharp.Core
  | SynExpr.LibraryOnlyILAssembly _ -> failwith "Only used in FSharp.Core"

  /// Only used in FSharp.Core
  | SynExpr.LibraryOnlyStaticOptimization _ -> failwith "Only used in FSharp.Core"

  /// Only used in FSharp.Core
  | SynExpr.LibraryOnlyUnionCaseFieldGet _ -> failwith "Only used in FSharp.Core"

  /// Only used in FSharp.Core
  | SynExpr.LibraryOnlyUnionCaseFieldSet _ -> failwith "Not implemented"
  
  /// Inserted for error recovery
  | SynExpr.ArbitraryAfterError _ -> failwith "Inserted for error recovery"

  /// Inserted for error recovery
  | SynExpr.FromParseError _ -> failwith "Inserted for error recovery"

  /// Inserted for error recovery when there is "expr." and missing tokens or error recovery after the dot
  | SynExpr.DiscardAfterMissingQualificationAfterDot _ -> failwith "Inserted for error recovery"

