using TPW.Prezentacja.ViewModel;

namespace TPW.Prezentacja.Tests
{
    public class MainViewModelTest
    {
        [Test]
        public void ConstructorAndGettersTest()
        {
            ViewModelBase viewModel = new MainViewModel();

            Assert.IsNotNull(viewModel);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(((MainViewModel)viewModel).model);
                Assert.IsNotNull(((MainViewModel)viewModel).Balls);
                Assert.AreEqual(0, ((MainViewModel)viewModel).Balls.Count);
                Assert.AreEqual(0, ((MainViewModel)viewModel).PlaneWidth);
                Assert.AreEqual(0, ((MainViewModel)viewModel).PlaneHeight);
                Assert.AreEqual(0, ((MainViewModel)viewModel).BallsNumber);
                Assert.AreEqual(0, ((MainViewModel)viewModel).CurrentMaxBallsNumber);
                Assert.IsNotNull(((MainViewModel)viewModel).GenerateBallsCommand);
                Assert.IsNotNull(((MainViewModel)viewModel).StopSimulationCommand);
            });
        }

        [Test]
        public void SettersTest()
        {
            ViewModelBase viewModel = new MainViewModel();

            Assert.IsNotNull(viewModel);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(0d, ((MainViewModel)viewModel).PlaneWidth, 0.01d);
                Assert.AreEqual(0d, ((MainViewModel)viewModel).PlaneHeight, 0.01d);
                Assert.AreEqual(0, ((MainViewModel)viewModel).BallsNumber);
                Assert.AreEqual(0, ((MainViewModel)viewModel).CurrentMaxBallsNumber);
            });

            ((MainViewModel)viewModel).PlaneWidth = 300d;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(300d, ((MainViewModel)viewModel).PlaneWidth, 0.01d);
                Assert.AreEqual(0d, ((MainViewModel)viewModel).PlaneHeight, 0.01d);
                Assert.AreEqual(0, ((MainViewModel)viewModel).CurrentMaxBallsNumber);
            });

            ((MainViewModel)viewModel).PlaneHeight = 200d;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(300d, ((MainViewModel)viewModel).PlaneWidth, 0.01d);
                Assert.AreEqual(200d, ((MainViewModel)viewModel).PlaneHeight, 0.01d);
                Assert.AreEqual(20d, ((MainViewModel)viewModel).CurrentMaxBallsNumber);
            });

            ((MainViewModel)viewModel).PlaneWidth = 2100d;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(2100d, ((MainViewModel)viewModel).PlaneWidth, 0.01d);
                Assert.AreEqual(200d, ((MainViewModel)viewModel).PlaneHeight, 0.01d);
                Assert.AreEqual(19, ((MainViewModel)viewModel).CurrentMaxBallsNumber);
            });

            ((MainViewModel)viewModel).BallsNumber = 1;
            Assert.AreEqual(1, ((MainViewModel)viewModel).BallsNumber);
        }
    }
}
