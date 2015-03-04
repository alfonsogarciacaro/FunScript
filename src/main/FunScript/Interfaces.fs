module internal FunScript.Interfaces

open System
open Microsoft.FSharp.Quotations

open AST
open InternalCompiler
open Reflection
open ReflectedDefinitions

let private getPrimaryConstructorVars compiler (t: System.Type) =
   let cons =
      t.GetConstructors(BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance)
      |> function [||] -> None | _ as arr -> Some arr.[0]
   match cons with
   | None -> None
   | Some cons ->
      Some <|
         if FSharpType.IsUnion t then  // For Union Types generate the same implementation for each case
            [ for uci in FSharpType.GetUnionCases t do yield getUnionCaseConstructorVar compiler uci ]
         elif FSharpType.IsRecord t then
            [ getRecordConstructorVar compiler t ]
         else
            [ getObjectConstructorVar compiler cons ]

let private generateImplMethod (compiler: ICompiler) implType vars lambdaExpr =
   Lambda(None, vars, Block(compiler.Compile ReturnStrategies.returnFrom lambdaExpr))

let private generateImplDic compiler (interfaceType: Type) (implTypes: Type list) mi =
   implTypes
   |> List.choose (fun typ ->
//      try
         let interfaceMap = typ.GetInterfaceMap(interfaceType)
         interfaceMap.InterfaceMethods |> Seq.tryFindIndex (fun x -> x = mi) |> function
         | None -> None
         | Some i ->
            match methodCallPattern interfaceMap.TargetMethods.[i] with
            | None -> None                                              // TODO: Throw ReflectedDefinitionException?
            | Some getVarsExpr ->
               getPrimaryConstructorVars compiler typ |> function
               | None -> None
               | Some consVars ->
                  consVars
                  |> List.map (fun consVar ->
                        let vars, lambdaExpr = getVarsExpr()
                        VarName consVar, generateImplMethod compiler typ vars lambdaExpr)
                  |> Some
//      with
//      | _ -> None
   )
   |> List.concat
   |> function
      | [] -> None
      | xs -> Some(Object xs)

let private defineMethod (compiler: ICompiler) (interfaceType: Type) (implTypes: Type list) mi =
   match generateImplDic compiler interfaceType implTypes mi with
   | None -> ()
   | Some implDic ->
      let name = JavaScriptNameMapper.mapMethod mi
      let specialization = getMethodSpecialization compiler mi
      let implDicVar, selfVar, argVars =
         Var("implDicVar", typeof<obj>),
         Var("self", interfaceType),
         mi.GetParameters() |> Seq.mapi (fun i arg ->
            Var("arg" + (string i), arg.ParameterType)) |> List.ofSeq
      let vars = selfVar::argVars
      let varRefs = vars |> List.map (fun v -> Reference v)
      compiler.DefineGlobal (name + specialization) (fun var ->
         [ Assign(Reference var,
            Apply(Lambda(None, [],
                     Block [
                        DeclareAndAssign(implDicVar, implDic)
                        Return(Lambda(None, vars,
                                 Block [
                                    Return(Apply(EmitExpr (fun (i, scope) ->
                                                sprintf "%s[Object.getPrototypeOf(%s).constructor.name]"
                                                   ((!scope).ObtainNameScope implDicVar FromReference |> fst)
                                                   ((!scope).ObtainNameScope selfVar FromReference |> fst)),
                                            varRefs)) ] )) ] ), [])) ])
      |> ignore

let build compiler (interfaces: (Type*Type list) list) =
   interfaces
   |> List.iter (fun (interfaceType, implTypes) ->
      interfaceType.GetMethods()
      |> Array.iter (fun mi -> defineMethod compiler interfaceType implTypes mi))