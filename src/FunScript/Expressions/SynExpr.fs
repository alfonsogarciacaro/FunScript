module FunScript.Applications

open AST
open Microsoft.FSharp.Compiler.SourceCodeServices

let rec compileArgs (com: ICompiler) (scope: IScopeInfo) (args: FSharpExpr list) =
  match args with
  | [] | [DerivedPatterns.Unit] -> []
  | _ -> args |> List.map (com.CompileExpr scope)

let rec compileArgVars (args: FSRef list) =
  match args with
  | [] -> []
  | [u] when u.FullType.TypeDefinition.FullName = "Microsoft.FSharp.Core.unit" -> []
  | _ -> args |> List.map Var

let getRef (com: ICompiler) (inf: IScopeInfo) (ent: FSharpEntity) (objExprOpt: FSharpExpr option) =
    match objExprOpt with
    | Some objExpr -> com.CompileExpr inf objExpr
    | None -> com.RefType ent

let compileBasicPattern (com: ICompiler) (scope: IScopeInfo) = function
  (** ## Values *)
  // TODO TODO TODO
//  | BasicPatterns.Value x when x.IsConstructorThisValue || x.IsMemberThisValue ->
//    buildExpr This
  | BasicPatterns.ThisValue _thisType ->
    buildExpr This
  | BasicPatterns.BaseValue _baseType ->
    buildExpr Super
  | BasicPatterns.Value v ->
    scope.ReplaceIfNeeded v |> Var |> buildExpr
  | BasicPatterns.Const(constValueObj, constType) ->
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
        else failwithf "Unexpected Const %s" <| constValueObj.GetType().FullName
   | BasicPatterns.DefaultValue(constType) ->
      if constType.TypeDefinition.IsValueType then JSExpr.Integer 0 else JSExpr.Null
      |> buildExpr

  (** ## Getters and Setters *)

  // TODO: Include Properties
  (** - Fields (including records) *)
  | BasicPatterns.FSharpFieldGet(objExprOpt, t, fieldInfo) ->
    buildExpr <| PropertyGet(getRef com scope t.TypeDefinition objExprOpt, String fieldInfo.Name)
  | BasicPatterns.FSharpFieldSet(objExprOpt, t, fieldInfo, CompileExpr com scope argExpr) as e ->
    buildStatement <| Assign(e.Range, PropertyGet(getRef com scope t.TypeDefinition objExprOpt, String fieldInfo.Name), argExpr)

  (** - Non-indexed properties *)
  | BasicPatterns.Call(objExprOpt, prop, _, _, [Unit]) when prop.IsPropertyGetterMethod ->
    buildExpr <| PropertyGet(getRef com scope prop.EnclosingEntity objExprOpt, String prop.CompiledName)
  | BasicPatterns.Call(objExprOpt, prop, _, _, [CompileExpr com scope arg]) as e when prop.IsPropertySetterMethod ->
    buildStatement <| Assign(e.Range, PropertyGet(getRef com scope prop.EnclosingEntity objExprOpt, String prop.CompiledName), arg)

  (** - Tuples *)
  | BasicPatterns.TupleGet(_tupleType, tupleElemIndex, CompileExpr com scope tupleExpr) ->
    buildExpr <| PropertyGet(tupleExpr, Integer tupleElemIndex)

  (** - Arrays *)
  | BasicPatterns.Call(None, f, _, _, [CompileExpr com scope ar; CompileExpr com scope idx])
    when f.FullName = "Microsoft.FSharp.Core.LanguagePrimitives.IntrinsicFunctions.GetArray" ->
    buildExpr <| PropertyGet(ar, idx)
  | BasicPatterns.Call(None, f, _, _, [CompileExpr com scope ar; CompileExpr com scope idx; CompileExpr com scope arg]) as e
    when f.FullName = "Microsoft.FSharp.Core.LanguagePrimitives.IntrinsicFunctions.SetArray" ->
    buildStatement <| Assign(e.Range, PropertyGet(ar, idx), arg)

  (** - Union cases *)
  // TODO TODO TODO: Put all ItemX fields in an array
  | BasicPatterns.UnionCaseGet(CompileExpr com scope unionExpr, _unionType, _unionCase, unionCaseField) ->
    buildExpr <| PropertyGet(unionExpr, String unionCaseField.Name)
  | BasicPatterns.UnionCaseSet(CompileExpr com scope unionExpr, _unionType, _unionCase, unionCaseField, CompileExpr com scope valueExpr) as e ->
    buildStatement <| Assign(e.Range, PropertyGet(unionExpr, String unionCaseField.Name), valueExpr)

  (** ## Let bindings *)
  | BasicPatterns.LetRec(bindingExprs, CompileStatement com scope body) ->
      bindingExprs
      |> List.fold (fun (acc: JSStatement) (var, assignment) ->
         Let(assignment.Range, var, com.CompileExpr scope assignment, acc)) body
      |> buildStatement
  | BasicPatterns.Let((var, CompileExpr com scope assignment), CompileStatement com scope body) as e ->
      Let(e.Range, var, assignment, body) |> buildStatement
  | BasicPatterns.ValueSet(var, CompileExpr com scope assignment) as e -> 
      buildStatement <| Assign(e.Range, JSExpr.Var var, assignment)

  (** ## Constructors *)
  | BasicPatterns.NewTuple(_typ, argExprs)
  | BasicPatterns.NewArray(_typ, argExprs) ->
    argExprs |> List.map (com.CompileExpr scope) |> Array |> buildExpr
  | BasicPatterns.ObjectExpr(objType, _baseCallExpr, overrides, interfaceImplementations) ->
    match interfaceImplementations with
    | [] -> failwith ""
    | _ -> failwith "Object Expressions implementing more than one interface not yet supported"
  
  | BasicPatterns.NewDelegate(delegateType, delegateBodyExpr) ->
    match delegateBodyExpr with
    | DerivedPatterns.Lambdas(lambdaVars, bodyExpr) -> failwith "Not implemented"
    | BasicPatterns.Value(lambda) -> failwith "Not implemented"
  | BasicPatterns.Lambda(lambdaVar, bodyExpr) -> failwith "Not implemented"
  
  | BasicPatterns.NewObject(ctor, _, argExprs) ->
    buildExpr <|
      if ctor.IsImplicitConstructor
      then New(com.RefType ctor.EnclosingEntity, compileArgs com scope argExprs)
      else failwith "Not implemented"
  | BasicPatterns.NewRecord(typ, argExprs) ->
    buildExpr <| New(com.RefType typ.TypeDefinition, compileArgs com scope argExprs)
  | BasicPatterns.NewUnionCase(typ, uci, argExprs) ->
    buildExpr <| New(com.RefType typ.TypeDefinition, [String uci.Name; Array(compileArgs com scope argExprs)])

  (** ## Control Flow *)
  | BasicPatterns.Sequential(firstExpr, secondExpr) -> failwith "Not implemented"
  | BasicPatterns.FastIntegerForLoop(startExpr, limitExpr, consumeExpr, isUp) -> failwith "Not implemented"
  | BasicPatterns.WhileLoop(guardExpr, bodyExpr) -> failwith "Not implemented"
  | BasicPatterns.TryFinally(bodyExpr, finalizeExpr) -> failwith "Not implemented"
  | BasicPatterns.TryWith(bodyExpr, _, _, catchVar, catchExpr) -> failwith "Not implemented"
  | BasicPatterns.IfThenElse (guardExpr, thenExpr, elseExpr) -> failwith "Not implemented"

  (** ## Pattern Matching *)
  | BasicPatterns.DecisionTree(decisionExpr, decisionTargets) -> failwith "Not implemented"
  | BasicPatterns.DecisionTreeSuccess (decisionTargetIdx, decisionTargetExprs) -> failwith "Not implemented"

  (** ## Type testing (and conversions) *)
  // TODO TODO TODO: Include cast operators

  // TODO: Replace if it's Let(Var, Coerce(Value), ...)
  | BasicPatterns.Coerce(_typ, CompileExpr com scope e) ->
    buildExpr e
  | BasicPatterns.TypeTest(ty, CompileExpr com scope jse) ->
    let tydef = ty.TypeDefinition
    // Check if primary constructor has inline replacement (o instanceof attr.Emit)
    // Rest -> o instanceof <type/constructor>
    buildExpr <|
        // TODO: Special cases
        if ty.IsTupleType || ty.IsFunctionType || tydef.IsValueType || tydef.IsEnum ||
          tydef.IsArrayType (* || tydef = typeof<string> || tydef = typeof<unit> *) then
          BinaryOp(UnaryOp("typeof", jse), "===", com.RefType tydef)
        // TODO TODO TODO
//        elif tydef = typeof<obj> then
//          Boolean true
//        elif tydef.IsInterface then
//          PropertyGet(jse, String "constructor")
//          |> fun cons -> PropertyGet(cons, com.RefType tydef)
//          |> fun infc -> BinaryOp(infc, "!==", Undefined)
        else
          BinaryOp(jse, "instanceof", com.RefType tydef)
  | BasicPatterns.UnionCaseTest(CompileExpr com scope unionExpr, unionType, uci) ->
    buildExpr <|
      if unionType.TypeDefinition.DisplayName.ToLower() = "option" then
        let op = if uci.Name = "Some" then "!=" else "=="
        BinaryOp(unionExpr, op, Null)
      else
        BinaryOp(PropertyGet(unionExpr, String "Tag"), "===", String uci.Name)
  | BasicPatterns.UnionCaseTag(unionExpr, _unionType) ->
    buildExpr <| PropertyGet(com.CompileExpr scope unionExpr, String "Tag")
  
  (** ## Applications *)
  | BasicPatterns.Application(funcExpr, typeArgs, argExprs) -> failwith "Not implemented"
  | BasicPatterns.Call(objExprOpt, memberOrFunc, typeArgs1, typeArgs2, argExprs) -> failwith "Not implemented"

  (** ## Not supported *)
  | BasicPatterns.Quote _ as e -> failwithf "Not supported %A" e
  | BasicPatterns.TraitCall _ as e -> failwithf "Not supported %A" e
  | BasicPatterns.TypeLambda _ as e -> failwithf "Not supported %A" e
  | BasicPatterns.AddressOf _ as e -> failwithf "Not supported %A" e
  | BasicPatterns.AddressSet _ as e -> failwithf "Not supported %A" e
  | BasicPatterns.ILAsm _ as e -> failwithf "Not supported %A" e
  | BasicPatterns.ILFieldGet _ as e -> failwithf "Not supported %A" e
  | BasicPatterns.ILFieldSet _ as e -> failwithf "Not supported %A" e
  | _ as e -> failwithf "Not supported %A" e
