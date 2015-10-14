using System;

namespace MVVMPro.Rules
{
    public class StringDoesntContainRule
    {
        Func<string> ValueEvaluator { get; }
        string ContainsValue { get; }
        public StringDoesntContainRule(Func<string> valueEvaluator, string containsValue)
        {
            ValueEvaluator = valueEvaluator;
            ContainsValue = containsValue;
        }

        public static implicit operator Func<bool>(StringDoesntContainRule rule)
        {
            return () => !rule.ValueEvaluator().Contains(rule.ContainsValue);
        }
    }
}
