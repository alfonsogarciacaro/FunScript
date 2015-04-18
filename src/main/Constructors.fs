module internal FunScript.Constructors

open AST
open System.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Reflection

let private foldBack state f source =
   let rec foldBacki i state f source =
      if i < 0 then state
      else foldBacki (i-1) (f i state <| Array.get source i) f source
   foldBacki (Array.length source - 1) state f source

let private genConstructor (com: ICompiler) (t: System.Type) (pis: PropertyInfo[]) =
   if pis.Length = 0
   then Assign(DebugInfo.Empty, com.RefType t, Lambda([], Empty))
   else 
      let this = Var("this", typeof<obj>)
      let vars = pis |> Array.mapi (fun i _ -> Var(sprintf "a%i" i, typeof<obj>))
      let assign i = Assign(DebugInfo.Empty,
                        PropertyGet(JSExpr.Var this, String pis.[i].Name),
                        JSExpr.Var vars.[i])
      pis
      |> foldBack Empty (fun i state _ -> Sequential(assign i, state))
      |> fun assignments -> Assign(DebugInfo.Empty, com.RefType t, Lambda(List.ofArray vars, assignments))

let compileUciConstructor (uci: UnionCaseInfo) (typeRef: JSExpr) (uciRef: JSExpr) =
   let this, pis, di = Var("_this", typeof<obj>), uci.GetFields(), DebugInfo.Empty
   Assign(DebugInfo.Empty, uciRef,
      if pis.Length = 0 then
         Lambda([],
            Let(di, this, New(typeRef, []),
               Sequential(Assign(di, PropertyGet(JSExpr.Var this, String "Tag"), Integer uci.Tag),
                  Return(di, JSExpr.Var this))))
      else
         let vars = pis |> Array.mapi (fun i _ -> Var(sprintf "a%i" i, typeof<obj>))
         let assign i = Assign(di, PropertyGet(JSExpr.Var this, String pis.[i].Name), JSExpr.Var vars.[i])
         pis
         |> foldBack (Return(di, JSExpr.Var this)) (fun i state _ -> Sequential(assign i, state))
         |> fun assignments ->
            Lambda(List.ofArray vars,
               Let(di, this, New(typeRef, []),
                  Sequential(Assign(di, PropertyGet(JSExpr.Var this, String "Tag"), Integer uci.Tag), assignments))))

let private compileObjectConstructor (com: ICompiler) (ci: ConstructorInfo) =
   let tref, di = com.RefType ci.DeclaringType, DebugInfo.Empty
   let proto =
      match ci.DeclaringType.TryGetAttribute<JSPrototypeAttribute>() with
      | Some att -> Some att.Prototype
      | None -> None
   let cons =
      match Expr.TryGetReflectedDefinition ci with
      | Some e ->
         match e with
         | DerivedPatterns.Lambdas(vars, CompileStatement com Inplace body) ->
            let body =
               match proto with
               | Some _ -> Sequential(Do(di, EmitExpr("this.constructor={0}", [tref])), body)
               | None -> body
            let vars = vars |> List.concat |> List.filter (fun v -> v.Type <> typeof<unit>)
            Applications.getMethGenArgs ci              // Receive also generic arguments
            |> List.map (fun a -> Var("$" + a.Name, a))
            |> List.append vars
            |> fun vars -> Lambda(vars, body)
         | _ ->
            failwithf "Please report: DerivedPatterns.Lambdas doesn't match %s constructor"
                        ci.DeclaringType.Name
      | None -> 
         failwithf "Type %s is not tagged with JS attribute" ci.DeclaringType.Name
   match proto with
   | None -> Assign(di, tref, cons)
   | Some proto -> Sequential(Assign(di, tref, cons),
                              Assign(di, PropertyGet(tref, String "prototype"),
                                           EmitExpr(proto, [])))

let compileConstructor (com: ICompiler) t =
   let tref, di = com.RefType t, DebugInfo.Empty
   let cons =
      if   FSharpType.IsUnion(t, true) then
         Assign(di, tref, Lambda([], Empty))
      elif FSharpType.IsRecord(t, true) then
         genConstructor com t (FSharpType.GetRecordFields(t, true))
      elif FSharpType.IsExceptionRepresentation(t, true) then
         genConstructor com t (FSharpType.GetExceptionFields(t, true))
      else
         match t.GetConstructors(BindingFlags.All) with
         | [||] -> Assign(di, tref, Object []) // Static types / Modules
         | cis -> compileObjectConstructor com cis.[0]
   
   // Static properties in modules. TODO: Use BindingFlags.All? Properties should all be public here.
   if FSharpType.IsModule t
   then t.GetProperties() |> Array.fold (fun (cons: JSStatement) pi ->
      match Expr.TryGetReflectedDefinition pi.GetMethod with
      | Some piExpr ->
         let piExpr =
            match piExpr with
            | Patterns.Value _ -> com.CompileExpr piExpr
            | _ -> Lambda([], Return(piExpr.DebugInfo, com.CompileExpr piExpr))
         Sequential(cons, Assign(di, PropertyGet(tref, String pi.Name), piExpr))
      | None -> cons (* failwithf "No reflected definition for %s.%s" t.Name pi.Name *)) cons
   else cons

let private constructInstance com ret = function
   // TODO: Use 'undefined' instead of 'null' for None? It'd make more sense for optional parameters.
   // (Update JSEmit expressions in FunScript.Core.Option if this changes)
   | Patterns.NewUnionCase (uci, args) when uci.DeclaringType.Name = typeof<obj option>.Name ->
      buildExpr <| match args with [CompileExpr com arg] -> arg | _ -> Null

   | Patterns.NewUnionCase (uci, args) ->
      buildExpr <| Apply(com.RefCase uci, List.map com.CompileExpr args)
   
   // TODO: Check if the problem with quotations generated by type providers is still happening (see PatternsExt)
   | PatternsExt.NewRecord (t, args) ->
      buildExpr <| New(com.RefType t, List.map com.CompileExpr args)

   // Creating instances of generic types with parameterless constructors (e.g. new T'())
   | Patterns.Call(None, mi, []) when mi.Name = "CreateInstance" && mi.IsGenericMethod ->
      let t = mi.GetGenericArguments().[0]
      buildExpr <| New(com.RefType t, [])

   | PatternsExt.NewObject (ci, args) ->
      let genCi = Applications.getGenMethodDef ci :?> ConstructorInfo
      let args = List.map com.CompileExpr args
      match ret with
      | Inplace ->
         buildStatement <|
            // Ignore primary constructors calling obj or System.Exception as base
            if genCi.DeclaringType = typeof<obj> || genCi.DeclaringType = typeof<System.Exception>
            then Empty
            else Do(DebugInfo.Empty,
                  Apply(PropertyGet(com.RefType genCi.DeclaringType, String "call"),
                        EmitExpr("this",[])::args))
      | _ ->
         buildExpr <|
            if genCi.DeclaringType = typeof<obj> then
               Object []
            else
               let genArgs = Applications.getMethGenArgs ci |> List.map com.RefType
               if genCi = genCi.DeclaringType.GetConstructors(BindingFlags.All).[0] then  // Primary constructor
                  New(com.RefType genCi.DeclaringType, args@genArgs)
               else
                  Apply(com.RefMethod(genCi, None), args@genArgs)
   | _ -> None

let components: CompilerComponent list = [
   constructInstance
]
