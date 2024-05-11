using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace TPW.Prezentacja.ViewModel
{
    /// <summary>
    /// Baza klas ViewModel
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public Thread mainThread;

        public ViewModelBase()
        {
            mainThread = Thread.CurrentThread;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            try
            {
                Dispatcher.FromThread(mainThread).Invoke(new Action(() => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }));
            }
            catch 
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            
        }
    }
}
