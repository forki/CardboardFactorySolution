using System.ComponentModel;
using CardboardFactory.Core.Tools;
using CardboardFactory.WpfCore;
using Domain.Product;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class ProductParameterViewModel : ViewModelBase, IDataErrorInfo {
        public ProductParameterViewModel(Product.ProductParameter parameter) {
            DisplayName = parameter.Name;
            Value = parameter.Value * 1000;
        }

        public override string DisplayName { get; }

        public double? Value {
            get => vValue;
            set {
                if (MathUtilities.AreEquals(vValue, value)) { return; }
                vValue = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        private double? vValue;

        public Product.ProductParameter GetDomainType() {
            return new Product.ProductParameter(DisplayName, Value / 1000.0);
        }

        string IDataErrorInfo.Error => ValidateValue();

        string IDataErrorInfo.this[string propertyName] {
            get {
                switch (propertyName) {
                    case nameof(Value):
                        return ValidateValue();
                    default:
                        return null;
                }
            }
        }

        private string ValidateValue() {
            if (Value == null) { return $"{DisplayName} должна быть числом"; }
            if (Value < 0) { return $"{DisplayName} должна быть больше 0"; }
            return null;
        }
    }
}
