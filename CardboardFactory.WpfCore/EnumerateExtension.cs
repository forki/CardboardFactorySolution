using System;
using System.Windows.Markup;
using CardboardFactory.Core.Properties;

namespace CardboardFactory.WpfCore {
    public sealed class EnumerateExtension : MarkupExtension {
        public Type Type { get; set; }

        public EnumerateExtension(Type type) {
            Type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            string[] names = Enum.GetNames(Type);
            var values = new string[names.Length];

            for (int i = 0; i < names.Length; i++) {
                string name = Type.Name + "_" + names[i];
                values[i] = Resources.ResourceManager.GetString(name);
            }

            return values;
        }
    }
}
