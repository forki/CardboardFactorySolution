using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CardboardFactory.DataAccess.Product {
    public class ProductTypesRepository {
        private const string BackUpDataFile = "Data/DefaultProductTypes.json";

        private readonly string StorageFileName;
        private Dictionary<string, Domain.Product.Product.ProductType> ProductTypes = new Dictionary<string, Domain.Product.Product.ProductType>();

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

        public Domain.Product.Product.ProductType[] AllProductTypes => ProductTypes.Values.ToArray();

        public string[] AllProductTypeKeys => ProductTypes.Keys.ToArray();

        public Domain.Product.Product.ProductType GetProductType(string name) {
            Domain.Product.Product.ProductType prototype;
            if (ProductTypes.TryGetValue(name, out prototype)) { }
            return prototype;
        }

        public void SaveProductTypes() {
            try {
                var serializer = new JsonSerializer {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = {
                        new StringEnumConverter(false)
                    }
                };
                using (var sw = new StreamWriter(StorageFileName, false, Encoding.Default)) {
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
                Converters = {
                    new StringEnumConverter(false)
                }
            };
            try {
                using (var textReader = new StreamReader(StorageFileName, Encoding.Default)) {
                    var productTypes = serializer.Deserialize<Domain.Product.Product.ProductType[]>(new JsonTextReader(textReader));
                    if (productTypes == null) {
                        ProductTypes = LoadCustomersFromBackUp();
                        return;
                    }
                    foreach (Domain.Product.Product.ProductType productType in productTypes) {
                        ProductTypes.Add(productType.Name, productType);
                    }
                }
            } catch (Exception exception) when (exception is FileNotFoundException || exception is DirectoryNotFoundException) {
                ProductTypes = LoadCustomersFromBackUp();
            }
        }

        private static Dictionary<string, Domain.Product.Product.ProductType> LoadCustomersFromBackUp() {
            var serializer = new JsonSerializer {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = {
                    new StringEnumConverter(false)
                }
            };
            using (var textReader = new StreamReader(BackUpDataFile, Encoding.Default)) {
                var productTypes = serializer.Deserialize<Domain.Product.Product.ProductType[]>(new JsonTextReader(textReader));
                return productTypes?.ToDictionary(type => type.Name, type => type);
            }
        }
    }
}
