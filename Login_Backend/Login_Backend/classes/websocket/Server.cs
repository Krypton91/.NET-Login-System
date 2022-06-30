using Login_Backend.classes.objects;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Login_Backend.classes.websocket
{
    internal class Server
    {
        private HttpListener listener;
        private readonly string url = "http://194.26.183.38:5060/";
        public static string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>Backend Login System by Mario Rauch</title>" +
            "  </head>" +
            "  <body>" +
            "    <p>Simple HTML Webinterface. CSS and HTML can be build in here easy! :)</p>" +
            "  </body>" +
            "</html>";

        private async Task HandleIncomingConnections()
        {
            byte[] Response = Encoding.UTF8.GetBytes(pageData);
            while (true)
            {
                try
                {
                    HttpListenerContext ctx = await listener.GetContextAsync();
                    HttpListenerRequest req = ctx.Request;
                    HttpListenerResponse resp = ctx.Response;
                    if (req.HttpMethod == "POST")
                    {
                        if (!req.HasEntityBody)
                        {
                            resp.Close();
                            return;
                        }
                        using (System.IO.Stream body = req.InputStream)
                        {
                            using (var reader = new System.IO.StreamReader(body, req.ContentEncoding))
                            {
                                string content = reader.ReadToEnd();
                                Console.WriteLine($"Incomming connection with content: {content}\n From: {req.RemoteEndPoint.Address}");
                                if (content.Contains("LoginRequest"))
                                {
                                    LoginRequestPayload loginData = JsonConvert.DeserializeObject<LoginRequestPayload>(content);
                                    if(loginData != null) 
                                    {
                                        bool result = Database.GetInstance().CanLoginToAccount(loginData.Username, loginData.Password);
                                        if (result) 
                                        {
                                            string AuthKey = "OK_edflkse@@€4324+#fs+ddf+wf+r+342r+efcsdfpcsadfdfh23we98irfzh2834";
                                            Response = Encoding.UTF8.GetBytes(AuthKey);
                                        }
                                        else 
                                        {
                                            Response = Encoding.UTF8.GetBytes("ERROR: Credentials are not stored in our database!\n Please ask the Admin Mario Rauch to get acess to this!");
                                        }
                                    }
                                    else 
                                    {
                                        //Error not needed to implemnt Error Handling because we only have 1 user for this example project.
                                        resp.Close();
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    // Write the response info
                    resp.ContentType = "text/html";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = Response.LongLength;
                    await resp.OutputStream.WriteAsync(Response, 0, Response.Length);
                    resp.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.StackTrace}");
                }
            }
        }


        internal void Start()
        {
            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add(url);
                listener.Start();
                Console.WriteLine($"Server Listening for connections on {url}");
                Task listenTask = HandleIncomingConnections();
                listenTask.GetAwaiter().GetResult();
                listener.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.StackTrace}");
            }
        }
    }
}
