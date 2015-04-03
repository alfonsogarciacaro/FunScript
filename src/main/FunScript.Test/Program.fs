// Scratchpad for quick testing of new FunScript features and easier debugging
open FunScript
open System.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

// TODO TODO TODO: Check JSEmit(Inline) expressions: args, paramarray...
[<ReflectedDefinition>]
module Test =
   type R = {a: string; b: int}

   type I =
      abstract member Value: int
   
   type A() =
      interface I with
         member val Value = 6

   type B() =
      interface I with
         member val Value = 4

   let getValue (v: I) = v.Value

   let create<'T when 'T: (new: unit -> 'T)>() = new 'T()

   let main() =
      let m = Map.empty.Add("hola", 55)
      m.["hola"]

[<EntryPoint; System.STAThread>]
let main argv =
   let code = FSCompiler.Compile <@ Test.main() @>
   if not(System.String.IsNullOrWhiteSpace code) then
      System.Windows.Forms.Clipboard.SetText(code)
   printfn "%s" code
   System.Console.ReadLine() |> ignore
   0 // return an integer exit code
