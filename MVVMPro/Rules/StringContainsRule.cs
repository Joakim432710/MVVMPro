using System;

namespace MVVMPro.Rules
{
    public class StringContainsRule
    {
        Func<string> ValueEvaluator { get; }
        string ContainsValue { get; }
        public StringContainsRule(Func<string> valueEvaluator, string containsValue)
        {
            ValueEvaluator = valueEvaluator;
            ContainsValue = containsValue;
        }

        public static implicit operator Func<bool>(StringContainsRule rule)
        {
            return () => rule.ValueEvaluator().Contains(rule.ContainsValue);
        }
    }
}
