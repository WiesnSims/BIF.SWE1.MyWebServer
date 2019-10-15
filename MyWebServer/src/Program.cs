using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            

            Console.WriteLine("MyWebServer starts now!");

            //Thread serverThread = new Thread(() => server.Start());
            //serverThread.Start();

            Console.WriteLine("Server is running.");

            while(true)
            {

            }

            //Read();


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
