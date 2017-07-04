using System;
using CardboardFactory.Core.EvaluationRules;
using NUnit.Framework;

namespace CardboardFactory.Core.Test.EvaluationRules {
    [TestFixture]
    public class TestRule {
        [Test]
        public void RuleThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new Rule<object>(null, "Error"));
            Assert.Throws<ArgumentNullException>(() => new Rule<object>(@object => @object != null, null));
        }

        [TestCase(5, 10)]
        [TestCase(5.5, 6.0)]
        [TestCase("Expected", "Not expected")]
        public void RuleEvaluateTest<T>(T expected, T notExpected) {
            var rule = new Rule<T>(obj => Equals(obj, expected), "Error");
            rule.Evaluate(notExpected);
            Assert.True(rule.HasError);
            Assert.AreEqual("Error", rule.Error);
            rule.Reset();
            rule.Evaluate(expected);
            Assert.False(rule.HasError);
            Assert.Null(rule.Error);

        }

        [Test]
        public void RuleRerunTest() {
            int calls = 0;
            var rule = new Rule<int>(number => {
                                         calls++;
                                         return number == 10;
                                     }, "Error");
            rule.Evaluate(10);
            Assert.False(rule.HasError);
            Assert.Null(rule.Error);
            Assert.AreEqual(1, calls);

            for (int i = 0; i < 100; i++) {
                rule.Evaluate(15);
                Assert.False(rule.HasError);
                Assert.Null(rule.Error);
            }
            Assert.AreEqual(1, calls);

            rule.Reset();
            rule.Evaluate(15);
            Assert.True(rule.HasError);
            Assert.AreEqual("Error", rule.Error);
            Assert.AreEqual(2, calls);
        }
    }
}
