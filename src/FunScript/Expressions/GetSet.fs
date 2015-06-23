module FunScript.GetSet

open AST
open Microsoft.FSharp.Compiler.SourceCodeServices
open Microsoft.FSharp.Core.LanguagePrimitives.IntrinsicFunctions

let private fieldGetSet com _  = function
  | BasicPatterns.FSharpFieldGet(target, typ, fi) ->
    match target with
      | Some (CompileExpr com target) -> PropertyGet(target, String fi.Name)
      | None -> PropertyGet(com.RefType fi.DeclaringEntity, String fi.Name)
    |> buildExpr
  | BasicPatterns.FSharpFieldSet(target, typ, fi, CompileExpr com value) as expr ->
    match target with
      | Some (CompileExpr com target) -> PropertyGet(target, String fi.Name)
      | None -> PropertyGet(com.RefType fi.DeclaringEntity, String fi.Name)
    |> fun get -> buildStatement(Assign(expr.Range, get, value))
  | _ -> None

let private arrayGetSet com _  expr =
   match expr with
   | SpecificCall
      <@ GetArray @> (_,_,[CompileExpr com ar; CompileExpr com i]) ->
      buildExpr <| PropertyGet(ar, i)

   | SpecificCall 
      <@ SetArray @> (_,_,[CompileExpr com ar; CompileExpr com i; CompileExpr com v]) ->
      buildStatement <| Assign(expr.DebugInfo, PropertyGet(ar, i), v)
   | _ -> None

let private propertyGetter (com: ICompiler) _ = function
   // F# Custom exceptions
//   | Patterns.PropertyGet(Some(Patterns.Coerce(Patterns.Var var, t)), pi, []) ->
//      buildExpr <| PropertyGet(JSExpr.Var var, String pi.Name)

  | BasicPatterns.TupleGet(typ, i, e) ->
    buildExpr <| PropertyGet(com.CompileExpr e, Integer i)

  | BasicPatterns.UnionCaseGet(target, typ, uci, fi) ->
    buildExpr <| PropertyGet(com.CompileExpr target, String fi.Name)

  | BasicPatterns.Call(target, pi, _, _, args) as expr when pi.IsPropertyGetterMethod ->
      // F# Ref
//      | Some target when pi.EnclosingEntity.IsByRef -> //.Name = typeof<obj ref>.Name ->
//        buildExpr <| PropertyGet(com.CompileExpr target, String "contents")
    match pi.TryGetAttribute<JSEmitInlineAttribute>() with
    | Some [:? string as emit] -> 
      let args = match target with Some t -> t::args | None -> args
      buildExpr <| EmitExpr(emit, List.map com.CompileExpr args)
    | _ ->
      // TODO: Check if the property has arguments or if it's Item, and if it's static
      buildExpr <| PropertyGet(com.CompileExpr target.Value, String pi.DisplayName)
  | _ -> None

let private propertySetter (com: ICompiler) _ = function
  | BasicPatterns.UnionCaseSet(target, typ, uci, fi, CompileExpr com v) as expr ->
    buildStatement <| Assign(expr.Range, PropertyGet(com.CompileExpr target, String fi.Name), v)

  | BasicPatterns.Call(target, pi, _, _, args) as expr when pi.IsPropertySetterMethod ->
//    // F# Ref
//    | Some target when pi.DeclaringType.Name = typeof<obj ref>.Name ->
//      buildStatement <| Assign(expr.DebugInfo,
//                                PropertyGet(com.CompileExpr target, String "contents"),
//                                com.CompileExpr assignment)
    match pi.TryGetAttribute<JSEmitInlineAttribute>() with
    | Some [:? string as emit] -> 
      let args = match target with Some t -> t::args | None -> args
      let emit = sprintf "(%s={%i})" emit (args.Length-1)
      buildStatement <| Do(Range.Zero, EmitExpr(emit, List.map com.CompileExpr args))
    | _ ->
      // TODO: Check if the property has arguments or if it's Item, and if it's static
      failwith "Not implemented"
  | _ -> None

let components: CompilerComponent list = [ 
   fieldGetSet
   arrayGetSet
   propertyGetter
   propertySetter
]