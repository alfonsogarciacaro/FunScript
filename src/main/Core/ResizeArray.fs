﻿namespace FunScript.Core.Collections
open FunScript
open FunScript.Core

//let ToSeq(xs: ResizeArray<'T>) =
//    Seq.unfold (fun i -> if i < xs.Count then Some(xs.[i], i+1) else None) 0

[<JS>]
type ResizeArray<'T> =
    [<JSEmitInline("{0}.length")>]
    static member private getCount(xs: ResizeArray<'T>): int = failwith "never"

    [<JSEmitInline("{0}[{1}]")>]
    static member private getItem(xs: ResizeArray<'T>, index: int): 'T = failwith "never"

    // NOTE: Some JS MVC frameworks override the Array.splice method to hook on array updates
    [<JSEmitInline("{0}.splice({1}, 1, {2})")>]
    static member private setItem(xs: ResizeArray<'T>, index: int, item: 'T): unit = failwith "never"

    [<JSEmitInline("[]")>]
    static member ZeroCreate(): ResizeArray<'T> = failwith "never"

    // NOTE: If we use 'new Array(size)' the array is automatically filled with undefined values which is not
    // the expected behaviour in .NET so we just create empty arrays no matter the given initial capacity.
    [<JSEmitInline("[]")>]
    static member ZeroCreateWithSize(size: int): ResizeArray<'T> = failwith "never"

    static member OfSeq(items: seq<'T>) =
        let ra = ResizeArray<'T>.ZeroCreate()
        ra.AddRange(items)
        ra

    member xs.Count with get() = ResizeArray<'T>.getCount(xs)

    member xs.Item
        with get(index: int) = ResizeArray<'T>.getItem(xs, index)
        and set(index: int) (item: 'T) = ResizeArray<'T>.setItem(xs, index, item)

    [<JSEmitInline("({0} = [])")>]
    member xs.Clear(): unit = failwith "never"

    [<JSEmitInline("{0}.push({1})")>]
    member xs.Add(item: 'T): unit = failwith "never"

    member xs.AddRange(items: seq<'T>): unit =
        for item in items do xs.Add(item)

    [<JSEmitInline("({0}.indexOf({1}) > -1)")>]
    member xs.Contains(searchElement: 'T): bool = failwith "never"    

    [<JSEmitInline("{0}.indexOf({1})")>]
    member xs.IndexOf(searchElement: 'T): int = failwith "never"

    [<JSEmit("var i = {0}.indexOf({1}); if (i > -1) { {0}.splice(i, 1); return true; } else { return false; }")>]
    member xs.Remove(item: 'T): bool = failwith "never"

    [<JSEmitInline("{0}.splice({1}, 1)")>]
    member xs.RemoveAt(index: int): unit = failwith "never"

    [<JSEmitInline("{0}.splice({1}, 0, {2})")>]
    member xs.Insert(index: int, item: 'T): unit = failwith "never"

    [<JSEmitInline("{0}.reverse()")>]
    member xs.Reverse(): unit = failwith "never"

    [<JSEmitInline("{0}.sort()")>]
    member xs.SortInPlace(): unit = failwith "never"

    [<JSEmitInline("{0}.sort({1})")>]
    member xs.SortInPlaceWith(comparison: System.Comparison<'T>): unit = failwith "never"

//    member xs.GetEnumerator(): System.Collections.Generic.IEnumerator<'T> =
//        upcast new Seq.UnfoldEnumerator<_,_>(-1, fun i -> if i < xs.Count then Some(xs.[i], i+1) else None)
