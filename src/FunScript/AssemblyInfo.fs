namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("FunScript")>]
[<assembly: AssemblyProductAttribute("FunScript")>]
[<assembly: AssemblyDescriptionAttribute("F# to JavaScript compiler")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
