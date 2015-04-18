namespace FunScript.Bindings.Node
open FunScript.Bindings.Lib
open FunScript
open FunScript.Core

type Global = interface end

type AddressInfo = interface end

type Agent = interface end

type AnonymousType431 = interface end

type AnonymousType432 = interface end

type AnonymousType433 = interface end

type AnonymousType434 = interface end

type AnonymousType435<'T> = interface end

type AnonymousType436 = interface end

type AnonymousType437 = interface end

type AnonymousType438 = interface end

type AnonymousType439 = interface end

type AnonymousType440 = interface end

type AnonymousType441 = interface end

type AnonymousType442 = interface end

type AnonymousType443 = interface end

type AnonymousType444 = interface end

type AnonymousType445 = interface end

type AnonymousType446 = interface end

type AnonymousType447 = interface end

type AnonymousType448 = interface end

type AnonymousType449 = interface end

type AnonymousType450 = interface end

type AnonymousType451 = interface end

type AnonymousType452 = interface end

type AnonymousType453 = interface end

type AnonymousType454 = interface end

type AnonymousType455 = interface end

type AnonymousType456 = interface end

type AnonymousType457 = interface end

type AnonymousType458 = interface end

type AnonymousType459 = interface end

type AnonymousType460 = interface end

type AnonymousType461 = interface end

type AnonymousType462 = interface end

type AnonymousType463 = interface end

type AnonymousType464 = interface end

type AnonymousType465 = interface end

type AnonymousType466 = interface end

type AnonymousType467 = interface end

type AnonymousType468 = interface end

type AnonymousType469 = interface end

type AnonymousType470 = interface end

type AnonymousType471 = interface end

type AnonymousType472 = interface end

type AnonymousType473 = interface end

type AnonymousType474 = interface end

type AnonymousType475 = interface end

type AnonymousType476 = interface end

type AnonymousType477 = interface end

type AnonymousType478 = interface end

type AnonymousType479 = interface end

type AnonymousType480 = interface end

type AnonymousType481 = interface end

type AnonymousType482<'T> = interface end

type AnonymousType483<'T> = interface end

type AnonymousType484<'T> = interface end

type AnonymousType485 = interface end

type AnonymousType486 = interface end

type AnonymousType487 = interface end

type AssertionError =
        inherit Error

type Cipher = interface end

type ClusterSettings = interface end

type ConnectionOptions = interface end

type Context = interface end

type CredentialDetails = interface end

type Credentials = interface end

type Decipher = interface end

type DiffieHellman = interface end

type ErrnoException =
        inherit Error

type EventEmitter = interface end

type Hash = interface end

type Hmac = interface end

type InspectOptions = interface end

type NodeBuffer = interface end

type NodeStringDecoder = interface end

type ReadLineOptions = interface end

type ReadableOptions = interface end

type RemoteInfo = interface end

type ReplOptions = interface end

type RequestOptions = interface end

type Script = interface end

type SecurePair = interface end

type ServerOptions = interface end

type Signer = interface end

type Stats = interface end

type Timer = interface end

type TlsOptions = interface end

type Url = interface end

type UrlOptions = interface end

type Verify = interface end

type WritableOptions = interface end

type ZlibOptions = interface end

type ucs2 = interface end

type Buffer =
        inherit NodeBuffer

type DuplexOptions =
        inherit ReadableOptions
        inherit WritableOptions

type Process =
        inherit EventEmitter

type ReadableStream =
        inherit EventEmitter

type TransformOptions =
        inherit ReadableOptions
        inherit WritableOptions

type WritableStream =
        inherit EventEmitter

type ChildProcess =
        inherit EventEmitter

type Domain =
        inherit EventEmitter

type FSWatcher =
        inherit EventEmitter

type ReadLine =
        inherit EventEmitter

type ReadWriteStream =
        inherit ReadableStream
        inherit WritableStream

type Readable =
        inherit EventEmitter
        inherit ReadableStream

type Stream =
        inherit EventEmitter

type Worker =
        inherit EventEmitter

type Writable =
        inherit EventEmitter
        inherit WritableStream

type ClientRequest =
        inherit EventEmitter
        inherit Writable

type ClientResponse =
        inherit EventEmitter
        inherit Readable

type Duplex =
        inherit Readable
        inherit ReadWriteStream

type ReadStream =
        inherit Readable

type ServerRequest =
        inherit EventEmitter
        inherit Readable

type ServerResponse =
        inherit EventEmitter
        inherit Writable

type Transform =
        inherit EventEmitter
        inherit ReadWriteStream

type WriteStream =
        inherit Writable

type ClearTextStream =
        inherit Duplex

type Deflate =
        inherit Transform

type DeflateRaw =
        inherit Transform

type Gunzip =
        inherit Transform

type Gzip =
        inherit Transform

type Inflate =
        inherit Transform

type InflateRaw =
        inherit Transform

type PassThrough =
        inherit Transform

type Socket =
        inherit Duplex

type dgramSocket =
        inherit EventEmitter

type Unzip =
        inherit Transform

type Server =
        inherit Socket

type _assert = interface end

type _internal = interface end

type buffer = interface end

type child_process = interface end

type cluster = interface end

type crypto = interface end

type dgram = interface end

type dns = interface end

type domain = interface end

type fs = interface end

type http = interface end

type https = interface end

type net = interface end

type os = interface end

type path = interface end

type punycode = interface end

type querystring = interface end

type readline = interface end

type repl = interface end

type string_decoder = interface end

type tls = interface end

type tty = interface end

type url = interface end

type util = interface end

type vm = interface end

type zlib = interface end


