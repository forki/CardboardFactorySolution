using System.ComponentModel;
using CardboardFactory.Core.Product;
using CardboardFactory.Core.Tools;
using CardboardFactory.WpfCore;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class ProductParameterViewModel : ViewModelBase, IDataErrorInfo {
        private readonly ProductParameter Parameter;

        public ProductParameterViewModel(ProductParameter parameter) {
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
                    Parameter.Value = vValue.Value / 1000.0;
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

        private void Initialize(ProductParameter parameter) {
            Value = parameter.Value * 1000;
        }
    }
}
