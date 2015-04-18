module internal FunScript.JSMapper

open System.Web
open System.Reflection
open System.Collections.Generic
open System.Text.RegularExpressions
open Microsoft.FSharp.Reflection
open Microsoft.FSharp.Quotations

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

let preventConflicts exists str =
   let rec check n =
      let name = if n > 0 then sprintf "%s%i" str n else str
      if not (exists name)
      then name
      else check (n+1)
   check 0

let mutable private cache = Dictionary<System.Type, string>()
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
   if cache.ContainsKey typ then cache.[typ]
   else
      let name =
         HttpUtility.JavaScriptStringEncode typ.Name
         |> fun x ->
            let i = x.IndexOf '`'
            if i >= 0 then x.Substring(0, i) else x
         |> preventConflicts (fun x ->
            cache.Values |> Seq.exists (fun x' -> x = x'))
      cache.Add(typ, name)
      name

let getSpace() = " "
let getNewline padding = System.Environment.NewLine + (String.init padding (fun _ -> "\t"))

// TODO
let compressEmittedExpr (jscode: string) = jscode

let removeTrailingZeroes =
   let reg = Regex("(\.\d+?)0+$")
   fun number -> reg.Replace(number, "$1")

let isValidJSVarName =
   let reg = Regex("^[$_a-zA-Z][$_a-zA-Z0-9]*$") // TODO: Improve this regex
   fun s -> reg.IsMatch s

let isJSString =
   let reg = Regex("^'(.*)'$")
   fun s ->
      let m = reg.Match s
      if m.Success then Some m.Groups.[1].Value else None
