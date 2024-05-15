using Microsoft.VisualBasic;
using System.Reflection;
using TPW.Dane;

namespace TPW.Dane.Tests
{
    public class BallLoggerTest
    {
        [Test]
        public void LoggingTest()
        {
            BallLogger.StartLogging();

            var method = typeof(BallLogger).GetMethod("GetLogsDirPath", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            Assert.That(method.IsConstructor, Is.False);

            string rootFolder = Assembly.GetExecutingAssembly().Location + "\\..\\" + method.Invoke(null, new object[] { });

            BallLogger.Log("Message", LogType.INFO);

            Thread.Sleep(2000);

            Assert.That(File.Exists(rootFolder + "\\LOG0.log"), Is.True);

            string[] s = File.ReadAllLines(rootFolder + "\\LOG0.log");

            Assert.That(s[0].Split('[')[^1], Is.EqualTo("INFO] Message"));

            BallLogger.StopLogging();

            File.Delete(rootFolder + "\\LOG0.log");

            Assert.That(File.Exists(rootFolder + "\\LOG0.log"), Is.False);
        }
    }
}
