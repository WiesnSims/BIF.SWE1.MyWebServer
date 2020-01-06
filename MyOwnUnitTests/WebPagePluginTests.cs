using NUnit.Framework;
using MyWebServer.src.Plugins;
using BIF.SWE1.Interfaces;
using MyWebServer;

namespace MyUnitTests
{
    [TestFixture]
    class WebPagePluginTests
    {

        WebPagePlugin _WebPagePlugin;
         
        [SetUp]
        public void Setup()
        {
            _WebPagePlugin = new WebPagePlugin();
        }

        [Test]
        [TestCase("/")]
        [TestCase("/home")]
        [TestCase("/navi")]
        [TestCase("/toLower")]
        [TestCase("/error")]
        [TestCase("/temperatures")]
        public void CanHandle_ValidUrl_Returns1(string url)
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream(url));
            float result = _WebPagePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 1.0f);
        }

        [Test]
        public void CanHandle_ValidURLWithParameter_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/home?id=1&text=hallo"));
            float result = _WebPagePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }
        [Test]
        public void CanHandle_InvalidURL_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/xxxx"));
            float result = _WebPagePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }


        [TearDown]
        public void TearDown()
        {
            _WebPagePlugin = null;
        }
    }
}
