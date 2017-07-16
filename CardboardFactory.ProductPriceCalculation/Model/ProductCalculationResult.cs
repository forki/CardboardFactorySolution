using System.Collections.Generic;
using System.Linq;
using CardboardFactory.Core;
using CardboardFactory.Core.Tools;
using CardboardFactory.Domain.Product;

namespace CardboardFactory.ProductPriceCalculation.Model {
    public class ProductCalculationResult {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; }
        public List<Product.SheetSizes> BlanksSizes { get; set; }
        public double? ProductArea => (BlanksSizes.Sum(sizes => sizes.Area)).Round(3);
        public double? ProductPrice { get; set; }
        public double? StampPrice { get; set; }

        public ProductCalculationResult() {
            BlanksSizes = new List<Product.SheetSizes>();
        }

        private ProductCalculationResult(string error) {
            BlanksSizes = new List<Product.SheetSizes>();
            ErrorMessage = error;
        }

        public static ProductCalculationResult GetCalculationErrorResult(string error) {
            return new ProductCalculationResult(error);
        }
    }
}
