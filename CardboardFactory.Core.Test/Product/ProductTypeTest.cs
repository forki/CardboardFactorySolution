//using System.Collections.Generic;
//using CardboardFactory.Core.Product;
//using Domain.Core.Cardboard;
//using NUnit.Framework;

//namespace CardboardFactory.Core.Test.Product {
//    [TestFixture]
//    public class TestProductType {
//        private const string LengthName = "Length";
//        private const string WidthName = "Width";

//        private readonly ProductType DefaultProductType = new ProductType {
//            Parameters = {
//                [LengthName] = new ProductParameter {
//                    Name = LengthName
//                },
//                [WidthName] = new ProductParameter {
//                    Name = WidthName
//                }
//            }
//        };

//        [Test]
//        public void CalculateLengthsForBox0200Test() {
//            CorrugationTypes.Enum executionOne = CorrugationTypes.Enum.C | CorrugationTypes.Enum.EAndC;
//            var executionTwo = CorrugationTypes.Enum.E;

//            var product = new ProductType("Короб 0200");
//            product.Parameters.Clear();
//            product.Parameters.Add("Длина", new ProductParameter {
//                Name = "Длина"
//            });
//            product.Parameters.Add("Ширина", new ProductParameter {
//                Name = "Ширина"
//            });
//            product.Parameters.Add("Высота", new ProductParameter {
//                Name = "Высота"
//            });

//            var sub = new SubProduct {
//                LengthOneFormulas = {
//                    [executionOne] = new LengthFormula(executionOne, "2 * ([Длина] + [Ширина]) + 0.058"),
//                    [executionTwo] = new LengthFormula(executionOne, "2 * ([Длина] + [Ширина]) + 0.038")
//                },
//                LengthTwoFormulas = {
//                    [executionOne] = new LengthFormula(executionOne, "[Высота] + [Ширина] / 2 + 0.006"),
//                    [executionTwo] = new LengthFormula(executionOne, "[Высота] + [Ширина] / 2 + 0.002")
//                }
//            };
//            product.SubProducts.Add(sub);
//            product.Parameters["Длина"].Value = 0.300;
//            product.Parameters["Ширина"].Value = 0.300;
//            product.Parameters["Высота"].Value = 0.200;

//            Assert.That(() => product.CalculateBlankSizeses(CorrugationTypes.Enum.C)[0].LengthOne, Is.EqualTo(1.258));
//            Assert.That(() => product.CalculateBlankSizeses(CorrugationTypes.Enum.C)[0].LengthTwo, Is.EqualTo(0.356));

//            Assert.That(() => product.CalculateBlankSizeses(CorrugationTypes.Enum.EAndC)[0].LengthOne, Is.EqualTo(1.258));
//            Assert.That(() => product.CalculateBlankSizeses(CorrugationTypes.Enum.EAndC)[0].LengthTwo, Is.EqualTo(0.356));

//            Assert.That(() => product.CalculateBlankSizeses(CorrugationTypes.Enum.E)[0].LengthOne, Is.EqualTo(1.238));
//            Assert.That(() => product.CalculateBlankSizeses(CorrugationTypes.Enum.E)[0].LengthTwo, Is.EqualTo(0.352));
//        }

//        [Test]
//        [TestCase(CorrugationTypes.Enum.C)]
//        [TestCase(CorrugationTypes.Enum.E)]
//        [TestCase(CorrugationTypes.Enum.EAndC)]
//        public void CalculateLengthsWithCustomValuesTest(CorrugationTypes.Enum type) {
//            var productType = new ProductType(DefaultProductType);
//            productType.Parameters[LengthName].Value = 120.0;
//            Assert.AreEqual(120.0, productType.Parameters[LengthName].Value);
//            productType.Parameters[WidthName].Value = 240.0;
//            Assert.AreEqual(240.0, productType.Parameters[WidthName].Value);

