type ComplexNumber = { real: float; imaginary: float } // records are like named tuples

// unions
type MixedType = 
  | Boo of bool
  | Flo of float

[<EntryPoint>]
let main argv = 
    let result1 = NinetyNineProblems.run
    let result2 = Lists.aggregateOperators()
    let result3 = ProjectEuler.run513()
    let result4 = ProjectEuler.run125()

    let rec fib n = 
        match n with
            | 0 -> 0
            | 1 -> 1
            | _ -> (fib (n - 2)) + (fib (n - 1)) 

    let calculateSequence fn x = fn x

    let fibs = List.map (calculateSequence fib) [0..5] // we partially-apply calculateSequence with fib
    let total = List.reduce (fun acc item -> acc + item) fibs

    // ###########################################
    // PARIAL APPLICATION / HIGHER-ORDER FUNCTIONS
    // ###########################################
    let printFloat = printfn "%f" // partial application of printfn
    printFloat 3.0

    // ###########################################
    // ############## F# TYPES ###################
    // ###########################################
    let tuple1: int * bool = 4, false // explicitly-typed tuple. tuples created by multiplication - domain of all integers multiplied by domain of all bools gives domain of all possible pairs
    let tuple2 = 1, true // tuple created by type inference
    let i, b = tuple2 // deconstructing a tuple

    let exponentFn value = (fun exponent -> value ** exponent)
    let powerOfTwo = exponentFn 2.0
    printFloat (powerOfTwo 8.0)

    let exponentFn2 value exponent = value ** exponent
    let powerOfTwo2 = exponentFn2 2.0
    printFloat (powerOfTwo2 8.0)

    let complex = { real = 1.0; imaginary = 3.2 } // F# can infer the type of our record from the named elements

    // unions are formed by summing types since they can hold a value of any of of the types specified
    let u1 = Boo true
    let u2 = Flo 3.1
    let mutable u1 = Boo false
    u1 <- Flo 3.2

    // options are like predefined options consisting of a "some" and a "none" option. here are some int options - they're like nulls in C#.
    let o1 = Some 41
    let o2 = Some 28
    let o3 = None

    // ###########################################
    // ############## F# LISTS ###################
    // ###########################################
    let matchIntOption o = 
        match o with
            | Some i -> printfn "integer has value of %i" i
            | None -> printfn "None int option - no value"

    let optionList = [o2; o3] // list initializer
    let fullOptionList = o1::optionList // create new list with o1 at head
    List.iter matchIntOption optionList

    // folding - reduces a list down to a single value. value and list elements can have different types & we can specify the accumulator's default value
    let fList = [192.1;68.0;0.1]
    let concat list = List.fold (fun s t -> s + t.ToString()) "" list
    let fListString = concat fList
    
    // reducing - reduces a list down to a single value. value and list elements have the same type & accumular is type default value
    let sum list = List.reduce (fun acc x -> acc + x) list
    let fListSum = sum fList

    // ###########################################
    // ############## CURRYING ###################
    // ###########################################
    let functionWithTwoInputs x y = x + y
    let sum = functionWithTwoInputs 1 5 // 6

    let innerCurry y = functionWithTwoInputs 1 y
    let currySum = innerCurry 5

    0