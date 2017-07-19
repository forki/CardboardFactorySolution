namespace Domain.Product.Test

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
module ProductTypeTest =
    open Domain.Product
    open Domain.Core.Cardboard
    open System.Collections.Generic
    open System
    open NUnit.Framework

    let private lengthName = "Length"
    let private widthName = "Width"

    let private defaultSubProduct = {
        Product.SubProduct.Name = "Default";
        Product.SubProduct.LengthOneFormulas = 
            new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>(dict []);
        Product.SubProduct.LengthTwoFormulas  = 
            new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>(dict [])
    }

    let private defaultProductType = {
        Product.ProductType.Name = "Default";
        Product.ProductType.Parameters = 
            new Dictionary<string, Product.ProductParameter>(
                dict 
                    [
                        lengthName, {Product.ProductParameter.Name = lengthName; Product.ProductParameter.Value = new Nullable<double>()};
                        widthName, {Product.ProductParameter.Name = widthName; Product.ProductParameter.Value = new Nullable<double>()} ]
                    );
        Product.ProductType.SubProducts = [];
        Product.ProductType.StampKnivesLengthFormula = ""
    }

    [<Test>]
    let ``Calculate lengths for Box0200``() =
        let executionOne = CorrugationTypes.Enum.C ||| CorrugationTypes.Enum.EAndC;
        let executionTwo = CorrugationTypes.Enum.E;

        let parameters = new Dictionary<string, Product.ProductParameter>()
        parameters.Add("Длина", { Name = "Длина"; Value = Nullable<double>(0.300)});
        parameters.Add("Ширина", { Name = "Ширина"; Value = Nullable<double>(0.300)});
        parameters.Add("Высота", { Name = "Высота"; Value = Nullable<double>(0.200)});

        let lengthOneFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthOneFormulas.Add(executionOne, { CorrugationType = executionOne; FormulaText = "2 * ([Длина] + [Ширина]) + 0.058"});
        lengthOneFormulas.Add(executionTwo, { CorrugationType = executionTwo; FormulaText = "2 * ([Длина] + [Ширина]) + 0.038"});

        let lengthTwoFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthTwoFormulas.Add(executionOne, { CorrugationType = executionOne; FormulaText = "[Высота] + [Ширина] / 2 + 0.006"});
        lengthTwoFormulas.Add(executionTwo, { CorrugationType = executionTwo; FormulaText = "[Высота] + [Ширина] / 2 + 0.002"});

        let sub =  { Product.SubProduct.Name = "Sub"; Product.SubProduct.LengthOneFormulas = lengthOneFormulas; Product.SubProduct.LengthTwoFormulas = lengthTwoFormulas }
        
        let product = { defaultProductType with Name = "Короб 0200"; Parameters = parameters; SubProducts = [sub] }

        Assert.That((Calculation.calculateSheetSizes CorrugationTypes.Enum.C product).[0].LengthOne, Is.EqualTo(1.258))
        Assert.That((Calculation.calculateSheetSizes CorrugationTypes.Enum.C product).[0].LengthTwo, Is.EqualTo(0.356))

        Assert.That((Calculation.calculateSheetSizes CorrugationTypes.Enum.EAndC product).[0].LengthOne, Is.EqualTo(1.258))
        Assert.That((Calculation.calculateSheetSizes CorrugationTypes.Enum.EAndC product).[0].LengthTwo, Is.EqualTo(0.356))

        Assert.That((Calculation.calculateSheetSizes CorrugationTypes.Enum.E product).[0].LengthOne, Is.EqualTo(1.238))
        Assert.That((Calculation.calculateSheetSizes CorrugationTypes.Enum.E product).[0].LengthTwo, Is.EqualTo(0.352))

    [<Test>]
    [<TestCase(CorrugationTypes.Enum.C)>]
    [<TestCase(CorrugationTypes.Enum.E)>]
    [<TestCase(CorrugationTypes.Enum.EAndC)>]
    let ``Calculate lengths with custom values``(enum:CorrugationTypes.Enum) =
        let lengthOneFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthOneFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = sprintf "[%s] + [%s]" lengthName lengthName});

        let lengthTwoFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthTwoFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = sprintf "[%s] + [%s] + 50" widthName lengthName});

        let sub =  { Product.SubProduct.Name = "Sub"; Product.SubProduct.LengthOneFormulas = lengthOneFormulas; Product.SubProduct.LengthTwoFormulas = lengthTwoFormulas }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }
        product.Parameters.[lengthName] <- { product.Parameters.[lengthName] with Value = Nullable<double>(120.0)}
        Assert.AreEqual(120.0, product.Parameters.[lengthName].Value)
        product.Parameters.[widthName] <- { product.Parameters.[lengthName] with Value = Nullable<double>(240.0)}
        Assert.AreEqual(240.0, product.Parameters.[widthName].Value)

        let sheetSizes = Calculation.calculateSheetSizes enum product
        Assert.AreEqual(1, sheetSizes.Length)
        Assert.AreEqual(240.0, sheetSizes.[0].LengthOne)
        Assert.AreEqual(410.0, sheetSizes.[0].LengthTwo)


    [<Test>]
    let ``Calculate lengths with not existing argument``() =
        let lengthOneFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthOneFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = "[BadParameter]"});

        let sub =  { defaultSubProduct with Product.SubProduct.LengthOneFormulas = lengthOneFormulas; }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }

        Assert.Throws<ArgumentException>
            (fun () -> Calculation.calculateSheetSizes CorrugationTypes.Enum.C product |> ignore)
            |> ignore

    [<Test>]
    let ``Calculate lengths with no formula``() =
        let lengthOneFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        let sub =  { defaultSubProduct with Product.SubProduct.LengthOneFormulas = lengthOneFormulas; }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }
        Assert.Throws<Calculation.ProductTypeNoFormulaException>
            (fun () -> Calculation.calculateSheetSizes CorrugationTypes.Enum.C product |> ignore)
            |> ignore

    [<Test>]
    let ``Calculate lengths with empty formula``() =
        let lengthOneFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthOneFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = String.Empty});
        let sub =  { defaultSubProduct with Product.SubProduct.LengthOneFormulas = lengthOneFormulas; }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }
        Assert.Throws<Calculation.ProductTypeNoFormulaException>
            (fun () -> Calculation.calculateSheetSizes CorrugationTypes.Enum.C product |> ignore)
            |> ignore
    
    [<Test>]
    let ``Calculate lengths with wrong formula``() =   
        let lengthOneFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthOneFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = "=-5224=+-342fff....+342---2342"});
        let sub =  { defaultSubProduct with Product.SubProduct.LengthOneFormulas = lengthOneFormulas; }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }
        Assert.Throws<Calculation.ProductTypeExpressionHasErrorException>
            (fun () -> Calculation.calculateSheetSizes CorrugationTypes.Enum.C product |> ignore)
            |> ignore

    [<Test>]
    [<TestCase(CorrugationTypes.Enum.C, 120.0, 240.0, "[Length] + [Length]", "[Width] + [Length] + 50]", 240.0, 410.0)>]
    [<TestCase(CorrugationTypes.Enum.E, 120.0, 120.0, "[Length] + [Width]", "[Width] + [Width] + 50]", 240.0, 290.0)>]
    [<TestCase(CorrugationTypes.Enum.C, 400.0, 240.0, "[Length] * 2", "[Length] - [Width] + 50]", 800.0, 210.0)>]
    let ``Calculate lengths with new values and formulas``(enum:CorrugationTypes.Enum, length:double, width:double, formulaLengthOne:string, formulaLengthTwo:string, result1:double, result2:double) =
        let lengthOneFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthOneFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = formulaLengthOne});

        let lengthTwoFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthTwoFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = formulaLengthTwo});
        let sub =  { Product.SubProduct.Name = "Sub"; Product.SubProduct.LengthOneFormulas = lengthOneFormulas; Product.SubProduct.LengthTwoFormulas = lengthTwoFormulas }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }
        product.Parameters.[lengthName] <- { product.Parameters.[lengthName] with Value = Nullable<double>(length)}
        product.Parameters.[widthName] <- { product.Parameters.[lengthName] with Value = Nullable<double>(width)}

        let sheetSizes = Calculation.calculateSheetSizes enum product
        Assert.AreEqual(1, sheetSizes.Length)
        Assert.AreEqual(result1, sheetSizes.[0].LengthOne);
        Assert.AreEqual(result2, sheetSizes.[0].LengthTwo);
