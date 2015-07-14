module FunScript.Calls

open AST
open System.Text.RegularExpressions
open Microsoft.FSharp.Compiler.SourceCodeServices

let rec typeabbr (t: FSharpType) =
  if t.IsAbbreviation then typeabbr t.AbbreviatedType else t

let eq (t1: FSharpType) (t2: FSharpType) =
  t1 = t2 || typeabbr t1 = typeabbr t2

let rec typedef (t: FSharpType) =
  if t.IsAbbreviation then typedef t.AbbreviatedType else t.TypeDefinition

let (==) (t1: FSharpEntity) (t2: System.Type) = 
  t1.FullName = t2.FullName

let eq<'T> (t: FSharpType) =
  (typedef t) == typeof<'T>

let preventConflicts exists str =
   let rec check n =
      let name = if n > 0 then sprintf "%s%i" str n else str
      if not (exists name)
      then name
      else check (n+1)
   check 0

let getClassMapp (com: ICompiler) (ent: FSharpEntity) =
  if not ent.IsClass then
    failwithf "Provided entity is not a class: %s" ent.CompiledName

  let checkInheritedFields (fname: string) =
    let rec checkParentOf (ent: FSharpEntity) =
      match ent.BaseType with
      | Some t when not (eq<obj> t) ->
        let parent = typedef t
        let exists =
          com.GetMappings(parent).Fields.Values
          |> Seq.exists (fun fi -> fi = fname)
        if exists then true else checkParentOf parent
      | _ -> false
    checkParentOf ent
  
  let fieldMappings =
    ent.FSharpFields
    |> Seq.fold (fun (dic: Dictionary<_,_>) fi ->
      dic.Add(fi, preventConflicts checkInheritedFields fi.Name); dic)
      (Dictionary<_,_>())

  // Check there are no clashing method names in implemented interfaces
  let allInterfaceMethods =
    ent.DeclaredInterfaces
    |> Seq.map (fun infc -> (typedef infc).MembersFunctionsAndValues)
    |> Seq.concat
    |> Seq.map (fun m -> m.CompiledName)
    |> Seq.toArray
  allInterfaceMethods.Length = Set(allInterfaceMethods).Count

  

// Private fields
// Properties
// Methods/Overloads
// Events?
// Interfaces
// 
let private binaryOps =
  Map [
    "( + )", "+"
    "( - )", "-"
    "( * )", "*"
    "( / )", "/"
    "( % )", "%"
    "( = )", "==="
    "( <> )", "!=="
    "( >= )", ">="
    "( <= )", "<="
    "( || )", "||"
    "( && )", "&&"
    "( ||| )", "|"
    "( &&& )", "&"
    "( >>> )", ">>"
    "( <<< )", "<<"
  ]

let equalityOps =
  Map [
    "( = )", "==="
    "( <> )", "!=="
  ]

let comparisonOps =
  Map [
    "( >= )", ">="
    "( <= )", "<="
  ]

let isPrimitive =
  let primitiveRefTypes = Set [typeof<string>.FullName; typeof<unit>.FullName]
  fun (t: FSharpType) ->
    let tdef = typedef t
    tdef.IsValueType || Set.contains tdef.FullName primitiveRefTypes

let findMethod (ent: FSharpEntity) (methName: string) (argTypes: FSharpType list) =
  ent.MembersFunctionsAndValues |> Seq.tryFind (fun m ->
    if m.CompiledName = methName then
      let pars = m.CurriedParameterGroups |> Seq.concat |> Seq.map (fun x -> x.Type) |> Seq.toList
      List.forall2 eq pars argTypes
    else
      false)
      
let compileOperator (com: ICompiler) (scope: IScopeInfo)
                    (meth: FSRef) (args: FSharpExpr list)  =
  match args with
  | [lhs; rhs] ->
    let opName = meth.DisplayName
    if isPrimitive lhs.Type then
      let jsOp =
        if Map.containsKey opName binaryOps then binaryOps.[opName]
        elif Map.containsKey opName equalityOps then equalityOps.[opName]
        elif Map.containsKey opName comparisonOps then comparisonOps.[opName]
        else failwithf "Operator %s not supported" opName
      BinaryOp(com.CompileExpr scope lhs, jsOp, com.CompileExpr scope rhs)
    else
      if Map.containsKey opName binaryOps then
        // Get the method of the type with the same method compiled name
        let tdef = typedef lhs.Type
        match findMethod tdef meth.CompiledName [lhs.Type; rhs.Type] with
        | Some originalMeth -> Apply(com.RefMethod None originalMeth, [com.CompileExpr scope lhs; com.CompileExpr scope rhs])
        | None -> failwithf "Method %s not found in type %s" meth.CompiledName tdef.FullName
      elif Map.containsKey opName equalityOps then equalityOps.[opName]
      elif Map.containsKey opName comparisonOps then comparisonOps.[opName]
      else failwithf "Operator %s not supported" opName


//    when binaryOps.ContainsKey meth.DisplayName ->
//    BinaryOp(lhs, operatorEquivalences.[meth.DisplayName], rhs)
  | _ -> failwithf "Operator %s is not supported" meth.DisplayName

  
 
