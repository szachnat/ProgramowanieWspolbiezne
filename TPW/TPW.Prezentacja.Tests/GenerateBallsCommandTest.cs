using System.Windows.Input;
using TPW.Dane;
using TPW.Prezentacja.Model;
using TPW.Prezentacja.ViewModel;
using TPW.Prezentacja.ViewModel.Commands;

namespace TPW.Prezentacja.Tests
{
    public class GenerateBallsCommandTest
    {
        [Test]
        public void ConstructorTest()
        {
            ViewModelBase viewModel = new MainViewModel();

            Assert.IsNotNull(viewModel);

            ICommand command = new GenerateBallsCommand((MainViewModel)viewModel);
            
            Assert.IsNotNull(command);
        }

        [Test]
        public void CanExecuteTest()
        {
            ViewModelBase viewModel = new MainViewModel();

            Assert.IsNotNull(viewModel);

            ICommand command = new GenerateBallsCommand((MainViewModel)viewModel);

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

            ICommand command = new GenerateBallsCommand((MainViewModel)viewModel);

            Assert.IsNotNull(command);
            ((MainViewModel)viewModel).PlaneWidth = 2100d;
            ((MainViewModel)viewModel).PlaneHeight = 200d;
            ((MainViewModel)viewModel).BallsNumber = 1;

            Assert.IsTrue(command.CanExecute("true"));

            command.Execute("execute");

            Assert.IsNotNull(((MainViewModel)viewModel).Balls);
            Assert.IsNotNull(((MainViewModel)viewModel).model.Balls);
            Assert.AreEqual(1, ((MainViewModel)viewModel).Balls.Count);
            Assert.AreEqual(1, ((MainViewModel)viewModel).model.Balls.Count);

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
