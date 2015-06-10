namespace FunScript

open System.IO
open Microsoft.FSharp.Compiler.SourceCodeServices

/// Documentation for my library
/// ## Example
///     let h = Library.hello 1
///     printfn "%d" h
module Library =

  type JSClass(e: FSharpEntity) =
    member __.Name = e.CompiledName
    override x.ToString() = x.Name

  type JSModule(e: FSharpEntity) =
    let classes = ResizeArray<JSClass>()
    member __.Path = e.FullName.Replace(".", "/")
    member __.AddClass(e: FSharpEntity) = classes.Add(JSClass e)
    override x.ToString() =
      sprintf "%s(%s)" x.Path (classes |> Seq.map string |> String.concat ",")

  // Active Patterns
  let (|Namespace|Module|Type|) (e: FSharpEntity) =
    if e.IsNamespace then Namespace e
    elif e.IsFSharpModule then Module e
    else Type e

  /// Returns the check project results
  /// ## Parameters
  ///  - `scriptFile` - path to the script file (.fsx) acting as the entry point for the application
  let parseAndCheckScript (scriptFile) =
    if (Path.GetExtension scriptFile).ToLower() <> ".fsx" then
      failwith "Only F# script files (.fsx) are supported"
    let checker = FSharpChecker.Create(keepAssemblyContents=true)
    let input = File.ReadAllText(scriptFile)
    // Get context representing a stand-alone (script) file
    let checkProjectResults =
      let projOptions = 
          checker.GetProjectOptionsFromScript(scriptFile, input)
          |> Async.RunSynchronously
      checker.ParseAndCheckProject(projOptions) 
      |> Async.RunSynchronously
    if checkProjectResults.Errors.Length = 0
    then checkProjectResults
    else failwithf "%A" checkProjectResults.Errors

  let rec visitDeclaration acc d = 
    match d with 
    | FSharpImplementationFileDeclaration.Entity (e, subDecls) -> 
        let acc = match e with
                  | Namespace _ -> acc
                  | Module e -> (JSModule e)::acc
                  | Type e -> match acc with
                              | [] -> failwith "Not implemented"
                              | x::_ -> x.AddClass(e); acc
        subDecls |> Seq.fold (fun acc subDecl ->
          visitDeclaration acc subDecl) acc
    | FSharpImplementationFileDeclaration.MemberOrFunctionOrValue (v,_,_) ->
        printfn "Member %s (%A)" v.FullName v.Accessibility; acc
    | FSharpImplementationFileDeclaration.InitAction _ -> acc
        
  let visitFiles (checkProjectResults: FSharpCheckProjectResults) =
    checkProjectResults.AssemblyContents.ImplementationFiles
    |> Seq.fold (fun acc file ->
      file.Declarations
      |> Seq.fold (fun acc decl ->
        visitDeclaration acc decl) acc) []
 