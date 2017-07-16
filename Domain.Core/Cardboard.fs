namespace Domain.Core

module Cardboard =
    [<System.FlagsAttribute>]
    type CorrugationTypes =
        | E = 0b000001
        | C = 0b000010
        | EAndC = 0b000100
        | AllWithoutPolygraphy = 0b000111
        | Polygraphy = 0b010000
        | All = 0b010111

    type CardboardBrands =
        | D = 1
        | T = 2
        | P = 3

    type CardboardClasses =
        | Class21 = 21
        | Class22 = 22
        | Class23 = 23

    type CardboardColors =
        | Brown = 1
        | White = 2
        | WhiteWhite = 3
