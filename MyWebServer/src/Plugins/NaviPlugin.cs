using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MyWebServer.src.Plugins
{
    public class NaviPlugin : IPlugin
    {
        Dictionary<string, List<string>> StreetsInCities = new Dictionary<string, List<string>>(); //Street and List of Cities it's in
        string city = String.Empty;
        string street = String.Empty;
        bool IsRebuilding = false;

        public float CanHandle(IRequest req)
        {
            if (req.Url.Path != "/naviQuery" || req.Method != "POST") return 0;
            if (!(req.ContentString.Contains("street=") || req.ContentString.Contains("refresh="))) return 0;
            return 1;
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response();
            response.ContentType = "text/plain";
            response.StatusCode = 200;
            if (IsRebuilding)
            {
                response.SetContent("Keine Abfrage möglich, Karte wird neu aufbereitet.");
                return response;
            }
            if (req.ContentString.Contains("refresh="))
            {
                Thread mapRebuilderThread = new Thread(() => RebuildMap(req.ContentString.Split('=')[1])); //austria
                IsRebuilding = true;
                mapRebuilderThread.Start();
                response.SetContent("Neu laden der Karte erfolgreich gestartet.");
                return response;
            }
            else
            {
                string street = req.ContentString.Split('=')[1].Replace('+', ' ');
                string content = GetCitiesOfStreet(street);
                response.SetContent(content);
            }

            return response;
        }

        private string GetCitiesOfStreet(string street)
        {
            if (street == String.Empty) return "Anfrage eingeben.";
            int cityCount = StreetsInCities.ContainsKey(street) ? StreetsInCities[street].Count : 0;
            string result = cityCount + " Orte gefunden";
            if (cityCount > 0)
            {
                result += ": ";
                foreach (string city in StreetsInCities[street])
                {
                    result += city + ", ";
                }
                result = result.Substring(0, result.Length - 2);
            }
            return result;
        }

        private void RebuildMap(string country)
        {
            StreetsInCities.Clear();
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //string filename = country + ".osm";
            string filename = "data.osm";
            string path = Path.Combine(directory, @"static-files", "osm_data\\" + filename);
            using (var fs = File.OpenRead(path))
            using (var xml = new System.Xml.XmlTextReader(fs))
            {
                while (xml.Read())
                {
                    if (xml.NodeType == System.Xml.XmlNodeType.Element && xml.Name == "osm")
                    {
                        ReadOsm(xml);
                    }
                }
            }
            IsRebuilding = false;
        }

        private void ReadOsm(System.Xml.XmlTextReader xml)
        {
            using (var osm = xml.ReadSubtree())
            {
                //while (osm.Read())
                //{
                //    if (osm.NodeType == System.Xml.XmlNodeType.Element && (osm.Name == "node" || osm.Name == "way"))
                //    {
                //        ReadAnyOsmElement(osm);
                //    }
                //}

                while (true)
                {
                    try
                    {
                        bool canRead = osm.Read();
                        if (!canRead) break;
                        if (osm.NodeType == System.Xml.XmlNodeType.Element && (osm.Name == "node" || osm.Name == "way"))
                        {
                            ReadAnyOsmElement(osm);
                        }
                    }
                    catch (Exception e)
                    {
                        ConsoleWrite.Red("Error occured when reading XML-tree.");
                    }
                }
            }
        }

        private void ReadAnyOsmElement(System.Xml.XmlReader osm)
        {
            using (var element = osm.ReadSubtree())
            {
                while (element.Read())
                {
                    if (element.NodeType == System.Xml.XmlNodeType.Element && element.Name == "tag")
                    {
                        ReadTag(element);
                    }
                }
            }
        }

        private void ReadTag(System.Xml.XmlReader element)
        {
            string tagType = element.GetAttribute("k");
            string value = element.GetAttribute("v");
            switch (tagType)
            {
                case "addr:city":
                    city = value;
                    break;
                case "addr:street":
                    street = value;
                    break;
            }
            if ((!String.IsNullOrEmpty(city)) && (!String.IsNullOrEmpty(street)))
            {
                if (!StreetsInCities.ContainsKey(street))
                {
                    StreetsInCities.Add(street, new List<string> { city });
                }
                else
                {
                    if (!StreetsInCities[street].Contains(city))
                    {
                        StreetsInCities[street].Add(city);
                    }
                }
                street = String.Empty;
                city = String.Empty;
            }
        }
    }
}
