#define KiriKiri
//#define SJIS
using System;
using TLIB;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
#if Age
using VNX.EushullyEditor;
#endif
#if Siglus
using SiglusSceneManager;
#endif
#if KiriKiri
using KrKrSceneManager;
#endif
namespace TLBOT {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            Port.Text = LEC.TryDiscoveryPort();
            OpenBinary.Filter = Filter;
            SaveBinary.Filter = Filter;
            LblInfo.Text = string.Format(LblInfo.Text,
#if Age
               "AgeGameEngine"
#endif
#if Siglus
                "SiglusEngine"
#endif
#if KiriKiri
                "KiriKiriZ"
#endif
                );
            string TLFilter = AppDomain.CurrentDomain.BaseDirectory + "Filter.cs";
            if (System.IO.File.Exists(TLFilter)) {
                VM = new DotNetVM(System.IO.File.ReadAllText(TLFilter));
                BlackList = new List<string>(VM.Call("Main", "GetBlackList"));
                string[] List = VM.Call("Main", "GetReplaces");
                for (int i = 0; i < List.Length; i += 2)
                    Replace.Add(List[i], List[i + 1]);
            }
        }
        Dictionary<string, string> Replace = new Dictionary<string, string>();
        List<string> BlackList = new List<string>();
        DotNetVM VM = null;
#if Age
        string Filter = "ALL Eshully Files|*.bin";
        EushullyEditor EE;
#endif
#if Siglus
        string Filter = "All SiglusEngine Scene Files|*.ss";
        SSManager Script;
#endif
#if KiriKiri
        string Filter = "All KiriKiri PSB Files|*.scn;*.psb";
        PSBStringManager SM;
