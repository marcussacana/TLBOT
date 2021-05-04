using SocketCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SocketClient.Clients
{
    class Session : CryptedWebSocketClient
    {
        string ServerUrl;
        List<object> Instances = new List<object>();
        enum Command : int
        {
            UserName, 
            OpenTranslation
        }

        public Session(string URL) : base(URL.TrimEnd('/') + "/Session", API.Key, CryptoHelper.IV) {
            ServerUrl = URL.TrimEnd('/');
        }

        protected override async Task OnMessage(byte[] Data)
        {
            using (MemoryStream Stream = new MemoryStream(Data))
            {
                switch ((Command)Stream.ReadData())
                {
                    case Command.UserName:
                        var ResponseType = ((int)Command.UserName).ParseToBytes();
                        var UserName = Environment.UserName.ParseToBytes();
                        _ = SendAsync(ResponseType, UserName);
                        break;
                }
            }
        }

        public Translation OpenTranslator() {
            return new Translation(ServerUrl, API.Key, CryptoHelper.IV);
        }

        protected override void OnOpen(object sender, EventArgs e)
        {
            var ResponseType = ((int)Command.UserName).ParseToBytes();
            var UserName = Environment.UserName.ParseToBytes();
            SendAsync(ResponseType, UserName).Wait();
        }
    }
}
