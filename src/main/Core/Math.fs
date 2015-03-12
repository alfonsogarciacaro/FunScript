namespace FunScript.Core
open FunScript

[<JS; CompiledName("FSMath")>]
type Math =
   [<JSEmitInline("Math.abs({0})")>]
   static member Abs(x: int) = System.Math.Abs(x)

   [<JSEmitInline("Math.abs({0})")>]
   static member Abs(x: float) = System.Math.Abs(x)

   [<JSEmitInline("Math.acos({0})")>]
   static member Acos(x: float) = System.Math.Acos(x)

   [<JSEmitInline("Math.asin({0})")>]
   static member Asin(x: float) = System.Math.Asin(x)

   [<JSEmitInline("Math.atan({0})")>]
   static member Atan(x: float) = System.Math.Atan(x)

   [<JSEmitInline("Math.atan2({0}, {1})")>]
   static member Atan2(x: float, y: float) = System.Math.Atan2(x, y)

   [<JSEmitInline("Math.ceil({0})")>]
   static member Ceiling(x: float) = System.Math.Ceiling(x)

   [<JSEmitInline("Math.cos({0})")>]
   static member Cos(x: float) = System.Math.Cos(x)

   [<JSEmitInline("Math.exp({0})")>]
   static member Exp(x: float) = System.Math.Exp(x)

   [<JSEmitInline("Math.floor({0})")>]
   static member Floor(x: float) = System.Math.Floor(x)

   [<JSEmitInline("Math.log({0})")>]
   static member Log(x: float) = System.Math.Log(x)

   [<JSEmitInline("Math.pow({0}, {1})")>]
   static member Pow(x: float, y: float) = System.Math.Pow(x, y)

   [<JSEmitInline("Math.round({0})")>]
   static member Round(x: float) = System.Math.Round(x)

   [<JSEmitInline("Math.sin({0})")>]
   static member Sin(x: float) = System.Math.Sin(x)

   [<JSEmitInline("Math.sqrt({0})")>]
   static member Sqrt(x: float) = System.Math.Sqrt(x)

   [<JSEmitInline("Math.tan({0})")>]
   static member Tan(x: float) = System.Math.Tan(x)
    
   [<JSEmitInline("(Math.log({0})/Math.LN10)")>]
   static member Log10(x: float) = System.Math.Log10(x)
