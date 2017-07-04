using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardboardFactory.Core.EvaluationRules {
    public class EvaluationRuleLibrary<T> {
        private readonly Dictionary<string, RuleSet<T>> Library = new Dictionary<string, RuleSet<T>>();

        public void AddRule(string propertyName, Rule<T> rule) {
            if (!Library.ContainsKey(propertyName)) {
                Library[propertyName] = new RuleSet<T>();
            }
            Library[propertyName].Add(rule);
        }

        public void RemoveRule(string propertyName, Rule<T> rule) {
            if (!Library.ContainsKey(propertyName)) { return; }
            Library[propertyName].Remove(rule);
            if (!Library[propertyName].HasRules) {
                Library.Remove(propertyName);
            }
        }

        public bool EvaluateRules(string propertyName, T value) {
            if (!Library.TryGetValue(propertyName, out RuleSet<T> set)) { return true; }
            set.Evaluate(value);
            return !set.HasError;
        }

        public void ResetRules(string propertyName) {
            if (!Library.TryGetValue(propertyName, out RuleSet<T> set)) { return; }
            set.Reset();
        }

        public string GetErrors(string propertyName) {
            if (!Library.TryGetValue(propertyName, out RuleSet<T> set)) { return null; }
            if (!set.HasError) { return null; }
            HashSet<string> errors = set.Errors;
            if (errors.Count == 1) { return errors.First(); }
            var sb = new StringBuilder();
            foreach (string error in errors) {
                sb.AppendLine(error);
            }
            return sb.ToString();
        }

        public string GetAllErrors() {
            var sb = new StringBuilder();
            foreach (string property in Library.Keys) {
                string errors = GetErrors(property);
                if (string.IsNullOrEmpty(errors)) { continue; }
                sb.AppendLine();
            }
            return sb.Length == 0 ? null : sb.ToString();
        }
    }
}
