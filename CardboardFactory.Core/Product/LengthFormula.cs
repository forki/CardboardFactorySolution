using Domain.Core.Cardboard;

namespace CardboardFactory.Core.Product {
    public class LengthFormula {
        public CorrugationTypes.Enum CorrugationType { get; set; } = CorrugationTypes.Enum.All;

        public string FormulaText { get; set; } = "Formula";

        public LengthFormula(CorrugationTypes.Enum corrugationType, string formulaText) {
            CorrugationType = corrugationType;
            FormulaText = formulaText;
        }

        public LengthFormula() { }
    }
}
