using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyWebServer
{
    public class Request : IRequest
    {
        //Constants:
        private static readonly string[] httpMethods = { "GET", "HEAD", "POST", "PUT", "DELETE", "CONNECT", "OPTIONS", "TRACE", "PATCH" };

        private bool isValid;
        private string method;
        private string url;
        private IDictionary<string, string> headers;
        private byte[] content;

        //public Request() : this(new MemoryStream())
        //{
        //    //Call constructor with stream parameter = new MemoryStream()
        //}

        /* ---HTTP-Request-Example--- */
        //POST /send.php HTTP/1.1
        //Host: example.com
        //User-Agent: Mozilla/5.0
        //Accept: image/gif, image/jpeg, */*
        //Content-type: application/x-www-form-urlencoded
        //Content-length: 51
        //Connection: close
        //
        //Name=Wei%C3%9Fes+R%C3%B6ssl&Ort=St.+Wolfgang&PLZ=5360

        public Request(Stream stream)
        {
            //Declare variables:
            string line;
            string[] parts;

            //Set isValid to false at the beginning (and to true if everything is valid at the end of the constructor):
            isValid = false;

            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            if (reader.EndOfStream) return; //break when stream is empty

            //firefox always sends request with Url "/facicon.ico" => should not be plugin request
            if (this.Url.RawUrl == "/favicon.ico") return;

            //Parse request-line:
            parts = reader.ReadLine().Split(' ');
            if (parts.Length < 2) return;
            method = parts[0].ToUpper();
            if (!httpMethods.Contains(method)) return;
            url = parts[1];

            //Parse request headers:
            if (this.headers == null) this.headers = new Dictionary<string, string>();
            while (true) //break when line is empty
            {
                line = reader.ReadLine();
                if (line == String.Empty) break;

                parts = line.Split(new[] { ':' }, 2);
                headers[parts[0].ToLower()] = parts[1].Remove(0, 1); //Remove leer
            }

            //Everything is valid
            isValid = true;

            //Parse request body:
            if (ContentLength == 0) return;

            content = new byte[ContentLength];

            char[] test = new char[ContentLength];
            reader.Read(test, 0, ContentLength);
            content = Encoding.UTF8.GetBytes(test);

            //int nextChar;
            //byte[] nextCharInBytes;
            //for(int x = 0; x < ContentLength; x=x)
            //{

            //    nextChar = reader.Read();
            //    nextCharInBytes = BitConverter.GetBytes(nextChar);
            //    for(int y = 0; y < nextCharInBytes.Length; y++)
            //    {
            //        content[x + y] = nextCharInBytes[y];
            //    }
            //    x += nextCharInBytes.Length;
            //}
        }

        public bool IsValid
        {
            get { return this.isValid; }
        }

        public string Method
        {
            get { return this.method; }
        }

        public IUrl Url
        {
            get { return new Url(this.url); }
        }

        public IDictionary<string, string> Headers
        {
            get { return this.headers; }
        }

        public int HeaderCount
        {
            get { return headers.Count; }
        }

        public int ContentLength
        {
            get
            {
                return headers.ContainsKey("content-length") ? int.Parse(headers["content-length"]) : 0;
            }
        }

        public string ContentType
        {
            get { return headers.FirstOrDefault(x => x.Key.Equals("content-type")).Value; }
        }

        public string UserAgent
        {
            get { return headers.FirstOrDefault(x => x.Key.Equals("user-agent")).Value; }
        }

        public Stream ContentStream
        {
            get { return new MemoryStream(content); }
        }

        public string ContentString
        {
            get { return UTF8Decoder.DecodeUmlauts(Encoding.UTF8.GetString(content)); }
        }

        public byte[] ContentBytes
        {
            get { return content; }
        }
    }
}
