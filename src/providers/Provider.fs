namespace FunScript.Providers.Require

open System.Reflection
open System.Collections.Generic
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Core.CompilerServices
open ProviderImplementation.ProvidedTypes

module Replacements =
   [<FunScript.JSEmitInline("{0}[{1}]")>]
   let get (o: obj) (k: string): obj = failwith "never"

   [<FunScript.JSEmitInline("({0}[{1}] = {2})")>]
   let set (o: obj) (k: string) (v: obj): obj = failwith "never"

   [<FunScript.JSEmitInline("(new {0}({args})")>]
   let create (args: obj[]): obj = failwith "never"

   [<FunScript.JSEmitInline("{0}({args})")>]
   let invoke (o: obj) (args: obj[]): obj = failwith "never"

   [<FunScript.JSEmitInline("require({0})")>]
   let require(moduleName: string): obj = failwith "never"

[<TypeProvider>]
type RequireProviderImpl(config: TypeProviderConfig) as this =
   inherit TypeProviderForNamespaces()

   let namespaceName = "FunScript.Providers"
   let thisAssembly = Assembly.GetExecutingAssembly()

   let getArgs =
      let reg = System.Text.RegularExpressions.Regex("\w+")
      fun (f: obj) ->
         reg.Matches((f :?> string).Substring(8))
         |> Seq.cast<System.Text.RegularExpressions.Match>
         |> Seq.map (fun m -> ProvidedParameter(m.Value, typeof<obj>))
         |> Seq.toList

   let parseProp name t =
      ProvidedProperty(
         propertyName = name,
         propertyType = t, 
         GetterCode = (fun args -> <@@ Replacements.get (%%(args.[0]): obj) name @@>),
         SetterCode = (fun args -> <@@ Replacements.set (%%(args.[0]): obj) name (%%(args.[1]): obj) @@>))

   let parseMethod name signature =
      ProvidedMethod(
         methodName = name, 
         parameters = getArgs signature,
         returnType = typeof<obj>, 
         InvokeCode = fun args ->
            <@@  Replacements.invoke (Replacements.get (%%(args.[0]): obj) name) %%(Expr.NewArray(typeof<obj>, args)) @@>) 

   let rec parseType (name: string) (tcount: int) (props: IDictionary<string, obj>) =
      let typ =
         if name.StartsWith("ReqAnonymous")
         then ProvidedTypeDefinition(name, Some typeof<obj>)
         else ProvidedTypeDefinition(thisAssembly, namespaceName, name, Some typeof<obj>)
      props |> Seq.iter (fun kv ->
         match kv.Value with
         | :? string as jstype ->
            if jstype.StartsWith("function") then
               typ.AddMember(parseMethod kv.Key kv.Value)
            else
               let t =
                  if Literals.typeMappings.ContainsKey(jstype)
                  then Literals.typeMappings.[jstype]
                  else typeof<obj>
               typ.AddMember(parseProp kv.Key t)
         | :? IDictionary<string, obj> as dic ->
            let name, tcount = "ReqAnonymous" + (string tcount), tcount + 1
            let typ' = if dic.ContainsKey("constructor") then parseConstructor name tcount dic else parseType name tcount dic
            typ.AddMember typ'
            this.AddNamespace(namespaceName, [typ'])
            //typ.AddMember(parseProp kv.Key typ')
         | _ as x -> failwith <| "Expected dictionary or string, but got: " + (string x.GetType)) 
      typ

   and parseConstructor name (tcount: int) (dic: IDictionary<string, obj>) =
      let instanceName, tcount = "ReqAnonymous" + (string tcount), tcount + 1
      let instanceType = parseType instanceName tcount (dic.["instance"] :?> IDictionary<string, obj>)
      let staticType = parseType name tcount (dic.["static"] :?> IDictionary<string, obj>)
      //staticType.AddMember instanceType
      this.AddNamespace(namespaceName, [instanceType])
      staticType.AddMember <| ProvidedMethod(
                                 methodName = "Create", 
                                 parameters = getArgs dic.["constructor"], 
                                 returnType = instanceType, 
                                 InvokeCode = fun args ->
                                    <@@ Replacements.create %%(Expr.NewArray(typeof<obj>, args)) @@>)
      staticType

   let staticParams = [ProvidedStaticParameter("nodeModule", typeof<string>)]
   let reqType = ProvidedTypeDefinition(thisAssembly, namespaceName, "RequireProvider", baseType = Some typeof<obj>)

   do reqType.DefineStaticParameters(staticParams, fun typeName parameterValues ->
      match parameterValues with
      | [| :? string as nodeModule |] ->
         EdgeJs.Edge.Func(Literals.jsCode).Invoke(nodeModule)
         |> Async.AwaitTask
         |> Async.RunSynchronously
         |> function
            | :? IDictionary<string, obj> as res ->
               let typ = if res.ContainsKey("constructor") then parseConstructor typeName 0 res else parseType typeName 0 res
               typ.AddMember <| ProvidedConstructor(
                                 parameters = [],
                                 InvokeCode = fun args -> <@@ Replacements.require(nodeModule) @@>)
               typ 
            | _ as x -> failwith <| "Expected dictionary from Edge, but got: " + (string x.GetType)
      | _ -> failwith "Unexpected parameter values")

   do this.AddNamespace(namespaceName, [reqType])

[<assembly:TypeProviderAssembly>]
do()


