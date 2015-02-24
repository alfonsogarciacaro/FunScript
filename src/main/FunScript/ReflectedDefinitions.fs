module internal FunScript.ReflectedDefinitions

open AST
open Quote
open Microsoft.FSharp.Quotations
open System
open System.Reflection
open Microsoft.FSharp.Reflection
open InternalCompiler

let private (|List|) = Option.toList

let private (|NonNull|_|) x = 
   if obj.ReferenceEquals(x, null) then None
   else Some x

let private replaceThisInExpr (expr : Expr) =
    let var = ref None
    let fixedExpr = expr.Substitute(fun v ->
        if v.Name = "this" then
            let thisVar = 
                match !var with
                | None -> 
                    let thisVar = Var("__this", v.Type) 
                    var := Some (v, thisVar)
                    thisVar
                | Some(_, v) -> v
            Some(Expr.Var thisVar)
        else None)
    match !var with
    | None -> expr
    | Some (originalThis, newThis) -> Expr.Let(newThis, Expr.Var originalThis, fixedExpr)

let private getJavaScriptVarsInEmitBlock(emitBlock : string) =
    emitBlock.Split [|';';'{';'}';'(';')'|] 
    |> Array.map (fun stmt -> stmt.Trim())
    |> Array.collect (fun stmt ->
        let parts = stmt.Split([|' '; ',';'\n';'\r';'\t'|], StringSplitOptions.RemoveEmptyEntries)
        if parts.Length > 1 && parts.[0] = "var" then parts.[1..]
        else [||])
    |> set

let private genMethod (mb:MethodBase) (replacementMi:MethodBase) (vars:Var list) bodyExpr var (compiler:InternalCompiler.ICompiler) =
   match replacementMi.GetCustomAttribute<JSEmitAttribute>() with
   | meth when meth <> Unchecked.defaultof<_> ->
      let code padding (scope : VariableScope ref) =
         let _, assignedNames = !scope |> addVarsToScope vars
         let jsBodyVars = 
            if assignedNames <> [] then getJavaScriptVarsInEmitBlock meth.Emit
            else Set.empty
         let conflicts = Set.intersect (set assignedNames) jsBodyVars
         let conflictResolution = 
             conflicts |> Seq.map (fun name -> sprintf "var $_%s = %s;" name name)
             |> String.concat ""
         let body =
             vars
             |> List.zip assignedNames
             |> List.mapi (fun i v -> i,v)
             |> List.fold (fun (acc:string) (i,(name, v)) ->
                let replacement =
                    if conflicts.Contains name then sprintf "$_%s" name
                    else (Reference v).Print(padding, scope)
                acc.Replace(sprintf "{%i}" i, replacement)
                ) meth.Emit
         conflictResolution + body
      [ Assign(Reference var, Lambda(vars, Block[EmitStatement(fun (padding, scope) -> code padding scope)])) ]
   | _ when mb.IsConstructor ->
      
      let fixedBodyExpr = replaceThisInExpr bodyExpr
      [
         Assign(Reference var, Lambda(vars, Block(compiler.Compile ReturnStrategy.ReturnFrom fixedBodyExpr)))
      ]
   | _ -> 
      [ Assign(
         Reference var, 
         Lambda(vars, 
            Block(compiler.Compile ReturnStrategy.ReturnFrom bodyExpr))) ]

let private deconstructTuple (tupleVar : Var) =
    if tupleVar.Type = typeof<unit> then
        [tupleVar], Expr.Value(())
    else
        let elementTypes = FSharpType.GetTupleElements tupleVar.Type
        let elementVars =
            elementTypes |> Array.mapi (fun i elementType ->
                Var(sprintf "%s_%i" tupleVar.Name i, elementType, tupleVar.IsMutable))
            |> Array.toList
        let elementExprs = elementVars |> List.map Expr.Var
        let tupleConstructionExpr =
            match elementExprs with
            | [] -> Expr.Value(()) 
            | _ -> Expr.NewTuple elementExprs
        elementVars, tupleConstructionExpr

