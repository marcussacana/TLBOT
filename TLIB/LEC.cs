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

}
