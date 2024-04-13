using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
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
        private uint _maxBallsNum;
        public uint MaxBallsNumber { get { return _maxBallsNum; } private set { if (_maxBallsNum != value) { _maxBallsNum = value; OnPropertyChanged(nameof(MaxBallsNumber)); } } }
        
        public static double BallsRadius => 20;
        public static double MaxBallVel => 100;
        public static double MinBallVel => 10;
        public ICommand GenerateBallsCommand { get; private set; }
        public ICommand StopSimulationCommand { get; private set; }

        public MainViewModel()
        {
            this.BallsNumber = 0;
            this.MaxBallsNumber = 0;
            this.GenerateBallsCommand = new GenerateBallsCommand(this);
            this.StopSimulationCommand = new StopSimulationCommand(this);
            this.model = ModelApiBase.GetApi();
            this.PropertyChanged += RecalculateMaxBallsNumber;
        }

        /// <summary>
        /// Funkcja kalkulująca maksymalną ilość kulek w zależności od wielkości planszy
        /// </summary>
        void RecalculateMaxBallsNumber(object? source, PropertyChangedEventArgs? e)
        {
            if (e?.PropertyName == nameof(PlaneWidth) || e?.PropertyName == nameof(PlaneHeight))
            {
                uint ballsInHeight = (uint)(PlaneHeight / (BallsRadius * 2));
                uint ballsInWidth = (uint)(PlaneWidth / (BallsRadius * 2));
                uint ballsNumber = ballsInHeight * ballsInWidth;
                MaxBallsNumber = ballsNumber >= 40 ? ballsNumber - 40 : 0;
            }
        }
    }
}
