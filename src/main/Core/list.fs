namespace FunScript.Core
open FunScript

[<JS; RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module List =
   // TODO TODO TODO
   let concat (lists:seq<'T list>):'T list =
      failwith "Not implemented"

   let ofSeq (source:seq<'T>):'T list =
      Seq.toList source

   let toSeq (source:'T list):seq<'T> =
      Seq.ofList source

   let head = function
      | [] -> failwith "List was empty"
      | x::xs -> x

   let tail = function
      | [] -> failwith "List was empty"
      | x::xs -> xs

   let rec foldIndexedAux f i acc = function
      | [] -> acc
      | x::xs -> foldIndexedAux f (i+1) (f i acc x) xs

   let foldIndexed<'a,'acc> f (seed:'acc) (xs: 'a list) = 
      foldIndexedAux f 0 seed xs

   let fold<'a,'acc> f (seed:'acc) (xs: 'a list) = 
      foldIndexed (fun _ acc x -> f acc x) seed xs

   let rev xs =
      fold (fun acc x -> x::acc) [] xs

   let foldBack<'a,'acc> f (xs: 'a list) (seed:'acc) = 
      fold (fun acc x -> f x acc) seed (rev xs)

   let rec foldIndexed2Aux f i acc bs cs =
      match bs, cs with
      | [], [] -> acc
      | x::xs, y::ys -> foldIndexed2Aux f (i+1) (f i acc x y) xs ys
      | _ -> invalidOp "Lists had different lengths"

   let foldIndexed2<'a, 'b, 'acc> f (seed:'acc) (xs: 'a list) (ys: 'b list) = 
      foldIndexed2Aux f 0 seed xs ys

   let fold2<'a, 'b, 'acc> f (seed:'acc) (xs: 'a list) (ys: 'b list) = 
      foldIndexed2 (fun _ acc x y -> f acc x y) seed xs ys

   let foldBack2<'a, 'b, 'acc> f (xs: 'a list) (ys: 'b list) (seed:'acc) = 
      fold2 (fun acc x y -> f x y acc) seed (rev xs) (rev ys)

   let rec foldIndexed3Aux f i acc bs cs ds =
      match bs, cs, ds with
      | [], [], [] -> acc
      | x::xs, y::ys, z::zs -> foldIndexed3Aux f (i+1) (f i acc x y z) xs ys zs
      | _ -> invalidOp "Lists had different lengths"

   let foldIndexed3<'a, 'b, 'c, 'acc> f (seed:'acc) (xs: 'a list) (ys: 'b list) (zs: 'c list) = 
      foldIndexed3Aux f 0 seed xs ys zs

   let fold3<'a, 'b, 'c, 'acc> f (seed:'acc) (xs: 'a list) (ys: 'b list) (zs: 'c list) = 
      foldIndexed3 (fun _ acc x y -> f acc x y) seed xs ys zs

   let scan<'a, 'acc> f (seed:'acc) (xs: 'a list) =
      fold (fun acc x ->
         match acc with
         | [] -> failwith "never"
         | y::ys -> (f y x)::acc) (seed::[]) xs
      |> rev

   let scanBack<'a, 'acc> f (xs: 'a list) (seed:'acc) =
      scan (fun acc x -> f x acc) seed (rev xs)
      |> rev

   let length xs = 
      fold (fun acc _ -> acc + 1) 0 xs

   let append xs ys =
      fold (fun acc x -> x::acc) ys (rev xs)

   let collect f xs =
      fold (fun acc x -> append (f x) acc) [] (rev xs)

   let map f xs =
      fold (fun acc x -> ((f x)::acc)) [] xs
      |> rev

   let mapi f xs =
      foldIndexed (fun i acc x -> (f i x)::acc) [] xs
      |> rev

   let map2 f xs ys =
      fold2 (fun acc x y -> (f x y)::acc) [] xs ys
      |> rev

   let mapi2 f xs ys =
      foldIndexed2 (fun i acc x y  -> (f i x y)::acc) [] xs ys
      |> rev

   let map3 f xs ys zs =
      fold3 (fun acc x y z -> (f x y z)::acc) [] xs ys zs
      |> rev

   let mapi3 f xs ys zs =
      foldIndexed3 (fun i acc x y z -> (f i x y z)::acc) [] xs ys zs
      |> rev

   let iter f xs =
      fold (fun () x -> f x) () xs

   let iter2 f xs ys =
      fold2 (fun () x y -> f x y) () xs ys

   let iteri f xs =
      foldIndexed (fun i () x -> f i x) () xs

   let iteri2 f xs ys =
      foldIndexed2 (fun i () x y -> f i x y) () xs ys

   let ofArray xs =
      Array.foldBack (fun x acc -> x::acc) xs []

   let toArray xs =
      let size = length xs
      let ys = Array.zeroCreate size
      iteri (fun i x -> ys.[i] <- x) xs
      ys

   let empty<'a> : 'a list = []

   let isEmpty = function
      | [] -> true
      | _ -> false

   let rec tryPickIndexedAux f i = function
      | [] -> None
      | x::xs -> 
         let result = f i x
         match result with
         | Some _ -> result
         | None -> tryPickIndexedAux f (i+1) xs

   let tryPickIndexed f xs =
      tryPickIndexedAux f 0 xs

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

   let nth xs n =
      findIndexed (fun i _ -> n = i) xs
      
   let filter f xs =
      fold (fun acc x ->
         if f x then x::acc
         else acc) [] xs

   let partition f xs =
      fold (fun (lacc, racc) x ->
         if f x then x::lacc, racc
         else lacc, x::racc) ([],[]) xs

   let choose f xs =
      fold (fun acc x ->
         match f x with
         | Some y -> y::acc
         | None -> acc) [] xs

   let init n f =
      let mutable xs = []
      for i = 1 to n do xs <- f (n - i)::xs
      xs

   let replicate n x =
      init n (fun _ -> x)

   let reduce f = function
      | [] -> invalidOp "List was empty"
      | h::t -> fold f h t

   let reduceBack f = function
      | [] -> invalidOp "List was empty"
      | h::t -> foldBack f t h
   
   let forall f xs =
      fold (fun acc x -> acc && f x) true xs

   let forall2 f xs ys =
      fold2 (fun acc x y -> acc && f x y) true xs ys

   let rec exists f = function
      | [] -> false
      | x::xs -> f x || exists f xs

   let rec exists2 f bs cs =
      match bs, cs with
      | [], [] -> false
      | x::xs, y::ys -> f x y || exists2 f xs ys
      | _ -> invalidOp "Lists had different lengths"

   let unzip xs =
      fold (fun (lacc, racc) (x, y) -> x::lacc, y::racc) ([],[]) xs

   let unzip3 xs =
      fold (fun (lacc, macc, racc) (x, y, z) -> x::lacc, y::macc, z::racc) ([],[],[]) xs

   let zip xs ys =
      map2 (fun x y -> x, y) xs ys

   let zip3 xs ys zs =
      map3 (fun x y z -> x, y, z) xs ys zs

   let sort xs =
      xs
      |> toArray 
      |> Array.sort
      |> ofArray

   let sortWith f xs =
      xs
      |> toArray 
      |> Array.sortWith f
      |> ofArray

   let inline sum (xs: ^a list) : ^a =
      fold (fun acc x -> acc + x) Core.GenericConstants.Zero xs

   let inline sumBy (f:^a -> ^b) (xs: ^a list) : ^b =
      fold (fun acc x -> acc + f x) Core.GenericConstants.Zero xs

   let inline maxBy f xs =
      reduce (fun x y -> if f y > f x then y else x) xs

   let inline max xs = 
      reduce max xs
            
   let inline minBy f xs =
      reduce (fun x y -> if f y > f x then x else y) xs

   let inline min xs = 
      reduce min xs
            
   let inline average (zs: ^a list) : ^a =
      let total = sum zs
      let count = sumBy (fun _ -> Core.GenericConstants.One< ^a >) zs
      total / count

   let inline averageBy (g: ^a -> ^b ) (zs: ^a list) : ^b =
      let total = sumBy g zs
      let count = sumBy (fun _ -> Core.GenericConstants.One< ^a >) zs
      total / count

   let permute f xs =
      xs  
      |> toArray
      |> Array.permute f 
      |> ofArray

   let sortBy f xs =
      let ys = xs |> toArray
      Array.sortInPlaceBy f ys
      ys |> ofArray

[<JS; Sealed>]
type internal FSList =
   member li.Head with [<JSEmitInline("{0}.Head")>] get() = failwith "never"
   member li.Tail with [<JSEmitInline("{0}.Tail")>] get() = failwith "never"
   member li.Length = List.length (unbox li)
   member li.Item = List.nth (unbox li)
   member li.IsEmpty = List.isEmpty (unbox li)
