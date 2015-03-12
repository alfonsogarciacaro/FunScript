// Scratchpad for quick testing of new FunScript features and easier debugging

open FunScript
open System.Reflection
open Microsoft.FSharp.Quotations

// TODO TODO TODO: Check JSEmit(Inline) expressions: args, paramarray...
[<ReflectedDefinition>]
module Test =
   type A(a, b) =
      member val B = a - b
      static member C = 2 //with [<JSEmit("return 2")>] get() = 2
      static member C2() = 3
      member x.D a b c = a - b - c

   module Test2 =
      type B =
         static member C() = 2
   
   let main() =
//      typeof<_ option>.GetMethods() |> Seq.iter (fun x -> printfn "%O" x)
      let xs = seq { for i=10 downto 0 do yield i }
      Seq.sum xs
//      A(1,2).B + (Test2.B.C())
//      Map.empty

[<EntryPoint; System.STAThread>]
let main argv =
   let code = FSCompiler.Compile <@ Test.main() @>
   if not(System.String.IsNullOrWhiteSpace code) then
      System.Windows.Forms.Clipboard.SetText(code)
   printfn "%s" code
   System.Console.ReadLine() |> ignore
   0 // return an integer exit code
