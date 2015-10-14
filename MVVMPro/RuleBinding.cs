using System;

namespace MVVMPro
{
    internal class RuleBinding
    {
        /// <summary>
        ///     The error message the RuleBinding contains
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        ///     Whether the RuleBinding contains an error
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        ///     Whether the RuleBinding has changed and needs to re-do its error checking
        /// </summary>
        public bool IsDirty { get; set; } = true;

        /// <summary>
        ///     The rule that returns whether this binding is in a valid state
        /// </summary>
        private Func<bool> Rule { get; }

        /// <summary>
        ///     The message to use when an error occurs
        /// </summary>
        private string Message { get; }

        internal RuleBinding(Func<bool> rule, string message)
        {
            Rule = rule;
            Message = message;
        }

        /// <summary>
        ///     Updates the RuleBinding to check for errors and correctly execute all required steps to put the binding in a non-dirty state again
        ///     Ensures IsDirty is false after called
        /// </summary>
        internal void Update()
        {
            if (!IsDirty) return; //If we're not dirty we don't need to do anything to remove our dirtiness

            IsDirty = false;
            Error = null;
            HasError = false;
            try
            {
                if (Rule()) return; //We're okay, no error so use the values above 

                Error = Message;
                HasError = true;
            }
            catch (Exception e) //Catch any exceptions and print them as the rule error
            {
                HasError = true;
                Error = e.Message;
            }
        }
    }
}
