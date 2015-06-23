#load "load-project.fsx"

open System.IO
open FunScript.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices

let project =
  Path.Combine(__SOURCE_DIRECTORY__, "../../../scripts/test.fsx")
  |> parseAndCheckScript

let kind (ent: FSharpEntity) =
  if ent.IsNamespace then "namespace"
  elif ent.IsFSharpModule then "module"
  else "class"

let rec print prefix (ent: FSharpEntity) printNested =
  printfn "%s%s %s (%s)" prefix ent.AccessPath ent.CompiledName (kind ent)
  if printNested then
    for nested in ent.NestedEntities do
      print (prefix + "  ") nested true

for ent in project.AssemblySignature.Entities do
  print "" ent true

let m = System.Collections.Generic.Dictionary<FSharpEntity, FSharpEntity option>()

let rec addEntity parent ent =
  m.Add(ent, Some parent)
  for nested in ent.NestedEntities do
    addEntity parent nested

for ent in project.AssemblySignature.Entities do
  m.Add(ent, None)
  for nested in ent.NestedEntities do
    addEntity ent nested

for kv in m do
  printfn "%s -> Module: %s" kv.Key.CompiledName (match kv.Value with Some e -> e.CompiledName | None -> "None")

let rec findParent (entities: FSharpEntity seq) (ent: FSharpEntity) =
  entities |> Seq.tryPick (fun e ->
    if ent.AccessPath = e.FullName
    then Some e
    else findParent e.NestedEntities ent)

let rec print' prefix (decl: FSharpImplementationFileDeclaration) =
  match decl with
  | FSharpImplementationFileDeclaration.Entity (e, subDecls) ->
    print prefix e false
//    printfn "%sParent: %A" prefix <| findParent project.AssemblySignature.Entities e
    for subDecl in subDecls do
      print' (prefix + "  ") subDecl
  | _ -> ()

for file in project.AssemblyContents.ImplementationFiles do
  printfn "FILE"
  for decl in file.Declarations do
    print' "" decl
