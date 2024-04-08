namespace TPW.Dane.Tests
{
    public class DaneApiTest
    {
        DaneApiBase? api;

        [SetUp]
        public void Setup()
        {
            api = new DaneApi();
        }

        [Test]
        public void CreateBallTest()
        {
            Assert.NotNull(api);

            IBall ball = api.CreateRandomBall(100d, new Pos2D { X = 0d, Y = 0d }, new Pos2D { X = 10d, Y = 20d }, 0d, 100d);

            Assert.NotNull(ball);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(ball.GetRadius(), 100d);

                Assert.LessOrEqual(ball.GetVel().X, 100d);
                Assert.GreaterOrEqual(ball.GetVel().X, 0d);
                Assert.LessOrEqual(ball.GetVel().Y, 100d);
                Assert.GreaterOrEqual(ball.GetVel().Y, 0d);

                Assert.LessOrEqual(ball.GetPos().X, 100d);
                Assert.GreaterOrEqual(ball.GetPos().X, -90d);
                Assert.LessOrEqual(ball.GetPos().Y, 100d);
                Assert.GreaterOrEqual(ball.GetPos().Y, -80d);
            });
        }

        [Test]
        public void CreatePlaneTest()
        {
            Assert.NotNull(api);

            Plane plane = api.CreatePlane(100d, 0.01d);

            Assert.NotNull(plane);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(100d, plane.GetWidth(), 0.01d);
                Assert.AreEqual(0.01d, plane.GetHeight(), 0.01d);
            });
        }

        [Test]
        public void GetApiTest()
        {
            Assert.NotNull(api);

            DaneApiBase api2 = api ?? DaneApiBase.GetApi();

            Assert.NotNull(api2);
            Assert.AreSame(api, api2);

            api2 = DaneApiBase.GetApi();

            Assert.NotNull(api2);
            Assert.AreNotSame(api, api2);
        }
    }
}
