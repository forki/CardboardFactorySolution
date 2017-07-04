using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Formatting = Newtonsoft.Json.Formatting;

namespace CardboardFactory.Core.Product {
    public class ProductTypesRepository {
        private const string BackUpDataFile = "Data/DefaultProductTypes.json";

        private readonly string StorageFileName;
        private Dictionary<string, ProductType> ProductTypes = new Dictionary<string, ProductType>();

        /// <summary>
        /// Load data from default resource file: Data/DefaultProductTypes.json
        /// </summary>
        public ProductTypesRepository() {
            ProductTypes = LoadCustomersFromBackUp();
        }

        public ProductTypesRepository(string productTypesDataFile) {
            StorageFileName = productTypesDataFile ?? throw new NullReferenceException(nameof(productTypesDataFile));
            LoadProductTypes();
        }

        public ProductType[] AllProductTypes => ProductTypes.Values.ToArray();

        public string[] AllProductTypeKeys => ProductTypes.Keys.ToArray();

        public ProductType GetProductType(string name) {
            ProductType prototype;
            if (ProductTypes.TryGetValue(name, out prototype)) { }
            return new ProductType(prototype ?? new ProductType());
        }

        public void SaveProductTypes() {
            try {
                var serializer = new JsonSerializer {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = { new StringEnumConverter(false) }
                };
                using (var sw = new StreamWriter(StorageFileName)) {
                    using (JsonWriter writer = new JsonTextWriter(sw)) {
                        writer.Formatting = Formatting.Indented;
                        serializer.Serialize(writer, AllProductTypes);
                    }
                }

            } catch (Exception e) {
                Console.WriteLine(@"error: " + e);
            }
        }

        private void LoadProductTypes() {
            var serializer = new JsonSerializer {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { new StringEnumConverter(false) }
            };
            try {
                using (var textReader = new StreamReader(StorageFileName)) {
                    var productTypes = serializer.Deserialize<ProductType[]>(new JsonTextReader(textReader));
                    if (productTypes == null) {
                        Debug.WriteLine("Failed to load data from file");
                        ProductTypes = LoadCustomersFromBackUp();
                        return;
                    }
                    foreach (ProductType productType in productTypes) {
                        ProductTypes.Add(productType.Name, productType);
                    }
                }
            } catch (Exception exception) when (exception is FileNotFoundException || exception is DirectoryNotFoundException) {
                Debug.WriteLine(exception);
                ProductTypes = LoadCustomersFromBackUp();
            }
        }

        private static Dictionary<string, ProductType> LoadCustomersFromBackUp() {
            var serializer = new JsonSerializer {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { new StringEnumConverter(false) }
            };
            using (Stream stream = GetResourceStream()) {
                TextReader tr = new StreamReader(stream, Encoding.Default);
                var productTypes = serializer.Deserialize<ProductType[]>(new JsonTextReader(tr));
                return productTypes?.ToDictionary(type => type.Name, type => type);
            }
        }

        private static Stream GetResourceStream() {
            var uri = new Uri(BackUpDataFile, UriKind.RelativeOrAbsolute);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            if (info?.Stream == null) {
                throw new ApplicationException("Missing resource file: " + BackUpDataFile);
            }
            return info.Stream;
        }
    }
}
