using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace TLIB {

    public class Google {
        const string UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
        /// <summary>
        /// Translate a string
        /// </summary>
        /// <param name="Strings">String to Translate</param>
        /// <param name="SourceLang">Oringinal Text Language</param>
        /// <param name="TargetLang">Target Text Language</param>
        /// <returns>Translated Text, thrown exception if fails</returns>
        public static string Translate(string Text, string SourceLang, string TargetLang) {
            const string REQ = "http://translate.googleapis.com/translate_a/single";
            const string TAG = "],[\"";
            SourceLang = SourceLang.Contains("-") ? SourceLang.Split('-')[0] : SourceLang;
            TargetLang = TargetLang.Contains("-") ? TargetLang.Split('-')[0] : TargetLang;
            int tries = 0;
            again:;
            try {
                WebClient Client = new WebClient();
                Client.Encoding = Encoding.UTF8;
                Client.Headers.Add(HttpRequestHeader.UserAgent, UserAgent);
                Client.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                Client.QueryString.Add("client", "gtx");
                Client.QueryString.Add("dt", "t");
                Client.QueryString.Add("sl", SourceLang.ToLower());
                Client.QueryString.Add("tl", TargetLang.ToLower());
                Client.QueryString.Add("q", Text);
                byte[] Response = Client.DownloadData(REQ);
                byte[] Content = new byte[Response.Length - 2];
                for (int i = 0; i < Content.Length; i++)
                    Content[i] = Response[i + 1];
                string TranslatedText = Encoding.UTF8.GetString(Content);
                if (!TranslatedText.StartsWith("[[\""))
                    throw new Exception();
                string TL = string.Empty;
                int pos = 3;
                while (true) {
                    TL += GetStringAt(pos, TranslatedText);
                    pos = TranslatedText.IndexOf(TAG, pos);
                    if (pos < 0)
                        break;
                    else
                        pos += TAG.Length;
                }
                return Decode(TL);
            } catch {
                if (tries++ < 2)
                    goto again;
                throw new Exception("Translation Error");
            }
        }

        const string FormTemplate = "{0}\r\nContent-Disposition: form-data; name=\"sl\"\r\n\r\n{2}\r\n{0}\r\nContent-Disposition: form-data; name=\"tl\"\r\n\r\n{3}\r\n{0}\r\nContent-Disposition: form-data; name=\"js\"\r\n\r\ny\r\n{0}\r\nContent-Disposition: form-data; name=\"ie\"\r\n\r\nUTF-8\r\n{0}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"Content.txt\"\r\nContent-Type: text/plain\r\n\r\n{1}\r\n{0}--";
        const string URL = "https://translate.googleusercontent.com/translate_f";
        const string Type = "multipart/form-data; boundary={0}";

        const string FormEntry = "----WebKitFormBoundary";


        /// <summary>
        /// Translate a string arryay
        /// </summary>
        /// <param name="Strings">Strings to Translate</param>
        /// <param name="SourceLang">Oringinal Text Language</param>
        /// <param name="TargetLang">Target Text Language</param>
        /// <param name="BufferLength">Short the request to prevent line skip</param>
        /// <returns>Translated Text, thrown exception if fails</returns>
        public static string[] Translate(string[] Strings, string SourceLang, string TargetLang, int BufferLength = 400) {
            if (Strings?.Length == 0)
                return new string[0];
            SourceLang = SourceLang.Contains("-") ? SourceLang.Split('-')[0] : SourceLang;
            TargetLang = TargetLang.Contains("-") ? TargetLang.Split('-')[0] : TargetLang;
            int tries = 0;
            again:;
            try {
                string[] Result = new string[Strings.LongLength];
                for (long i = 0, unchanged = 0; i < Strings.LongLength;) {
                    string[] Buffer = new string[i + BufferLength < Strings.LongLength ? BufferLength : Strings.LongLength - i];

                    for (int x = 0; x < Buffer.Length; x++)
                        Buffer[x] = Strings[i + x];

                    Buffer = TransBlock(Buffer, SourceLang, TargetLang);

                    for (int x = 0; x < Buffer.Length; x++) {
                        if (Buffer[x].ToLower() == Strings[i + x].ToLower() && Buffer[x].Length > 10)
                            unchanged++;
                        if (unchanged > Strings.Length / 3 && tries == 0) {
                            if (Buffer.Length > 10)
                                BufferLength /= 2;
                            throw new Exception();
                        }
                        Result[i + x] = Buffer[x];
                    }

                    i += Buffer.Length;
                }
                return Result;
            } catch (Exception ex) {
                if (tries++ > 2)
                    throw ex;
                else
                    goto again;
            }
        }

        private static string[] TransBlock(string[] Strings, string SourceLang, string TargetLang) {
            int tries = 0;
            again:;
            try {
                //Get Valid Form Info
                string Entry = FormEntry;
                while (Strings.Where(x => x.StartsWith(Entry)).Select(x => x).LongCount() != 0) {
                    Entry = FormEntry + RndTxt();
                }

                //Generate Text
                StringBuilder Builder = new StringBuilder();
                foreach (string String in Strings) {
                    if (String.Contains("\n") || String.Contains("\r")) {
                        Builder.AppendLine(String.Replace("\r", "").Replace("\n", " "));
                    } else
                        Builder.AppendLine(String);
                }
                byte[] Content = new UTF8Encoding(false).GetBytes(string.Format(FormTemplate, "--" + Entry, Builder.ToString(), SourceLang, TargetLang));
                Builder = null;

                //Request
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(URL);
                Request.Method = "POST";
                Request.UserAgent = UserAgent;
                Request.ContentType = string.Format(Type, Entry);
                Stream Upload = Request.GetRequestStream();
                MemoryStream Data = new MemoryStream(Content);
                Data.CopyTo(Upload);
                Data.Close();
                Content = null;

                //Response
                WebResponse Response = Request.GetResponse();
                Stream Download = Response.GetResponseStream();
                Data = new MemoryStream();
                Download.CopyTo(Data);
                Download.Close();
                Request = null;
                Response = null;

                //Convert Response
                string Reply = Encoding.UTF8.GetString(Data.ToArray());
                if (!(Reply.StartsWith("<pre>") && Reply.EndsWith("</pre>")))
                    throw new Exception("Failed To Translate");
                Reply = Reply.Substring(5, Reply.Length - 11);//Cut off <pre></pre>
                string[] ReplyArr = Reply.Replace("\r\n", "\n").Split('\n');
                Reply = null;
                string[] Return = new string[Strings.LongLength];
                for (long i = 0; i < Return.LongLength; i++) {
                    string Str = ReplyArr[i];
                    while (Str != HttpUtility.HtmlDecode(Str))
                        Str = HttpUtility.HtmlDecode(Str);
                    Return[i] = Str;
                }
                ReplyArr = null;
                return Return;

            } catch (Exception ex) {
                if (tries++ > 2)
                    throw ex;
                else
                    goto again;
            }
        }

        private static string RndTxt(int len = 10) {
            const int Min = 'A', Max = 'Z';
            string Rnd = string.Empty;
            int Seed = new Random().Next(int.MinValue, int.MaxValue);
            while (Rnd.Length < len) {
                char C = (char)new Random(Seed).Next(Min, Max);
                if (new Random(Seed).Next(0, 2) == 1)
                    C = C.ToString().ToLower()[0];
                Rnd += C;
                //Fast Random
                int XSD = (int)((new Random().Next(int.MinValue, int.MaxValue) * Rnd.Length) & 0xFFFFFFFF);
                Seed = new Random(Seed ^ XSD).Next(int.MinValue, int.MaxValue);
            }
            return Rnd;
        }

        private static string GetStringAt(int pos, string Text) {
            string STR = string.Empty;
            for (int i = pos; i < Text.Length; i++) {
                if (Text[i] == '"' && Text[i - 1] != '\\') {
                    const string Mask = "\",,,";
                    int Index = Text.IndexOf(Mask, i);
                    if (Index < 0)
                        break;
                    i = Index + 4 + Mask.Length;
                    if (i + 8 > Text.Length)
                        break;
                } else
                    STR += Text[i];
            }
            return STR;
        }
        private static string Decode(string text) {
            string Output = string.Empty;
            for (int i = 0; i < text.Length; i++) {
                if (text[i] != '\\') {
                    Output += text[i];
                    continue;
                }
                i++;
                switch (text[i]) {
                    case '"':
                        Output += "\"";
                        break;
                    case 'n':
                        Output += "\n";
                        break;
                    case 'u':
                        i++;
                        byte b2 = Convert.ToByte(text[i] + (text[i + 1] + ""), 16);
                        i += 2;
                        byte b1 = Convert.ToByte(text[i] + (text[i + 1] + ""), 16);
                        i += 1;
                        byte[] Unicode = new byte[] { b1, b2 };
                        string C = Encoding.Unicode.GetString(Unicode);
                        Output += C;
                        break;
                }
            }
            foreach (string Replacement in new string[] { "ę,ê", "ă,ã", "ő,õ", "ï¿,ã", "½,o" }) {
                Output = Output.Replace(Replacement.Split(',')[0], Replacement.Split(',')[1]);
            }
            return Output;
        }
    }
}
