using Domain.Core.Cardboard;

namespace CardboardFactory.ProductPriceCalculation.Model {
    public class OrderParameter {
        public CorrugationTypes.Enum CorrugationType { get; set; }
        public double? CardboardPrice { get; set; }
        public bool ShouldCalculateStampPrice { get; set; }
        public double? PricePerKnifeMeter { get; set; }

        public OrderParameter() {
            CorrugationType = CorrugationTypes.Enum.C;
        }
    }
}
