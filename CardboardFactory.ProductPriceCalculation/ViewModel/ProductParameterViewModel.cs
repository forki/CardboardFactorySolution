using System.ComponentModel;
using CardboardFactory.Core.Tools;
using CardboardFactory.Domain.Product;
using CardboardFactory.WpfCore;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class ProductParameterViewModel : ViewModelBase, IDataErrorInfo {
        private Product.ProductParameter Parameter;

        public ProductParameterViewModel(Product.ProductParameter parameter) {
            Parameter = parameter;
            Initialize(Parameter);
        }

        public override string DisplayName => Parameter.Name;

        public double? Value {
            get => vValue;
            set {
                if (MathUtilities.AreEquals(vValue, value)) { return; }
                vValue = value;
                OnPropertyChanged(nameof(Value));
                if (vValue.HasValue) {
                    Parameter = new Product.ProductParameter(Parameter.Name, vValue.Value / 1000.0);
                }
            }
        }
        private double? vValue;

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

        private void Initialize(Product.ProductParameter parameter) {
            Value = parameter.Value * 1000;
        }
    }
}
