using System.Collections.ObjectModel;
using CardboardFactory.Core;
using CardboardFactory.ProductPriceCalculation.Model;
using CardboardFactory.WpfCore;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class CalculationResultsViewModel : ViewModelBase {
        private readonly ProductCalculationResult Result;

        public CalculationResultsViewModel(ProductCalculationResult result) {
            Result = result;
        }

        public override string DisplayName => null;

        public ObservableCollection<BlankSizesViewModel> BlanksSizes {
            get {
                if (Result != null && vBlanksSizes == null) {
                    vBlanksSizes = new ObservableCollection<BlankSizesViewModel>();
                    foreach (BlankSizes blankSizes in Result.BlanksSizes) {
                        vBlanksSizes.Add(new BlankSizesViewModel(blankSizes));
                    }
                    return vBlanksSizes;
                }
                return vBlanksSizes;
            }
        }
        private ObservableCollection<BlankSizesViewModel> vBlanksSizes;

        public double? ProductArea => Result?.ProductArea;

        public double? ProductPrice => Result?.ProductPrice;

        public bool DoShowStampPrice => StampPrice != null && StampPrice > 0.0;

        public double? StampPrice => Result?.StampPrice;

        public bool DoShowError => !string.IsNullOrEmpty(Result.ErrorMessage);

        public string Error => Result.ErrorMessage;
    }
}
