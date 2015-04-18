namespace FunScript.Core
open FunScript

// TODO TODO TODO: implement dict and IEnumerable/IDictionary interfaces

[<JS; CompiledName("FSDic")>]
type Dictionary<'k,'v>() =
   [<JSEmitInline("{0}[{1}]")>] 
   member x.Item with get (k: 'k) = failwith "never" and set (k: 'k) (v: 'v) = failwith "never"
   
   member x.Count = x.Keys.Count

   [<JSEmitInline("Object.keys({0})")>]
   member x.Keys: ResizeArray<'k> = failwith "never"

   [<JSEmit("var values = []; for (var k in {0}) { values.push({0}[k]) } return values")>]
   member x.Values: ResizeArray<'k> = failwith "never"

   [<JSEmit("if ({0}[{1}] === undefined) { {0}[{1}] = {2} } else { throw 'Key already exists' }")>]
   member x.Add(key: 'k, value: 'v): unit = failwith "never"

   [<JSEmitInline("({0} = {})")>]
   member x.Clear(): unit = failwith "never"

   [<JSEmitInline("({0}[{1}] !== undefined)")>]
   member x.ContainsKey(key: 'k): bool = failwith "never"

   [<JSEmit("for (var key in {0}) { if ({0}[key] === {1}) { return true } } return false")>]
   member x.ContainsValue(value: 'v): bool = failwith "never"

   [<JSEmit("if ({0}[{1}] !== undefined) { delete {0}[{1}]; return true; } else { return false; }")>]
   member x.Remove(key: 'k): bool = failwith "never"

//   interface System.Collections.Generic.IEnumerable<KeyValuePair<'k,'v>> with
//      member x.GetEnumerator(): System.Collections.Generic.IEnumerator<KeyValuePair<'k, 'v>> = 
//         failwith "Not implemented yet"
//      
//      member x.GetEnumerator(): System.Collections.IEnumerator =
//         upcast (x :> System.Collections.Generic.IEnumerable<_>).GetEnumerator()

