using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using CardboardFactory.Core.Product;
using CardboardFactory.Management.ViewModel;

namespace CardboardFactory.Management {
    public partial class App {
        private const string PRODUCT_TYPES_DATA_FILE = "ProductTypes.json";

        private readonly ProductTypesRepository _productTypesRepository;

        static App() {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        public App() {
            _productTypesRepository = new ProductTypesRepository(PRODUCT_TYPES_DATA_FILE);
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var window = new MainWindow();
            var viewModel = new MainWindowViewModel(_productTypesRepository);

            void RequestCloseHandler(object sender, EventArgs args) {
                viewModel.RequestClose -= RequestCloseHandler;
                window.Close();
            }

            viewModel.RequestClose += RequestCloseHandler;
            window.DataContext = viewModel;

            window.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e) {
            //_productTypesRepository.SaveProductTypes();
        }
    }
}
