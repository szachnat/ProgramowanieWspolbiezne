using System.Windows.Media;
using TPW.Dane;
using TPW.Prezentacja.Model;

namespace TPW.Prezentacja.Tests
{
    public class ModelBallTest
    {
        [Test]
        public void ConstructorAndGettersTest()
        {
            ModelBall m_ball = new(new Ball(100L, 10d, new Pos2D { X = 0d, Y = 4d }, new Pos2D { X = 1d, Y = 1d }), Brushes.Fuchsia);

            Assert.IsNotNull(m_ball);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(Brushes.Fuchsia, m_ball.Color);
                Assert.AreEqual(20d, m_ball.Diameter, 0.01d);
                Assert.AreEqual(-10d, m_ball.CanvasPos.X, 0.01d);
                Assert.AreEqual(-6d, m_ball.CanvasPos.Y, 0.01d);
            });
        }

        [Test]
        public void CanvasPosSetterTest()
        {
            ModelBall m_ball = new(new Ball(100L, 10d, new Pos2D { X = 0d, Y = 4d }, new Pos2D { X = 1d, Y = 1d }), Brushes.Fuchsia);

            Assert.IsNotNull(m_ball);
            Assert.AreEqual(-10d, m_ball.CanvasPos.X, 0.01d);
            Assert.AreEqual(-6d, m_ball.CanvasPos.Y, 0.01d);

            m_ball.CanvasPos = new Pos2D { X = 0d, Y = 0d };

            Assert.AreEqual(0d, m_ball.CanvasPos.X, 0.01d);
            Assert.AreEqual(0d, m_ball.CanvasPos.Y, 0.01d);
        }
    }
}
