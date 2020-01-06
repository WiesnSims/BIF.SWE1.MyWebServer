using BIF.SWE1.Interfaces;
using MyWebServer.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MyWebServer.src.Plugins
{
    public class TemperaturePlugin : IPlugin
    {
        private SensorTemperatureDB db = new SensorTemperatureDB();

        public float CanHandle(IRequest req)
        {
            if (req.Url.Path != "/temperatures" && req.Url.Path != "/getTemperatures") return 0;
            if (req.Url.ParameterCount != 2) return 0;
            if (!req.Url.Parameter.ContainsKey("from") || !req.Url.Parameter.ContainsKey("until")) return 0;

            try
            {
                var from = Convert.ToDateTime(req.Url.Parameter["from"].Replace("%3A", ":"));
                var until = Convert.ToDateTime(req.Url.Parameter["until"].Replace("%3A", ":"));
            }
            catch
            {
                return 0;
            }
            return 1;
        }

        public IResponse Handle(IRequest req)
        {
            IUrl url = req.Url;
            if (url.Path == "/temperatures")
            {
                //Return HTML-page temperatures
                return WebPagePlugin.CreateWebpageResponse("/temperatures");
            }
            else //url.Path == "/getTemperatures"
            {
                //Return XML-file with temperatures
                DateTime from = Convert.ToDateTime(url.Parameter["from"]);
                DateTime until = Convert.ToDateTime(url.Parameter["until"]);
                Response response = new Response();
                response.StatusCode = 200;
                response.ContentType = "text/xml";
                response.SetContent(GetTemperaturesContentXML(from, until));
                return response;
            }
        }

        private string GetTemperaturesContentXML(DateTime from, DateTime until)
        {
            Dictionary<DateTime, double> temperatures = db.GetTemperaturesOfTimespan(from, until);
            var baseXML = new XElement("temperatures");
            foreach (var temp in temperatures)
            {
                var measurementXML = new XElement("measurement");
                var timeXML = new XElement("time", temp.Key.ToString("yyyy-MM-dd HH:mm:ss"));
                var tempXML = new XElement("temperature", temp.Value);
                measurementXML.Add(timeXML);
                measurementXML.Add(tempXML);
                baseXML.Add(measurementXML);
            }
            return baseXML.ToString();
        }
    }
}
