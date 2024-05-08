using TPW.Dane;
using TPW.Logika;

namespace Tpw.Logika.Tests
{
    public class CollisionManagerTest
    {
        [Test]
        public void TimeOfCollisionWithStaticLineTest()
        {
            Pos2D startPos = new()
            {
                X = 0d,
                Y = 0d
            };

            Pos2D vel = new()
            {
                X = 1d,
                Y = 1d
            };

            Pos2D linePoint1 = new()
            {
                X = 2.5d,
                Y = 0d
            };

            Pos2D linePoint2 = new()
            {
                X = 7.34d,
                Y = 9.68d
            };

            Assert.AreEqual(5d, CollisionManager.TimeOfCollisionWithStaticLine(startPos, vel, linePoint1, linePoint2), 0.01d);
        }

        [Test]
        public void TimeOfCollisionWithMovingLineTest()
        {
            Pos2D startPos = new()
            {
                X = 0d,
                Y = 0d
            };

            Pos2D vel = new()
            {
                X = 1d,
                Y = 1d
            };

            Pos2D linePoint1 = new()
            {
                X = 2.5d,
                Y = 0d
            };

            Pos2D linePoint2 = new()
            {
                X = 7.34d,
                Y = 9.68d
            };

            Pos2D lineVel = new()
            {
                X = 0.25d,
                Y = 0.25d
            };

            Assert.AreEqual(6.666d, CollisionManager.TimeOfCollisionWithMovingLine(startPos, vel, linePoint1, linePoint2, lineVel), 0.01d);
        }

        [Test]
        public void TimeOfCollisionWithStaticCircleTest()
        {
            Pos2D objPos = new(0, 20);
            Pos2D objVel = new(5, -2);

            Pos2D circlePos = new(15, 15);
            double radius = 5;

            Assert.AreEqual(2.0188d, CollisionManager.TimeOfCollisionWithStaticCircle(objPos, objVel, circlePos, radius), 0.01d);
        }

        [Test]
        public void TimeOfCollisionWithMovingCircleTest()
        {
            Pos2D objPos = new(0, 20);
            Pos2D objVel = new(5, -2);

            Pos2D circlePos = new(15, 15);
            Pos2D circleVel = new(-5, 0);
            double radius = 5;

            Assert.AreEqual(1.0875d, CollisionManager.TimeOfCollisionWithMovingCircle(objPos, objVel, circlePos, circleVel, radius), 0.01d);

            circleVel.X = 2;
            Assert.AreEqual(3.46d, CollisionManager.TimeOfCollisionWithMovingCircle(objPos, objVel, circlePos, circleVel, radius), 0.01d);
        }

        [Test]
        public void VelocitiesAfterCollisionTest()
        {
            double m1 = 2;
            Pos2D v1 = new()
            {
                X = 2,
                Y = 2
            };

            double m2 = 1;
            Pos2D v2 = new()
            {
                X = -3,
                Y = 5
            };

            (Pos2D v3, Pos2D v4) = CollisionManager.VelocitiesAfterCollision(v1, m1, v2, m2);

            Assert.NotNull(v3);
            Assert.NotNull(v4);

            Assert.AreEqual(-1.333d, v3.X, 0.01d);
            Assert.AreEqual(4.000d, v3.Y, 0.01d);
            Assert.AreEqual(3.666d, v4.X, 0.01d);
            Assert.AreEqual(1.000d, v4.Y, 0.01d);
        }

        [Test]
        public void VelocitiesAfterBallsCollisionTest()
        {
            double m1 = 2;
            Pos2D v1 = new()
            {
                X = 2,
                Y = 2
            };

            Pos2D c1 = new()
            {
                X = 0,
                Y = 0
            };

            double m2 = 1;
            Pos2D v2 = new()
            {
                X = -3,
                Y = 5
            };

            Pos2D c2 = new()
            {
                X = 2,
                Y = 2
            };

            (Pos2D v3, Pos2D v4) = CollisionManager.VelocitiesAfterBallsCollision(v1, m1, c1, v2, m2, c2);

            Assert.NotNull(v3);
            Assert.NotNull(v4);

            Assert.AreEqual(-0.666d, v3.X, 0.01d);
            Assert.AreEqual(4.666d, v3.Y, 0.01d);
            Assert.AreEqual(2.333d, v4.X, 0.01d);
            Assert.AreEqual(-0.333d, v4.Y, 0.01d);
        }
    }
}