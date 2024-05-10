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

        public static double MinBallMass => 2;
        public static double MaxBallMass => 20;
        
        public static double BallsRadius => 20;
        public static double MaxBallVel => 100;
        public static double MinBallVel => -100;
        public ICommand GenerateBallsCommand { get; private set; }
        public ICommand StopSimulationCommand { get; private set; }

        public MainViewModel() : base()
        {
            BallsNumber = 0;
            CurrentMaxBallsNumber = 0;
            this.GenerateBallsCommand = new GenerateBallsCommand(this);
            this.StopSimulationCommand = new StopSimulationCommand(this);
            model = ModelApiBase.GetApi();
            PropertyChanged += RecalculateMaxBallsNumber;
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
