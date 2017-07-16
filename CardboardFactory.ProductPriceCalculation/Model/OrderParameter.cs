using CardboardFactory.Domain.Product;

namespace CardboardFactory.ProductPriceCalculation.Model {
    public class OrderParameter {
        public Product.CorrugationTypes CorrugationType { get; set; }
        public double? CardboardPrice { get; set; }
        public bool ShouldCalculateStampPrice { get; set; }
        public double? PricePerKnifeMeter { get; set; }

        public OrderParameter() {
            CorrugationType = Product.CorrugationTypes.C;
        }
    }
}
