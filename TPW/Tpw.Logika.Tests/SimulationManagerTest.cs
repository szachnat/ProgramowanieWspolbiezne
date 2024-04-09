using Newtonsoft.Json.Linq;
using System.Reflection;
using TPW.Dane;

namespace TPW.Logika.Tests
{
    public class SimulationManagerTest
    {
        [Test]
        public void ConstructorAndGettersTest()
        {
            SimulationManager sim = new(new Plane(100d, 90d));

            Assert.IsNotNull(sim);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(sim.Balls);
                Assert.AreEqual(0, sim.Balls.Count);
                Assert.AreEqual(90d, sim.PlaneHeight, 0.01d);
                Assert.AreEqual(100d, sim.PlaneWidth, 0.01d);
            });
        }

        [Test]
        public void SettersTest()
        {
            SimulationManager sim = new(new Plane(100d, 90d));

            Assert.IsNotNull(sim);

            // Height
            sim.PlaneHeight = 120.33d;

            Assert.AreEqual(120.33d, sim.PlaneHeight, 0.01d);

            sim.PlaneHeight = 120.33d;

            Assert.AreEqual(120.33d, sim.PlaneHeight, 0.01d);

            // Width
            sim.PlaneWidth = 33.32d;

            Assert.AreEqual(33.32d, sim.PlaneWidth, 0.01d);

            sim.PlaneHeight = 33.32d;

            Assert.AreEqual(33.32d, sim.PlaneWidth, 0.01d);
        }

        [Test]
        public void CreateRandomBallsTest()
        {
            SimulationManager sim = new(new Plane(100d, 90d));

            Assert.IsNotNull(sim);
            Assert.IsNotNull(sim.Balls);

            uint count = 10;

            sim.CreateRandomBalls(count, 10d, 1d, 6d);

            Assert.IsNotNull(sim.Balls);
            Assert.AreEqual(count, sim.Balls.Count);

            for (uint i = 0; i < count; ++i)
            {
                // Radius
                Assert.LessOrEqual(sim.Balls[(int)i].GetRadius(), 10d);
                Assert.GreaterOrEqual(sim.Balls[(int)i].GetRadius(), 0.1d);

                // Pos
                Assert.LessOrEqual(sim.Balls[(int)i].GetPos().X, 100d);
                Assert.GreaterOrEqual(sim.Balls[(int)i].GetPos().X, 0d);
                Assert.LessOrEqual(sim.Balls[(int)i].GetPos().Y, 90d);
                Assert.GreaterOrEqual(sim.Balls[(int)i].GetPos().Y, 0d);

                // Vel
                Assert.LessOrEqual(sim.Balls[(int)i].GetVel().X, 6d);
                Assert.GreaterOrEqual(sim.Balls[(int)i].GetVel().X, 1d);
                Assert.LessOrEqual(sim.Balls[(int)i].GetVel().Y, 6d);
                Assert.GreaterOrEqual(sim.Balls[(int)i].GetVel().Y, 1d);
            }
        }

        [Test, RequiresThread]
        public void StartSimulationTest()
        {
            SimulationManager sim = new(new Plane(100d, 90d));

            Assert.IsNotNull(sim);
            Assert.IsNotNull(sim.Balls);

            sim.CreateRandomBalls(1, 10d, 1d, 6d);

            Assert.IsNotNull(sim.Balls);
            Assert.AreEqual(1, sim.Balls.Count);

            Pos2D startPos = sim.Balls[0].GetPos();

            sim.StartSimulation();

            Thread.Sleep(10);

            Pos2D pos = sim.Balls[0].GetPos();

            Assert.Multiple(() =>
            {
                Assert.AreNotEqual(startPos.X, pos.X);
                Assert.AreNotEqual(startPos.Y, pos.Y);
            });

            sim.ClearBalls();

            Assert.AreEqual(0, sim.Balls.Count);
        }

        [Test]
        public void CheckCollisionsTest()
        {
            Pos2D lastPos = new()
            {
                X = 90d,
                Y = 85d
            };

            Pos2D newPos = new()
            {
                X = 158d,
                Y = 179d
            };

            Pos2D currVel = new()
            {
                X = 3.4d,
                Y = 4.7d
            };

            double radius = 4d;

            double totalTime = 20d;

            SimulationManager c = new(new Plane(100d, 90d));
            Assert.IsNotNull(c);

            IBall ball = new Ball(100L, radius, lastPos, currVel);
            Assert.IsNotNull(ball);

            var method = c.GetType().GetMethod("CheckCollisions", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(method, Is.Not.Null);
            Assert.That(method.IsConstructor, Is.False);

            method.Invoke(c, new object[] { ball, new PositionChangeEventArgs(lastPos, newPos, totalTime) });

            Assert.Multiple(() =>
            {
                Assert.AreEqual(34d, ball.GetPos().X, 0.01d);
                Assert.AreEqual(15d, ball.GetPos().Y, 0.01d);
                Assert.AreEqual(-3.4d, ball.GetVel().X, 0.01d);
                Assert.AreEqual(4.7d, ball.GetVel().Y, 0.01d);
            });
        }

        [Test]
        public void CheckCollisionsRecursionTest()
        {
            Pos2D lastPos = new()
            {
                X = 90d,
                Y = 85d
            };

            Pos2D newPos = new()
            {
                X = 158d,
                Y = 179d
            };

            Pos2D currVel = new()
            {
                X = 3.4d,
                Y = 4.7d
            };

            double radius = 4d;

            double totalTime = 20d;

            SimulationManager c = new(new Plane(100d, 90d));
            Assert.IsNotNull(c);
            var method = c.GetType().GetMethod("CheckCollisionsRecursion", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(method, Is.Not.Null);
            Assert.That(method.IsConstructor, Is.False);

            ValueTuple<Pos2D, Pos2D> test = (ValueTuple<Pos2D, Pos2D>)method.Invoke(c, new object[] { lastPos, newPos, currVel, radius, totalTime, (uint)3, (uint)0 });

            List<Pos2D> list = new()
            {
                test.Item1, test.Item2
            };

            Assert.AreEqual(2, list.Count);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(test.Item1, list[0]);
                Assert.AreEqual(test.Item2, list[1]);
            });

            Assert.Multiple(() =>
            {
                Assert.AreEqual(34d, list[0].X, 0.01d);
                Assert.AreEqual(15d, list[0].Y, 0.01d);
                Assert.AreEqual(-3.4d, list[1].X, 0.01d);
                Assert.AreEqual(4.7d, list[1].Y, 0.01d);
            });
        }
    }
}
