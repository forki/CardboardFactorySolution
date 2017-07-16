namespace CardboardFactory.Domain.Product

[<RequireQualifiedAccess>]
module Product =
    open System
    open System.Collections.Generic

    type ProductParameter = {
        Name: string
        Value: Nullable<double>
    }
    
    [<Flags>]
    type CorrugationTypes =
        | E = 0b00000001
        | C = 0b00000010
        | EAndC = 0b00000100
        | AllWithoutPolygraphy = 0b00000111
        | Polygraphy = 0b00001000
        | All = 0b00001111
    
    type LengthFormula = {
        CorrugationType : CorrugationTypes
        FormulaText : string
    }
    
    type SubProduct = {
        Name : string
        LengthOneFormulas : Dictionary<CorrugationTypes, LengthFormula>
        LengthTwoFormulas : Dictionary<CorrugationTypes, LengthFormula>
    }
    
    type ProductType = {
        Name : string
        Parameters : Dictionary<string, ProductParameter>
        SubProducts : List<SubProduct>
        StampKnivesLengthFormula : string
    }

    type SheetSizes(name:string,lengthOne:double,lengthTwo:double) =
        member this.Name = name
        member this.LengthOne = lengthOne
        member this.LengthTwo = lengthTwo
        member this.Area = lengthOne * lengthTwo

    let setParametersFromOther (product:ProductType, otherProduct:ProductType) = 
        { product with Parameters = otherProduct.Parameters }
