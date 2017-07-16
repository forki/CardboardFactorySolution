using CardboardFactory.Domain.Product;
using CardboardFactory.WpfCore;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class BlankSizesViewModel : ViewModelBase {
        private readonly Product.SheetSizes BlankSizes;

        public BlankSizesViewModel(Product.SheetSizes blankSizes) {
            BlankSizes = blankSizes;
        }

        public override string DisplayName => BlankSizes.Name;

        public double? LengthOne => BlankSizes?.LengthOne;

        public double? LengthTwo => BlankSizes?.LengthTwo;
    }
}
