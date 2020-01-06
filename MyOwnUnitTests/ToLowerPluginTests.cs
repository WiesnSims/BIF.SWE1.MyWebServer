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
    class ToLowerPluginTests
    {
        ToLowerPlugin _ToLowerPlugin;

        [SetUp]
        public void SetUp()
        {
            _ToLowerPlugin = new ToLowerPlugin();
        }

        [Test]
        public void CanHandle_ValidURLNoContent_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/toLower", method: "POST", body: " "));
            float result = _ToLowerPlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }
        [Test]
        public void CanHandle_ValidURLInvalidMethod_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/toLower", body: "text=hallo"));
            float result = _ToLowerPlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }
        [Test]
        public void CanHandle_InValidURLValidMethod_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/xxxx", method: "POST", body: "text=hallo"));
            float result = _ToLowerPlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }
        [Test]
        public void CanHandle_ValidURLInvalidContent_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/toLower", method: "POST", body: "words="));
            float result = _ToLowerPlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }
        [Test]
        public void CanHandle_ValidURLValidContent_Returns1()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/toLower", method: "POST", body: "text=Hallo"));
            float result = _ToLowerPlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 1f);
        }
        [Test]
        public void CanHandle_ValidURLInvalidContent1_Returns0()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/toLower", method: "POST", body: "text=Hallo=Test"));
            float result = _ToLowerPlugin.CanHandle(simpleRequest);
            Assert.AreEqual(result, 0.0f);
        }
        [Test]
        public void Handle_InvalidContent_ReturnsErrorMessage()
        {
            Request simpleRequest = new Request(MyRequestHelper.GetValidRequestStream("/toLower", method: "POST", body: "text="));
            IResponse result = _ToLowerPlugin.Handle(simpleRequest);
            Assert.AreEqual(result.ContentLength, "Bitte geben Sie einen Text ein.".Length);
        }

        [TearDown]
        public void TearDown()
        {
            _ToLowerPlugin = null;
        }
    }
}
