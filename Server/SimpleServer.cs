using System;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using KonicaServer.BusinessObjects;
using KonicaServer.BusinessLogic;

namespace KonicaServer
{
    public class SimpleServer
    {
        private readonly HttpListener listener;

        // init the server
        public SimpleServer(HttpListener listener, string url)
        {
            this.listener = listener;
            listener.Prefixes.Add(url);
        }

        // Start listening
        public void Start()
        {
            if (listener.IsListening)
                return;

            listener.Start();
            listener.GetContextAsync().ContinueWith(ProcessRequestHandler);
        }

        // Stop listening
        public void Stop()
        {
            if (listener.IsListening)
                listener.Stop();
        }

        // process http requests and send response
        private void ProcessRequestHandler(Task<HttpListenerContext> result)
        {
            if (!listener.IsListening)
                return;

            listener.GetContextAsync().ContinueWith(ProcessRequestHandler);

            // get context and endpoint
            var context = result.Result;
            string endpoint = context.Request.RawUrl;
            string responsePayload = "";

            if (endpoint.Equals("/initialize")) // if initialize endpoint go to init logic
                responsePayload = GameLogic.InitGame();
            else if (endpoint.Equals("/node-clicked") && context.Request.HttpMethod.Equals("POST")) // if node-clicked endpoint, do node-clicked logic
            {
                try
                {
                    string text;
                    using (var reader = new StreamReader(context.Request.InputStream,
                                                         context.Request.ContentEncoding))
                    {
                        text = reader.ReadToEnd(); // read the request body
                    }

                    Point point = JsonConvert.DeserializeObject<Point>(text); // convert from json string to .net object with Newtonsoft.Json
                    if (GameLogic.GameState.TurnStage == TurnStage.point1)
                        responsePayload = GameLogic.GameStep1(point); // If node-clicked endpoint start step 1 or step 2 based on game state
                    else
                        responsePayload = GameLogic.GameStep2(point);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message); // catch exceptions and write error to Console
                }                
            }
            else if (endpoint.Equals("/error"))
            {
                string body = new StreamReader(context.Request.InputStream).ReadToEnd();
                Console.WriteLine("Client Error" + Environment.NewLine + body); // Write Client Errors to Console
            }
            else if (!context.Request.HttpMethod.Equals("OPTIONS"))
            {
                Console.WriteLine("Unrecognized Endpoint: " + endpoint); // report unrecognized endpoints
            }

            byte[] outBuffer = Encoding.ASCII.GetBytes(responsePayload);
            context.Response.ContentLength64 = outBuffer.Length; // set headers
            context.Response.AddHeader("Access-Control-Allow-Origin", "null"); 
            context.Response.AddHeader("Access-Control-Allow-Methods", "OPTIONS, GET, POST");
            context.Response.AddHeader("Access-Control-Allow-Headers", "content-type"); 

            var output = context.Response.OutputStream;
            output.WriteAsync(outBuffer, 0, outBuffer.Length); // write response body to response output stream
            output.Close();
        }
    }
}
