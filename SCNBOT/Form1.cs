#define SSA
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
#if WillPlus
using WillPlusManager;
#endif
#if Automata
using AutomataTranslator;
#endif
#if FairyFencer
using BruteGDStringEditor;
#endif
#if KrKr2CSV
using KrKr2CSV;
#endif
#if KrKr2SQL
using KrKr2SQL;
#endif
#if SSA
using SSA;
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
#if WillPlus
                "WillPlus"
#endif
#if Automata
                "NieR: Automata"
#endif
#if PlainText
                "Plain Text"
#endif
#if FairyFencer
                "Fairy Fencer F Advent - Dark Force"
#endif
#if KrKr2CSV
                "KiriKiri 2 SQL CSV"
#endif
#if KrKr2SQL
                "KiriKiri 2 SQLite"
#endif
#if SSA
                "Subtitle"
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
#if WillPlus
        string Filter = "All WillPlus WS2 Files|*.ws2";
        WS2 Script;
        WS2String[] StringsClass;
#endif
#if Automata
        string Filter = "All Nier Subtitle Files|*.smd";
        SMDManager Script;
        string[] Strs;
#endif
#if PlainText
        string[] Strs;
        string Fully;
        string Filter = "All Files|*.*";
        Encoding Encoding = Encoding.Unicode;
#endif
#if FairyFencer
        string[] strs;
        string Filter = "All dat files|*.dat";
        GlobalDataStringEditor Editor;
#endif
#if KrKr2CSV
        string[] Strs;
        string Filter = "All TXT or CSV Files|*.txt;*.csv";
        KrKrCSV Editor;
#endif
#if KrKr2SQL
        string[] Strs;
        string Filter = "All Sqlite Files|*.sdb;*.sqlite;*.db|All Files|*.*";
        SQLOpen Editor;
#endif
#if SSA
        string[] Strs;
        string Filter = "All SSA/ASS Files|*.ssa;*.ass|All Files|*.*";
        Subtitle Editor;
#endif
        private void BntOpen_Click(object sender, EventArgs e) {
            OpenBinary.ShowDialog();
        }
        private void OpenBinary_FileOk(object sender, CancelEventArgs e) {
            Open(OpenBinary.FileName);
        }

#if FairyFencer
        bool failed = false;
#endif
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
#if WillPlus
            Script = new WS2(script, true, new SJExt());
            StringsClass = Script.Import();
            string[] Strings = new string[StringsClass.Length];
            for (int i = 0; i < Strings.Length; i++)
                Strings[i] = StringsClass[i].String.Replace("\\n", "\n");
#endif
#if Automata
            Script = new SMDManager(script);
            string[] Strings = Script.Import();
            Strs = Strings;
#endif
#if PlainText
            string[] Strings = Encoding.GetString(script).Replace("\r\n", "\n").Split('\n');
            Strs = Strings;
#endif
#if FairyFencer
            Editor = new GlobalDataStringEditor(script);
            failed = false;
            try {
                strs = Editor.Import();
            } catch {
                failed = true;
                return;
            }
            string[] Strings = strs;
#endif
#if KrKr2CSV
            Editor = new KrKrCSV(script);
            string[] Strings = Editor.Import();
            Strs = Strings;
#endif
#if KrKr2SQL
            Editor = new SQLOpen(file);
            Strs = Editor.Import();
            string[] Strings = Strs;
#endif
#if SSA
            Editor = new Subtitle(script);
            string[] Strings = Strs = Editor.Import();
