using System.ComponentModel;
using System.Windows;
using TPW.Dane;
using TPW.Prezentacja.Model;

namespace TPW.Prezentacja.ViewModel.Commands
{
    /// <summary>
    /// Klasa implementująca komendę zatrzymująca symulację
    /// </summary>
    internal class StopSimulationCommand : CommandBase
    {
        private readonly MainViewModel mainViewModel;

        public StopSimulationCommand(MainViewModel mainView)
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
            //this.mainViewModel.model.GenerateBalls(this.mainViewModel.BallsNumber, MainViewModel.BallsRadius, MainViewModel.MinBallVel, MainViewModel.MaxBallVel);
            this.mainViewModel.OnPropertyChanged(nameof(this.mainViewModel.Balls));
            this.mainViewModel.model.Stop();
            MessageBox.Show("Simulation stopped", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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