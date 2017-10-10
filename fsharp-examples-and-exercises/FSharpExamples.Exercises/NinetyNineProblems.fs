// http://www.ic.unicamp.br/~meidanis/courses/mc336/2006s2/funcional/L-99_Ninety-Nine_Lisp_Problems.html
// Solutions: https://github.com/paks/99-FSharp-Problems

module NinetyNineProblems

// 1
let rec getLastItem xs =
    match xs with
        | [] -> failwith "Empty list!"
        | [x] -> x
        | head::tail -> getLastItem tail

// 2
let rec getLastButOneItem xs =
    match xs with
        | [] -> failwith "Empty list!"
        | [x] -> failwith "Only one item!"
        | [x;_] -> x
        | head::tail -> getLastButOneItem tail

// 3
let getItemAt xs i =
    xs |> List.item i

// 4
let listCount xs =
    xs |> List.length

// 5
let reverse xs =
    xs |> List.rev

// 6
let isPalindrome xs =
    let reversed = reverse xs
    xs = reversed 

// wrote this for manual list comparison - leaving it here for reference
let rec sameLists xs1 xs2 =
    match xs1 with
        | [] -> 
            match xs2 with
                | [] -> true
                | _ -> false
        | [x] -> 
            match xs2 with
                | [] -> false
                | [y] -> x = y
                | _ -> false
        | head::tail -> 
            match xs2 with
                | [] -> false
                | [y] -> false
                | reversedHead::reversedTail -> 
                    if reversedHead = head then
                        sameLists tail reversedTail
                    else
                        false

// 7
type 'a NestedList = 
    | List of 'a NestedList list 
    | Elem of 'a

let flatten list = 
    let rec inner x acc =
        match x with
            | Elem xx -> xx::acc
            | List xs -> List.foldBack(fun x' acc' -> inner x' acc') xs acc
    inner list []

// 8
let compress list =
    List.foldBack(fun x acc -> if List.length acc = 0 || List.head acc <> x then x::acc else acc) list []

// 9
let collect x acc =
    match acc with
        | (y::xs)::xss when x = y -> (x::y::xs)::xss
        | xss -> [x]::xss

let pack list =
    List.foldBack (fun x acc -> collect x acc) list []

// 10
let encodePacked list =
    let encode x acc =
        let length = List.length x
        match x with
            | [] -> acc
            | item::_ -> 
                let encoded = length.ToString() + " " + item.ToString()
                encoded::acc

    let packed = pack list
    List.foldBack(fun x acc -> encode x acc) packed []

// 11
type 'a Encoded =
    | Many of int * 'a
    | One of 'a

let encodeDirect list =
    let packed = pack list

    let encode x acc =
        match x with
            | [] -> acc
            | [item] -> (One item)::acc
            | item::xs ->  (Many (List.length xs + 1, item))::acc

    List.foldBack(fun x acc -> encode x acc) packed []

// 12
let decode encoded =
    let each x acc =
        match x with
            | One a -> a::acc
            | Many (i,a) -> List.append (List.replicate i a) acc

    List.foldBack(fun x acc -> each x acc) encoded []

// 13
// TODO

// 14

let duplicate list =
    List.foldBack(fun x acc -> x::x::acc) list []

// 15

let duplicateMultiple list i =
    List.foldBack(fun x acc -> List.append(List.replicate i x) acc) list []

// 16

let dropEvery list n =
    let numbered = List.mapi(fun i x -> (i + 1, x)) list
    List.foldBack(fun x acc -> match x with | (i', x) when i' % n = 0 -> acc | (_, x) -> x::acc) numbered []

// 17-30
// TODO

// 31

let isPrime n = 
    let possibleFactors = List.init (n - 2) (fun i -> i + 2)
    let rec inner factors =
        match factors with
            | [] -> true
            | x::xs when n % x = 0 -> false
            | x::xs ->  inner xs 
    inner possibleFactors

// 32

let rec greatestCommonDivisor x1 x2 =
    let remainder = x1 % x2
    match remainder with
        | 0 -> x2
        | r -> greatestCommonDivisor x2 r

// execute to test
let run =
    let ints = [1; 2; 3; 4; 5; 6]
    let palindrome = [1; 2; 19; 2; 1]

    let last = getLastItem ints 
    let lastButOne = getLastButOneItem ints
    let fourthItem = getItemAt ints 3
    let length = listCount ints
    let reversed = reverse ints
    let palin1 = isPalindrome ints
    let palin2 = isPalindrome palindrome

    let nested = List [Elem 4; Elem 17; Elem 2; List [Elem 12; Elem 1]; Elem 6; List [Elem 999; Elem 999; Elem 999]]
    let flat = flatten nested

    let duplicates = [1; 1; 1; 1; 2; 2; 3; 4; 4; 4; 1; 1; 1; 2; 1; 2; 5; 5; 5; 5]
    let duplicateCharacters = ['A'; 'A'; 'A'; 'A'; 'B'; 'B'; 'C'; 'D'; 'D'; 'D'; 'E'; 'E'; 'F'; 'G'; 'H'; 'H']
    let compressed = compress duplicates
    let packed = pack duplicates
    let encodedPacked = encodePacked duplicates
    let encodeDirectPacked = encodeDirect duplicates
    let encodeCharactersDirectPacked = encodeDirect duplicateCharacters
    let decoded = decode encodeCharactersDirectPacked
    let duplicated = duplicate ints
    let duplicatedThrice = duplicateMultiple ints 3
    let dropped = dropEvery ints 3
    let prime13 = isPrime 13
    let prime8 = isPrime 8
    let prime941 = isPrime 941

    let common12 = greatestCommonDivisor 72 60
    let common1 =  greatestCommonDivisor 72 941
    0
    