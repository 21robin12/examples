module Types 

// https://fsharpforfunandprofit.com/posts/types-intro/

// ##########################
// ### TYPE ABBREVIATIONS ###
// ##########################

(*
    We can add an alias for an existing type. These can be useful to provide documentation, 
    and to avoid writing a length signature repeatedly.

    However, aliases do not provide any additional type safety: in calculateForce below, the 
    first parameter is mass and the second acceleration - although the compiler doesn't mind if
    we pass these in the wrong way around, since they're essentially both just floats. To get 
    true encapsulated types like this, use a single case union type (see below).
*)

type RealNumber = float
type Complex = float * float
type ComplexAdditionFunction = Complex -> Complex -> Complex
type Mass = float
type Acceleration = float

let calculateForce (m: Mass, a: Acceleration) =
    let force = m * a
    force

let runCalculation () = 
    let mass: Mass = 74.4
    let acceleration: Acceleration = 9.81
    let force = calculateForce (acceleration, mass)
    force

// ##############
// ### TUPLES ###
// ##############

(*
    Tuples are formed by "multiplication", using * notation. If we have a domain X consisting of every integer, 
    and another domain Y consisting of every string, the domain consisting of every possible combination of every integer AND every
    string is formed by the cartesian product of X * Y. When we create a tuple of e.g. int * string we are doing exactly this:
    creating a new type which is the domain X * Y.

    The types of a tuple can be declared explicitly (tuple1), or F#'s type-inference algorithm can usually work it out (tuple2). 
    Tuples are not restricted to two items (tuple3). Deconstructing a tuple is acheived with the usual syntax.
*)

let tuples () = 
    let tuple1: bool * int = (true, 3)
    let tuple2 = (true, 3)
    let tuple3 = (1, bool, "hello")
    let number, booly, word = tuple3
    0

// ###############
// ### RECORDS ###
// ###############

(*
    Like Tuples, Records are also formed by "multiplication". A major drawback of using typles is that the elements are unlabelled. 
    If for example we had a two tuples, each of float * float, one used for coordinates and the other for a complex number, we have 
    no way to distinguish between these. A record is a tuple where each element is labelled. 

    F# is very good at inferring the type of a record from the types and labels used when creating it (complex). If there is any 
    ambiguity, the most recently-defined record will be used (coordOther) - if we wish to use a different record, we must specify it
    by name. Deconstructing a record is acheived with the usual syntax.
*)

type ComplexNumber = { real: float; imaginary: float }
type GeoCoord = { lat: float; long: float }
type GeoCoordOther = { lat: float; long: float }

let records () =
    let complex = { real = 3.1; imaginary = 2.0 }
    let coordOther = { lat = 1.2; long = 1.3 }
    let coord : GeoCoord = { lat = 1.2; long = 1.3 }
    let { real = myReal; imaginary = myImaginary } = complex
    0

// ############################
// ### DISCRIMINATED UNIONS ###
// ############################

(*
    Unions are formed by "summing" two types together. If we have a domain X consisting of every boolean,
    and another domain Y consisting of every float, then the domain consisting of every boolean AND every float is formed
    by the sum X + Y. When we create an instance of a union type, we are creating something which will sit somewhere in
    the  domain X + Y; it will have either a boolean or a float value.

    Unions can be declared on a single line, or across multiple lines. Each possible value is called a "case". The first | symbol is 
    optional. Type ambiguity is handled by specifying type. In the Shape union, the Rectangle case is a Tuple of float * float.
    
    We can match on the possible values of a union (unionsMatching), and also easily compare two unions (unionsCompare).

    Unions can also be declared with "empty cases" (Directory). If every case is an empty case (Answer), then the union is an
    "enum style" union. Note that this is NOT the same as an enum type.
    
    It can also be useful to create a union type with only one case. This can be used to enforce type safety. The function calculateSpeed
    takes two parameters distance and time. Each of these is a single-case union type, which prevents the wrong parameter from being passed
    in. We can do something similar in C#, but there's more overhead in doing so.
*)

type AmbiguousMixedType = 
  | Boo of bool
  | Flo of float

type MixedType = 
  | Boo of bool
  | Flo of float

