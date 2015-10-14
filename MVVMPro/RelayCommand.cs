using System;
using System.Windows.Input;

namespace MVVMPro
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Action Execute { get; }
        private Func<bool> CanExecute { get; }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            Execute = execute;
            CanExecute = canExecute;
        }
        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand() : this(null, null) { }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute == null || CanExecute.Invoke();
        }

        void ICommand.Execute(object parameter)
        {
            Execute?.Invoke();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Action<T> Execute { get; }
        private Func<T, bool> CanExecute { get; }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            Execute = execute;
            CanExecute = canExecute;
        }
        public RelayCommand(Action<T> execute) : this(execute, null)
        { }
        public RelayCommand() : this(null, null) { }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute == null || CanExecute.Invoke((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute?.Invoke((T)parameter);
        }
    }
}
