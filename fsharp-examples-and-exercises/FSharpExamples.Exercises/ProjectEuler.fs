module ProjectEuler

// #################################################################
// https://projecteuler.net/problem=513

let countEqual acc (c, b, a) = match a with | a when (a = b) && (b = c) -> acc + 1 | _ -> acc
let overAllAs acc (c, b) = List.fold (fun acc a -> countEqual acc (c, b, a)) acc [1..b]
let overAllBs acc c = List.fold (fun acc b -> overAllAs acc (c, b)) acc [1..c]
let overAllCs n:int = List.fold overAllBs 0 [1..n]

let run513 () = 
    // example of iterating over required domain - doesn't perform calcs to answer problem though
    let n = 19
    
    let result = overAllCs n

    let resultInline = 
        List.fold (fun acc c ->  
            List.fold(fun acc b -> 
                List.fold (fun acc a -> 
                    match a with | a when (a = b) && (b = c) -> acc + 1 | _ -> acc
                ) acc [1..b]
            ) acc [1..c]
        ) 0 [1..n]

    // return 0
    0

// #################################################################
// https://projecteuler.net/problem=125

let isMatch acc x =
    let isPalindrome (i:int) =
        let s = List.ofSeq (i.ToString())
        let reversed = List.rev s
        s = reversed 
        
    let isSumOfSquares (i:int) = true // implement this

    match x with
        | x when isPalindrome x && isSumOfSquares x -> x::acc
        | _ -> acc

let run125 () =
    let n = 1000
    // probably a bad problem to practice functional programming with - list is too long for very large n
    let matches = List.fold isMatch [] [1..n] 
    0