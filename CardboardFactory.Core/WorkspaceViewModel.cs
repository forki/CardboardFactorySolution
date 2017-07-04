using System;
using System.Windows.Input;

namespace CardboardFactory.Core {
    public abstract class WorkspaceViewModel : ViewModelBase {
        public ICommand CloseCommand {
            get {
                return vCloseCommand ?? (vCloseCommand = new RelayCommand(param => OnRequestClose()));
            }
        }
        private RelayCommand vCloseCommand;

        public event EventHandler RequestClose;

        private void OnRequestClose() {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
