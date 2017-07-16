
namespace CardboardFactory.Core.Product {
    public class LengthFormula {
        public CorrugationTypes CorrugationType { get; set; } = CorrugationTypes.All;

        public string FormulaText { get; set; } = "Formula";

        public LengthFormula(CorrugationTypes corrugationType, string formulaText) {
            CorrugationType = corrugationType;
            FormulaText = formulaText;
        }

        public LengthFormula() { }
    }
}
