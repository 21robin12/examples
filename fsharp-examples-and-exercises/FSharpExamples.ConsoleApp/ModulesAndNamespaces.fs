// notes taken from O'Reilly book; these are quite basic. for much more detailed discussion on organizing code, see:
// https://fsharpforfunandprofit.com/posts/organizing-functions/

namespace PlayingCards // each file has a top-level module OR namespace. doesn't need = sign. O'Reilly book suggests just using modules for learning.
    type Suit = // namespaces can include types
        | Spade
        | Club
        | Diamond
        | Heart

    type PlayingCard =
        | Ace of Suit
        | King of Suit
        | Queen of Suit
        | Jack of Suit
        | ValueCard of int * Suit

    // namespaces CANNOT contain values
    // let square x = x * x 

    module ModulesAndNamespaces = // namespaces can also contain modules
        let square a =
            a * a

    module ConversionUtils = // subsequent modules/namespaces must have an = to distinguish from top-level module
        let intToString (x : int) =
            x.ToString()

        module ConvertBase = // further nested modules must be indented
            let convertToHex x =
                sprintf "%x" x
