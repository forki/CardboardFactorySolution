using System.ComponentModel;
using CardboardFactory.Core;
using CardboardFactory.Core.Product;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class ProductParameterViewModel : ViewModelBase, IDataErrorInfo {
        private readonly ProductParameter Parameter;

        public ProductParameterViewModel(ProductParameter parameter) {
            Parameter = parameter;
            Initialize(Parameter);
        }

        public override string DisplayName => Parameter.Name;

        public int? Value {
            get => vValue;
            set {
                if (vValue == value) { return; }
                vValue = value;
                OnPropertyChanged(nameof(Value));
                if (vValue.HasValue) {
                    Parameter.Value = vValue.Value / 1000.0;
                }
            }
        }
        private int? vValue;

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
            if (Value <= 0) { return $"{DisplayName} должна быть больше 0"; }
            return null;
        }

        private void Initialize(ProductParameter parameter) {
            Value = (int)(parameter.Value * 1000);
        }
    }
}
