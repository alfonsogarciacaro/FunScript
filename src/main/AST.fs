module internal FunScript.AST

open JSMapper
open System
open System.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Reflection

type JSExpr =
   | Null
   | Undefined
   | Boolean of bool
   | Number of float
   | Integer of int
   | String of string
   | Var of Var
   /// Do not use this constructor directly. Call ICompiler.RefType instead.
   | Type of Type
   | Object of (string * JSExpr) list
   | PropertyGet of JSExpr * JSExpr
   | Array of JSExpr list
   | Apply of JSExpr * JSExpr list
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
      | Var var ->
         match var.Name with
         // In private methods defined with let, `_this` is passed as argument but then `this` is used as variable
         | "this" -> scope |> Map.tryFindKey (fun (k: Var) v -> v = "_this" && k.Type = var.Type)
                           |> function Some _ -> "_this" | None -> "this"
         | _ -> if Map.containsKey var scope then scope.[var] else failwithf "Var %s not found in scope" var.Name

      | Type t ->
         if t.IsGenericParameter then sprintf "$%s" t.Name
         elif t.IsInterface then "'" + (mapType t) + "'"
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

//   member value.Traverse (exprVisitor: JSExpr->'a->'a) (stmentVisitor: JSStatement->'a->'a) (state: 'a) =
//      let state, traverse = exprVisitor value state, fun (e: JSExpr) -> e.Traverse exprVisitor stmentVisitor
//      match value with
//      | Reference _
//      | Null        
//      | Undefined   
//      | Boolean _   
//      | Integer _   
//      | Number _    
//      | String _ -> state 
//      | New (expr, _)
//      | UnaryOp(_, expr)
//      | BinaryOp(exprA, _, exprB)
//      | PropertyGet(exprA, exprB) -> traverse exprA state |> traverse exprB
//      | Array exprs
//      | EmitExpr (_, exprs) -> exprs |> List.fold (fun st e -> traverse e st) state 
//      | Object propExprs -> propExprs |> List.fold (fun st (_, e) -> traverse e st) state
//      | Lambda(_, block) -> block.Traverse exprVisitor stmentVisitor state
//      | Apply(lambdaExpr, argExprs) ->
//         traverse lambdaExpr state |> fun st -> List.fold (fun st e -> traverse e st) st argExprs

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
   | Let of DebugInfo * Var * JSExpr * JSStatement
   | IfThenElse of DebugInfo * JSExpr * JSStatement * JSStatement
   | WhileLoop of DebugInfo * JSExpr * JSStatement
   | ForLoop of DebugInfo * Var * JSExpr * JSExpr * JSStatement
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
      
      | ForLoop(_, var, fromExpr, toExpr, block) ->
         let newScope, name = mapVar scope var
         sprintf "for(var %s=%s;%s<=%s;%s++)%s{%s%s%s}"
            name (fromExpr.Print pad scope)
            name (toExpr.Print pad scope) name
            sp newL' (block.Print (pad + 1) newScope) newL

//   member statement.Traverse (exprVisitor: JSExpr->'a->'a) (stmentVisitor: JSStatement->'a->'a) (state: 'a) =
//      let state = stmentVisitor statement state
//      let travExpr = fun (e: JSExpr) -> e.Traverse exprVisitor stmentVisitor
//      let travStment = fun (s: JSStatement) -> s.Traverse exprVisitor stmentVisitor
//      match statement with
//      | Empty -> state
//      | Throw(_, expr)
//      | Do(_, expr)
//      | Return(_, expr) -> travExpr expr state
//      | Assign(_, exprA, exprB) -> travExpr exprA state |> travExpr exprB
//      | Sequential(stmentA, stmentB)
//      | TryCatch(stmentA, _, stmentB)
//      | TryFinally(stmentA, stmentB) -> travStment stmentA state |> travStment stmentB
//      | TryCatchFinally(tryStment, _, catchStment, finalStment) ->
//         travStment tryStment state |> travStment catchStment |> travStment finalStment
//      | WhileLoop(_, expr, stment)
//      | Let(_, _, expr, stment) -> travExpr expr state |> travStment stment
//      | IfThenElse(_, cond, trueBlock, falseBlock) ->
//         travExpr cond state |> travStment trueBlock |> travStment falseBlock
//      | ForLoop(_, _, fromExpr, toExpr, block) ->
//         travExpr fromExpr state |> travExpr toExpr |> travStment block

type JSInstruction =
   | Expr of JSExpr
   | Statement of JSStatement

type ReturnStrategy =
   | Inplace    
   | ReturnFrom
   member x.Return(dinfo: DebugInfo, e: JSExpr) =
      match x with
      | Inplace -> Do(dinfo, e)
      | ReturnFrom -> Return(dinfo, e)

type ICompiler =
   abstract member TypeMappings: Map<string, Type>

   abstract member CompileExpr: Expr -> JSExpr
   abstract member CompileCall: Expr -> Expr list -> JSExpr
   abstract member CompileStatement: ReturnStrategy -> Expr -> JSStatement

   abstract member RefType: System.Type -> JSExpr
   abstract member RefCase: UnionCaseInfo -> JSExpr
   abstract member RefMethod: MethodBase * JSExpr option -> JSExpr

   abstract member AddInterface: impl:Type * infc:Type -> unit

type CompilerComponent = ICompiler -> ReturnStrategy -> Expr -> JSInstruction option

let buildExpr = JSInstruction.Expr >> Some
let buildStatement = JSInstruction.Statement >> Some

let (|CompileExpr|)      (compiler: ICompiler) = compiler.CompileExpr
let (|CompileStatement|) (compiler: ICompiler) = compiler.CompileStatement

