[<FunScript.JS>]
module FunScript.Dynamic
open FunScript

type System.Object with
   [<JSEmitInline("(new {0}({args}))")>]
   member x.Create([<System.ParamArray>] args: obj[]): 'a = failwith "never"
   [<JSEmitInline("{0}({args})")>]
   member x.Invoke([<System.ParamArray>] args: obj[]): 'a = failwith "never"
   [<JSEmitInline("{0}[{1}]")>]
   member x.Item with get(i: int) = failwith "never" and set(i: int) v: unit = failwith "never"
   [<JSEmitInline("{0}[{1}]")>]
   member x.Item with get(s: string) = failwith "never" and set(s: string) v: unit = failwith "never"

[<JSEmitInline("{0}[{1}]")>]
let (?) (o: obj) (s: string): 'a = unbox o.[s]
[<JSEmitInline("({0}[{1}]={2})")>]
let (?<-) (o: obj) (s: string) (v: 'a): unit = o.[s] <- box v
