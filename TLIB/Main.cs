using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

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
                Client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)");
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
