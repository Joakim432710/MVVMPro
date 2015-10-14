using System;

namespace MVVMPro.Rules
{
    public class MinStringLengthRule
    {
        Func<string> ValueEvaluator { get; }
        int MinLength { get; }
        public MinStringLengthRule(Func<string> valueEvaluator, int minLength)
        {
            ValueEvaluator = valueEvaluator;
            MinLength = minLength;
        }

        public static implicit operator Func<bool>(MinStringLengthRule rule)
        {
            return () => rule.ValueEvaluator().Length >= rule.MinLength;
        }
    }
}
