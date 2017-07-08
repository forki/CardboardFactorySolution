using System;
using System.Globalization;
using System.Windows.Data;
using CardboardFactory.Core.Properties;

namespace CardboardFactory.WpfCore.Tools {
    public sealed class EnumToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? null : Resources.ResourceManager.GetString(targetType.Name + "_" + value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is string str) {
                foreach (object enumValue in Enum.GetValues(targetType)) {
                    if (str == Resources.ResourceManager.GetString(targetType.Name + "_" + enumValue)) { return enumValue; }
                }
            }
            throw new ArgumentException(null, nameof(value));
        }
    }
}
