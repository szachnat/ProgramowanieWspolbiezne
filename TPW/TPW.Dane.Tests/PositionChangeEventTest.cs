using System.Numerics;

namespace TPW.Dane.Tests
{
    public class PositionChangeEventTest
    {
        [Test]
        public void ConstructorTest()
        {
            Pos2D pos = new(1.1d, 2.0d);
            Pos2D vel = new(3.4d, 3.3d);
            double time = 0.123d;

            PositionChangeEventArgs eventArgs = new(pos, vel, time);

            Assert.Multiple(() =>
            {
                //Assert.That(pos, Is.Not.Null);
                //Assert.That(vel, Is.Not.Null);
                Assert.That(eventArgs, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                Assert.That(eventArgs.LastPos, Is.EqualTo(pos));
                Assert.That(eventArgs.Vel, Is.EqualTo(vel));
                Assert.AreEqual(eventArgs.ElapsedSeconds, time, 0.001d);
            });
        }
    }
}
