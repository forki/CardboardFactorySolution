﻿using CardboardFactory.WpfCore;
using Domain.Product;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class BlankSizesViewModel : ViewModelBase {
        private readonly Product.SheetSizes SheetSizes;

        public BlankSizesViewModel(Product.SheetSizes sheetSizes) {
            SheetSizes = sheetSizes;
        }

        public override string DisplayName => SheetSizes.Name;

        public double? LengthOne => SheetSizes?.LengthOne;

        public double? LengthTwo => SheetSizes?.LengthTwo;
    }
}
