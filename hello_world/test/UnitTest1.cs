using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using HelloWorld;

namespace HelloWorldTest
{
    [TestClass]
    public class UnitTest1
    {
        private const string expected = "Hello World!";
        [TestMethod]
        public void HelloWorldTest()
        {
            using (var line = new StringWriter())
            {
                Console.SetOut(line);
                Hello.Main();
                var result = line.ToString().Trim();
                Assert.AreEqual(expected, result);
            }
        }
    }
}