using System;
using System.Collections.Generic;
using System.Linq;
using CardboardFactory.Core.Tools;
using Domain.Core.Cardboard;
using NCalc;

namespace CardboardFactory.Core.Product {
    public class ProductType {
        private const string LENGTH_ONE_PATTERN = "L1";
        private const string LENGTH_TWO_PATTERN = "L2";

        private CorrugationTypes.Enum CorrugationTypeForCalculations;
        private bool IsSimpleProduct => SubProducts.Count == 1;

        public string Name { get; set; }
        public Dictionary<string, ProductParameter> Parameters { get; }
        public List<SubProduct> SubProducts { get; }
        public string StampKnivesLengthFormula { get; set; }

        public ProductType() : this((string)null) { }

        public ProductType(string name) {
            Parameters = new Dictionary<string, ProductParameter>();
            SubProducts = new List<SubProduct>();
            Name = name ?? "DEFAULT";
        }

        public ProductType(ProductType other) {
            Parameters = new Dictionary<string, ProductParameter>(other.Parameters.Values.ToDictionary(parameter => parameter.Name, parameter => new ProductParameter {
                Name = parameter.Name,
                Value = parameter.Value
            }));
            SubProducts = new List<SubProduct>(other.SubProducts.Select(product => new SubProduct(product)));
            Name = other.Name;
            StampKnivesLengthFormula = other.StampKnivesLengthFormula;
        }

        public List<BlankSizes> CalculateBlankSizeses(CorrugationTypes.Enum corrugationType) {
            var blanks = new List<BlankSizes>();
            foreach (SubProduct subProduct in SubProducts) {
                var blank = new BlankSizes {
                    BlankName = subProduct.Name,
                    LengthOne = CalculateSheetLength(corrugationType, subProduct).Round(3),
                    LengthTwo = CalculateSheetWidth(corrugationType, subProduct).Round(3)
                };
                blanks.Add(blank);
            }
            return blanks;
        }

        public void SetParametersFromOther(ProductType other) {
            foreach (KeyValuePair<string, ProductParameter> parameter in Parameters) {
                if (other.Parameters.ContainsKey(parameter.Key)) {
                    parameter.Value.Value = other.Parameters[parameter.Key].Value;
                }
            }
        }

        public double CalculateStampKnivesLength(CorrugationTypes.Enum corrugationType) {
            CorrugationTypeForCalculations = corrugationType;
            return CreateAndEvaluateExpresion(StampKnivesLengthFormula);
        }

        private double CalculateSheetLength(CorrugationTypes.Enum corrugationType, SubProduct subProduct) {
            LengthFormula formula = subProduct.LengthOneFormulas.Where(pair => pair.Key.HasFlag(corrugationType)).Select(pair => pair.Value).FirstOrDefault();
            return CreateAndEvaluateExpresion(formula?.FormulaText ?? throw new ProductTypeNoFormulaException($"No formula found for {corrugationType}"));
        }

        private double CalculateSheetWidth(CorrugationTypes.Enum corrugationType, SubProduct subProduct) {
            LengthFormula formula = subProduct.LengthTwoFormulas.Where(pair => pair.Key.HasFlag(corrugationType)).Select(pair => pair.Value).FirstOrDefault();
            return CreateAndEvaluateExpresion(formula?.FormulaText ?? throw new ProductTypeNoFormulaException($"No formula found for {corrugationType}"));
        }

        private double CreateAndEvaluateExpresion(string formula) {
            if (string.IsNullOrEmpty(formula)) { throw new ProductTypeNoFormulaException("No formula found"); }
            var expression = new Expression(formula);
            expression.EvaluateParameter += ExpressionOnEvaluateParameter;
            if (expression.HasErrors()) {
                throw new ProductTypeExpressionHasErrorException(expression.Error);
            }
            return (double)expression.Evaluate();
        }

        private void ExpressionOnEvaluateParameter(string name, ParameterArgs args) {
            if (name.StartsWith(LENGTH_ONE_PATTERN)) {
                args.Result = GetLangthOneParameter(name, LENGTH_ONE_PATTERN, CalculateSheetLength);
                return;
            }
            if (name.StartsWith(LENGTH_TWO_PATTERN)) {
                args.Result = GetLangthOneParameter(name, LENGTH_TWO_PATTERN, CalculateSheetWidth);
                return;
            }
            if (Parameters.TryGetValue(name, out ProductParameter parameter)) {
                args.Result = parameter.Value;
                return;
            }
            throw new ProductTypeBadArgumentException($"There is no parameter with name: {name} in product");
        }

        private double GetLangthOneParameter(string name, string pattern, Func<CorrugationTypes.Enum, SubProduct, double> func) {
            if (name.Equals(pattern)) {
                SubProduct subProduct = SubProducts.SingleOrDefault();
                if (!IsSimpleProduct || subProduct == null) {
                    throw new ProductTypeBadArgumentException("Для сложных продуктов стоит использовать [L1_1], [L2_1] в формуле");
                }
                return func(CorrugationTypeForCalculations, subProduct);
            }
            if (IsSimpleProduct) {
                throw new ProductTypeBadArgumentException("Для простых продуктов стоит использовать [L1], [L2] в формуле");
            }
            if (name.Contains('_')) {
                string[] split = name.Split('_');
                if (split.Length == 2 && int.TryParse(split[1], out int number) && number > 0 && number <= SubProducts.Count) {
                    return func(CorrugationTypeForCalculations, SubProducts[number - 1]);
                }
            }
            throw new ProductTypeBadArgumentException("Для сложных продуктов допустимые параметры [L1_1], [L2_1], [L1_2], [L2_2] в формуле");
        }
    }

    [Serializable]
    public class ProductTypeNoFormulaException : Exception {
        public override string Message { get; }

        public ProductTypeNoFormulaException(string message) {
            Message = message;
        }
    }

    [Serializable]
    public class ProductTypeExpressionHasErrorException : Exception {
        public override string Message { get; }

        public ProductTypeExpressionHasErrorException(string message) {
            Message = message;
        }
    }

    [Serializable]
    public class ProductTypeBadArgumentException : Exception {
        public override string Message { get; }

        public ProductTypeBadArgumentException(string message) {
            Message = message;
        }
    }
}
