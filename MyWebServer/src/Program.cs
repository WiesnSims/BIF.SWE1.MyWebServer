using MyWebServer.src;
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
            Webserver server = new Webserver();

            ConsoleWrite.Green("MyWebServer starts now!");
            try
            {
                Thread serverThread = new Thread(() => server.Start());
                serverThread.Start();
                ConsoleWrite.Green("Server is running.");
            }
            catch(Exception e)
            {
                ConsoleWrite.Red("Server could not be started! ERROR: " + e.Message);
            }

            while(true)
            {

            }
        }
    }
}
