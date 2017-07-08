using System;
using System.Collections.Generic;
using System.Text;
using CardboardFactory.Core;
using CardboardFactory.Core.Product;
using CardboardFactory.WpfCore.Tools;

namespace CardboardFactory.ProductPriceCalculation.Model {
    public class FormatProductPriceCalculation {
        private readonly ProductType _productType;
        private readonly OrderParameter _orderParameter;
        private readonly ProductCalculationResult _calculationResult;

        public FormatProductPriceCalculation(
            ProductType productType,
            OrderParameter orderParameter,
            ProductCalculationResult calculationResult) {
            _productType = productType;
            _orderParameter = orderParameter;
            _calculationResult = calculationResult;
        }

        public string Format() {
            var sb = new StringBuilder();
            sb.AppendLine($"Коробка: {_productType.Name}");
            sb.AppendLine("Размеры: ");
            int i = 1;
            foreach (KeyValuePair<string, ProductParameter> pair in _productType.Parameters) {
                sb.Append(i == _productType.Parameters.Count ? $"[{pair.Key}]{pair.Value.Value}{Environment.NewLine}" : $"[{pair.Key}]{pair.Value.Value}/");
                i++;
            }
            sb.AppendLine();
            var converter = new EnumToStringConverter();
            sb.AppendLine($"Тип гофры: {converter.Convert(_orderParameter.CorrugationType, typeof(CorrugationTypes), null, null)}");
            foreach (BlankSizes blankSizes in _calculationResult.BlanksSizes) {
                sb.AppendLine($"{blankSizes.BlankName} L1 = {blankSizes.LengthOne:F3} L2 = {blankSizes.LengthTwo:F3}");
            }
            sb.AppendLine();
            sb.AppendLine($"Площадь: {_calculationResult.ProductArea:F3}");
            sb.AppendLine($"Стоимость изделия: {_calculationResult.ProductPrice:F2}");
            sb.AppendLine($"Стоимость штампа: {_calculationResult.StampPrice:F2}");
            return sb.ToString();
        }
    }
}
