using System;

namespace CardboardFactory.Core.EvaluationRules {
    public class Rule<T> {
        private readonly Predicate<T> RuleHandler;
        private readonly string ErrorMesage;
        private bool IsEvaluated;

        public string Error { get; private set; }
        public bool HasError => !string.IsNullOrEmpty(Error);

        public Rule(Predicate<T> ruleHandler, string errorMesage) {
            RuleHandler = ruleHandler ?? throw new ArgumentNullException(nameof(ruleHandler));
            ErrorMesage = errorMesage ?? throw new ArgumentNullException(nameof(errorMesage));
        }

        public void Evaluate(T value) {
            if (IsEvaluated) { return; }
            Error = null;
            try {
                if (!RuleHandler(value)) {
                    Error = ErrorMesage;
                }
            } catch (Exception e) {
                Error = e.Message;
            } finally {
                IsEvaluated = true;
            }
        }

        public void Reset() {
            IsEvaluated = false;
        }
    }
}
