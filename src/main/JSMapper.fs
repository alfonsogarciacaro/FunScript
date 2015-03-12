module internal FunScript.JSMapper

open System.Web
open System.Reflection
open System.Collections.Generic
open System.Text.RegularExpressions
open Microsoft.FSharp.Reflection
open Microsoft.FSharp.Quotations

//let alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_"
//
//let obtainFreeCompressedName (scope: Set<Var>) (var: Var) =
//   let rec buildName (acc : StringBuilder) i =
//      let letterCode = i % alphabet.Length
//      acc.Append alphabet.[letterCode] |> ignore
//      let nextI = (i - letterCode) / alphabet.Length
//      if nextI <> 0 then
//         buildName acc nextI
//      else acc.ToString()
//   buildName (StringBuilder()) scope.Count

let private keywords =
   set [
      "break"
      "case"
      "catch"
      "continue"
      "default"
      "delete"
      "do"
      "else"
      "finally"
      "for"
      "function"
      "if"
      "in"
      "instanceof"
      "new"
      "return"
      "switch"
      "this"
      "throw"
      "try"
      "typeof"
      "var"
      "void"
      "while"
      "with"
   ]

let private reservedWords =
   set [
      "ns"
      "abstract"
      "arguments"
      "boolean"
      "byte"
      "char"
      "class"
      "const"
      "debugger"
      "double"
      "enum"
      "export"
      "extends"
      "final"
      "float"
      "goto"
      "int"
      "interface"
      "implements"
      "import"
      "long"
      "native"
      "package"
      "private"
      "protected"
      "public"
      "short"
      "static"
      "super"
      "synchronized"
      "throws"
      "transient"
      "volatile"
   ]

let private unsafeWords = keywords + reservedWords

let private preventConflicts exists str =
   let rec check n =
      let name = if n > 0 then sprintf "%s%i" str n else str
      if not (exists name)
      then name
      else check (n+1)
   check 0

let mutable private cache = Dictionary<System.Type, string * Dictionary<obj, string>>()
let resetCache() = cache <- Dictionary<_,_>()

let private sanitizeVarName =
   let regex = Regex "[^0-9a-zA-Z$_]"
   fun (var: Quotations.Var) ->
      let str = regex.Replace(var.Name, "_")
      if unsafeWords.Contains str then "_" + str else str

let mapVar scope var =
   sanitizeVarName var
   |> preventConflicts (fun x ->
      scope |> Map.exists (fun _ x' -> x = x'))
   |> fun name -> scope.Add(var, name), name

let mapType (typ: System.Type) =
   if cache.ContainsKey typ then fst cache.[typ]
   else
      let name =
         HttpUtility.JavaScriptStringEncode typ.Name
         |> fun x ->
            let i = x.IndexOf '`'
            if i >= 0 then x.Substring(0, i) else x
         |> preventConflicts (fun x ->
            cache.Values |> Seq.exists (fun (x',_) -> x = x'))
      cache.Add(typ, (name, Dictionary<_,_>()))
      name

let private mapCall (call: obj, typ: System.Type, name: string) =
   ignore(mapType typ)           // Force type mapping in case it wasn't cached yet
   let meths = snd cache.[typ]
   if meths.ContainsKey call then meths.[call]
   else
      let name =
         HttpUtility.JavaScriptStringEncode name
         |> preventConflicts (fun x ->
            meths.Values |> Seq.exists (fun x' -> x = x'))
      meths.Add(call, name)
      name

let mapCase (uci: UnionCaseInfo) = mapCall(uci, uci.DeclaringType, uci.Name)
let mapMethod (meth: MethodBase) = mapCall(meth, meth.DeclaringType, meth.Name)

let getSpace() = " "
let getNewline padding = System.Environment.NewLine + (String.init padding (fun _ -> "  "))

// TODO
let compressEmittedExpr (jscode: string) = jscode

let removeTrailingZeroes =
   let reg = Regex("(\.\d+?)0+$")
   fun number -> reg.Replace(number, "$1")


