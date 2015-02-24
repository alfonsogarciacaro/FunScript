module (*internal*) FunScript.InternalCompiler

open AST
open System
open System.Reflection
open Microsoft.FSharp.Quotations

type ReturnStrategy =
    | ReturnFrom
    | InPlace
    | AssignVar of Var
    member strategy.Return expr =
        match strategy with
        | ReturnFrom -> Return expr
        | InPlace -> Do expr
        | AssignVar var -> Assign(Reference var, expr)

type MethodMapping =
    { JSName: string; Var: Var; Assignment: JSStatement list }

type ImplTypeMapping =
    { ImplType: Type; JSName: string; Methods: Map<string, MethodMapping>}

// Types with no type arguments will also be considered as generics with a unique implementation
type GenericTypeMapping =
    { GenericType: Type; JSName: string; Implementations: Map<string, ImplTypeMapping>}

type ICompiler = 
   abstract Compile: returnStrategy: ReturnStrategy -> expr:Expr -> JSStatement list
   abstract ReplacementFor: MethodBase -> Quote.CallType -> MethodInfo option
   abstract NextTempVar: unit -> Var
   abstract DefineGlobal: MethodBase -> (Var -> JSStatement list) -> Var
   abstract Globals: JSStatement list

type ICompilerComponent =
   abstract TryCompile: compiler:ICompiler -> returnStrategy: ReturnStrategy -> expr:Expr -> JSStatement list

type CallReplacer = 
  { Target: MethodBase
    TargetType: Quote.CallType
    Replacement: MethodInfo option
    TryReplace: ICompiler -> ReturnStrategy -> Expr option * Type [] * Expr list -> JSStatement list }

type CompilerComponent =
   | CallReplacer of CallReplacer
   | CompilerComponent of ICompilerComponent