//            var sub = new SubProduct {
//                LengthOneFormulas = { [CorrugationTypes.Enum.All] = new LengthFormula(CorrugationTypes.Enum.All, $"[{LengthName}] + [{LengthName}]") },
//                LengthTwoFormulas = { [CorrugationTypes.Enum.All] = new LengthFormula(CorrugationTypes.Enum.All, $"[{WidthName}] + [{LengthName}] + 50]") }
//            };
//            productType.SubProducts.Add(sub);
//            List<BlankSizes> blankSizeses = productType.CalculateBlankSizeses(type);
//            Assert.AreEqual(1, blankSizeses.Count);
//            Assert.AreEqual(240.0, blankSizeses[0].LengthOne);
//            Assert.AreEqual(410.0, blankSizeses[0].LengthTwo);
//        }

//        [Test]
//        public void CalculateLengthsWithErrorsTest() {
//            var productType = new ProductType(DefaultProductType);
//            var sub = new SubProduct {
//                LengthOneFormulas = { [CorrugationTypes.Enum.All] = new LengthFormula(CorrugationTypes.Enum.All, "[BadParameter]") }
//            };
//            productType.SubProducts.Add(sub);
//            Assert.Throws<ProductTypeBadArgumentException>(() => productType.CalculateBlankSizeses(CorrugationTypes.Enum.C));

//            productType = new ProductType(DefaultProductType);
//            sub = new SubProduct {
//                LengthOneFormulas = { [CorrugationTypes.Enum.All] = new LengthFormula(CorrugationTypes.Enum.All, string.Empty) }
//            };
//            productType.SubProducts.Add(sub);
//            Assert.Throws<ProductTypeNoFormulaException>(() => productType.CalculateBlankSizeses(CorrugationTypes.Enum.C));

//            productType = new ProductType(DefaultProductType);
//            sub = new SubProduct {
//                LengthOneFormulas = { [CorrugationTypes.Enum.All] = new LengthFormula(CorrugationTypes.Enum.All, "=-5224=+-342fff....+342---2342") }
//            };
//            productType.SubProducts.Add(sub);
//            Assert.Throws<ProductTypeExpressionHasErrorException>(() => productType.CalculateBlankSizeses(CorrugationTypes.Enum.C));
//        }

//        [Test]
//        [TestCase(CorrugationTypes.Enum.C, 120.0, 240.0, "[Length] + [Length]", "[Width] + [Length] + 50]", 240.0, 410.0)]
//        [TestCase(CorrugationTypes.Enum.E, 120.0, 120.0, "[Length] + [Width]", "[Width] + [Width] + 50]", 240.0, 290.0)]
//        [TestCase(CorrugationTypes.Enum.C, 400.0, 240.0, "[Length] * 2", "[Length] - [Width] + 50]", 800.0, 210.0)]
//        public void CalculateLengthsWithNewValuesAndFormulasTest(CorrugationTypes.Enum type, double length, double width, string formulaLengthOne, string formulaLengthTwo, double result1, double result2) {
//            var productType = new ProductType(DefaultProductType);
//            productType.Parameters[LengthName].Value = length;
//            productType.Parameters[WidthName].Value = width;
//            var sub = new SubProduct {
//                LengthOneFormulas = { [CorrugationTypes.Enum.All] = new LengthFormula(CorrugationTypes.Enum.All, formulaLengthOne) },
//                LengthTwoFormulas = { [CorrugationTypes.Enum.All] = new LengthFormula(CorrugationTypes.Enum.All, formulaLengthTwo) }
//            };
//            productType.SubProducts.Add(sub);
//            List<BlankSizes> blankSizeses = productType.CalculateBlankSizeses(type);
//            Assert.AreEqual(1, blankSizeses.Count);
//            Assert.AreEqual(result1, blankSizeses[0].LengthOne);
//            Assert.AreEqual(result2, blankSizeses[0].LengthTwo);
//        }
//    }
//}
