using NUnit.Framework;
using MyWebServer.src.Plugins;
using BIF.SWE1.Interfaces;
using MyWebServer;
namespace MyUnitTests
{
    [TestFixture]
    class NaviPluginTests
    {

        NaviPlugin _NaviPlugin;

        [SetUp]
        public void SetUp()
        {
            _NaviPlugin = new NaviPlugin();
        }

        [Test]
        public void CanHandle_InvalidUrl_is0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/naviquery", method: "POST", body: "street=teststreet"));
            float result = _NaviPlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0);
        }

        [Test]
        public void CanHandle_ValidURLWithoutStreet()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/naviQuery", method: "POST", body: "street="));
            float result = _NaviPlugin.CanHandle(simpleRequest);
            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        public void CanHandle_ValidURLValidMethodInvalidContent2_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/naviQuery", method: "POST", body: "refresh"));
            float result = _NaviPlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }


        [TearDown]
        public void TearDown()
        {
            _NaviPlugin = null;
        }
    }
}
