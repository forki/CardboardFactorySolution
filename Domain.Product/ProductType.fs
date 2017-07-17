namespace Domain.Product
 
 module Product =
     open System
     open System.Collections.Generic
     open Domain.Core.Cardboard
 
     type ProductParameter = {
         Name: string
         Value: Nullable<double>
     }
     
     type LengthFormula = {
         CorrugationType : CorrugationTypes.Enum
         FormulaText : string
     }
     
     type SubProduct = {
         Name : string
         LengthOneFormulas : Dictionary<CorrugationTypes.Enum, LengthFormula>
         LengthTwoFormulas : Dictionary<CorrugationTypes.Enum, LengthFormula>
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

     let setParametersFromOtherParameters (product:ProductType, otherParameters:Dictionary<string, ProductParameter>) = 
         { product with Parameters = otherParameters }