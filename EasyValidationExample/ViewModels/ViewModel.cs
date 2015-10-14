using System.Windows;
using System.Windows.Input;
using MVVMPro;
using MVVMPro.Rules;

namespace EasyValidationExample.ViewModels
{
    public class ViewModel : EasyValidatableObject
    {
        public ViewModel()
        {
            AddRule(new StringDoesntContainRule(() => Name, " "), "Please do not use spaces", () => Name);
            AddRule(new StringDoesntContainRule(() => Name, "a"), "Please do not use lowercase a", () => Name);
            AddRule(new MinStringLengthRule(() => Name, 3), "Please make sure to use a name of at least three characters", () => Name);
            AddRule(new MaxStringLengthRule(() => Name, 10), "Please make sure to use a name of at most ten characters", () => Name);

            AddRule(new GreaterThanOrEqualsRule<int>(() => Age, 18), "Please make sure you are at least eighteen years of age", () => Age);

            //Unnecessary because ICommand will turn off the button, but still a possible way to use this instead of ICommand's disable
            //Also, looks fugly =) I do not recommend visual styles like these
            //AddRule(() => !HasErrors, "Fill out all the fields correctly first", () => SubmitCommand); 
        }

        public string Name
        {
            get { return Get(() => Name, "Enter a name"); }
            set { Set(() => Name, value); }
        }

        public int Age
        {
            get { return Get(() => Age); }
            set { Set(() => Age, value); }
        }

        public ICommand SubmitCommand
        {
            // This is what we worked for, beautiful! INotifyPropertyChanged, ICommand, Default Initialization and Lambda Commands Support. 
            // It doesn't get quicker than this to write a command with this functionality, and you can make a template for this EASILY!
            get { return Get(() => SubmitCommand, new RelayCommand(() => MessageBox.Show("Submitted successfully"), () => !HasErrors)); } //Will work without CanExecute portion but will look worse! =)
        }
    }
}
