﻿namespace FunScript

open System
open System.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Reflection

[<AutoOpen>]
module internal AttExt =
   let private findAttribute<'attr> =
      Array.tryPick (fun (obj:obj) -> 
         match obj with
         | :? 'attr as attr -> Some attr
         | _ -> None)

   type System.Reflection.MethodBase with
      member x.TryGetAttribute<'attr>() =
         x.GetCustomAttributes(true)
         |> findAttribute<'attr>

   type System.Reflection.ParameterInfo with
      member x.TryGetAttribute<'attr>() =
         x.GetCustomAttributes(true)
         |> findAttribute<'attr>

[<AutoOpen>]
module internal PatternsExt = 
   // In quotations generated by type provider code, records and
   // objects are mixed up a bit (???) so this corrects the behaviour.

   let (|NewObject|_|) = function
      | Patterns.NewObject(ctor, args) ->
         if FSharpType.IsRecord(ctor.DeclaringType, BindingFlags.Public ||| BindingFlags.NonPublic) then None
         else Some(ctor, args)
      | _ -> None
   
   let (|NewRecord|_|) = function
      | Patterns.NewRecord(typ, args) -> Some(typ, args)
      | Patterns.NewObject(ctor, args) ->
         if FSharpType.IsRecord(ctor.DeclaringType, BindingFlags.Public ||| BindingFlags.NonPublic) then 
           Some(ctor.DeclaringType, args)
         else None
      | _ -> None

[<AutoOpen>]
module internal ExprExt =
   type DebugInfo =
      { file: string; line: int; col: int }
      static member Empty = { file=""; line=0; col=0 }

   // From http://stackoverflow.com/questions/11772046/file-and-line-numbers-in-f-code-quotations
   let private getDebugInfo (e: Expr) = 
       let (|Val|_|) = function
           | Patterns.Value(:? 't as v,_) -> Some v
           | _ -> None
       e.CustomAttributes
       |> List.tryPick (function
         | Patterns.NewTuple
            [ Val("DebugRange"); Patterns.NewTuple
               [ Val(file:string); Val(startLine:int); Val(startCol:int); _; _] ] ->
            Some { file=file; line=startLine; col=startCol }
         | _ -> None)
       |> function
         | Some di -> di
         | None -> { file=""; line=0; col=0 } // TODO: Throw exception instead?

   let private flags = BindingFlags.NonPublic ||| BindingFlags.Instance
   let private setDebugInfo (e: Expr) (dinfo: DebugInfo) =
      typeof<Expr>.GetField("attribs", flags).SetValue(e,
         [Expr.NewTuple [Expr.Value("DebugRange")
                         Expr.NewTuple [Expr.Value dinfo.file
                                        Expr.Value dinfo.line
                                        Expr.Value dinfo.col
                                        Expr.Value 0
                                        Expr.Value 0]]])

   type Expr with
      member expr.DebugInfo = getDebugInfo expr
      member expr.With dinfo =
         setDebugInfo expr dinfo
         expr
