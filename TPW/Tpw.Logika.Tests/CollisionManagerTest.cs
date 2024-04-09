using TPW.Dane;
using TPW.Logika;

namespace Tpw.Logika.Tests
{
    public class CollisionManagerTest
    {
        [Test]
        public void TimeOfCollisionWithLineTest()
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

            Assert.AreEqual(5d, CollisionManager.TimeOfCollisionWithLine(startPos, vel, linePoint1, linePoint2), 0.01d);
        }
    }
}