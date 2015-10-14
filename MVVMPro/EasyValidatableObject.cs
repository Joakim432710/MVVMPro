﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace MVVMPro
{
    public class EasyValidatableObject : EasyObservableObject, IDataErrorInfo
    {
        private List<RuleBinding> RuleList { get; } = new List<RuleBinding>();
        private Dictionary<string, IList<RuleBinding>> RuleMap { get; } = new Dictionary<string, IList<RuleBinding>>();

        /// <summary>
        ///     Adds a rule to the model that does error checking when the model changes
        /// </summary>
        /// <param name="rule">The rule to add</param>
        /// <param name="errorMessage">The error message to throw when the error occurs</param>
        /// <param name="expression">An optional property path, if specified only updates to this property will invoke the error checking</param>
        public void AddRule<T>(Func<bool> rule, string errorMessage, Expression<Func<T>> expression = null)
        {
            if (expression == null)
            {
                RuleList.Add(new RuleBinding(rule, errorMessage));
                return;
            }
            var name = GetPropertyName(expression);
            if (RuleMap.ContainsKey(name))
                RuleMap[name].Add(new RuleBinding(rule, errorMessage));
            else
                RuleMap.Add(name, new List<RuleBinding>(1) { new RuleBinding(rule, errorMessage) });
        }

        /// <summary>
        ///     Sets the value of a specific property path using the internal map
        ///     If the map already contains an identic value then forceUpdate decides whether we re-update and re-notify
        ///     By default this behaviour is off
        /// 
        ///     Also makes sure the rule for this property (if it exists) is set to dirty
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="path">A path to the property to set</param>
        /// <param name="value">A value to update propName with</param>
        /// <param name="forceUpdate">Whether to enforce update if the previous value turns out to equal the new value, default false</param>
        protected override void Set<T>(Expression<Func<T>> path, T value, bool forceUpdate = false)
        {
            var name = GetPropertyName(path);
            if (RuleMap.ContainsKey(name))
                foreach(var rule in RuleMap[name])
                    rule.IsDirty = true;
            base.Set(name, value, forceUpdate);
        }

        /// <summary>
        ///     Whether this model contains any errors
        /// </summary>
        public bool HasErrors
        {
            get
            {
                var values = RuleMap.Values.SelectMany(ruleList => ruleList).ToList();
                var oldValues = values.Where(rule => !rule.IsDirty);
                if (oldValues.Any(rule => rule.HasError)) return true;
                var newValues = values.Where(rule => rule.IsDirty).ToList();
                newValues.ForEach(rule => rule.Update());
                return newValues.Any(rule => rule.HasError);
            }
        }

        /// <summary>
        ///     A <see cref="IDataErrorInfo.this[string]"/> implementor that simply calls <see cref="GetError(string)"/>
        /// </summary>
        /// <param name="columnName">The rule name to get</param>
        /// <returns>The error message associated with the rule name</returns>
        public string this[string columnName] => GetError(columnName);

        /// <summary>
        ///     A string containing a list of errors generated by this model's state
        /// </summary>
        public string Error
        {
            get
            {
                var firstList = RuleMap.Values.SelectMany(ruleList => ruleList).Where(rule => rule.HasError).Select(rule => rule.Error).ToList(); ;
                var secondList = RuleList.Where(rule => rule.HasError).Select(rule => rule.Error);
                var firstString = string.Join(Environment.NewLine, firstList);
                var secondString = string.Join(Environment.NewLine, secondList);
                if (string.IsNullOrEmpty(firstString))
                    return string.IsNullOrEmpty(secondString) ? string.Empty : secondString;
                if (string.IsNullOrEmpty(secondString))
                    return firstString;
                return firstString + Environment.NewLine + secondString;
            }
        }

        /// <summary>
        ///     Gets a specific error based on property / error name
        ///     Shall not throw if the propertyName does not exist as a rule
        /// </summary>
        /// <param name="propertyName">The property / error name to get the error message for</param>
        /// <returns>propertyName's isolated error message</returns>
        public string GetError(string propertyName)
        {
            if (!RuleMap.ContainsKey(propertyName)) return string.Empty;

            foreach (var rule in RuleMap[propertyName].Where(rule => rule.IsDirty))
                rule.Update();
            var errors = RuleMap[propertyName].Where(rule => rule.HasError).Select(rule => rule.Error).ToList();
            return errors.Count == 0 ? string.Empty : string.Join(Environment.NewLine, errors);
        }

        /// <summary>
        ///     Gets a specific error based on property / error name
        ///     Shall throw if the propertyName does not exist as a rule
        /// </summary>
        /// <param name="propertyPath">A path to the property to get the error for</param>
        /// <returns>propertyName's isolated error message</returns>
        public string GetError<T>(Expression<Func<T>> propertyPath)
        {
            return GetError(GetPropertyName(propertyPath));
        }
    }
}