using System.ComponentModel;
using System.Windows;
using TPW.Dane;

namespace TPW.Prezentacja.ViewModel.Commands
{
    internal class GenerateBallsCommand : CommandBase
    {
        private readonly MainViewModel mainViewModel;

        public GenerateBallsCommand(MainViewModel mainView)
        {
            mainViewModel = mainView;
            mainViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        public override bool CanExecute(object? parameter)
        {
            return mainViewModel.BallsNumber > 0 && base.CanExecute(parameter) && mainViewModel.BallsNumber <= mainViewModel.MaxBallsNumber;
        }
        public override void Execute(object? parameter) 
        {
            this.mainViewModel.model.GenerateBalls(this.mainViewModel.BallsNumber, MainViewModel.BallsRadius, MainViewModel.MinBallVel, MainViewModel.MaxBallVel);
            this.mainViewModel.OnPropertyChanged(nameof(this.mainViewModel.Balls));
            MessageBox.Show("Generated " + mainViewModel.BallsNumber + " Balls" + "Radius is" + MainViewModel.BallsRadius + "Minimal Velocity " + MainViewModel.MinBallVel + "Maximal Velocity " + MainViewModel.MaxBallVel + "Max Balls" + this.mainViewModel.MaxBallsNumber, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.mainViewModel.model.Start();
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
