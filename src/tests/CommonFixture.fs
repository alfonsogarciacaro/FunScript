﻿[<AutoOpen>]
module FunScript.Tests.Common

open FunScript
open NUnit.Framework
open Microsoft.FSharp.Linq.QuotationEvaluation

let defaultCompile quote =
   FSCompiler.Compile(quote) //, noReturn = false, shouldCompress = true)

//let compileWithRx quote =
//      Compiler.Compiler.Compile(quote, components = Rx.Interop.components(), noReturn = false, isEventMappingEnabled = false, shouldCompress = true)

let checkAreEqualWith prerequisiteJS compile (expectedResult : obj) quote =
   let code : string = compile quote
   try
      let result =
          let code =
                "return function (data, callback) {"
                    + "callback(null, (function () {"
                    + System.Environment.NewLine
                    + prerequisiteJS
                    + System.Environment.NewLine 
                    + code + ";"
                    + System.Environment.NewLine 
                    + "    })());"
                    + "};"
          Async.AwaitTask(EdgeJs.Edge.Func(code).Invoke(""))
          |> Async.RunSynchronously
      let result =
          match expectedResult with
          | :? float ->
              match result with
              | :? int as x -> box(float x)
              | x -> x
          | _ -> result
      let message (ex: 'a) (re: 'b) = sprintf "%sExpected: %A%sBut was: %A" System.Environment.NewLine ex System.Environment.NewLine re
      Assert.That((result = expectedResult), (message expectedResult result))
   // Wrap xUnit exceptions to stop pauses.
   with
    | :? System.AggregateException as e ->
        let ex = e.InnerException
        printfn "// Code:\n%s" code
        if ex.GetType().Namespace.StartsWith "FunScript"
        then Microsoft.FSharp.Core.Operators.raise ex
        else failwithf "Message: %s\n" ex.Message
    | ex ->
        printfn "// Code:\n%s" code
        if ex.GetType().Namespace.StartsWith "FunScript"
        then Microsoft.FSharp.Core.Operators.raise ex
        else failwithf "Message: %s\n" ex.Message

let checkAreEqual expectedResult quote =
   checkAreEqualWith "" defaultCompile expectedResult quote

/// Bootstrapping:
/// Generates code. Runs it through a JS interpreter. 
/// Checks the result against the compiled expression.
let check (quote:Quotations.Expr) =
   let expectedResult = quote.EvalUntyped()
   checkAreEqual expectedResult quote

//let checkRx (quote:Quotations.Expr) =
//   let expectedResult = quote.EvalUntyped()
//   checkAreEqualWith(sprintf "var Rx = require('%s../../../lib/RxJs/rx.all.js');" __SOURCE_DIRECTORY__)
//        compileWithRx expectedResult quote

let checkAsync (quote:Quotations.Expr) =
    let expectedResult = <@@ Async.RunSynchronously %%quote @@>.EvalUntyped()
    let immediateQuote = 
        <@@
            let result = ref None
            async { 
                let! v = %%quote 
                result := Some v
            } |> Async.StartImmediate
            !result |> Option.get 
        @@>
    checkAreEqual expectedResult immediateQuote