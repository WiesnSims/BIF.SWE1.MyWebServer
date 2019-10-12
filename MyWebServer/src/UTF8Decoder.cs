using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    public class UTF8Decoder
    {
        public static string DecodeUmlauts(string input)
        {
            Dictionary<string, string> partsToReplace = new Dictionary<string, string>
            {
                {"%C3%A4", "ä" },
                {"%C3%B6", "ö" },
                {"%C3%BC", "ü" },
                {"%C3%84", "Ä" },
                {"%C3%96", "Ö" },
                {"%C3%9C", "Ü" },
                {"%C3%9F", "ß" },
                {"+", " " },
                {"%3A", ":" }
            };
            foreach(var part in partsToReplace.Keys)
            {
                input = input.Replace(part, partsToReplace[part]);
            }
            return input;
        }
    }
}
