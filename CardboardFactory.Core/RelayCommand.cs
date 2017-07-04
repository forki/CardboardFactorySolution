using System;
using System.Diagnostics;
using System.Windows.Input;

namespace CardboardFactory.Core {
    public class RelayCommand : ICommand {
        private readonly Action<object> ExecuteHandler;
        private readonly Predicate<object> CanExecuteHandler;

        public RelayCommand(Action<object> executeHandler, Predicate<object> canExecuteHandler = null) {
            ExecuteHandler = executeHandler ?? throw new ArgumentNullException(nameof(executeHandler));
            CanExecuteHandler = canExecuteHandler;
        }

        public void Execute(object parameter) {
            ExecuteHandler?.Invoke(parameter);
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter) {
            return CanExecuteHandler?.Invoke(parameter) ?? true;
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
