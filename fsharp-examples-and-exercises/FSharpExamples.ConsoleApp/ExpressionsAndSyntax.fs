module ExpressionsAndSyntax

// https://fsharpforfunandprofit.com/series/expressions-and-syntax.html

// NOTE: this series is actually a bit annoying - it seems like a completely beginner-level series
// which aims to introduce the reader to the very basics, but then starts talking about .NET objects
// and async functions...

// #################################
// ### EXPRESSIONS VS STATEMENTS ###
// #################################

(*
    In programming terminology, an EXPRESSION is a combination of values and functions that are 
    interpreted by the compiler to creare a new value. A STATEMENT is just a standalone unit of
    execution and doesn't return anything.

    In other words,, the purpose of an expression is to create a value (with possible side-
    effects), whereas the sole purpose of a statement is to have side-effects.

    In F#, everything is an expression. This gives us an advantage: smaller expressions can be 
    combined (or "composed") into larger expressions. If everything is an expression, then everything
    is composable.

    Everything in expressionsVsStatements is an expression; that is, each of these returns a value
    that can be used for something else. We can see this by binding a value to the result of
    each expression. 
*)

let expressionsVsStatements () =
    let value1 = 
        fun () -> 1

    let value2 = 
        match 1 with
            | 1 -> "a"
            | _ -> "b"

    let value3 = 
        if true then "a" else "b"    

    let value4 = 
        for i in [1..10]           
            do printf "%i" i

    let value5 = 
        try                         
            let result = 1 / 0
            printfn "%i" result
        with
            | e -> 
                printfn "%s" e.Message            
    0

// #################################
// ### BINDING WITH LET, USE, DO ###
// #################################

(*
    There are no "variables" in F#. Instead there are values. Keywords such as "let", "use", 
    and "do" act as BINDINGS - associating an identifier with a value or a function expression.
     
    The "let" keyword has two slightly different usages: firstly, for binding an expression at 
    the top-level of a module (topLevelName), and secondly for defining a local name used in the
    context of some expression (nestedName1, nestedName2). The top level name is a DEFINITION,
    and can be accessed elsewhere with e.g. ExpressionsAndSyntax.topLevelName.
    
*)

let topLevelName () =
    let nestedName1 = 5
    let nestedName2 = 12
    nestedName1 * nestedName2