#endif
        private void BntOpen_Click(object sender, EventArgs e) {
            OpenBinary.ShowDialog();
        }
        int engine = 0;
        private void OpenBinary_FileOk(object sender, CancelEventArgs e) {
            Open(OpenBinary.FileName);
        }
        
        private void Open(string file) {
            byte[] script = System.IO.File.ReadAllBytes(file);
#if Age
            EE = new EushullyEditor(script, new FormatOptions());
            EE.SJISBase = new SJExt();
            EE.LoadScript();
            string[] Strings = new string[EE.Strings.Length];
            for (int i = 0; i < Strings.Length; i++)
                Strings[i] = EE.Strings[i].getString();
#endif
#if Siglus
            Script = new SSManager(script);
            Script.Import();
            string[] Strings = Script.Strings;
#endif
#if KiriKiri
            SM = new PSBStringManager();
            SM.Import(script);
            string[] Strings = SM.Strings;
#endif

            StringList.Items.Clear();
            foreach (string str in Strings) {
                StringList.Items.Add(str, false);
            }
        }

        private void BntSave_Click(object sender, EventArgs e) {
            SaveBinary.ShowDialog();
        }
        
        private void ImportList() {
#if Age
            for (int i = 0; i < EE.Strings.Length; i++)
                EE.Strings[i].setString(StringList.Items[i].ToString());
#endif
#if Siglus
            for (int i = 0; i < Script.Strings.Length; i++)
                Script.Strings[i] = StringList.Items[i].ToString(); 
#endif
#if KiriKiri
            for (int i = 0; i < SM.Strings.Length; i++)
                SM.Strings[i] = StringList.Items[i].ToString();
#endif
        }

        bool BM = false;
        private void Save(string As) {
            SaveBinary.FileName = As;
            SaveBinary_FileOk(null, null);
        }
        private void SaveBinary_FileOk(object sender, CancelEventArgs e) {
            ImportList();
            byte[] script =
#if Age
            EE.Export();
#endif
#if Siglus
            Script.Export();
#endif
#if KiriKiri
                SM.Export();
#endif
            System.IO.File.WriteAllBytes(SaveBinary.FileName, script);
            if (!BM)
                MessageBox.Show("File Saved", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        bool Error = false;
        private void BntProc_Click(object sender, EventArgs e) {
            if (!LEC.ServerIsOpen(Port.Text)) {
                MessageBox.Show("Invalid Server Port", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Error = true;
                return;
            }
            for (int i = 0; i < StringList.Items.Count; i++) {
                bool Checked = StringList.GetItemChecked(i);
                if (!Checked)
                    continue;
                string Input = StringList.Items[i].ToString();
                if (BlackList.Contains(Input))
                    continue;
                if (Replace.ContainsKey(Input)) {
                    StringList.Items[i] = Replace[Input];
                    StringList.SelectedIndex = i;
                    continue;
                }
                int tries = -1;
                string Translation = null;
                while (tries < 5 && Translation == null) {
                    Translation = 
                        CkOffline.Checked?
                        LEC.Translate(Input, InputLang.Text, OutLang.Text, LEC.Gender.Male, LEC.Formality.Formal, Port.Text): 
                        Google.Translate(Input, InputLang.Text, OutLang.Text);
                }
                if (Translation == null)
                    continue;
                if (VM != null)
                    Translation = VM.Call("Main", "Filter", Translation);
                FixTL(ref Translation, Input);
                StringList.Items[i] = Translation;
                StringList.SelectedIndex = i;
                Application.DoEvents();
            }
            Text = "TLBOT - ("+System.IO.Path.GetFileName(OpenBinary.FileName)+") - In Game Machine Translation";
            if (!BM)
                MessageBox.Show("All Lines Translated.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FixTL(ref string translation, string input) {
            char[] Open = new char[] { '<', '"', '(', '\'', '「' , '『' , '«' };
            char[] Close = new char[] { '>', '"', ')', '\'', '」', '』', '»' };
            if (Open.Length != Close.Length)
                throw new Exception("Wrong Devloper Configuration");
            for (int i = 0; i < Open.Length; i++) {
                char O = Open[i];
                char C = Close[i];
                if (!(input.StartsWith(O + "") && input.EndsWith(C + "")))
                    continue;
                if (translation.StartsWith(".")) {
                    translation = translation.Substring(1, translation.Length - 1);
                    if (translation.EndsWith(".."))
                        translation = translation.Substring(0, translation.Length - 1);
                }
                if (!translation.StartsWith(O + ""))
                    translation = O + translation;
                if (translation.EndsWith(C + ".")) {
                    translation = translation.Substring(0, translation.Length - 2) + "." + C;
                }
                if (translation.EndsWith(C + "!")) {
                    translation = translation.Substring(0, translation.Length - 2) + "!" + C;
                }
                if (translation.EndsWith(C + "?")) {
                    translation = translation.Substring(0, translation.Length - 2) + "?" + C;
                }
                if (!translation.EndsWith(C + ""))
                    translation += C;
            }
        }

        private void BotSelect_Click(object sender, EventArgs e) {
            uint count = 0;
            for (int i = 0; i < StringList.Items.Count; i++) {
                string text = StringList.Items[i].ToString();
                bool Status = true;
                int Process = 0;
                bool Asian = InputLang.Text == "JA" || InputLang.Text == "CH";
                while (Status) {
                    switch (Process) {
                        default:
                            goto ExitWhile;
                        case 0:
                            bool Alternate = false;
                            if (text.StartsWith("<") && text.EndsWith(">"))
                                Alternate = true;
                            Status = !ContainsOR(text, Alternate ? "@,§,$,\\,|,_,/" : "@,§,$,\\,|,_,<,>,/");
                            break;
                        case 1:
                            Status = NumberLimiter(text, text.Length / 4);
                            break;
                        case 2:
                            Status = text.Length >= 4 || EndsWithOr(text, ".,!,?");
                            break;
                        case 3:
                            if (Asian)
                                Status = MinimiumFound(text, Properties.Resources.JapCommom, text.Length / 4);
                            else
                                Status = text.Contains(((char)32).ToString()) || EndsWithOr(text, ".\",!\",?\",.,!,?");
                            break;
                        case 4:
                            if (!Asian)
                                break;
                            Status = !ContainsOR(text, "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,x,w,y,z,▽,★,♪,.");
                            break;
                    }
                    Process++;
                }
            ExitWhile:
                ;
                if (Status)
                    count++;
                StringList.SetItemChecked(i, Status);
            }
        }

        private bool EndsWithOr(string text, string v) {
            string[] letters = v.Split(',');
            foreach (string letter in letters)
                if (text.EndsWith(letter))
                    return true;
            return false;
        }

        private bool MinimiumFound(string text, string MASK, int min) {
            string[] Entries = MASK.Split(',');
            int found = 0;
            for (int i = 0; i < text.Length; i++) {
                for (int ind = 0; ind < Entries.Length; ind++)
                    if (text[i] == Entries[ind][0]) {
                        found++;
                        break;
                    }
            }
            return found >= min;
        }

        private bool NumberLimiter(string text, int val) {
            int min = '0', max = '9', total = 0;
            int asmin = '０', asmax = '９';
            foreach (char chr in text)
                if (chr >= min && chr <= max)
                    total++;
                else if (chr >= asmin && chr <= asmax)
                    total++;
            return total < val;
        }
        private bool ContainsOR(string text, string MASK) {
            string[] entries = MASK.Split(',');
            foreach (string entry in entries)
                if (text.Contains(entry))
                    return true;
            return false;
        }
        private bool ContainsAND(string text, string MASK) {
            string[] entries = MASK.Split(',');
            foreach (string entry in entries)
                if (!text.Contains(entry))
                    return false;
            return true;
        }

        private void StringList_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                Text = string.Format("TLBOT - {2} ({0}/{1})", StringList.SelectedIndex, StringList.Items.Count, System.IO.Path.GetFileName(OpenBinary.FileName));
            } catch { }
        }

        private void button1_Click(object sender, EventArgs e) {
            MessageBox.Show("Copy the string to find now...", "TLBOT");
            string content = Clipboard.GetText();
            OpenFileDialog FD = new OpenFileDialog();
            FD.Multiselect = true;
            FD.Filter = Filter;
            FD.Title = "Select all files to find the string";
            string Founds = "Found At:\n";
            DialogResult dr = FD.ShowDialog();
            if (dr == DialogResult.OK) {
                for (int i = 0; i < FD.FileNames.Length; i++) {
                    byte[] Script = System.IO.File.ReadAllBytes(FD.FileNames[i]);
#if Age
                    EushullyEditor EE = new EushullyEditor(Script, new FormatOptions());
                    EE.LoadScript();
                    string[] strs = new string[EE.Strings.Length];
                    for (int ind = 0; ind < strs.Length; ind++)
                        strs[ind] = EE.Strings[ind].getString();
#endif
#if Siglus
                    SSManager tmp = new SSManager(Script);
                    tmp.Import();
                    string[] strs = tmp.Strings;
#endif
#if KiriKiri
                    PSBStringManager tmp = new PSBStringManager();
                    tmp.Import(Script);
                    string[] strs = tmp.Strings;
#endif
                    foreach (string s in strs)
                        if (s.Contains(content)) {
                            Founds += System.IO.Path.GetFileName(FD.FileNames[i]) + "\n";
                            break;
                        }
                }
            }
            if (Founds != "Found At:\n") {
                MessageBox.Show(Founds, "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("No Results...", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void BntBathProc_Click(object sender, EventArgs e) {
            BM = true;
            OpenFileDialog FD = new OpenFileDialog();
            FD.Multiselect = true;
            FD.Filter = Filter;
            FD.Title = "Select all files to find the string";
            DialogResult dr = FD.ShowDialog();
            if (dr == DialogResult.OK) {
                Error = false;
                foreach (string File in FD.FileNames) {
                    Open(File);
                    BotSelect_Click(null, null);
                    BntProc_Click(null, null);
                    if (Error)
                        break;
                    Save(File);
                }
            }
            BM = false;
            MessageBox.Show("Operation Cleared!", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
#if SJIS
    class SJExt : Encoding {
        private Encoding BASE = GetEncoding(932);
        private const char uci = 'ﾚ', oci = 'ﾓ', eci = 'ﾉ', aci = 'ﾁ', ici = 'ﾍ', ati = 'ﾃ', oti = 'ﾕ', cdi = 'ﾇ', ech = 'ﾊ', ach = 'ﾂ', acr = 'ﾀ', ucr = 'ﾙ',
            Uci = 'ｺ', Ucr = 'ｹ', ecr = 'ﾈ', icr = 'ﾌ', Oci = 'ｳ', Ocr = 'ｲ', Cdi = 'ｧ', Ach = 'ｫ', Ecr = 'ｨ', Eci = 'ｩ', Aci = '｡', Ici = 'ｭ',
            Icr = 'ｬ', Ech = 'ｪ', Ich = 'ｮ', Och = 'ｴ', Uch = 'ｻ', Oti = 'ｵ', ocr = 'ﾒ', och = 'ﾔ';
        public override int GetByteCount(char[] chars, int index, int count) {
            return BASE.GetByteCount(chars, index, count);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {
            return BASE.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        private char Encode(char lt) {
            switch (lt) {
#region Cases
                default:
                    return lt;
                case 'ú':
                    return uci;
                case 'ó':
                    return oci;
                case 'é':
                    return eci;
                case 'á':
                    return aci;
                case 'í':
                    return ici;
                case 'ã':
                    return ati;
                case 'õ':
                    return oti;
                case 'ç':
                    return cdi;
                case 'ê':
                    return ech;
                case 'â':
                    return ach;
                case 'à':
                    return acr;
                case 'ù':
                    return ucr;
                case 'Ú':
                    return Uci;
                case 'Ù':
                    return Ucr;
                case 'è':
                    return ecr;
                case 'ì':
                    return icr;
                case 'Ó':
                    return Oci;
                case 'Ò':
                    return Ocr;
                case 'Ç':
                    return Cdi;
                case 'Â':
                    return Ach;
                case 'È':
                    return Ecr;
                case 'É':
                    return Eci;
                case 'Á':
                    return Aci;
                case 'Í':
                    return Ici;
                case 'Ì':
                    return Icr;
                case 'Ê':
                    return Ech;
                case 'Î':
                    return Ich;
                case 'Ô':
                    return Och;
                case 'Û':
                    return Uch;
                case 'Õ':
                    return Oti;
                case 'ò':
                    return ocr;
                case 'ô':
                    return och;
#endregion
            }
        }

        private char Decode(char lt) {
            switch (lt) {
#region cases
                default:
                    return lt;
                case uci:
                    return 'ú';
                case oci:
                    return 'ó';
                case eci:
                    return 'é';
                case aci:
                    return 'á';
                case ici:
                    return 'í';
                case ati:
                    return 'ã';
                case oti:
                    return 'õ';
                case cdi:
                    return 'ç';
                case ech:
                    return 'ê';
                case ach:
                    return 'â';
                case acr:
                    return 'à';
                case ucr:
                    return 'ù';
                case Uci:
                    return 'Ú';
                case Ucr:
                    return 'Ù';
                case ecr:
                    return 'è';
                case icr:
                    return 'ì';
                case Oci:
                    return 'Ó';
                case Ocr:
                    return 'Ò';
                case Cdi:
                    return 'Ç';
                case Ach:
                    return 'Â';
                case Ecr:
                    return 'È';
                case Eci:
                    return 'É';
                case Aci:
                    return 'Á';
                case Ici:
                    return 'Í';
                case Icr:
                    return 'Ì';
                case Ech:
                    return 'Ê';
                case Ich:
                    return 'Î';
                case Och:
                    return 'Ô';
                case Uch:
                    return 'Û';
                case Oti:
                    return 'Õ';
                case ocr:
                    return 'ò';
                case och:
                    return 'ô';
#endregion
            }
        }

        public override int GetCharCount(byte[] bytes, int index, int count) {
            return BASE.GetCharCount(bytes, index, count);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {
            return BASE.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        public override string GetString(byte[] bytes) {
            string rst = base.GetString(bytes);
            string decoded = string.Empty;
            for (int i = 0; i < rst.Length; i++)
                decoded += Decode(rst[i]);
            return decoded; 
        }

        public override byte[] GetBytes(string str) {
            string encoded = string.Empty;
            for (int i = 0; i < str.Length; i++)
                encoded += Encode(str[i]);
            return base.GetBytes(encoded);
        }

        public override int GetMaxByteCount(int charCount) {
            return BASE.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount) {
            return BASE.GetMaxCharCount(byteCount);
        }
    }
#endif
}
