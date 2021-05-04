using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SocketCommon;
using WebSocketSharp;
using SocketClient.Clients;
using static SocketCommon.CryptoHelper;
using static SocketCommon.DataHelper;

namespace SocketClient
{
    public static class API
    {
        public static EventHandler ConnectionFailed;

        const int Version = 1;

        internal static byte[] Key = new byte[0x20];

        static bool Connected = false;

        static Translation Translator;
        static Session Session;

        public static string Translate(string Line, string SourceLanguage, string TargetLanguage) {
            return Translate(new string[] { Line }, SourceLanguage, TargetLanguage).First();
        }

        public static string[] Translate(string[] Lines, string SourceLanguage, string TargetLanguage) {
            if (!Connected) {
                var Connection = BeginConnection();
                Connection.Wait();

                if (!Connection.Result)
                {
                    ConnectionFailed?.Invoke(null, new EventArgs());
                    return null;
                }

                Session = new Session("ws://marcussacanawan.dynv6.net:5525");
                
                Translator = Session.OpenTranslator();
                Translator.Initializer.Wait();
            }

            var TK = Translator.Translate(Lines, SourceLanguage, TargetLanguage);
            TK.Wait();

            return TK.Result;
        }
        static async Task<bool> BeginConnection()
        {
            TaskCompletionSource<CloseEventArgs> Connection = new TaskCompletionSource<CloseEventArgs>();

            var OK = false;

            //Generate the connection info to send to the server.
            var Data = Version.ParseToBytes();
            Data = Data.Concat(Key.ParseToBytes()).ToArray();
            Data = Data.Encrypt(SeedKey, new byte[16]);

            var Socket = new WebSocket("ws://marcussacanawan.dynv6.net:5525/Connection");

            Socket.Compression = CompressionMethod.Deflate;

            //On connected send the Connection Info to the server
            Socket.OnOpen += (sender, e) => Socket.SendAsync(Data, (_) => { });

            //After the server recive the Connection Info, he returns the IV and maybe the Key
            Socket.OnMessage += (sender, e) =>
            {
                using (var Stream = new MemoryStream(e.RawData.Decrypt(SeedKey, new byte[16])))
                {
                    var ForceKey = Stream.ReadData();
                    if (ForceKey)
                        Key = Stream.ReadData();
                    IV = Stream.ReadData();
                }
                OK = true;
                Socket.SendAsync(OK.ParseToBytes(), (_) => { });
            };

            //After the server send the IV to the client, he close the connection
            Socket.OnClose += async (sender, e) => { Connection.SetResult(e); };

            Socket.ConnectAsync();

            //Wait the Connection to be closed
            await Connection.Task;

            return OK;
        }
    }
}
