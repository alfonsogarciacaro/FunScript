module FunScript.Providers.Require.Literals

let typeMappings = dict [ "boolean", typeof<bool>
                          "null", typeof<obj>
                          "undefined", typeof<unit>
                          "number", typeof<float>
                          "string", typeof<string>
                          "array", typeof<obj[]> ]

let [<Literal>] jsCode = """return function (data, callback) {
// Assume it's a constructor if either the obj or prototype constains properties
function isConstructor(obj) {
    return Object.keys(obj.prototype).length ||
            Object.getOwnPropertyNames(obj).length;
}

function getType(obj, depth) {
    depth = depth ? depth : 0;
    var type = obj instanceof Array ? "array" : typeof obj;
    if (type === "object") {
        var props = getProps(obj, depth);
        type = Object.keys(props).length ? props : type;
    }
    else if (type === "function") {
        var fun = obj.toString();
        type = fun.substr(0, / ?{/.exec(fun).index);
        if (isConstructor(obj) && depth < 2) {
            type = { constructor: type };
            type.static = getProps(obj, depth);
            type.instance = getProps(obj.prototype, depth);
        }
    }
    return type;
}

function getProps(obj, depth) {
    var props = {};
    var names = Object.getOwnPropertyNames(obj);
    for (var i = 0; i < names.length; i++) {
        try {
            var name = names[i];
            if (name !== "prototype" && name !== "constructor" && name !== "__proto__")
                props[name] = getType(obj[name], depth + 1);
        } catch (e) { }
    }
    return props;
}

try {
   var required = require(data);
   var info = getType(required);
   callback(null, info); 
} catch (e) {
   callback(e, null);
}
}"""
