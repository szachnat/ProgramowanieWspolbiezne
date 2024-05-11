using System.Windows.Input;
using TPW.Dane;
using TPW.Prezentacja.Model;
using TPW.Prezentacja.ViewModel;
using TPW.Prezentacja.ViewModel.Commands;

namespace TPW.Prezentacja.Tests
{
    public class SimpleCommandTest
    {
        [Test]
        public void ConstructorTest()
        {
            ViewModelBase viewModel = new MainViewModel();

            Assert.IsNotNull(viewModel);

            ICommand command = new SimpleCommand((MainViewModel)viewModel, async (param) => { return await Task.Run(() => { return true; }); }, (param) => { return true; });

            Assert.IsNotNull(command);
        }

        [Test]
        public void CanExecuteTest()
        {
            ViewModelBase viewModel = new MainViewModel();

            Assert.IsNotNull(viewModel);

            ICommand command = new SimpleCommand((MainViewModel)viewModel, async (param) => { return await Task.Run(() => { return true; }); }, (param) => { return ((MainViewModel)viewModel).BallsNumber > 0 && ((MainViewModel)viewModel).BallsNumber <= ((MainViewModel)viewModel).CurrentMaxBallsNumber; });

            Assert.IsNotNull(command);

            Assert.IsFalse(command.CanExecute("false"));

            ((MainViewModel)viewModel).PlaneWidth = 2100d;

            Assert.IsFalse(command.CanExecute("false"));

            ((MainViewModel)viewModel).PlaneHeight = 200d;

            Assert.IsFalse(command.CanExecute("false"));

            ((MainViewModel)viewModel).BallsNumber = 1;

            Assert.IsTrue(command.CanExecute("true"));
        }

        [Test]
        public void ExecuteTest()
        {
            ViewModelBase viewModel = new MainViewModel();

            Assert.IsNotNull(viewModel);

            ICommand command = new SimpleCommand((MainViewModel)viewModel, async (param) =>
            {
                return await Task.Run(() =>
                {
                    lock (((MainViewModel)viewModel).Balls)
                    {
                        ((MainViewModel)viewModel).model.GenerateBalls(((MainViewModel)viewModel).BallsNumber, MainViewModel.BallsRadius, MainViewModel.MinBallVel, MainViewModel.MaxBallVel);
                        ((MainViewModel)viewModel).model.Start();
                    }
                    return true;
                });

            }, (param) =>
            {
                return ((MainViewModel)viewModel).BallsNumber > 0 && ((MainViewModel)viewModel).BallsNumber <= ((MainViewModel)viewModel).CurrentMaxBallsNumber;
            });

            bool generated = false;

            ((SimpleCommand)command).OnExecuteDone += (object source, CommandEventArgs e) =>
            {
                generated = !generated;
            };

            Assert.IsNotNull(command);
            ((MainViewModel)viewModel).PlaneWidth = 2100d;
            ((MainViewModel)viewModel).PlaneHeight = 200d;
            ((MainViewModel)viewModel).BallsNumber = 1;

            Assert.IsTrue(command.CanExecute("true"));

            command.Execute("execute");

            Thread.Sleep(1000);

            Assert.IsNotNull(((MainViewModel)viewModel).Balls);
            Assert.IsNotNull(((MainViewModel)viewModel).model.Balls);
            Assert.AreEqual(1, ((MainViewModel)viewModel).model.Balls.Count);
            Assert.AreEqual(1, ((MainViewModel)viewModel).Balls.Count);
            Assert.IsTrue(generated);

            Pos2D startPos = ((IModelBall)((MainViewModel)viewModel).model.Balls.ToArray().GetValue(0)).CanvasPos;

            Thread.Sleep(10);

            Pos2D pos = ((IModelBall)((MainViewModel)viewModel).model.Balls.ToArray().GetValue(0)).CanvasPos;

            Assert.Multiple(() =>
            {
                Assert.AreNotEqual(startPos.X, pos.X);
                Assert.AreNotEqual(startPos.Y, pos.Y);
            });

            ((MainViewModel)viewModel).model.Stop();
        }
    }
}
