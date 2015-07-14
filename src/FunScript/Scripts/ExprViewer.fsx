#I "../../../../FSharp.Compiler.Service/bin/v4.5/"

#r "FSharp.Compiler.Service.dll"
open System
open System.IO
open Microsoft.FSharp.Compiler.SourceCodeServices

// Create an interactive checker instance 
let checker = FSharpChecker.Create(keepAssemblyContents=true)

let parseAndCheckSingleFile (input) = 
    let file = Path.ChangeExtension(System.IO.Path.GetTempFileName(), "fsx")  
    File.WriteAllText(file, input)
    // Get context representing a stand-alone (script) file
    let projOptions = 
        checker.GetProjectOptionsFromScript(file, input)
        |> Async.RunSynchronously

    checker.ParseAndCheckProject(projOptions) 
    |> Async.RunSynchronously

let rec printArgs args =
  args |> Seq.map printVal |> String.concat ", "

and printVal = function
  | BasicPatterns.Const(o, _) -> string o
  | BasicPatterns.Value(v) -> v.CompiledName
  | BasicPatterns.ThisValue _ -> "this"
  | BasicPatterns.Coerce(t,v) -> sprintf "(%s)%s" t.TypeDefinition.DisplayName (printVal v)
  | BasicPatterns.NewObject(ctor,_,args) -> sprintf "new %s(%s)" ctor.EnclosingEntity.DisplayName (printArgs args)
  | BasicPatterns.NewRecord(t,args) -> sprintf "new %s(%s)" t.TypeDefinition.DisplayName  (printArgs args)
  | BasicPatterns.NewUnionCase(t,uci,args) -> sprintf "new %s(%s)" t.TypeDefinition.DisplayName  (printArgs args)
  | BasicPatterns.UnionCaseGet(v,t,uci,fi) -> sprintf "%s.%s" (printVal v) fi.Name
  | BasicPatterns.FSharpFieldGet(v,t,fi) -> sprintf "%s%s" (match v with Some v -> (printVal v) + "." | None -> "") fi.Name
  | BasicPatterns.UnionCaseTag(v,t) -> printVal v
  | BasicPatterns.Call(None, meth, _, _, [lhs;rhs]) when meth.CompiledName = "op_PipeRight" -> sprintf "%s |> %s" (printVal lhs) (printVal rhs)
  | BasicPatterns.Call(None, meth, _, _, [lhs;rhs]) when meth.CompiledName = "op_PipeLeft" -> sprintf "%s <| %s" (printVal lhs) (printVal rhs)
  | BasicPatterns.Call(objExprOpt, memberOrFunc, typeArgs1, typeArgs2, args) ->
      match objExprOpt with
      | Some (BasicPatterns.Value(v)) -> sprintf "%s.%s(%s)" v.CompiledName memberOrFunc.CompiledName (printArgs args)
      | _ -> sprintf "%s(%s)" memberOrFunc.CompiledName (printArgs args)
  | BasicPatterns.Lambda(v, expr) -> sprintf "%s -> %s" v.CompiledName (printVal expr)
  | BasicPatterns.Let((bindingVar, bindingExpr), bodyExpr) -> sprintf "LET %s = %s IN %s" bindingVar.DisplayName (printVal bindingExpr) (printVal bodyExpr)
  | BasicPatterns.ILAsm(s,_,args) -> sprintf "ILAsm(%s)"(printArgs args)
  | _ as x -> string x

