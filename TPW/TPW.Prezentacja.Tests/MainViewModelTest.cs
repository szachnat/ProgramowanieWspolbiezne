﻿using TPW.Prezentacja.ViewModel;

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
                Assert.AreEqual(0, ((MainViewModel)viewModel).MaxBallsNumber);
                Assert.IsNotNull(((MainViewModel)viewModel).GenerateBallsCommand);
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
                Assert.AreEqual(0, ((MainViewModel)viewModel).MaxBallsNumber);
            });

            ((MainViewModel)viewModel).PlaneWidth = 100d;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(100d, ((MainViewModel)viewModel).PlaneWidth, 0.01d);
                Assert.AreEqual(0d, ((MainViewModel)viewModel).PlaneHeight, 0.01d);
                Assert.AreEqual(0, ((MainViewModel)viewModel).MaxBallsNumber);
            });

            ((MainViewModel)viewModel).PlaneHeight = 200d;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(100d, ((MainViewModel)viewModel).PlaneWidth, 0.01d);
                Assert.AreEqual(200d, ((MainViewModel)viewModel).PlaneHeight, 0.01d);
                Assert.AreEqual(0, ((MainViewModel)viewModel).MaxBallsNumber);
            });

            ((MainViewModel)viewModel).PlaneWidth = 2100d;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(2100d, ((MainViewModel)viewModel).PlaneWidth, 0.01d);
                Assert.AreEqual(200d, ((MainViewModel)viewModel).PlaneHeight, 0.01d);
                Assert.AreEqual(220, ((MainViewModel)viewModel).MaxBallsNumber);
            });

            ((MainViewModel)viewModel).BallsNumber = 1;
            Assert.AreEqual(1, ((MainViewModel)viewModel).BallsNumber);
        }
    }
}