let unionsAmbiguity () =
    let b = Boo true
    let f = Flo 99.56
    
    let b' = AmbiguousMixedType.Boo true
    let f' = AmbiguousMixedType.Flo 99.56
    0

type Shape = Square of float | Rectangle of float * float

let unionsMatching () =
    let calculateArea shape =
        match shape with
            | Square s ->
                3.14 * s * s
            | Rectangle (l1, l2) ->
                l1 * l2

    let square = Square 10.2
    let rect = Rectangle (9.3, 34.56)
    let squareArea = calculateArea square
    let rectArea = calculateArea rect
    0

let unionsComapre () =
    let b = Boo true
    let f = Flo 99.56
    let equals = b = f
    equals

type Directory =
    | Root
    | Subdirectory of string

type Answer =
    | Yes
    | No

let emptyCases () =
    let directory1 = Root
    let directory2 = Subdirectory "C:/users/me/documents/"
    let answer = Yes
    0

type Distance = Distance of float
type Time = Time of float

let calculateSpeed (Distance distance, Time time) =
    let speed = distance / time
    speed

// ####################
// ### OPTION TYPES ###
// ####################

(* 
    An Option is like a Union consisting of a generic "Some of" case and an empty "None" case. They're so useful
    that they've been build directly into the F# language. Options are used for values that might be missing or
    invalid, sort of like a null in C#, but with a key advantage. In C#, a null has the same type as the object it's
    assigned to, so we don't know until runtime whether it will have the value null. This results in a lot of null
    checks to avoid NullReferenceExceptions. An F# None does NOT have the same type as a Some, and so we can 
    distinguish between the two at compile-time.

    When defining an option type, we must specify which generic type to use. We can do this C#-style, or by using the 
    option keyword postfix - both ar eidentical. An option can use any type, not just a primitive.

    Options can tell us about the success of an operation. For example, List.tryFind returns an option with None
    if the value was not found. In tryParseInt, we return None if the integer could not be parsed.

    It's better to use matching rather than if-else with options (optionsBadly, optionsProperly).

    Like List, there's an Option module which has some useful functions (optionsModule).
*)

let optionTypes () =
    let validInt = Some 67
    let invalidInt = None 
    
    match validInt with
        | Some x -> printfn "the valid value is %A" x
        | None -> printfn "value is invalid"
    0

type SearchResult1 = Option<string>
type SearchResult2 = string option

let usingOptions () =
    let some2 = [1;2;3] |> List.tryFind (fun x -> x = 2)
    let none = [1;2;3] |> List.tryFind (fun x -> x = 999)
    0

let tryParseInt intStr =
    try
        let i = System.Int32.Parse intStr
        Some i
    with _ -> None

let optionsCompare () =
    let o1 = Some 41
    let o2 = Some 41
    let equals = o1 = o2
    equals

let optionsBadly () =
    let x = Some 99
    if x.IsSome then printfn "x is %i" x.Value

let optionsProperly () = 
    let x = Some 99
    match x with
        | Some i -> printfn "x is Some %i" i
        | None -> printfn "x is None"

let optionsModule () =
    let x = Some 99
    let doubled = x |> Option.map (fun v -> v * 2)
    0

// ##################
// ### ENUM TYPES ###
// ##################

(*
    Enums are defined in a very similar way to Unions, except we must specify a value for each option. Values can 
    be a of few different types such as integers or characters, but not e.g. strings. 

    We can easily parse an enum back to it's underlying value, and visa versa.

    We can also assign an undefined value to an enum. This means that when matching on an enum, we must include a 
    wildcard match to define a complete pattern match.
*)

type ColorUnion = Red | Blue | Green
type ColorEnum = Red = 0 | Blue = 1 | Green = 2
type ColorEnumChars = Red = 'R' | Blue = 'B' | Green = 'G'

let enumTypes () =
    let red = ColorEnum.Red
    let redInt = int ColorEnum.Red
    let redAgain: ColorEnum = enum redInt
    
    let unknown:ColorEnum = enum 99 

    match red with
        | ColorEnum.Red -> printfn "red"
        | ColorEnum.Blue -> printfn "blue"
        | ColorEnum.Green -> printfn "green"
        | _ -> printfn "unknown"
    0