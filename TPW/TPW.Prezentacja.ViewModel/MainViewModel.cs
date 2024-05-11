using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TPW.Prezentacja.Model;
using TPW.Prezentacja.ViewModel.Commands;

namespace TPW.Prezentacja.ViewModel
{
    /// <summary>
    /// Klasa implementująca główny ViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ModelApiBase model;

        public ObservableCollection<IModelBall> Balls => model.Balls;

        public double PlaneWidth { get => model.PlaneWidth; set { model.PlaneWidth = value; OnPropertyChanged(nameof(PlaneWidth)); } }
        public double PlaneHeight { get => model.PlaneHeight; set { model.PlaneHeight = value; OnPropertyChanged(nameof(PlaneHeight)); } }
        private uint _ballsNum;
        public uint BallsNumber { get { return _ballsNum; } set { _ballsNum = value; OnPropertyChanged(nameof(BallsNumber)); } }

        private uint _currentMaxBallsNumber;
        public uint CurrentMaxBallsNumber { get { return _currentMaxBallsNumber; } set { if (value != _currentMaxBallsNumber) { _currentMaxBallsNumber = value; OnPropertyChanged(nameof(CurrentMaxBallsNumber)); } } }

        public static uint MaxBallsNumber => 20;        
        public static double BallsRadius => 20;
        public static double MaxBallVel => 100;
        public static double MinBallVel => -100;
        public ICommand GenerateBallsCommand { get; private set; }
        public ICommand StopSimulationCommand { get; private set; }

        public MainViewModel() : base()
        {
            BallsNumber = 0;
            CurrentMaxBallsNumber = 0;
            //this.GenerateBallsCommand = new GenerateBallsCommand(this);
            //this.StopSimulationCommand = new StopSimulationCommand(this);
            GenerateBallsCommand = new SimpleCommand(this, Generate, (param) => { return BallsNumber > 0 && BallsNumber <= MaxBallsNumber; });
            ((SimpleCommand)GenerateBallsCommand).OnExecuteDone += (object source, CommandEventArgs e) =>
            {
                MessageBox.Show("Generated " + BallsNumber + " balls", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            };
            StopSimulationCommand = new SimpleCommand(this, Stop, (param) => { return BallsNumber > 0; });
            ((SimpleCommand)StopSimulationCommand).OnExecuteDone += (object source, CommandEventArgs e) =>
            {
                MessageBox.Show("Simulation stopped", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            };
            
            model = ModelApiBase.GetApi();
            PropertyChanged += RecalculateMaxBallsNumber;
        }

        private async Task<bool> Generate(object? parameter)
        {
            return await Task.Run(() =>
            {
                lock (Balls)
                {
                    model.GenerateBalls(BallsNumber, BallsRadius, MinBallVel, MaxBallVel);
                    model.Start();
                    OnPropertyChanged(nameof(Balls));
                }
                return true;
            });
        }

        private async Task<bool> Stop(object? parameter)
        {
             model.Stop();
            OnPropertyChanged(nameof(Balls));
            return true;
        }
        /// <summary>
        /// Funkcja kalkulująca maksymalną ilość kulek w zależności od wielkości planszy
        /// </summary>
        void RecalculateMaxBallsNumber(object? source, PropertyChangedEventArgs? e)
        {
            if (e?.PropertyName == nameof(PlaneWidth) || e?.PropertyName == nameof(PlaneHeight))
            {
                double height = Math.Max(PlaneHeight - 2 * BallsRadius, 0);
                double width = Math.Max(PlaneWidth -  2* BallsRadius, 0);
                double radius = Math.Sqrt((height * width) / (4 * (MaxBallsNumber + 40)));
                uint currentMaxNumber = MaxBallsNumber;
                currentMaxNumber = (uint)((height * width) / (4 * radius * radius));
                currentMaxNumber = currentMaxNumber > 40 ? currentMaxNumber - 40 : currentMaxNumber;
                CurrentMaxBallsNumber = currentMaxNumber;
            }
        }
    }
}
