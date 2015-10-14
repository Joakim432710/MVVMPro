using System;

namespace MVVMPro.Rules
{
    public class NotEqualsRule<T> where T : IComparable<T>
    {
        Func<T> ValueEvaluator { get; }
        T CriticalValue { get; }
        public NotEqualsRule(Func<T> valueEvaluator, T criticalValue)
        {
            ValueEvaluator = valueEvaluator;
            CriticalValue = criticalValue;
        }

        public static implicit operator Func<bool>(NotEqualsRule<T> rule)
        {
            return () => rule.ValueEvaluator().CompareTo(rule.CriticalValue) != 0;
        }
    }
}
