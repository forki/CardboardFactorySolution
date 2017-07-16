//using System;
//using System.Collections.Generic;
//using System.Linq;
//using CardboardFactory.Core.Tools;
//using NCalc;

//namespace CardboardFactory.Core.Product {
//    public class ProductType {
//        private const string LENGTH_ONE_PATTERN = "L1";
//        private const string LENGTH_TWO_PATTERN = "L2";

//        private CorrugationTypes CorrugationTypeForCalculations;
//        private bool IsSimpleProduct => SubProducts.Count == 1;

//        public string Name { get; set; }
//        public Dictionary<string, ProductParameter> Parameters { get; }
//        public List<SubProduct> SubProducts { get; }
//        public string StampKnivesLengthFormula { get; set; }


//        public double CalculateStampKnivesLength(CorrugationTypes corrugationType) {
//            CorrugationTypeForCalculations = corrugationType;
//            return CreateAndEvaluateExpresion(StampKnivesLengthFormula);
//        }

//        private static double CalculateSheetLength(CorrugationTypes corrugationType, SubProduct subProduct) {
//            LengthFormula formula = subProduct.LengthOneFormulas.Where(pair => pair.Key.HasFlag(corrugationType)).Select(pair => pair.Value).FirstOrDefault();
//            return CreateAndEvaluateExpresion(formula?.FormulaText ?? throw new ProductTypeNoFormulaException($"No formula found for {corrugationType}"));
//        }

//        private double CalculateSheetWidth(CorrugationTypes corrugationType, SubProduct subProduct) {
//            LengthFormula formula = subProduct.LengthTwoFormulas.Where(pair => pair.Key.HasFlag(corrugationType)).Select(pair => pair.Value).FirstOrDefault();
//            return CreateAndEvaluateExpresion(formula?.FormulaText ?? throw new ProductTypeNoFormulaException($"No formula found for {corrugationType}"));
//        }

//        private double CreateAndEvaluateExpresion(string formula) {
//            if (string.IsNullOrEmpty(formula)) { throw new ProductTypeNoFormulaException("No formula found"); }
//            var expression = new Expression(formula);
//            expression.EvaluateParameter += ExpressionOnEvaluateParameter;
//            if (expression.HasErrors()) {
//                throw new ProductTypeExpressionHasErrorException(expression.Error);
//            }
//            return (double)expression.Evaluate();
//        }

//        private void ExpressionOnEvaluateParameter(string name, ParameterArgs args) {
//            if (name.StartsWith(LENGTH_ONE_PATTERN)) {
//                args.Result = GetLangthOneParameter(name, LENGTH_ONE_PATTERN, CalculateSheetLength);
//                return;
//            }
//            if (name.StartsWith(LENGTH_TWO_PATTERN)) {
//                args.Result = GetLangthOneParameter(name, LENGTH_TWO_PATTERN, CalculateSheetWidth);
//                return;
//            }
//            if (Parameters.TryGetValue(name, out ProductParameter parameter)) {
//                args.Result = parameter.Value;
//                return;
//            }
//            throw new ProductTypeBadArgumentException($"There is no parameter with name: {name} in product");
//        }

//        private double GetLangthOneParameter(string name, string pattern, Func<CorrugationTypes, SubProduct, double> func) {
//            if (name.Equals(pattern)) {
//                SubProduct subProduct = SubProducts.SingleOrDefault();
//                if (!IsSimpleProduct || subProduct == null) {
//                    throw new ProductTypeBadArgumentException("Для сложных продуктов стоит использовать [L1_1], [L2_1] в формуле");
//                }
//                return func(CorrugationTypeForCalculations, subProduct);
//            }
//            if (IsSimpleProduct) {
//                throw new ProductTypeBadArgumentException("Для простых продуктов стоит использовать [L1], [L2] в формуле");
//            }
//            if (name.Contains('_')) {
//                string[] split = name.Split('_');
//                if (split.Length == 2 && int.TryParse(split[1], out int number) && number > 0 && number <= SubProducts.Count) {
//                    return func(CorrugationTypeForCalculations, SubProducts[number - 1]);
//                }
//            }
//            throw new ProductTypeBadArgumentException("Для сложных продуктов допустимые параметры [L1_1], [L2_1], [L1_2], [L2_2] в формуле");
//        }
//    }
//}
