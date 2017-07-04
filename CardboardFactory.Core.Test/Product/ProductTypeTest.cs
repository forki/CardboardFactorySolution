using CardboardFactory.Core.Product;
using NUnit.Framework;

namespace CardboardFactory.Core.Test.Product {
    [TestFixture]
    public class TestProductType {
        private const string LengthName = "Length";
        private const string WidthName = "Width";

        private readonly ProductType DefaultProductType = new ProductType {
            Parameters = {
                [LengthName] = new ProductParameter {
                    Name = LengthName
                },
                [WidthName] = new ProductParameter {
                    Name = WidthName
                }
            }
        };

        //[Test]
        //public void CalculateLengthsForBox0200Test() {
        //    CorrugationTypes executionOne = CorrugationTypes.C | CorrugationTypes.EAndC;
        //    var executionTwo = CorrugationTypes.E;

        //    var product = new ProductType("Короб 0200");
        //    product.Parameters.Clear();
        //    product.Parameters.Add("Длина", new ProductParameter {
        //        Name = "Длина"
        //    });
        //    product.Parameters.Add("Ширина", new ProductParameter {
        //        Name = "Ширина"
        //    });
        //    product.Parameters.Add("Высота", new ProductParameter {
        //        Name = "Высота"
        //    });

        //    product.LengthOneFormulas.Clear();
        //    product.LengthOneFormulas.Add(executionOne, new LengthFormula(executionOne,
        //        "2 * ([Длина] + [Ширина]) + 0.058"));
        //    product.LengthOneFormulas.Add(executionTwo, new LengthFormula(executionTwo,
        //        "2 * ([Длина] + [Ширина]) + 0.038"));

        //    product.LengthTwoFormulas.Clear();
        //    product.LengthTwoFormulas.Add(executionOne, new LengthFormula(executionOne,
        //        "[Высота] + [Ширина] / 2 + 0.006"));
        //    product.LengthTwoFormulas.Add(executionTwo, new LengthFormula(executionTwo,
        //        "[Высота] + [Ширина] / 2 + 0.002"));

        //    product.Parameters["Длина"].Value = 0.300;
        //    product.Parameters["Ширина"].Value = 0.300;
        //    product.Parameters["Высота"].Value = 0.200;

        //    Assert.That(() => product.CalculateSheetLength(CorrugationTypes.C), Is.EqualTo(1.258));
        //    Assert.That(() => product.CalculateSheetWidth(CorrugationTypes.C), Is.EqualTo(0.356));

        //    Assert.That(() => product.CalculateSheetLength(CorrugationTypes.EAndC), Is.EqualTo(1.258));
        //    Assert.That(() => product.CalculateSheetWidth(CorrugationTypes.EAndC), Is.EqualTo(0.356));

        //    Assert.That(() => product.CalculateSheetLength(CorrugationTypes.E), Is.EqualTo(1.238));
        //    Assert.That(() => product.CalculateSheetWidth(CorrugationTypes.E), Is.EqualTo(0.352));
        //}

        //[Test]
        //[TestCase(CorrugationTypes.C)]
        //[TestCase(CorrugationTypes.E)]
        //[TestCase(CorrugationTypes.EAndC)]
        //public void CalculateLengthsWithCustomValuesTest(CorrugationTypes type) {
        //    ProductType productType = DefaultProductType;
        //    productType.Parameters[LengthName].Value = 120.0;
        //    Assert.AreEqual(120.0, productType.Parameters[LengthName].Value);
        //    productType.Parameters[WidthName].Value = 240.0;
        //    Assert.AreEqual(240.0, productType.Parameters[WidthName].Value);

        //    productType.LengthOneFormulas[CorrugationTypes.All] = new LengthFormula(CorrugationTypes.All, $"[{LengthName}] + [{LengthName}]");
        //    productType.LengthTwoFormulas[CorrugationTypes.All] = new LengthFormula(CorrugationTypes.All, $"[{WidthName}] + [{LengthName}] + 50]");

        //    Assert.AreEqual(240.0, productType.CalculateSheetLength(type));
        //    Assert.AreEqual(410.0, productType.CalculateSheetWidth(type));
        //}

        //[Test]
        //public void CalculateLengthsWithErrorsTest() {
        //    ProductType productType = DefaultProductType;
        //    productType.LengthOneFormulas[CorrugationTypes.All] = new LengthFormula(CorrugationTypes.All, "[BadParameter]");
        //    Assert.Throws<ProductTypeBadArgumentException>(() => productType.CalculateSheetLength(CorrugationTypes.C));
        //    productType.LengthOneFormulas[CorrugationTypes.All] = new LengthFormula(CorrugationTypes.All, string.Empty);
        //    Assert.Throws<ProductTypeNoFormulaException>(() => productType.CalculateSheetLength(CorrugationTypes.C));
        //    productType.LengthOneFormulas[CorrugationTypes.All] = new LengthFormula(CorrugationTypes.All, "=-5224=+-342fff....+342---2342");
        //    Assert.Throws<ProductTypeExpressionHasErrorException>(() => productType.CalculateSheetLength(CorrugationTypes.C));
        //}

        //[Test]
        //[TestCase(CorrugationTypes.C, 120.0, 240.0, "[Length] + [Length]", "[Width] + [Length] + 50]", 240.0, 410.0)]
        //[TestCase(CorrugationTypes.E, 120.0, 120.0, "[Length] + [Width]", "[Width] + [Width] + 50]", 240.0, 290.0)]
        //[TestCase(CorrugationTypes.C, 400.0, 240.0, "[Length] * 2", "[Length] - [Width] + 50]", 800.0, 210.0)]
        //public void CalculateLengthsWithNewValuesAndFormulasTest(CorrugationTypes type, double length, double width, string formulaLengthOne, string formulaLengthTwo, double result1, double result2) {
        //    ProductType productType = DefaultProductType;
        //    productType.Parameters["Length"].Value = length;
        //    productType.Parameters[WidthName].Value = width;
        //    productType.LengthOneFormulas[CorrugationTypes.All] = new LengthFormula(CorrugationTypes.All, formulaLengthOne);
        //    productType.LengthTwoFormulas[CorrugationTypes.All] = new LengthFormula(CorrugationTypes.All, formulaLengthTwo);

        //    Assert.AreEqual(result1, productType.CalculateSheetLength(type));
        //    Assert.AreEqual(result2, productType.CalculateSheetWidth(type));
        //}
    }
}