let rec visit (e:FSharpExpr) preffix = 
    match e with
    | BasicPatterns.Call(None, meth, _, _, [lhs;rhs]) when meth.CompiledName = "op_PipeRight" ->
        visit lhs preffix; printfn "%s|>" preffix; visit rhs preffix
    | BasicPatterns.Call(None, meth, _, _, [lhs;rhs]) when meth.CompiledName = "op_PipeLeft" ->
        visit lhs preffix; printfn "%s<|" preffix; visit rhs preffix
    | BasicPatterns.ILAsm(s,_,args) ->
      printfn "%sILAsm(%s)" preffix (printArgs args)
    | BasicPatterns.TypeTest(t,v) ->
      printfn "%s%s IS %s" preffix (printVal v) t.TypeDefinition.DisplayName
    | BasicPatterns.UnionCaseTest(v,t,uci) ->
      printfn "%s%s IS %s" preffix (printVal v) uci.Name
    | BasicPatterns.Coerce(t,v) ->
      printfn "%s(%s)%s" preffix t.TypeDefinition.DisplayName (printVal v)
    | BasicPatterns.Application(f, typs, args) ->
      printfn "%sAPPLY" preffix
      printfn "%s%s" (preffix + "  ") (printArgs args)
      printfn "%sTO" preffix
      printfn "%s%s" (preffix + "  ") (printVal f) 
    // GET
    | BasicPatterns.Call(objExprOpt, f, _, _, [BasicPatterns.Const(:? unit,_)]) when f.IsPropertyGetterMethod ->
      match objExprOpt with
      | Some (BasicPatterns.Value(v)) -> printfn "%s%s.%s" preffix v.CompiledName f.DisplayName
      | _ -> printfn "%s%s.%s" preffix f.EnclosingEntity.FullName f.DisplayName
    | BasicPatterns.Call(objExprOpt, memberOrFunc, typeArgs1, typeArgs2, args) ->
      match objExprOpt with
      | Some v -> printfn "%s%s.%s(%s)" preffix (printVal v) memberOrFunc.CompiledName (printArgs args)
      | _ -> printfn "%s%s(%s)" preffix memberOrFunc.FullName (printArgs args)
    | BasicPatterns.IfThenElse (guardExpr, thenExpr, elseExpr) ->
        printfn "%sIF" preffix
        visit guardExpr (preffix + "  ")
        printfn "%sTHEN" preffix
        visit thenExpr (preffix + "  ")
        printfn "%sELSE" preffix
        visit elseExpr (preffix + "  ")
    | BasicPatterns.TryWith (tryExpr, _, _, withVar, withExpr) ->
        printfn "%sTRY" preffix
        visit tryExpr (preffix + "  ")
        printfn "%sWITH %s" preffix withVar.CompiledName
        visit withExpr (preffix + "  ")
    | BasicPatterns.Let((bindingVar, bindingExpr), bodyExpr) ->
        printfn "%sLET %s = %s" preffix bindingVar.CompiledName (printVal bindingExpr)
        visit bodyExpr (preffix + "  ")
    | BasicPatterns.ValueSet(var, v) ->
        printfn "%s%s <- %s" preffix var.CompiledName (printVal v)
    | BasicPatterns.DecisionTree(decisionExpr, decisionTargets) ->
        printfn "%sDECISION TREE" preffix
        visit decisionExpr (preffix + "  ")
        decisionTargets |> List.iteri (fun i (targets, e) ->
          printfn "%sBRANCH %i [%s]" preffix i (targets |> Seq.map (fun x -> x.CompiledName) |> String.concat ", ")
          visit e (preffix + "  "))
    | BasicPatterns.DecisionTreeSuccess (decisionTargetIdx, decisionTargetExprs) ->
        printfn "%sGOTO BRANCH %i [%s]" preffix decisionTargetIdx (printArgs decisionTargetExprs)
    | BasicPatterns.Const(o, t) ->
        printfn "%sCONST %O" preffix o //t.AbbreviatedType.TypeDefinition.FullName
    | BasicPatterns.Value(v) ->
//        printfn "%sVAL %s" preffix v.FullType.TypeDefinition.DisplayName
        printfn "%sVAL %s" preffix v.CompiledName
    | BasicPatterns.Sequential(expr1, expr2) ->
        printfn "%sSEQ 1" preffix
        visit expr1 (preffix + "  ")
        printfn "%sSEQ 2" preffix
        visit expr2 (preffix + "  ")
    | BasicPatterns.NewObject(ctor,_,args) ->
        printfn "%snew %s(%s)" preffix ctor.EnclosingEntity.DisplayName (printArgs args)
    | BasicPatterns.NewRecord(t,args) ->
        printfn "%snew %s(%s)" preffix t.TypeDefinition.DisplayName  (printArgs args)
    | BasicPatterns.NewUnionCase(t,uci,args) ->
        printfn "%snew %s(%s)" preffix t.TypeDefinition.DisplayName  (printArgs args)
    | BasicPatterns.Lambda(v, expr) -> printfn "%s%s -> %s" preffix v.CompiledName (printVal expr)
    | _ -> failwith (sprintf "unrecognized %+A" e)

let checkProject input =
  let checkProjectResults = parseAndCheckSingleFile input
  if checkProjectResults.HasCriticalErrors then
    failwithf "ERROR: %A" checkProjectResults.Errors
  checkProjectResults

let getExpr exprIndex input = 
  let checkProjectResults = parseAndCheckSingleFile input
//  checkProjectResults.GetAllUsesOfAllSymbols() |> Async.RunSynchronously
//  |> Seq.iter (fun s -> printfn "Symbol %A" s)

//  checkProjectResults.ProjectContext.GetReferencedAssemblies()
//  |> Seq.iter (fun x ->
//    printfn "Assembly %A" x.SimpleName
//    x.Contents.Entities
//    |> Seq.iter (fun e ->
//      try
//        ignore e.AbbreviatedType.TypeDefinition.FullName
//      with
//      | _ -> printfn "%A is not abbreviated" e))
  if checkProjectResults.HasCriticalErrors then
    failwithf "ERROR: %A" checkProjectResults.Errors