let private extractVars (mb : MethodBase) (argCounts : CompilationArgumentCountsAttribute) = function
    | DerivedPatterns.Lambdas(vars, bodyExpr) ->
        let instanceVar, argVars =
            if mb.IsStatic || mb.IsConstructor then None, vars
            elif vars.Head.Length <> 1 then failwith "Unexpected argument format"
            else Some vars.Head.[0], vars.Tail
        let actualArgCounts =
            let hasCounts = argCounts <> Unchecked.defaultof<_>
            if hasCounts then argCounts.Counts |> Seq.toList |> Some
            else None
        let expectedArgCount = 
            let baseParamCount = max 1 (mb.GetParameters().Length)
            match actualArgCounts with
            | None -> baseParamCount
            | Some counts -> max baseParamCount (counts |> Seq.sum)
        let groupCounts =
            match actualArgCounts with
            | None -> argVars |> List.map List.length
            | Some counts -> counts
        let bodyExpr, freeArgVars = 
            List.zip groupCounts argVars
            |> List.fold (fun (totalCount, groups) (groupCount, varGroup) ->
                let subTotal = groupCount + totalCount
                subTotal, (subTotal, groupCount, varGroup) :: groups) (0, [])
            |> snd
            |> List.fold (fun (restExpr, freeVars) (subTotal, groupCount, varGroup) ->
                if subTotal > expectedArgCount && 
                   subTotal - groupCount >= expectedArgCount then
                    if varGroup.Length = 1 then 
                        Expr.Lambda(varGroup.[0], restExpr), freeVars
                    elif varGroup.Length = groupCount then
                        failwith "todo"
                    else failwith "Unexpected argument format"
                elif subTotal > expectedArgCount then
                    failwith "Unexpected argument format"
                else
                    if varGroup.Length = groupCount then
                        restExpr, varGroup @ freeVars
                    elif varGroup.Length = 1 then
                        let tupleVar = varGroup.[0]
                        let elementVars, tupleConstructionExpr =
                            deconstructTuple tupleVar
                        Expr.Let(tupleVar, tupleConstructionExpr, restExpr), elementVars @ freeVars
                    else
                        failwith "Unexpected argument format") (bodyExpr, [])
        let freeVars =
            match instanceVar with
            | None -> freeArgVars
            | Some ivar -> ivar :: freeArgVars
        freeVars, bodyExpr
    | expr -> [], expr

let methodCallPattern (mb:MethodBase) =
    let argCounts = mb.GetCustomAttribute<CompilationArgumentCountsAttribute>()
    match Expr.tryGetReflectedDefinition mb with
    | Some fullExpr -> Some(fun () -> extractVars mb argCounts fullExpr)
    | None -> None

let (|CallPattern|_|) = methodCallPattern

let replaceIfAvailable (compiler:InternalCompiler.ICompiler) (mb : MethodBase) callType =
   match compiler.ReplacementFor mb callType with
   | None -> mb //GetGenericMethod()...
   | Some mi -> upcast mi

let tryCreateGlobalMethod compiler mb callType =
   match replaceIfAvailable compiler mb callType with
   | CallPattern getVarsExpr as replacementMi ->
      let typeArgs = Reflection.getGenericMethodArgs replacementMi
      Some(
         compiler.DefineGlobal mb (fun var ->
            let vars, bodyExpr = getVarsExpr()
            genMethod mb replacementMi vars bodyExpr var compiler))
   | _ -> None

let createGlobalMethod compiler mb callType =
    match tryCreateGlobalMethod compiler mb callType with
    | None -> raise <| Exceptions.ReflectedDefinition mb
    | Some x -> x

let getObjectConstructorVar compiler ci =
   createGlobalMethod compiler ci Quote.ConstructorCall

let private createConstruction
      (|Split|) 
      (returnStrategy: InternalCompiler.ReturnStrategy)
      (compiler: InternalCompiler.ICompiler)
      (exprs: seq<Expr list>)
      (ci: MethodBase) =
    let decls, refs =
        exprs |> List.concat |> Reflection.getDeclarationAndReferences (|Split|)
    let call =
        if FSharpType.IsExceptionRepresentation ci.DeclaringType then
            let cons = Reflection.getCustomExceptionConstructorVar compiler ci
            New(cons, refs)
        else
            let cons = getObjectConstructorVar compiler ci
            if Reflection.isPrimaryConstructor ci
            then New(cons, refs)
            else Apply(Reference cons, refs) // Secondary constructors don't need new keyword
    [ yield! decls |> List.concat
      yield returnStrategy.Return call ]

let (|OptionPattern|_|) = function
    | Patterns.NewUnionCase(uci, []) when 
            uci.Tag = 0 && 
            uci.DeclaringType.Name = typeof<obj option>.Name ->
        Some(None)
    | Patterns.NewUnionCase(uci, [expr]) when 
            uci.Tag = 1 && 
            uci.DeclaringType.Name = typeof<obj option>.Name ->
        Some(Some expr)
    | _ -> None

let tryReplace (x : string) y (str : string) =
    let newStr = str.Replace(x, y)
    if str <> newStr then Some newStr
    else None

let orElse f = function
    | None -> f()
    | Some _ as x -> x

