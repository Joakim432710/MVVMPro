using System;

namespace MVVMPro.Rules
{
    public class GreaterThanRule<T> where T : IComparable<T>
    {
        Func<T> ValueEvaluator { get; }
        T CriticalValue { get; }
        public GreaterThanRule(Func<T> valueEvaluator, T criticalValue)
        {
            ValueEvaluator = valueEvaluator;
            CriticalValue = criticalValue;
        }

        public static implicit operator Func<bool>(GreaterThanRule<T> rule)
        {
            return () => rule.ValueEvaluator().CompareTo(rule.CriticalValue) > 0;
        }
    }
}
