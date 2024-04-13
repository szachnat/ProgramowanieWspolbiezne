using System;
using System.Windows.Input;

namespace TPW.Prezentacja.ViewModel.Commands
{
    /// <summary>
    /// Baza klas Command
    /// </summary>
    internal abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }
        public abstract void Execute(object? parameter);
        protected void OnCanExecuteChanged() 
        { 
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
