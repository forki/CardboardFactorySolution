using System;
using System.Windows.Input;

namespace CardboardFactory.WpfCore {
    public class CommandViewModel : ViewModelBase {
        public override string DisplayName { get; }

        public ICommand Command { get; }

        public CommandViewModel(string displayName, ICommand command) {
            DisplayName = displayName;
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }
    }
}