#endif
            StringList.Items.Clear();
            foreach (string str in Strings) {
                StringList.Items.Add(str, false);
            }
            Begin.Maximum = End.Maximum = StringList.Items.Count;
            Begin.Value = 0;
            End.Value = End.Maximum;
        }

        private void BntSave_Click(object sender, EventArgs e) {
#if KrKr2SQL
            SaveBinary_FileOk(null, null);
#else
            SaveBinary.ShowDialog();
#endif
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
#if WillPlus
            for (int i = 0; i < StringsClass.Length; i++)
                StringsClass[i].String = StringList.Items[i].ToString().Replace("\n", "\\n");
#endif
#if Automata || KrKr2CSV || KrKr2SQL || SSA
            for (int i = 0; i < Strs.Length; i++)
                Strs[i] = StringList.Items[i].ToString();
#endif
#if PlainText
            for (int i = 0; i < Strs.Length; i++)
                Strs[i] = StringList.Items[i].ToString();

            Fully = string.Empty;
            foreach (string str in Strs)
                Fully += "\r\n" + str;
#endif
#if FairyFencer
            for (int i = 0; i < strs.Length; i++)
                strs[i] = Encoding.GetEncoding(932).GetString(Encoding.GetEncoding(932).GetBytes(StringList.Items[i].ToString()));//remove special chars
#endif

        }

        bool BM = false;
        private void Save(string As) {
            SaveBinary.FileName = As;
            SaveBinary_FileOk(null, null);
        }
        private void SaveBinary_FileOk(object sender, CancelEventArgs e) {
            ImportList();
#if !KrKr2SQL
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
#if WillPlus
                Script.Export(StringsClass);
#endif
#if Automata
                Script.Export(Strs);
#endif
#if PlainText
            Encoding.GetBytes(Fully);
#endif
#if FairyFencer
                Editor.Export(strs);
#endif
#if KrKr2CSV
                Editor.Export(Strs);
#endif
#if SSA
                Editor.Export(Strs);
#endif
            System.IO.File.WriteAllBytes(SaveBinary.FileName, script);
#else
            Editor.ProgressChange = new Action(() => { Text = "Saving - " + Editor.ExportProgress; });
            Editor.Export(Strs);
#endif
            if (!BM)
                MessageBox.Show("File Saved", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        bool Error = false;
        bool Abort = false;
        bool CanAbort = false;
        private void BntProc_Click(object sender, EventArgs e) {
            if (CanAbort) {
                Abort = true;
                return;
            }
            if (!LEC.ServerIsOpen(Port.Text)) {
                MessageBox.Show("Invalid Server Port", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Error = true;
                return;
            }
            CanAbort = true;
            Abort = false;
            BntProc.Text = "Abort!";
            for (int i = 0; i < StringList.Items.Count; i++) {
                if (Abort) {
                    MessageBox.Show("Operation Aborted!", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BntProc.Text = "Translate!";
                    CanAbort = false;
                    return;
                }
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
                if (Input.Length > 3 && string.IsNullOrWhiteSpace(Input.Replace(Input[0] + "", "").Replace(Input[1] + "", "").Replace(".", "").Replace("!", "").Replace("?", ""))) {
                    continue;
                }
                int tries = -1;
                string Translation = null;
                while (tries < 5 && Translation == null) {
                    Translation =
                        CkOffline.Checked ?
                        LEC.Translate(Input, InputLang.Text, OutLang.Text, LEC.Gender.Male, LEC.Formality.Formal, Port.Text) :
                        Google.Translate(Input, InputLang.Text, OutLang.Text);
                }
                if (Translation.ToLower().StartsWith("-benzóico")) {
                    Translation = LEC.Translate(Input, InputLang.Text, OutLang.Text, LEC.Gender.Male, LEC.Formality.Formal, Port.Text);
                }
                if (Translation == null || Translation.ToLower().StartsWith("-benzóico"))
                    continue;
                if (VM != null)
                    Translation = VM.Call("Main", "Filter", Translation);
                FixTL(ref Translation, Input);
                Translation = FixTLAlgo2(Translation, Input);
                StringList.Items[i] = Translation;
                StringList.SelectedIndex = i;
                Application.DoEvents();
            }
            Text = "TLBOT - (" + System.IO.Path.GetFileName(OpenBinary.FileName) + ") - In Game Machine Translation";
            CanAbort = false;
            BntProc.Text = "Translate!";
            if (!BM) {
                if (Shutdown.Checked) {
                    BM = true;
                    Save(System.IO.Path.GetDirectoryName(OpenBinary.FileName) + "\\" + System.IO.Path.GetFileNameWithoutExtension(OpenBinary.FileName) + "-autosave" + System.IO.Path.GetExtension(OpenBinary.FileName));
                    System.Diagnostics.Process.Start("shutdown.exe", "/f /s /t 120");
                }
                MessageBox.Show("All Lines Translated.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FixTL(ref string translation, string input) {
            char[] Open = new char[] { '<', '"', '(', '\'', '「', '『', '«' };
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
            for (int i = (int)Begin.Value; i < End.Value; i++) {
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
                Text = string.Format("TLBOT - {2} ({0}/{1} - {3}%)", StringList.SelectedIndex, StringList.Items.Count, System.IO.Path.GetFileName(OpenBinary.FileName), (int)(((double)StringList.SelectedIndex / StringList.Items.Count) * 100));
            }
            catch { }
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
#if WillPlus
                    WS2 tmp = new WS2(Script, true, new SJExt());
                    WS2String[] strclass = tmp.Import();
                    string[] strs = new string[strclass.Length];
                    for (int x = 0; x < strclass.Length; x++)
                        strs[x] = strclass[x].String.Replace("\\n", "\n");
#endif
#if Automata
                    SMDManager tmp = new SMDManager(Script);
                    string[] strs = tmp.Import();
#endif
#if PlainText
                    string[] strs = Encoding.GetString(Script).Replace("\r\n", "\n").Split('\n');
#endif
#if FairyFencer
                    GlobalDataStringEditor tmp = new GlobalDataStringEditor(Script);
                    string[] strs;
                    try {
                        strs = tmp.Import();
                    } catch {
                        Founds += "Failed to Open: "+ System.IO.Path.GetFileName(FD.FileNames[i]) + "\n";
                        continue;
                    }
#endif
#if KrKr2CSV
                    KrKrCSV tmp = new KrKrCSV(Script);
                    string[] strs = tmp.Import();
#endif
#if KrKr2SQL
                    SQLOpen tmp;
                    string[] strs;
                    try {
                        tmp = new SQLOpen(FD.FileNames[i]);
                        strs = tmp.Import();
                    }
                    catch {
                        Founds += "Failed to Open: " + System.IO.Path.GetFileName(FD.FileNames[i]);
                        continue;
                    }
#endif
#if SSA
                    Subtitle tmp = new Subtitle(Script);
                    string[] strs = tmp.Import();
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


        //PTBR Prefix And Sufix to ignore when fix letter repeat
        private List<string> BlackSplitList = new List<string>(new string[] { "auto", "me", "se", "lhe", "tes", "te", "ti", "a", "bem", "mal"});
        public string FixTLAlgo2(string TL, string Ori) {
            string[] NewWords = TL.Split(' ');
            for (int i = 0; i < NewWords.Length; i++) {
                string Word = NewWords[i];
                if (Word.Contains("-")) {
                    string[] Splited = Word.Split('-');
                    bool Repeat = Splited.Length > 2;
                    try {
                        if (Repeat) {
                            for (int x = 0; x < Splited.Length - 1; x++) {
                                if (Splited[x].ToLower() != Splited[0].ToLower())
                                    Repeat = false;
                                if (string.IsNullOrEmpty(Splited[0]) || BlackSplitList.Contains(Splited[0].ToLower()) || Splited[x].Length == 0 || Splited[x + 1].Length == 0) {
                                    Repeat = false;
                                    throw new Exception();
                                }
                            }
                        } else {
                            string last = Splited[Splited.Length - 1].ToLower();
                            for (int x = 0; x < Splited.Length - 1; x++) {
                                if (last.Length <= Splited[x].Length || RepeatCheck(last, "sama") || RepeatCheck(last, "san") || RepeatCheck(last, "chan") || RepeatCheck(last, "kun") || RepeatCheck(last, "senpai") || RepeatCheck(last, "sensei")  || RepeatCheck(last, "chi") || RepeatCheck(last, "tan") || (Splited.Length > 3 && !Ori.ToLower().Contains(Splited[0].ToLower() + "-") || (Splited[0][0].ToString().ToLower() != Splited[1][0].ToString().ToLower() && Splited.Length >= 3)))
                                    Repeat = false;
                            }
                        }
                    }
                    catch {
                        continue;
                    }
                    if (!Repeat)
                        continue;
                    for (int x = 0; x < Splited.Length - 1; x++) {
                        bool firstonly = Splited[x].Length >= Splited[Splited.Length - 1].Length;
                        string NewPrefix = Splited[Splited.Length - 1].Substring(0, !firstonly ? Splited[x].Length : 1);
                        Splited[x] = NewPrefix;
                    }
                    Word = "";
                    foreach (string pre in Splited)
                        Word += pre + "-";
                    Word = Word.Substring(0, Word.Length - 1);
                    if (i == 0 || NewWords[i - 1].EndsWith("."))
                        Word = Word.Substring(0, 1).ToUpper() + Word.Substring(1, Word.Length - 1);
                }
                NewWords[i] = Word;
            }
            string NewStr = "";
            for (int i = 0; i < NewWords.Length; i++) {
                NewStr += NewWords[i] + (i + 1 < NewWords.Length && !NewWords[i + 1].StartsWith(".") && !NewWords[i + 1].StartsWith("?") && !NewWords[i + 1].StartsWith("!") ? " " : "");
            }
            return NewStr;
        }

        private bool RepeatCheck(string Str, string Word, bool AllowMissMatch = true) {
            bool Trigger = false;
            Str = Str.ToLower();
            for (int i = 0, x = 0; i < Word.Length; i++) {
                while (x < Str.Length && Str[x] == '.' || Str[x] == '?' || Str[x] == '!') 
                    x++;
                if (x >= Str.Length)
                    return false;
                if (Str[x] != Word[i] && Trigger)
                    return false;
                else if (Str[x] != Word[i]) {
                    Trigger = true;
                    while (x < Str.Length && Str[x] != Word[i])
                        x++;
                    if (x >= Str.Length)
                        return false;
                }
                while (x < Str.Length && Str[x] == Word[i])
                    x++;
                if (x >= Str.Length)
                    return false;
            }
            return true;
        }

        private void BntBathProc_Click(object sender, EventArgs e) {
            BM = true;
            OpenFileDialog FD = new OpenFileDialog();
            FD.Multiselect = true;
            FD.Filter = Filter;
            FD.Title = "Select all files to find the string";
            string log = string.Empty;
            DialogResult dr = FD.ShowDialog();
            if (dr == DialogResult.OK) {
                Error = false;
                foreach (string File in FD.FileNames) {
                    Open(File);
#if FairyFencer
                    if (failed)
                        continue;
#endif
                    BotSelect_Click(null, null);
                    BntProc_Click(null, null);
                    if (Error)
                        break;
                    try {
                        Save(File);
                    }
                    catch {
                        log += "\nError: " + System.IO.Path.GetFileName(File);
                    }
                }
            }
            BM = false;
            if (Shutdown.Checked) {
                System.Diagnostics.Process.Start("shutdown.exe", "/f /s /t 120");
            }
            MessageBox.Show(string.Empty == log ? "Operation Cleared!" : "Sucess, but that files have a problem:" + log, "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
#if KrKr2SQL
            Editor.Close();
#endif
        }

        private void SearchKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\n' || e.KeyChar == '\r') {
                string bak = Text;
                e.Handled = true;
                string text = SearchTB.Text.ToLower();
                for (int i = (int)Begin.Value; i < End.Value; i++) {
                    Text = "Searching... " + i + "/" + (int)End.Value;
                    string Str = StringList.Items[i].ToString().ToLower();
                    if (Str.Contains(text)) {
                        Text = bak;
                        StringList.SelectedIndex = i;
                        return;
                    }
                    Application.DoEvents();
                }
                Text = bak;
            }
        }

        private void MassSelect_Click(object sender, EventArgs e) {
            if (StringList.Items.Count > 1) {
                bool Status = !StringList.GetItemChecked(0);
                for (int i = 0; i < StringList.Items.Count; i++)
                    StringList.SetItemChecked(i, Status);
            }
        }
    }
#if SJIS
    class SJExt : Encoding {
        private Encoding BASE = GetEncoding(932);
#if WillPlus
        private const char uci = '/', oci = '*', eci = '|', aci = '#', ici = '`', ati = '&', oti = '^', cdi = '<', ech = '[', ach = '%', acr = '$', ucr = 'ﾙ',
            Uci = 'ｺ', Ucr = 'ｹ', ecr = 'ﾈ', icr = 'ﾌ', Oci = ']', Ocr = 'ｲ', Cdi = 'ｧ', Ach = 'ｫ', Ecr = 'ｨ', Eci = '{', Aci = '=', Ici = '}',
            Icr = 'ｬ', Ech = 'ｪ', Ich = 'ｮ', Och = '_', Uch = 'ｻ', Oti = 'ｵ', ocr = 'ﾒ', och = '+';
#else
        private const char uci = 'ﾚ', oci = 'ﾓ', eci = 'ﾉ', aci = 'ﾁ', ici = 'ﾍ', ati = 'ﾃ', oti = 'ﾕ', cdi = 'ﾇ', ech = 'ﾊ', ach = 'ﾂ', acr = 'ﾀ', ucr = 'ﾙ',
            Uci = 'ｺ', Ucr = 'ｹ', ecr = 'ﾈ', icr = 'ﾌ', Oci = 'ｳ', Ocr = 'ｲ', Cdi = 'ｧ', Ach = 'ｫ', Ecr = 'ｨ', Eci = 'ｩ', Aci = '｡', Ici = 'ｭ',
            Icr = 'ｬ', Ech = 'ｪ', Ich = 'ｮ', Och = 'ｴ', Uch = 'ｻ', Oti = 'ｵ', ocr = 'ﾒ', och = 'ﾔ';
#endif
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
