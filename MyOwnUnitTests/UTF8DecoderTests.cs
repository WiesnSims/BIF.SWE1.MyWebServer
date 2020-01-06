using NUnit.Framework;
using MyWebServer.src.Plugins;
using BIF.SWE1.Interfaces;
using MyWebServer;

namespace MyUnitTests
{
    [TestFixture]
    class UTF8DecoderTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void CanHandle_AllUmlauts()
        {
            string test = "%C3%A4%C3%B6%C3%BC%C3%84%C3%96%C3%9C%C3%9F+%3A";
            Assert.AreEqual(UTF8Decoder.DecodeUmlauts(test), "äöüÄÖÜß :");
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}
