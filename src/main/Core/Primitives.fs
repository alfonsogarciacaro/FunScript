namespace FunScript.Core
open FunScript

[<JS>]
type KeyValuePair<'Key, 'Value>(key, value) =
   member __.Key: 'Key = key
   member __.Value: 'Value = value

[<JS>]
type GenericComparer<'a when 'a: comparison>() =
   interface System.Collections.Generic.IComparer<'a> with
      member __.Compare(x, y) = compare x y

[<JS>]
type Lazy<'T>(value : 'T, factory: unit -> 'T) =
    let mutable isCreated = false
    let mutable value = value 

    static member Create(f: (unit->'T)) : Lazy<'T> = 
        Lazy<'T> (value = Unchecked.defaultof<'T>, factory = f)
    static member CreateFromValue(x:'T) : Lazy<'T> = 
        Lazy<'T> (value = x, factory = fun () -> x)
    member x.IsValueCreated = isCreated
    member x.Value =  
        if not isCreated then
            value <- factory()
            isCreated <- true
        value

[<JS; CompiledName("FSException")>]
type Exception(message: string) =
    member __.Message = message

[<JS>]
module GenericConstants =
   [<JSEmitInline("0")>]
   let Zero<'a> :'a = failwith "never"

   [<JSEmitInline("1")>]
   let One<'a> : 'a = failwith "never"