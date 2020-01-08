using BIF.SWE1.Interfaces;
using MyWebServer.src;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
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

                //Manual Loading of Plugins (not working as intended)

                //ConsoleWrite.Green("Server is running...");
                //ConsoleWrite.White("Hit the F1-key to stop the server.");
                //ConsoleWrite.White("Press F2 to add a plugin to the plugin-manager.");
                //while (true)
                //{
                //    var key = Console.ReadKey().Key;
                //    if (key == ConsoleKey.F1)
                //    {
                //        serverThread.Abort();

                //        ConsoleWrite.White("\nServer stopped.");
                //    }

                //    if (key == ConsoleKey.F2)
                //    {
                //        ConsoleWrite.White("DLL-Name:");
                //        string dllName = Console.ReadLine();

                //        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //        string path = Path.Combine(directory, @"static-files", dllName);

                //        try
                //        {
                //            var DLL = Assembly.LoadFile(path);

                //            foreach (Type type in DLL.GetExportedTypes())
                //            {
                //                if (type.GetInterface("IPlugin") != null)
                //                {
                //                    server.AddPlugin((IPlugin)Activator.CreateInstance(type));
                //                    ConsoleWrite.Green("Plugin added successfully!");
                //                }
                //            }
                //        }
                //        catch (Exception e)
                //        {
                //            ConsoleWrite.Red("Error adding plugin: " + e.Message);
                //        }
                //    }
                //}
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
