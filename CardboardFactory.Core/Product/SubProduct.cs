using System.Collections.Generic;
using System.Linq;
using Domain.Core.Cardboard;

namespace CardboardFactory.Core.Product {
    public class SubProduct {
        public string Name { get; set; }
        public Dictionary<CorrugationTypes.Enum, LengthFormula> LengthOneFormulas { get; }
        public Dictionary<CorrugationTypes.Enum, LengthFormula> LengthTwoFormulas { get; }

        public SubProduct() : this((string)null) { }

        public SubProduct(string name) {
            LengthOneFormulas = new Dictionary<CorrugationTypes.Enum, LengthFormula>();
            LengthTwoFormulas = new Dictionary<CorrugationTypes.Enum, LengthFormula>();
            Name = name ?? "Заготовка";
        }

        public SubProduct(SubProduct other) {
            LengthOneFormulas = new Dictionary<CorrugationTypes.Enum, LengthFormula>(other.LengthOneFormulas.Values
                                                                                     .ToDictionary(formula => formula.CorrugationType, formula => new LengthFormula {
                                                                                         CorrugationType = formula.CorrugationType,
                                                                                         FormulaText = formula.FormulaText
                                                                                     }));
            LengthTwoFormulas = new Dictionary<CorrugationTypes.Enum, LengthFormula>(other.LengthTwoFormulas.Values
                                                                                     .ToDictionary(formula => formula.CorrugationType, formula => new LengthFormula {
                                                                                         CorrugationType = formula.CorrugationType,
                                                                                         FormulaText = formula.FormulaText
                                                                                     }));
            Name = other.Name;
        }
    }
}
