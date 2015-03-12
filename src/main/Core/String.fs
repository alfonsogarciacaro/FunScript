namespace FunScript.Core
open FunScript

[<JS; Sealed; CompiledName("FSString")>]
type String =
   [<JSEmitInline("''")>]
   static member Empty: string = ""

   [<JSEmit("""return {0}.replace(/\{(\d+)(,-?\d+)?(?:\:(.+?))?\}/g, function(match, number, alignment, format) {
   var rep = match;
   if ({1}[number] !== undefined) {
      rep = {1}[number];
      if (format !== undefined) {
            if (typeof rep === 'number') {            
               switch (format.substring(0,1)) {
                  case "f": case "F": return format.length > 1 ? rep.toFixed(format.substring(1)) : rep.toFixed(2);
                  case "g": case "G": return format.length > 1 ? rep.toPrecision(format.substring(1)) : rep.toPrecision();
                  case "e": case "E": return format.length > 1 ? rep.toExponential(format.substring(1)) : rep.toExponential();
                  case "p": case "P": return (format.length > 1 ? (rep * 100).toFixed(format.substring(1)) : (rep * 100).toFixed(2)) + " %";
               }                
            }
            else if (rep instanceof Date) {
               if (format.length === 1) {
                  switch (format) {
                        case "D": return rep.toDateString();
                        case "T": return rep.toLocaleTimeString();
                        case "d": return rep.toLocaleDateString();
                        case "t": return rep.toLocaleTimeString().replace(/:\d\d(?!:)/, '');
                  }        
               }
               return format.replace(/(\w)\1*/g, function (match2) {
                  var rep2 = match2;
                  switch (match2.substring(0,1)) {
                        case "y": rep2 = match2.length < 4 ? rep.getFullYear() % 100 : rep.getFullYear(); break;
                        case "h": rep2 = rep.getHours() > 12 ? rep.getHours() % 12 : rep.getHours();      break;
                        case "M": rep2 = rep.getMonth() + 1; break;
                        case "d": rep2 = rep.getDate();      break;
                        case "H": rep2 = rep.getHours();     break;
                        case "m": rep2 = rep.getMinutes();   break;
                        case "s": rep2 = rep.getSeconds();   break;
                  }
                  if (rep2 !== match2 && rep2 < 10 && match2.length > 1) { rep2 = "0" + rep2; }
                  return rep2;
               })                
            }
      }
   }
   return rep;
   })""")>]
   static member Format(s: string, [<System.ParamArray>] args: obj[]): string = failwith "never"

   [<JSEmitInline("{0}.indexOf({1})")>]
   static member IndexOf(s:string, search: string): int = failwith "never"      

   [<JSEmitInline("{0}.indexOf({1}, {2})")>]
   static member IndexOf(s:string, search:string, offset:int): int = failwith "never"

   [<JSEmitInline("({0}.indexOf({1})>=0))")>]
   static member Contains(s:string): int = failwith "never"

   [<JSEmitInline("{0}.lastIndexOf({1})")>]
   static member LastIndexOf(s: string, search: string): int = failwith "never"

   [<JSEmitInline("{0}.lastIndexOf({1}, {2})")>]
   static member LastIndexOf(s: string, search: string, offset:int): int = failwith "never"

   [<JSEmitInline("{0}.trim()")>]
   static member Trim(s: string): string = failwith "never"

   static member StartsWith(s, search) =
       String.IndexOf(s, search) = 0

   static member EndsWith(s: string, search: string) =
       let offset = s.Length - search.Length
       let index = String.IndexOf(s, search, offset)
       index <> -1 && index = offset

   [<JSEmitInline("{0}.toLowerCase()")>]
   static member ToLower(s:string): string = failwith "never"

   [<JSEmitInline("{0}.toUpperCase()")>]
   static member ToUpper(s:string): string = failwith "never"

   [<JSEmitInline("({0}==null)||({0}=='')")>]
   static member IsNullOrEmpty(s:string): bool = failwith "never"

   [<JSEmitInline("{0}.length")>]
   static member Length(s:string): int = failwith "never"

   [<JSEmitInline("{0}.charAt({1})")>]
   static member Item(s:string, length:int): char = failwith "never"

   [<JSEmitInline("{0}.substr({1},{2})")>]
   static member Substring(s:string, offset:int, length:int): string = failwith "never"

   [<JSEmitInline("{0}.substr({1})")>]
   static member Substring(s:string, offset:int): string = failwith "never"

   [<JSEmit("return {0}.split(new RegExp({1}.map({0}).join('|')))")>]
   static member SplitImpl(s:string, delimiter:string[], regexEscape: string->string): string[] = failwith "never"

   static member Split(s:string, delimiters:string[]): string[] =
      String.SplitImpl(s, delimiters, FSUtil.escapeRegex)
  
   static member Split(s, delimiters: string[], opts) =
      String.SplitImpl(s, delimiters, FSUtil.escapeRegex)
      |> Array.filter (fun inp ->
         match opts with
         | System.StringSplitOptions.RemoveEmptyEntries -> inp <> ""
         | _ -> true)

   [<JSEmitInline("{1}.join({0})")>]
   static member Join(separator:string, s:string[]): string = failwith "never"

   [<JSEmitInline("{1}.replace({0})")>]
   static member Replace(s:string, search:string, replace:string): string = failwith "never"

   [<JSEmitInline("{0}")>]
   static member ToCharArray(str:string): char[] = failwith "never"


