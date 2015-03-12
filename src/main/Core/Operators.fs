[<FunScript.JS; AutoOpen>]
module FunScript.Core.Operators
open FunScript
open Microsoft

module Unchecked =
   [<JSEmitInline("({0}<{1}?-1:({0}==={1}?0:1))")>]
   let compare x y: int = failwith "Not implemented"

   // TODO TODO TODO: Resolve default values through components
   [<JSEmitInline("null")>]
   let defaultof<'a> : 'a = failwith "Not implemented"

[<JSEmitInline("{0}[0]")>]
let fst (t: 'a*'b) = FSharp.Core.Operators.fst t

[<JSEmitInline("{0}[0]")>]
let snd (t: 'a*'b) = FSharp.Core.Operators.snd t

let ref x = { contents = x }
let (!) (r:_ Ref) = r.contents
let (:=) (r: _ Ref) v = r.contents <- v

let (|>) x f = f x
let (||>) (x, y) f = f x y
let (|||>) (x, y, z) f = f x y z

let (<|) f x = f x
let (<||) f (x, y) = f x y
let (<|||) f (x, y, z) = f x y z

let (>>) (f:'a->'b) (g:'b->'c):'a->'c = fun x -> g(f x)
let (<<) (f:'b->'c) (g:'a->'b):'a->'c = fun x -> f(g x)

[<JSEmitInline("({0}===null?{1}:{0})")>]
let defaultArg x y =
   match x with None -> y | Some v -> v

let incr (x:int ref): unit = x := !x + 1
let decr (x:int ref): unit = x := !x - 1


let exn(msg: string) = FunScript.Core.Exception(msg)
// TODO TODO TODO: raise, failwith, invalidOp, invalidArg

// Don't emit just "{0}" as this will probably be pased as lambda function
let id x = x

[<JSEmitInline("{0}")>]
let ignore x = FSharp.Core.Operators.ignore x

[<JSEmitInline("{0}")>]
let box x = FSharp.Core.Operators.box x

[<JSEmitInline("{0}")>]
let unbox x = FSharp.Core.Operators.unbox x
