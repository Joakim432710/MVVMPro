using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MVVMPro
{
    /// <summary>
    ///     An easy notifier meant to simplify MVVM Properties
    ///     Create properties that implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> in a flash using Get(() => PropertyName); and Set(() => PropertyName, value);
    /// </summary>
    public abstract class EasyObservableObject : ObservableObject
    {
        private Dictionary<string, object> PropertyValueDictionary { get; } = new Dictionary<string, object>();

        /// <summary>
        ///     A helper method that retrieves a fully qualified name for the current method discarding any other irrelevant information
        /// </summary>
        /// <typeparam name="T">The type of the function that returns the property path</typeparam>
        /// <param name="expression">An expresssion pointing to the property, for example () => PropertyName</param>
        /// <returns>The Property's qualified name within the class</returns>
        protected static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var body = expression.Body;
            var memberExpression = body as MemberExpression ?? (MemberExpression)((UnaryExpression)body).Operand;
            return memberExpression.Member.Name;
        }

        /// <summary>
        ///     An overload to allow OnPropertyChanged behaviour based solely on the safer path-based approach this model presents
        ///     Usage example: OnPropertyChanged(() => PropertyName);
        /// </summary>
        /// <typeparam name="T">The type of the property to update</typeparam>
        /// <param name="path">A path to the property, see summary</param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> path)
        {
            OnPropertyChanged(GetPropertyName(path));
        }

        /// <summary>
        ///     Gets the value of a specific property name using the internal map to store values previously accessed
        ///     If the map contains no such entry an entry shall be created and defaultValue shall be returned
        /// </summary>
        /// <typeparam name="T">The type of the return value</typeparam>
        /// <param name="propName">The name of the property</param>
        /// <param name="defaultValue">A default value to use if the property does not exist in the internal map, by default = default(T)</param>
        /// <returns>The value for propName</returns>
        protected virtual internal T Get<T>(string propName, T defaultValue = default(T))
        {
            if (PropertyValueDictionary.ContainsKey(propName))
                return (T)PropertyValueDictionary[propName];

            //First time logic
            PropertyValueDictionary.Add(propName, defaultValue);
            return defaultValue;
        }

        /// <summary>
        ///     Gets the value of a specific property path using the internal map to store values previously accessed
        ///     If the map contains no such entry an entry shall be created and defaultValue shall be returned
        /// </summary>
        /// <typeparam name="T">The type of the return value</typeparam>
        /// <param name="path">A path to the property to access</param>
        /// <param name="defaultValue">A default value to use if the property does not exist in the internal map, by default = default(T)</param>
        /// <returns>A value for path</returns>
        protected virtual T Get<T>(Expression<Func<T>> path, T defaultValue = default(T))
        {
            return Get(GetPropertyName(path), defaultValue);
        }

        /// <summary>
        ///     Sets the value of a specific property path using the internal map
        ///     If the map already contains an identic value then forceUpdate decides whether we re-update and re-notify
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="propName">The name of the property</param>
        /// <param name="value">A value to update propName with</param>
        /// <param name="forceUpdate">Whether to enforce update if the previous value turns out to equal the new value</param>
        protected virtual internal void Set<T>(string propName, T value, bool forceUpdate)
        {
            var old = Get<T>(propName);
            if (Equals(old, value) && !forceUpdate) return;
            PropertyValueDictionary[propName] = value;
            OnPropertyChanged(propName);
        }

        /// <summary>
        ///     Sets the value of a specific property path using the internal map
        ///     If the map already contains an identic value then forceUpdate decides whether we re-update and re-notify
        ///     By default this behaviour is off
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="path">A path to the property to set</param>
        /// <param name="value">A value to update propName with</param>
        /// <param name="forceUpdate">Whether to enforce update if the previous value turns out to equal the new value, default false</param>
        protected virtual void Set<T>(Expression<Func<T>> path, T value, bool forceUpdate = false)
        {
            Set(GetPropertyName(path), value, forceUpdate);
        }
    }
}
