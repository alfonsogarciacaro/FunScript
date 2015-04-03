namespace FunScript.Core
open FunScript

// TODO TODO TODO: Nullable primitives

[<JS; Sealed; CompiledName("FSOption")>]
type internal Option =
   static member IsSome with [<JSEmitInline("({0}!==null)")>] get(o: _ option) = failwith "never"
   static member IsNone with [<JSEmitInline("({0}===null)")>] get(o: _ option) = failwith "never"
   static member Value with [<JSEmitInline("{0}")>] get(o: _ option) = failwith "never"

[<JS>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Option =
   [<CompiledName("IsSome")>]
   [<JSEmitInline("({0}!==null)")>]
   let isSome option = 
      match option with  
      | None -> false 
      | Some _ -> true

   [<CompiledName("IsNone")>]
   [<JSEmitInline("({0}===null)")>]
   let isNone option = not (isSome option)

   [<CompiledName("GetValue")>]
   [<JSEmitInline("{0}")>]
   let get (option: 'a option) = Option.get option

   //[<CompiledName("Count")>]
   [<JSEmitInline("({0}===null?0:1)")>]
   let count option = 
      match option with  
      | None -> 0 
      | Some _ -> 1

   //[<CompiledName("Fold")>]
   let fold<'a, 'acc> (f:'acc -> 'a -> 'acc) (s:'acc) (inp:'a option) = 
      match inp with 
      | None -> s 
      | Some x -> f s x

   //[<CompiledName("FoldBack")>]
   let foldBack<'a, 'acc> (f:'a -> 'acc -> 'acc) (inp:'a option) (s:'acc) = 
      match inp with 
      | None -> s 
      | Some x -> f x s

   //[<CompiledName("Exists")>]
   let exists p inp = 
      match inp with 
      | None -> false 
      | Some x -> p x

   //[<CompiledName("ForAll")>]
   let forall p inp = 
      match inp with 
      | None -> true 
      | Some x -> p x

   //[<CompiledName("Iterate")>]
   let iter f inp = 
      match inp with 
      | None -> () 
      | Some x -> f x

   //[<CompiledName("Map")>]
   let map f inp = 
      match inp with 
      | None -> None 
      | Some x -> Some (f x)

   //[<CompiledName("Bind")>]
   let bind f inp = 
      match inp with 
      | None -> None 
      | Some x -> f x

   //[<CompiledName("ToArray")>]
   let toArray option = 
      match option with 
      | None -> [||] 
      | Some x -> [| x |]

   //[<CompiledName("ToList")>]
   let toList (option: 'a option): 'a list = 
      match option with
      | None -> [] 
      | Some x -> [ x ]