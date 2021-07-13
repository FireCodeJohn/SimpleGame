using System;
using System.Net;

namespace KonicaServer
{
    class Program
    {
        // Start server and wait for user to close server
        static void Main()
        {
            StartServer();
            Console.WriteLine("Server started.  Waiting for http requests...");
            Console.ReadKey();
        }

        // start server by setting up httplistener on http://localhost:8080/
        public static void StartServer()
        {
            var httpListener = new HttpListener();
            var simpleServer = new SimpleServer(httpListener, "http://localhost:8080/");
            simpleServer.Start();
        }
    }
}
