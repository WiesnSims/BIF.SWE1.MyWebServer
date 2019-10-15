using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyWebServer
{
    public class Response : IResponse
    {
        #region CONSTANTS
        private static readonly IDictionary<int, string> STATUS_CODES = new Dictionary<int, string>
        {
            {200, "OK"},
            {404, "Not Found" },
            {500, "INTERNAL SERVER ERROR" }
        };
        private static readonly string HTTP_VERSION = "HTTP/1.1";
        private static readonly string DEFAULT_SERVER_HEADER = "BIF-SWE1-Server";
        #endregion

        private IDictionary<string, string> headers = new Dictionary<string, string>();
        private string contentType;
        private int statusCode;
        private byte[] content;

        public IDictionary<string, string> Headers
        {
            get { return this.headers; }
        }

        public int ContentLength
        {
            get { return content == null ? 0 : content.Length; }
        }

        public string ContentType
        {
            get { return this.contentType; }
            set { this.contentType = value; }
        }

        public string ContentString
        {
            get
            {
                return Encoding.UTF8.GetString(content);
            }
        }

        public int StatusCode
        {
            get
            {
                if(this.statusCode == 0)
                {
                    throw new InvalidOperationException("Statuscode has to be set!");
                }
                return this.statusCode;
            }
            set { this.statusCode = value; }
        }

        public string Status
        {
            get
            {
                return string.Format("{0} {1}", this.statusCode, STATUS_CODES.FirstOrDefault(x => x.Key == this.statusCode).Value);
            }
        }

        public string ServerHeader
        {
            get
            {
                var header = this.headers.Where(x => x.Key == "Server").Select(x => x.Value).FirstOrDefault();
                return header == default(string) ? DEFAULT_SERVER_HEADER : header;
            }
            set { this.headers["Server"] = value; }
        }

        public void AddHeader(string header, string value)
        {
            headers[header] = value;
        }

        public void SetContent(string content)
        {
            this.content = Encoding.UTF8.GetBytes(content);
        }

        public void SetContent(byte[] content)
        {
            this.content = content;
        }

        public void SetContent(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                this.content = ms.ToArray();
            }
        }

        public void Send(Stream network)
        {
            //Throw exception if content-type is set but content is not:
            if(!String.IsNullOrEmpty(this.contentType) && this.content == null)
            {
                throw new InvalidOperationException("Content-type was set without setting the content!");
            }

            StreamWriter writer = new StreamWriter(network, Encoding.UTF8);
            
            //Write status-line:
            writer.WriteLine(string.Format("{0} {1}", HTTP_VERSION, this.Status));

            //Write server:
            writer.WriteLine(string.Format("{0}: {1}", "Server", ServerHeader));

            //Write content-length:
            writer.WriteLine(string.Format("{0}: {1}", "Content-Length", ContentLength));

            //Write conten-type:
            writer.WriteLine(string.Format("{0}: {1}", "Content-Type", ContentType));

            //Write all other headers:
            foreach (var item in this.headers)
            {
                writer.WriteLine(string.Format("{0}: {1}", item.Key, item.Value));
            }

            if (content != null && ContentLength != 0)
            {
                //Write seperator (blank line):
                writer.WriteLine();

                //Write content:
                writer.Write(Encoding.UTF8.GetString(content));
            }

            writer.Flush();

            //LASSEN UNIT TESTS NICHT ZU - SPÄTER HINZUFÜGEN
            //writer.Close();
        }
    }
}
