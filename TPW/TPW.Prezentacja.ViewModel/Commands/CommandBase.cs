using System;
using System.Windows.Input;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace TPW.Prezentacja.ViewModel.Commands
{

    public delegate void CommandEventHandler(object source, CommandEventArgs e);

    public class CommandEventArgs : EventArgs
    {
        private readonly string EventInfo;

        public CommandEventArgs(string eventInfo) 
        { 
            EventInfo = eventInfo;
        }

        public string GeInfo()
        {
            return EventInfo;
        }
    }

    /// <summary>
    /// Baza klas Command
    /// </summary>
    internal class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public event CommandEventHandler? OnExecuteDone;

        private readonly Func<object?, Task<bool>> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public CommandBase(Func<object?, Task<bool>> execute, Func<object?, bool>? canExecute)
        {
            _execute = execute ?? (async (param) => { return await Task.Run(() => { return true; }); });
            _canExecute = canExecute;
        }

        private async Task ExecuteAsync(object? parameter)
        {
            await _execute(parameter);
            OnExecuteDone?.Invoke(this, new CommandEventArgs(""));
        }

        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public void Execute(object? parameter)
        {
            ExecuteAsync(parameter);
        }
        #endregion
    }
}
