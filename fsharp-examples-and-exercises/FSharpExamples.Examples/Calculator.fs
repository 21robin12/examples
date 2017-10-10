module Calculator

(* 
    http://fsharpforfunandprofit.com/posts/stack-based-calculator/
*)

// ==============================================
// Types
// ==============================================

// StackContents is a "single case union type" - https://fsharpforfunandprofit.com/posts/designing-with-types-single-case-dus/
// Stack is the type name
// StackContents is the case name
// These two names can be the same: "One is a type, and one is a constructor function with the same name."
type Stack = StackContents of float list

// ==============================================
// Stack primitives
// ==============================================

let push x aStack =   
    let (StackContents contents) = aStack
    let newContents = x::contents // :: operator prepends x at the start of contents. note that contents is immutable so we must return a new StackContents
    StackContents newContents 

let pop (StackContents contents) =
    match contents with
    | top::rest ->
        let newStack = StackContents rest
        (top, newStack) // return a tuple
    | [] ->
        failwith "Stack underflow" // throw an exception

let binary f stack =
    let first, firstPopped = pop stack
    let second, secondPopped = pop firstPopped
    let result = f first second
    push result secondPopped

// define function
let ADD stack =
    let fn a b = a + b
    binary fn stack

// use lambda
let MUL stack =
    binary (fun a b -> a * b) stack

// substitute for symbol for this built-in function: -
let SUB stack =
    binary (-) stack

// use partial application to simplify
let DIV = binary (/)

let unary f stack =
    let first, stack' = pop stack
    push (f first) stack' // can substitute to reduce lines

// some more functions
let NEG = unary (fun x -> -x)
let SQUARE = unary (fun x -> x * x)

// print the answer
let SHOW stack = 
    let x,_ = pop stack
    printfn "The answer is %f" x

// duplicate the top item
let DUP stack =
    let x, stack' = pop stack
    push x stack

// swap top 2 items
let SWAP stack =
    let x1, stack' = pop stack
    let x2, stack'' = pop stack'
    push x2 (push x1 stack)

// operation to return an empty stack:
let EMPTY = 
    StackContents []

// composition. >> is composition operator
// different to piping in that we're just defining something to run later, whereas piping is a "realtime transformation" and runs whenever it's defined
let CUBE =
    DUP >> DUP >> MUL >> MUL
    

// ==============================================
// Execution
// ==============================================

let run =
    // test usage:
    let emptyStack = StackContents []
    let stackWith1 = push 1.0 emptyStack
    let withWith1And2 = push 2.0 stackWith1

    // define an operation to push 1 into a stack...
    let ONE stack = push 1.0 stack

    // ...but we can simplify this using "partial application", since stack is on both sides:
    let ONE = push 1.0
    let TWO = push 2.0
    let THREE = push 3.0
    let FOUR = push 4.0
    let FIVE = push 5.0
    
    // using these operations:
    let STACK1 = ONE EMPTY;
    let STACK21 = TWO STACK1
    let STACK521  = FIVE STACK21
    
    // but we can actually chain these together:
    let STACK4521 = EMPTY |> ONE |> TWO |> FIVE |> FOUR

    // executing pop and getting values:
    let four, poppedStack = pop STACK4521 
    
    // chain all the functions together - this is called "piping"
    let calc = EMPTY |> TWO |> FIVE |> SWAP |> ADD |> THREE |> ADD |> FOUR |> MUL |> DUP |> SHOW


    0
