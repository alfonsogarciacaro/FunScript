module (*private*) FunScript.JavaScriptNameMapper

open System.Reflection
open System.Text.RegularExpressions

let keywords =
   set [
      "arguments"
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
      // TODO: It is hard to protect against "this" being used incorrectly
      //"this"
      "throw"
      "try"
      "typeof"
      "var"
      "void"
      "while"
      "with"
   ]

let reservedWords =
   set [
      "abstract"
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

let unsafeWords = keywords + reservedWords

type Replacements() =
   let replacements = ref Map.empty
   let used = ref Set.empty
   member x.ContainsReplacement replacement =
      !used |> Set.contains replacement
   member x.Add key replacement =
      used := !used |> Set.add replacement
      replacements := !replacements |> Map.add key replacement
      replacement  
   member x.TryFind key =
      !replacements |> Map.tryFind key

let private replacements = ref Unchecked.defaultof<Replacements>
let reset() = replacements := Replacements()

let sanitizeAux =
   let regex = Regex "[^0-9a-zA-Z$_]"
   fun str ->
      let str = regex.Replace(str, "_")
      if unsafeWords.Contains str
      then "_" + str
      else str

let sanitize key str =
   let replacement = lazy sanitizeAux str
   // This is for when multiple unsafe strings map onto the same safe string.
   let rec sanitize n =
      match (!replacements).TryFind key with
      | Some replacement -> replacement
      | None ->
         let replacement =
            match n with
            | 0 -> replacement.Value
            | n -> sprintf "%s%i" replacement.Value n
         if (!replacements).ContainsReplacement replacement
         then sanitize (n+1)
         else (!replacements).Add key replacement
   sanitize 0

let rec private getBestTypeName (t : System.Type) =
   let args =
      if t.IsGenericType || t.IsGenericTypeDefinition then
         t.GetGenericArguments()
      else [||]
   t.Name + "_" + (args |> Seq.map getBestTypeName |> String.concat "_")

let (*internal*) mapType (t : System.Type) =
   sanitize t.FullName (getBestTypeName t)

let getConstructorIndex (mb: MethodBase) =
    mb.DeclaringType.GetConstructors (BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance)
    |> Array.findIndex (fun ci -> ci.Equals(mb))

let private getBestMethodName (mb : MethodBase) =
   // Make sure constructors have same index so primary is always 0 (omitted)
   let suffix =
      if mb.IsConstructor
      then let i = getConstructorIndex mb in if i > 0 then string i else ""
      else ""
   let args =
      if mb.IsGenericMethod || mb.IsGenericMethodDefinition
      then mb.GetGenericArguments()
      else [||]
   (mapType mb.DeclaringType) + "_" + mb.Name + "$" + suffix + (args |> Seq.map mapType |> String.concat "_")

let (*internal*) mapMethod mb =
   let suggestedName = getBestMethodName mb
   let paramKey =
      mb.GetParameters() |> Array.map (fun pi -> pi.ParameterType.Name)
      |> String.concat ","
   let key =
      mb.DeclaringType.FullName + suggestedName + "[" + paramKey + "]"
   sanitize key suggestedName