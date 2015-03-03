module internal FunScript.Options

open Microsoft.FSharp.Quotations
open AST

[<FunScript.JS>]
module Replacements =

   [<JSEmitInline("{0}")>]
   let fakeNullable x = x

   [<JSEmitInline("({0} !== null)")>]
   let hasValue x = not(obj.ReferenceEquals(x, null))

   let defaultValue =
      CompilerComponent.create <| fun (|Split|) _ returnStrategy ->
         let getDefault (t: System.Type) =
            if t.IsEnum then Integer(0)
            elif t = typeof<bool> then Boolean(false)
            elif Reflection.jsIntegerTypes.Contains t.FullName then Integer(0)
            elif Reflection.jsNumberTypes.Contains t.FullName then Number(0.)
//            elif t.Name.StartsWith("Nullable") then Null
            else Null
         function
         | Patterns.DefaultValue t ->
            [ returnStrategy.Return <| getDefault t ]
         | Patterns.Call(None, mi, []) when mi.DeclaringType.Name = "Unchecked" && mi.Name = "DefaultOf" ->
            [ returnStrategy.Return <| getDefault mi.ReturnType ]
         | _ -> [] 

let components = 
   [
      [
         ExpressionReplacer.create <@ fun (maybe:_ option) -> maybe.IsNone @> <@ FunScript.Core.Option.IsNone @>
         ExpressionReplacer.create <@ fun (maybe:_ option) -> maybe.IsSome @> <@ FunScript.Core.Option.IsSome @>
         ExpressionReplacer.createUnsafe <@ fun (maybe:_ option) -> maybe.Value @> <@ FunScript.Core.Option.GetValue @>
         
         Replacements.defaultValue  // TODO: This should be in CommonOperators but seems to be intercepted before getting there
         ExpressionReplacer.createUnsafe <@ fun x -> System.Nullable(x) @> <@ Replacements.fakeNullable @>
         ExpressionReplacer.createUnsafe <@ fun (x:_ System.Nullable) -> x.HasValue @> <@ Replacements.hasValue @>
         ExpressionReplacer.createUnsafe <@ fun (x:_ System.Nullable) -> x.Value @> <@ Replacements.fakeNullable @>
      ]
      ExpressionReplacer.createModuleMapping
         "FSharp.Core" "Microsoft.FSharp.Core.OptionModule"
         "FunScript" "FunScript.Core.Option"

   ] |> List.concat