using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Linq;

namespace TLIB {
    public class LEC {
        public enum Gender {
            Male, Female
        }
        public enum Formality {
            Formal, Formal_Plural,
            Informal, Informal_Plural
        }
        public static string Translate(string Text, string SourceLang, string TargetLang, Gender ForceGender, Formality Formal, string Port) {
            //Convert enum's to string
            string gender = ForceGender == Gender.Male ? "Male" : "Female";
            string formality = string.Empty;
            switch (Formal) {
                case Formality.Formal:
                    formality = "Formal";
                    break;
                case Formality.Formal_Plural:
                    formality = "Formal Plural";
                    break;
                case Formality.Informal:
                    formality = "Familiar";
                    break;
                case Formality.Informal_Plural:
                    formality = "Familiar Plural";
                    break;
            }

            //Prepare Variables
            Text = HttpUtility.HtmlEncode(Text);
            SourceLang = SourceLang.Contains("-") ? SourceLang.Split('-')[0].ToLower() : SourceLang.ToLower();
            TargetLang = TargetLang.Contains("-") ? TargetLang.Split('-')[0].ToLower() : TargetLang.ToLower();

            //Prepare XML Request Data
            string XMLBASE = string.Format(Properties.Resources.XML, SourceLang, TargetLang, gender, formality, Text);
            byte[] XML = Encoding.UTF8.GetBytes(XMLBASE);

            //Initalize and Send Request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format("http://localhost:{0}/Translation", Port));
            req.ContentType = "text/xml; charset=utf-8";
            req.ContentLength = XML.Length;
            req.Method = "POST";
            req.Timeout = 20000;
            Stream ReqStream = req.GetRequestStream();
            ReqStream.Write(XML, 0, XML.Length);
            ReqStream.Close();

            //Read Response
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream RespStream = response.GetResponseStream();
            byte[] dat = new byte[req.ContentLength];
            RespStream.Read(dat, 0, dat.Length);

            //Convert to String
            string OutXML = Encoding.UTF8.GetString(dat);

            //Set XML Entries to Find
            string XmlOpen = "<return>";
            string XmlClose = "</return>";

            //Check if is a valid response and return the translation
            if (OutXML.Contains(XmlOpen) && OutXML.Contains(XmlClose)) {
                int Start = OutXML.IndexOf(XmlOpen) + XmlOpen.Length;
                int End = OutXML.IndexOf(XmlClose);
                string output = OutXML.Substring(Start, End - Start);
                string Translation = output;
                while (Translation != HttpUtility.HtmlDecode(Translation))
                    Translation = HttpUtility.HtmlDecode(Translation);
                return Translation;
            }
            return null;
        }

        /// <summary>
        /// Try Discovery the LEC Running Port
        /// </summary>
        /// <returns>Probabbly Server Port or Null if fails</returns>
        public static string TryDiscoveryPort() {
            List<ProcessPort> Ports = ProcessPorts.ProcessPortMap;
            foreach (ProcessPort Port in Ports) {
                if (Port.ProcessName.Contains("TranslateDotNet Server"))
                    return Port.PortNumber.ToString();
            }
            return null;
        }
        public static bool ServerIsOpen(string Port) {
            try {
                LimitedWC wc = new LimitedWC();
                string rst = wc.DownloadString(string.Format("http://localhost:{0}/Status.htm", Port));
                if (rst.Contains("Status is OK"))
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }
        private class LimitedWC : WebClient {
            protected override WebRequest GetWebRequest(Uri uri) {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 10 * 1000;
                return w;
            }
        }
    }

    public class Google {
        const string UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
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
            }
            catch {
                if (tries++ < 2)
                    goto again;
               throw new Exception("Translation Error");
            }
        }

        const string FormEntry = "----WebKitFormBoundary";
        public static string[] Translate(string[] Strings, string SourceLang, string TargetLang) {
            const string FormTemplate = "{0}\r\nContent-Disposition: form-data; name=\"sl\"\r\n\r\nen\r\n{0}\r\nContent-Disposition: form-data; name=\"tl\"\r\n\r\npt\r\n{0}\r\nContent-Disposition: form-data; name=\"js\"\r\n\r\ny\r\n{0}\r\nContent-Disposition: form-data; name=\"ie\"\r\n\r\nUTF-8\r\n{0}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"Content.txt\"\r\nContent-Type: text/plain\r\n\r\n{1}\r\n{0}--";
            const string URL = "https://translate.googleusercontent.com/translate_f";
            const string Type = "multipart/form-data; boundary={0}";

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
            Builder = null;
            byte[] Content = new UTF8Encoding(false).GetBytes(string.Format(FormTemplate, Entry, Builder.ToString()));

            //Request
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(URL);
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
            string[] Result = Reply.Replace("\r\n", "\n").Split('\n');
            Reply = null;

            for (long i = 0; i < Result.LongLength; i++) {
                string Str = string.Empty;
                while (Str != HttpUtility.HtmlDecode(Str))
                    Str = HttpUtility.HtmlDecode(Str);
                Result[i] = Str;
            }
            return Result;
        }

        private static string RndTxt(int len = 10) {
            const int Min = 'A', Max = 'z';
            string Rnd = string.Empty;
            int Seed = new Random().Next(int.MinValue, int.MaxValue);
            while (Rnd.Length < len) {
                Rnd += (char)new Random(Seed).Next(Min, Max);
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
                        byte b2 = Convert.ToByte(text[i] + (text[i+1]+""), 16);
                        i += 2;
                        byte b1 = Convert.ToByte(text[i] + (text[i + 1]+""), 16);
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
