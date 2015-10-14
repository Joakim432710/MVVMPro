using System;

namespace MVVMPro.Rules
{
    public class GreaterThanOrEqualsRule<T> where T : IComparable<T>
    {
        Func<T> ValueEvaluator { get; }
        T CriticalValue { get; }
        public GreaterThanOrEqualsRule(Func<T> valueEvaluator, T criticalValue)
        {
            ValueEvaluator = valueEvaluator;
            CriticalValue = criticalValue;
        }

        public static implicit operator Func<bool>(GreaterThanOrEqualsRule<T> rule)
        {
            return () => rule.ValueEvaluator().CompareTo(rule.CriticalValue) >= 0;
        }
    }
}
