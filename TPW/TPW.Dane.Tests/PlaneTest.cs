namespace TPW.Dane.Tests
{
    public class PlaneTest
    {
        [Test]
        public void ConstructorAndGettersTest()
        {
            Plane plane = new(10.333d, 11d);

            Assert.That(plane, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(10.333d, plane.GetWidth(), 0.001d);
                Assert.AreEqual(11d, plane.GetHeight(), 0.01d);
            });
        }

        [Test]
        public void SettersTest()
        {
            Plane plane = new(0d, 0d);

            Assert.That(plane, Is.Not.Null);

            plane.SetWidth(90d);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(90d, plane.GetWidth(), 0.01d);
                Assert.AreEqual(0d, plane.GetHeight(), 0.01d);
            });

            plane.SetHeight(110d);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(90d, plane.GetWidth(), 0.01d);
                Assert.AreEqual(110d, plane.GetHeight(), 0.01d);
            });
        }
    }
}
