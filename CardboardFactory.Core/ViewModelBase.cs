using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CardboardFactory.Core {
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable {
        private readonly bool ThrowOnInvalidPropertyName = false;

        public abstract string DisplayName { get; }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName) {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null) {
                string msg = "Invalid property name: " + propertyName;
                if (ThrowOnInvalidPropertyName) {
                    throw new Exception(msg);
                }
                Debug.Fail(msg);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose() {
            OnDispose();
        }

        protected virtual void OnDispose() { }

#if DEBUG
        /// <summary>
        /// Useful for ensuring that ViewModel objects are properly garbage collected.
        /// </summary>
        ~ViewModelBase() {
            string msg = $"{GetType().Name} ({DisplayName}) ({GetHashCode()}) Finalized";
            Debug.WriteLine(msg);
        }
#endif
    }
}
