using TPW.Dane;

namespace TPW.Logika.Tests
{
    public class LogikaApiTest
    {
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        LogikaApiBase? api;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public void Setup()
        {
             api = new LogikaApi(null);
        }

        [Test]
        public void ConstructorAndGettersTest()
        {
            LogikaApiBase testApi = new LogikaApi(null);
            Assert.IsNotNull(testApi);
            Assert.IsNotNull(testApi.Balls);
            Assert.AreEqual(0, testApi.Balls.Count());
            Assert.AreEqual(0d, testApi.PlaneWidth, 0.01d);
            Assert.AreEqual(0d, testApi.PlaneHeight, 0.01d);
        }

        [Test]
        public void SettersTest()
        {
            Assert.IsNotNull(api);

            Assert.AreEqual(0d, api.PlaneHeight, 0.01d);
            Assert.AreEqual(0d, api.PlaneWidth, 0.01d);

            api.PlaneHeight = 90d;
            api.PlaneWidth = 100d;

            Assert.AreEqual(90d, api.PlaneHeight, 0.01d);
            Assert.AreEqual(100d, api.PlaneWidth, 0.01d);
        }

        [Test]
        public void GenerateRandomBallsTest()
        {
            Assert.IsNotNull(api);
            api.PlaneHeight = 90d;
            api.PlaneWidth = 100d;
            Assert.AreEqual(90d, api.PlaneHeight, 0.01d);
            Assert.AreEqual(100d, api.PlaneWidth, 0.01d);

            Assert.IsNotNull(api.Balls);
            Assert.AreEqual(0, api.Balls.Count());

            uint count = 10;

            api.GenerateRandomBalls(count, 10d, 1d, 6d);

            Assert.AreEqual(count, api.Balls.Count());

            Array balls = api.Balls.ToArray();

            for (uint i = 0; i < count; ++i)
            {
                // Radius
                Assert.LessOrEqual(((IBall)balls.GetValue((int)i)).GetRadius(), 10d);
                Assert.GreaterOrEqual(((IBall)balls.GetValue((int)i)).GetRadius(), 0.1d);

                // Pos
                Assert.LessOrEqual(((IBall)balls.GetValue((int)i)).GetPos().X, 100d);
                Assert.GreaterOrEqual(((IBall)balls.GetValue((int)i)).GetPos().X, 0d);
                Assert.LessOrEqual(((IBall)balls.GetValue((int)i)).GetPos().Y, 90d);
                Assert.GreaterOrEqual(((IBall)balls.GetValue((int)i)).GetPos().Y, 0d);

                // Vel
                Assert.LessOrEqual(((IBall)balls.GetValue((int)i)).GetVel().X, 6d);
                Assert.GreaterOrEqual(((IBall)balls.GetValue((int)i)).GetVel().X, 1d);
                Assert.LessOrEqual(((IBall)balls.GetValue((int)i)).GetVel().Y, 6d);
                Assert.GreaterOrEqual(((IBall)balls.GetValue((int)i)).GetVel().Y, 1d);
            }
        }

        [Test]
        public void StartSimulationTest()
        {
            Assert.IsNotNull(api);
            api.PlaneHeight = 90d;
            api.PlaneWidth = 100d;
            Assert.AreEqual(90d, api.PlaneHeight, 0.01d);
            Assert.AreEqual(100d, api.PlaneWidth, 0.01d);

            Assert.IsNotNull(api.Balls);
            Assert.AreEqual(0, api.Balls.Count());

            api.GenerateRandomBalls(1, 10d, 1d, 6d);

            Assert.IsNotNull(api.Balls);
            Assert.AreEqual(1, api.Balls.Count());

            Pos2D startPos = ((IBall)api.Balls.ToArray().GetValue(0)).GetPos();

            api.StartSimulation();

            Thread.Sleep(10);

            Pos2D pos = ((IBall)api.Balls.ToArray().GetValue(0)).GetPos();

            Assert.Multiple(() =>
            {
                Assert.AreNotEqual(startPos.X, pos.X);
                Assert.AreNotEqual(startPos.Y, pos.Y);
            });

            api.StopSimulation();
        }

        [Test]
        public void GetApiTest()
        {
            Assert.NotNull(api);

            LogikaApiBase api2 = api ?? LogikaApiBase.GetApi();

            Assert.NotNull(api2);
            Assert.AreSame(api, api2);

            api2 = LogikaApiBase.GetApi();

            Assert.NotNull(api2);
            Assert.AreNotSame(api, api2);
        }
    }
}
