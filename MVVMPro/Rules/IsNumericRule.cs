using System;
using System.Collections.Generic;
using System.Linq;
namespace MVVMPro.Rules
{
    public class IsNumericRule
    {
        private static List<char> AllowedNumerals { get; } = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static List<char> AllowedSpecials { get; } = new List<char> { '.', ',' };

        Func<string> ValueEvaluator { get; }
        public IsNumericRule(Func<string> valueEvaluator)
        {
            ValueEvaluator = valueEvaluator;
        }

        public static implicit operator Func<bool>(IsNumericRule rule)
        {
            return () =>
            {
                var value = rule.ValueEvaluator();
                if (string.IsNullOrEmpty(value)) return false;

                var specials = value.Where(c => !AllowedNumerals.Contains(c)).ToList();
                if (specials.Count == 0) return true; //No special characters means everything was numerical for example 0125214
                if (specials.Count > 1) return false; //Can't have more than 1 decimal separator
                if(specials.Any(c => !AllowedSpecials.Contains(c))) return false; //Found a character not matching the decimal separators

                var index = value.IndexOf(specials.First());
                if (index == 0) return false; //Decimal can't be first in our notation, some implementations might allow this though
                return (index - 1) != value.Length; //If the decimal point is not the last character it has at least one numeric character on left and right side, rule passes validation
            };
        }
    }
}
