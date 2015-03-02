﻿module (*private*) FunScript.JavaScriptNameMapper

open System.Reflection

// ES5/ES6 reserved words plus undefined
let unsafeWords =
   set [
        "do"
        "if"
        "in"
        "for"
        "let"
        "new"
        "try"
        "var"
        "case"
        "else"
        "enum"
        "eval"
        "null"
        "this"
        "true"
        "void"
        "with"
        "break"
        "catch"
        "class"
        "const"
        "false"
        "super"
        "throw"
        "while"
        "yield"
        "delete"
        "export"
        "import"
        "public"
        "return"
        "static"
        "switch"
        "typeof"
        "default"
        "extends"
        "finally"
        "package"
        "private"
        "continue"
        "debugger"
        "function"
        "arguments"
        "interface"
        "protected"
        "implements"
        "instanceof"
        "undefined"
   ]

let filterUnsafe str =
   if unsafeWords.Contains str then
      "_" + str
   else str

let sanitizeAux(str:string) =
   str |> Seq.map (function
      | c when (c >= 'a' && c <= 'z') || 
               (c >= '0' && c <= '9') ||
               (c >= 'A' && c <= 'Z') ||
               c = '_' -> c
      | _ -> '_')
   |> Seq.toArray
   |> fun chars -> System.String(chars)
   |> filterUnsafe


let sanitize =
    let replacements = ref Map.empty
    let used = ref Set.empty
    fun key str ->
        let replacement = lazy sanitizeAux str
        // This is for when multiple unsafe strings map onto the
        // same safe string.
        let rec sanitize n =
            match !replacements |> Map.tryFind key with
            | Some replacement -> replacement
            | None ->
                let replacement =
                    match n with
                    | 0 -> replacement.Value
                    | n -> sprintf "%s%i" replacement.Value n
                if !used |> Set.contains replacement then
                    sanitize (n+1)
                else
                    used := !used |> Set.add replacement
                    replacements := !replacements |> Map.add key replacement
                    replacement
        sanitize 0

let rec private getBestTypeName (t : System.Type) =
   let args =
      if t.IsGenericType || t.IsGenericTypeDefinition
      then t.GetGenericArguments()
      else [||]
   t.Name + "_" + (args |> Seq.map mapType |> String.concat "_")

and (*internal*) mapType (t : System.Type) =
   sanitize t.FullName (getBestTypeName t)

let getConstructorIndex (mb: MethodBase) =
    mb.DeclaringType.GetConstructors (BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance)
    |> Array.findIndex (fun ci -> ci.Equals(mb))

let getBestMethodName (mb : MethodBase) =
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