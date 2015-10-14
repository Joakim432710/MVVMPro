using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MVVMPro.Annotations;

namespace MVVMPro
{
    /// <summary>
    ///     A basic light-weight notification model to be used with the most performance critical models
    ///     Nothing except implementation is done for you and all INotifyPropertyChange calls will be left up to the consumer to handle
    ///     An example call from within a property's set method use the [CallerMemberName] attribute and look like this: OnPropertyChanged();
    ///     If from within another property consider the string literal and nameof expression: 
    ///         OnPropertyChanged(nameof(PropertyName)); (Default and preferred)
    ///         OnPropertyChanged("PropertyName") (Okay, but avoid if possible)
    ///     
    ///     Example usage cases of the latter include C# 5 or below
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        ///     An event to implement <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     A method to invoke <see cref="INotifyPropertyChanged.PropertyChanged"/>
        ///     Utilizes the <see cref="CallerMemberNameAttribute"/> to deduce names automatically where possible
        ///     Shall not throw if PropertyChanged is null
        ///  </summary>
        /// <param name="propertyName">The name of the property that invoked the model change update behaviour</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Subscribes a handler that listens to this model's update behaviour
        /// </summary>
        /// <param name="e">A method handler to add to the subscription list</param>
        public virtual void SubscribeOnPropertyChanged(PropertyChangedEventHandler e)
        {
            PropertyChanged += e;
        }

        /// <summary>
        ///     Unsubscribes an existing handler that listened to this model's update behaviour
        ///     This will ensure the handler is no longer invoked
        /// 
        ///     Shall throw if the handler does not exist in the subscription list
        /// </summary>
        /// <param name="e">A method to remove from the subscription list</param>
        public virtual void UnsubscribeOnPropertyChanged(PropertyChangedEventHandler e)
        {
            if (PropertyChanged == null) //Doesn't exist because EventHandler has no events
                throw new ArgumentException("The event must already have been subscribed to be removed.");
            if (!(PropertyChanged?.GetInvocationList()).Contains(e)) //Doesn't exist in EventHandler list
                throw new ArgumentException("The event must already have been subscribed to be removed.");

            PropertyChanged -= e;
        }
    }
}
