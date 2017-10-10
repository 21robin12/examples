module Lists

// from O'Reilly book, pages 34 - 42

let listOperations () = 
    let empty = []; // generic
    let intsList = 13::empty // one of only two primitive list operations: cons, with the cons operator ::. adds to head of list
    let moreIntsList = 14::19::intsList // can use multiple cons together
    let joinedLists = intsList @ moreIntsList // other primitive operation: append, uses @ operator. joins two lists together
    0

let listRanges () =
    let intsList = [1..3] // list range syntax: starting at the first item, and increment in 1 until the last item. here: [1;2;3]
    let floatsList = [10.456..29.456] // not limited to integers
    let odds = [1..2..17] // not limited to step of 1 - can specify a stepping value
    let floats = [1.3..0.1..2.9] // and the stepping value can be a float
    let negative = [10..-1..0] // or even negative
    0

let listComprehension () = 
    let multiples x y = // we can use a "list comprehension" to build a list. 
        [ // list comprehensions are surrounded by square brackets
            for i in 1..y do
                yield x * i // use the yield keyword to return an item
        ]

    let multiplesOf5 = multiples 5 4 // [5;10;15;20]

    let multiplesSuccinct x y = [for i in 1..y -> x * i] // we can replace "yield" with ->
    let multiplesOf6 = multiples 6 4 // [6;12;18;24]
    0

let moduleFunctions () =
    let list = [1;2;3;4;5;6] // the List module contains a load of methods to help process lists:
    let charsList = ['A';'B';'C';'D';'E';'F']

    let length = List.length list // 6
    let head = List.head list // 1
    let tail = List.tail list // [2;3;4;5;6]
    let exists = List.exists (fun a -> a = 3) list // true
    let rev = List.rev list // [6;5;4;3;2;1]
    let tryFind = List.tryFind (fun a -> a = 3) list // Option, Some(3) 
    let zip = List.zip list charsList // List of tuples, [{1,A},{2,B},{3,C},{4,D},{5,E},{6,F}]
    let filter = List.filter (fun a -> a % 2 = 0) list // [2;4;6]
    let partition = List.partition (fun a -> a % 2 = 0) list // Tuple, Item1: [2;4;6] Item2: [1;3;5]
    0

let aggregateOperators () =
    let square x = x * x // this function will be applied to each element in the list
    let squares = List.map square [1..10] // builds a new list by applying "square" to each element

    let insertCommas acc item = acc + ", " + item // List.reduce uses an accumulator to build up a final result. acc and item must have same type
    let joined = List.reduce insertCommas ["Pen"; "Apple"; "Pineapple"] // "Pen, Apple, Pineapple" - List.reduce started with the second element (no leading comma)

    let countVowels acc item = // List.fold similar to List.reduce, but acc and item may have differrent types
        match item with // often use match expressions with folds
            | 'a' | 'e' | 'i' | 'o' | 'u' -> acc + 1
            | _ -> acc
    let vowelsCount = List.fold countVowels 0 (List.ofSeq "the quick brown fox jumps over the lazy dog") // 11. notice that we must pass an initial value

    let printNumber x = printfn "printing %d" x // List.iter returns unit, and is mainly used for evaluating side-effects (like printing)
    List.iter printNumber [1..3] // outputs "printing 1" / "printing 2" / "printing 3"
    0