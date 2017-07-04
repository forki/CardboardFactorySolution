using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CardboardFactory.Core;
using CardboardFactory.Core.Product;
using CardboardFactory.Core.Tools;
using CardboardFactory.ProductPriceCalculation.Model;

namespace CardboardFactory.ProductPriceCalculation.ViewModel {
    public class OrderParameterViewModel : ViewModelBase, IDataErrorInfo {
        private readonly Dictionary<string, CorrugationTypes> EnumMap = new Dictionary<string, CorrugationTypes>();

        private OrderParameter Parameter;

        public OrderParameterViewModel(OrderParameter parameter) {
            Parameter = parameter;
            GetCorrugationTypes();
            Initialize(Parameter);
        }

        public override string DisplayName => null;

        public string[] CorrugationTypes => vCorrugationTypes ?? (vCorrugationTypes = EnumMap.Keys.ToArray());
        private string[] vCorrugationTypes;

        public string CorrugationType {
            get => vCorrugationType;
            set {
                if (vCorrugationType == value) { return; }
                vCorrugationType = value;
                Parameter.CorrugationType = EnumMap[vCorrugationType];
                OnPropertyChanged(nameof(Parameter.CorrugationType));
                OnPropertyChanged(nameof(CorrugationType));
            }
        }
        private string vCorrugationType;

        public double? CardboardPrice {
            get => Parameter.CardboardPrice;
            set {
                if (MathUtilities.AreEquals(Parameter.CardboardPrice, value)) { return; }
                Parameter.CardboardPrice = value;
                OnPropertyChanged(nameof(Parameter.CardboardPrice));
                OnPropertyChanged(nameof(CardboardPrice));
            }
        }

        public bool ShouldCalculateStampPrice {
            get => Parameter.ShouldCalculateStampPrice;
            set {
                if (Parameter.ShouldCalculateStampPrice == value) { return; }
                Parameter.ShouldCalculateStampPrice = value;
                OnPropertyChanged(nameof(Parameter.ShouldCalculateStampPrice));
                OnPropertyChanged(nameof(ShouldCalculateStampPrice));
                if (!ShouldCalculateStampPrice) {
                    PricePerKnifeMeter = default(double?);
                }
            }
        }

        public double? PricePerKnifeMeter {
            get => Parameter.PricePerKnifeMeter;
            set {
                if (MathUtilities.AreEquals(Parameter.PricePerKnifeMeter, value)) { return; }
                Parameter.PricePerKnifeMeter = value;
                OnPropertyChanged(nameof(Parameter.PricePerKnifeMeter));
                OnPropertyChanged(nameof(PricePerKnifeMeter));
            }
        }

        private void Initialize(OrderParameter parameter) {
            CorrugationType = CorrugationTypes.First();
            CardboardPrice = parameter.CardboardPrice;
            ShouldCalculateStampPrice = parameter.ShouldCalculateStampPrice;
            PricePerKnifeMeter = parameter.PricePerKnifeMeter;
        }

        private void GetCorrugationTypes() {
            Type type = typeof(CorrugationTypes);
            foreach (CorrugationTypes @enum in Enum.GetValues(type).OfType<CorrugationTypes>().Except(new[] { Core.Product.CorrugationTypes.All, })) {
                string name = type.Name + "_" + @enum;
                string enumText = Core.Properties.Resources.ResourceManager.GetString(name);
                if (enumText == null) { continue; }
                EnumMap[enumText] = @enum;
            }
        }

        string IDataErrorInfo.Error {
            get {
                string error = ValidateCardboardPrice();
                error = error ?? ValidatePricePerKnifeMeter();
                return error;
            }
        }

        string IDataErrorInfo.this[string propertyName] {
            get {
                switch (propertyName) {
                    case nameof(CardboardPrice):
                        return ValidateCardboardPrice();
                    case nameof(PricePerKnifeMeter):
                        return ValidatePricePerKnifeMeter();
                    default:
                        return null;
                }
            }
        }

        private string ValidateCardboardPrice() {
            if (CardboardPrice == null) { return "\"Расценка за м кв.:\" должна быть числом"; }
            if (CardboardPrice <= 0) { return "\"Расценка за м кв.:\" должна быть больше 0"; }
            return null;
        }

        private string ValidatePricePerKnifeMeter() {
            if (!ShouldCalculateStampPrice) { return null; }
            if (PricePerKnifeMeter == null) { return "\"Расценка за пог. м ножей\" должна быть числом"; }
            if (PricePerKnifeMeter <= 0) { return "\"Расценка за пог. м ножей\" должна быть больше 0"; }
            return null;
        }
    }
}
