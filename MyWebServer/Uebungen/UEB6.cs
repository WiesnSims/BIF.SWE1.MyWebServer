using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using MyWebServer;
using MyWebServer.src.Plugins;

namespace Uebungen
{
    public class UEB6
    {
        public void HelloWorld()
        {
        }

        public IPluginManager GetPluginManager()
        {
            return new PluginManager();
        }

        public IRequest GetRequest(System.IO.Stream network)
        {
            return new Request(network);
        }

        public string GetNaviUrl()
        {
            return "/naviQuery";
        }

        public IPlugin GetNavigationPlugin()
        {
            return new NaviPlugin();
        }

        public IPlugin GetTemperaturePlugin()
        {
            return new TemperaturePlugin();
        }

        public string GetTemperatureRestUrl(DateTime from, DateTime until)
        {
            return "/getTemperatures?from=" + from.ToString("yyyy-MM-dd") + "&until=" + until.ToString("yyyy-MM-dd");
        }

        public string GetTemperatureUrl(DateTime from, DateTime until)
        {
            return "/temperatures?from=" + from.ToString("yyyy-MM-dd") + "&until=" + until.ToString("yyyy-MM-dd");
        }

        public IPlugin GetToLowerPlugin()
        {
            return new ToLowerPlugin();
        }

        public string GetToLowerUrl()
        {
            return "/toLower";
        }
    }
}
