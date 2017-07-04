using System.Collections.Generic;
using System.Linq;

namespace CardboardFactory.Core.EvaluationRules {
    public class RuleSet<T> {
        private HashSet<Rule<T>> Rules = new HashSet<Rule<T>>();
        private bool IsEvaluated;

        public bool HasRules => Rules.Count != 0;
        public bool HasError => Rules.Any(rule => rule.HasError);
        public HashSet<string> Errors => new HashSet<string>(Rules.Where(rule => rule.HasError).Select(rule => rule.Error));

        public void Add(Rule<T> rule) {
            IsEvaluated = false;
            Rules.Add(rule);
        }

        public void Remove(Rule<T> rule) {
            IsEvaluated = false;
            Rules.Remove(rule);
        }

        public void Evaluate(T value) {
            if (IsEvaluated) { return; }
            foreach (Rule<T> rule in Rules) {
                rule.Evaluate(value);
            }
            IsEvaluated = true;
        }

        public void Reset() {
            foreach (Rule<T> rule in Rules) {
                rule.Reset();
            }
            IsEvaluated = false;
        }
    }
}
