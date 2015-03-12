module internal FunScript.InternalCompiler

open AST
open Microsoft.FSharp.Quotations

type ReturnStrategy =
   | Inplace    
   | ReturnFrom
   member x.Return(dinfo: DebugInfo, e: JSExpr) =
      match x with
      | Inplace -> Do(dinfo, e)
      | ReturnFrom -> Return(dinfo, e)

type CompilerComponent = Compiler -> ReturnStrategy -> Expr -> JSInstruction option

and Compiler(components: CompilerComponent list) as this = 
   let compile ret expr =
      try
         components |> List.pick (fun part -> part this ret expr)
      with
      | _ -> failwithf "Cannot compile %A. Check if FunScript.JS attribute is missing. %s"
                       expr "If that doesn't help, please report the issue."

   let nextId = ref 0
   member __.NextTempVar() = 
      incr nextId
      Var(sprintf "_%i" !nextId, typeof<obj>, false) 

   member __.CompileStatement ret expr = 
      match compile ret expr with
      | Statement jsSt -> jsSt
      | Expr jsEx -> ret.Return(expr.DebugInfo, jsEx)

   member __.CompileExpr expr = 
      match compile ReturnFrom expr with
      | Expr jsEx -> jsEx
      | Statement jsSt -> Apply(Lambda([], jsSt), [])

   // TODO: This only accepts static method calls at the moment,
   // extend to instance methods and properties. Add DebugInfo?
   member __.CompileCall quote args =
      try
         match quote with
         | DerivedPatterns.Lambdas(_, Patterns.Call(_,mi,_)) ->
            let args = List.map __.CompileExpr args
            Apply(refMethodCall(mi, None), args)
         | _ -> failwith "Expecting a lambda with a static method call"
      with
      | ex -> failwith ex.Message


let (|CompileExpr|)      (compiler: Compiler) = compiler.CompileExpr
let (|CompileStatement|) (compiler: Compiler) = compiler.CompileStatement
