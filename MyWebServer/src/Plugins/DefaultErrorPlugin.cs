using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyWebServer.src.Plugins
{
    class DefaultErrorPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            return 0.0001f;
        }

        public IResponse Handle(IRequest req)
        {
            return WebPagePlugin.CreateWebpageResponse("/error");
        }
    }
}