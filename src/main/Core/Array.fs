namespace FunScript.Core.Collections
open FunScript
open FunScript.Core

/// <summary>Basic operations on arrays.</summary>
[<JS; RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Array =
   [<JSEmitInline("{0}.length")>]
   let length (xs:'a[]): int =
      failwith "never"

   [<JSEmitInline("({0}[{1}] = {2})")>]
   let set (xs:'a[]) (i:int) (v:'a): unit =
      failwith "never"

   [<JSEmitInline("{0}.slice({1}, {1} + {2})")>]
   let sub (xs:'a[]) (offset:int) (length:int): 'a[] =
      failwith "never"
      
   [<JSEmitInline("{0}.slice(0)")>]
   let copy (xs:'a[]): 'a[] =
      failwith "never"

   [<JSEmitInline("{1}.sort(function($0,$1){return {0}($0)($1)})")>]
   let sortInPlaceWith (f:'a -> 'a -> int) (xs:'a[]): unit =
      failwith "never"

   let sortInPlaceBy f xs =
      sortInPlaceWith (fun (x:'a) (y:'a) -> 
         let x = f x
         let y = f y
         compare x y) xs

   let sortInPlace xs =
      sortInPlaceWith (fun (x:'a) (y:'a) -> 
         compare x y) xs

   let sortBy f xs =
      let ys = copy xs
      sortInPlaceBy f ys
      ys

   let sort xs =
      let ys = copy xs
      sortInPlace ys
      ys

   let sortWith f xs =
      let ys = copy xs
      sortInPlaceWith f ys
      ys

   [<JSEmitInline("(new Array({0}))")>]
   let zeroCreate (size:int) : 'a[] = failwith "never"

   let createInstance(_type : obj, size) =
      zeroCreate size : obj[]

   let fill (xs:'a []) offset count value =
      for i = offset to offset + count - 1 do
         xs.[i] <- value

   let copyTo (xs:'a []) fromOffset (ys:'a []) toOffset count =
      let diff = toOffset - fromOffset
      for i = fromOffset to fromOffset + count - 1 do
         ys.[i + diff] <- xs.[i]
      ys

   let isEmpty xs = 
      length xs = 0

   let foldIndexed f seed xs =
      let mutable acc = seed
      for i = 0 to length xs - 1 do
         acc <- f i acc xs.[i]
      acc

   let fold<'a,'acc> f (seed:'acc) (xs: 'a []) = 
      foldIndexed (fun _ acc x -> f acc x) seed xs

   let foldBackIndexed<'a,'acc> f (xs: 'a []) (seed:'acc) =
      let mutable acc = seed
      let size = length xs
      for i = 1 to size do
         acc <- f (i-1) xs.[size - i] acc
      acc

   let foldBack<'a,'acc> f (xs: 'a []) (seed:'acc) = 
      foldBackIndexed (fun _ x acc -> f x acc) xs seed 

   let foldIndexed2 f seed xs ys =
      let mutable acc = seed
      if length xs <> length ys then failwith "Arrays had different lengths"
      for i = 0 to length xs - 1 do
         acc <- f i acc xs.[i] ys.[i]
      acc

   let fold2<'a, 'b, 'acc> f (seed:'acc) (xs: 'a []) (ys: 'b [])  = 
      foldIndexed2 (fun _ acc x y -> f acc x y) seed xs ys

   let foldBackIndexed2<'a, 'b, 'acc> f (xs: 'a []) (ys: 'b []) (seed:'acc) =
      let mutable acc = seed
      if length xs <> length ys then failwith "Arrays had different lengths"
      let size = length xs
      for i = 1 to size do
         acc <- f (i-1) xs.[size - i] ys.[size - i] acc
      acc

   let foldBack2<'a, 'b, 'acc> f (xs: 'a []) (ys: 'b []) (seed:'acc) = 
      foldBackIndexed2 (fun _ x y acc -> f x y acc) xs ys seed 

   let inline iter f xs =
      fold (fun () x -> f x) () xs

   let iteri f xs =
      foldIndexed (fun i () x -> f i x) () xs

   let forall f xs =
      fold (fun acc x -> f x && acc) true xs

   let permute f xs = 
      let size = length xs
      let ys  = zeroCreate size
      let checkFlags = zeroCreate size
      iteri (fun i x ->
         let j = f i 
         if j < 0 || j >= size then 
            invalidOp "Not a valid permutation"
         ys.[j] <- x
         checkFlags.[j] <- 1) xs
      let isValid = forall ((=) 1) checkFlags
      if not isValid then
         invalidOp "Not a valid permutation"
      ys

   let rev xs =
      let size = length xs
      let ys = zeroCreate size
      for i = 0 to size - 1 do
         ys.[i] <- xs.[size - 1 - i]
      ys

   let scan<'a, 'acc> f (seed:'acc) (xs: 'a []) =
      let ys = zeroCreate (length xs + 1)
      ys.[0] <- seed
      for i = 0 to length xs - 1 do
         ys.[i + 1] <- f ys.[i] xs.[i]
      ys

   let scanBack<'a, 'acc> f (xs: 'a []) (seed:'acc) =
      let ys = zeroCreate (length xs + 1)
      let size = length xs
      ys.[length xs] <- seed
      for i = 1 to length xs do
         ys.[size - i] <- f xs.[size - i] ys.[size - i + 1]
      ys

   [<JSEmitInline("{0}.concat({1})")>]
   let append (xs:'a []) (ys:'a []) : 'a [] = 
      failwith "never"

   [<JSEmitInline("[].concat.apply([], {0})")>]
   let concatImpl (xss: 'a [][]) : 'a [] = failwith "never"

   let mapi f (xs:'a []) =
      let ys = zeroCreate (length xs)
      for i = 0 to length xs - 1 do
         ys.[i] <- f i xs.[i]
      ys

   let inline map f xs =
      mapi (fun _ x -> f x) xs

   let mapi2 f (xs:'a []) (ys:'b []) =
      if length xs <> length ys then failwith "Arrays had different lengths"
      let zs = zeroCreate(length xs)
      for i = 0 to length xs - 1 do
         zs.[i] <- f i xs.[i] ys.[i]
      zs

   let map2 f xs ys =
      mapi2 (fun _ x y -> f x y) xs ys

   let mapi3 f (xs:'a []) (ys:'b []) (zs:'c []) =
      if length xs <> length ys || length ys <> length zs then failwith "Arrays had different lengths"
      let rs = zeroCreate(length xs)
      for i = 0 to length xs - 1 do
         rs.[i] <- f i xs.[i] ys.[i] zs.[i]
      rs

   let map3 f xs ys zs =
      mapi3 (fun _ x y z -> f x y z) xs ys zs

   let concat (xs:'a [] seq) : 'a[] =
      xs 
      |> Array.ofSeq
      |> concatImpl

   let collect f xs =
      map f xs
      |> concatImpl

   let iter2 f xs ys =
      fold2 (fun () x y -> f x y) () xs ys

   let iteri2 f xs ys =
      foldIndexed2 (fun i () x y -> f i x y) () xs ys
      
   let empty<'a> : 'a [] = [||]

   let rec tryPickIndexedAux f i xs =
      if i = length xs then None
      else 
         let result = f i xs.[i]
         match result with
         | Some _ -> result
         | None -> tryPickIndexedAux f (i+1) xs

   let tryPickIndexed f xs =
      tryPickIndexedAux f 0 xs

   let tryPick f xs =
      tryPickIndexed (fun _ x -> f x) xs

   let pick f xs =
      match tryPick f xs with
      | None -> invalidOp "Array did not contain any matching elements"
      | Some x -> x 

   let tryFindIndexed f xs =
      tryPickIndexed (fun i x -> if f i x then Some x else None) xs

   let tryFind f xs =
      tryPickIndexed (fun _ x -> if f x then Some x else None) xs

   let findIndexed f xs =
      match tryFindIndexed f xs with
      | None -> invalidOp "Array did not contain any matching elements"
      | Some x -> x
      
   let find f xs =
      findIndexed (fun _ x -> f x) xs

   let get xs n =
      findIndexed (fun i _ -> n = i) xs
      
   let tryFindIndex f xs =
      tryPickIndexed (fun i x -> if f x then Some i else None) xs

   let findIndex f xs =
      match tryFindIndex f xs with
      | None -> invalidOp "Array did not contain any matching elements"
      | Some x -> x

   let filter f xs =
      let ys = zeroCreate 0
      let mutable j = 0
      for i = 0 to length xs - 1 do
         if f xs.[i] then 
            ys.[j] <- xs.[i]
            j <- j + 1
      ys

   let partition f xs =
      let ys = zeroCreate 0
      let zs = zeroCreate 0
      let mutable j = 0
      let mutable k = 0
      for i = 0 to length xs - 1 do
         if f xs.[i] then 
            ys.[j] <- xs.[i]
            j <- j + 1
         else 
            zs.[k] <- xs.[i]
            k <- k + 1
      ys, zs

   let choose f xs =
      let ys = zeroCreate 0
      let mutable j = 0
      for i = 0 to length xs - 1 do
         match f xs.[i] with
         | Some y -> 
            ys.[j] <- y
            j <- j + 1
         | None -> ()
      ys

   let inline init n f =
      let mutable xs = zeroCreate 0
      for i = 0 to n - 1 do xs.[i] <- f i
      xs

   let create n x =
      let mutable xs = zeroCreate 0
      for i = 0 to n - 1 do xs.[i] <- x
      xs    

   let replicate n x =
      init n (fun _ -> x)

   let reduce f xs =
      if length xs = 0 then invalidOp "Array was empty"
      else foldIndexed (fun i acc x -> if i = 0 then x else f acc x) Unchecked.defaultof<_> xs

   let reduceBack f xs =
      if length xs = 0 then invalidOp "Array was empty"
      else foldBackIndexed (fun i x acc -> if i = 0 then x else f acc x) xs Unchecked.defaultof<_>

   let forall2 f xs ys =
      fold2 (fun acc x y -> acc && f x y) true xs ys

   let rec existsOffset f xs i =
      if i = length xs then false
      else f xs.[i] || existsOffset f xs (i+1)

   let exists f xs = 
      existsOffset f xs 0
      
   let rec existsOffset2 f xs (ys:_ []) i =
      if i = length xs then false
      else f xs.[i] ys.[i] || existsOffset2 f xs ys (i+1)

   let rec exists2 f xs ys =
      if length xs <> length ys then failwith "Arrays had different lengths"
      existsOffset2 f xs ys 0

   let unzip xs =
      let bs = zeroCreate 0
      let cs = zeroCreate 0
      iteri (fun i (b, c) ->
         bs.[i] <- b
         cs.[i] <- c
      ) xs
      bs, cs

   let unzip3 xs =
      let bs = zeroCreate 0
      let cs = zeroCreate 0
      let ds = zeroCreate 0
      iteri (fun i (b, c, d) ->
         bs.[i] <- b
         cs.[i] <- c
         ds.[i] <- d
      ) xs
      bs, cs, ds

   let zip xs ys =
      map2 (fun x y -> x, y) xs ys

   let zip3 xs ys zs =
      map3 (fun x y z -> x, y, z) xs ys zs

   let inline sum (xs: ^a []) : ^a =
      fold (fun acc x -> acc + x) Core.GenericConstants.Zero xs

   let inline sumBy (f:^a -> ^b) (xs: ^a []) : ^b =
      fold (fun acc x -> acc + f x) Core.GenericConstants.Zero xs

   let inline maxBy f xs =
      reduce (fun x y -> if f y > f x then y else x) xs

   let inline max xs = 
      reduce max xs
            
   let inline minBy f xs =
      reduce (fun x y -> if f y > f x then x else y) xs

   let inline min xs = 
      reduce min xs
            
   let inline average (zs: ^a []) : ^a =
      let total = sum zs
      let count = sumBy (fun _ -> Core.GenericConstants.One< ^a >) zs
      total / count

   let inline averageBy (g: ^a -> ^b ) (zs: ^a []) : ^b =
      let total = sumBy g zs
      let count = sumBy (fun _ -> Core.GenericConstants.One< ^a >) zs
      total / count

   