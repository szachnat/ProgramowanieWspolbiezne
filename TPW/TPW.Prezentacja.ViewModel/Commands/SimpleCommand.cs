using System;
using System.Threading.Tasks;

namespace TPW.Prezentacja.ViewModel.Commands
{
    internal class SimpleCommand : CommandBase
    {
        public SimpleCommand(MainViewModel mainView, Func<object?, Task<bool>> execute, Func<object?, bool>? canExecute) : base(execute, canExecute)
        {
            mainView.PropertyChanged += (sender, e) => OnCanExecuteChanged();
        }
    }
}

