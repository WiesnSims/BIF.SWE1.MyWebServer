using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyWebServer.src.Plugins
{
    public class StaticFilePlugin : IPlugin
    {
        private string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly IDictionary<string, string> MIME_TYPES = new Dictionary<string, string>
        {
            {"html", "text/html"},
            {"txt", "text/plain" },
            {"css", "text/css" },
            {"js", "text/javascript" },
            {"jpg", "image/jpeg" }
        };

        public float CanHandle(IRequest req)
        {
            string filename = directory + "/static-files" + req.Url.RawUrl;
            bool canHandle = File.Exists(filename);

            return canHandle ? 0.7f : 0;
        }

        public IResponse Handle(IRequest req)
        {
            string projectDirectory = Directory.GetParent(directory).Parent.FullName;
            string filename = projectDirectory + "/static-files" + req.Url.RawUrl;

            Response response = new Response();
            response.StatusCode = 200;
            response.SetContent(File.ReadAllBytes(filename));
            response.ContentType = MIME_TYPES[req.Url.RawUrl.Split('.').Last()] ?? "text/plain";
            return response;
        }
    }
}
