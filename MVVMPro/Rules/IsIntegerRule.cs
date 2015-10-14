using System;
using System.Collections.Generic;
using System.Linq;
namespace MVVMPro.Rules
{
    public class IsIntegerRule
    {
        private static List<char> AllowedNumerals { get; } = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        Func<string> ValueEvaluator { get; }
        public IsIntegerRule(Func<string> valueEvaluator)
        {
            ValueEvaluator = valueEvaluator;
        }

        public static implicit operator Func<bool>(IsIntegerRule rule)
        {
            return () =>
            {
                var value = rule.ValueEvaluator();
                return !string.IsNullOrEmpty(value) && value.All(c => AllowedNumerals.Contains(c));  //If the string isn't empty or null and all characters are numeric we're good to go
            };
        }
    }
}
