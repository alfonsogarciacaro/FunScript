module FunScript.Compiler

open System.IO
open System.Collections.Generic
open Microsoft.FSharp.Compiler.SourceCodeServices

type FSRef = FSharpMemberOrFunctionOrValue
type FSMeth = {def: FSRef; args: FSRef list list; body: FSharpExpr}
type FSVal = {def: FSRef; body: FSharpExpr}
type ValDic = Dictionary<string, FSVal>
type MethDic = Dictionary<string, FSMeth>
type OverloadDic = Dictionary<string, ResizeArray<FSMeth>>

let (|IsNamespace|IsModule|IsClass|) (e: FSharpEntity) =
  if e.IsNamespace then IsNamespace
  elif e.IsFSharpModule then IsModule
  else IsClass

let typdef (m: FSRef) =
  let rec typ (t: FSharpType) =
    if not t.IsAbbreviation then t else typ t.AbbreviatedType
  (typ m.FullType).TypeDefinition

let filterArgs (meth: FSRef) (args: FSRef list list) =
  match args with
  | [] -> []
  | [_;[x]] when meth.IsInstanceMember && (typdef x).CompiledName = "Unit" -> []
  | [[x]] when (not meth.IsInstanceMember) && (typdef x).CompiledName = "Unit" -> []
  | x::xs -> List.concat (if meth.IsInstanceMember then xs else args)      

type JSClass(e: FSharpEntity, parent: JSModule) =
  let mutable cons: FSMeth option = None
  let secondaryConstructors = ResizeArray<FSMeth>()
  let getters = MethDic()
  let setters = MethDic()
  let staticMethods = MethDic()
  let instanceMethods = MethDic()
  let staticOverloads = OverloadDic()
  let instanceOverloads = OverloadDic()

  member __.AddMember(def: FSRef, args: FSRef list list, body: FSharpExpr) =
    let name, meth, filteredArgs =
      def.CompiledName, {def=def; args=args; body=body}, filterArgs def args

    let addMeth (dic: MethDic) (odic: OverloadDic) =
      if odic.ContainsKey name then
        odic.[name].Add(meth)
      elif dic.ContainsKey name then
        let oldMeth = dic.[name]
        dic.Remove(name) |> ignore
        odic.Add(name, ResizeArray([oldMeth; meth]))
      else
        dic.Add(name, meth)

    if def.IsImplicitConstructor then
      cons <- Some meth
    elif def.CompiledName = ".ctor" then
      secondaryConstructors.Add(meth)
    elif not def.IsInstanceMember then
      addMeth staticMethods staticOverloads
    elif def.IsPropertyGetterMethod && filteredArgs.Length = 0 then
      getters.Add(def.CompiledName, meth)
    elif def.IsPropertySetterMethod && filteredArgs.Length = 1 then
      setters.Add(def.CompiledName, meth)
    else
      addMeth instanceMethods instanceOverloads

  override __.ToString() = e.FullName

and JSModule(e: FSharpEntity, ?parent: JSModule) =
  let values = ValDic()
  let methods = MethDic()
  let classes = Dictionary<string, JSClass>()
  let nestedModules = Dictionary<string, JSModule>()
  let initActions = ResizeArray<FSharpExpr>()
  let imports = ResizeArray<string>()
  let privSymbols = ResizeArray<string>()

  member __.Depth = match parent with None -> 0 | Some m -> m.Depth + 1
  member __.LastClass = if classes.Count > 0 then Some(Seq.last classes.Values) else None

  member self.AddNested(e: FSharpEntity) =
    if e.IsFSharpModule then
      let md = JSModule(e, self)
      nestedModules.Add(e.CompiledName, md)
      NestedModule md
    else
      let cl = JSClass(e, self)
      classes.Add(e.CompiledName, cl)
      NestedClass cl

  member __.AddMember(def: FSRef, args: FSRef list list, body: FSharpExpr) =
    match args with
    | [] -> values.Add(def.CompiledName, {def=def; body=body})
    | _ -> methods.Add(def.CompiledName, {def=def; args=args; body=body})

  member __.AddInitAction(e: FSharpExpr) =
    initActions.Add(e)

  override __.ToString() = e.FullName

and Nested = NestedClass of JSClass | NestedModule of JSModule

type JSProgram() =
  let modules = Dictionary<string, JSModule>()
  member __.Modules = modules
  member __.AddModule(e: FSharpEntity) =
    let md = JSModule e in modules.Add(e.FullName, md); md
      
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

let rec visitNested (md: JSModule) d =
  match d with 
  | FSharpImplementationFileDeclaration.Entity (e, subDecls) ->
    match md.AddNested e with
    | NestedModule nested -> subDecls |> Seq.iter (fun subDecl -> visitNested nested subDecl)
    | NestedClass _ -> ()
  | FSharpImplementationFileDeclaration.MemberOrFunctionOrValue (def, args, body) ->
    if def.IsModuleValueOrMember then
      md.AddMember(def, args, body)
    if def.IsCompilerGenerated then
      () // Ignore compiler generated members for now
    elif def.IsExtensionMember then
      failwith "TODO: Extension Members"
    else
      md.LastClass.Value.AddMember(def, args, body)
  | FSharpImplementationFileDeclaration.InitAction e ->
    md.AddInitAction e

let rec visitRoot (prg: JSProgram) d =
  let containsClasses subDecls = 
    subDecls |> Seq.exists (function
      | FSharpImplementationFileDeclaration.Entity (e,_) -> not e.IsFSharpModule && not e.IsNamespace
      | _ -> failwith "Unexpected")
  match d with 
  | FSharpImplementationFileDeclaration.Entity (e, subDecls) ->
    let md = 
      match e with
      | IsNamespace -> if containsClasses subDecls then Some(prg.AddModule e) else None
      | IsModule -> Some(prg.AddModule e)
      | IsClass -> failwith "Unexpected"
    match md with
    | Some md -> subDecls |> Seq.iter (fun subDecl -> visitNested md subDecl)
    | None -> subDecls |> Seq.iter (fun subDecl -> visitRoot prg subDecl)
  | _ -> failwith "Unexpected"
        
let visitFiles (checkProjectResults: FSharpCheckProjectResults) =
  let prg = JSProgram()
  for file in checkProjectResults.AssemblyContents.ImplementationFiles do
    for decl in file.Declarations do
      visitRoot prg decl