let private createEmitInlineExpr (|Split|) (emit : JSEmitInlineAttribute) exprs =
    let isOptionalParam i =
        emit.Emit.Contains (sprintf "{?%i}" i)
    let isArrayParam i =
        emit.Emit.Contains (sprintf "{%i...}" i) &&
        not(emit.Emit.Contains (sprintf "{%i" (i+1))) &&
        not(emit.Emit.Contains (sprintf "{?%i" (i+1)))
    let decls, refs =
        exprs |> List.mapi (fun i ->
            function
            | OptionPattern None when isOptionalParam i -> []
            | OptionPattern(Some(Split(decls, ref))) when isOptionalParam i -> [decls, (i, ref)]
            | Patterns.NewArray(_, exprs) when isArrayParam i ->
                // Note: Assuming that this is the last parameter
                exprs |> List.mapi (fun j (Split(decls, ref)) ->
                    decls, ((i+j), ref))
            | Split(decls, ref) -> [decls, (i, ref)])
        |> List.concat
        |> List.toArray
        |> Array.unzip
    let refMap = refs |> Map.ofArray

    decls |> Seq.concat |> Seq.toList,
    EmitExpr(fun (padding, scope) ->
        let rec build i acc =
            let next =
                match refMap.TryFind i with
                | None -> 
                    acc |> tryReplace (sprintf ", {?%i}" i) ""
                    |> orElse (fun () ->
                        acc |> tryReplace (sprintf ", {%i...}" i) "")
                    |> orElse (fun () ->
                        acc |> tryReplace (sprintf ",{%i...}" i) "")
                    |> orElse (fun () ->
                        acc |> tryReplace (sprintf "{?%i}" i) "")
                    |> orElse (fun () ->
                        acc |> tryReplace (sprintf "{%i...}" i) "")
                | Some (ref : JSExpr) ->
                    acc |> tryReplace (sprintf "{%i}" i) (ref.Print(padding, scope))
                    |> orElse (fun () ->
                        acc |> tryReplace (sprintf "{?%i}" i) (ref.Print(padding, scope)))
                    |> orElse (fun () ->
                        acc |> tryReplace (sprintf ", {%i...}" i) (sprintf ", %s, {%i...}" (ref.Print(padding, scope)) (i+1)))
                    |> orElse (fun () ->
                        acc |> tryReplace (sprintf ",{%i...}" i) (sprintf ", %s, {%i...}" (ref.Print(padding, scope)) (i+1)))
                    |> orElse (fun () ->
                        acc |> tryReplace (sprintf "{%i...}" i) (sprintf "%s, {%i...}" (ref.Print(padding, scope)) (i+1)))
            match next with
            | None -> acc
            | Some nextAcc -> build (i+1) nextAcc
        build 0 emit.Emit)


let private (|JSEmitInlineMethod|_|) (mi : MethodInfo) =
    match mi.GetCustomAttribute<JSEmitInlineAttribute>() with
    | x when x = Unchecked.defaultof<_> -> None
    | attr -> Some(mi, attr)
        
let (@.) x ys =
    match x with
    | Some y -> y :: ys
    | None -> ys

let private jsEmitInlineMethodCalling =
    CompilerComponent.create <| fun (|Split|) compiler returnStrategy ->
        function
        | Patterns.Call(objExpr, JSEmitInlineMethod(mi, attr), exprs) ->
            let allExprs = objExpr @. exprs |> List.toArray
            let argExprs, k =
                let nameParts = mi.Name.Split [|'.'|]
                if nameParts |> Array.exists (fun part -> part.StartsWith "set_") then
                    let (Split(valDecl, valExpr)) = allExprs.[allExprs.Length - 1]
                    allExprs.[.. allExprs.Length - 1] |> Array.toList,
                    fun (propDecls, propExpr) ->
                        valDecl @ propDecls @ [Assign(propExpr, valExpr)],
                        JSExpr.Null
                else allExprs |> Array.toList, id
            let decls, ref =
                createEmitInlineExpr (|Split|) attr argExprs
                |> k
            [
                yield! decls
                yield returnStrategy.Return ref
            ]
        | _ -> []
             

let private (|SpecialOp|_|) = Quote.specialOp

let private createCall 
      (|Split|) 
      (returnStrategy:InternalCompiler.ReturnStrategy)
      (compiler:InternalCompiler.ICompiler)
      exprs mi =
    let exprs = exprs |> List.concat
    let decls, refs = Reflection.getDeclarationAndReferences (|Split|) exprs
    let methRef = createGlobalMethod compiler mi Quote.MethodCall
    [ yield! decls |> List.concat
      yield returnStrategy.Return <| Apply(Reference methRef, refs) ]

let private methodCalling =
   CompilerComponent.create <| fun split compiler returnStrategy ->
      function
      | Patterns.Call(List objExpr, mi, exprs) -> 
         createCall split returnStrategy compiler [objExpr; exprs] mi
      | _ -> []

