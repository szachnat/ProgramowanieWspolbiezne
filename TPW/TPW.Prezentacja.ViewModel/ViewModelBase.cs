using System.ComponentModel

namespace TPW.Prezentacja.ViewModel
{
    public class ViewModelBase
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
