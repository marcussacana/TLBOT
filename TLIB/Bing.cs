using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TLIB {

    public class Bing {
        private struct SingleForm {
            public string Text;
            public string SourceLanguage;
            public string TargetLanguage;
        }
        struct SingleResponse {
            public string text;
            public string sourceLanguage;
            public string targetLanguage;
            public string resultSMT;
            public string resultNMT;
        }

        /// <summary>
        /// Translate a string
        /// </summary>
        /// <param name="Text">Text to Translate</param>
        /// <param name="SourceLang">Oringinal Text Language</param>
        /// <param name="TargetLang">Target Text Language</param>
        /// <param name="NeuralMode">Enables or Disable the Neural Translation</param>
        /// <returns>Translated Text, or null if fails.</returns>
        public static string Translate(string Text, string SourceLang, string TargetLang, bool NeuralMode) {
            const string URL = "https://translator.microsoft.com/neural/api/translator/translate";
            SourceLang = SourceLang.Contains("-") ? SourceLang.Split('-')[0] : SourceLang;
            TargetLang = TargetLang.Contains("-") ? TargetLang.Split('-')[0] : TargetLang;
            int tries = 0;
            Again:;
            try {
                HttpWebRequest Request = WebRequest.Create(URL) as HttpWebRequest;
                Request.ContentType = "application/json;charset=UTF-8";
                Request.Method = "POST";

                string JSON = Encode(new SingleForm() {
                    SourceLanguage = SourceLang,
                    TargetLanguage = TargetLang,
                    Text = Text
                });

                byte[] Data = Encoding.UTF8.GetBytes(JSON);
                MemoryStream Writer = new MemoryStream(Data);
                Stream RequestStrm = Request.GetRequestStream();
                Writer.CopyTo(RequestStrm);
                RequestStrm.Close();
                Writer.Close();

                HttpWebResponse Response = Request.GetResponse() as HttpWebResponse;
                if (Response.StatusCode != HttpStatusCode.OK)
                    return null;
                Writer = new MemoryStream();
                Stream Reader = Response.GetResponseStream();
                Reader.CopyTo(Writer);
                Reader.Close();
                Data = Writer.ToArray();
                Writer.Close();

                JSON = Encoding.UTF8.GetString(Data);
                var Resp = Decode<SingleResponse>(JSON);
                if (NeuralMode)
                    return Resp.resultNMT;
                return Resp.resultSMT;
            } catch {
                if (tries++ > 3)
                    goto Again;
                return null;
            }
        }

        private static string Encode(object Data) {
            return System.Web.Helpers.Json.Encode(Data);
        }
        private static T Decode<T>(string Json) {
            return System.Web.Helpers.Json.Decode(Json, typeof(T));
        }

        private static Cookie[] Cookies = null;

        /// <summary>
        /// Massive String Translation (Unstable),
        /// Statical Translation isn't avaliable to massive mode.
        /// </summary>
        /// <param name="Strings">Strings to Translate</param>
        /// <param name="SourceLang">Oringinal Text Language</param>
        /// <param name="TargetLang">Target Text Language</param>
        /// <returns>Translated Text, or null if fails.</returns>
        public static string[] Translate(string[] Strings, string SourceLang, string TargetLang, int BufferLength = 200) {
            SourceLang = SourceLang.Contains("-") ? SourceLang.Split('-')[0] : SourceLang;
            TargetLang = TargetLang.Contains("-") ? TargetLang.Split('-')[0] : TargetLang;
            if (!(Strings?.Length > 0))
                return Strings;
            Proxy = null;
            int Tries = 0;
            Again:;
            try {
                if (Cookies == null) {
                    GetCookies();
                }
                string[] Result = new string[Strings.Length];
                for (long i = 0, unchanged = 0; i < Strings.LongLength;) {
                    string[] Buffer = new string[i + BufferLength < Strings.LongLength ? BufferLength : Strings.LongLength - i];

                    for (int x = 0; x < Buffer.Length; x++) {
                        Buffer[x] = Strings[x+i];
                    }
                    int tries = 0;
                    Rty:;
                    try {
                        Buffer = TransBlock(Buffer, SourceLang, TargetLang);
                        for (int x = 0; x < Buffer.Length; x++) {
                            if (Buffer[x].ToLower() == Strings[x+i].ToLower() && Buffer[x].Length > 10 || (string.IsNullOrWhiteSpace(Buffer[x]) && !string.IsNullOrWhiteSpace(Strings[x+i])))
                                unchanged++;
                            if (unchanged > Strings.Length / 3 && tries == 0) {
                                if (Buffer.Length > 10)
                                    BufferLength /= 2;
                                throw new Exception();
                            }
                            Result[x+i] = Buffer[x];
                        }
                        Tries = 0;
                    } catch {
                        if (tries++ > 6)
                            throw new Exception();
                        goto Rty;
                    }

                    i += Buffer.Length;
                }
                return Result;
            } catch {
                Cookies = null;
                if (Tries++ > 3) {
                    return null;
                }
                goto Again;
            }
        }

        static string Proxy = null;
        private static string[] TransBlock(string[] Strings, string SourceLang, string TargetLang) {
            const string URLMask = "https://www.bing.com/translator/api/Translate/TranslateArray?from={0}&to={1}";
            TextEntry[] Form = new TextEntry[0];

            for (int i = 0; i < Strings.Length; i++) {
                AppendArray(ref Form, new TextEntry() {
                    id = i,
                    text = Strings[i]
                });
            }

            string JSON = Encode((from x in Form where !string.IsNullOrWhiteSpace(x.text) select x).ToArray());

            MemoryStream Buffer = new MemoryStream(Encoding.UTF8.GetBytes(JSON));
            HttpWebRequest Request = WebRequest.Create(string.Format(URLMask, SourceLang, TargetLang)) as HttpWebRequest;
            HttpWebResponse Response = null;
            string Reply = null;
            try {
                Request.Method = "POST";
                Request.ContentType = "application/json; charset=UTF-8";
                Request.Expect = "200-OK";
                Request.Timeout = 20 * 1000;
                if (Proxy != null) {
                    Request.Proxy = new WebProxy(Proxy);
                }

                CookieContainer Container = new CookieContainer();
                foreach (Cookie Cookie in Cookies)
                    Container.Add(Cookie);
                Request.CookieContainer = Container;

                Stream Writer = Request.GetRequestStream();
                Buffer.CopyTo(Writer);
                Writer.Close();
                Buffer.Close();

                Response = Request.GetResponse() as HttpWebResponse;
                Stream Reader = Response.GetResponseStream();
                Buffer = new MemoryStream();
                Reader.CopyTo(Buffer);
                Reader.Close();
                Reply = Encoding.UTF8.GetString(Buffer.ToArray());
                Buffer.Close();
            } catch {
                if (Proxy == null)
                    Proxy = GetProxy();
                else
                    Proxy = null;
                Cookies = null;
                throw new Exception("IP Blocked");
            }


            if (Response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to Translate");

            string[] Result = new string[Strings.Length];
            var Resp = Decode<ResponseForm>(Reply);
            Reply = string.Empty;
            for (int i = 0; i < Result.Length; i++) {
                string[] TL = (from x in Resp.items where x.id == i select x.text).ToArray();
                if (TL.Length == 0)
                    Result[i] = Strings[i];
                else
                    Result[i] = TL[0];
            }

            return Result;
        }

        const string ProxyAPI = "http://gimmeproxy.com/api/getProxy?get=true&post=true&cookies=true&supportsHttps=true&protocol=http&minSpeed=65";
        private static string GetProxy() {
            string Reply = string.Empty;
            string Proxy = null;
            while (Reply == string.Empty) {
                string Response = new WebClient().DownloadString(ProxyAPI).Replace(@" ", "");
                Proxy = ReadJson(Response, "curl");
                if (string.IsNullOrWhiteSpace(Proxy))
                    continue;

                try {
                    WebClient Client = new WebClient();
                    Client.Proxy = new WebProxy(Proxy);
                    Reply = Client.DownloadString("https://www.google.com/");
                } catch {
                }
            }
            return Proxy;
        }
        static string ReadJson(string JSON, string Name) {
            string Finding = string.Format("\"{0}\":", Name);
            int Pos = JSON.IndexOf(Finding) + Finding.Length;
            if (Pos - Finding.Length == -1)
                return null;

            string Cutted = JSON.Substring(Pos, JSON.Length - Pos).TrimStart(' ', '\n', '\r');
            char Close = Cutted.StartsWith("\"") ? '"' : ',';
            Cutted = Cutted.TrimStart('"');
            string Data = string.Empty;
            foreach (char c in Cutted) {
                if (c == Close)
                    break;
                Data += c;
            }
            if (Data.Contains("\\"))
                throw new Exception("Ops... Unsupported Json Format...");

            return Data;
        }
        private struct ResponseForm {
            public string from;
            public string to;
            public TextEntry[] items;
        }

        private struct TextEntry {
            public int id;
            public string text;
        }
        private static void GetCookies() {
            const string URL = "https://www.bing.com/translator/";
            HttpWebRequest Request = WebRequest.Create(URL) as HttpWebRequest;
            Request.Method = "GET";
            Request.Timeout = 5 * 1000;
            HttpWebResponse Response = Request.GetResponse() as HttpWebResponse;
            string[] SetCookies = Response.Headers.GetValues("Set-Cookie");
            Cookies = new Cookie[0];
            foreach (string SetCookie in SetCookies) {
                if (!SetCookie.Contains(";"))
                    continue;
                if (!SetCookie.Split(';')[0].Contains("="))
                    continue;
                string CookieName = SetCookie.Split('=')[0].Trim();
                string CookieVal = SetCookie.Split('=')[1].Split(';')[0];
                AppendArray(ref Cookies, new Cookie(CookieName, CookieVal, "/", "bing.com"));
            }
        }

        private static void AppendArray<T>(ref T[] Arr, T Val) {
            T[] NArr = new T[Arr.Length + 1];
            Arr.CopyTo(NArr, 0);
            NArr[Arr.Length] = Val;
            Arr = NArr;
        }
    }
}
