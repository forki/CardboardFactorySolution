using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CardboardFactory.Core.Order;
using CardboardFactory.Core.Product;
using CardboardFactory.WpfCore;

namespace CardboardFactory.OrderCreation {
    public class OrderCreationMainViewModel : WorkspaceViewModel, IDataErrorInfo {
        private readonly Dictionary<string, CorrugationTypes> CorrugationTypesEnumMap = new Dictionary<string, CorrugationTypes>();
        private readonly Dictionary<string, CardboardBrands> CardboardBrandsEnumMap = new Dictionary<string, CardboardBrands>();
        private readonly Dictionary<string, CardboardClasses> CardboardClassesEnumMap = new Dictionary<string, CardboardClasses>();
        private readonly Dictionary<string, CardboardColors> CardboardColorsEnumMap = new Dictionary<string, CardboardColors>();

        public override string DisplayName => "Оформление заказа на линию";

        public OrderCreationMainViewModel() {
            OrderDateTime = DateTime.Today;
            MinimumOrderDateTime = OrderDateTime.AddMonths(-1);
            MaximunOrderDateTime = OrderDateTime.AddMonths(5);

            InitializeEnumMaps();
        }

        public DateTime MinimumOrderDateTime { get; }
        public DateTime MaximunOrderDateTime { get; }

        public DateTime OrderDateTime {
            get => vOrderDateTime;
            set {
                if (value == vOrderDateTime) { return; }
                vOrderDateTime = value;
                OnPropertyChanged(nameof(OrderDateTime));
            }
        }
        private DateTime vOrderDateTime;

        public string[] CorrugationTypes => vCorrugationTypes ?? (vCorrugationTypes = CorrugationTypesEnumMap.Keys.ToArray());
        private string[] vCorrugationTypes;

        public string CorrugationType {
            get => vCorrugationType;
            set {
                if (vCorrugationType == value) { return; }
                vCorrugationType = value;
                OnPropertyChanged(nameof(CorrugationType));
            }
        }
        private string vCorrugationType;

        public string[] CardboardBrands => vCardboardBrands ?? (vCardboardBrands = CardboardBrandsEnumMap.Keys.ToArray());
        private string[] vCardboardBrands;

        public string CardboardBrand {
            get => vCardboardBrand;
            set {
                if (vCardboardBrand == value) { return; }
                vCardboardBrand = value;
                OnPropertyChanged(nameof(CardboardBrand));
            }
        }
        private string vCardboardBrand;

        public string[] CardboardClasses => vCardboardClasses ?? (vCardboardClasses = CardboardClassesEnumMap.Keys.ToArray());
        private string[] vCardboardClasses;

        public string CardboardClass {
            get => vCardboardClass;
            set {
                if (vCardboardClass == value) { return; }
                vCardboardClass = value;
                OnPropertyChanged(nameof(CardboardClass));
            }
        }
        private string vCardboardClass;

        public string[] CardboardColors => vCardboardColors ?? (vCardboardColors = CardboardColorsEnumMap.Keys.ToArray());
        private string[] vCardboardColors;

        public string CardboardColor {
            get => vCardboardColor;
            set {
                if (vCardboardColor == value) { return; }
                vCardboardColor = value;
                OnPropertyChanged(nameof(CardboardColor));
            }
        }
        private string vCardboardColor;

        private void InitializeEnumMaps() {
            GetCorrugationTypes();
            GetCardboardBrands();
            GetCardboardClasses();
            GetCardboardColors();
        }

        private void GetCorrugationTypes() {
            Type type = typeof(CorrugationTypes);
            foreach (CorrugationTypes @enum in Enum.GetValues(type).OfType<CorrugationTypes>().Except(new[] {
                Core.Product.CorrugationTypes.All, Core.Product.CorrugationTypes.Polygraphy
            })) {
                string name = type.Name + "_" + @enum;
                string enumText = Core.Properties.Resources.ResourceManager.GetString(name);
                if (enumText == null) { continue; }
                CorrugationTypesEnumMap[enumText] = @enum;
            }
            CorrugationType = CorrugationTypes.First();
        }

        private void GetCardboardBrands() {
            Type type = typeof(CardboardBrands);
            foreach (CardboardBrands @enum in Enum.GetValues(type).OfType<CardboardBrands>()) {
                string name = type.Name + "_" + @enum;
                string enumText = Core.Properties.Resources.ResourceManager.GetString(name);
                if (enumText == null) { continue; }
                CardboardBrandsEnumMap[enumText] = @enum;
            }
            CardboardBrand = CardboardBrands.First();
        }

        private void GetCardboardClasses() {
            Type type = typeof(CardboardClasses);
            foreach (CardboardClasses @enum in Enum.GetValues(type).OfType<CardboardClasses>()) {
                string name = type.Name + "_" + @enum;
                string enumText = Core.Properties.Resources.ResourceManager.GetString(name);
                if (enumText == null) { continue; }
                CardboardClassesEnumMap[enumText] = @enum;
            }
            CardboardClass = CardboardClasses.First();
        }

        private void GetCardboardColors() {
            Type type = typeof(CardboardColors);
            foreach (CardboardColors @enum in Enum.GetValues(type).OfType<CardboardColors>()) {
                string name = type.Name + "_" + @enum;
                string enumText = Core.Properties.Resources.ResourceManager.GetString(name);
                if (enumText == null) { continue; }
                CardboardColorsEnumMap[enumText] = @enum;
            }
            CardboardColor = CardboardColors.First();
        }

        public string this[string columnName] => null;
        public string Error => null;
    }
}
