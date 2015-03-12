namespace FunScript

open System
open System.Reflection

type JS = ReflectedDefinitionAttribute

[<AttributeUsage(AttributeTargets.Property|||AttributeTargets.Method)>]
type JSEmitAttribute(emit:string) =
   inherit System.Attribute()
   member __.Emit = emit

[<AttributeUsage(AttributeTargets.Property|||AttributeTargets.Method)>]
type JSEmitInlineAttribute(emit:string) =
   inherit System.Attribute()
   member __.Emit = emit