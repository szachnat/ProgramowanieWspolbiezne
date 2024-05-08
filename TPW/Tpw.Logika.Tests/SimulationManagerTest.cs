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

            bool IsInBall(IBall ball)
            {
                foreach (IBall b in sim.Balls)
                {
                    if (b == ball) continue;

                    double x = b.GetPos().X - ball.GetPos().X;
                    double y = b.GetPos().Y - ball.GetPos().Y;
                    double r = b.GetRadius() + ball.GetRadius();

                    if (x * x + y * y < r * r)
                    {
                        return true;
                    }
                }

                return false;
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
        public void IsInBallTest()
        {
            SimulationManager sim = new(new Plane(100d, 90d));

            Assert.IsNotNull(sim);
            Assert.IsNotNull(sim.Balls);

            sim.CreateRandomBalls(1, 10d, 1d, 6d);

            Assert.IsNotNull(sim.Balls);
            Assert.AreEqual(1, sim.Balls.Count);

            Pos2D pos = sim.Balls[0].GetPos();
            double radius = sim.Balls[0].GetRadius();

            Ball centerBall = new(111, 10d, pos, new Pos2D(10, 10));
            Ball insideBall = new(666, 10d, new Pos2D(pos.X - radius, pos.Y + radius), new Pos2D(10, 10));
            Ball rightBall = new(222, 10d, new Pos2D(pos.X - radius - 10.05, pos.Y), new Pos2D(10, 10));
            Ball leftBall = new(333, 10d, new Pos2D(pos.X + radius + 10.05, pos.Y), new Pos2D(10, 10));
            Ball topBall = new(444, 10d, new Pos2D(pos.X, pos.Y - radius - 10.05), new Pos2D(10, 10));
            Ball bottomBall = new(555, 10d, new Pos2D(pos.X, pos.Y + radius + 10.05), new Pos2D(10, 10));
            Ball someBall = new(666, 10d, new Pos2D(pos.X - radius - 20, pos.Y - radius - 20), new Pos2D(10, 10));

            var method = sim.GetType().GetMethod("IsInBall", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(method, Is.Not.Null);
            Assert.That(method.IsConstructor, Is.False);

            Assert.Multiple(() =>
            {
                Assert.That((bool)method.Invoke(sim, new object[] { centerBall }), Is.True);
                Assert.That((bool)method.Invoke(sim, new object[] { insideBall }), Is.True);
                Assert.That((bool)method.Invoke(sim, new object[] { rightBall }), Is.False);
                Assert.That((bool)method.Invoke(sim, new object[] { leftBall }), Is.False);
                Assert.That((bool)method.Invoke(sim, new object[] { topBall }), Is.False);
                Assert.That((bool)method.Invoke(sim, new object[] { bottomBall }), Is.False);
                Assert.That((bool)method.Invoke(sim, new object[] { someBall }), Is.False);
            });
        }

        [Test]
        public void CheckPlaneBordersCollisionsTest()
        {
            SimulationManager sim = new SimulationManager(new Plane(10, 10));

            Pos2D lastPos = new Pos2D { X = 4, Y = 2 };
            Pos2D lastVel = new Pos2D { X = 2, Y = -2 };
            double totalTime = 2d;
            IBall ball = new Ball(1L, 1d, new Pos2D { X = 8, Y = -2 }, new Pos2D { X = 2, Y = 2 });

            var method = sim.GetType().GetMethod("CheckPlaneBordersCollisions", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(method);

            var res = (ValueTuple<Pos2D, Pos2D, double>)method.Invoke(sim, new object?[] { ball, lastPos, lastVel, totalTime });

            Assert.NotNull(res);

            (Pos2D newLast, Pos2D newVel, double newTime) = res;

            Assert.NotNull(newLast);
            Assert.NotNull(newVel);

            Assert.AreEqual(totalTime - 0.5, newTime, 0.01d);
            Assert.AreEqual(5.0d, newLast.X, 0.01d);
            Assert.AreEqual(1.0d, newLast.Y, 0.01d);
            Assert.AreEqual(2.0d, newVel.X, 0.01d);
            Assert.AreEqual(2.0d, newVel.Y, 0.01d);
        }

        [Test]
        public void CheckBallsCollisionsTest()
        {
            SimulationManager sim = new SimulationManager(new Plane(10, 10));

            sim.CreateRandomBalls(1, 1, 0, 0);

            Assert.That(sim.Balls.Count, Is.EqualTo(1));

            Pos2D lastPos = sim.Balls[0].GetPos() + new Pos2D { X = 3, Y = 3 };
            Pos2D lastVel = new Pos2D { X = -3, Y = -3 };
            double totalTime = 2d;
            IBall ball = new Ball(1L, 1d, sim.Balls[0].GetPos() - new Pos2D { X = 3, Y = 3 }, new Pos2D { X = -3, Y = -3 });

            var method = sim.GetType().GetMethod("CheckBallsCollisions", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(method);

            var res = (ValueTuple<Pos2D, Pos2D, double>)method.Invoke(sim, new object?[] { ball, lastPos, lastVel, totalTime });

            Assert.NotNull(res);

            (Pos2D newLast, Pos2D newVel, double newTime) = res;

            Assert.NotNull(newLast);
            Assert.NotNull(newVel);

            Assert.AreEqual(totalTime - 0.528888d, newTime, 0.01d);
        }

        [Test]
        public void CheckCollisionsTest()
        {
            SimulationManager sim = new(new Plane(10, 10));

            sim.CreateRandomBalls(1, 1, 0, 0);

            Assert.That(sim.Balls.Count, Is.EqualTo(1));

            Pos2D lastPos = sim.Balls[0].GetPos() + new Pos2D { X = 3, Y = 3 };
            Pos2D lastVel = new Pos2D { X = -3, Y = -3 };
            double totalTime = 2d;
            IBall ball = new Ball(1L, 1d, sim.Balls[0].GetPos() - new Pos2D { X = 3, Y = 3 }, new Pos2D { X = -3, Y = -3 });

            var method = sim.GetType().GetMethod("CheckCollisions", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(method);

            method.Invoke(sim, new object?[] { ball, new PositionChangeEventArgs(lastPos, lastVel, totalTime) });

            Assert.AreNotEqual(lastPos, ball.GetPos());
        }
    }
}
