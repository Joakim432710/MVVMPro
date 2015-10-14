using System;

namespace MVVMPro.Rules
{
    public class LessThanOrEqualsRule<T> where T : IComparable<T>
    {
        Func<T> ValueEvaluator { get; }
        T CriticalValue { get; }
        public LessThanOrEqualsRule(Func<T> valueEvaluator, T criticalValue)
        {
            ValueEvaluator = valueEvaluator;
            CriticalValue = criticalValue;
        }

        public static implicit operator Func<bool>(LessThanOrEqualsRule<T> rule)
        {
            return () => rule.ValueEvaluator().CompareTo(rule.CriticalValue) <= 0;
        }
    }
}
