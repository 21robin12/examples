module FunctionalProgramming

// Chapter 3, Functional Programming, O'Reilly C# 3.0 book, pages 51 - 95

(*
    Key features of a functional language:

    - immutable data
    - ability to compose functions
    - functions can be treated as data
    - lazy evaluation
    - pattern matching
*)

// ####################
// ### Immutability ###
// ####################

(*
    In pure F# we don't have mutable "variables", we have immutable "values".

    The functional style is "better" because we can easily read WHAT a bit of code does by
    quickly glancing at it: take a list of numbers, map them onto their squares, and then 
    sum these squares. The imperative style goes into detail of HOW to acheive the desired
    result, so we have to read and understand all the code.
*)

let square x = x * x

let imperativeSum numbers =
    let mutable total = 0
    for i in numbers do
        let x = square i
        total <- total + x
    total

let functionalSum numbers = numbers |> Seq.map square |> Seq.sum 

// #######################
// ### Function values ###
// #######################

(*
    It's easy to pass functions around - see passFunction. Most languages treat functions
    and values as very different things; even C# makes this sort of thing quite tricky by
    requiring a load of messy Func<,,> definitions.

    Partial Application also simplifies things. We can either call a function with all of
    its parameters (printfn "%d" 3) and get its return value, or we can pass an incomplete
    set of parameters (printfn "%d") and get another function. In this case, printfn takes a
    string and a number and returns unit, so has a signature of string -> int -> unit. By 
    partially applying just the first parameter (a string), we get a method with signature
    int -> unit. This shows why F# function signatures are written in this way - by applying
    parameters we just shift right along the chain.

    Functions can also return other functions. "generatePowerOfFunc" returns a function with
    signature float -> float -> float, which we then partially apply to get the function
    powerOfTwo (float -> float), which we then finally call to get a result. Note that when
    we define powerOfTwo by partially applying generatePowerOfFunc, baseValue is fixed as 2.0.
    But where was that 2.0 stored, and how is it in scope when we later call powerOfTwo 8.0?
    This bit of magic is known as a "closure" - more details in later chapters.

    Functions that take other functions as parameters, or return other functions, are known
    as higher-order functions.
*)

let passFunction () =
    let negate x = -x
    let result = List.map negate [1..10]
    0

let partialApplication () =
    printfn "%d" 3
    List.iter (printfn "%d") [1..3]
    0

let functionsReturningFunctions () =
    let generatePowerOfFunc baseValue =
        (fun exponent -> baseValue ** exponent)

    let powerOfTwo = generatePowerOfFunc 2.0
    let result = powerOfTwo 8.0
    0

// ###########################
// ### Recursive functions ###
// ###########################

(*
    Recursive functions require the "rec" keyword.

    By using recursion and higher-order functions, we can easily simulate control flow expressions
    like "for" and "while". In fact, in most functional programming languages, recursion is the ONLY
    way to set up looping constructs: if we can't mutate variables, we can't increment a loop counter.
    Pure functional F# would only use recursion rather than for loops; however it's best to use
    whatever makes the code clear and simple.

    "Mutually-recursive" functions are functions that call each other recursively. This is a bit of an
    issue in F#; in order to call a function, it must already be defined. In order to define mutually 
    recursive functions, we must join them together with the "and" keyword.
*)

let rec factorial x =
    if x <= 1 then
        1
    else
        x * factorial (x - 1)

let recursiveControlFlow () =
    let rec forLoop body times = 
        if times <= 0 then
            ()
        else    
            body()
            forLoop body (times - 1)

    forLoop (fun () -> printfn "Looping") 3
    0

let mutualRecursion () =
    let rec isOdd x =
        if x = 0 then false
        elif x = 1 then true
        else isEven(x - 1)
    and isEven x =
        if x = 0 then true
        elif x = 1 then false
        else isOdd(x - 1)

    let result314 = isOdd 314
    let result1337 = isEven 1337  
    0