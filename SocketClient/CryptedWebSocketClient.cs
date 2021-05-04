using SocketCommon;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebSocketSharp;

namespace SocketClient
{
    abstract class CryptedWebSocketClient
    {
        TaskCompletionSource<object> Connection = new TaskCompletionSource<object>();
        byte[] Key;
        byte[] IV;

        WebSocket Socket;
        public CryptedWebSocketClient(string URL, byte[] Key, byte[] IV)
        {
            this.Key = Key;
            this.IV = IV;

            Socket = new WebSocket(URL);
            Socket.Compression = CompressionMethod.Deflate;
            Socket.OnMessage += OnCryptedMessage;
            Socket.OnOpen += OnOpen;
            Socket.OnClose += OnClose;
            Socket.OnError += OnError;
            Socket.ConnectAsync();
        }

        void OnCryptedMessage(object sender, MessageEventArgs e) {
            var Event = OnMessage(e.RawData.Decrypt(Key, IV));
            Event.Wait();
        }

        public async Task<bool> SendAsync(byte[] Data)
        {
            TaskCompletionSource<bool> Send = new TaskCompletionSource<bool>();
            Socket.SendAsync(Data.Encrypt(Key, IV), (OK) => Send.SetResult(OK));
            return await Send.Task;
        }

        public async Task<bool> SendAsync<T>(T Data)
        {
            var RawData = Data.ParseToBytes();
            return await SendAsync(RawData);
        }

        public async Task<bool> SendAsync(params byte[][] Data)
        {
            using (var Stream = new MemoryStream())
            {
                foreach (var PartialData in Data)
                    Stream.Write(PartialData, 0, PartialData.Length);

                return await SendAsync(Stream.ToArray());
            }
        }

        public async Task<dynamic> SendAsync(params dynamic[] Params) {
            using (var Stream = new MemoryStream())
            {
                foreach (var ParamData in (from x in Params select (byte[])DataHelper.ParseToBytes(x)))
                    Stream.Write(ParamData, 0, ParamData.Length);

                return await SendAsync(Stream.ToArray());
            }
        }

        public async Task WaitAsync() {
            await Connection.Task;
            return;
        }

        abstract protected Task OnMessage(byte[] Data);
        virtual protected void OnError(object sender, EventArgs e) { }
        virtual protected void OnOpen(object sender, EventArgs e) { }
        virtual protected void OnClose(object sender, EventArgs e) { 
            Connection.SetResult(null); 
        }
    }
}
