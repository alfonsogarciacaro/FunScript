module FunScript.Exceptions

open System
open System.Reflection

type ReflectedDefinition(mb: MethodBase) =
    inherit Exception(
        "No replacement for " + mb.Name + ": " +
        "either ReflectedDefinitionAttribute is missing or " +
        "the method is not yet implemented in FunScript.")

type Inheritance(t: Type) =
    inherit Exception(
        t.Name + " constructor is being called from another constructor." +
        "Funscript only supports interface inheritance.")

type StaticMutableProperty(pi: PropertyInfo) =
    inherit Exception(
        "FunScript doesn't support static mutable properties: " +
        pi.DeclaringType.FullName + pi.Name)

type StaticField(fi: FieldInfo) =
    inherit Exception(
        "FunScript doesn't support static fields: " +
        fi.DeclaringType.FullName + fi.Name)