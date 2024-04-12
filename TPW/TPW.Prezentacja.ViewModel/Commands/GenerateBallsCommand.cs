using System.ComponentModel;
using System.Windows;

namespace TPW.Prezentacja.ViewModel.Commands
{
    internal class GenerateBallsCommand : CommandBase
    {
        private readonly MainViewModel mainViewModel;

        public GenerateBallsCommand(MainViewModel mainView)
        {
            this.mainViewModel = mainView;
            mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        public override bool CanExecute(object? parameter)
        {
            return mainViewModel.BallsNumber > 0 && base.CanExecute(parameter) && mainViewModel.BallsNumber <= mainViewModel.MaxBallsNumber;
        }
        public override void Execute(object? parameter) 
        {
            this.mainViewModel.model.GenerateBalls(this.mainViewModel.BallsNumber, this.mainViewModel.BallsRadius, MainViewModel.MinBallVel, MainViewModel.MaxBallVel); ; ; ;  ;  ;
            this.mainViewModel.OnPropertyChanged(nameof(this.mainViewModel.Balls));
            this.mainViewModel.Start();
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(mainViewModel.BallsNumber))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
