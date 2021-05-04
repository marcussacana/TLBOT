using SocketCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SocketClient.Clients
{
    class Translation : CryptedWebSocketClient
    {
        public Task<bool> Initializer => InitializerSource.Task;
        TaskCompletionSource<bool> InitializerSource = new TaskCompletionSource<bool>();

        Dictionary<int, TaskCompletionSource<string[]>> TaskMap = new Dictionary<int, TaskCompletionSource<string[]>>();

        enum Command : int { 
            RequestTranslation,
            TranslationResponse
        }

        public Translation(string URL, byte[] Key, byte[] IV) : base(URL + "/TL", Key, IV) { }

        protected override async Task OnMessage(byte[] Data)
        {
            using (MemoryStream Stream = new MemoryStream(Data))
            {
                switch ((Command)Stream.ReadData())
                {
                    case Command.TranslationResponse:
                        int ID = Stream.ReadData();
                        TaskMap[ID].SetResult((string[])Stream.ReadData());
                        break;
                }
            }
        }


        public async Task<string[]> Translate(string[] Text, string SourceLang, string TargetLang) {
            var TaskCompletion = new TaskCompletionSource<string[]>();
            
            int TID = TaskMap.Count;
            TaskMap[TID] = TaskCompletion;

            await SendAsync((int)Command.RequestTranslation, TID, SourceLang, TargetLang, Text);

            return await TaskCompletion.Task;
        }

        protected override void OnOpen(object sender, EventArgs e)
        {
            base.OnOpen(sender, e);
            InitializerSource.SetResult(true);
        }

        protected override void OnError(object sender, EventArgs e) {
            ErrorEventArgs Error = (ErrorEventArgs)e;
            foreach (var Task in TaskMap.Values)
            {
                if (Task.Task.IsCompleted || Task.Task.IsFaulted || Task.Task.IsFaulted)
                    continue;

                Task.SetException(Error.GetException());
            }
        }

        protected override void OnClose(object sender, EventArgs e)
        {
            foreach (var Task in TaskMap.Values) {
                if (Task.Task.IsCompleted || Task.Task.IsFaulted || Task.Task.IsFaulted)
                    continue;

                Task.SetException(new Exception("Connection Closed"));
            }
        }
    }
}
