namespace FunScript.Core.Collections
open FunScript

// TODO: implement dict and IEnumerable interface

[<JS>]
type Dictionary<'k,'v> [<JSEmitInline("{}")>] () =
   member x.Item
      with [<JSEmitInline("{0}[{1}]")>] get k = failwith "never"
      and [<JSEmitInline("{0}[{1}] = {2}")>] set k v = failwith "never"
   
   member x.Count = x.Keys.Count

   [<JSEmitInline("Object.keys({0})")>]
   member x.Keys: ResizeArray<'k> = failwith "never"

   [<JSEmit("var values = []; for (var k in {0}) { values.push({0}[k]) } return values")>]
   member x.Values: ResizeArray<'k> = failwith "never"

   [<JSEmit("if ({0}[{1}] === undefined) { {0}[{1}] = {2} } else { throw 'Key already exists' }")>]
   member x.Add(dic: Dictionary<'k,'v>, key: 'k, value: 'v): unit = failwith "never"

   [<JSEmitInline("({0} = {})")>]
   member x.Clear(dic: Dictionary<'k,'v>): unit = failwith "never"

   [<JSEmitInline("({0}[{1}] !== undefined)")>]
   member x.ContainsKey(dic: Dictionary<'k,'v>, key: 'k): bool = failwith "never"

   [<JSEmit("for (var key in {0}) { if ({0}[key] === {1}) { return true } } return false")>]
   member x.ContainsValue(dic: Dictionary<'k,'v>, value: 'v): bool = failwith "never"

   [<JSEmit("if ({0}[{1}] !== undefined) { delete {0}[{1}]; return true; } else { return false; }")>]
   member x.Remove(dic: Dictionary<'k,'v>, key: 'k): bool = failwith "never"

   interface System.Collections.Generic.IEnumerable<Core.KeyValuePair<'k,'v>> with
      member x.GetEnumerator(): System.Collections.Generic.IEnumerator<Core.KeyValuePair<'k, 'v>> = 
         failwith "Not implemented yet"
      
      member x.GetEnumerator(): System.Collections.IEnumerator =
         upcast (x :> System.Collections.Generic.IEnumerable<_>).GetEnumerator()
      
      