//  for ent in checkProjectResults.AssemblySignature.Entities do
//    for sub in ent.NestedEntities do
//      printfn "%A" sub
  let checkedFile = checkProjectResults.AssemblyContents.ImplementationFiles.[0]
  let myLibraryEntity, myLibraryDecls =    
      match checkedFile.Declarations.[0] with 
      | FSharpImplementationFileDeclaration.Entity (e, subDecls) -> (e, subDecls)
      | _ -> failwith "unexpected"
  match myLibraryDecls.[exprIndex] with 
  | FSharpImplementationFileDeclaration.MemberOrFunctionOrValue(m,_, e) -> e
  | FSharpImplementationFileDeclaration.InitAction e -> e
  | FSharpImplementationFileDeclaration.Entity(e,_) -> failwithf "Entity detected: %A" e.TryFullName

let viewExpr exprIndex input =
  visit (getExpr exprIndex input) ""

let input = """
namespace MyNs

type A() =
  let a = 1

//  module MyNestedMod =
//    let a = 1
//
//type A(i:int) =
//    member x.Value = i
//
//type B(i:int) as b =
//    inherit A(i*2)
//    let a = b.Overload(i)
//    member x.Overload() = a
//    member x.Overload(y: int) = y + y
//    member x.BaseValue = base.Value
//
//let [<Literal>] lit = 1.0
//let notLit = 1.0
//let callToOverload = B(5).Overload(4)
"""
let pr = checkProject input
pr.GetAllUsesOfAllSymbols()
|> Async.RunSynchronously
|> Seq.iter (fun s -> printfn "%s" s.Symbol.DisplayName)
let ms = pr.AssemblySignature.Entities.[0].NestedEntities.[0].MembersFunctionsAndValues
ms |> Seq.iter (fun m -> printfn "%O" m)

let (=>) a b = a, b
let obj' (xs: #seq<'a*'b>) =
  let di = System.Collections.Generic.Dictionary<'a,'b>()
  for x in xs do
    if di.ContainsKey (fst x) then
      di.[fst x] <- snd x
    else
    di.Add(fst x, snd x)
  box di

let (?) (o: obj) (k: string): obj = failwith "never"
let (?<-) (o: obj) (k: string) (v: obj): unit = failwith "never"
let ($) (o: obj) (args: obj) = failwith "never"

let di: obj = obj() // [ms.[1] => 1; ms.[2] => 2]
di?hola <- "hola"
di?hola$()


//di.Add(ms.[0], 0)
//di.Values |> Seq.head
//di.Count

let rec typeabbr (t: FSharpType) =
  if t.IsAbbreviation then typeabbr t.AbbreviatedType else t

let eq (t1: FSharpType) (t2: FSharpType) =
  t1 = t2 || typeabbr t1 = typeabbr t2

let rec typedef (t: FSharpType) =
  if t.IsAbbreviation then typedef t.AbbreviatedType else t.TypeDefinition

let findMethod (ent: FSharpEntity) (methName: string) (argTypes: FSharpType list) =
  ent.MembersFunctionsAndValues |> Seq.tryFind (fun m ->
    if m.CompiledName = methName then
      let pars = m.CurriedParameterGroups |> Seq.concat |> Seq.map (fun x -> x.Type) |> Seq.toList
      List.forall2 eq pars argTypes
    else
      false)

getExpr 4 input
|> function
    | BasicPatterns.ThisValue t -> ()
    | BasicPatterns.Let((x, _),_)
    | BasicPatterns.FSharpFieldGet(Some(BasicPatterns.Value x),_,_)
    | BasicPatterns.Call(_,_,_,_,[BasicPatterns.Value x;_]) ->
        printfn "IsBaseValue %b IsConstructorThisValue %b x.IsMemberThisValue %b LiteralValue %A"
                x.IsBaseValue x.IsConstructorThisValue x.IsMemberThisValue x.LiteralValue
//   | BasicPatterns.Application(BasicPatterns.Lambda(_, Delay asyncBuilder expr),_,[BasicPatterns.Value b1]) -> printfn "GOTCHA %A" expr
//   | BasicPatterns.Application(_,_,[arg]) -> printfn "%s" arg.Type.TypeDefinition.FullName
//   | BasicPatterns.Call(_, meth, typeGenArgs, genArgs, args) ->
//      findMethod meth.EnclosingEntity meth.CompiledName (args |> List.map (fun a -> a.Type))
    | BasicPatterns.Const(_, typ) -> printfn "%s" typ.AbbreviatedType.TypeDefinition.FullName
    | BasicPatterns.Lambda(v1, BasicPatterns.Call(None, meth, _, _, [BasicPatterns.Value v1'])) when v1 = v1' -> printfn "Wrapped Method"
    | BasicPatterns.Lambda(v, delegateBodyExpr) -> printfn "%A" v.FullType
    | BasicPatterns.ObjectExpr(objType, BasicPatterns.Call(_,ctor,_,_,_), overrides, interfaceImplementations) -> printfn "%A" ctor.EnclosingEntity.FullName
    | _ as e -> printfn "%s" e.Type.AbbreviatedType.TypeDefinition.FullName


type P(x: int, y: int) =
  member __.X = x
  interface System.IEquatable<P> with
    member __.Equals(p: P) = x = p.X

P(5,6).Equals()


