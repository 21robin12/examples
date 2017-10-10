module Basics

// http://www.tutorialspoint.com/fsharp/

// ####################
// ### MISC. BASICS ###
// ####################

(*
    declaration. infer or specify type.

    variables in F# are immutable
    we can create a mutable variable if we need to
    recommendation is not to ever use these as a beginner (like dynamic in C#)
*)

let misc () =
    let number = 10
    let otherNumber:int32 = 5

    let mutable total = 999
    total <- number + otherNumber
    0

// #################
// ### FUNCTIONS ###
// #################

(*
    methods are defined as: let function-name parameter-list : return-type
    
    however return type is optional; if not provided then it will be inferred from return statements
    
    same goes for paramater types

    recursion: need to specify "rec"

    fTotal is a composite function; it applies f1 followed by f2

    pipelining - like function composition but without declaring the composite function i.e. execute the pipeline now, once
    note that with pipelining we must provide parameters to the first function in the pipeline, but with composition we omit them

    currying - all mathematical functions take one input and return one result. to allow for multi-parameter functions, F# uses "currying" (named after Haskell Curry)
    we define a multi-parameter function...
    ... and F# rewrites it internally to something like:
    let multiFunction x =
        let subFunction y =
            x * y
        subFunction
    so, running multiFunction with x, y firstly returns subFunction with a pre-set value for x, and then applies y to this function
    we can see this in action by running multiFunction with just one parameter - it returns this inner function (x * y) with a pre-set value of x = 5
    we can then run this inner function against a number of y values:
*)

let sign (num : int) : string =
   if num > 0 then "positive"
   elif num < 0 then "negative"
   else "zero"


let signInferred num =
   if num > 0 then "positive"
   elif num < 0 then "negative"
   else "zero"

let rec fib n = if n < 2 then 1 else fib (n - 1) + fib (n - 2)

let composition () =
    let f1 x = x + 1
    let f2 x = x * 5
    let fTotal = f1 >> f2
    let res3 = fTotal 10
    printfn "%i" res3
    0

let pipelining () =
    let f1 x = x + 1
    let f2 x = x * 5
    let res4 = 10 |> f1 |> f2
    printfn "%i" res4
    0

let currying () =
    let multiFunction x y =
        x * y

    let innerCurry = multiFunction 5
    let result50 = innerCurry 10
    let result15 = innerCurry 3
    0

let lambdas () =
    let applyFunction (f: int -> int -> int) x y = f x y
    let multiply x y = x * y
    let res = applyFunction multiply 5 7
    printfn "%i" res 

    let res2 = applyFunction (fun x y -> x * y) 5 7
    printfn "%i" res2 
    0

let generics () =
    let listify x = [x] // functions are generic until some constraint prevents them from being so
    let listify2 (x:'a) = [x] // if we wish to specify generic types, their names must start with '
    0

// ########################
// ### PATTERN MATCHING ###
// ########################

(*
    TODO: in-depth bit about pattern matching

    For a function that only contains a match statement, we can use slightly different syntax by using the "function" 
    keyword. This only really offers an advantage when using a match statement inside a lambda, by providing slightly
    more terse syntax.
*)

let expressionOrFunction () =
    let patternMatchingExpression x =
        match x with
            | 1 -> "one"
            | _ -> "not one"

    let patternMatchingFunction x = function
        | 1 -> "one"
        | _ -> "not one"

    let resultExpression = List.map (fun x -> match x with | 1 -> "one" | _ -> "not one") [0;1;2]
    let resultFunction = List.map (function 1 -> "one" | _ -> "not one") [0;1;2]
    0

// ####################
// ### CONTROL FLOW ###
// ####################

(*
    avoid all of this while learning
*)

let controlFlow () =
    let total = 5
    if(total > 900) then
        printfn "Larger than 900"
    else 
        printfn "Not larger than 900"

    for i = 10 to 20 do
        printfn "%i" i
    0