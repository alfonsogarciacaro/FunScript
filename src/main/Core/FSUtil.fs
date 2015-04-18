[<FunScript.JS>]
module internal FunScript.Core.FSUtil
open FunScript

// From http://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex
[<JSEmit(@"return {0}.replace(/[\-\[\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, '\\$&')")>]
let escapeRegex(s: string): string = failwith "never"
[<JSEmit(@"return {0}.replace(/\\([\-\[\/\{\}\(\)\*\+\?\.\\\^\$\|])/g, '$1')")>]
let unescapeRegex(s: string): string = failwith "never"

[<JSEmitInline("JSON.parse({0})")>]
let parseJSON(s: string) = failwith "never"
[<JSEmitInline("JSON.stringify({0})")>]
let stringifyJSON(o: obj) = failwith "never"