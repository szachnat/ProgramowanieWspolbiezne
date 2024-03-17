using Hello_World_etap0;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace testy
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Assert.AreEqual(dotestu.tesst(), "Hello world");
        }
    }
}