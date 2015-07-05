module FunScript.ComputationExpressions

open AST
open Microsoft.FSharp.Compiler.SourceCodeServices

let instanceMethod typName methName = function
   | BasicPatterns.Call(Some (BasicPatterns.Value o), meth, _, _, args)
      when o.FullType.TypeDefinition.FullName = typName && meth.CompiledName = methName -> Some args
   | _ -> None

let staticMethod typName methName = function
   | BasicPatterns.Call(None, meth, _, _, args)
      when meth.EnclosingEntity.FullName = typName && meth.CompiledName = methName -> Some args
   | _ -> None

let (|Delay|_|) builderType expr =
   instanceMethod builderType "Delay" expr
   |> function Some [BasicPatterns.Lambda(_, delayedExpr)] -> Some delayedExpr | _ -> None

let (|Return|_|) builderType expr =
   instanceMethod builderType "Return" expr
   |> function Some [retExpr] -> Some retExpr | _ -> None

let (|ReturnFrom|_|) builderType expr =
   instanceMethod builderType "ReturnFrom" expr
   |> function Some [retExpr] -> Some retExpr | _ -> None


module Async =
   let private bname = "Microsoft.FSharp.Control.FSharpAsyncBuilder"
   let compileAsyncComp (com: ICompiler) (scope: IScopeInfo) = function
      | Return bname expr
      | ReturnFrom bname expr ->
         Apply(EmitExpr("Promise.resolve",[]), [com.CompileExpr scope expr]) |> buildExpr
      | Delay bname expr
      | expr ->
         com.CompileExpr scope expr |> buildExpr

