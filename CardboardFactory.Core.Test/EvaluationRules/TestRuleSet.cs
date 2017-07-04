using CardboardFactory.Core.EvaluationRules;
using NUnit.Framework;

namespace CardboardFactory.Core.Test.EvaluationRules {
    [TestFixture]
    public class TestRuleSet {
        [Test]
        public void HasRulesTest() {
            var set = new RuleSet<int>();
            Assert.False(set.HasRules);
            set.Add(new Rule<int>(@int => @int == 10, "Error"));
            Assert.True(set.HasRules);
        }

        [TestCase(5, false, null)]
        [TestCase(0, true, "Error 1")]
        [TestCase(12, true, "Error 2")]
        public void EvaluateTest(int inputValue, bool expectedHasError, string expectedError) {
            var set = new RuleSet<int>();
            set.Add(new Rule<int>(@int => @int > 0, "Error 1"));
            set.Add(new Rule<int>(@int => @int <= 10, "Error 2"));
            set.Evaluate(inputValue);
            Assert.AreEqual(expectedHasError, set.HasError);
            if (expectedHasError) {
                Assert.That(set.Errors, Has.Exactly(1).EqualTo(expectedError));
            }
        }

        [Test]
        public void RuleSetRerunTest() {
            int calls = 0;
            var rule = new Rule<int>(number => {
                                         calls++;
                                         return number == 10;
                                     }, "Error");
            var set = new RuleSet<int>();
            set.Add(rule);
            set.Evaluate(10);
            Assert.False(set.HasError);
            Assert.AreEqual(1, calls);

            for (int i = 0; i < 100; i++) {
                set.Evaluate(15);
                Assert.False(set.HasError);
            }
            Assert.AreEqual(1, calls);

            set.Reset();
            set.Evaluate(15);
            Assert.True(set.HasError);
            Assert.AreEqual(2, calls);
        }
    }
}
