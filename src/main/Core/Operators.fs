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

//let (|>) x f = f x
//let (<|) f x = f x

let (||>) (x, y) f = f x y
let (|||>) (x, y, z) f = f x y z

let (<||) f (x, y) = f x y
let (<|||) f (x, y, z) = f x y z

let (>>) (f:'a->'b) (g:'b->'c):'a->'c = fun x -> g(f x)
let (<<) (f:'b->'c) (g:'a->'b):'a->'c = fun x -> f(g x)

[<JSEmitInline("({0}===null?{1}:{0})")>]
let defaultArg x y =
   match x with None -> y | Some v -> v

let incr (x:int ref): unit = x := !x + 1
let decr (x:int ref): unit = x := !x - 1

let exn(msg: string) = Core.Exception(msg)

// This will be replaced by a Throw statement (see PrimiveTypes.errors component)
let raise(ex: Core.Exception) =
   System.Exception(ex.Message) |> FSharp.Core.Operators.raise

let failwith(msg: string) = raise(exn msg)

// TODO: invalidOp, invalidArg

// When `ignore` and `id` are passed as lambdas, they'll be captured by Applications.fnDefinition
[<JSEmitInline("{0}"); CompiledName("FSIdentitiy")>]
let id x = x

// TODO: Use a component to turn it into undefined if return strategy is ReturnFrom?
[<JSEmitInline("{0}"); CompiledName("FSIgnore")>]
let ignore x = ()

// `box` and `unbox` should never be passed as lambdas,
[<JSEmitInline("{0}")>]
let box x = FSharp.Core.Operators.box x

[<JSEmitInline("{0}")>]
let unbox<'T>(x: obj) = FSharp.Core.Operators.unbox<'T> x

// Convenience JS functions
[<JSEmitInline("require({0})")>]
let require<'T>(path: string): 'T = failwith "never"

// TODO: define, requireAMD