[<AutoOpen>]
module TypeExtensions_Node =

    type Global with 

            [<JSEmitInline("(process)"); CompiledName("_process1")>]
            static member _process with get() : Process = failwith "never" and set (v : Process) : unit = failwith "never"
            [<JSEmitInline("(global)"); CompiledName("_global1")>]
            static member _global with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("(__filename)"); CompiledName("__filename")>]
            static member __filename with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("(__dirname)"); CompiledName("__dirname")>]
            static member __dirname with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("(setTimeout({args}))"); CompiledName("setTimeout4")>]
            static member setTimeout(callback : Func<array<obj>, unit>, ms : float) : Timer = failwith "never"
            [<JSEmitInline("(setTimeout({args}))"); CompiledName("setTimeout5")>]
            static member setTimeoutOverload2(callback : Func<array<obj>, unit>, ms : float, [<System.ParamArray>] args : array<obj>) : Timer = failwith "never"
            [<JSEmitInline("(clearTimeout({args}))"); CompiledName("clearTimeout2")>]
            static member clearTimeout(timeoutId : Timer) : unit = failwith "never"
            [<JSEmitInline("(setInterval({args}))"); CompiledName("setInterval4")>]
            static member setInterval(callback : Func<array<obj>, unit>, ms : float) : Timer = failwith "never"
            [<JSEmitInline("(setInterval({args}))"); CompiledName("setInterval5")>]
            static member setIntervalOverload2(callback : Func<array<obj>, unit>, ms : float, [<System.ParamArray>] args : array<obj>) : Timer = failwith "never"
            [<JSEmitInline("(clearInterval({args}))"); CompiledName("clearInterval2")>]
            static member clearInterval(intervalId : Timer) : unit = failwith "never"
            [<JSEmitInline("(setImmediate({args}))"); CompiledName("setImmediate2")>]
            static member setImmediate(callback : Func<array<obj>, unit>) : obj = failwith "never"
            [<JSEmitInline("(setImmediate({args}))"); CompiledName("setImmediate3")>]
            static member setImmediateOverload2(callback : Func<array<obj>, unit>, [<System.ParamArray>] args : array<obj>) : obj = failwith "never"
            [<JSEmitInline("(clearImmediate({args}))"); CompiledName("clearImmediate1")>]
            static member clearImmediate(immediateId : obj) : unit = failwith "never"
            [<JSEmitInline("(require)"); CompiledName("require")>]
            static member require with get() : AnonymousType431 = failwith "never" and set (v : AnonymousType431) : unit = failwith "never"
            [<JSEmitInline("(module)"); CompiledName("_module")>]
            static member _module with get() : AnonymousType432 = failwith "never" and set (v : AnonymousType432) : unit = failwith "never"
            [<JSEmitInline("(exports)"); CompiledName("exports")>]
            static member exports with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("(SlowBuffer)"); CompiledName("SlowBuffer")>]
            static member SlowBuffer with get() : AnonymousType433 = failwith "never" and set (v : AnonymousType433) : unit = failwith "never"
            [<JSEmitInline("(Buffer)"); CompiledName("Buffer")>]
            static member Buffer with get() : AnonymousType434 = failwith "never" and set (v : AnonymousType434) : unit = failwith "never"

    type AddressInfo with 

            [<JSEmitInline("({0}.address)"); CompiledName("address")>]
            member __.address with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.family)"); CompiledName("family")>]
            member __.family with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.port)"); CompiledName("port3")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type Agent with 

            [<JSEmitInline("({0}.maxSockets)"); CompiledName("maxSockets")>]
            member __.maxSockets with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.sockets)"); CompiledName("sockets")>]
            member __.sockets with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.requests)"); CompiledName("requests")>]
            member __.requests with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"

    type AnonymousType431 with 

            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke36")>]
            member __.Invoke(id : string) : obj = failwith "never"
            [<JSEmitInline("({0}.resolve({args}))"); CompiledName("resolve")>]
            member __.resolve(id : string) : string = failwith "never"
            [<JSEmitInline("({0}.cache)"); CompiledName("cache")>]
            member __.cache with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.extensions)"); CompiledName("extensions1")>]
            member __.extensions with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.main)"); CompiledName("main")>]
            member __.main with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"

    type AnonymousType432 with 

            [<JSEmitInline("({0}.exports)"); CompiledName("exports1")>]
            member __.exports with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.require({args}))"); CompiledName("require1")>]
            member __.require(id : string) : obj = failwith "never"
            [<JSEmitInline("({0}.id)"); CompiledName("id5")>]
            member __.id with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.filename)"); CompiledName("filename2")>]
            member __.filename with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.loaded)"); CompiledName("loaded1")>]
            member __.loaded with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.parent)"); CompiledName("parent2")>]
            member __.parent with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.children)"); CompiledName("children1")>]
            member __.children with get() : array<obj> = failwith "never" and set (v : array<obj>) : unit = failwith "never"

    type AnonymousType433 with 

            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create463")>]
            member __.Create(str : string, ?encoding : string) : Buffer = failwith "never"
            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create464")>]
            member __.Create(size : float) : Buffer = failwith "never"
            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create465")>]
            member __.Create(size : Uint8Array) : Buffer = failwith "never"
            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create466")>]
            member __.Create(array : array<obj>) : Buffer = failwith "never"
            [<JSEmitInline("({0}.prototype)"); CompiledName("prototype418")>]
            member __.prototype with get() : Buffer = failwith "never" and set (v : Buffer) : unit = failwith "never"
            [<JSEmitInline("({0}.isBuffer({args}))"); CompiledName("isBuffer")>]
            member __.isBuffer(_obj : obj) : bool = failwith "never"
            [<JSEmitInline("({0}.byteLength({args}))"); CompiledName("byteLength2")>]
            member __.byteLength(_string : string, ?encoding : string) : float = failwith "never"
            [<JSEmitInline("({0}.concat({args}))"); CompiledName("concat8")>]
            member __.concat(list : array<Buffer>, ?totalLength : float) : Buffer = failwith "never"

    type AnonymousType434 with 

            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create467")>]
            member __.Create(str : string, ?encoding : string) : Buffer = failwith "never"
            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create468")>]
            member __.Create(size : float) : Buffer = failwith "never"
            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create469")>]
            member __.Create(size : Uint8Array) : Buffer = failwith "never"
            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create470")>]
            member __.Create(array : array<obj>) : Buffer = failwith "never"
            [<JSEmitInline("({0}.prototype)"); CompiledName("prototype419")>]
            member __.prototype with get() : Buffer = failwith "never" and set (v : Buffer) : unit = failwith "never"
            [<JSEmitInline("({0}.isBuffer({args}))"); CompiledName("isBuffer1")>]
            member __.isBuffer(_obj : obj) : bool = failwith "never"
            [<JSEmitInline("({0}.byteLength({args}))"); CompiledName("byteLength3")>]
            member __.byteLength(_string : string, ?encoding : string) : float = failwith "never"
            [<JSEmitInline("({0}.concat({args}))"); CompiledName("concat9")>]
            member __.concat(list : array<Buffer>, ?totalLength : float) : Buffer = failwith "never"

    type AnonymousType435<'T> with 

            [<JSEmitInline("({0}.end)"); CompiledName("_end1")>]
            member __._end with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType436 with 

            [<JSEmitInline("({0}.http_parser)"); CompiledName("http_parser")>]
            member __.http_parser with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.node)"); CompiledName("node")>]
            member __.node with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.v8)"); CompiledName("v8")>]
            member __.v8 with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.ares)"); CompiledName("ares")>]
            member __.ares with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.uv)"); CompiledName("uv")>]
            member __.uv with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.zlib)"); CompiledName("zlib")>]
            member __.zlib with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.openssl)"); CompiledName("openssl")>]
            member __.openssl with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType437 with 

            [<JSEmitInline("({0}.target_defaults)"); CompiledName("target_defaults")>]
            member __.target_defaults with get() : AnonymousType438 = failwith "never" and set (v : AnonymousType438) : unit = failwith "never"
            [<JSEmitInline("({0}.variables)"); CompiledName("variables")>]
            member __.variables with get() : AnonymousType439 = failwith "never" and set (v : AnonymousType439) : unit = failwith "never"

    type AnonymousType438 with 

            [<JSEmitInline("({0}.cflags)"); CompiledName("cflags")>]
            member __.cflags with get() : array<obj> = failwith "never" and set (v : array<obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.default_configuration)"); CompiledName("default_configuration")>]
            member __.default_configuration with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.defines)"); CompiledName("defines")>]
            member __.defines with get() : array<string> = failwith "never" and set (v : array<string>) : unit = failwith "never"
            [<JSEmitInline("({0}.include_dirs)"); CompiledName("include_dirs")>]
            member __.include_dirs with get() : array<string> = failwith "never" and set (v : array<string>) : unit = failwith "never"
            [<JSEmitInline("({0}.libraries)"); CompiledName("libraries")>]
            member __.libraries with get() : array<string> = failwith "never" and set (v : array<string>) : unit = failwith "never"

    type AnonymousType439 with 

            [<JSEmitInline("({0}.clang)"); CompiledName("clang")>]
            member __.clang with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.host_arch)"); CompiledName("host_arch")>]
            member __.host_arch with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.node_install_npm)"); CompiledName("node_install_npm")>]
            member __.node_install_npm with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.node_install_waf)"); CompiledName("node_install_waf")>]
            member __.node_install_waf with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.node_prefix)"); CompiledName("node_prefix")>]
            member __.node_prefix with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.node_shared_openssl)"); CompiledName("node_shared_openssl")>]
            member __.node_shared_openssl with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.node_shared_v8)"); CompiledName("node_shared_v8")>]
            member __.node_shared_v8 with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.node_shared_zlib)"); CompiledName("node_shared_zlib")>]
            member __.node_shared_zlib with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.node_use_dtrace)"); CompiledName("node_use_dtrace")>]
            member __.node_use_dtrace with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.node_use_etw)"); CompiledName("node_use_etw")>]
            member __.node_use_etw with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.node_use_openssl)"); CompiledName("node_use_openssl")>]
            member __.node_use_openssl with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.target_arch)"); CompiledName("target_arch")>]
            member __.target_arch with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.v8_no_strict_aliasing)"); CompiledName("v8_no_strict_aliasing")>]
            member __.v8_no_strict_aliasing with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.v8_use_snapshot)"); CompiledName("v8_use_snapshot")>]
            member __.v8_use_snapshot with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.visibility)"); CompiledName("visibility1")>]
            member __.visibility with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType440 with 

            [<JSEmitInline("({0}.rss)"); CompiledName("rss")>]
            member __.rss with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.heapTotal)"); CompiledName("heapTotal")>]
            member __.heapTotal with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.heapUsed)"); CompiledName("heapUsed")>]
            member __.heapUsed with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type AnonymousType441 with 

            [<JSEmitInline("({0}.maxKeys)"); CompiledName("maxKeys")>]
            member __.maxKeys with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type AnonymousType442 with 

            [<JSEmitInline("({0}.port)"); CompiledName("port4")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.family)"); CompiledName("family1")>]
            member __.family with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.address)"); CompiledName("address1")>]
            member __.address with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType443 with 

            [<JSEmitInline("({0}[{1}])"); CompiledName("Item44")>]
            member __.Item with get(i : int) : string = failwith "never" and set (i : int) (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}[{1}])"); CompiledName("Item45")>]
            member __.Item with get(i : string) : string = failwith "never" and set (i : string) (v : string) : unit = failwith "never"

    type AnonymousType444 with 

            [<JSEmitInline("({0}.model)"); CompiledName("model")>]
            member __.model with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.speed)"); CompiledName("speed1")>]
            member __.speed with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.times)"); CompiledName("times")>]
            member __.times with get() : AnonymousType445 = failwith "never" and set (v : AnonymousType445) : unit = failwith "never"

    type AnonymousType445 with 

            [<JSEmitInline("({0}.user)"); CompiledName("user")>]
            member __.user with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.nice)"); CompiledName("nice")>]
            member __.nice with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.sys)"); CompiledName("sys")>]
            member __.sys with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.idle)"); CompiledName("idle")>]
            member __.idle with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.irq)"); CompiledName("irq")>]
            member __.irq with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type AnonymousType446 with 

            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create471")>]
            member __.Create(?options : RequestOptions) : Agent = failwith "never"

    type AnonymousType447 with 

            [<JSEmitInline("({0}.cwd)"); CompiledName("cwd")>]
            member __.cwd with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.stdio)"); CompiledName("stdio")>]
            member __.stdio with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.custom)"); CompiledName("custom")>]
            member __.custom with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.env)"); CompiledName("env")>]
            member __.env with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.detached)"); CompiledName("detached")>]
            member __.detached with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType448 with 

            [<JSEmitInline("({0}.cwd)"); CompiledName("cwd1")>]
            member __.cwd with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.stdio)"); CompiledName("stdio1")>]
            member __.stdio with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.customFds)"); CompiledName("customFds")>]
            member __.customFds with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.env)"); CompiledName("env1")>]
            member __.env with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding1")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.timeout)"); CompiledName("timeout3")>]
            member __.timeout with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.maxBuffer)"); CompiledName("maxBuffer")>]
            member __.maxBuffer with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.killSignal)"); CompiledName("killSignal")>]
            member __.killSignal with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType449 with 

            [<JSEmitInline("({0}.cwd)"); CompiledName("cwd2")>]
            member __.cwd with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.stdio)"); CompiledName("stdio2")>]
            member __.stdio with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.customFds)"); CompiledName("customFds1")>]
            member __.customFds with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.env)"); CompiledName("env2")>]
            member __.env with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding2")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.timeout)"); CompiledName("timeout4")>]
            member __.timeout with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.maxBuffer)"); CompiledName("maxBuffer1")>]
            member __.maxBuffer with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.killSignal)"); CompiledName("killSignal1")>]
            member __.killSignal with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType450 with 

            [<JSEmitInline("({0}.cwd)"); CompiledName("cwd3")>]
            member __.cwd with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.env)"); CompiledName("env3")>]
            member __.env with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding3")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType451 with 

            [<JSEmitInline("({0}.port)"); CompiledName("port5")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.family)"); CompiledName("family2")>]
            member __.family with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.address)"); CompiledName("address2")>]
            member __.address with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType452 with 

            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create472")>]
            member __.Create(?options : AnonymousType453) : Socket = failwith "never"

    type AnonymousType453 with 

            [<JSEmitInline("({0}.fd)"); CompiledName("fd")>]
            member __.fd with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.type)"); CompiledName("_type35")>]
            member __._type with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.allowHalfOpen)"); CompiledName("allowHalfOpen")>]
            member __.allowHalfOpen with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType454 with 

            [<JSEmitInline("({0}.port)"); CompiledName("port6")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.family)"); CompiledName("family3")>]
            member __.family with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.address)"); CompiledName("address3")>]
            member __.address with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType455 with 

            [<JSEmitInline("({0}.allowHalfOpen)"); CompiledName("allowHalfOpen1")>]
            member __.allowHalfOpen with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType456 with 

            [<JSEmitInline("({0}.allowHalfOpen)"); CompiledName("allowHalfOpen2")>]
            member __.allowHalfOpen with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType457 with 

            [<JSEmitInline("({0}.allowHalfOpen)"); CompiledName("allowHalfOpen3")>]
            member __.allowHalfOpen with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType458 with 

            [<JSEmitInline("({0}[{1}])"); CompiledName("Item46")>]
            member __.Item with get(i : string) : string = failwith "never" and set (i : string) (v : string) : unit = failwith "never"

    type AnonymousType459 with 

            [<JSEmitInline("({0}[{1}])"); CompiledName("Item47")>]
            member __.Item with get(i : string) : string = failwith "never" and set (i : string) (v : string) : unit = failwith "never"

    type AnonymousType460 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding4")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType461 with 

            [<JSEmitInline("({0}.flag)"); CompiledName("flag1")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType462 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding5")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag2")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType463 with 

            [<JSEmitInline("({0}.flag)"); CompiledName("flag3")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType464 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding6")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode3")>]
            member __.mode with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag4")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType465 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding7")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode4")>]
            member __.mode with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag5")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType466 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding8")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode5")>]
            member __.mode with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag6")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType467 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding9")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode6")>]
            member __.mode with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag7")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType468 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding10")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode7")>]
            member __.mode with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag8")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType469 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding11")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode8")>]
            member __.mode with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag9")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType470 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding12")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode9")>]
            member __.mode with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag10")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType471 with 

            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding13")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode10")>]
            member __.mode with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.flag)"); CompiledName("flag11")>]
            member __.flag with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType472 with 

            [<JSEmitInline("({0}.persistent)"); CompiledName("persistent")>]
            member __.persistent with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.interval)"); CompiledName("interval1")>]
            member __.interval with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type AnonymousType473 with 

            [<JSEmitInline("({0}.persistent)"); CompiledName("persistent1")>]
            member __.persistent with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType474 with 

            [<JSEmitInline("({0}.flags)"); CompiledName("flags")>]
            member __.flags with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding14")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.fd)"); CompiledName("fd1")>]
            member __.fd with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode11")>]
            member __.mode with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.bufferSize)"); CompiledName("bufferSize")>]
            member __.bufferSize with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type AnonymousType475 with 

            [<JSEmitInline("({0}.flags)"); CompiledName("flags1")>]
            member __.flags with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding15")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.fd)"); CompiledName("fd2")>]
            member __.fd with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode12")>]
            member __.mode with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.bufferSize)"); CompiledName("bufferSize1")>]
            member __.bufferSize with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type AnonymousType476 with 

            [<JSEmitInline("({0}.flags)"); CompiledName("flags2")>]
            member __.flags with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding16")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.string)"); CompiledName("_string")>]
            member __._string with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType477 with 

            [<JSEmitInline("(new {0}({args}))"); CompiledName("Create473")>]
            member __.Create(encoding : string) : NodeStringDecoder = failwith "never"

    type AnonymousType478 with 

            [<JSEmitInline("({0}.port)"); CompiledName("port7")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.family)"); CompiledName("family4")>]
            member __.family with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.address)"); CompiledName("address4")>]
            member __.address with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType479 with 

            [<JSEmitInline("({0}.key)"); CompiledName("key5")>]
            member __.key with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.cert)"); CompiledName("cert")>]
            member __.cert with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.ca)"); CompiledName("ca")>]
            member __.ca with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType480 with 

            [<JSEmitInline("({0}.name)"); CompiledName("name35")>]
            member __.name with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.version)"); CompiledName("version4")>]
            member __.version with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType481 with 

            [<JSEmitInline("({0}.port)"); CompiledName("port8")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.family)"); CompiledName("family5")>]
            member __.family with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.address)"); CompiledName("address5")>]
            member __.address with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type AnonymousType482<'T> with 

            [<JSEmitInline("({0}.end)"); CompiledName("_end2")>]
            member __._end with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType483<'T> with 

            [<JSEmitInline("({0}.end)"); CompiledName("_end3")>]
            member __._end with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType484<'T> with 

            [<JSEmitInline("({0}.end)"); CompiledName("_end4")>]
            member __._end with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type AnonymousType485 with 

            [<JSEmitInline("({0}.message)"); CompiledName("message8")>]
            member __.message with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.actual)"); CompiledName("actual")>]
            member __.actual with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.expected)"); CompiledName("expected")>]
            member __.expected with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.operator)"); CompiledName("_operator2")>]
            member __._operator with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.stackStartFunction)"); CompiledName("stackStartFunction")>]
            member __.stackStartFunction with get() : Function = failwith "never" and set (v : Function) : unit = failwith "never"

    type AnonymousType486 with 

            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke37")>]
            member __.Invoke(block : Function, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke38")>]
            member __.Invoke(block : Function, error : Function, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke39")>]
            member __.Invoke(block : Function, error : RegExp, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke40")>]
            member __.Invoke(block : Function, error : Func<obj, bool>, ?message : string) : unit = failwith "never"

    type AnonymousType487 with 

            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke41")>]
            member __.Invoke(block : Function, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke42")>]
            member __.Invoke(block : Function, error : Function, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke43")>]
            member __.Invoke(block : Function, error : RegExp, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}({args}))"); CompiledName("Invoke44")>]
            member __.Invoke(block : Function, error : Func<obj, bool>, ?message : string) : unit = failwith "never"

    type AssertionError with 

            [<JSEmitInline("({0}.name)"); CompiledName("name36")>]
            member __.name with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.message)"); CompiledName("message9")>]
            member __.message with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.actual)"); CompiledName("actual1")>]
            member __.actual with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.expected)"); CompiledName("expected1")>]
            member __.expected with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.operator)"); CompiledName("_operator3")>]
            member __._operator with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.generatedMessage)"); CompiledName("generatedMessage")>]
            member __.generatedMessage with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("(new AssertionError({args}))"); CompiledName("Create474")>]
            static member Create(?options : AnonymousType485) : AssertionError = failwith "never"

    type ChildProcess with 

            [<JSEmitInline("({0}.stdin)"); CompiledName("stdin")>]
            member __.stdin with get() : Writable = failwith "never" and set (v : Writable) : unit = failwith "never"
            [<JSEmitInline("({0}.stdout)"); CompiledName("stdout")>]
            member __.stdout with get() : Readable = failwith "never" and set (v : Readable) : unit = failwith "never"
            [<JSEmitInline("({0}.stderr)"); CompiledName("stderr")>]
            member __.stderr with get() : Readable = failwith "never" and set (v : Readable) : unit = failwith "never"
            [<JSEmitInline("({0}.pid)"); CompiledName("pid")>]
            member __.pid with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.kill({args}))"); CompiledName("kill")>]
            member __.kill(?signal : string) : unit = failwith "never"
            [<JSEmitInline("({0}.send({args}))"); CompiledName("send3")>]
            member __.send(message : obj, sendHandle : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.disconnect())"); CompiledName("disconnect1")>]
            member __.disconnect() : unit = failwith "never"

    type Cipher with 

            [<JSEmitInline("({0}.update({args}))"); CompiledName("update3")>]
            member __.update(data : Buffer) : Buffer = failwith "never"
            [<JSEmitInline("({0}.update({args}))"); CompiledName("update4")>]
            member __.update(data : string, ?input_encoding : string, ?output_encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.final())"); CompiledName("final")>]
            member __.final() : Buffer = failwith "never"
            [<JSEmitInline("({0}.final({args}))"); CompiledName("final1")>]
            member __.final(output_encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.setAutoPadding({args}))"); CompiledName("setAutoPadding")>]
            member __.setAutoPadding(auto_padding : bool) : unit = failwith "never"

    type ClearTextStream with 

            [<JSEmitInline("({0}.authorized)"); CompiledName("authorized")>]
            member __.authorized with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.authorizationError)"); CompiledName("authorizationError")>]
            member __.authorizationError with get() : Error = failwith "never" and set (v : Error) : unit = failwith "never"
            [<JSEmitInline("({0}.getPeerCertificate())"); CompiledName("getPeerCertificate")>]
            member __.getPeerCertificate() : obj = failwith "never"
            [<JSEmitInline("({0}.getCipher)"); CompiledName("getCipher")>]
            member __.getCipher with get() : AnonymousType480 = failwith "never" and set (v : AnonymousType480) : unit = failwith "never"
            [<JSEmitInline("({0}.address)"); CompiledName("address6")>]
            member __.address with get() : AnonymousType481 = failwith "never" and set (v : AnonymousType481) : unit = failwith "never"
            [<JSEmitInline("({0}.remoteAddress)"); CompiledName("remoteAddress")>]
            member __.remoteAddress with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.remotePort)"); CompiledName("remotePort")>]
            member __.remotePort with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type ClientRequest with 

            [<JSEmitInline("({0}.write({args}))"); CompiledName("write2")>]
            member __.write(buffer : Buffer) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write3")>]
            member __.writeOverload2(buffer : Buffer, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write4")>]
            member __.write(str : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write5")>]
            member __.writeOverload2(str : string, ?encoding : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write6")>]
            member __.writeOverload3(str : string, ?encoding : string, ?fd : string) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write7")>]
            member __.write(chunk : obj, ?encoding : string) : unit = failwith "never"
            [<JSEmitInline("({0}.abort())"); CompiledName("abort7")>]
            member __.abort() : unit = failwith "never"
            [<JSEmitInline("({0}.setTimeout({args}))"); CompiledName("setTimeout6")>]
            member __.setTimeout(timeout : float, ?callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.setNoDelay({args}))"); CompiledName("setNoDelay")>]
            member __.setNoDelay(?noDelay : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.setSocketKeepAlive({args}))"); CompiledName("setSocketKeepAlive")>]
            member __.setSocketKeepAlive(?enable : bool, ?initialDelay : float) : unit = failwith "never"
            [<JSEmitInline("({0}.end())"); CompiledName("_end5")>]
            member __._end() : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end6")>]
            member __._end(buffer : Buffer, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end7")>]
            member __._end(str : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end8")>]
            member __._endOverload2(str : string, ?encoding : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end9")>]
            member __._endOverload2(?data : obj, ?encoding : string) : unit = failwith "never"

    type ClientResponse with 

            [<JSEmitInline("({0}.statusCode)"); CompiledName("statusCode")>]
            member __.statusCode with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.httpVersion)"); CompiledName("httpVersion")>]
            member __.httpVersion with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.headers)"); CompiledName("headers1")>]
            member __.headers with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.trailers)"); CompiledName("trailers")>]
            member __.trailers with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.setEncoding({args}))"); CompiledName("setEncoding")>]
            member __.setEncoding(?encoding : string) : unit = failwith "never"
            [<JSEmitInline("({0}.pause())"); CompiledName("pause1")>]
            member __.pause() : unit = failwith "never"
            [<JSEmitInline("({0}.resume())"); CompiledName("resume")>]
            member __.resume() : unit = failwith "never"

    type ClusterSettings with 

            [<JSEmitInline("({0}.exec)"); CompiledName("exec1")>]
            member __.exec with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.args)"); CompiledName("args")>]
            member __.args with get() : array<string> = failwith "never" and set (v : array<string>) : unit = failwith "never"
            [<JSEmitInline("({0}.silent)"); CompiledName("silent")>]
            member __.silent with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type ConnectionOptions with 

            [<JSEmitInline("({0}.host)"); CompiledName("host3")>]
            member __.host with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.port)"); CompiledName("port9")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.socket)"); CompiledName("socket")>]
            member __.socket with get() : Socket = failwith "never" and set (v : Socket) : unit = failwith "never"
            [<JSEmitInline("({0}.pfx)"); CompiledName("pfx")>]
            member __.pfx with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.key)"); CompiledName("key6")>]
            member __.key with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.passphrase)"); CompiledName("passphrase")>]
            member __.passphrase with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.cert)"); CompiledName("cert1")>]
            member __.cert with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.ca)"); CompiledName("ca1")>]
            member __.ca with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.rejectUnauthorized)"); CompiledName("rejectUnauthorized")>]
            member __.rejectUnauthorized with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.NPNProtocols)"); CompiledName("NPNProtocols")>]
            member __.NPNProtocols with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.servername)"); CompiledName("servername")>]
            member __.servername with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type CredentialDetails with 

            [<JSEmitInline("({0}.pfx)"); CompiledName("pfx1")>]
            member __.pfx with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.key)"); CompiledName("key7")>]
            member __.key with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.passphrase)"); CompiledName("passphrase1")>]
            member __.passphrase with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.cert)"); CompiledName("cert2")>]
            member __.cert with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.ca)"); CompiledName("ca2")>]
            member __.ca with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.crl)"); CompiledName("crl")>]
            member __.crl with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.ciphers)"); CompiledName("ciphers")>]
            member __.ciphers with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type Credentials with 

            [<JSEmitInline("({0}.context)"); CompiledName("context")>]
            member __.context with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"

    type Decipher with 

            [<JSEmitInline("({0}.update({args}))"); CompiledName("update5")>]
            member __.update(data : Buffer) : Buffer = failwith "never"
            [<JSEmitInline("({0}.update({args}))"); CompiledName("update6")>]
            member __.update(data : string, ?input_encoding : string, ?output_encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.final())"); CompiledName("final2")>]
            member __.final() : Buffer = failwith "never"
            [<JSEmitInline("({0}.final({args}))"); CompiledName("final3")>]
            member __.final(output_encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.setAutoPadding({args}))"); CompiledName("setAutoPadding1")>]
            member __.setAutoPadding(auto_padding : bool) : unit = failwith "never"

    type DiffieHellman with 

            [<JSEmitInline("({0}.generateKeys({args}))"); CompiledName("generateKeys")>]
            member __.generateKeys(?encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.computeSecret({args}))"); CompiledName("computeSecret")>]
            member __.computeSecret(other_public_key : string, ?input_encoding : string, ?output_encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.getPrime({args}))"); CompiledName("getPrime")>]
            member __.getPrime(?encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.getGenerator({args}))"); CompiledName("getGenerator")>]
            member __.getGenerator(encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.getPublicKey({args}))"); CompiledName("getPublicKey")>]
            member __.getPublicKey(?encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.getPrivateKey({args}))"); CompiledName("getPrivateKey")>]
            member __.getPrivateKey(?encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.setPublicKey({args}))"); CompiledName("setPublicKey")>]
            member __.setPublicKey(public_key : string, ?encoding : string) : unit = failwith "never"
            [<JSEmitInline("({0}.setPrivateKey({args}))"); CompiledName("setPrivateKey")>]
            member __.setPrivateKey(public_key : string, ?encoding : string) : unit = failwith "never"

    type Domain with 

            [<JSEmitInline("({0}.run({args}))"); CompiledName("run")>]
            member __.run(fn : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.add({args}))"); CompiledName("add7")>]
            member __.add(emitter : EventEmitter) : unit = failwith "never"
            [<JSEmitInline("({0}.remove({args}))"); CompiledName("remove5")>]
            member __.remove(emitter : EventEmitter) : unit = failwith "never"
            [<JSEmitInline("({0}.bind({args}))"); CompiledName("bind2")>]
            member __.bind(cb : Func<Error, obj, obj>) : obj = failwith "never"
            [<JSEmitInline("({0}.intercept({args}))"); CompiledName("intercept1")>]
            member __.intercept(cb : Func<obj, obj>) : obj = failwith "never"
            [<JSEmitInline("({0}.dispose())"); CompiledName("dispose")>]
            member __.dispose() : unit = failwith "never"
            [<JSEmitInline("({0}.addListener({args}))"); CompiledName("addListener1")>]
            member __.addListener(_event : string, listener : Function) : Domain = failwith "never"
            [<JSEmitInline("({0}.on({args}))"); CompiledName("on")>]
            member __.on(_event : string, listener : Function) : Domain = failwith "never"
            [<JSEmitInline("({0}.once({args}))"); CompiledName("once")>]
            member __.once(_event : string, listener : Function) : Domain = failwith "never"
            [<JSEmitInline("({0}.removeListener({args}))"); CompiledName("removeListener1")>]
            member __.removeListener(_event : string, listener : Function) : Domain = failwith "never"
            [<JSEmitInline("({0}.removeAllListeners({args}))"); CompiledName("removeAllListeners")>]
            member __.removeAllListeners(?_event : string) : Domain = failwith "never"

    type Duplex with 

            [<JSEmitInline("({0}.writable)"); CompiledName("writable1")>]
            member __.writable with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("(new {0}.Duplex({args}))"); CompiledName("Create475")>]
            member __.Create(?opts : DuplexOptions) : Duplex = failwith "never"
            [<JSEmitInline("({0}._write({args}))"); CompiledName("_write")>]
            member __._write(data : Buffer, encoding : string, callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}._write({args}))"); CompiledName("_write1")>]
            member __._write(data : string, encoding : string, callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write8")>]
            member __.write(buffer : Buffer, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write9")>]
            member __.write(str : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write10")>]
            member __.writeOverload2(str : string, ?encoding : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.end())"); CompiledName("_end10")>]
            member __._end() : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end11")>]
            member __._end(buffer : Buffer, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end12")>]
            member __._end(str : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end13")>]
            member __._endOverload2(str : string, ?encoding : string, ?cb : Function) : unit = failwith "never"

    type DuplexOptions with 

            [<JSEmitInline("({0}.allowHalfOpen)"); CompiledName("allowHalfOpen4")>]
            member __.allowHalfOpen with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type ErrnoException with 

            [<JSEmitInline("({0}.errno)"); CompiledName("errno")>]
            member __.errno with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.code)"); CompiledName("code10")>]
            member __.code with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.path)"); CompiledName("path")>]
            member __.path with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.syscall)"); CompiledName("syscall")>]
            member __.syscall with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type EventEmitter with 

            [<JSEmitInline("({0}.EventEmitter.listenerCount({args}))"); CompiledName("listenerCount")>]
            member __.listenerCount(emitter : EventEmitter, _event : string) : float = failwith "never"
            [<JSEmitInline("({0}.addListener({args}))"); CompiledName("addListener3")>]
            member __.addListener(_event : string, listener : Function) : EventEmitter = failwith "never"
            [<JSEmitInline("({0}.on({args}))"); CompiledName("on2")>]
            member __.on(_event : string, listener : Function) : EventEmitter = failwith "never"
            [<JSEmitInline("({0}.once({args}))"); CompiledName("once2")>]
            member __.once(_event : string, listener : Function) : EventEmitter = failwith "never"
            [<JSEmitInline("({0}.removeListener({args}))"); CompiledName("removeListener3")>]
            member __.removeListener(_event : string, listener : Function) : EventEmitter = failwith "never"
            [<JSEmitInline("({0}.removeAllListeners({args}))"); CompiledName("removeAllListeners2")>]
            member __.removeAllListeners(?_event : string) : EventEmitter = failwith "never"
            [<JSEmitInline("({0}.setMaxListeners({args}))"); CompiledName("setMaxListeners1")>]
            member __.setMaxListeners(n : float) : unit = failwith "never"
            [<JSEmitInline("({0}.listeners({args}))"); CompiledName("listeners1")>]
            member __.listeners(_event : string) : array<Function> = failwith "never"
            [<JSEmitInline("({0}.emit({args}))"); CompiledName("emit2")>]
            member __.emit(_event : string) : bool = failwith "never"
            [<JSEmitInline("({0}.emit({args}))"); CompiledName("emit3")>]
            member __.emitOverload2(_event : string, [<System.ParamArray>] args : array<obj>) : bool = failwith "never"

    type FSWatcher with 

            [<JSEmitInline("({0}.close())"); CompiledName("close9")>]
            member __.close() : unit = failwith "never"

    type Hash with 

            [<JSEmitInline("({0}.update({args}))"); CompiledName("update7")>]
            member __.update(data : obj, ?input_encoding : string) : Hash = failwith "never"
            [<JSEmitInline("({0}.digest(\"buffer\"))"); CompiledName("digest1")>]
            member __.digest_buffer() : Buffer = failwith "never"
            [<JSEmitInline("({0}.digest({args}))"); CompiledName("digest2")>]
            member __.digest(encoding : string) : obj = failwith "never"
            [<JSEmitInline("({0}.digest())"); CompiledName("digest3")>]
            member __.digest() : Buffer = failwith "never"

    type Hmac with 

            [<JSEmitInline("({0}.update({args}))"); CompiledName("update8")>]
            member __.update(data : obj, ?input_encoding : string) : Hmac = failwith "never"
            [<JSEmitInline("({0}.digest(\"buffer\"))"); CompiledName("digest4")>]
            member __.digest_buffer() : Buffer = failwith "never"
            [<JSEmitInline("({0}.digest({args}))"); CompiledName("digest5")>]
            member __.digest(encoding : string) : obj = failwith "never"
            [<JSEmitInline("({0}.digest())"); CompiledName("digest6")>]
            member __.digest() : Buffer = failwith "never"

    type InspectOptions with 

            [<JSEmitInline("({0}.showHidden)"); CompiledName("showHidden")>]
            member __.showHidden with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.depth)"); CompiledName("depth")>]
            member __.depth with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.colors)"); CompiledName("colors")>]
            member __.colors with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.customInspect)"); CompiledName("customInspect")>]
            member __.customInspect with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type NodeBuffer with 

            [<JSEmitInline("({0}[{1}])"); CompiledName("Item48")>]
            member __.Item with get(i : int) : float = failwith "never" and set (i : int) (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write11")>]
            member __.write(_string : string, ?offset : float, ?length : float, ?encoding : string) : float = failwith "never"
            [<JSEmitInline("({0}.toString({args}))"); CompiledName("toString22")>]
            member __.toString(?encoding : string, ?start : float, ?_end : float) : string = failwith "never"
            [<JSEmitInline("({0}.toJSON())"); CompiledName("toJSON4")>]
            member __.toJSON() : obj = failwith "never"
            [<JSEmitInline("({0}.length)"); CompiledName("length53")>]
            member __.length with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.copy({args}))"); CompiledName("copy")>]
            member __.copy(targetBuffer : Buffer, ?targetStart : float, ?sourceStart : float, ?sourceEnd : float) : float = failwith "never"
            [<JSEmitInline("({0}.slice({args}))"); CompiledName("slice4")>]
            member __.slice(?start : float, ?_end : float) : Buffer = failwith "never"
            [<JSEmitInline("({0}.readUInt8({args}))"); CompiledName("readUInt8")>]
            member __.readUInt8(offset : float, ?noAsset : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readUInt16LE({args}))"); CompiledName("readUInt16LE")>]
            member __.readUInt16LE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readUInt16BE({args}))"); CompiledName("readUInt16BE")>]
            member __.readUInt16BE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readUInt32LE({args}))"); CompiledName("readUInt32LE")>]
            member __.readUInt32LE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readUInt32BE({args}))"); CompiledName("readUInt32BE")>]
            member __.readUInt32BE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readInt8({args}))"); CompiledName("readInt8")>]
            member __.readInt8(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readInt16LE({args}))"); CompiledName("readInt16LE")>]
            member __.readInt16LE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readInt16BE({args}))"); CompiledName("readInt16BE")>]
            member __.readInt16BE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readInt32LE({args}))"); CompiledName("readInt32LE")>]
            member __.readInt32LE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readInt32BE({args}))"); CompiledName("readInt32BE")>]
            member __.readInt32BE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readFloatLE({args}))"); CompiledName("readFloatLE")>]
            member __.readFloatLE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readFloatBE({args}))"); CompiledName("readFloatBE")>]
            member __.readFloatBE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readDoubleLE({args}))"); CompiledName("readDoubleLE")>]
            member __.readDoubleLE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.readDoubleBE({args}))"); CompiledName("readDoubleBE")>]
            member __.readDoubleBE(offset : float, ?noAssert : bool) : float = failwith "never"
            [<JSEmitInline("({0}.writeUInt8({args}))"); CompiledName("writeUInt8")>]
            member __.writeUInt8(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeUInt16LE({args}))"); CompiledName("writeUInt16LE")>]
            member __.writeUInt16LE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeUInt16BE({args}))"); CompiledName("writeUInt16BE")>]
            member __.writeUInt16BE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeUInt32LE({args}))"); CompiledName("writeUInt32LE")>]
            member __.writeUInt32LE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeUInt32BE({args}))"); CompiledName("writeUInt32BE")>]
            member __.writeUInt32BE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeInt8({args}))"); CompiledName("writeInt8")>]
            member __.writeInt8(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeInt16LE({args}))"); CompiledName("writeInt16LE")>]
            member __.writeInt16LE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeInt16BE({args}))"); CompiledName("writeInt16BE")>]
            member __.writeInt16BE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeInt32LE({args}))"); CompiledName("writeInt32LE")>]
            member __.writeInt32LE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeInt32BE({args}))"); CompiledName("writeInt32BE")>]
            member __.writeInt32BE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeFloatLE({args}))"); CompiledName("writeFloatLE")>]
            member __.writeFloatLE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeFloatBE({args}))"); CompiledName("writeFloatBE")>]
            member __.writeFloatBE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeDoubleLE({args}))"); CompiledName("writeDoubleLE")>]
            member __.writeDoubleLE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writeDoubleBE({args}))"); CompiledName("writeDoubleBE")>]
            member __.writeDoubleBE(value : float, offset : float, ?noAssert : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.fill({args}))"); CompiledName("fill2")>]
            member __.fill(value : obj, ?offset : float, ?_end : float) : unit = failwith "never"

    type NodeStringDecoder with 

            [<JSEmitInline("({0}.write({args}))"); CompiledName("write12")>]
            member __.write(buffer : Buffer) : string = failwith "never"
            [<JSEmitInline("({0}.detectIncompleteChar({args}))"); CompiledName("detectIncompleteChar")>]
            member __.detectIncompleteChar(buffer : Buffer) : float = failwith "never"

    type Process with 

            [<JSEmitInline("({0}.stdout)"); CompiledName("stdout1")>]
            member __.stdout with get() : WritableStream = failwith "never" and set (v : WritableStream) : unit = failwith "never"
            [<JSEmitInline("({0}.stderr)"); CompiledName("stderr1")>]
            member __.stderr with get() : WritableStream = failwith "never" and set (v : WritableStream) : unit = failwith "never"
            [<JSEmitInline("({0}.stdin)"); CompiledName("stdin1")>]
            member __.stdin with get() : ReadableStream = failwith "never" and set (v : ReadableStream) : unit = failwith "never"
            [<JSEmitInline("({0}.argv)"); CompiledName("argv")>]
            member __.argv with get() : array<string> = failwith "never" and set (v : array<string>) : unit = failwith "never"
            [<JSEmitInline("({0}.execPath)"); CompiledName("execPath")>]
            member __.execPath with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.abort())"); CompiledName("abort8")>]
            member __.abort() : unit = failwith "never"
            [<JSEmitInline("({0}.chdir({args}))"); CompiledName("chdir")>]
            member __.chdir(directory : string) : unit = failwith "never"
            [<JSEmitInline("({0}.cwd())"); CompiledName("cwd4")>]
            member __.cwd() : string = failwith "never"
            [<JSEmitInline("({0}.env)"); CompiledName("env4")>]
            member __.env with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.exit({args}))"); CompiledName("exit")>]
            member __.exit(?code : float) : unit = failwith "never"
            [<JSEmitInline("({0}.getgid())"); CompiledName("getgid")>]
            member __.getgid() : float = failwith "never"
            [<JSEmitInline("({0}.setgid({args}))"); CompiledName("setgid")>]
            member __.setgid(id : float) : unit = failwith "never"
            [<JSEmitInline("({0}.setgid({args}))"); CompiledName("setgid1")>]
            member __.setgid(id : string) : unit = failwith "never"
            [<JSEmitInline("({0}.getuid())"); CompiledName("getuid")>]
            member __.getuid() : float = failwith "never"
            [<JSEmitInline("({0}.setuid({args}))"); CompiledName("setuid")>]
            member __.setuid(id : float) : unit = failwith "never"
            [<JSEmitInline("({0}.setuid({args}))"); CompiledName("setuid1")>]
            member __.setuid(id : string) : unit = failwith "never"
            [<JSEmitInline("({0}.version)"); CompiledName("version5")>]
            member __.version with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.versions)"); CompiledName("versions")>]
            member __.versions with get() : AnonymousType436 = failwith "never" and set (v : AnonymousType436) : unit = failwith "never"
            [<JSEmitInline("({0}.config)"); CompiledName("config")>]
            member __.config with get() : AnonymousType437 = failwith "never" and set (v : AnonymousType437) : unit = failwith "never"
            [<JSEmitInline("({0}.kill({args}))"); CompiledName("kill1")>]
            member __.kill(pid : float, ?signal : string) : unit = failwith "never"
            [<JSEmitInline("({0}.pid)"); CompiledName("pid1")>]
            member __.pid with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.title)"); CompiledName("title4")>]
            member __.title with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.arch)"); CompiledName("arch")>]
            member __.arch with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.platform)"); CompiledName("platform1")>]
            member __.platform with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.memoryUsage())"); CompiledName("memoryUsage")>]
            member __.memoryUsage() : AnonymousType440 = failwith "never"
            [<JSEmitInline("({0}.nextTick({args}))"); CompiledName("nextTick")>]
            member __.nextTick(callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.umask({args}))"); CompiledName("umask")>]
            member __.umask(?mask : float) : float = failwith "never"
            [<JSEmitInline("({0}.uptime())"); CompiledName("uptime")>]
            member __.uptime() : float = failwith "never"
            [<JSEmitInline("({0}.hrtime({args}))"); CompiledName("hrtime")>]
            member __.hrtime(?time : array<float>) : array<float> = failwith "never"
            [<JSEmitInline("({0}.send({args}))"); CompiledName("send4")>]
            member __.send(message : obj, ?sendHandle : obj) : unit = failwith "never"

    type ReadLine with 

            [<JSEmitInline("({0}.setPrompt({args}))"); CompiledName("setPrompt")>]
            member __.setPrompt(prompt : string, length : float) : unit = failwith "never"
            [<JSEmitInline("({0}.prompt({args}))"); CompiledName("prompt3")>]
            member __.prompt(?preserveCursor : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.question({args}))"); CompiledName("question")>]
            member __.question(query : string, callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.pause())"); CompiledName("pause2")>]
            member __.pause() : unit = failwith "never"
            [<JSEmitInline("({0}.resume())"); CompiledName("resume1")>]
            member __.resume() : unit = failwith "never"
            [<JSEmitInline("({0}.close())"); CompiledName("close10")>]
            member __.close() : unit = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write13")>]
            member __.write(data : obj, ?key : obj) : unit = failwith "never"

    type ReadLineOptions with 

            [<JSEmitInline("({0}.input)"); CompiledName("input1")>]
            member __.input with get() : ReadableStream = failwith "never" and set (v : ReadableStream) : unit = failwith "never"
            [<JSEmitInline("({0}.output)"); CompiledName("output")>]
            member __.output with get() : WritableStream = failwith "never" and set (v : WritableStream) : unit = failwith "never"
            [<JSEmitInline("({0}.completer)"); CompiledName("completer")>]
            member __.completer with get() : Function = failwith "never" and set (v : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.terminal)"); CompiledName("terminal")>]
            member __.terminal with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type ReadStream with 

            [<JSEmitInline("({0}.isRaw)"); CompiledName("isRaw")>]
            member __.isRaw with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.setRawMode({args}))"); CompiledName("setRawMode")>]
            member __.setRawMode(mode : bool) : unit = failwith "never"

    type Readable with 

            [<JSEmitInline("({0}.readable)"); CompiledName("readable")>]
            member __.readable with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("(new {0}.Readable({args}))"); CompiledName("Create476")>]
            member __.Create(?opts : ReadableOptions) : Readable = failwith "never"
            [<JSEmitInline("({0}._read({args}))"); CompiledName("_read")>]
            member __._read(size : float) : unit = failwith "never"
            [<JSEmitInline("({0}.read({args}))"); CompiledName("read")>]
            member __.read(?size : float) : obj = failwith "never"
            [<JSEmitInline("({0}.setEncoding({args}))"); CompiledName("setEncoding1")>]
            member __.setEncoding(encoding : string) : unit = failwith "never"
            [<JSEmitInline("({0}.pause())"); CompiledName("pause3")>]
            member __.pause() : unit = failwith "never"
            [<JSEmitInline("({0}.resume())"); CompiledName("resume2")>]
            member __.resume() : unit = failwith "never"
            [<JSEmitInline("({0}.pipe({args}))"); CompiledName("pipe")>]
            member __.pipe<'T when 'T :> WritableStream>(destination : 'T, ?options : AnonymousType483<'T>) : 'T = failwith "never"
            [<JSEmitInline("({0}.unpipe({args}))"); CompiledName("unpipe")>]
            member __.unpipe<'T when 'T :> WritableStream>(?destination : 'T) : unit = failwith "never"
            [<JSEmitInline("({0}.unshift({args}))"); CompiledName("unshift4")>]
            member __.unshift(chunk : string) : unit = failwith "never"
            [<JSEmitInline("({0}.unshift({args}))"); CompiledName("unshift5")>]
            member __.unshift(chunk : Buffer) : unit = failwith "never"
            [<JSEmitInline("({0}.wrap({args}))"); CompiledName("wrap1")>]
            member __.wrap(oldStream : ReadableStream) : ReadableStream = failwith "never"
            [<JSEmitInline("({0}.push({args}))"); CompiledName("push4")>]
            member __.push(chunk : obj, ?encoding : string) : bool = failwith "never"

    type ReadableOptions with 

            [<JSEmitInline("({0}.highWaterMark)"); CompiledName("highWaterMark")>]
            member __.highWaterMark with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.encoding)"); CompiledName("encoding17")>]
            member __.encoding with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.objectMode)"); CompiledName("objectMode")>]
            member __.objectMode with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type ReadableStream with 

            [<JSEmitInline("({0}.readable)"); CompiledName("readable1")>]
            member __.readable with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.read({args}))"); CompiledName("read1")>]
            member __.read(?size : float) : obj = failwith "never"
            [<JSEmitInline("({0}.setEncoding({args}))"); CompiledName("setEncoding2")>]
            member __.setEncoding(encoding : string) : unit = failwith "never"
            [<JSEmitInline("({0}.pause())"); CompiledName("pause4")>]
            member __.pause() : unit = failwith "never"
            [<JSEmitInline("({0}.resume())"); CompiledName("resume3")>]
            member __.resume() : unit = failwith "never"
            [<JSEmitInline("({0}.pipe({args}))"); CompiledName("pipe1")>]
            member __.pipe<'T when 'T :> WritableStream>(destination : 'T, ?options : AnonymousType435<'T>) : 'T = failwith "never"
            [<JSEmitInline("({0}.unpipe({args}))"); CompiledName("unpipe1")>]
            member __.unpipe<'T when 'T :> WritableStream>(?destination : 'T) : unit = failwith "never"
            [<JSEmitInline("({0}.unshift({args}))"); CompiledName("unshift6")>]
            member __.unshift(chunk : string) : unit = failwith "never"
            [<JSEmitInline("({0}.unshift({args}))"); CompiledName("unshift7")>]
            member __.unshift(chunk : Buffer) : unit = failwith "never"
            [<JSEmitInline("({0}.wrap({args}))"); CompiledName("wrap2")>]
            member __.wrap(oldStream : ReadableStream) : ReadableStream = failwith "never"

    type RemoteInfo with 

            [<JSEmitInline("({0}.address)"); CompiledName("address7")>]
            member __.address with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.port)"); CompiledName("port10")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.size)"); CompiledName("size7")>]
            member __.size with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type ReplOptions with 

            [<JSEmitInline("({0}.prompt)"); CompiledName("prompt4")>]
            member __.prompt with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.input)"); CompiledName("input2")>]
            member __.input with get() : ReadableStream = failwith "never" and set (v : ReadableStream) : unit = failwith "never"
            [<JSEmitInline("({0}.output)"); CompiledName("output1")>]
            member __.output with get() : WritableStream = failwith "never" and set (v : WritableStream) : unit = failwith "never"
            [<JSEmitInline("({0}.terminal)"); CompiledName("terminal1")>]
            member __.terminal with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.eval)"); CompiledName("eval1")>]
            member __.eval with get() : Function = failwith "never" and set (v : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.useColors)"); CompiledName("useColors")>]
            member __.useColors with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.useGlobal)"); CompiledName("useGlobal")>]
            member __.useGlobal with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.ignoreUndefined)"); CompiledName("ignoreUndefined")>]
            member __.ignoreUndefined with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writer)"); CompiledName("writer")>]
            member __.writer with get() : Function = failwith "never" and set (v : Function) : unit = failwith "never"

    type RequestOptions with 

            [<JSEmitInline("({0}.host)"); CompiledName("host4")>]
            member __.host with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.hostname)"); CompiledName("hostname3")>]
            member __.hostname with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.port)"); CompiledName("port12")>]
            member __.port with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.path)"); CompiledName("path1")>]
            member __.path with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.method)"); CompiledName("_method2")>]
            member __._method with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.headers)"); CompiledName("headers2")>]
            member __.headers with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.auth)"); CompiledName("auth")>]
            member __.auth with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.agent)"); CompiledName("agent")>]
            member __.agent with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.pfx)"); CompiledName("pfx2")>]
            member __.pfx with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.key)"); CompiledName("key8")>]
            member __.key with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.passphrase)"); CompiledName("passphrase2")>]
            member __.passphrase with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.cert)"); CompiledName("cert3")>]
            member __.cert with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.ca)"); CompiledName("ca3")>]
            member __.ca with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.ciphers)"); CompiledName("ciphers1")>]
            member __.ciphers with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.rejectUnauthorized)"); CompiledName("rejectUnauthorized1")>]
            member __.rejectUnauthorized with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type Script with 

            [<JSEmitInline("({0}.runInThisContext())"); CompiledName("runInThisContext")>]
            member __.runInThisContext() : unit = failwith "never"
            [<JSEmitInline("({0}.runInNewContext({args}))"); CompiledName("runInNewContext")>]
            member __.runInNewContext(?sandbox : Context) : unit = failwith "never"

    type SecurePair with 

            [<JSEmitInline("({0}.encrypted)"); CompiledName("encrypted")>]
            member __.encrypted with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.cleartext)"); CompiledName("cleartext")>]
            member __.cleartext with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"

    type Server with 

            [<JSEmitInline("({0}.listen({args}))"); CompiledName("listen6")>]
            member __.listen(port : float, ?host : string, ?backlog : float, ?listeningListener : Function) : Server = failwith "never"
            [<JSEmitInline("({0}.listen({args}))"); CompiledName("listen7")>]
            member __.listen(path : string, ?listeningListener : Function) : Server = failwith "never"
            [<JSEmitInline("({0}.listen({args}))"); CompiledName("listen8")>]
            member __.listen(handle : obj, ?listeningListener : Function) : Server = failwith "never"
            [<JSEmitInline("({0}.listen({args}))"); CompiledName("listen9")>]
            member __.listenOverload2(port : float, ?host : string, ?callback : Function) : Server = failwith "never"
            [<JSEmitInline("({0}.close())"); CompiledName("close13")>]
            member __.close() : Server = failwith "never"
            [<JSEmitInline("({0}.address())"); CompiledName("address10")>]
            member __.address() : AnonymousType478 = failwith "never"
            [<JSEmitInline("({0}.addContext({args}))"); CompiledName("addContext")>]
            member __.addContext(hostName : string, credentials : AnonymousType479) : unit = failwith "never"
            [<JSEmitInline("({0}.maxConnections)"); CompiledName("maxConnections1")>]
            member __.maxConnections with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.connections)"); CompiledName("connections1")>]
            member __.connections with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.maxHeadersCount)"); CompiledName("maxHeadersCount")>]
            member __.maxHeadersCount with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type ServerOptions with 

            [<JSEmitInline("({0}.pfx)"); CompiledName("pfx3")>]
            member __.pfx with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.key)"); CompiledName("key9")>]
            member __.key with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.passphrase)"); CompiledName("passphrase3")>]
            member __.passphrase with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.cert)"); CompiledName("cert4")>]
            member __.cert with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.ca)"); CompiledName("ca4")>]
            member __.ca with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.crl)"); CompiledName("crl1")>]
            member __.crl with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.ciphers)"); CompiledName("ciphers2")>]
            member __.ciphers with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.honorCipherOrder)"); CompiledName("honorCipherOrder")>]
            member __.honorCipherOrder with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.requestCert)"); CompiledName("requestCert")>]
            member __.requestCert with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.rejectUnauthorized)"); CompiledName("rejectUnauthorized2")>]
            member __.rejectUnauthorized with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.NPNProtocols)"); CompiledName("NPNProtocols1")>]
            member __.NPNProtocols with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.SNICallback)"); CompiledName("SNICallback")>]
            member __.SNICallback with get() : Func<string, obj> = failwith "never" and set (v : Func<string, obj>) : unit = failwith "never"

    type ServerRequest with 

            [<JSEmitInline("({0}.method)"); CompiledName("_method3")>]
            member __._method with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.url)"); CompiledName("url4")>]
            member __.url with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.headers)"); CompiledName("headers3")>]
            member __.headers with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.trailers)"); CompiledName("trailers1")>]
            member __.trailers with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.httpVersion)"); CompiledName("httpVersion1")>]
            member __.httpVersion with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.setEncoding({args}))"); CompiledName("setEncoding3")>]
            member __.setEncoding(?encoding : string) : unit = failwith "never"
            [<JSEmitInline("({0}.pause())"); CompiledName("pause5")>]
            member __.pause() : unit = failwith "never"
            [<JSEmitInline("({0}.resume())"); CompiledName("resume4")>]
            member __.resume() : unit = failwith "never"
            [<JSEmitInline("({0}.connection)"); CompiledName("connection")>]
            member __.connection with get() : Socket = failwith "never" and set (v : Socket) : unit = failwith "never"

    type ServerResponse with 

            [<JSEmitInline("({0}.write({args}))"); CompiledName("write14")>]
            member __.write(buffer : Buffer) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write15")>]
            member __.writeOverload2(buffer : Buffer, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write16")>]
            member __.write(str : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write17")>]
            member __.writeOverload2(str : string, ?encoding : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write18")>]
            member __.writeOverload3(str : string, ?encoding : string, ?fd : string) : bool = failwith "never"
            [<JSEmitInline("({0}.writeContinue())"); CompiledName("writeContinue")>]
            member __.writeContinue() : unit = failwith "never"
            [<JSEmitInline("({0}.writeHead({args}))"); CompiledName("writeHead")>]
            member __.writeHead(statusCode : float, ?reasonPhrase : string, ?headers : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.writeHead({args}))"); CompiledName("writeHead1")>]
            member __.writeHeadOverload2(statusCode : float, ?headers : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.statusCode)"); CompiledName("statusCode1")>]
            member __.statusCode with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.setHeader({args}))"); CompiledName("setHeader")>]
            member __.setHeader(name : string, value : string) : unit = failwith "never"
            [<JSEmitInline("({0}.sendDate)"); CompiledName("sendDate")>]
            member __.sendDate with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.getHeader({args}))"); CompiledName("getHeader")>]
            member __.getHeader(name : string) : string = failwith "never"
            [<JSEmitInline("({0}.removeHeader({args}))"); CompiledName("removeHeader")>]
            member __.removeHeader(name : string) : unit = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write19")>]
            member __.write(chunk : obj, ?encoding : string) : obj = failwith "never"
            [<JSEmitInline("({0}.addTrailers({args}))"); CompiledName("addTrailers")>]
            member __.addTrailers(headers : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.end())"); CompiledName("_end14")>]
            member __._end() : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end15")>]
            member __._end(buffer : Buffer, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end16")>]
            member __._end(str : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end17")>]
            member __._endOverload2(str : string, ?encoding : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end18")>]
            member __._endOverload2(?data : obj, ?encoding : string) : unit = failwith "never"

    type Signer with 

            [<JSEmitInline("({0}.update({args}))"); CompiledName("update9")>]
            member __.update(data : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.sign({args}))"); CompiledName("sign1")>]
            member __.sign(private_key : string, output_format : string) : string = failwith "never"

    type dgramSocket with 
            [<JSEmitInline("({0}.send({args}))"); CompiledName("send5")>]
            member __.send(buf : Buffer, offset : float, length : float, port : float, address : string, ?callback : Func<Error, float, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.bind({args}))"); CompiledName("bind3")>]
            member __.bind(port : float, ?address : string, ?callback : Func<unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.close())"); CompiledName("close14")>]
            member __.close() : unit = failwith "never"
            [<JSEmitInline("({0}.address())"); CompiledName("address11")>]
            member __.address() : AddressInfo = failwith "never"
            [<JSEmitInline("({0}.setBroadcast({args}))"); CompiledName("setBroadcast")>]
            member __.setBroadcast(flag : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.setMulticastTTL({args}))"); CompiledName("setMulticastTTL")>]
            member __.setMulticastTTL(ttl : float) : unit = failwith "never"
            [<JSEmitInline("({0}.setMulticastLoopback({args}))"); CompiledName("setMulticastLoopback")>]
            member __.setMulticastLoopback(flag : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.addMembership({args}))"); CompiledName("addMembership")>]
            member __.addMembership(multicastAddress : string, ?multicastInterface : string) : unit = failwith "never"
            [<JSEmitInline("({0}.dropMembership({args}))"); CompiledName("dropMembership")>]
            member __.dropMembership(multicastAddress : string, ?multicastInterface : string) : unit = failwith "never"

    type Socket with
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write20")>]
            member __.write(buffer : Buffer) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write21")>]
            member __.writeOverload2(buffer : Buffer, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write22")>]
            member __.write(str : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write23")>]
            member __.writeOverload2(str : string, ?encoding : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write24")>]
            member __.writeOverload3(str : string, ?encoding : string, ?fd : string) : bool = failwith "never"
            [<JSEmitInline("({0}.connect({args}))"); CompiledName("connect")>]
            member __.connect(port : float, ?host : string, ?connectionListener : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.connect({args}))"); CompiledName("connect1")>]
            member __.connect(path : string, ?connectionListener : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.bufferSize)"); CompiledName("bufferSize2")>]
            member __.bufferSize with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.setEncoding({args}))"); CompiledName("setEncoding4")>]
            member __.setEncoding(?encoding : string) : unit = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write25")>]
            member __.write(data : obj, ?encoding : string, ?callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.destroy())"); CompiledName("destroy")>]
            member __.destroy() : unit = failwith "never"
            [<JSEmitInline("({0}.pause())"); CompiledName("pause6")>]
            member __.pause() : unit = failwith "never"
            [<JSEmitInline("({0}.resume())"); CompiledName("resume5")>]
            member __.resume() : unit = failwith "never"
            [<JSEmitInline("({0}.setTimeout({args}))"); CompiledName("setTimeout7")>]
            member __.setTimeout(timeout : float, ?callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.setNoDelay({args}))"); CompiledName("setNoDelay1")>]
            member __.setNoDelay(?noDelay : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.setKeepAlive({args}))"); CompiledName("setKeepAlive")>]
            member __.setKeepAlive(?enable : bool, ?initialDelay : float) : unit = failwith "never"
            [<JSEmitInline("({0}.address())"); CompiledName("address12")>]
            member __.address() : AnonymousType451 = failwith "never"
            [<JSEmitInline("({0}.unref())"); CompiledName("unref")>]
            member __.unref() : unit = failwith "never"
            [<JSEmitInline("({0}.ref())"); CompiledName("_ref")>]
            member __._ref() : unit = failwith "never"
            [<JSEmitInline("({0}.remoteAddress)"); CompiledName("remoteAddress1")>]
            member __.remoteAddress with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.remotePort)"); CompiledName("remotePort1")>]
            member __.remotePort with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.bytesRead)"); CompiledName("bytesRead")>]
            member __.bytesRead with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.bytesWritten)"); CompiledName("bytesWritten")>]
            member __.bytesWritten with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.end())"); CompiledName("_end19")>]
            member __._end() : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end20")>]
            member __._end(buffer : Buffer, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end21")>]
            member __._end(str : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end22")>]
            member __._endOverload2(str : string, ?encoding : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end23")>]
            member __._endOverload2(?data : obj, ?encoding : string) : unit = failwith "never"

    type Stats with 

            [<JSEmitInline("({0}.isFile())"); CompiledName("isFile")>]
            member __.isFile() : bool = failwith "never"
            [<JSEmitInline("({0}.isDirectory())"); CompiledName("isDirectory")>]
            member __.isDirectory() : bool = failwith "never"
            [<JSEmitInline("({0}.isBlockDevice())"); CompiledName("isBlockDevice")>]
            member __.isBlockDevice() : bool = failwith "never"
            [<JSEmitInline("({0}.isCharacterDevice())"); CompiledName("isCharacterDevice")>]
            member __.isCharacterDevice() : bool = failwith "never"
            [<JSEmitInline("({0}.isSymbolicLink())"); CompiledName("isSymbolicLink")>]
            member __.isSymbolicLink() : bool = failwith "never"
            [<JSEmitInline("({0}.isFIFO())"); CompiledName("isFIFO")>]
            member __.isFIFO() : bool = failwith "never"
            [<JSEmitInline("({0}.isSocket())"); CompiledName("isSocket")>]
            member __.isSocket() : bool = failwith "never"
            [<JSEmitInline("({0}.dev)"); CompiledName("dev")>]
            member __.dev with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.ino)"); CompiledName("ino")>]
            member __.ino with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.mode)"); CompiledName("mode13")>]
            member __.mode with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.nlink)"); CompiledName("nlink")>]
            member __.nlink with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.uid)"); CompiledName("uid")>]
            member __.uid with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.gid)"); CompiledName("gid")>]
            member __.gid with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.rdev)"); CompiledName("rdev")>]
            member __.rdev with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.size)"); CompiledName("size8")>]
            member __.size with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.blksize)"); CompiledName("blksize")>]
            member __.blksize with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.blocks)"); CompiledName("blocks")>]
            member __.blocks with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.atime)"); CompiledName("atime")>]
            member __.atime with get() : Date = failwith "never" and set (v : Date) : unit = failwith "never"
            [<JSEmitInline("({0}.mtime)"); CompiledName("mtime")>]
            member __.mtime with get() : Date = failwith "never" and set (v : Date) : unit = failwith "never"
            [<JSEmitInline("({0}.ctime)"); CompiledName("ctime")>]
            member __.ctime with get() : Date = failwith "never" and set (v : Date) : unit = failwith "never"

    type Stream with 

            [<JSEmitInline("({0}.pipe({args}))"); CompiledName("pipe2")>]
            member __.pipe<'T when 'T :> WritableStream>(destination : 'T, ?options : AnonymousType482<'T>) : 'T = failwith "never"

    type Timer with 

            [<JSEmitInline("({0}.ref())"); CompiledName("_ref1")>]
            member __._ref() : unit = failwith "never"
            [<JSEmitInline("({0}.unref())"); CompiledName("unref1")>]
            member __.unref() : unit = failwith "never"

    type TlsOptions with 

            [<JSEmitInline("({0}.pfx)"); CompiledName("pfx4")>]
            member __.pfx with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.key)"); CompiledName("key10")>]
            member __.key with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.passphrase)"); CompiledName("passphrase4")>]
            member __.passphrase with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.cert)"); CompiledName("cert5")>]
            member __.cert with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.ca)"); CompiledName("ca5")>]
            member __.ca with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.crl)"); CompiledName("crl2")>]
            member __.crl with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.ciphers)"); CompiledName("ciphers3")>]
            member __.ciphers with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.honorCipherOrder)"); CompiledName("honorCipherOrder1")>]
            member __.honorCipherOrder with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.requestCert)"); CompiledName("requestCert1")>]
            member __.requestCert with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.rejectUnauthorized)"); CompiledName("rejectUnauthorized3")>]
            member __.rejectUnauthorized with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.NPNProtocols)"); CompiledName("NPNProtocols2")>]
            member __.NPNProtocols with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.SNICallback)"); CompiledName("SNICallback1")>]
            member __.SNICallback with get() : Func<string, obj> = failwith "never" and set (v : Func<string, obj>) : unit = failwith "never"

    type Transform with 

            [<JSEmitInline("({0}.readable)"); CompiledName("readable2")>]
            member __.readable with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.writable)"); CompiledName("writable2")>]
            member __.writable with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("(new {0}.Transform({args}))"); CompiledName("Create477")>]
            member __.Create(?opts : TransformOptions) : Transform = failwith "never"
            [<JSEmitInline("({0}._transform({args}))"); CompiledName("_transform")>]
            member __._transform(chunk : Buffer, encoding : string, callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}._transform({args}))"); CompiledName("_transform1")>]
            member __._transform(chunk : string, encoding : string, callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}._flush({args}))"); CompiledName("_flush")>]
            member __._flush(callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.read({args}))"); CompiledName("read2")>]
            member __.read(?size : float) : obj = failwith "never"
            [<JSEmitInline("({0}.setEncoding({args}))"); CompiledName("setEncoding5")>]
            member __.setEncoding(encoding : string) : unit = failwith "never"
            [<JSEmitInline("({0}.pause())"); CompiledName("pause7")>]
            member __.pause() : unit = failwith "never"
            [<JSEmitInline("({0}.resume())"); CompiledName("resume6")>]
            member __.resume() : unit = failwith "never"
            [<JSEmitInline("({0}.pipe({args}))"); CompiledName("pipe3")>]
            member __.pipe<'T when 'T :> WritableStream>(destination : 'T, ?options : AnonymousType484<'T>) : 'T = failwith "never"
            [<JSEmitInline("({0}.unpipe({args}))"); CompiledName("unpipe2")>]
            member __.unpipe<'T when 'T :> WritableStream>(?destination : 'T) : unit = failwith "never"
            [<JSEmitInline("({0}.unshift({args}))"); CompiledName("unshift8")>]
            member __.unshift(chunk : string) : unit = failwith "never"
            [<JSEmitInline("({0}.unshift({args}))"); CompiledName("unshift9")>]
            member __.unshift(chunk : Buffer) : unit = failwith "never"
            [<JSEmitInline("({0}.wrap({args}))"); CompiledName("wrap3")>]
            member __.wrap(oldStream : ReadableStream) : ReadableStream = failwith "never"
            [<JSEmitInline("({0}.push({args}))"); CompiledName("push5")>]
            member __.push(chunk : obj, ?encoding : string) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write26")>]
            member __.write(buffer : Buffer, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write27")>]
            member __.write(str : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write28")>]
            member __.writeOverload2(str : string, ?encoding : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.end())"); CompiledName("_end24")>]
            member __._end() : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end25")>]
            member __._end(buffer : Buffer, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end26")>]
            member __._end(str : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end27")>]
            member __._endOverload2(str : string, ?encoding : string, ?cb : Function) : unit = failwith "never"

    type Url with 

            [<JSEmitInline("({0}.href)"); CompiledName("href10")>]
            member __.href with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.protocol)"); CompiledName("protocol5")>]
            member __.protocol with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.auth)"); CompiledName("auth1")>]
            member __.auth with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.hostname)"); CompiledName("hostname4")>]
            member __.hostname with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.port)"); CompiledName("port13")>]
            member __.port with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.host)"); CompiledName("host5")>]
            member __.host with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.pathname)"); CompiledName("pathname3")>]
            member __.pathname with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.search)"); CompiledName("search5")>]
            member __.search with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.query)"); CompiledName("query")>]
            member __.query with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.slashes)"); CompiledName("slashes")>]
            member __.slashes with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.hash)"); CompiledName("hash3")>]
            member __.hash with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.path)"); CompiledName("path2")>]
            member __.path with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type UrlOptions with 

            [<JSEmitInline("({0}.protocol)"); CompiledName("protocol6")>]
            member __.protocol with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.auth)"); CompiledName("auth2")>]
            member __.auth with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.hostname)"); CompiledName("hostname5")>]
            member __.hostname with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.port)"); CompiledName("port14")>]
            member __.port with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.host)"); CompiledName("host6")>]
            member __.host with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.pathname)"); CompiledName("pathname4")>]
            member __.pathname with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.search)"); CompiledName("search6")>]
            member __.search with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.query)"); CompiledName("query1")>]
            member __.query with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.hash)"); CompiledName("hash4")>]
            member __.hash with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.path)"); CompiledName("path3")>]
            member __.path with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type Verify with 

            [<JSEmitInline("({0}.update({args}))"); CompiledName("update10")>]
            member __.update(data : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.verify({args}))"); CompiledName("verify1")>]
            member __.verify(_object : string, signature : string, ?signature_format : string) : bool = failwith "never"

    type Worker with 

            [<JSEmitInline("({0}.id)"); CompiledName("id6")>]
            member __.id with get() : string = failwith "never" and set (v : string) : unit = failwith "never"
            [<JSEmitInline("({0}.process)"); CompiledName("_process2")>]
            member __._process with get() : ChildProcess = failwith "never" and set (v : ChildProcess) : unit = failwith "never"
            [<JSEmitInline("({0}.suicide)"); CompiledName("suicide")>]
            member __.suicide with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.send({args}))"); CompiledName("send6")>]
            member __.send(message : obj, ?sendHandle : obj) : unit = failwith "never"
            [<JSEmitInline("({0}.kill({args}))"); CompiledName("kill2")>]
            member __.kill(?signal : string) : unit = failwith "never"
            [<JSEmitInline("({0}.destroy({args}))"); CompiledName("destroy1")>]
            member __.destroy(?signal : string) : unit = failwith "never"
            [<JSEmitInline("({0}.disconnect())"); CompiledName("disconnect2")>]
            member __.disconnect() : unit = failwith "never"

    type Writable with 

            [<JSEmitInline("({0}.writable)"); CompiledName("writable3")>]
            member __.writable with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("(new {0}.Writable({args}))"); CompiledName("Create478")>]
            member __.Create(?opts : WritableOptions) : Writable = failwith "never"
            [<JSEmitInline("({0}._write({args}))"); CompiledName("_write2")>]
            member __._write(data : Buffer, encoding : string, callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}._write({args}))"); CompiledName("_write3")>]
            member __._write(data : string, encoding : string, callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write29")>]
            member __.write(buffer : Buffer, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write30")>]
            member __.write(str : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write31")>]
            member __.writeOverload2(str : string, ?encoding : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.end())"); CompiledName("_end28")>]
            member __._end() : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end29")>]
            member __._end(buffer : Buffer, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end30")>]
            member __._end(str : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end31")>]
            member __._endOverload2(str : string, ?encoding : string, ?cb : Function) : unit = failwith "never"

    type WritableOptions with 

            [<JSEmitInline("({0}.highWaterMark)"); CompiledName("highWaterMark1")>]
            member __.highWaterMark with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.decodeStrings)"); CompiledName("decodeStrings")>]
            member __.decodeStrings with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"

    type WritableStream with 

            [<JSEmitInline("({0}.writable)"); CompiledName("writable4")>]
            member __.writable with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write32")>]
            member __.write(buffer : Buffer, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write33")>]
            member __.write(str : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write34")>]
            member __.writeOverload2(str : string, ?encoding : string, ?cb : Function) : bool = failwith "never"
            [<JSEmitInline("({0}.end())"); CompiledName("_end32")>]
            member __._end() : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end33")>]
            member __._end(buffer : Buffer, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end34")>]
            member __._end(str : string, ?cb : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.end({args}))"); CompiledName("_end35")>]
            member __._endOverload2(str : string, ?encoding : string, ?cb : Function) : unit = failwith "never"

    type WriteStream with 

            [<JSEmitInline("({0}.columns)"); CompiledName("columns1")>]
            member __.columns with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.rows)"); CompiledName("rows4")>]
            member __.rows with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type ZlibOptions with 

            [<JSEmitInline("({0}.chunkSize)"); CompiledName("chunkSize")>]
            member __.chunkSize with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.windowBits)"); CompiledName("windowBits")>]
            member __.windowBits with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.level)"); CompiledName("level")>]
            member __.level with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.memLevel)"); CompiledName("memLevel")>]
            member __.memLevel with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.strategy)"); CompiledName("strategy")>]
            member __.strategy with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.dictionary)"); CompiledName("dictionary")>]
            member __.dictionary with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"

    type _assert with 

            [<JSEmitInline("({0}.internal({args}))"); CompiledName("_internal")>]
            member __._internal(value : obj, ?message : string) : unit = failwith "never"

    type buffer with 

            [<JSEmitInline("({0}.INSPECT_MAX_BYTES)"); CompiledName("INSPECT_MAX_BYTES")>]
            member __.INSPECT_MAX_BYTES with get() : float = failwith "never" and set (v : float) : unit = failwith "never"

    type child_process with 

            [<JSEmitInline("({0}.spawn({args}))"); CompiledName("spawn")>]
            member __.spawn(command : string, ?args : array<string>, ?options : AnonymousType447) : ChildProcess = failwith "never"
            [<JSEmitInline("({0}.exec({args}))"); CompiledName("exec2")>]
            member __.exec(command : string, options : AnonymousType448, callback : Func<Error, Buffer, Buffer, unit>) : ChildProcess = failwith "never"
            [<JSEmitInline("({0}.exec({args}))"); CompiledName("exec3")>]
            member __.exec(command : string, callback : Func<Error, Buffer, Buffer, unit>) : ChildProcess = failwith "never"
            [<JSEmitInline("({0}.execFile({args}))"); CompiledName("execFile")>]
            member __.execFile(file : string, args : array<string>, options : AnonymousType449, callback : Func<Error, Buffer, Buffer, unit>) : ChildProcess = failwith "never"
            [<JSEmitInline("({0}.fork({args}))"); CompiledName("fork")>]
            member __.fork(modulePath : string, ?args : array<string>, ?options : AnonymousType450) : ChildProcess = failwith "never"

    type cluster with 

            [<JSEmitInline("({0}.settings)"); CompiledName("settings")>]
            member __.settings with get() : ClusterSettings = failwith "never" and set (v : ClusterSettings) : unit = failwith "never"
            [<JSEmitInline("({0}.isMaster)"); CompiledName("isMaster")>]
            member __.isMaster with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.isWorker)"); CompiledName("isWorker")>]
            member __.isWorker with get() : bool = failwith "never" and set (v : bool) : unit = failwith "never"
            [<JSEmitInline("({0}.setupMaster({args}))"); CompiledName("setupMaster")>]
            member __.setupMaster(?settings : ClusterSettings) : unit = failwith "never"
            [<JSEmitInline("({0}.fork({args}))"); CompiledName("fork1")>]
            member __.fork(?env : obj) : Worker = failwith "never"
            [<JSEmitInline("({0}.disconnect({args}))"); CompiledName("disconnect3")>]
            member __.disconnect(?callback : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.worker)"); CompiledName("worker")>]
            member __.worker with get() : Worker = failwith "never" and set (v : Worker) : unit = failwith "never"
            [<JSEmitInline("({0}.workers)"); CompiledName("workers")>]
            member __.workers with get() : array<Worker> = failwith "never" and set (v : array<Worker>) : unit = failwith "never"
            [<JSEmitInline("({0}.addListener({args}))"); CompiledName("addListener4")>]
            member __.addListener(_event : string, listener : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.on({args}))"); CompiledName("on3")>]
            member __.on(_event : string, listener : Function) : obj = failwith "never"
            [<JSEmitInline("({0}.once({args}))"); CompiledName("once3")>]
            member __.once(_event : string, listener : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.removeListener({args}))"); CompiledName("removeListener4")>]
            member __.removeListener(_event : string, listener : Function) : unit = failwith "never"
            [<JSEmitInline("({0}.removeAllListeners({args}))"); CompiledName("removeAllListeners3")>]
            member __.removeAllListeners(?_event : string) : unit = failwith "never"
            [<JSEmitInline("({0}.setMaxListeners({args}))"); CompiledName("setMaxListeners2")>]
            member __.setMaxListeners(n : float) : unit = failwith "never"
            [<JSEmitInline("({0}.listeners({args}))"); CompiledName("listeners2")>]
            member __.listeners(_event : string) : array<Function> = failwith "never"
            [<JSEmitInline("({0}.emit({args}))"); CompiledName("emit4")>]
            member __.emit(_event : string) : bool = failwith "never"
            [<JSEmitInline("({0}.emit({args}))"); CompiledName("emit5")>]
            member __.emitOverload2(_event : string, [<System.ParamArray>] args : array<obj>) : bool = failwith "never"

    type crypto with 

            [<JSEmitInline("({0}.createCredentials({args}))"); CompiledName("createCredentials")>]
            member __.createCredentials(details : CredentialDetails) : Credentials = failwith "never"
            [<JSEmitInline("({0}.createHash({args}))"); CompiledName("createHash")>]
            member __.createHash(algorithm : string) : Hash = failwith "never"
            [<JSEmitInline("({0}.createHmac({args}))"); CompiledName("createHmac")>]
            member __.createHmac(algorithm : string, key : string) : Hmac = failwith "never"
            [<JSEmitInline("({0}.createHmac({args}))"); CompiledName("createHmac1")>]
            member __.createHmac(algorithm : string, key : Buffer) : Hmac = failwith "never"
            [<JSEmitInline("({0}.createCipher({args}))"); CompiledName("createCipher")>]
            member __.createCipher(algorithm : string, password : obj) : Cipher = failwith "never"
            [<JSEmitInline("({0}.createCipheriv({args}))"); CompiledName("createCipheriv")>]
            member __.createCipheriv(algorithm : string, key : obj, iv : obj) : Cipher = failwith "never"
            [<JSEmitInline("({0}.createDecipher({args}))"); CompiledName("createDecipher")>]
            member __.createDecipher(algorithm : string, password : obj) : Decipher = failwith "never"
            [<JSEmitInline("({0}.createDecipheriv({args}))"); CompiledName("createDecipheriv")>]
            member __.createDecipheriv(algorithm : string, key : obj, iv : obj) : Decipher = failwith "never"
            [<JSEmitInline("({0}.createSign({args}))"); CompiledName("createSign")>]
            member __.createSign(algorithm : string) : Signer = failwith "never"
            [<JSEmitInline("({0}.createVerify({args}))"); CompiledName("createVerify")>]
            member __.createVerify(algorith : string) : Verify = failwith "never"
            [<JSEmitInline("({0}.createDiffieHellman({args}))"); CompiledName("createDiffieHellman")>]
            member __.createDiffieHellman(prime_length : float) : DiffieHellman = failwith "never"
            [<JSEmitInline("({0}.createDiffieHellman({args}))"); CompiledName("createDiffieHellman1")>]
            member __.createDiffieHellmanOverload2(prime : float, ?encoding : string) : DiffieHellman = failwith "never"
            [<JSEmitInline("({0}.getDiffieHellman({args}))"); CompiledName("getDiffieHellman")>]
            member __.getDiffieHellman(group_name : string) : DiffieHellman = failwith "never"
            [<JSEmitInline("({0}.pbkdf2({args}))"); CompiledName("pbkdf2")>]
            member __.pbkdf2(password : string, salt : string, iterations : float, keylen : float, callback : Func<Error, Buffer, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.pbkdf2Sync({args}))"); CompiledName("pbkdf2Sync")>]
            member __.pbkdf2Sync(password : string, salt : string, iterations : float, keylen : float) : Buffer = failwith "never"
            [<JSEmitInline("({0}.randomBytes({args}))"); CompiledName("randomBytes")>]
            member __.randomBytes(size : float) : Buffer = failwith "never"
            [<JSEmitInline("({0}.randomBytes({args}))"); CompiledName("randomBytes1")>]
            member __.randomBytes(size : float, callback : Func<Error, Buffer, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.pseudoRandomBytes({args}))"); CompiledName("pseudoRandomBytes")>]
            member __.pseudoRandomBytes(size : float) : Buffer = failwith "never"
            [<JSEmitInline("({0}.pseudoRandomBytes({args}))"); CompiledName("pseudoRandomBytes1")>]
            member __.pseudoRandomBytes(size : float, callback : Func<Error, Buffer, unit>) : unit = failwith "never"

    type dgram with 

            [<JSEmitInline("({0}.createSocket({args}))"); CompiledName("createSocket")>]
            member __.createSocket(_type : string, ?callback : Func<Buffer, RemoteInfo, unit>) : Socket = failwith "never"

    type dns with 

            [<JSEmitInline("({0}.lookup({args}))"); CompiledName("lookup")>]
            member __.lookup(domain : string, family : float, callback : Func<Error, string, float, unit>) : string = failwith "never"
            [<JSEmitInline("({0}.lookup({args}))"); CompiledName("lookup1")>]
            member __.lookup(domain : string, callback : Func<Error, string, float, unit>) : string = failwith "never"
            [<JSEmitInline("({0}.resolve({args}))"); CompiledName("resolve1")>]
            member __.resolve(domain : string, rrtype : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.resolve({args}))"); CompiledName("resolve2")>]
            member __.resolve(domain : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.resolve4({args}))"); CompiledName("resolve4")>]
            member __.resolve4(domain : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.resolve6({args}))"); CompiledName("resolve6")>]
            member __.resolve6(domain : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.resolveMx({args}))"); CompiledName("resolveMx")>]
            member __.resolveMx(domain : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.resolveTxt({args}))"); CompiledName("resolveTxt")>]
            member __.resolveTxt(domain : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.resolveSrv({args}))"); CompiledName("resolveSrv")>]
            member __.resolveSrv(domain : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.resolveNs({args}))"); CompiledName("resolveNs")>]
            member __.resolveNs(domain : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.resolveCname({args}))"); CompiledName("resolveCname")>]
            member __.resolveCname(domain : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"
            [<JSEmitInline("({0}.reverse({args}))"); CompiledName("reverse2")>]
            member __.reverse(ip : string, callback : Func<Error, array<string>, unit>) : array<string> = failwith "never"

    type domain with 

            [<JSEmitInline("({0}.create())"); CompiledName("create5")>]
            member __.create() : Domain = failwith "never"

    type fs with 

            [<JSEmitInline("({0}.rename({args}))"); CompiledName("rename")>]
            member __.rename(oldPath : string, newPath : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.renameSync({args}))"); CompiledName("renameSync")>]
            member __.renameSync(oldPath : string, newPath : string) : unit = failwith "never"
            [<JSEmitInline("({0}.truncate({args}))"); CompiledName("truncate")>]
            member __.truncate(path : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.truncate({args}))"); CompiledName("truncate1")>]
            member __.truncate(path : string, len : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.truncateSync({args}))"); CompiledName("truncateSync")>]
            member __.truncateSync(path : string, ?len : float) : unit = failwith "never"
            [<JSEmitInline("({0}.ftruncate({args}))"); CompiledName("ftruncate")>]
            member __.ftruncate(fd : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.ftruncate({args}))"); CompiledName("ftruncate1")>]
            member __.ftruncate(fd : float, len : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.ftruncateSync({args}))"); CompiledName("ftruncateSync")>]
            member __.ftruncateSync(fd : float, ?len : float) : unit = failwith "never"
            [<JSEmitInline("({0}.chown({args}))"); CompiledName("chown")>]
            member __.chown(path : string, uid : float, gid : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.chownSync({args}))"); CompiledName("chownSync")>]
            member __.chownSync(path : string, uid : float, gid : float) : unit = failwith "never"
            [<JSEmitInline("({0}.fchown({args}))"); CompiledName("fchown")>]
            member __.fchown(fd : float, uid : float, gid : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.fchownSync({args}))"); CompiledName("fchownSync")>]
            member __.fchownSync(fd : float, uid : float, gid : float) : unit = failwith "never"
            [<JSEmitInline("({0}.lchown({args}))"); CompiledName("lchown")>]
            member __.lchown(path : string, uid : float, gid : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.lchownSync({args}))"); CompiledName("lchownSync")>]
            member __.lchownSync(path : string, uid : float, gid : float) : unit = failwith "never"
            [<JSEmitInline("({0}.chmod({args}))"); CompiledName("chmod")>]
            member __.chmod(path : string, mode : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.chmod({args}))"); CompiledName("chmod1")>]
            member __.chmod(path : string, mode : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.chmodSync({args}))"); CompiledName("chmodSync")>]
            member __.chmodSync(path : string, mode : float) : unit = failwith "never"
            [<JSEmitInline("({0}.chmodSync({args}))"); CompiledName("chmodSync1")>]
            member __.chmodSync(path : string, mode : string) : unit = failwith "never"
            [<JSEmitInline("({0}.fchmod({args}))"); CompiledName("fchmod")>]
            member __.fchmod(fd : float, mode : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.fchmod({args}))"); CompiledName("fchmod1")>]
            member __.fchmod(fd : float, mode : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.fchmodSync({args}))"); CompiledName("fchmodSync")>]
            member __.fchmodSync(fd : float, mode : float) : unit = failwith "never"
            [<JSEmitInline("({0}.fchmodSync({args}))"); CompiledName("fchmodSync1")>]
            member __.fchmodSync(fd : float, mode : string) : unit = failwith "never"
            [<JSEmitInline("({0}.lchmod({args}))"); CompiledName("lchmod")>]
            member __.lchmod(path : string, mode : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.lchmod({args}))"); CompiledName("lchmod1")>]
            member __.lchmod(path : string, mode : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.lchmodSync({args}))"); CompiledName("lchmodSync")>]
            member __.lchmodSync(path : string, mode : float) : unit = failwith "never"
            [<JSEmitInline("({0}.lchmodSync({args}))"); CompiledName("lchmodSync1")>]
            member __.lchmodSync(path : string, mode : string) : unit = failwith "never"
            [<JSEmitInline("({0}.stat({args}))"); CompiledName("stat")>]
            member __.stat(path : string, ?callback : Func<ErrnoException, Stats, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.lstat({args}))"); CompiledName("lstat")>]
            member __.lstat(path : string, ?callback : Func<ErrnoException, Stats, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.fstat({args}))"); CompiledName("fstat")>]
            member __.fstat(fd : float, ?callback : Func<ErrnoException, Stats, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.statSync({args}))"); CompiledName("statSync")>]
            member __.statSync(path : string) : Stats = failwith "never"
            [<JSEmitInline("({0}.lstatSync({args}))"); CompiledName("lstatSync")>]
            member __.lstatSync(path : string) : Stats = failwith "never"
            [<JSEmitInline("({0}.fstatSync({args}))"); CompiledName("fstatSync")>]
            member __.fstatSync(fd : float) : Stats = failwith "never"
            [<JSEmitInline("({0}.link({args}))"); CompiledName("link1")>]
            member __.link(srcpath : string, dstpath : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.linkSync({args}))"); CompiledName("linkSync")>]
            member __.linkSync(srcpath : string, dstpath : string) : unit = failwith "never"
            [<JSEmitInline("({0}.symlink({args}))"); CompiledName("symlink")>]
            member __.symlink(srcpath : string, dstpath : string, ?_type : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.symlinkSync({args}))"); CompiledName("symlinkSync")>]
            member __.symlinkSync(srcpath : string, dstpath : string, ?_type : string) : unit = failwith "never"
            [<JSEmitInline("({0}.readlink({args}))"); CompiledName("readlink")>]
            member __.readlink(path : string, ?callback : Func<ErrnoException, string, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.readlinkSync({args}))"); CompiledName("readlinkSync")>]
            member __.readlinkSync(path : string) : string = failwith "never"
            [<JSEmitInline("({0}.realpath({args}))"); CompiledName("realpath")>]
            member __.realpath(path : string, ?callback : Func<ErrnoException, string, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.realpath({args}))"); CompiledName("realpath1")>]
            member __.realpath(path : string, cache : AnonymousType458, callback : Func<ErrnoException, string, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.realpathSync({args}))"); CompiledName("realpathSync")>]
            member __.realpathSync(path : string, ?cache : AnonymousType459) : string = failwith "never"
            [<JSEmitInline("({0}.unlink({args}))"); CompiledName("unlink")>]
            member __.unlink(path : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.unlinkSync({args}))"); CompiledName("unlinkSync")>]
            member __.unlinkSync(path : string) : unit = failwith "never"
            [<JSEmitInline("({0}.rmdir({args}))"); CompiledName("rmdir")>]
            member __.rmdir(path : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.rmdirSync({args}))"); CompiledName("rmdirSync")>]
            member __.rmdirSync(path : string) : unit = failwith "never"
            [<JSEmitInline("({0}.mkdir({args}))"); CompiledName("mkdir")>]
            member __.mkdir(path : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.mkdir({args}))"); CompiledName("mkdir1")>]
            member __.mkdir(path : string, mode : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.mkdir({args}))"); CompiledName("mkdir2")>]
            member __.mkdir(path : string, mode : string, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.mkdirSync({args}))"); CompiledName("mkdirSync")>]
            member __.mkdirSync(path : string, ?mode : float) : unit = failwith "never"
            [<JSEmitInline("({0}.mkdirSync({args}))"); CompiledName("mkdirSync1")>]
            member __.mkdirSyncOverload2(path : string, ?mode : string) : unit = failwith "never"
            [<JSEmitInline("({0}.readdir({args}))"); CompiledName("readdir")>]
            member __.readdir(path : string, ?callback : Func<ErrnoException, array<string>, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.readdirSync({args}))"); CompiledName("readdirSync")>]
            member __.readdirSync(path : string) : array<string> = failwith "never"
            [<JSEmitInline("({0}.close({args}))"); CompiledName("close15")>]
            member __.close(fd : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.closeSync({args}))"); CompiledName("closeSync")>]
            member __.closeSync(fd : float) : unit = failwith "never"
            [<JSEmitInline("({0}.open({args}))"); CompiledName("_open6")>]
            member __._open(path : string, flags : string, ?callback : Func<ErrnoException, float, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.open({args}))"); CompiledName("_open7")>]
            member __._open(path : string, flags : string, mode : float, ?callback : Func<ErrnoException, float, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.open({args}))"); CompiledName("_open8")>]
            member __._open(path : string, flags : string, mode : string, ?callback : Func<ErrnoException, float, obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.openSync({args}))"); CompiledName("openSync")>]
            member __.openSync(path : string, flags : string, ?mode : float) : float = failwith "never"
            [<JSEmitInline("({0}.openSync({args}))"); CompiledName("openSync1")>]
            member __.openSyncOverload2(path : string, flags : string, ?mode : string) : float = failwith "never"
            [<JSEmitInline("({0}.utimes({args}))"); CompiledName("utimes")>]
            member __.utimes(path : string, atime : float, mtime : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.utimes({args}))"); CompiledName("utimes1")>]
            member __.utimes(path : string, atime : Date, mtime : Date, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.utimesSync({args}))"); CompiledName("utimesSync")>]
            member __.utimesSync(path : string, atime : float, mtime : float) : unit = failwith "never"
            [<JSEmitInline("({0}.utimesSync({args}))"); CompiledName("utimesSync1")>]
            member __.utimesSync(path : string, atime : Date, mtime : Date) : unit = failwith "never"
            [<JSEmitInline("({0}.futimes({args}))"); CompiledName("futimes")>]
            member __.futimes(fd : float, atime : float, mtime : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.futimes({args}))"); CompiledName("futimes1")>]
            member __.futimes(fd : float, atime : Date, mtime : Date, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.futimesSync({args}))"); CompiledName("futimesSync")>]
            member __.futimesSync(fd : float, atime : float, mtime : float) : unit = failwith "never"
            [<JSEmitInline("({0}.futimesSync({args}))"); CompiledName("futimesSync1")>]
            member __.futimesSync(fd : float, atime : Date, mtime : Date) : unit = failwith "never"
            [<JSEmitInline("({0}.fsync({args}))"); CompiledName("fsync")>]
            member __.fsync(fd : float, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.fsyncSync({args}))"); CompiledName("fsyncSync")>]
            member __.fsyncSync(fd : float) : unit = failwith "never"
            [<JSEmitInline("({0}.write({args}))"); CompiledName("write35")>]
            member __.write(fd : float, buffer : Buffer, offset : float, length : float, position : float, ?callback : Func<ErrnoException, float, Buffer, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.writeSync({args}))"); CompiledName("writeSync")>]
            member __.writeSync(fd : float, buffer : Buffer, offset : float, length : float, position : float) : float = failwith "never"
            [<JSEmitInline("({0}.read({args}))"); CompiledName("read3")>]
            member __.read(fd : float, buffer : Buffer, offset : float, length : float, position : float, ?callback : Func<ErrnoException, float, Buffer, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.readSync({args}))"); CompiledName("readSync")>]
            member __.readSync(fd : float, buffer : Buffer, offset : float, length : float, position : float) : float = failwith "never"
            [<JSEmitInline("({0}.readFile({args}))"); CompiledName("readFile")>]
            member __.readFile(filename : string, encoding : string, callback : Func<ErrnoException, string, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.readFile({args}))"); CompiledName("readFile1")>]
            member __.readFile(filename : string, options : AnonymousType460, callback : Func<ErrnoException, string, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.readFile({args}))"); CompiledName("readFile2")>]
            member __.readFile(filename : string, options : AnonymousType461, callback : Func<ErrnoException, Buffer, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.readFile({args}))"); CompiledName("readFile3")>]
            member __.readFile(filename : string, callback : Func<ErrnoException, Buffer, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.readFileSync({args}))"); CompiledName("readFileSync")>]
            member __.readFileSync(filename : string, encoding : string) : string = failwith "never"
            [<JSEmitInline("({0}.readFileSync({args}))"); CompiledName("readFileSync1")>]
            member __.readFileSync(filename : string, options : AnonymousType462) : string = failwith "never"
            [<JSEmitInline("({0}.readFileSync({args}))"); CompiledName("readFileSync2")>]
            member __.readFileSync(filename : string, ?options : AnonymousType463) : Buffer = failwith "never"
            [<JSEmitInline("({0}.writeFile({args}))"); CompiledName("writeFile")>]
            member __.writeFile(filename : string, data : obj, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.writeFile({args}))"); CompiledName("writeFile1")>]
            member __.writeFile(filename : string, data : obj, options : AnonymousType464, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.writeFile({args}))"); CompiledName("writeFile2")>]
            member __.writeFile(filename : string, data : obj, options : AnonymousType465, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.writeFileSync({args}))"); CompiledName("writeFileSync")>]
            member __.writeFileSync(filename : string, data : obj, ?options : AnonymousType466) : unit = failwith "never"
            [<JSEmitInline("({0}.writeFileSync({args}))"); CompiledName("writeFileSync1")>]
            member __.writeFileSyncOverload2(filename : string, data : obj, ?options : AnonymousType467) : unit = failwith "never"
            [<JSEmitInline("({0}.appendFile({args}))"); CompiledName("appendFile")>]
            member __.appendFile(filename : string, data : obj, options : AnonymousType468, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.appendFile({args}))"); CompiledName("appendFile1")>]
            member __.appendFile(filename : string, data : obj, options : AnonymousType469, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.appendFile({args}))"); CompiledName("appendFile2")>]
            member __.appendFile(filename : string, data : obj, ?callback : Func<ErrnoException, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.appendFileSync({args}))"); CompiledName("appendFileSync")>]
            member __.appendFileSync(filename : string, data : obj, ?options : AnonymousType470) : unit = failwith "never"
            [<JSEmitInline("({0}.appendFileSync({args}))"); CompiledName("appendFileSync1")>]
            member __.appendFileSyncOverload2(filename : string, data : obj, ?options : AnonymousType471) : unit = failwith "never"
            [<JSEmitInline("({0}.watchFile({args}))"); CompiledName("watchFile")>]
            member __.watchFile(filename : string, listener : Func<Stats, Stats, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.watchFile({args}))"); CompiledName("watchFile1")>]
            member __.watchFile(filename : string, options : AnonymousType472, listener : Func<Stats, Stats, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.unwatchFile({args}))"); CompiledName("unwatchFile")>]
            member __.unwatchFile(filename : string, ?listener : Func<Stats, Stats, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.watch({args}))"); CompiledName("watch")>]
            member __.watch(filename : string, ?listener : Func<string, string, obj>) : FSWatcher = failwith "never"
            [<JSEmitInline("({0}.watch({args}))"); CompiledName("watch1")>]
            member __.watch(filename : string, options : AnonymousType473, ?listener : Func<string, string, obj>) : FSWatcher = failwith "never"
            [<JSEmitInline("({0}.exists({args}))"); CompiledName("exists")>]
            member __.exists(path : string, ?callback : Func<bool, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.existsSync({args}))"); CompiledName("existsSync")>]
            member __.existsSync(path : string) : bool = failwith "never"
            [<JSEmitInline("({0}.createReadStream({args}))"); CompiledName("createReadStream")>]
            member __.createReadStream(path : string, ?options : AnonymousType474) : ReadStream = failwith "never"
            [<JSEmitInline("({0}.createReadStream({args}))"); CompiledName("createReadStream1")>]
            member __.createReadStreamOverload2(path : string, ?options : AnonymousType475) : ReadStream = failwith "never"
            [<JSEmitInline("({0}.createWriteStream({args}))"); CompiledName("createWriteStream")>]
            member __.createWriteStream(path : string, ?options : AnonymousType476) : WriteStream = failwith "never"

    type http with 

            [<JSEmitInline("({0}.STATUS_CODES)"); CompiledName("STATUS_CODES")>]
            member __.STATUS_CODES with get() : AnonymousType443 = failwith "never" and set (v : AnonymousType443) : unit = failwith "never"
            [<JSEmitInline("({0}.createServer({args}))"); CompiledName("createServer")>]
            member __.createServer(?requestListener : Func<ServerRequest, ServerResponse, unit>) : Server = failwith "never"
            [<JSEmitInline("({0}.createClient({args}))"); CompiledName("createClient")>]
            member __.createClient(?port : float, ?host : string) : obj = failwith "never"
            [<JSEmitInline("({0}.request({args}))"); CompiledName("request")>]
            member __.request(options : obj, ?callback : Function) : ClientRequest = failwith "never"
            [<JSEmitInline("({0}.get({args}))"); CompiledName("get13")>]
            member __.get(options : obj, ?callback : Function) : ClientRequest = failwith "never"
            [<JSEmitInline("({0}.globalAgent)"); CompiledName("globalAgent")>]
            member __.globalAgent with get() : Agent = failwith "never" and set (v : Agent) : unit = failwith "never"

    type https with 

            [<JSEmitInline("({0}.Agent)"); CompiledName("Agent")>]
            member __.Agent with get() : AnonymousType446 = failwith "never" and set (v : AnonymousType446) : unit = failwith "never"
            [<JSEmitInline("({0}.createServer({args}))"); CompiledName("createServer1")>]
            member __.createServer(options : ServerOptions, ?requestListener : Function) : Server = failwith "never"
            [<JSEmitInline("({0}.request({args}))"); CompiledName("request1")>]
            member __.request(options : RequestOptions, ?callback : Func<EventEmitter, unit>) : ClientRequest = failwith "never"
            [<JSEmitInline("({0}.get({args}))"); CompiledName("get14")>]
            member __.get(options : RequestOptions, ?callback : Func<EventEmitter, unit>) : ClientRequest = failwith "never"
            [<JSEmitInline("({0}.globalAgent)"); CompiledName("globalAgent1")>]
            member __.globalAgent with get() : Agent = failwith "never" and set (v : Agent) : unit = failwith "never"

    type _internal with 

            [<JSEmitInline("({0}.internal.fail({args}))"); CompiledName("fail")>]
            member __.fail(?actual : obj, ?expected : obj, ?message : string, ?_operator : string) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.ok({args}))"); CompiledName("ok")>]
            member __.ok(value : obj, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.equal({args}))"); CompiledName("equal")>]
            member __.equal(actual : obj, expected : obj, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.notEqual({args}))"); CompiledName("notEqual")>]
            member __.notEqual(actual : obj, expected : obj, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.deepEqual({args}))"); CompiledName("deepEqual")>]
            member __.deepEqual(actual : obj, expected : obj, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.notDeepEqual({args}))"); CompiledName("notDeepEqual")>]
            member __.notDeepEqual(acutal : obj, expected : obj, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.strictEqual({args}))"); CompiledName("strictEqual")>]
            member __.strictEqual(actual : obj, expected : obj, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.notStrictEqual({args}))"); CompiledName("notStrictEqual")>]
            member __.notStrictEqual(actual : obj, expected : obj, ?message : string) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.throws)"); CompiledName("throws")>]
            member __.throws with get() : AnonymousType486 = failwith "never" and set (v : AnonymousType486) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.doesNotThrow)"); CompiledName("doesNotThrow")>]
            member __.doesNotThrow with get() : AnonymousType487 = failwith "never" and set (v : AnonymousType487) : unit = failwith "never"
            [<JSEmitInline("({0}.internal.ifError({args}))"); CompiledName("ifError")>]
            member __.ifError(value : obj) : unit = failwith "never"

    type net with 

            [<JSEmitInline("({0}.Socket)"); CompiledName("Socket")>]
            member __.Socket with get() : AnonymousType452 = failwith "never" and set (v : AnonymousType452) : unit = failwith "never"
            [<JSEmitInline("({0}.createServer({args}))"); CompiledName("createServer2")>]
            member __.createServer(?connectionListener : Func<Socket, unit>) : Server = failwith "never"
            [<JSEmitInline("({0}.createServer({args}))"); CompiledName("createServer3")>]
            member __.createServerOverload2(?options : AnonymousType455, ?connectionListener : Func<Socket, unit>) : Server = failwith "never"
            [<JSEmitInline("({0}.connect({args}))"); CompiledName("connect2")>]
            member __.connect(options : AnonymousType456, ?connectionListener : Function) : Socket = failwith "never"
            [<JSEmitInline("({0}.connect({args}))"); CompiledName("connect3")>]
            member __.connect(port : float, ?host : string, ?connectionListener : Function) : Socket = failwith "never"
            [<JSEmitInline("({0}.connect({args}))"); CompiledName("connect4")>]
            member __.connect(path : string, ?connectionListener : Function) : Socket = failwith "never"
            [<JSEmitInline("({0}.createConnection({args}))"); CompiledName("createConnection")>]
            member __.createConnection(options : AnonymousType457, ?connectionListener : Function) : Socket = failwith "never"
            [<JSEmitInline("({0}.createConnection({args}))"); CompiledName("createConnection1")>]
            member __.createConnection(port : float, ?host : string, ?connectionListener : Function) : Socket = failwith "never"
            [<JSEmitInline("({0}.createConnection({args}))"); CompiledName("createConnection2")>]
            member __.createConnection(path : string, ?connectionListener : Function) : Socket = failwith "never"
            [<JSEmitInline("({0}.isIP({args}))"); CompiledName("isIP")>]
            member __.isIP(input : string) : float = failwith "never"
            [<JSEmitInline("({0}.isIPv4({args}))"); CompiledName("isIPv4")>]
            member __.isIPv4(input : string) : bool = failwith "never"
            [<JSEmitInline("({0}.isIPv6({args}))"); CompiledName("isIPv6")>]
            member __.isIPv6(input : string) : bool = failwith "never"

    type os with 

            [<JSEmitInline("({0}.tmpDir())"); CompiledName("tmpDir")>]
            member __.tmpDir() : string = failwith "never"
            [<JSEmitInline("({0}.hostname())"); CompiledName("hostname6")>]
            member __.hostname() : string = failwith "never"
            [<JSEmitInline("({0}.type())"); CompiledName("_type36")>]
            member __._type() : string = failwith "never"
            [<JSEmitInline("({0}.platform())"); CompiledName("platform2")>]
            member __.platform() : string = failwith "never"
            [<JSEmitInline("({0}.arch())"); CompiledName("arch1")>]
            member __.arch() : string = failwith "never"
            [<JSEmitInline("({0}.release())"); CompiledName("release")>]
            member __.release() : string = failwith "never"
            [<JSEmitInline("({0}.uptime())"); CompiledName("uptime1")>]
            member __.uptime() : float = failwith "never"
            [<JSEmitInline("({0}.loadavg())"); CompiledName("loadavg")>]
            member __.loadavg() : array<float> = failwith "never"
            [<JSEmitInline("({0}.totalmem())"); CompiledName("totalmem")>]
            member __.totalmem() : float = failwith "never"
            [<JSEmitInline("({0}.freemem())"); CompiledName("freemem")>]
            member __.freemem() : float = failwith "never"
            [<JSEmitInline("({0}.cpus())"); CompiledName("cpus")>]
            member __.cpus() : array<AnonymousType444> = failwith "never"
            [<JSEmitInline("({0}.networkInterfaces())"); CompiledName("networkInterfaces")>]
            member __.networkInterfaces() : obj = failwith "never"
            [<JSEmitInline("({0}.EOL)"); CompiledName("EOL")>]
            member __.EOL with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type path with 

            [<JSEmitInline("({0}.normalize({args}))"); CompiledName("normalize1")>]
            member __.normalize(p : string) : string = failwith "never"
            [<JSEmitInline("({0}.join())"); CompiledName("join2")>]
            member __.join() : string = failwith "never"
            [<JSEmitInline("({0}.join({args}))"); CompiledName("join3")>]
            member __.joinOverload2([<System.ParamArray>] paths : array<obj>) : string = failwith "never"
            [<JSEmitInline("({0}.resolve())"); CompiledName("resolve3")>]
            member __.resolve() : string = failwith "never"
            [<JSEmitInline("({0}.resolve({args}))"); CompiledName("resolve5")>]
            member __.resolveOverload2([<System.ParamArray>] pathSegments : array<obj>) : string = failwith "never"
            [<JSEmitInline("({0}.relative({args}))"); CompiledName("relative")>]
            member __.relative(from : string, _to : string) : string = failwith "never"
            [<JSEmitInline("({0}.dirname({args}))"); CompiledName("dirname")>]
            member __.dirname(p : string) : string = failwith "never"
            [<JSEmitInline("({0}.basename({args}))"); CompiledName("basename")>]
            member __.basename(p : string, ?ext : string) : string = failwith "never"
            [<JSEmitInline("({0}.extname({args}))"); CompiledName("extname")>]
            member __.extname(p : string) : string = failwith "never"
            [<JSEmitInline("({0}.sep)"); CompiledName("sep")>]
            member __.sep with get() : string = failwith "never" and set (v : string) : unit = failwith "never"

    type punycode with 

            [<JSEmitInline("({0}.decode({args}))"); CompiledName("decode")>]
            member __.decode(_string : string) : string = failwith "never"
            [<JSEmitInline("({0}.encode({args}))"); CompiledName("encode")>]
            member __.encode(_string : string) : string = failwith "never"
            [<JSEmitInline("({0}.toUnicode({args}))"); CompiledName("toUnicode")>]
            member __.toUnicode(domain : string) : string = failwith "never"
            [<JSEmitInline("({0}.toASCII({args}))"); CompiledName("toASCII")>]
            member __.toASCII(domain : string) : string = failwith "never"
            [<JSEmitInline("({0}.ucs2)"); CompiledName("ucs2")>]
            member __.ucs2 with get() : ucs2 = failwith "never" and set (v : ucs2) : unit = failwith "never"
            [<JSEmitInline("({0}.version)"); CompiledName("version6")>]
            member __.version with get() : obj = failwith "never" and set (v : obj) : unit = failwith "never"

    type querystring with 

            [<JSEmitInline("({0}.stringify({args}))"); CompiledName("stringify5")>]
            member __.stringify(_obj : obj, ?sep : string, ?eq : string) : string = failwith "never"
            [<JSEmitInline("({0}.parse({args}))"); CompiledName("parse2")>]
            member __.parse(str : string, ?sep : string, ?eq : string, ?options : AnonymousType441) : obj = failwith "never"
            [<JSEmitInline("({0}.escape())"); CompiledName("escape")>]
            member __.escape() : obj = failwith "never"
            [<JSEmitInline("({0}.unescape())"); CompiledName("unescape")>]
            member __.unescape() : obj = failwith "never"

    type readline with 

            [<JSEmitInline("({0}.createInterface({args}))"); CompiledName("createInterface")>]
            member __.createInterface(options : ReadLineOptions) : ReadLine = failwith "never"

    type repl with 

            [<JSEmitInline("({0}.start({args}))"); CompiledName("start7")>]
            member __.start(options : ReplOptions) : EventEmitter = failwith "never"

    type string_decoder with 

            [<JSEmitInline("({0}.StringDecoder)"); CompiledName("StringDecoder")>]
            member __.StringDecoder with get() : AnonymousType477 = failwith "never" and set (v : AnonymousType477) : unit = failwith "never"

    type tls with 

            [<JSEmitInline("({0}.CLIENT_RENEG_LIMIT)"); CompiledName("CLIENT_RENEG_LIMIT")>]
            member __.CLIENT_RENEG_LIMIT with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.CLIENT_RENEG_WINDOW)"); CompiledName("CLIENT_RENEG_WINDOW")>]
            member __.CLIENT_RENEG_WINDOW with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.createServer({args}))"); CompiledName("createServer4")>]
            member __.createServer(options : TlsOptions, ?secureConnectionListener : Func<ClearTextStream, unit>) : Server = failwith "never"
            [<JSEmitInline("({0}.connect({args}))"); CompiledName("connect5")>]
            member __.connect(options : TlsOptions, ?secureConnectionListener : Func<unit>) : ClearTextStream = failwith "never"
            [<JSEmitInline("({0}.connect({args}))"); CompiledName("connect6")>]
            member __.connect(port : float, ?host : string, ?options : ConnectionOptions, ?secureConnectListener : Func<unit>) : ClearTextStream = failwith "never"
            [<JSEmitInline("({0}.connect({args}))"); CompiledName("connect7")>]
            member __.connectOverload2(port : float, ?options : ConnectionOptions, ?secureConnectListener : Func<unit>) : ClearTextStream = failwith "never"
            [<JSEmitInline("({0}.createSecurePair({args}))"); CompiledName("createSecurePair")>]
            member __.createSecurePair(?credentials : Credentials, ?isServer : bool, ?requestCert : bool, ?rejectUnauthorized : bool) : SecurePair = failwith "never"

    type tty with 

            [<JSEmitInline("({0}.isatty({args}))"); CompiledName("isatty")>]
            member __.isatty(fd : float) : bool = failwith "never"

    type ucs2 with 

            [<JSEmitInline("({0}.decode({args}))"); CompiledName("decode1")>]
            member __.decode(_string : string) : string = failwith "never"
            [<JSEmitInline("({0}.encode({args}))"); CompiledName("encode1")>]
            member __.encode(codePoints : array<float>) : string = failwith "never"

    type url with 

            [<JSEmitInline("({0}.parse({args}))"); CompiledName("parse3")>]
            member __.parse(urlStr : string, ?parseQueryString : bool, ?slashesDenoteHost : bool) : Url = failwith "never"
            [<JSEmitInline("({0}.format({args}))"); CompiledName("format2")>]
            member __.format(url : UrlOptions) : string = failwith "never"
            [<JSEmitInline("({0}.resolve({args}))"); CompiledName("resolve7")>]
            member __.resolve(from : string, _to : string) : string = failwith "never"

    type util with 

            [<JSEmitInline("({0}.format({args}))"); CompiledName("format3")>]
            member __.format(format : obj) : string = failwith "never"
            [<JSEmitInline("({0}.format({args}))"); CompiledName("format4")>]
            member __.formatOverload2(format : obj, [<System.ParamArray>] param : array<obj>) : string = failwith "never"
            [<JSEmitInline("({0}.debug({args}))"); CompiledName("debug2")>]
            member __.debug(_string : string) : unit = failwith "never"
            [<JSEmitInline("({0}.error())"); CompiledName("error9")>]
            member __.error() : unit = failwith "never"
            [<JSEmitInline("({0}.error({args}))"); CompiledName("error10")>]
            member __.errorOverload2([<System.ParamArray>] param : array<obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.puts())"); CompiledName("puts")>]
            member __.puts() : unit = failwith "never"
            [<JSEmitInline("({0}.puts({args}))"); CompiledName("puts1")>]
            member __.putsOverload2([<System.ParamArray>] param : array<obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.print())"); CompiledName("print2")>]
            member __.printOverload2() : unit = failwith "never"
            [<JSEmitInline("({0}.print({args}))"); CompiledName("print3")>]
            member __.printOverload3([<System.ParamArray>] param : array<obj>) : unit = failwith "never"
            [<JSEmitInline("({0}.log({args}))"); CompiledName("log3")>]
            member __.log(_string : string) : unit = failwith "never"
            [<JSEmitInline("({0}.inspect({args}))"); CompiledName("inspect")>]
            member __.inspect(_object : obj, ?showHidden : bool, ?depth : float, ?color : bool) : string = failwith "never"
            [<JSEmitInline("({0}.inspect({args}))"); CompiledName("inspect1")>]
            member __.inspect(_object : obj, options : InspectOptions) : string = failwith "never"
            [<JSEmitInline("({0}.isArray({args}))"); CompiledName("isArray1")>]
            member __.isArray(_object : obj) : bool = failwith "never"
            [<JSEmitInline("({0}.isRegExp({args}))"); CompiledName("isRegExp")>]
            member __.isRegExp(_object : obj) : bool = failwith "never"
            [<JSEmitInline("({0}.isDate({args}))"); CompiledName("isDate")>]
            member __.isDate(_object : obj) : bool = failwith "never"
            [<JSEmitInline("({0}.isError({args}))"); CompiledName("isError")>]
            member __.isError(_object : obj) : bool = failwith "never"
            [<JSEmitInline("({0}.inherits({args}))"); CompiledName("inherits")>]
            member __.inherits(_constructor : obj, superConstructor : obj) : unit = failwith "never"

    type vm with 

            [<JSEmitInline("({0}.runInThisContext({args}))"); CompiledName("runInThisContext1")>]
            member __.runInThisContext(code : string, ?filename : string) : unit = failwith "never"
            [<JSEmitInline("({0}.runInNewContext({args}))"); CompiledName("runInNewContext1")>]
            member __.runInNewContext(code : string, ?sandbox : Context, ?filename : string) : unit = failwith "never"
            [<JSEmitInline("({0}.runInContext({args}))"); CompiledName("runInContext")>]
            member __.runInContext(code : string, context : Context, ?filename : string) : unit = failwith "never"
            [<JSEmitInline("({0}.createContext({args}))"); CompiledName("createContext")>]
            member __.createContext(?initSandbox : Context) : Context = failwith "never"
            [<JSEmitInline("({0}.createScript({args}))"); CompiledName("createScript")>]
            member __.createScript(code : string, ?filename : string) : Script = failwith "never"

    type zlib with 

            [<JSEmitInline("({0}.createGzip({args}))"); CompiledName("createGzip")>]
            member __.createGzip(?options : ZlibOptions) : Gzip = failwith "never"
            [<JSEmitInline("({0}.createGunzip({args}))"); CompiledName("createGunzip")>]
            member __.createGunzip(?options : ZlibOptions) : Gunzip = failwith "never"
            [<JSEmitInline("({0}.createDeflate({args}))"); CompiledName("createDeflate")>]
            member __.createDeflate(?options : ZlibOptions) : Deflate = failwith "never"
            [<JSEmitInline("({0}.createInflate({args}))"); CompiledName("createInflate")>]
            member __.createInflate(?options : ZlibOptions) : Inflate = failwith "never"
            [<JSEmitInline("({0}.createDeflateRaw({args}))"); CompiledName("createDeflateRaw")>]
            member __.createDeflateRaw(?options : ZlibOptions) : DeflateRaw = failwith "never"
            [<JSEmitInline("({0}.createInflateRaw({args}))"); CompiledName("createInflateRaw")>]
            member __.createInflateRaw(?options : ZlibOptions) : InflateRaw = failwith "never"
            [<JSEmitInline("({0}.createUnzip({args}))"); CompiledName("createUnzip")>]
            member __.createUnzip(?options : ZlibOptions) : Unzip = failwith "never"
            [<JSEmitInline("({0}.deflate({args}))"); CompiledName("deflate")>]
            member __.deflate(buf : Buffer, callback : Func<Error, obj, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.deflateRaw({args}))"); CompiledName("deflateRaw")>]
            member __.deflateRaw(buf : Buffer, callback : Func<Error, obj, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.gzip({args}))"); CompiledName("gzip")>]
            member __.gzip(buf : Buffer, callback : Func<Error, obj, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.gunzip({args}))"); CompiledName("gunzip")>]
            member __.gunzip(buf : Buffer, callback : Func<Error, obj, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.inflate({args}))"); CompiledName("inflate")>]
            member __.inflate(buf : Buffer, callback : Func<Error, obj, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.inflateRaw({args}))"); CompiledName("inflateRaw")>]
            member __.inflateRaw(buf : Buffer, callback : Func<Error, obj, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.unzip({args}))"); CompiledName("unzip")>]
            member __.unzip(buf : Buffer, callback : Func<Error, obj, unit>) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_NO_FLUSH)"); CompiledName("Z_NO_FLUSH")>]
            member __.Z_NO_FLUSH with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_PARTIAL_FLUSH)"); CompiledName("Z_PARTIAL_FLUSH")>]
            member __.Z_PARTIAL_FLUSH with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_SYNC_FLUSH)"); CompiledName("Z_SYNC_FLUSH")>]
            member __.Z_SYNC_FLUSH with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_FULL_FLUSH)"); CompiledName("Z_FULL_FLUSH")>]
            member __.Z_FULL_FLUSH with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_FINISH)"); CompiledName("Z_FINISH")>]
            member __.Z_FINISH with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_BLOCK)"); CompiledName("Z_BLOCK")>]
            member __.Z_BLOCK with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_TREES)"); CompiledName("Z_TREES")>]
            member __.Z_TREES with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_OK)"); CompiledName("Z_OK")>]
            member __.Z_OK with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_STREAM_END)"); CompiledName("Z_STREAM_END")>]
            member __.Z_STREAM_END with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_NEED_DICT)"); CompiledName("Z_NEED_DICT")>]
            member __.Z_NEED_DICT with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_ERRNO)"); CompiledName("Z_ERRNO")>]
            member __.Z_ERRNO with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_STREAM_ERROR)"); CompiledName("Z_STREAM_ERROR")>]
            member __.Z_STREAM_ERROR with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_DATA_ERROR)"); CompiledName("Z_DATA_ERROR")>]
            member __.Z_DATA_ERROR with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_MEM_ERROR)"); CompiledName("Z_MEM_ERROR")>]
            member __.Z_MEM_ERROR with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_BUF_ERROR)"); CompiledName("Z_BUF_ERROR")>]
            member __.Z_BUF_ERROR with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_VERSION_ERROR)"); CompiledName("Z_VERSION_ERROR")>]
            member __.Z_VERSION_ERROR with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_NO_COMPRESSION)"); CompiledName("Z_NO_COMPRESSION")>]
            member __.Z_NO_COMPRESSION with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_BEST_SPEED)"); CompiledName("Z_BEST_SPEED")>]
            member __.Z_BEST_SPEED with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_BEST_COMPRESSION)"); CompiledName("Z_BEST_COMPRESSION")>]
            member __.Z_BEST_COMPRESSION with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_DEFAULT_COMPRESSION)"); CompiledName("Z_DEFAULT_COMPRESSION")>]
            member __.Z_DEFAULT_COMPRESSION with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_FILTERED)"); CompiledName("Z_FILTERED")>]
            member __.Z_FILTERED with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_HUFFMAN_ONLY)"); CompiledName("Z_HUFFMAN_ONLY")>]
            member __.Z_HUFFMAN_ONLY with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_RLE)"); CompiledName("Z_RLE")>]
            member __.Z_RLE with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_FIXED)"); CompiledName("Z_FIXED")>]
            member __.Z_FIXED with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_DEFAULT_STRATEGY)"); CompiledName("Z_DEFAULT_STRATEGY")>]
            member __.Z_DEFAULT_STRATEGY with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_BINARY)"); CompiledName("Z_BINARY")>]
            member __.Z_BINARY with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_TEXT)"); CompiledName("Z_TEXT")>]
            member __.Z_TEXT with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_ASCII)"); CompiledName("Z_ASCII")>]
            member __.Z_ASCII with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_UNKNOWN)"); CompiledName("Z_UNKNOWN")>]
            member __.Z_UNKNOWN with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_DEFLATED)"); CompiledName("Z_DEFLATED")>]
            member __.Z_DEFLATED with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
            [<JSEmitInline("({0}.Z_NULL)"); CompiledName("Z_NULL")>]
            member __.Z_NULL with get() : float = failwith "never" and set (v : float) : unit = failwith "never"
