module FunScript.Playbox

open AST
open System
open System.IO
open System.Collections.Generic
open Microsoft.FSharp.Compiler.SourceCodeServices

let makeScope (parent: IScopeInfo option) (ret: ReturnStrategy) (vals: string list): IScopeInfo = failwith "Not implemented"
let valScope = makeScope None NoReturn []
let funScope (args: FSRef list list) =
  args |> List.concat |> List.map (fun a -> a.CompiledName) |> makeScope None Return

let isClassMember (classEnt: FSharpEntity) = function
  | FSDeclaration.MemberOrFunctionOrValue(m,_,_) -> m.EnclosingEntity = classEnt
  | _ -> false

let getModuleName (modEnt: FSharpEntity) =
  modEnt.QualifiedName.Replace(".", "/").Replace("+", ".")

let rec compileDeclarations (com: ICompiler) acc = function
  | [] -> acc
  | decl::restDecls ->
    let modMember, restDecls =
      match decl with
      | MemberOrFunctionOrValue(v, [], body) ->
        ESValue(v, com.CompileExpr valScope body), restDecls

      | MemberOrFunctionOrValue(f, args, body) ->
        ESFunction(f, com.CompileStatement (funScope args) body), restDecls

      | InitAction e ->
        ESInitAction(com.CompileStatement valScope e), restDecls

      | Entity(ent, subDecls) ->
        if ent.IsFSharpModule then
          ESNestedModule(com.CompileModule (getModuleName ent) subDecls), restDecls
        else
          assert subDecls.IsEmpty
          let memberDecls, restDecls = List.partition (isClassMember ent) restDecls
          ESNestedClass(com.CompileClass ent memberDecls), restDecls

    compileDeclarations com (modMember::acc) restDecls

let rec getRootModules (com: ICompiler) (decls: FSDeclaration list) = seq {
  match decls with
  | [] -> ()
  | decl::restDecls ->
    match decl with
    | Entity(ent, subDecls) ->
      if ent.IsNamespace then
        yield! getRootModules com subDecls
        yield! getRootModules com restDecls
      else
        com.ResetImportsCache()
        let modDecls, restDecls =
          if ent.IsFSharpModule then
            subDecls, restDecls
          else
            assert subDecls.IsEmpty
            let memberDecls, restDecls =
              List.partition (isClassMember ent) restDecls
            decl::memberDecls, restDecls
        yield com.CompileModule (getModuleName ent) modDecls
        yield! getRootModules com restDecls
    | _ -> failwith "unexpected"
}

let parseFSharpScript (checker: FSharpChecker) file = 
    let projOptions = 
        checker.GetProjectOptionsFromScript(file, File.ReadAllText file)
        |> Async.RunSynchronously
    checker.ParseAndCheckProject(projOptions) 
    |> Async.RunSynchronously

let compileFSharpScript file = 
  let com: ICompiler = failwith "Not implemented"          // TODO
  let checker = FSharpChecker.Create(keepAssemblyContents=true) // TODO: Recycle checker (check also load times)
  let checkProjectResults = parseFSharpScript checker file
  if checkProjectResults.HasCriticalErrors then
    failwithf "The script contains critical errors %A" checkProjectResults.Errors
  for file in checkProjectResults.AssemblyContents.ImplementationFiles do
    getRootModules com file.Declarations
    |> Seq.iter (fun _ -> failwith "Not implemented") // TODO: Print
