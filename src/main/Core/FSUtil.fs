[<FunScript.JS>]
module internal FunScript.Core.FSUtil
open FunScript

// From http://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex
[<JSEmit(@"return {0}.replace(/[\-\[\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, '\\$&')")>]
let escapeRegex(s: string): string = failwith "never"
[<JSEmit(@"return {0}.replace(/\\([\-\[\/\{\}\(\)\*\+\?\.\\\^\$\|])/g, '$1')")>]
let unescapeRegex(s: string): string = failwith "never"