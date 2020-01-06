using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MyWebServer.src.Plugins;
using BIF.SWE1.Interfaces;
using MyWebServer;

namespace MyUnitTests
{
    [TestFixture]
    public class TemperaturePluginTests
    {
        TemperaturePlugin _TemperaturePlugin;
        [SetUp]
        public void SetUp()
        {
            _TemperaturePlugin = new TemperaturePlugin();
        }



        [Test]
        [TestCase("/temperatures?from=2019-01-15&until=2019-01-20")]
        [TestCase("/getTemperatures?from=2019-01-15&until=2019-01-20")]
        public void CanHandle_ValidURLValidParameters_Returns1(string url)
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream(url));
            float result = _TemperaturePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 1.0f);
        }

        [Test]
        public void CanHandle_ValidURLInvalidParameters1_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/getTemperatures?from=2019-01-15&until=2019"));
            float result = _TemperaturePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }

        [Test]
        public void CanHandle_ValidURLInvalidParameters2_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/getTemperatures?until=2019-01-20"));
            float result = _TemperaturePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }

        [Test]
        public void CanHandle_ValidURLNoParameters_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/getTemperatures"));
            float result = _TemperaturePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }

        [Test]
        public void CanHandle_ValidURLIWrongFormattedParameters1_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/getTemperatures?from=15x01x2019&until=20x01x2019"));
            float result = _TemperaturePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }

        [Test]
        public void CanHandle_InValidURL_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/xxxx?from=2019-01-15&until=2019-01-20"));
            float result = _TemperaturePlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }



        [TearDown]
        public void TearDown()
        {
            _TemperaturePlugin = null;
        }
    }
}
