using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyWebServer.src.Plugins
{
    class WebPagePlugin :IPlugin
    {
        private static readonly IDictionary<string, string> PAGES = new Dictionary<string, string>
        {
            {"/", "home.html"},
            {"/home", "home.html" },
            {"/toLower", "toLower.html" },
            {"/navi", "navi.html" },
            {"/temperatures", "temperatures.html" },
            {"/error", "error.html" }
        };

        public float CanHandle(IRequest req)
        {
            bool canHandle = PAGES.Keys.Contains(req.Url.Path) && req.Url.ParameterCount == 0 && req.Method == "GET";
            return canHandle ? 1 : 0;
        }

        public IResponse Handle(IRequest req)
        {
            return CreateWebpageResponse(req.Url.Path);
        }

        public static IResponse CreateWebpageResponse(string pageName)
        {
            string filename = PAGES[pageName];
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Response response = new Response();
            response.StatusCode = pageName == "/error" ? 404 : 200;
            response.SetContent(File.ReadAllBytes(Path.Combine(directory, @"static-files\www\pages", filename)));
            response.ContentType = "text/html";
            return response;
        }
    }
}
