using System.Collections.ObjectModel;
using CardboardFactory.ProductPriceCalculation.Model;
using CardboardFactory.WpfCore;
using Domain.Product;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class CalculationResultsViewModel : ViewModelBase {
        private readonly ProductCalculationResult Result;

        public CalculationResultsViewModel(ProductCalculationResult result) {
            Result = result;
        }

        public override string DisplayName => null;

        public ObservableCollection<SheetSizesViewModel> SteetSizes {
            get {
                if (Result != null && vSteetSizes == null) {
                    vSteetSizes = new ObservableCollection<SheetSizesViewModel>();
                    foreach (Product.SheetSizes sheetSizes in Result.SheetsSizes) {
                        vSteetSizes.Add(new SheetSizesViewModel(sheetSizes));
                    }
                    return vSteetSizes;
                }
                return vSteetSizes;
            }
        }
        private ObservableCollection<SheetSizesViewModel> vSteetSizes;

        public double? ProductArea => Result?.ProductArea;

        public double? ProductPrice => Result?.ProductPrice;

        public bool DoShowStampPrice => StampPrice != null && StampPrice > 0.0;

        public double? StampPrice => Result?.StampPrice;

        public bool DoShowError => !string.IsNullOrEmpty(Result.ErrorMessage);

        public string Error => Result.ErrorMessage;
    }
}
