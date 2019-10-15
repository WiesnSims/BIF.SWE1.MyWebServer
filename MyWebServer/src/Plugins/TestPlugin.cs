using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer.src.Plugins
{
    class TestPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            float canHandle = 0;

            if (req.IsValid && (req.Url.Segments.Contains("test") || req.Url.Parameter.FirstOrDefault(x => x.Key == "test_plugin").Value == "true")) canHandle = 1;
            if (req.Url.RawUrl == "/") canHandle = 0.1f;

            return canHandle;
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response();
            response.StatusCode = 200;
            response.SetContent("<b>Test-Plugin-Content</b>");
            response.ContentType = "text/html";
            return response;
        }
    }
}
