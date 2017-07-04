using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using CardboardFactory.Core;
using CardboardFactory.Core.Product;
using CardboardFactory.ProductPriceCalculation.Model;
using CardboardFactory.ProductPriceCalculation.ViewModel;
using Microsoft.Win32;

namespace CardboardFactory.ProductPriceCalculation {
    public class ProductPriceCalculationMainViewModel : WorkspaceViewModel, IDataErrorInfo {
        private readonly ProductTypesRepository _repository;

        private ProductType _productType;
        private readonly OrderParameter _orderParameter;
        private ProductCalculationResult _calculationResult;

        public ProductPriceCalculationMainViewModel(ProductTypesRepository repository) : this(repository, null, new OrderParameter(), new ProductCalculationResult()) { }

        public ProductPriceCalculationMainViewModel(
            ProductTypesRepository repository,
            ProductType productType,
            OrderParameter orderParameter,
            ProductCalculationResult calculationResult) {
            _repository = repository;
            _productType = productType;
            _orderParameter = orderParameter;
            _calculationResult = calculationResult;
        }

        public override string DisplayName => "Расчёт стоимости изделия";

        public string ProductType {
            get => vProductType;
            set {
                if (value == vProductType || string.IsNullOrEmpty(value)) { return; }
                vProductType = value;
                OnPropertyChanged(nameof(ProductType));
                SetNewProductType();
            }
        }
        private string vProductType;

        public string[] ProductTypeOptions => vProductTypeOptionsOptions ?? (vProductTypeOptionsOptions = _repository.AllProductTypeKeys);
        private string[] vProductTypeOptionsOptions;

        public ObservableCollection<ProductParameterViewModel> ProductParameters {
            get {
                if (_productType != null && vProductParameters == null) {
                    vProductParameters = new ObservableCollection<ProductParameterViewModel>();
                    foreach (KeyValuePair<string, ProductParameter> keyValuePair in _productType.Parameters) {
                        vProductParameters.Add(new ProductParameterViewModel(keyValuePair.Value));
                    }
                }
                return vProductParameters;
            }
        }
        private ObservableCollection<ProductParameterViewModel> vProductParameters;

        public OrderParameterViewModel OrderParameter {
            get {
                if (_orderParameter != null && vOrderParameter == null) {
                    vOrderParameter = new OrderParameterViewModel(_orderParameter);
                }
                return vOrderParameter;
            }
        }
        private OrderParameterViewModel vOrderParameter;

        public CalculationResultsViewModel CalculationResult {
            get {
                if (_calculationResult != null && vCalculationResult == null) {
                    vCalculationResult = new CalculationResultsViewModel(_calculationResult);
                }
                return vCalculationResult;
            }
        }
        private CalculationResultsViewModel vCalculationResult;

        public ICommand CalculateProductCommand => vCalculateProductCommand ?? (vCalculateProductCommand =
            new RelayCommand(CalculateProductExecuteHandler, CalculateProductCanExecuteHandler));
        private RelayCommand vCalculateProductCommand;

        public ICommand SaveCalculatedProductCommand => vSaveCalculatedProductCommand ?? (vSaveCalculatedProductCommand =
            new RelayCommand(SaveCalculatedProductExecuteHandler, SaveCalculatedProductCanExecuteHandler));
        private RelayCommand vSaveCalculatedProductCommand;

        private void SetNewProductType() {
            ProductType newProductType = _repository.GetProductType(ProductType);
            if (_productType != null) {
                newProductType.SetParametersFromOther(_productType);
            }
            _productType = newProductType;
            vProductParameters = null;
            OnPropertyChanged(nameof(ProductParameters));
        }

        private void CalculateProductExecuteHandler(object o1) {
            var calculator = new ProductPriceCalculator(_productType, _orderParameter);
            _calculationResult = calculator.Calculate();
            vCalculationResult = null;
            OnPropertyChanged(nameof(CalculationResult));
        }

        private bool CalculateProductCanExecuteHandler(object o1) {
            return ((IDataErrorInfo)this).Error == null;
        }

        private void SaveCalculatedProductExecuteHandler(object o1) {
            var saveFileDialog = new SaveFileDialog {
                Filter = "Text documents (.txt)|*.txt",
                DefaultExt = ".txt",
                FileName = $"{_productType.Name}-{DateTime.Now:dd/MM/yyyy-hh.mm.ss}"
            };
            if (saveFileDialog.ShowDialog() == true) {
                SaveBox(saveFileDialog.FileName);
            }
        }

        private bool SaveCalculatedProductCanExecuteHandler(object o1) {
            return _calculationResult != null && _calculationResult.IsValid;
        }

        private void SaveBox(string fileName) {
            var file = new FileInfo(fileName);
            StreamWriter writer = file.CreateText();
            var formatter = new FormatProductPriceCalculation(_productType, _orderParameter, _calculationResult);
            string formatted = formatter.Format();
            writer.Write(formatted);
            writer.Close();
        }

        string IDataErrorInfo.Error {
            get {
                string error = (OrderParameter as IDataErrorInfo)?.Error;
                if (error == null && ProductParameters != null) {
                    foreach (ProductParameterViewModel parameterViewModel in ProductParameters) {
                        error = ((IDataErrorInfo)parameterViewModel).Error;
                        if (error != null) { break; }
                    }
                }
                return error;
            }
        }

        string IDataErrorInfo.this[string propertyName] {
            get {
                string error = (OrderParameter as IDataErrorInfo)?[propertyName];
                if (error == null && ProductParameters != null) {
                    foreach (ProductParameterViewModel parameterViewModel in ProductParameters) {
                        error = ((IDataErrorInfo)parameterViewModel)[propertyName];
                        if (error != null) { break; }
                    }
                }
                return error;
            }
        }
    }
}
