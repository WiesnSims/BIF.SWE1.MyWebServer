using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer.src.Plugins
{
    public class ToLowerPlugin : IPlugin
    {
        private static readonly string ERROR_RESPONSE = "Bitte Text eingeben.";

        public float CanHandle(IRequest req)
        {
            if (req.Url.Path != "/toLower" || req.Method != "POST") return 0;
            if (!req.ContentString.Contains("text=")) return 0;
            if (req.ContentString.Split('=').Count() != 2) return 0;
            return 1;
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response();
            response.StatusCode = 200;
            string[] parts = req.ContentString.Split('=');
            if (!String.IsNullOrEmpty(parts[1]))
            {
                response.SetContent(parts[1].ToLower());
            }
            else
            {
                response.SetContent(ERROR_RESPONSE);
            }
            response.ContentType = "text/plain";
            return response;
        }
    }
}
