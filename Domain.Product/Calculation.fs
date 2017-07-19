namespace Domain.Product

[<RequireQualifiedAccess>]
module Calculation =
    open System
    open System.Collections.Generic
    open Domain.Core.Cardboard
    open NCalc
    open Product

    let private LENGTH_ONE_PATTERN = "L1";
    let private LENGTH_TWO_PATTERN = "L2";

    type ProductTypeNoFormulaException (msg:string) =
        inherit Exception(msg)

    type ProductTypeExpressionHasErrorException (msg:string) = 
        inherit Exception(msg)

    let private parametersToExpressionParameters (parameters:Dictionary<string, Product.ProductParameter>) = 
        parameters
        |> Seq.map (fun pair -> pair.Key, pair.Value.Value :> obj)
        |> dict

    let private createAndEvaluateExpresion (parameters:Dictionary<string, Product.ProductParameter>) (formula:string) =
        let expression = new Expression(formula)
        expression.Parameters <- new Dictionary<string, obj>(parametersToExpressionParameters parameters)
        if expression.HasErrors()
        then raise (ProductTypeExpressionHasErrorException(expression.Error))
        let result = expression.Evaluate()
        unbox<double>(result)
    
    let private findFormula (corrugationType:CorrugationTypes.Enum) (formulas:Dictionary<CorrugationTypes.Enum, Product.LengthFormula>)  =
        let formula = 
            formulas
            |> Seq.tryFind (fun (pair) -> pair.Key.HasFlag(corrugationType))
            |> Option.map (fun (pair) -> pair.Value.FormulaText)
        match formula with
        | Some formula when String.IsNullOrEmpty formula -> raise (ProductTypeNoFormulaException(sprintf "Formula is empty for %A" corrugationType))
        | Some formula -> formula
        | None -> raise (ProductTypeNoFormulaException (sprintf "No formula found for %A" corrugationType))

    let private calculateSheetLength (product:Product.ProductType) (subProduct:Product.SubProduct) (corrugationType:CorrugationTypes.Enum) = 
        subProduct.LengthOneFormulas
        |> findFormula corrugationType
        |> createAndEvaluateExpresion product.Parameters

    let private calculateSheetWidth (product:Product.ProductType) (subProduct:Product.SubProduct) (corrugationType:CorrugationTypes.Enum) = 
        subProduct.LengthTwoFormulas
        |> findFormula corrugationType
        |> createAndEvaluateExpresion product.Parameters

    [<CompiledName("CalculateSheetSizes")>] 
    let calculateSheetSizes(corrugationType:CorrugationTypes.Enum) (product:Product.ProductType) =
        product.SubProducts
        |> Seq.map (fun (subProduct) -> 
            { 
                SheetSizes.Name = subProduct.Name; 
                SheetSizes.LengthOne = calculateSheetLength product subProduct corrugationType; 
                SheetSizes.LengthTwo = calculateSheetWidth product subProduct corrugationType
            })
        |> Seq.toArray
    
    let private lengthParametersToExpressionParameters (product:Product.ProductType) (corrugationType:CorrugationTypes.Enum) = 
        let l1 = "L1"
        let l2 = "L2"
        let list =
            match product.SubProducts with
            | single :: [] -> 
                [
                    (l1, (calculateSheetLength product single corrugationType) |> box)
                    (l2, (calculateSheetWidth product single corrugationType) |> box) ]
            | m :: ultiple -> 
                product.SubProducts
                |> List.mapi  (fun i prod -> 
                    [
                    (sprintf "%s_%i" l1 (i + 1), (calculateSheetLength product prod corrugationType) |> box)
                    (sprintf "%s_%i" l2 (i + 1), (calculateSheetWidth product prod corrugationType) |> box) ])
                |> List.concat
            | [] -> failwith "Product has no SubProducts"
        dict list

    [<CompiledName("CalculateStampKnivesLength")>] 
    let calculateStampKnivesLength(corrugationType:CorrugationTypes.Enum) (product:Product.ProductType) =
        let expression = new Expression(product.StampKnivesLengthFormula)
        expression.Parameters <- Dictionary<string, obj>(lengthParametersToExpressionParameters product corrugationType)
        if expression.HasErrors()
        then raise (ProductTypeExpressionHasErrorException expression.Error)
        let result = expression.Evaluate()
        unbox<double>(result)