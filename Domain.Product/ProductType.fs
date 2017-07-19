namespace Domain.Product
 
 module Product =
     open System
     open Domain.Core.Cardboard

     type Name = string

     type ProductParameter = {
         Name: Name
         Value: float Nullable
     }
     
     type LengthFormula = {
         CorrugationType : CorrugationTypes.Enum
         FormulaText : string
     }
     
     type SubProduct = {
         Name : Name
         LengthOneFormulas : Map<CorrugationTypes.Enum, LengthFormula>
         LengthTwoFormulas : Map<CorrugationTypes.Enum, LengthFormula>
     }
     
     type ProductType = {
         Name : Name
         Parameters : Map<string, ProductParameter>
         SubProducts : SubProduct list
         StampKnivesLengthFormula : string
     }
 
     type SheetSizes = {
         Name : Name
         LengthOne : float
         LengthTwo : float }
         with member s.Area = s.LengthOne * s.LengthTwo

     [<CompiledName("SetParametersFromOther")>] 
     let setParametersFromOther (product:ProductType) (otherProduct:ProductType) = 
         { product with Parameters = otherProduct.Parameters }

     [<CompiledName("SetParametersFromOtherParameters")>] 
     let setParametersFromOtherParameters (product:ProductType) (otherParameters:Map<string, ProductParameter>) = 
         { product with Parameters = otherParameters }