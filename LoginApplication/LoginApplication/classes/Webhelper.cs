using LoginApplication.classes.objects;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace LoginApplication.classes
{
    internal class Webhelper
    {

        internal bool RequestServerToLogin(string username, string password, out string errorcode)
        {
            string hashedPassword = ComputeHash(password);
            string Response = "";
            if (!string.IsNullOrEmpty(hashedPassword) && !string.IsNullOrEmpty(username)) 
            {
                LoginPayload logindata = new LoginPayload();
                logindata.Username = username;
                logindata.Password = hashedPassword;
                string Json = JsonConvert.SerializeObject(logindata);
                Response = WebPost("http://194.26.183.38:5060/", Json);
                if(Response.Contains("OK_")) 
                {
                    errorcode = Response;
                    return true;
                }
                else 
                {
                    errorcode = Response;
                    return false;
                }
            }
#if DEBUG
            else 
            {
                Console.WriteLine("ERROR Hashed Password was null or password empty can not send webrequest.");
            }
#endif
            errorcode = Response;
            return false;
        }

        protected string WebPost(string url, string jsoncontent)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            byte[] data = Encoding.ASCII.GetBytes(jsoncontent);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        private string ComputeHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /* Global Instance Handling */
        private static Webhelper g_Webhelper;
        public static Webhelper GetInstance() 
        {
            if(g_Webhelper == null)
                g_Webhelper = new Webhelper();
            return g_Webhelper;
        }
    }
}
