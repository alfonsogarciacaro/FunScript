
namespace FunScript.Core
open FunScript

[<JS; Sealed; CompiledName("FSString")>]
type String =
   [<JSEmitInline("''")>]
   static member Empty: string = ""

   [<JSEmitInline("({0}==null)||({0}=='')")>]
   static member IsNullOrEmpty(s:string): bool = failwith "never"

   [<JSEmitInline("{1}.join({0})")>]
   static member Join(separator:string, s:string[]): string = failwith "never"

   [<JSEmit("""var reg = /%[+\-* ]?\d*(?:\.(\d+))?(\w)/;
   function formatToString(pattern) {
    return function(rep) {
       var formatted = pattern.replace(reg, function(match, precision, format) {
       switch (format) {
       case "f": case "F": return precision ? rep.toFixed(precision) : rep.toFixed(6);
       case "g": case "G": return rep.toPrecision(precision);
       case "e": case "E": return rep.toExponential(precision);
       case "A": return JSON.stringify(rep);
       default:  return rep;
       }
       });
       return reg.test(formatted) ? formatToString(formatted) : formatted;
    };
   }
   return formatToString({0})""")>]
   static member internal PrintFormatToString(s: string): obj = failwith "never"

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

   member __.Length with [<JSEmitInline("{0}.length")>] get(): int = failwith "never"

   member __.Item with [<JSEmitInline("{0}.charAt({1})")>] get(i:int): char = failwith "never"

   [<JSEmitInline("{0}.indexOf({1})")>]
   member __.IndexOf(search: string): int = failwith "never"      

   [<JSEmitInline("{0}.indexOf({1}, {2})")>]
   member __.IndexOf(search:string, offset:int): int = failwith "never"

   [<JSEmitInline("({0}.indexOf({1})>=0))")>]
   member __.Contains(s:string): int = failwith "never"

   [<JSEmitInline("{0}.lastIndexOf({1})")>]
   member __.LastIndexOf(search: string): int = failwith "never"

   [<JSEmitInline("{0}.lastIndexOf({1}, {2})")>]
   member __.LastIndexOf(search: string, offset:int): int = failwith "never"

   [<JSEmitInline("{0}.trim()")>]
   member __.Trim(): string = failwith "never"

   [<JSEmitInline("({0}.indexOf({1})===0)")>]
   member __.StartsWith(s, search) = failwith "never"

   member __.EndsWith(search: string) =
      let s: string = unbox __
      let offset = s.Length - search.Length
      let index = s.IndexOf(search, offset)
      index <> -1 && index = offset

   [<JSEmitInline("{0}.toLowerCase()")>]
   member __.ToLower(s:string): string = failwith "never"

   [<JSEmitInline("{0}.toUpperCase()")>]
   member __.ToUpper(s:string): string = failwith "never"

   [<JSEmitInline("{0}.substr({1},{2})")>]
   member __.Substring(offset:int, length:int): string = failwith "never"

   [<JSEmitInline("{0}.substr({1})")>]
   member __.Substring(offset:int): string = failwith "never"

   [<JSEmit("return {0}.split(new RegExp({1}.map({2}).join('|')))")>]
   static member SplitImpl(s: string, delimiter:string[], regexEscape: string->string): string[] = failwith "never"

   // TODO TODO TODO: Implement Split with count
   member __.Split(delimiters: char[]): string[] =
      String.SplitImpl(unbox __, unbox delimiters, FSUtil.escapeRegex)

   member __.Split(delimiters: string[]): string[] =
      String.SplitImpl(unbox __, delimiters, FSUtil.escapeRegex)
  
   member __.Split(delimiters: string[], opts) =
      String.SplitImpl(unbox __, delimiters, FSUtil.escapeRegex)
      |> Collections.Array.filter (fun inp ->
         match opts with
         | System.StringSplitOptions.RemoveEmptyEntries -> inp <> ""
         | _ -> true)

   [<JSEmitInline("{0}.replace({1}, {2})")>]
   member __.Replace(search:string, replace:string): string = failwith "never"

   [<JSEmitInline("{0}")>]
   member __.ToCharArray(str:string): char[] = failwith "never"


// Re-implementation of functions from Microsoft.FSharp.Core.StringModule
[<JS; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module String =
   [<JSEmitInline("({0}===null?'':{0})")>]
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

[<JS; Sealed; CompiledName("FSConsole")>]
type Console =
   [<JSEmitInline("console.log({0})")>]
   static member Log(s: string): unit = failwith "never"

   static member WriteLine(s: string, [<System.ParamArray>] args: obj[]): unit =
      let s = String.Format(s, args)
      Console.Log(s)
