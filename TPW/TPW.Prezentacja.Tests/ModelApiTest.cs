using TPW.Dane;
using TPW.Prezentacja.Model;

namespace TPW.Prezentacja.Tests
{
    public class ModelApiTest
    {
        ModelApiBase? api;

        [SetUp]
        public void Setup()
        {
            api = new ModelApi(null);
        }

        [Test]
        public void ConstructorAndGettersTest()
        {
            ModelApiBase testApi = new ModelApi(null);
            Assert.IsNotNull(testApi);
            Assert.IsNotNull(testApi.Balls);
            Assert.AreEqual(0, testApi.Balls.Count);
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
        public void GenerateBallsTest()
        {
            Assert.IsNotNull(api);
            api.PlaneHeight = 90d;
            api.PlaneWidth = 100d;
            Assert.AreEqual(90d, api.PlaneHeight, 0.01d);
            Assert.AreEqual(100d, api.PlaneWidth, 0.01d);

            Assert.IsNotNull(api.Balls);
            Assert.AreEqual(0, api.Balls.Count);

            uint count = 10;

            api.GenerateBalls(count, 0.1d, 1d, 6d);

            Assert.AreEqual(count, api.Balls.Count);

            Array balls = api.Balls.ToArray();

            for (uint i = 0; i < count; ++i)
            {
                //Brush
                Assert.IsNotNull(((IModelBall)balls.GetValue((int)i)).Color);

                // CanvasPos
                Assert.LessOrEqual(((IModelBall)balls.GetValue((int)i)).CanvasPos.X, 100d);
                Assert.GreaterOrEqual(((IModelBall)balls.GetValue((int)i)).CanvasPos.X, 0d);
                Assert.LessOrEqual(((IModelBall)balls.GetValue((int)i)).CanvasPos.Y, 90d);
                Assert.GreaterOrEqual(((IModelBall)balls.GetValue((int)i)).CanvasPos.Y, 0d);
            }
        }

        [Test]
        public void StartTest()
        {
            Assert.IsNotNull(api);
            api.PlaneHeight = 90d;
            api.PlaneWidth = 100d;
            Assert.AreEqual(90d, api.PlaneHeight, 0.01d);
            Assert.AreEqual(100d, api.PlaneWidth, 0.01d);

            Assert.IsNotNull(api.Balls);
            Assert.AreEqual(0, api.Balls.Count);

            api.GenerateBalls(1, 0.1d, 1d, 6d);

            Assert.IsNotNull(api.Balls);
            Assert.AreEqual(1, api.Balls.Count);

            Pos2D startPos = ((IModelBall)api.Balls.ToArray().GetValue(0)).CanvasPos;

            api.Start();

            Thread.Sleep(10);

            Pos2D pos = ((IModelBall)api.Balls.ToArray().GetValue(0)).CanvasPos;

            Assert.Multiple(() =>
            {
                Assert.AreNotEqual(startPos.X, pos.X);
                Assert.AreNotEqual(startPos.Y, pos.Y);
            });

            api.Stop();
        }

        [Test]
        public void GetApiTest()
        {
            Assert.NotNull(api);

            ModelApiBase api2 = api ?? ModelApiBase.GetApi();

            Assert.NotNull(api2);
            Assert.AreSame(api, api2);

            api2 = ModelApiBase.GetApi();

            Assert.NotNull(api2);
            Assert.AreNotSame(api, api2);
        }
    }
}