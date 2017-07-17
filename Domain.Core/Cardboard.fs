namespace Domain.Core.Cardboard

open System
open FSharp.Configuration

type Resource = ResXProvider<file="Domain.Core.resx">

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
        | Enum.E -> Some Resource.CorrugationTypes_E
        | Enum.C -> Some Resource.CorrugationTypes_C
        | Enum.EAndC -> Some Resource.CorrugationTypes_EAndC
        | Enum.Polygraphy -> Some Resource.CorrugationTypes_Polygraphy
        | _ -> None

    let stringToEnumMap ([<ParamArray>] enum) =
        enum
        |> Array.map (fun(enum) -> enum, enumToString enum)
        |> Array.filter (fun(_, text) -> text.IsSome)
        |> Array.map (fun(enum, text) -> text.Value, enum)
        |> dict

[<RequireQualifiedAccessAttribute>]
module CardboardBrands =
        type Enum =
        | D = 1
        | T = 2
        | P = 3

        let enumToString enum =
            match enum with
            | Enum.D -> Some Resource.CardboardBrands_D
            | Enum.T -> Some Resource.CardboardBrands_T
            | Enum.P -> Some Resource.CardboardBrands_P
            | _ -> None

        let stringToEnumMap ([<ParamArray>] enum) =
            enum
            |> Array.map (fun(enum) -> enum, enumToString enum)
            |> Array.filter (fun(_, text) -> text.IsSome)
            |> Array.map (fun(enum, text) -> text.Value, enum)
            |> dict

[<RequireQualifiedAccessAttribute>]        
module CardboardClasses =
        type Enum =
        | Class21 = 21
        | Class22 = 22
        | Class23 = 23

        let enumToString enum =
            match enum with
            | Enum.Class21 -> Some Resource.CardboardClasses_Class21
            | Enum.Class22 -> Some Resource.CardboardClasses_Class22
            | Enum.Class23 -> Some Resource.CardboardClasses_Class23
            | _ -> None

        let stringToEnumMap ([<ParamArray>] enum) =
            enum
            |> Array.map (fun(enum) -> enum, enumToString enum)
            |> Array.filter (fun(_, text) -> text.IsSome)
            |> Array.map (fun(enum, text) -> text.Value, enum)
            |> dict

[<RequireQualifiedAccessAttribute>]
module CardboardColors =
    type Enum =
    | Brown = 1
    | White = 2
    | WhiteWhite = 3

    let enumToString enum =
        match enum with
        | Enum.Brown -> Some Resource.CardboardColors_Brown
        | Enum.White -> Some Resource.CardboardColors_White
        | Enum.WhiteWhite -> Some Resource.CardboardColors_WhiteWhite
        | _ -> None

    let stringToEnumMap ([<ParamArray>] enum) =
        enum
        |> Array.map (fun(enum) -> enum, enumToString enum)
        |> Array.filter (fun(_, text) -> text.IsSome)
        |> Array.map (fun(enum, text) -> text.Value, enum)
        |> dict