// Re-implementation of functions from Microsoft.FSharp.Core.StringModule
[<JS; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module String =
   [<JSEmit("""var reg = /%[+\-* ]?\d*(?:\.(\d+))?(\w)/;
       function formatToString(rep) {
           {0} = {0}.replace(reg, function(match, precision, format) {
               switch (format) {
                   case "f": case "F": return precision ? rep.toFixed(precision) : rep.toFixed(6);
                   case "g": case "G": return rep.toPrecision(precision);
                   case "e": case "E": return rep.toExponential(precision);
                   case "A": return JSON.stringify(rep);
                   default:  return rep;
               }
           });
           return reg.test({0}) ? formatToString : {0};
       }
       return formatToString""")>]
   let PrintFormatToString(s: string): obj = failwith "never"

   [<JSEmit("return {0}==null?\"\":{0};")>]
   let private emptyIfNull (str:string) : string = failwith "never"

   [<JSEmitInline("{1}.join({0})")>]
   let private join(separator:string, s:string[]) : string = failwith "never"

   let concat sep (strings : seq<string>) =  
      join(sep, Array.ofSeq strings)

   let iter (f : (char -> unit)) (str:string) =
      let str = emptyIfNull str
      for i = 0 to str.Length - 1 do
            f str.[i] 

   let iteri f (str:string) =
      let str = emptyIfNull str
      for i = 0 to str.Length - 1 do
            f i str.[i] 

   let map (f: char -> char) (str:string) =
      let str = emptyIfNull str
      str.ToCharArray() |> Array.map (fun c -> (f c).ToString()) |> concat ""

   let mapi (f: int -> char -> char) (str:string) =
      let str = emptyIfNull str
      str.ToCharArray() |> Array.mapi (fun i c -> (f i c).ToString()) |> concat ""

   let collect (f: char -> string) (str:string) =
      let str = emptyIfNull str
      str.ToCharArray() |> Array.map f |> concat ""

   let init (count:int) (initializer: int-> string) =
      if count < 0 then invalidArg "count" "String length must be non-negative"
      Array.init count initializer |> concat ""

   let replicate (count:int) (str:string) =
      if count < 0 then  invalidArg "count" "String length must be non-negative"
      let str = emptyIfNull str
      init count (fun _ -> str)

   let forall f (str:string) =
      let str = emptyIfNull str
      let rec check i = (i >= str.Length) || (f str.[i] && check (i+1)) 
      check 0

   let exists f (str:string) =
      let str = emptyIfNull str
      let rec check i = (i < str.Length) && (f str.[i] || check (i+1)) 
      check 0  

   let length (str:string) =
      let str = emptyIfNull str
      str.Length
