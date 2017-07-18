namespace Domain.Product

[<RequireQualifiedAccess>]
module Calculation =
    open System
    open System.Collections.Generic
    open Domain.Core.Cardboard
    open NCalc

    let private LENGTH_ONE_PATTERN = "L1";
    let private LENGTH_TWO_PATTERN = "L2";

    type ProductTypeNoFormulaException (msg:string) =
        inherit Exception(msg)

    type ProductTypeExpressionHasErrorException (msg:string) = 
        inherit Exception(msg)

    type ProductTypeBadArgumentException (msg:string) = 
        inherit Exception(msg)

    let private parametersToExpressionParameters (parameters:Dictionary<string, Product.ProductParameter>) = 
        parameters
        |> Seq.map (fun pair -> pair.Key, pair.Value.Value :> obj)
        |> dict

    let private createAndEvaluateExpresion (formula:string, parameters:Dictionary<string, Product.ProductParameter>) =
        let expression = new Expression(formula)
        expression.Parameters <- new Dictionary<string, obj>(parametersToExpressionParameters parameters)
        if expression.HasErrors()
        then raise (ProductTypeExpressionHasErrorException(expression.Error))
        let result = expression.Evaluate()
        unbox<double>(result)
    
    let private calculateSheetLength (product:Product.ProductType, subProduct:Product.SubProduct, corrugationType:CorrugationTypes.Enum) = 
        let formula = 
            subProduct.LengthOneFormulas
            |> Seq.tryFind (fun (pair) -> pair.Key.HasFlag(corrugationType))
            |> Option.map (fun (pair) -> pair.Value.FormulaText)
        match formula with
        | Some text -> createAndEvaluateExpresion(text, product.Parameters)
        | None -> raise (ProductTypeNoFormulaException(sprintf "No formula found for %A" corrugationType))

    let private calculateSheetWidth (product:Product.ProductType, subProduct:Product.SubProduct, corrugationType:CorrugationTypes.Enum) = 
        let formula = 
            subProduct.LengthTwoFormulas
            |> Seq.tryFind (fun (pair) -> pair.Key.HasFlag(corrugationType))
            |> Option.map (fun (pair) -> pair.Value.FormulaText)
        match formula with
        | Some text -> createAndEvaluateExpresion(text, product.Parameters)
        | None -> raise (ProductTypeNoFormulaException(sprintf "No formula found for %A" corrugationType))

    [<CompiledName("CalculateSheetSizes")>] 
    let calculateSheetSizes(corrugationType:CorrugationTypes.Enum, product:Product.ProductType) =
        product.SubProducts
        |> Seq.map (fun (subProduct) -> new Product.SheetSizes(
                                            subProduct.Name,
                                            calculateSheetLength(product, subProduct, corrugationType),
                                            calculateSheetWidth(product, subProduct, corrugationType)))
        |> Seq.toArray
    
    let private lengthParametersToExpressionParameters (product:Product.ProductType) (corrugationType:CorrugationTypes.Enum) = 
        let list =
            match product.SubProducts with
            | single :: [] -> 
                [
                (LENGTH_ONE_PATTERN, calculateSheetLength(product, single, corrugationType) :> obj)
                (LENGTH_TWO_PATTERN, calculateSheetWidth(product, single, corrugationType) :> obj) ]
            | m :: ultiple -> 
                product.SubProducts
                |> List.mapi  (fun i prod -> 
                    [
                    (sprintf "%s_%i" LENGTH_ONE_PATTERN (i + 1), calculateSheetLength(product, prod, corrugationType) :> obj)
                    (sprintf "%s_%i" LENGTH_TWO_PATTERN (i + 1), calculateSheetWidth(product, prod, corrugationType) :> obj) ])
                |> List.concat
            | [] -> failwith "Product has no SubProducts"
        dict list

    let private createAndEvaluateStampKnivesLengthExpresion (formula:string, product:Product.ProductType, corrugationType:CorrugationTypes.Enum) =
        let expression = new Expression(formula)
        expression.Parameters <- new Dictionary<string, obj>(lengthParametersToExpressionParameters product corrugationType)
        if expression.HasErrors()
        then raise (ProductTypeExpressionHasErrorException(expression.Error))
        let result = expression.Evaluate()
        unbox<double>(result)

    [<CompiledName("CalculateStampKnivesLength")>] 
    let calculateStampKnivesLength(corrugationType:CorrugationTypes.Enum, product:Product.ProductType) =
        createAndEvaluateStampKnivesLengthExpresion(product.StampKnivesLengthFormula, product, corrugationType)
