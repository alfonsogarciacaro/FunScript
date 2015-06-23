namespace FunScript
open System

[<AttributeUsage(AttributeTargets.Property|||AttributeTargets.Method)>]
type JSEmitAttribute(emit: string) =
   inherit Attribute()
   member __.Emit = emit

[<AttributeUsage(AttributeTargets.Property|||AttributeTargets.Method)>]
type JSEmitInlineAttribute(emit: string) =
   inherit Attribute()
   member __.Emit = emit

[<AttributeUsage(AttributeTargets.Class)>]
type JSMaskAttribute(proto: string) =
   inherit Attribute()
   member __.Prototype = proto

[<AttributeUsage(AttributeTargets.Interface)>]
type JSClassAttribute() =
   inherit Attribute()