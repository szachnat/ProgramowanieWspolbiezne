namespace TPW.Dane.Tests
{
    public class BallTest
    {
        [Test]
        public void ConstructorAndGettersTest()
        {
            IBall ball = new Ball(100L, 0.1d, new Pos2D { X = 10.0d, Y = 11.34d } , new Pos2D { X = 9d, Y = 2.2222d });

            Assert.NotNull(ball);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(100L, ball.GetId());
                Assert.AreEqual(0.1d, ball.GetRadius(), 0.01d);
                Assert.AreEqual(10.0d, ball.GetPos().X, 0.01d);
                Assert.AreEqual(11.34d, ball.GetPos().Y, 0.01d);
                Assert.AreEqual(9d, ball.GetVel().X, 0.01d);
                Assert.AreEqual(2.2222d, ball.GetVel().Y, 0.01d);
            });
        }

        [Test]
        public void SettersTest()
        {
            IBall ball = new Ball(100L, 0d, new Pos2D { X = 0d, Y = 0d }, new Pos2D { X = 0d, Y = 0d });

            Assert.NotNull(ball);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(0d, ball.GetRadius(), 0.01d);
                Assert.AreEqual(0d, ball.GetPos().X, 0.01d);
                Assert.AreEqual(0d, ball.GetPos().Y, 0.01d);
                Assert.AreEqual(0d, ball.GetVel().X, 0.01d);
                Assert.AreEqual(0d, ball.GetVel().Y, 0.01d);
            });

            ball.SetRadius(10.222d);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(10.222d, ball.GetRadius(), 0.01d);
                Assert.AreEqual(0d, ball.GetPos().X, 0.01d);
                Assert.AreEqual(0d, ball.GetPos().Y, 0.01d);
                Assert.AreEqual(0d, ball.GetVel().X, 0.01d);
                Assert.AreEqual(0d, ball.GetVel().Y, 0.01d);
            });

            ball.SetPos(new Pos2D { X = 34533.33d, Y = 12.333d});

            Assert.Multiple(() =>
            {
                Assert.AreEqual(10.222d, ball.GetRadius(), 0.01d);
                Assert.AreEqual(34533.33d, ball.GetPos().X, 0.01d);
                Assert.AreEqual(12.333d, ball.GetPos().Y, 0.01d);
                Assert.AreEqual(0d, ball.GetVel().X, 0.01d);
                Assert.AreEqual(0d, ball.GetVel().Y, 0.01d);
            });

            ball.SetVel(new Pos2D { X = 0.005d, Y = 1.12d });

            Assert.Multiple(() =>
            {
                Assert.AreEqual(10.222d, ball.GetRadius(), 0.01d);
                Assert.AreEqual(34533.33d, ball.GetPos().X, 0.01d);
                Assert.AreEqual(12.333d, ball.GetPos().Y, 0.01d);
                Assert.AreEqual(0.005d, ball.GetVel().X, 0.001d);
                Assert.AreEqual(1.12d, ball.GetVel().Y, 0.01d);
            });
        }

        [Test, RequiresThread]
        public void PositionChangeTest()
        {
            IBall ball = new Ball(100L, 0d, new Pos2D { X = 0d, Y = 0d }, new Pos2D { X = 1d, Y = 1d });

            Assert.NotNull(ball);

            ball.StartThread();

            Thread.Sleep(10);

            Pos2D pos = ball.GetPos();

            Assert.Multiple(() =>
            {
                Assert.AreNotEqual(0d, pos.X);
                Assert.AreNotEqual(0d, pos.Y);
            });

            ball.Dispose();
        }

        [Test, RequiresThread]
        public void OnPositionChangeEventTest()
        {
            bool eventRaised = false;

            IBall ball = new Ball(100L, 0d, new Pos2D { X = 0d, Y = 0d }, new Pos2D { X = 0d, Y = 0d });

            Assert.NotNull(ball);

            ball.OnPositionChange += (object source, PositionChangeEventArgs e) => { eventRaised = true; };

            ball.StartThread();

            Thread.Sleep(10);

            Assert.That(eventRaised, Is.True.After(10));

            ball.Dispose();
        }
    }
}