using BIF.SWE1.Interfaces;
using MyWebServer.Database;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyWebServer.src
{
    class Webserver
    {
        private static readonly string DEFAULT_IP_ADDRESS = "127.0.0.1";
        private static readonly string DEFAULT_PORT = "8080";

        private string ipadress;
        private string port;
        private PluginManager pluginManager = new PluginManager();


        private SensorTemperatureDB db = new SensorTemperatureDB();


        public Webserver(string ipAdress, string port) //Creates Webserver with ip Adress and port
        {
            this.ipadress = ipAdress;
            this.port = port;
        }
        public Webserver()
        {
            this.ipadress = DEFAULT_IP_ADDRESS;
            this.port = DEFAULT_PORT;
        }

        public void Start() //Starts Server
        {
            //generate random sensor data
            Thread sensorThread = new Thread(() => MeasureSensorTemperature());
            sensorThread.Start();


            TcpListener listener = new TcpListener(IPAddress.Parse(ipadress), int.Parse(port));
            listener.Start();
            List<Thread> requestHandlerThreads = new List<Thread>();
            while (true) //Creates new Thread / Socket when new User connects
            {
                Socket socket = listener.AcceptSocket();
                Thread t = new Thread(() => HandleRequest(socket));
                t.Start();
                requestHandlerThreads.Add(t);
            }
        }

        private void HandleRequest(Socket socket)
        {
            try
            {
                NetworkStream networkStream = new NetworkStream(socket);
                IRequest request = new Request(networkStream);

                //if(request.Url.RawUrl.Contains("favicon.ico"))
                //{
                //    ConsoleWrite.Red("favicon.ico");
                //    var faviconResponse = new Response();
                //    faviconResponse.StatusCode = 404;

                //    faviconResponse.Send(networkStream);
                //    return;
                //}

                if (!request.IsValid)
                {
                    ConsoleWrite.Red("Invalid request.");
                    return;
                }

                IPlugin plugin = pluginManager.GetBestSuitingPlugin(request);
                IResponse response = plugin.Handle(request);
                
                response.Send(networkStream);
                networkStream.Flush();
                //networkStream.Close(); //???
                ConsoleWrite.Green("Response sent successfully. (" + plugin.GetType().Name + ")");
            }
            catch (Exception e)
            {
                ConsoleWrite.Red("Error occured! Error: " + e.Message + ": " + e.InnerException);
            }
        }

        public void AddPlugin(IPlugin plugin) //Adds plugins to server
        {
            pluginManager.Add(plugin);
        }

        private void MeasureSensorTemperature()
        {
            //only executed once to fill test data
            //db.InitializeDatabase();

            double currentTemperature = TemperatureTestData.GetRandomTemperature();
            while (true)
            {
                currentTemperature = TemperatureTestData.GetRandomTemperature(currentTemperature);
                db.InsertTemperature(DateTime.Now, currentTemperature);
                Thread.Sleep(5000);
            }
        }
    }
}
