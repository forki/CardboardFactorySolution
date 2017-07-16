using System.Collections.Generic;
using CardboardFactory.Core;
using CardboardFactory.Core.Tools;
using CardboardFactory.Domain.Product;

namespace CardboardFactory.ProductPriceCalculation.Model {
    public class ProductPriceCalculator {
        private readonly Product.ProductType _productType;
        private readonly OrderParameter _orderParameter;

        public ProductPriceCalculator(Product.ProductType productType, OrderParameter orderParameter) {
            _productType = productType;
            _orderParameter = orderParameter;
        }

        public ProductCalculationResult Calculate() {
            var result = new ProductCalculationResult();
            try {
                result.BlanksSizes = new List<Product.SheetSizes>(Calculation.calculateSheetSizes(_orderParameter.CorrugationType, _productType));
                result.ProductPrice = (result.ProductArea * _orderParameter.CardboardPrice).Round(2);
                result.IsValid = true;

                //if (_orderParameter.ShouldCalculateStampPrice && _orderParameter.PricePerKnifeMeter.HasValue) {
                //    result.StampPrice = (_productType.CalculateStampKnivesLength(_orderParameter.CorrugationType) * _orderParameter.PricePerKnifeMeter.Value).Round(2);
                //}
            } catch (Calculation.ProductTypeNoFormulaException) {
                return ProductCalculationResult.GetCalculationErrorResult("Не найдена формула для данного типа картона");
            } catch (Calculation.ProductTypeExpressionHasErrorException e) {
                return ProductCalculationResult.GetCalculationErrorResult($"В процессе расчета формулы произошла ошибка:\n{e.Message}");
            }
            return result;
        }
    }
}
