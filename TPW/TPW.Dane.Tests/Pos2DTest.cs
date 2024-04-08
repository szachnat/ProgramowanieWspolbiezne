namespace TPW.Dane.Tests
{
    public class Pos2DTest
    {
        readonly double x = 0.12d;
        readonly double y = 12.22d;

        [Test]
        public void ConstructorTest()
        {
            Pos2D pos = new(x, y);

            //Assert.That(pos, Is.Not.Null);
            Assert.AreEqual(x, pos.X);
            Assert.AreEqual(y, pos.Y);
        }

        [Test]
        public void SetXTest()
        {
            Pos2D pos = new(0d, 0d)
            {
                X = x
            };

            //Assert.That(pos, Is.Not.Null);
            Assert.AreEqual(x, pos.X);
        }

        [Test]
        public void SetYTest()
        {
            Pos2D pos = new(0d, 0d)
            {
                Y = y
            };

            Assert.AreEqual(y, pos.Y);
        }

        [Test]
        public void MulTest()
        {
            Pos2D pos = new(x, y);
            Pos2D mulPos;

            //Assert.That(pos, Is.Not.Null);

            Assert.Multiple(() =>
            {
                int mul = 10;
                mulPos = pos * mul;
                //Assert.That(mulPos, Is.Not.Null);
                Assert.AreEqual(x * mul, mulPos.X, 0.01d);
                Assert.AreEqual(y * mul, mulPos.Y, 0.01d);
            });

            Assert.Multiple(() =>
            {
                float mul = 12;
                mulPos = pos * mul;
                //Assert.That(mulPos, Is.Not.Null);
                Assert.AreEqual(x * mul, mulPos.X, 0.01d);
                Assert.AreEqual(y * mul, mulPos.Y, 0.01d);
            });

            Assert.Multiple(() =>
            {
                double mul = 8;
                mulPos = pos * mul;
                //Assert.That(mulPos, Is.Not.Null);
                Assert.AreEqual(x * mul, mulPos.X, 0.01d);
                Assert.AreEqual(y * mul, mulPos.Y, 0.01d);
            });
        }

        [Test]
        public void AddTest()
        {
            Pos2D pos1 = new(x, y);
            Pos2D pos2 = new(11d, 2d);
            Pos2D addPos = pos1 + pos2;

            /*Assert.Multiple(() =>
            {
                Assert.That(pos1, Is.Not.Null);
                Assert.That(pos2, Is.Not.Null);
                Assert.That(addPos, Is.Not.Null);
            });*/

            Assert.Multiple(() =>
            {
                Assert.AreEqual(x + 11d, addPos.X, 0.01d);
                Assert.AreEqual(y + 2d, addPos.Y, 0.01d);
            });
        }

        [Test]
        public void SubTest()
        {
            Pos2D pos1 = new(x, y);
            Pos2D pos2 = new(11d, 2d);
            Pos2D subPos = pos1 - pos2;

            /*Assert.Multiple(() =>
            {
                Assert.That(pos1, Is.Not.Null);
                Assert.That(pos2, Is.Not.Null);
                Assert.That(subPos, Is.Not.Null);
            });*/

            Assert.Multiple(() =>
            {
                Assert.AreEqual(x - 11d, subPos.X, 0.01d);
                Assert.AreEqual(y - 2d, subPos.Y, 0.01d);
            });
        }

        [Test]
        public void EqualOperatorTest()
        {
            Pos2D pos1 = new(x, y);
            Pos2D pos2 = new(x, y);
            Pos2D pos3 = new(0.4d, 11d);

            /*Assert.Multiple(() =>
            {
                Assert.That(pos1, Is.Not.Null);
                Assert.That(pos2, Is.Not.Null);
                Assert.That(pos3, Is.Not.Null);
            });*/

            Assert.Multiple(() =>
            {
                Assert.That(pos1 == pos2, Is.True);
                Assert.That(pos1 == pos3, Is.False);
                Assert.That(pos2 == pos3, Is.False);
            });
        }

        [Test]
        public void NotEqualOperatorTest()
        {
            Pos2D pos1 = new(x, y);
            Pos2D pos2 = new(x, y);
            Pos2D pos3 = new(0.4d, 11d);

            /*Assert.Multiple(() =>
            {
                Assert.That(pos1, Is.Not.Null);
                Assert.That(pos2, Is.Not.Null);
                Assert.That(pos3, Is.Not.Null);
            });*/

            Assert.Multiple(() =>
            {
                Assert.That(pos1 != pos2, Is.False);
                Assert.That(pos1 != pos3, Is.True);
                Assert.That(pos2 != pos3, Is.True);
            });
        }

        [Test]
        public void EqualAndHashTest()
        {
            Pos2D pos1 = new(x, y);
            Pos2D pos2 = new(x, y);
            Pos2D pos3 = new(0.4d, 11d);
            int a = 2;

            /*Assert.Multiple(() =>
            {
                Assert.That(pos1, Is.Not.Null);
                Assert.That(pos2, Is.Not.Null);
                Assert.That(pos3, Is.Not.Null);
            });*/

            Assert.That(pos1.Equals(a), Is.False);

            Assert.Multiple(() =>
            {
                Assert.AreNotSame(pos1, pos2);
                Assert.That(pos1.GetHashCode() == pos2.GetHashCode(), Is.True);
                Assert.That(pos1.Equals(pos2), Is.True);
            });

            Assert.Multiple(() =>
            {
                Assert.AreNotSame(pos1, pos3);
                Assert.That(pos1.GetHashCode() == pos3.GetHashCode(), Is.False);
                Assert.That(pos1.Equals(pos3), Is.False);
            });

            Assert.Multiple(() =>
            {
                Assert.That(pos2.GetHashCode() == pos1.GetHashCode(), Is.True);
                Assert.That(pos2.GetHashCode() == pos3.GetHashCode(), Is.False);
            });
        }
    }
}