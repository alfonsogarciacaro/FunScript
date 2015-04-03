module internal FunScript.AST

open JSMapper
open System
open System.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Reflection

type JSRef =
   | Var of Var
   | Type of Type
   | Method of MethodBase
   | Case of UnionCaseInfo

// ATTENTION: if new cases are added to JSExpr, FSCompiler.traverseJsi may need to be updated
and JSExpr =
   | Null
   | Undefined
   | Boolean of bool
   | Number of float
   | Integer of int
   | String of string
   | Object of (string * JSExpr) list
   | PropertyGet of JSExpr * JSExpr
   | Array of JSExpr list
   | Apply of JSExpr * JSExpr list
   | Reference of JSRef
   | Coerce of Type * Type * JSExpr
   | New of JSExpr * JSExpr list
   | Lambda of Var list * JSStatement
   | UnaryOp of string * JSExpr
   | BinaryOp of JSExpr * string * JSExpr
   | EmitExpr of string * JSExpr list
   member value.Print pad scope =
      let sp = getSpace()
      let printArgs (args: JSExpr list) =
         args |> List.map (fun a -> a.Print pad scope) |> String.concat ("," + sp)
      match value with
      | Reference ref ->
         match ref with
         | Method meth -> sprintf "'%s'" (mapMethod meth)
         | Case uci -> sprintf "'%s'" (mapCase uci)
         | Var var ->
            if var.Name = "this" then "this" else Map.find var scope
         | Type t ->
            if t.IsGenericParameter then sprintf "$%s" t.Name // findGenericVarName scope t
            elif t.IsInterface then sprintf "'%s'" (mapType t)
            else sprintf "ns['%s']" (mapType t)

      | Coerce (_,_,e) ->
         e.Print pad scope

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
               | [] -> failwithf "JSEmit \"%s\" is being called without argument %s" template pattern
               | arg::args -> code.Replace(pattern, arg.Print pad scope), args
            else
               code, args
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
   | TryCatch of JSStatement * Var * JSStatement
   | TryFinally of JSStatement * JSStatement
   | TryCatchFinally of JSStatement * Var * JSStatement * JSStatement

   // DebugInfo
   | Do of DebugInfo * JSExpr
   | Return of DebugInfo * JSExpr
   | Throw of DebugInfo * JSExpr
   | Assign of DebugInfo * JSExpr * JSExpr
   | AssignGlobal of JSExpr * JSExpr
   | Let of DebugInfo * Var * JSExpr * JSStatement
   | IfThenElse of DebugInfo * JSExpr * JSStatement * JSStatement
   | WhileLoop of DebugInfo * JSExpr * JSStatement
   | ForLoop of DebugInfo * Var * JSExpr * JSExpr * JSStatement
   member statement.PrintGlobal() = statement.Print 0 Map.empty
   member statement.Print pad scope =
      let sp, newL, newL' = getSpace(), getNewline pad, getNewline (pad + 1)
      match statement with
      | Empty -> ""

      | Sequential (first, second) ->
         match first with
         | Empty -> second.Print pad scope
         | AssignGlobal _ -> sprintf "%s%s" (first.Print pad scope) (second.Print pad scope)
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
         sprintf "%s" (expr.Print pad scope)
      
      | Return(_, expr) ->
         sprintf "return %s" (expr.Print pad scope)
      
      | Throw(_, valExpr) ->
         sprintf "throw(%s)" (valExpr.Print pad scope)
      
      | Assign(_, varExpr, valExpr) ->
         sprintf "%s%s=%s%s" (varExpr.Print pad scope) sp sp (valExpr.Print pad scope)
      
      | AssignGlobal(varExpr, valExpr) ->
         sprintf "%s%s=%s%s;%s" (varExpr.Print pad scope) sp sp (valExpr.Print pad scope) newL
      
      | Let(_, var, assignment, body) ->
         let newScope, name = mapVar scope var
         sprintf "var %s%s=%s%s;%s%s" name sp sp (assignment.Print pad scope) newL (body.Print pad newScope)

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
      
      | ForLoop(_, var, fromExpr, toExpr, block) ->
         let newScope, name = mapVar scope var
         sprintf "for(var %s=%s;%s<=%s;%s++)%s{%s%s%s}"
            name (fromExpr.Print pad scope)
            name (toExpr.Print pad scope) name
            sp newL' (block.Print (pad + 1) newScope) newL

type JSInstruction =
   | Expr of JSExpr
   | Statement of JSStatement

let buildExpr = JSInstruction.Expr >> Some
let buildStatement = JSInstruction.Statement >> Some

let refVar v = Reference(Var v)

let refType t =
   if   t = typeof<bool> then String("boolean")
   elif t = typeof<string> || t = typeof<char> then String("string")
   elif t.IsPrimitive then String("number")
   elif t = typeof<obj> then String("object")
   elif t.IsArray || t.Name.StartsWith("Tuple") then String("Array") // TODO TODO TODO
   else
      // Generic types shouldn't get to this point, but make the check just in case
      // (Note: it's possible that generic types arguments of other types reach here, like List<IEnumerable<int>>)
      let t = if t.IsGenericType then t.GetGenericTypeDefinition() else t
      let cis = t.GetConstructors(BindingFlags.All)
      if cis.Length > 0 then
         match cis.[0].TryGetAttribute<JSEmitInlineAttribute>() with
         | Some att -> EmitExpr(att.Emit, [])
         | None -> Reference(Type t)
      else
         Reference(Type t)

let refMethodCall(meth: MethodBase, target: JSExpr option) =
   if meth.DeclaringType.IsInterface then
      match target with
      | None -> failwith "Please report: Interface method referred as static"
      | Some target -> PropertyGet(
                        PropertyGet(
                           PropertyGet(target, String "constructor"),
                           refType meth.DeclaringType),
                        Reference(Method meth))
   else
      PropertyGet(refType meth.DeclaringType, Reference(Method meth))

let refMethodDef(meth: MethodBase, typ: Type ) =
   if meth.DeclaringType.IsInterface then
      PropertyGet(PropertyGet(refType typ, refType meth.DeclaringType),
                  Reference(Method meth))
   else
      PropertyGet(refType typ, Reference(Method meth))
