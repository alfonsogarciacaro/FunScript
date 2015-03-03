namespace FunScript

open System
open System.Reflection

type ReflectedDefinitionException(mb: MethodBase) =
   inherit Exception(
      (sprintf "No replacement for %s.%s. " mb.DeclaringType.Name mb.Name) +
      "Either ReflectedDefinitionAttribute is missing or " +
      "the method is not yet implemented in FunScript.")

type InheritanceException(t: Type) =
   inherit Exception(
      t.Name + " constructor is being called from another constructor. " +
      "Funscript only supports interface inheritance.")

