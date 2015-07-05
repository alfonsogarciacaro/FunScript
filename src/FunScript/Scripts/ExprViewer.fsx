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
      | Some (BasicPatterns.Value(v)) -> printfn "%s%s.%s(%s)" preffix v.CompiledName memberOrFunc.CompiledName (printArgs args)
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
    | BasicPatterns.Const(o, _) ->
        printfn "%sCONST %O" preffix o
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

let getExpr exprIndex input = 
  let checkProjectResults = parseAndCheckSingleFile input
  if checkProjectResults.Errors.Length > 0 then
    failwithf "ERROR: %A" checkProjectResults.Errors
  let checkedFile = checkProjectResults.AssemblyContents.ImplementationFiles.[0]
  let myLibraryEntity, myLibraryDecls =    
      match checkedFile.Declarations.[0] with 
      | FSharpImplementationFileDeclaration.Entity (e, subDecls) -> (e, subDecls)
      | _ -> failwith "unexpected"
  match myLibraryDecls.[exprIndex] with 
  | FSharpImplementationFileDeclaration.MemberOrFunctionOrValue(_,_, e) -> e
  | FSharpImplementationFileDeclaration.InitAction e -> e
  | FSharpImplementationFileDeclaration.Entity(e,_) -> failwithf "Entity detected: %s" e.FullName

let viewExpr exprIndex input =
  visit (getExpr exprIndex input) ""

let input = """
//type I1 =
//   abstract member I1: unit -> int
//
//type I2 =
//   abstract member I2: unit -> int
//
//{
//   new I1 with member __.I1() = 4
//   interface I2 with member __.I2() = 4
//}

module O1 =
   let Sqr x y z = x + y + z

type O2 =
   static member Sqr x y z = x + y + z

O1.Sqr 1 2
"""
viewExpr 3 input

getExpr 2 input
|> function
//   | BasicPatterns.Application(BasicPatterns.Lambda(_, Delay asyncBuilder expr),_,[BasicPatterns.Value b1]) -> printfn "GOTCHA %A" expr
//   | BasicPatterns.Application(_,_,[arg]) -> printfn "%s" arg.Type.TypeDefinition.FullName
   | BasicPatterns.Lambda(v1, BasicPatterns.Call(None, meth, _, _, [BasicPatterns.Value v1'])) when v1 = v1' -> printfn "Wrapped Method"
   | BasicPatterns.Lambda(v, delegateBodyExpr) -> printfn "%A" v.FullType
   | BasicPatterns.ObjectExpr(objType, BasicPatterns.Call(_,ctor,_,_,_), overrides, interfaceImplementations) -> printfn "%A" ctor.EnclosingEntity.FullName
   | _ -> failwith "unexpected"




