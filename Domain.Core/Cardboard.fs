namespace Domain.Core

module Cardboard =
    open System

    [<RequireQualifiedAccessAttribute>]
    module CorrugationTypes =
        [<FlagsAttribute>]
        type Enum =
        | E = 0b000001
        | C = 0b000010
        | EAndC = 0b000100
        | AllWithoutPolygraphy = 0b000111
        | Polygraphy = 0b010000
        | All = 0b010111

        let enumToString enum =
            match enum with
            | Enum.E -> Some "\"E\" волна"
            | Enum.C -> Some "\"C\" волна"
            | Enum.EAndC -> Some "\"E\" волна"
            | Enum.Polygraphy -> Some "Полиграф. картон"
            | _ -> None

        let enumToStringMap ([<ParamArray>] enum) =
            enum
            |> Array.map (fun(enum) -> enum, enumToString enum)
            |> Array.filter (fun(_, text) -> text.IsSome)
            |> Array.map (fun(enum, text) -> enum, text.Value)
            |> dict
    
    [<RequireQualifiedAccessAttribute>]
    module CardboardBrands =
        type Enum =
        | D = 1
        | T = 2
        | P = 3

    [<RequireQualifiedAccessAttribute>]        
    module CardboardClasses =
        type Enum =
        | Class21 = 21
        | Class22 = 22
        | Class23 = 23

    [<RequireQualifiedAccessAttribute>]
    module CardboardColors =
        type Enum =
        | Brown = 1
        | White = 2
        | WhiteWhite = 3
