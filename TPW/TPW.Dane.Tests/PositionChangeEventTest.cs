using System.Numerics;

namespace TPW.Dane.Tests
{
    public class PositionChangeEventTest
    {
        [Test]
        public void ConstructorTest()
        {
            Pos2D pos1 = new(1.1d, 2.0d);
            Pos2D pos2 = new(3.4d, 3.3d);
            double time = 0.123d;

            PositionChangeEventArgs eventArgs = new(pos1, pos2, time);

            Assert.Multiple(() =>
            {
                //Assert.That(pos1, Is.Not.Null);
                //Assert.That(pos2, Is.Not.Null);
                Assert.That(eventArgs, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                Assert.That(eventArgs.LastPos, Is.EqualTo(pos1));
                Assert.That(eventArgs.NewPos, Is.EqualTo(pos2));
                Assert.AreEqual(eventArgs.ElapsedSeconds, time, 0.001d);
            });
        }
    }
}
