module internal FunScript.LambdaApplication

open AST
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

let (|RefExpr|_|) (e: Expr) =
   match e with
   | Var _
   | Value _
   | PropertyGet _   // TODO: Consider property getting as a simple reference? (sometimes it can be expensive)
   | FieldGet _ -> Some e
   | _ -> None

let private replaceVars1 (e: Expr) var1 varRef1 = e.Substitute(fun v ->
   if v = var1 then Some varRef1
   else None)

let private replaceVars2 (e: Expr) var1 varRef1 var2 varRef2 = e.Substitute(fun v ->
   if v = var1 then Some varRef1
   elif v = var2 then Some varRef2
   else None)

let private replaceVars3 (e: Expr) var1 varRef1 var2 varRef2 var3 varRef3 = e.Substitute(fun v ->
   if v = var1 then Some varRef1
   elif v = var2 then Some varRef2
   elif v = var3 then Some varRef3
   else None)

let private replaceVars4 (e: Expr) var1 varRef1 var2 varRef2 var3 varRef3 var4 varRef4 = e.Substitute(fun v ->
   if v = var1 then Some varRef1
   elif v = var2 then Some varRef2
   elif v = var3 then Some varRef3
   elif v = var4 then Some varRef4
   else None)

let private application =
   CompilerComponent.create <| fun (|Split|) compiler returnStrategy ->
      function
      | Application(Lambda(var1, body), RefExpr varRef1) ->
        compiler.Compile returnStrategy (replaceVars1 body var1 varRef1)

      | Application(Application(Lambda(var1, Lambda(var2, body)),
                                RefExpr varRef1),
                    RefExpr varRef2) ->
        compiler.Compile returnStrategy (replaceVars2 body var1 varRef1 var2 varRef2)

      | Application(Application(Application(Lambda(var1, Lambda(var2, Lambda(var3, body))),
                                            RefExpr varRef1),
                                RefExpr varRef2),
                    RefExpr varRef3) ->
        compiler.Compile returnStrategy (replaceVars3 body var1 varRef1 var2 varRef2 var3 varRef3)

      | Application(Application(Application(Application(Lambda(var1, Lambda(var2, Lambda(var3, Lambda(var4, body)))),
                                                        RefExpr varRef1),
                                            RefExpr varRef2),
                                RefExpr varRef3),
                    RefExpr varRef4) ->
        compiler.Compile returnStrategy (replaceVars4 body var1 varRef1 var2 varRef2 var3 varRef3 var4 varRef4)

      | Application(Let(letVar1, letVarRef1, Lambda(var1, body)), RefExpr varRef1) ->
        compiler.Compile returnStrategy <| Expr.Let(letVar1, letVarRef1, (replaceVars1 body var1 varRef1))

      | Application(Let(letVar1, letVarRef1,
                        Let(letVar2, letVarRef2, Lambda(var1, body))), RefExpr varRef1) ->
        compiler.Compile returnStrategy <| Expr.Let(letVar1, letVarRef1,
                                                    Expr.Let(letVar2, letVarRef2, (replaceVars1 body var1 varRef1)))

      | Application(Let(letVar1, letVarRef1,
                        Let(letVar2, letVarRef2,
                            Let(letVar3, letVarRef3, Lambda(var1, body)))), RefExpr varRef1) ->
        compiler.Compile returnStrategy <| Expr.Let(letVar1, letVarRef1,
                                                    Expr.Let(letVar2, letVarRef2,
                                                             Expr.Let(letVar3, letVarRef3, (replaceVars1 body var1 varRef1))))

      | Patterns.Application(Split(lambdaDecl, lambdaRef), Split(argDecl, argRef)) ->
         [ yield! lambdaDecl
           yield! argDecl
           yield returnStrategy.Return <| Apply(lambdaRef, [argRef])
         ]

      | Patterns.Call(Some (Split(delDecl, delRef)), mi, argExprs) 
        when typeof<System.Delegate>.IsAssignableFrom mi.DeclaringType ->
         let argDecls, argRefs =
            Reflection.getDeclarationAndReferences (|Split|) argExprs
         [ yield! delDecl
           yield! argDecls |> List.concat
           yield returnStrategy.Return <| Apply(delRef, argRefs)
         ]
      | _ -> []

let private definition =
   CompilerComponent.create <| fun (|Split|) compiler returnStrategy ->
      let (|Return|) = compiler.Compile
      function
      | Patterns.Lambda(var, expr) ->
         let block = compiler.Compile ReturnStrategies.returnFrom expr
         [ yield returnStrategy.Return <| Lambda(None, [var], Block block) ]
      | Patterns.NewDelegate(_, vars, expr) ->
         let block = compiler.Compile ReturnStrategies.returnFrom expr
         [ yield returnStrategy.Return <| Lambda(None, vars, Block block) ]
      | _ -> []

let components = [ 
   application
   definition
]