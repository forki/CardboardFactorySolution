namespace Domain.Product.Test

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
    let ``Calculate lengths for box0200``() =
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

        Assert.That(Calculation.calculateSheetSizes(CorrugationTypes.Enum.C, product).[0].LengthOne, Is.EqualTo(1.258))
        Assert.That(Calculation.calculateSheetSizes(CorrugationTypes.Enum.C, product).[0].LengthTwo, Is.EqualTo(0.356))

        Assert.That(Calculation.calculateSheetSizes(CorrugationTypes.Enum.EAndC, product).[0].LengthOne, Is.EqualTo(1.258))
        Assert.That(Calculation.calculateSheetSizes(CorrugationTypes.Enum.EAndC, product).[0].LengthTwo, Is.EqualTo(0.356))

        Assert.That(Calculation.calculateSheetSizes(CorrugationTypes.Enum.E, product).[0].LengthOne, Is.EqualTo(1.238))
        Assert.That(Calculation.calculateSheetSizes(CorrugationTypes.Enum.E, product).[0].LengthTwo, Is.EqualTo(0.352))

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

        let sheetSizes = Calculation.calculateSheetSizes(enum, product)
        Assert.AreEqual(1, sheetSizes.Length)
        Assert.AreEqual(240.0, sheetSizes.[0].LengthOne)
        Assert.AreEqual(410.0, sheetSizes.[0].LengthTwo)


    [<Test>]
    let ``Calculate lengths with errors``() =
        let lengthOneFormulas = new Dictionary<CorrugationTypes.Enum, Product.LengthFormula>()
        lengthOneFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = "[BadParameter]"});

        let sub =  { defaultSubProduct with Product.SubProduct.LengthOneFormulas = lengthOneFormulas; }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }

        Assert.Throws<ArgumentException>
            (fun () -> Calculation.calculateSheetSizes(CorrugationTypes.Enum.C, product) |> ignore)
            |> ignore

        lengthOneFormulas.Clear()
        lengthOneFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = String.Empty});
        let sub =  { defaultSubProduct with Product.SubProduct.LengthOneFormulas = lengthOneFormulas; }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }
        Assert.Throws<Calculation.ProductTypeNoFormulaException>
            (fun () -> Calculation.calculateSheetSizes(CorrugationTypes.Enum.C, product) |> ignore)
            |> ignore
        
        lengthOneFormulas.Clear()
        lengthOneFormulas.Add(CorrugationTypes.Enum.All, { 
            CorrugationType = CorrugationTypes.Enum.All; 
            FormulaText = "=-5224=+-342fff....+342---2342"});
        let sub =  { defaultSubProduct with Product.SubProduct.LengthOneFormulas = lengthOneFormulas; }
        let list = [sub]
        let product = { defaultProductType with Name = "Test"; SubProducts = list }
        Assert.Throws<Calculation.ProductTypeExpressionHasErrorException>
            (fun () -> Calculation.calculateSheetSizes(CorrugationTypes.Enum.C, product) |> ignore)
            |> ignore
