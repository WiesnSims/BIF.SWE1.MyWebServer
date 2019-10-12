using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    public class Url : IUrl
    {
        private string rawUrl;
        private string path;
        private Dictionary<string, string> parameters = new Dictionary<string, string>();
        private string extension;
        private string fileName;
        private string fragment;
        private string[] segments;

        public Url() : this(null)
        {
            //Call constructor with raw-string parameter = null
        }

        public Url(string raw)
        {
            this.rawUrl = string.IsNullOrEmpty(raw) ? string.Empty : raw;
            parseUrl();
        }

        private void parseUrl()
        {
            //Declare variables:
            string[] parts;

            //Set path to rawUrl before parsing everything
            path = rawUrl;

            //Parse fragment:
            if (rawUrl.Contains('#'))
            {
                parts = path.Split('#');
                path = parts[0];
                fragment = parts[1];
            }

            //Parse parameters:
            if(path.Contains('?'))
            {
                parts = path.Split(new char[] { '?', '&' });
                path = parts[0];
                for (int x = 1; x < parts.Length; x++)
                {
                    string[] par = parts[x].Split('=');
                    parameters[par[0]] = UTF8Decoder.DecodeUmlauts(par[1]);
                }
            }

            //Parse segments:
            segments = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public IDictionary<string, string> Parameter
        {
            get { return parameters; }
        }

        public int ParameterCount
        {
            get { return parameters.Count; }
        }

        public string Path
        {
            get { return this.path; }
        }

        public string RawUrl
        {
            get { return this.rawUrl; }
        }

        public string Extension
        {
            get { throw new NotImplementedException(); }
        }

        public string FileName
        {
            get { throw new NotImplementedException(); }
        }

        public string Fragment
        {
            get { return this.fragment; }
        }

        public string[] Segments
        {
            get { return this.segments; }
        }
    }
}
