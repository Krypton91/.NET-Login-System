using Login_Backend.classes.websocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Backend
{
    internal class Program
    {
        #region Instances
        private static Server webserver = new Server();
        #endregion
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            webserver.Start();
            await Task.Delay(-1).ConfigureAwait(false); //Never close our exe!
        }
    }
}
