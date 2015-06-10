// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.
#r @"C:\Users\Alfonso\Documents\GitHub\FunScript\packages\FSharp.Compiler.Service\lib\net45\FSharp.Compiler.Service.dll"
#load "Library.fs"

open FunScript
open System.IO

Path.Combine(__SOURCE_DIRECTORY__, "../../scripts/test.fsx")
|> Library.parseAndCheckScript
|> Library.visitFiles
