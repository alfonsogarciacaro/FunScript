namespace FunScript.Core
open FunScript

open System.Collections
open System.Collections.Generic

[<JS>]
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Seq =
   let enumerator (xs:_ seq) =   
      xs.GetEnumerator()

   let foldIndexedAux f acc (xs:_ IEnumerator) =
      let i = ref 0
      let acc = ref acc
      while xs.MoveNext() do
         acc := f !i !acc xs.Current
         i := !i + 1
      !acc

   let foldIndexed<'a,'acc> f (seed:'acc) (xs: 'a seq) = 
      foldIndexedAux f seed (enumerator xs)

   let fold<'a,'acc> f (seed:'acc) (xs: 'a seq) = 
      foldIndexed (fun _ acc x -> f acc x) seed xs

   let foldIndexed2Aux f acc (xs:_ IEnumerator) (ys:_ IEnumerator) =
      let i = ref 0
      let acc = ref acc
      while xs.MoveNext() && ys.MoveNext() do
         acc := f !i !acc xs.Current ys.Current
         i := !i + 1
      !acc

   let foldIndexed2<'a, 'b, 'acc> f (seed:'acc) (xs: 'a seq) (ys: 'b seq) = 
      foldIndexed2Aux f seed (enumerator xs) (enumerator ys)

   let fold2<'a, 'b, 'acc> f (seed:'acc) (xs: 'a seq) (ys: 'b seq) = 
      foldIndexed2 (fun _ acc x y -> f acc x y) seed xs ys

   let head (xs:_ seq) =
      let enumerator = enumerator xs
      if enumerator.MoveNext() then
         enumerator.Current
      else failwith "Seq was empty"

   type UnfoldEnumerator<'acc, 'a>(seed:'acc, unfold) =
      let mutable acc = Some seed
      let mutable current = Unchecked.defaultof<'a>
      interface IEnumerator<'a> with
         member __.Current: 'a = current
         member __.Current: obj = current :> obj
         member __.Dispose() = ()
         member __.Reset() = acc <- Some seed; current <- Unchecked.defaultof<_>
         member __.MoveNext() =
            match acc with
            | None -> false
            | Some currAcc ->
               match unfold currAcc with
               | None -> 
                  acc <- None
                  current <- Unchecked.defaultof<_>
                  false
               | Some (value, nextAcc) ->
                  acc <- Some nextAcc
                  current <- value
                  true

   type CreateEnumerable<[<EqualityConditionalOnAttribute; ComparisonConditionalOnAttribute>] 'a>(factory) =
//      interface System.IComparable<'a seq> with 
//         member this.CompareTo ys =
//            let xs = this :> IEnumerable<'a>
//            Seq.compareWith (fun x y -> Unchecked.compare x y) 
//               xs ys
      interface IEnumerable<'a> with
         member __.GetEnumerator() = factory() :> 'a IEnumerator
         member __.GetEnumerator() = factory() :> IEnumerator

   let fromFactory f =
      CreateEnumerable(f) :> _ IEnumerable

   let delay (f:unit -> 'a seq): 'a seq =
      fromFactory(fun () -> enumerator(f()))

   let unfold f seed =
      fromFactory(fun () -> upcast new UnfoldEnumerator<'acc, 'a>(seed, f))

   let append xs ys =
      delay <| fun () ->
         let enums = lazy enumerator xs, lazy enumerator ys
         enums |> unfold (function
            | (curr, next) as enums when curr.Value.MoveNext() ->
               Some (curr.Value.Current, enums)
            | (curr, next) when next.Value.MoveNext() ->
               Some (next.Value.Current, (next, curr))
            | _ -> None)

   let skip n xs =
      fromFactory(fun () ->
         let enum = enumerator xs
         for i = 1 to n do enum.MoveNext() |> ignore
         enum)

   let empty<'a> : 'a seq =
       unfold (fun _ -> None) false

   let length xs =
      fold (fun count _ -> count + 1) 0 xs

   let isEmpty xs =
      not ((enumerator xs).MoveNext())

   let inline sum (xs: ^a seq) : ^a =
      fold (fun acc x -> acc + x) Core.GenericConstants.Zero xs

   let inline sumBy (f:^a -> ^b) (xs: ^a seq) : ^b =
      fold (fun acc x -> acc + f x) Core.GenericConstants.Zero xs

   let reduce f xs =
      let first = head xs
      let rest = skip 1 xs
      fold f first rest

   let filter f xs =
      let rec trySkipToNext(enum:_ IEnumerator) =
         if enum.MoveNext() then
            if f enum.Current then
               Some(enum.Current, enum)
            else trySkipToNext enum
         else None
      delay <| fun () ->
         enumerator xs
         |> unfold trySkipToNext

   let skipWhile f xs =
      delay <| fun () ->
         let hasPassed = ref false
         xs |> filter (fun x ->
            !hasPassed || (
               hasPassed := not (f x)
               !hasPassed
            ))

   let choose f xs =
      let rec trySkipToNext(enum:_ IEnumerator) =
         if enum.MoveNext() then
            match f enum.Current with
            | None -> trySkipToNext enum
            | Some value -> Some(value, enum)
         else None
      delay <| fun () ->
         enumerator xs
         |> unfold trySkipToNext

   let inline maxBy f xs =
      reduce (fun x y -> if f y > f x then y else x) xs

   let inline max xs = 
      reduce max xs
            
   let inline minBy f xs =
      reduce (fun x y -> if f y > f x then x else y) xs

   let inline min xs = 
      reduce min xs
            
   let inline average (zs: ^a seq) : ^a =
      let total = sum zs
      let count = sumBy (fun _ -> Core.GenericConstants.One< ^a >) zs
      total / count

   let inline averageBy (g: ^a -> ^b ) (zs: ^a seq) : ^b =
      let total = sumBy g zs
      let count = sumBy (fun _ -> Core.GenericConstants.One< ^a >) zs
      total / count

   let forall f xs =
      fold (fun acc x -> acc && f x) true xs

   let forall2 f xs ys =
      fold2 (fun acc x y -> acc && f x y) true xs ys

   let rec existsAux f (xs:_ IEnumerator) =
      xs.MoveNext() && (f xs.Current || existsAux f xs)

   let exists f xs =
      existsAux f (enumerator xs)

   let rec exists2Aux f (xs:_ IEnumerator) (ys:_ IEnumerator) =
      xs.MoveNext() && ys.MoveNext() && (f xs.Current ys.Current || exists2Aux f xs ys)

   let exists2 f xs ys =
      exists2Aux f (enumerator xs) (enumerator ys)

   let iter f xs =
      fold (fun () x -> f x) () xs

   let iter2 f xs ys =
      fold2 (fun () x y -> f x y) () xs ys

   let iteri f xs =
      foldIndexed (fun i () x -> f i x) () xs

   let map f xs =
      delay <| fun () ->
         enumerator xs
         |> unfold (fun enum ->
            if enum.MoveNext() then Some(f enum.Current, enum)
            else None)

   let mapi f xs =
      delay <| fun () ->
         (enumerator xs, 0)
         |> unfold (fun (enum, i) ->
            if enum.MoveNext() then Some(f i enum.Current, (enum, i+1))
            else None)

   let map2 f xs ys =
      delay <| fun () ->
         let xs = enumerator xs
         let ys = enumerator ys
         ()
         |> unfold (fun () ->
            if xs.MoveNext() && ys.MoveNext() then
               Some(f xs.Current ys.Current, ())
            else None)

   let map3 f xs ys zs =
      delay <| fun () ->
         let xs = enumerator xs
         let ys = enumerator ys
         let zs = enumerator zs
         ()
         |> unfold (fun () ->
            if xs.MoveNext() && ys.MoveNext() && zs.MoveNext() then
               Some(f xs.Current ys.Current zs.Current, ())
            else None)

   let zip xs ys =
      map2 (fun x y -> x, y) xs ys

   let zip3 xs ys zs =
      map3 (fun x y z -> x, y, z) xs ys zs

   let rec tryPickIndexedAux f i (xs:_ IEnumerator) =
      if xs.MoveNext() then 
         let result = f i xs.Current
         match result with
         | Some _ -> result
         | None -> tryPickIndexedAux f (i+1) xs
      else None

   let tryPickIndexed f xs =
      tryPickIndexedAux f 0 (enumerator xs)

   let tryPick f xs =
      tryPickIndexed (fun _ x -> f x) xs

   let pick f xs =
      match tryPick f xs with
      | None -> invalidOp "List did not contain any matching elements"
      | Some x -> x 

   let tryFindIndexed f xs =
      tryPickIndexed (fun i x -> if f i x then Some x else None) xs

   let tryFind f xs =
      tryPickIndexed (fun _ x -> if f x then Some x else None) xs

   let findIndexed f xs =
      match tryFindIndexed f xs with
      | None -> invalidOp "List did not contain any matching elements"
      | Some x -> x
      
   let find f xs =
      findIndexed (fun _ x -> f x) xs

   let tryFindIndex f xs =
      tryPickIndexed (fun i x -> if f x then Some i else None) xs

   let findIndex f xs =
      match tryFindIndex f xs with
      | None -> invalidOp "List did not contain any matching elements"
      | Some x -> x

   let nth n xs =
      let xs = enumerator xs
      for i = 0 to n do
         if not (xs.MoveNext()) then
            failwith "index out of range"
      xs.Current

   let scan f seed xs =
      delay <| fun () ->
         let xs = enumerator xs
         None |> unfold (function
            | None -> Some(seed, Some seed)
            | Some acc -> 
               if xs.MoveNext() then 
                  let acc = f acc xs.Current
                  Some (acc, Some acc)
               else None)

   let toList xs =
      fold (fun acc x -> x::acc) [] xs
      |> List.rev

   let ofList xs =
      xs |> unfold (function
         | [] -> None
         | x::xs -> Some(x, xs))

   let toArray xs =
      let ys = Array.zeroCreate 0
      xs |> iteri (fun i x -> ys.[i] <- x)
      ys

   let ofArray (xs:_ []) =
      0 |> unfold (fun i ->
         if i < xs.Length then Some(xs.[i], i+1)
         else None)

   let init n f =
      0 |> unfold (fun i ->
         if i < n then Some(f i, i+1)
         else None)

   let compareWith f (xs:'a seq) (ys:'a seq) =
      let nonZero =
         map2 (fun x y -> f x y) xs ys
         |> tryFind (fun i -> i <> 0)
      match nonZero with
      | Some diff -> diff
      | None -> length xs - length ys

   let exactlyOne xs =
      let xs = enumerator xs
      if not <| xs.MoveNext() then failwith "Sequence was empty"
      let result = xs.Current
      if xs.MoveNext() then failwith "Sequence had multiple items"
      result

   let initInfinite f =
      delay <| fun () ->
         0 |> unfold (fun i ->
            Some(f i, i+1))

   let where f xs = 
      filter f xs

   let take n xs =
      delay <| fun () ->
         let xs = enumerator xs
         0 |> unfold (fun i ->
            if i < n && xs.MoveNext() then
               Some(xs.Current, i+1)
            else None)

   let truncate n xs =
      take n xs

   let takeWhile f xs =
      delay <| fun () ->
         let xs = enumerator xs
         () |> unfold (fun () ->
            if xs.MoveNext() && f xs.Current then Some(xs.Current, ())
            else None)

   let last xs =
      reduce (fun _ x -> x) xs

   let pairwise xs =
      scan 
         (fun (_, last) next -> last, next) 
         (Unchecked.defaultof<_>, Unchecked.defaultof<_>)
         xs
      |> skip 1

   let readOnly xs =
      map id xs

   let singleton x =
      Some x |> unfold (function
         | Some x -> Some(x, None)
         | None -> None)

   let compare xs ys =
      compareWith compare xs ys

   let sort (xs: 'a seq): 'a seq =
      let ys = xs |> toArray
      Array.sort ys
      |> ofArray

   let sortBy f xs =
      let ys = xs |> toArray
      Array.sortInPlaceBy f ys
      ys |> ofArray

   let distinctBy f xs =
      scan (fun (_, acc) x ->
         let y = f x
         if acc |> Set.contains y then
            None, acc
         else Some x, acc |> Set.add y) 
         (None, Set.empty) xs
      |> choose fst

   let distinct xs =
      distinctBy id xs

   let groupBy (f : 'a -> 'b) (xs : 'a seq) =
      fold (fun (acc:Map<_,_>) x ->
         let k = f x
         match acc.TryFind k with
         | Some vs -> acc.Add(k, x::vs)
         | None -> acc.Add(k, [x])) Map.empty xs
      |> Map.toSeq
      |> map (fun (k, vs) -> k, vs :> 'a seq)

   let countBy f xs =
      groupBy f xs
      |> map (fun (k, vs) -> k, length vs)

   let concat xs =
       delay <| fun () ->
           let enum = enumerator xs
           let tryGetNext innerEnum =
               let innerEnum = ref innerEnum
               let output = ref None
               let hasFinished = ref false
               while not !hasFinished do
                   match !innerEnum with
                   | None -> 
                       if enum.MoveNext() then
                           innerEnum := Some(enumerator enum.Current)
                       else hasFinished := true
                   | Some currentEnum ->
                       if currentEnum.MoveNext() then
                           output := Some currentEnum.Current
                           hasFinished := true
                       else innerEnum := None
               match !innerEnum, !output with
               | Some e, Some x -> Some(x, Some e)
               | _ -> None
           unfold (fun x -> tryGetNext x) None

   let collect f xs =
      map f xs |> concat

   module RuntimeHelpers =
       let enumerateWhile (g : unit -> bool) (b: seq<'T>) : seq<'T> =
           unfold(fun () ->
               if g() then Some(b, ())
               else None) ()
           |> concat

       let enumerateThenFinally (rest : seq<'T>) (compensation : unit -> unit)  =
           delay <| fun () ->
             let enum = 
                try enumerator rest
                finally compensation()
             enum
             |> unfold (fun enum ->
                try
                   if enum.MoveNext() then Some(enum.Current, enum)
                   else None
                finally compensation())

       let enumerateUsing (resource : 'T :> System.IDisposable) (rest: 'T -> #seq<'U>) =
           let isDisposed = ref false
           let disposable = resource :> System.IDisposable
           let disposeOnce() =
               if not !isDisposed then
                   isDisposed := true
                   disposable.Dispose()
           try enumerateThenFinally (rest resource) disposeOnce
           finally disposeOnce()

[<JS>]
module Range =
   let inline customStep (first: ^a) (stepping: ^b) (last: ^a) : ^a seq =
      let zero: ^b = Core.GenericConstants.Zero
      first |> Seq.unfold (fun x -> 
         if (stepping > zero && x <= last) || (stepping < zero && x >= last) then Some(x, x + stepping)
         else None)

   let inline oneStep (first: ^a) (last: ^a) : ^a seq =
      customStep first Core.GenericConstants.One< ^a> last