using System;

namespace MVVMPro.Rules
{
    public class EqualsRule<T> where T : IComparable<T>
    {
        Func<T> ValueEvaluator { get; }
        T CriticalValue { get; }
        public EqualsRule(Func<T> valueEvaluator, T criticalValue)
        {
            ValueEvaluator = valueEvaluator;
            CriticalValue = criticalValue;
        }

        public static implicit operator Func<bool>(EqualsRule<T> rule)
        {
            return () => rule.ValueEvaluator().CompareTo(rule.CriticalValue) == 0;
        }
    }
}
