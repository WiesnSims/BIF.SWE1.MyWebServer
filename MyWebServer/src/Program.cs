using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace MyWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Read();


        }
        public static void Read()
        {
            var listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();
            var s = listener.AcceptSocket();
            var stream = new NetworkStream(s);
            var sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                Console.WriteLine(line);
            }

            
        }
    }
}
