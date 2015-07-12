module FunScript.AST

open JSMapper
open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.Ast
open Microsoft.FSharp.Compiler.SourceCodeServices

type FSVal = FSharpMemberOrFunctionOrValue
type Range = Range.range
type Dictionary<'k,'v> = System.Collections.Generic.Dictionary<'k,'v>

type JSExpr =
   | Null
   | Undefined
   | Boolean of bool
   | Number of float
   | Integer of int
   | String of string
   | Var of FSVal
   | Super
   | This
   /// Do not use this constructor directly. Call ICompiler.RefType instead.
   | Type of FSharpType
   | Object of (string * JSExpr) list
   | PropertyGet of JSExpr * JSExpr
   | Array of JSExpr list
   | Apply of JSExpr * JSExpr list
   | New of JSExpr * JSExpr list
   | Lambda of args: FSVal list * body: JSInstruction * isGenerator: bool
   | UnaryOp of string * JSExpr
   | BinaryOp of JSExpr * string * JSExpr
   | EmitExpr of string * JSExpr list

   member value.Print pad (scope: System.Collections.Generic.Dictionary<FSVal,string>) =
      let sp = getSpace()
      let printArgs (args: JSExpr list) =
         args |> List.map (fun a -> a.Print pad scope) |> String.concat ("," + sp)
      match value with
      | Var var ->
         match var.CompiledName with
         // In private methods defined with let, `_this` is passed as argument but then `this` is used as variable
         | "this" -> scope |> Seq.tryFind (fun kv -> kv.Value = "_this" && kv.Key.FullType = var.FullType)
                           |> function Some _ -> "_this" | None -> "this"
         | _ -> if scope.ContainsKey(var) then scope.[var] else failwithf "Var %s not found in scope" var.CompiledName

      | Type t ->
         if t.IsGenericParameter then sprintf "$%s" t.TypeDefinition.CompiledName
         elif t.TypeDefinition.IsInterface then "'" + (mapType t) + "'"
         else let tname = mapType t
              if isValidJSVarName tname
              then "ns."  + tname
              else "ns['" + tname + "']"

      | New (expr, args) ->
         sprintf "new %s(%s)" (expr.Print pad scope) (printArgs args)

      | Null         -> "null"
      | Undefined    -> "undefined"
      | Boolean b    -> b.ToString().ToLower()
      | Integer i    -> sprintf "%d" i
      | Number f     -> removeTrailingZeroes(sprintf "%f" f)
      | String str   -> sprintf "'%s'" (System.Web.HttpUtility.JavaScriptStringEncode str)

      | Object propExprs ->
         let filling =
            propExprs |> List.map (fun (name, expr) ->
               sprintf "%s:%s" name (expr.Print pad scope))
            |> String.concat ("," + sp)
         sprintf "{%s}" filling

      | PropertyGet(objExpr, propExpr) ->
         match propExpr with
         | String str when isValidJSVarName str ->
            sprintf "%s.%s" (objExpr.Print pad scope) str
         | _ ->
            let prop = propExpr.Print pad scope
            match isJSString prop with
            | Some str when isValidJSVarName str ->
               sprintf "%s.%s" (objExpr.Print pad scope) str
            | _ ->
               sprintf "%s[%s]" (objExpr.Print pad scope) (propExpr.Print pad scope)

      | Array exprs -> 
         sprintf "[%s]" (printArgs exprs)

      | Apply(lambdaExpr, argExprs) ->
         sprintf "%s(%s)" (lambdaExpr.Print pad scope) (printArgs argExprs)

      | Lambda(vars, block) ->
         let newL, newL' = getNewline pad, getNewline (pad + 1)
         let newScope, names =
            vars |> List.fold (fun (scope, names) var ->
               let scope, name = mapVar scope var
               scope, name::names) (scope, [])
         sprintf "(function(%s)%s{%s})" (String.concat ", " (List.rev names)) sp  // Names got reversed while folding
            (match block with Empty -> "" | _-> newL' + (block.Print (pad + 1) newScope) + newL)

      | UnaryOp(symbol, expr) ->
         sprintf "%s%s" symbol (expr.Print pad scope)

      | BinaryOp(lhsExpr, symbol, rhsExpr) ->
         sprintf "(%s%s%s%s%s)" (lhsExpr.Print pad scope) sp symbol sp (rhsExpr.Print pad scope)

      | EmitExpr (template, args) ->
         let code = compressEmittedExpr template
         let replace (code: string) (args: JSExpr list) (i: int) =
            let pattern = sprintf "{%i}" i
            if code.Contains pattern then
               match args with
               | [] -> failwithf "EmitExpr \"%s\" called without arg%s" template pattern
               | arg::args -> code.Replace(pattern, arg.Print pad scope), args
            else
               match args with [] -> code, [] | arg::args -> code, args
         let rec replaceRec code args i =
               match args with
               | [] -> code
               | _ -> let code, args = replace code args i
                      replaceRec code args (i + 1)
         // Even when using {args}, most of the times we'll use {0} to refer the owner of the method
         let code, args = replace code args 0
         if code.Contains "{args}"
         then args |> List.map (fun a -> a.Print pad scope)
                   |> String.concat ("," + sp)
                   |> fun args -> code.Replace("{args}", args)
         else replaceRec code args 1

and JSStatement =
   // No DebugInfo
   | Empty
   | Sequential of JSStatement * JSStatement
   | TryCatch of JSStatement * FSVal * JSStatement
   | TryFinally of JSStatement * JSStatement
   | TryCatchFinally of JSStatement * FSVal * JSStatement * JSStatement

   // DebugInfo
   | Do of Range * JSExpr
   | Return of Range * JSExpr
   | Throw of Range * JSExpr
   | Assign of Range * JSExpr * JSExpr
   | Let of Range * FSVal * JSExpr * JSStatement
   | IfThenElse of Range * JSExpr * JSStatement * JSStatement
   | WhileLoop of Range * JSExpr * JSStatement
   | ForIntegerLoop of Range * var: FSVal * beginning: JSExpr * ending: JSExpr * body: JSStatement * isUp: bool
   | ForOfLoop of Range * var: FSVal * iterable: JSExpr * body: JSStatement
   member statement.Print pad scope =
      let sp, newL, newL' = getSpace(), getNewline pad, getNewline (pad + 1)
      match statement with
      | Empty -> ""

      | Sequential (first, second) ->
         match first with
         | Empty -> second.Print pad scope
         | _ ->
            match second with
            | Empty -> first.Print pad scope
            | _ -> sprintf "%s;%s%s" (first.Print pad scope) newL (second.Print pad scope)
      
      | TryCatch(tryExpr, var, catchExpr) ->
         let newScope, name = mapVar scope var
         sprintf "try%s{%s%s%s}%scatch(%s)%s{%s%s%s}"
            sp newL' (tryExpr.Print (pad + 1) scope) newL newL name
            sp newL' (catchExpr.Print (pad + 1) newScope) newL
      
      | TryFinally(tryExpr, finallyExpr) ->
         sprintf "try%s{%s%s%s}%sfinally%s{%s%s%s}"
            sp newL' (tryExpr.Print (pad + 1) scope) newL newL
            sp newL' (finallyExpr.Print (pad + 1) scope) newL
      
      | TryCatchFinally(tryExpr, var, catchExpr, finallyExpr) ->
         let newScope, name = mapVar scope var
         sprintf "try%s{%s%s%s}%scatch(%s)%s{%s%s%s}%sfinally%s{%s%s%s}"
            sp newL' (tryExpr.Print (pad + 1) scope) newL newL name
            sp newL' (catchExpr.Print (pad + 1) newScope) newL newL
            sp newL' (finallyExpr.Print (pad + 1) scope) newL
      
      | Do(_, expr) ->
         expr.Print pad scope
      
      | Return(_, expr) ->
         "return " + (expr.Print pad scope)
      
      | Throw(_, valExpr) ->
         sprintf "throw(%s)" (valExpr.Print pad scope)
      
      | Assign(_, varExpr, valExpr) ->
         (sprintf "%s%s=%s%s" (varExpr.Print pad scope) sp sp (valExpr.Print pad scope)) +
         if (* Global Assign: types, methods... *) pad = 0 then ";" + newL else ""
            
      | Let(_, var, assignment, body) ->
         let scope, name = mapVar scope var
         sprintf "var %s%s=%s%s;%s%s" name sp sp (assignment.Print pad scope) newL (body.Print pad scope)

      | IfThenElse(_, cond, trueBlock, falseBlock) ->
         let cond' = cond.Print pad scope
         (+) (sprintf "if%s%s{%s%s%s}"
               (match cond with BinaryOp _ -> cond' | _ -> "("+cond'+")")
               sp newL' (trueBlock.Print  (pad + 1) scope) newL)
             (match falseBlock with
              | Do(_, Undefined) -> ""
              | IfThenElse _ -> sprintf "%selse %s" newL (falseBlock.Print pad scope)
              | _ -> sprintf "%selse%s{%s%s%s}" newL sp newL' (falseBlock.Print (pad + 1) scope) newL)
      
      | WhileLoop(_, cond, block) ->
         let cond' = cond.Print pad scope
         sprintf "while(%s)%s{%s%s%s}"
            (match cond with BinaryOp _ -> cond' | _ -> "("+cond'+")")
            sp newL' (block.Print (pad + 1) scope) newL
      
      | ForIntegerLoop(_, var, fromExpr, toExpr, block) ->
         let newScope, name = mapVar scope var
         sprintf "for(var %s=%s;%s<=%s;%s++)%s{%s%s%s}"
            name (fromExpr.Print pad scope)
            name (toExpr.Print pad scope) name
            sp newL' (block.Print (pad + 1) newScope) newL

and JSInstruction =
   | Expr of JSExpr
   | Statement of JSStatement

type ReturnStrategy =
   | Inplace    
   | ReturnFrom
   member x.Return(dinfo: Range, e: JSExpr) =
      match x with
      | Inplace -> Do(dinfo, e)
      | ReturnFrom -> Return(dinfo, e)

type IMappings =
  abstract member Fields: Dictionary<FSharpField, string>
  abstract member Methods: Dictionary<FSVal, string>

type IScopeInfo =
  abstract member ReplaceIfNeeded: FSVal -> FSVal

type ICompiler =
   abstract member GetMappings: FSharpEntity -> IMappings

   abstract member CompileCall: IScopeInfo -> FSharpExpr option -> FSVal -> FSharpExpr list -> JSExpr
   abstract member CompileExpr: IScopeInfo -> FSharpExpr -> JSExpr
   abstract member CompileStatement: IScopeInfo -> FSharpExpr -> JSStatement

   abstract member RefCase: FSharpUnionCase -> JSExpr
   
   // TODO: Check imports
   abstract member RefType: FSharpEntity -> JSExpr

   // TODO: Watch overloads
   abstract member RefMethod: JSExpr option -> FSharpMemberOrFunctionOrValue -> JSExpr

//   abstract member AddInterface: impl:FSharpType * infc:FSharpType -> unit
   abstract member AddReplacement: repl:FSVal -> ICompiler
   abstract member ReplaceIfNeeded: repl:FSVal -> FSVal

type CompilerComponent = ICompiler -> ReturnStrategy -> FSharpExpr -> JSInstruction option

let buildExpr = JSInstruction.Expr >> Some
let buildStatement = JSInstruction.Statement >> Some

let (|CompileExpr|)      (compiler: ICompiler) = compiler.CompileExpr
let (|CompileStatement|) (compiler: ICompiler) = compiler.CompileStatement

