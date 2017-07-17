using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CardboardFactory.WpfCore;
using Domain.Core.Cardboard;

namespace CardboardFactory.OrderCreation {
    public class OrderCreationMainViewModel : WorkspaceViewModel, IDataErrorInfo {
        private IDictionary<string, CorrugationTypes.Enum> CorrugationTypesEnumMap;
        private IDictionary<string, CardboardBrands.Enum> CardboardBrandsEnumMap;
        private IDictionary<string, CardboardClasses.Enum> CardboardClassesEnumMap;
        private IDictionary<string, CardboardColors.Enum> CardboardColorsEnumMap;

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

        public string[] AllCorrugationTypes => vCorrugationTypes ?? (vCorrugationTypes = CorrugationTypesEnumMap.Keys.ToArray());
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

        public string[] AllCardboardBrands => vCardboardBrands ?? (vCardboardBrands = CardboardBrandsEnumMap.Keys.ToArray());
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

        public string[] AllCardboardClasses => vCardboardClasses ?? (vCardboardClasses = CardboardClassesEnumMap.Keys.ToArray());
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

        public string[] AllCardboardColors => vCardboardColors ?? (vCardboardColors = CardboardColorsEnumMap.Keys.ToArray());
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
            Type type = typeof(CorrugationTypes.Enum);
            CorrugationTypesEnumMap = CorrugationTypes.stringToEnumMap(Enum.GetValues(type)
                .OfType<CorrugationTypes.Enum>()
                .Except(new[] { CorrugationTypes.Enum.All, CorrugationTypes.Enum.Polygraphy, CorrugationTypes.Enum.AllWithoutPolygraphy })
                .ToArray());
            CorrugationType = AllCorrugationTypes.First();
        }

        private void GetCardboardBrands() {
            Type type = typeof(CardboardBrands.Enum);
            CardboardBrandsEnumMap = CardboardBrands.stringToEnumMap(Enum.GetValues(type).OfType<CardboardBrands.Enum>().ToArray());
            CardboardBrand = AllCardboardBrands.First();
        }

        private void GetCardboardClasses() {
            Type type = typeof(CardboardClasses.Enum);
            CardboardClassesEnumMap = CardboardClasses.stringToEnumMap(Enum.GetValues(type).OfType<CardboardClasses.Enum>().ToArray());
            CardboardClass = AllCardboardClasses.First();
        }

        private void GetCardboardColors() {
            Type type = typeof(CardboardColors.Enum);
            CardboardColorsEnumMap = CardboardColors.stringToEnumMap(Enum.GetValues(type).OfType<CardboardColors.Enum>().ToArray());
            CardboardColor = AllCardboardColors.First();
        }

        public string this[string columnName] => null;
        public string Error => null;
    }
}
