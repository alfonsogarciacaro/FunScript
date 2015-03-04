﻿module (*internal*) FunScript.InternalCompiler

open AST
open System
open System.Reflection
open Microsoft.FSharp.Quotations

type Helpers =
   static member Cast(x:obj) : 'a = failwith "never"

type IReturnStrategy =
   abstract Return: JSExpr -> JSStatement

type ICompiler = 
   abstract Compile: returnStrategy:IReturnStrategy -> expr:Expr -> JSStatement list
   abstract ReplacementFor: MethodBase -> Quote.CallType -> MethodInfo option
   abstract NextTempVar: unit -> Var
   abstract DefineGlobal: string -> (Var -> JSStatement list) -> Var
   abstract DefineGlobalInitialization: JSStatement list -> unit
   abstract DefineInterface: Type -> Type -> unit
   abstract Globals: JSStatement list

type ICompilerComponent =
   abstract TryCompile: compiler:ICompiler -> returnStrategy:IReturnStrategy -> expr:Expr -> JSStatement list

type CallReplacer = 
  { Target: MethodBase
    TargetType: Quote.CallType
    Replacement: MethodInfo option
    TryReplace: ICompiler -> IReturnStrategy -> Expr option * Type [] * Expr list -> JSStatement list }

type CompilerComponent =
   | CallReplacer of CallReplacer
   | CompilerComponent of ICompilerComponent

type Compiler(components, buildInterfaces) as this = 
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

   let mutable globals = Map.empty
   let mutable interfaces = Map.empty

   let define name cons =
      match globals |> Map.tryFind name with
      | Some (var, _) -> var
      | None -> 
         // Define upfront to avoid problems with mutually recursive methods
         let var = Var.Global(name, typeof<obj>)
         globals <- globals |> Map.add name (var, [])
         let assignment = cons var
         globals <- globals |> Map.add name (var, assignment)
         var

   let defineInterface (interfaceType: Type) (implType: Type) =
      let newEntry =
         match interfaces |> Map.tryFind interfaceType.FullName with
         | Some (_, implTypes) -> (interfaceType, implType::implTypes)
         | None -> (interfaceType, [implType])
      interfaces <- interfaces |> Map.add interfaceType.FullName newEntry

   let mutable initialization = List.empty

   let getGlobals() =
      buildInterfaces this (interfaces |> Seq.map (fun x -> x.Value) |> List.ofSeq)

      let globals = globals |> Map.toList |> List.map snd
      let declarations = 
         match globals with
         | [] -> []
         | _ -> [Declare (globals |> List.map (fun (var, _) -> var))]
      let assignments = globals |> List.collect snd

      List.append
         (List.append declarations assignments)
         initialization

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

   member __.Compile returnStrategy expr  = 
      compile returnStrategy expr

   member __.Globals = getGlobals()

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
         
      member __.DefineGlobal name cons =
         define name cons

      member __.DefineGlobalInitialization stmts =
         initialization <- List.append initialization stmts

      member __.DefineInterface interfaceType implType =
         defineInterface interfaceType implType

      member __.Globals = getGlobals()
         