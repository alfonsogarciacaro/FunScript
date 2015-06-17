namespace TestNs

type Test1() =
  member __.Hola = 5

module TestMod =
  module TestMod2 =
    module TestMod3 =
      let a = 3
    let a = 5

namespace TestNs

type private Test3 = {r: int}

namespace TestNs2

type Test2 private() =
  member __.A = 5


//namespace Microsoft.FSharp.Collections
//
//module List =
//  let chorrada f (li: 'a List) =
//    List.iter f li
//
//namespace Test3
//open Microsoft.FSharp.Collections
//
//module Test =
////  [] |> List.chorrada (fun i -> printfn "Hola")
//
//  let private a = 5
//  let b = 6
//  module Nested =
//    type A = {a: int}
//
//    let c = a + 5
//    let private d = 6
//  
//  let Nested = 4
//  
//module L =
//  Test.Nested
//