let private propertyGetting =
   CompilerComponent.create <| fun split compiler returnStrategy ->
      function
      // F# Custom exceptions // TODO: refactor?
      | Patterns.PropertyGet(Some(Patterns.Coerce(Patterns.Var var, t)), pi, exprs)
        when FSharpType.IsExceptionRepresentation pi.DeclaringType && pi.Name.StartsWith "Data" ->
         [ returnStrategy.Return <| PropertyGet(Reference var, pi.Name) ]

      // Implement literals directly in code 
      | Patterns.PropertyGet(None, pi, [])
        when pi.PropertyType.IsPrimitive || pi.PropertyType.IsEnum || pi.PropertyType = typeof<string> ->
        try
            let v, t = pi.GetValue(null), pi.PropertyType
            let expr =
                // TODO: Test if this works for TypeScript enumerations
                if t.IsEnum then JSExpr.Integer(unbox v)
                elif t = typeof<string> || t = typeof<char> then JSExpr.String(string v)
                elif t = typeof<bool> then JSExpr.Boolean(unbox v)
                elif t = typeof<int> then JSExpr.Integer(unbox v)
                elif t = typeof<float> then JSExpr.Number(unbox v)
                elif t = typeof<single> then JSExpr.Number(unbox v)

                elif t = typeof<byte> then JSExpr.Integer(unbox v)
                elif t = typeof<sbyte> then JSExpr.Integer(unbox v)
                elif t = typeof<int16> then JSExpr.Integer(unbox v)

                elif t = typeof<uint16> then JSExpr.Number(unbox v)
                elif t = typeof<uint32> then JSExpr.Number(unbox v)
                elif t = typeof<int64> then JSExpr.Number(unbox v)
                elif t = typeof<uint64> then JSExpr.Number(unbox v)

                else failwithf "%s is not recognized as a primitive type" t.Name
            [ returnStrategy.Return expr ]
        with
        | _ -> createCall split returnStrategy compiler [] (pi.GetGetMethod(true))
      | Patterns.PropertyGet(List objExpr, pi, exprs) ->
         // TODO: Test module let bounds and member val x = 5 with get, set still work properly
        createCall split returnStrategy compiler [objExpr; exprs] (pi.GetGetMethod(true))
      | _ -> []
      
let private propertySetting =
   CompilerComponent.create <| fun (|Split|) compiler returnStrategy ->
      function
      | Patterns.PropertySet(None, pi, _, _) ->
        raise <| Exceptions.StaticMutableProperty pi
      | Patterns.PropertySet(List objExpr, pi, exprs, valExpr) ->
        createCall (|Split|) returnStrategy compiler [objExpr; exprs; [valExpr]] (pi.GetSetMethod(true))
      | _ -> []

let private fieldGetting =
   CompilerComponent.create <| fun (|Split|) _ returnStrategy ->
      function
      | Patterns.FieldGet(None, fi) ->
         raise <| Exceptions.StaticField fi
      | Patterns.FieldGet(Some(Split(objDecl, objRef)), fi) ->
         [ yield! objDecl
           yield returnStrategy.Return <| PropertyGet(objRef, JavaScriptNameMapper.sanitizeAux fi.Name) ]
      | _ -> []

let private fieldSetting =
   CompilerComponent.create <| fun (|Split|) _ returnStrategy ->
      function
      | Patterns.FieldSet(None, fi, _) ->
         raise <| Exceptions.StaticField fi
      | Patterns.FieldSet(Some(Split(objDecl, objRef)), fi, Split(valDecl, valRef)) ->
         [ yield! objDecl
           yield! valDecl
           yield Assign(PropertyGet(objRef, JavaScriptNameMapper.sanitizeAux fi.Name), valRef) ]
      | _ -> []

let private constructingInstances =
   CompilerComponent.create <| fun split compiler returnStrategy ->
      function
      | PatternsExt.NewObject(ci, exprs) ->
         let isEmptyObj = ci.DeclaringType.GUID = typeof<obj>.GUID &&
                          ci.DeclaringType.FullName = typeof<obj>.FullName
         match returnStrategy with
         // All constructors call new obj(), emit nothing
         | InPlace when isEmptyObj -> [ Scope <| Block [] ]
         // A constructor within another means inheritance, throw exception
         | InPlace -> raise <| Exceptions.Inheritance ci.DeclaringType
         | _ when isEmptyObj -> [ returnStrategy.Return <| JSExpr.Object [] ]
         | _ -> createConstruction split returnStrategy compiler [exprs] ci
      // Creating instances of generic types with parameterless constructors (e.g. new T'())
      | Patterns.Call(None, mi, []) when mi.Name = "CreateInstance" && mi.IsGenericMethod ->
         let t = mi.GetGenericArguments().[0]
         let ci = t.GetConstructor([||])
         createConstruction split returnStrategy compiler [] ci
      | _ -> []

let components = [ 
   jsEmitInlineMethodCalling
   methodCalling
   propertyGetting
   propertySetting
   constructingInstances
   fieldGetting
   fieldSetting
]