using System;

namespace MVVMPro.Rules
{
    public class MaxStringLengthRule
    {
        Func<string> ValueEvaluator { get; }
        int MaxLength { get; }
        public MaxStringLengthRule(Func<string> valueEvaluator, int maxLength)
        {
            ValueEvaluator = valueEvaluator;
            MaxLength = maxLength;
        }

        public static implicit operator Func<bool>(MaxStringLengthRule rule)
        {
            return () => rule.ValueEvaluator().Length <= rule.MaxLength;
        }
    }
}
