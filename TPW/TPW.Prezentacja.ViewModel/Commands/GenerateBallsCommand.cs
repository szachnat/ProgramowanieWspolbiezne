﻿using System.ComponentModel;
using System.Windows;
using TPW.Dane;

namespace TPW.Prezentacja.ViewModel.Commands
{
    /// <summary>
    /// Klasa implementująca komendę generującą kulku
    /// </summary>
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
            MessageBox.Show("Generated " + mainViewModel.BallsNumber + " balls", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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