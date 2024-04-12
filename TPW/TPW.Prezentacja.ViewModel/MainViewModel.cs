using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TPW.Prezentacja.Model;
using TPW.Prezentacja.ViewModel.Commands;

namespace TPW.Prezentacja.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ModelApiBase model;

        public ObservableCollection<IModelBall> Balls => model.Balls;

        public double PlaneWidth { get => model.PlaneWidth; set { model.PlaneWidth = value; OnPropertyChanged(nameof(PlaneWidth)); } }
        public double PlaneHeight { get => model.PlaneHeight; set { model.PlaneHeight = value; OnPropertyChanged(nameof(PlaneHeight)); } }
        private uint _ballsNum;
        public uint BallsNumber { get { return _ballsNum; } set { _ballsNum = value; OnPropertyChanged(nameof(BallsNumber)); } }
        private uint _maxBallsNum;
        public uint MaxBallsNumber { get { return _maxBallsNum; } set { _maxBallsNum = value; OnPropertyChanged(nameof(MaxBallsNumber)); } }
        private uint _ballsRadius;
        public uint BallsRadius { get { return _ballsRadius; } set { _ballsRadius = value; OnPropertyChanged(nameof(BallsRadius)); } }
        public static uint MaxBallVel => 10;
        public static uint MinBallVel => 100;
        public ICommand GenerateBallsCommand { get; private set; }

        public MainViewModel()
        {
            this.BallsNumber = 0;
            this.MaxBallsNumber = 0;
            this.GenerateBallsCommand = new GenerateBallsCommand(this);
            this.model = ModelApiBase.GetApi();
            this.PropertyChanged += RecalculateMaxBallsNumber;
        }
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
