//module TestNs.Pepin2

#load "File1.fs"
#load "File2.fs"

exception MyEx of int

try
  raise (MyEx 5)
with
| MyEx i -> i

type Func2 = delegate of arg1: int* arg2: int->int
let f = Func2(fun x y -> x + y)
f.Invoke(3, 4)
let t = TestNs.Test1()

//module Francisco =
//  let e = 4
