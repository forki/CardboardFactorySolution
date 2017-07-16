using CardboardFactory.Core.Product;
using CardboardFactory.Core.Tools;

namespace CardboardFactory.ProductPriceCalculation.Model {
    public class ProductPriceCalculator {
        private readonly ProductType _productType;
        private readonly OrderParameter _orderParameter;

        public ProductPriceCalculator(ProductType productType, OrderParameter orderParameter) {
            _productType = productType;
            _orderParameter = orderParameter;
        }

        public ProductCalculationResult Calculate() {
            var result = new ProductCalculationResult();
            try {
                result.BlanksSizes = _productType.CalculateBlankSizeses(_orderParameter.CorrugationType);
                result.ProductPrice = (result.ProductArea * _orderParameter.CardboardPrice).Round(2);
                result.IsValid = true;

                if (_orderParameter.ShouldCalculateStampPrice && _orderParameter.PricePerKnifeMeter.HasValue) {
                    result.StampPrice = (_productType.CalculateStampKnivesLength(_orderParameter.CorrugationType) * _orderParameter.PricePerKnifeMeter.Value).Round(2);
                }
            } catch (ProductTypeNoFormulaException) {
                return ProductCalculationResult.GetCalculationErrorResult("Не найдена формула для данного типа картона");
            } catch (ProductTypeBadArgumentException e) {
                return ProductCalculationResult.GetCalculationErrorResult($"В формуле присутствует неизвестный аргумент:\n{e.Message}");
            } catch (ProductTypeExpressionHasErrorException e) {
                return ProductCalculationResult.GetCalculationErrorResult($"В процессе расчета формулы произошла ошибка:\n{e.Message}");
            }
            return result;
        }
    }
}
