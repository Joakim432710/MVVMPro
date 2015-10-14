using System;
using System.Linq.Expressions;

namespace MVVMPro.Rules
{
    public class LessThanRule<T> where T : IComparable<T>
    {
        Func<T> ValueEvaluator { get; }
        T CriticalValue { get; }
        public LessThanRule(Func<T> valueEvaluator, T criticalValue)
        {
            ValueEvaluator = valueEvaluator;
            CriticalValue = criticalValue;
        }

        public static implicit operator Func<bool>(LessThanRule<T> rule)
        {
            return () => rule.ValueEvaluator().CompareTo(rule.CriticalValue) < 0;
        } 
    }
}
