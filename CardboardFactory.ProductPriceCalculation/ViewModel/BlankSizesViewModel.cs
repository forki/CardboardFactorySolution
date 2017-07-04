using CardboardFactory.Core;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class BlankSizesViewModel : ViewModelBase {
        private readonly BlankSizes BlankSizes;

        public BlankSizesViewModel(BlankSizes blankSizes) {
            BlankSizes = blankSizes;
        }

        public override string DisplayName => BlankSizes.BlankName;

        public double? LengthOne => BlankSizes?.LengthOne;

        public double? LengthTwo => BlankSizes?.LengthTwo;
    }
}