type Compiler(components) as this = 
   let parameterKey (mb : MethodBase) =
      mb.GetParameters() |> Array.map (fun pi ->
         pi.ParameterType.Name)
   let key (mb:MethodBase) tt =
      mb.Name, mb.DeclaringType.Name, mb.DeclaringType.Namespace, tt
 
   let callerReplacers =
      components |> Seq.choose (function
         | CallReplacer r -> Some r
         | _ -> None) 
      |> Seq.groupBy (fun r -> key r.Target r.TargetType)
      |> Seq.map (fun (key, values) ->
         key, values |> Seq.map (fun r -> parameterKey r.Target, r) |> Map.ofSeq)
      |> Map.ofSeq

   let callerReplacements =
      callerReplacers |> Seq.choose (fun (KeyValue(id, rs)) ->
         let replacements =
            rs 
            |> Map.map (fun k r -> r.Replacement)
            |> Map.filter (fun k r -> r.IsSome)
            |> Map.map (fun k r -> r.Value)
         if replacements = Map.empty then
            None
         else Some (id, replacements))
      |> Map.ofSeq

   let rest = 
      components |> Seq.choose (function
         | CompilerComponent c -> Some c
         | _ -> None) |> Seq.toList

   let tryComponent returnStrategy expr (part:ICompilerComponent) =
      match part.TryCompile this returnStrategy expr with
      | [] -> None
      | procCodes -> Some procCodes

   let tryAllComponents returnStrategy expr =
      let result = 
         rest |> List.tryPick(tryComponent returnStrategy expr)
      match result with
      | None -> []
      | Some statements -> statements

   let getTypeArgs(mi:MethodBase) =
      if mi.IsConstructor then mi.DeclaringType.GetGenericArguments()
      else
         Array.append
            (mi.DeclaringType.GetGenericArguments())
            (mi.GetGenericArguments())

   let tryCompileCall callType returnStrategy mi obj exprs  =
      let this = this :> ICompiler
      match callerReplacers.TryFind (key mi callType) with
      | Some rs ->
         let paramKey = parameterKey mi
         let r =
            match rs.TryFind paramKey with
            | None -> 
                // Try to pick overload with same number of parameters.
                // Favour those with most like params.
                let rec ordering i (ks : _[]) acc =
                    if i = paramKey.Length && i = ks.Length then
                        acc
                    elif i = paramKey.Length || i = ks.Length then
                        Int32.MaxValue
                    elif paramKey.[i] = ks.[i] then
                        ordering (i+1) ks (acc-1)
                    else ordering (i+1) ks acc
                rs |> Map.toArray 
                |> Array.minBy (fun (ks, _) -> ordering 0 ks 0)
                |> snd
            | Some r -> r
         let typeArgs = getTypeArgs mi
         r.TryReplace this returnStrategy (obj, typeArgs, exprs)
      | None -> []
         
   let compile returnStrategy expr =
      let replacementResult =
         match Quote.tryToMethodBase expr with
         | Some (obj, mi, exprs, callType) ->
            match Quote.specialOp mi with
            | Some opMi -> 
               match tryCompileCall callType returnStrategy opMi obj exprs with
               | [] -> tryCompileCall callType returnStrategy mi obj exprs
               | stmts -> stmts
            | None -> tryCompileCall callType returnStrategy mi obj exprs
         | None -> []
      let result =
         match replacementResult with
         | [] -> tryAllComponents returnStrategy expr
         | statements -> statements
      match result with
      | [] -> failwithf "Could not compile expression: %A" expr
      | statements -> statements

   let nextId = ref 0

   let mutable globals = Map.empty<string, GenericTypeMapping>

   let define (mb: MethodBase) cons =
      let implType = mb.DeclaringType
      let genType = if implType.IsGenericType then implType.GetGenericTypeDefinition() else implType

      let genTypeMapping =
          match globals.TryFind genType.FullName with
          | Some m -> m
          | None ->
            let jsName = "" // TODO: generate unique name
            { JSName = jsName; GenericType = genType; Implementations = Map.empty }

      let implTypeMapping =
          match genTypeMapping.Implementations.TryFind implType.FullName with
          | Some m -> m
          | None ->
            let jsName = "" // TODO: concatenate type args to genTypeMapping.JSName
            { JSName = jsName; ImplType = implType; Methods = Map.empty }

      let methodMapping =
          match implTypeMapping.Methods.TryFind (string mb) with
          | Some m -> m // TODO: Regenerate method for interfaces if genType is different
          | None ->
            let jsName = "" // TODO: generate unique name. Watch for overloads!
            let var = Var.Global(jsName, typeof<obj>)
            // TODO: generic methods optimizations
            { JSName = jsName; Var = var; Assignment = cons var }

      // TODO: Reassign globals
      // globals <- globals |> Map.add mb (var, []) |> Map.add mb (var, assignment)
      methodMapping.Var

   let getGlobals() =
      let globals = globals |> Map.toList |> List.map snd
      let declarations = 
         match globals with
         | [] -> []
         | _ -> [Declare (globals |> List.map (fun (var, _) -> var))]
      let assignments = globals |> List.collect snd
      List.append declarations assignments

   let tryFixDeclaringTypeGenericParameters (originalMethod:MethodBase) (replacementMethod:MethodInfo) =
        let genericTypeDefinition = 
            if replacementMethod.DeclaringType.IsGenericType then Some(replacementMethod.DeclaringType.GetGenericTypeDefinition())
            elif replacementMethod.DeclaringType.IsGenericTypeDefinition then Some replacementMethod.DeclaringType
            else None
        match genericTypeDefinition with
        | None -> replacementMethod
        | Some gt ->
            let typedDeclaringType = gt.MakeGenericType(originalMethod.DeclaringType.GetGenericArguments())
            let flags = BindingFlags.Instance ||| BindingFlags.Static ||| BindingFlags.NonPublic ||| BindingFlags.Public
            typedDeclaringType.GetMethods(flags)
            |> Seq.append (
                typedDeclaringType.GetInterfaces() 
                |> Seq.collect (fun it -> typedDeclaringType.GetInterfaceMap(it).TargetMethods))
            |> Seq.find (fun m -> m.Name = replacementMethod.Name 
                                  && m.IsStatic = replacementMethod.IsStatic // TODO: We may need to make this comparison safer
                                  && m.GetParameters().Length = replacementMethod.GetParameters().Length) 

   interface ICompiler with

      member __.Compile returnStrategy expr = 
         compile returnStrategy expr

      member __.ReplacementFor (pi:MethodBase) targetType =
         callerReplacements.TryFind (key pi targetType)
         |> Option.map (fun rs ->
            let paramKey = parameterKey pi
            let r =
               match rs.TryFind paramKey with
               | None -> (rs |> Seq.head).Value
               | Some r -> r
            tryFixDeclaringTypeGenericParameters pi r)

      member __.NextTempVar() = 
         incr nextId
         Var(sprintf "_%i" !nextId, typeof<obj>, false) 
         
      member __.DefineGlobal mb cons =
         define mb cons

      member __.Globals = getGlobals()
         