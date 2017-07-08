using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using CardboardFactory.DataAccess.Product;
using CardboardFactory.OrderCreation;
using CardboardFactory.ProductPriceCalculation;
using CardboardFactory.WpfCore;

namespace CardboardFactory.Management.ViewModel {
    public class MainWindowViewModel : WorkspaceViewModel {
        private readonly ProductTypesRepository _productTypesRepository;

        public override string DisplayName => Properties.Resources.MainWindowViewModel_DisplayName;

        public IEnumerable<CommandViewModel> Commands => vCommands ?? (vCommands = new ReadOnlyCollection<CommandViewModel>(new List<CommandViewModel>(CreateCommands())));
        private ReadOnlyCollection<CommandViewModel> vCommands;

        public ObservableCollection<WorkspaceViewModel> Workspaces {
            get {
                if (vWorkspaces == null) {
                    vWorkspaces = new ObservableCollection<WorkspaceViewModel>();
                    vWorkspaces.CollectionChanged += OnWorkspacesChanged;
                }
                return vWorkspaces;
            }
        }

        private ObservableCollection<WorkspaceViewModel> vWorkspaces;

        public MainWindowViewModel(ProductTypesRepository productTypesRepository) {
            _productTypesRepository = productTypesRepository;
        }

        private IEnumerable<CommandViewModel> CreateCommands() {
            yield return new CommandViewModel(
                Properties.Resources.MainWindowViewModel_Command_ProductPriceCalculation,
                new RelayCommand(param => ShowProductPriceCalculation(), o => true));
            yield return new CommandViewModel(
                Properties.Resources.MainWindowViewModel_Command_OrderCreation,
                new RelayCommand(param => ShowOrderCreation(), o => true));
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= OnWorkspaceRequestClose;
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e) {
            var workspace = sender as WorkspaceViewModel;
            workspace?.Dispose();
            Workspaces.Remove(workspace);
        }

        private void ShowProductPriceCalculation() {
            WorkspaceViewModel workspace = Workspaces.FirstOrDefault(vm => vm is ProductPriceCalculationMainViewModel);
            if (workspace == null) {
                workspace = new ProductPriceCalculationMainViewModel(_productTypesRepository);
                Workspaces.Add(workspace);
            }
            SetActiveWorkspace(workspace);
        }

        private void ShowOrderCreation() {
            WorkspaceViewModel workspace = Workspaces.FirstOrDefault(vm => vm is OrderCreationMainViewModel);
            if (workspace == null) {
                workspace = new OrderCreationMainViewModel();
                Workspaces.Add(workspace);
            }
            SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace) {
            Debug.Assert(Workspaces.Contains(workspace));
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            collectionView?.MoveCurrentTo(workspace);
        }
    }
}
