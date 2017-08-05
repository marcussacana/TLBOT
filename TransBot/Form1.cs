﻿//#define SJIS
using System;
using TLIB;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.IO;
using AdvancedBinary;
using SacanaWrapper;
using System.Linq;

namespace TLBOT {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        Dictionary<string, string> Replace = new Dictionary<string, string>();
        List<string> BlackList = new List<string>() {
            "ON", "On", "on", "OFF", "Off", "off"
        };
        DotNetVM VM = null;

        string[] Strs;
        string Filter = "All Files|*.*";
        Wrapper Editor;
        private void BntOpen_Click(object sender, EventArgs e) {
            OpenBinary.ShowDialog();
        }
        private void OpenBinary_FileOk(object sender, CancelEventArgs e) {
            Open(OpenBinary.FileName);
        }
        private void Open(string file) {
            byte[] Script = File.ReadAllBytes(file);
            Editor = new Wrapper();
            Strs = Editor.Import(Script, Path.GetExtension(file), true);
            LastScript = file;

            StringList.Items.Clear();
            foreach (string str in Strs) {
#if SJIS
                StringList.Items.Add(Remap.GetString(SJIS.GetBytes(str)), false);
#else
                StringList.Items.Add(str, false);
#endif
            }

            Begin.Maximum = End.Maximum = StringList.Items.Count;
            Begin.Value = 0;
            End.Value = End.Maximum;
        }

        private void BntSave_Click(object sender, EventArgs e) {
            SaveBinary.ShowDialog();
        }

#if SJIS
        SJExt Remap = new SJExt();
        Encoding SJIS = Encoding.GetEncoding(932);
#endif
        private void ImportList() {
#if SJIS
            for (int i = 0; i < Strs.Length; i++)
                Strs[i] = SJIS.GetString(Remap.GetBytes(StringList.Items[i].ToString()));
#else
            for (int i = 0; i < Strs.Length; i++)
                Strs[i] = StringList.Items[i].ToString();
#endif
        }

        bool BM = false;
        private void Save(string As) {
            SaveBinary.FileName = As;
            SaveBinary_FileOk(null, null);
        }
        private void SaveBinary_FileOk(object sender, CancelEventArgs e) {
            ImportList();
            byte[] script = Editor.Export(Strs);
            File.WriteAllBytes(SaveBinary.FileName, script);

            if (!BM)
                MessageBox.Show("File Saved", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        Dictionary<string, string> Cache = new Dictionary<string, string>();
        bool Error = false;
        bool Abort = false;
        bool CanAbort = false;
        private void BntProc_Click(object sender, EventArgs e) {
            if (CanAbort) {
                Abort = true;
                return;
            }
            if (CkOffline.Checked && !LEC.ServerIsOpen(Port.Text)) {
                MessageBox.Show("Invalid Server Port", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Error = true;
                return;
            }
            CanAbort = true;
            Abort = false;
            BntProc.Text = "Abort!";
            if (MassMode.Checked) {
                MassiveTranslate();
            } else {
                LinePerLineTranslate();
            }
            Text = "TLBOT - (" + Path.GetFileName(OpenBinary.FileName) + ") - In Game Machine Translation";
            CanAbort = false;
            BntProc.Text = "Translate!";
            if (!BM) {
                MessageBox.Show("All Lines Translated.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LinePerLineTranslate() {
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
                PrefixAndSufix(ref Input, false);

                BreakLine(ref Input, false);
                string[] Lines = Input.Split('\n');

                for (int x = 0; x < Lines.Length; x++) {
                    if (string.IsNullOrWhiteSpace(Lines[x]) || (Lines[x].Length > 3 && string.IsNullOrEmpty(NullStringWorker(Lines[x]))))
                        continue;
                    string Result = string.Empty;
                    if (!Cache.ContainsKey(Lines[x])) {
                        if (InputLang.Text == OutLang.Text) {
                            Result = Lines[x];
                        } else {
                            if (VM != null)
                                Result = VM.Call("Main", "BeforeTL", Result);
                            Translate(out Result, Lines[x]);
                            if (Result.ToLower().StartsWith("-benzóico")) {
                                try {
                                    Result = LEC.Translate(Lines[x], InputLang.Text, OutLang.Text, LEC.Gender.Male, LEC.Formality.Formal, Port.Text);
                                } catch { }
                            }
                            if (Result == null || Result.ToLower().StartsWith("-benzóico"))
                                continue;
                            if (VM != null)
                                Result = VM.Call("Main", "AfterTL", Result);
                            FixTL(ref Result, Lines[x]);
                            Result = FixTLAlgo2(Result, Lines[x]);
                            Cache.Add(Lines[x], Result);
                        }
                    } else
                        Result = Cache[Lines[x]];

                    Lines[x] = Result;
                }

                string Translation = string.Empty;
                for (int x = 0; x < Lines.Length; x++)
                    Translation += Lines[x] + "\n";
                Translation = Translation.Substring(0, Translation.Length - 1);

                BreakLine(ref Translation, true);

                PrefixAndSufix(ref Translation, true);
                StringList.Items[i] = Translation;
                StringList.SelectedIndex = i;
                Application.DoEvents();
            }
        }

        private void MassiveTranslate() {
            Text = "TLBOT - Initializing Massive Translation...";
            var Strings = new List<string>();
            var IndMap = new Dictionary<int, int>();
            var LinesCount = new Dictionary<int, int>();
            var Prx = new Dictionary<int, string>();
            var Sfx = new Dictionary<int, string>();
            var RetFlg = new Dictionary<int, bool>();
            var DecFlg = new Dictionary<int, bool>();
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
                if (Cache.ContainsKey(Input)) {
                    StringList.Items[i] = Cache[Input];
                    continue;
                }

                PrefixAndSufix(ref Input, false);
                Prx[i] = Prefix;
                Sfx[i] = Sufix;

                BreakLine(ref Input, false);
                DecFlg[i] = DecodedFlag;
                RetFlg[i] = ReturnFlag;

                string[] Lines = Input.Split('\n');

                for (int x = 0; x < Lines.Length; x++) {
                    IndMap[Strings.Count()] = i;
                    Strings.Add(Lines[x]);
                }
                StringList.Items[i] = string.Empty;
                Application.DoEvents();
            }
            Text = "TLBOT - Translating...";

            string[] Original = Strings.ToArray();
            if (VM != null)
                Original = VM.Call("Main", "BeforeTL", (object)Original);

            string[] Translated;
            if (ckDoubleStep.Checked) {
                Translated = Google.Translate(Original, InputLang.Text, "EN");
                Translated = Google.Translate(Translated, "EN", OutLang.Text);
            } else
                Translated = Google.Translate(Original, InputLang.Text, OutLang.Text);

            if (VM != null)
                Translated = VM.Call("Main", "AfterTL", (object)Translated);

            for (int i = 0; i < Translated.Length; i++) {
                int Index = IndMap[i];
                bool Continue = (i + 1 < Translated.Length && IndMap[i + 1] == Index);
                if (Continue) {
                    StringList.Items[Index] += Translated[i] + "\n";
                } else {
                    string Str = StringList.Items[Index].ToString();
                    Str += Translated[i];

                    Prefix = Prx[Index];
                    Sufix = Sfx[Index];
                    DecodedFlag = DecFlg[Index];
                    ReturnFlag = RetFlg[Index];

                    BreakLine(ref Str, true);
                    PrefixAndSufix(ref Str, true);

                    StringList.Items[Index] = Str;

                    if (i % 10 == 0 || !(i + 1 < Translated.Length))
                        StringList.SelectedIndex = Index;

                    if (!Cache.ContainsKey(Original[i]))
                        Cache.Add(Original[i], Translated[i]);

                    if (i % 10 == 0 || !(i + 1 < Translated.Length))
                        Application.DoEvents();
                }
            }
        }

        private string NullStringWorker(string Str) {
            return Str.Replace(Str[0] + "", "").Replace(Str[1] + "", "").Replace(".", "").Replace("!", "").Replace("?", "").Replace("-", "").Trim();
        }

        bool ReturnFlag;
        bool DecodedFlag;

        private void BreakLine(ref string Input, bool Mode) {
            if (Mode) {
                if (ReturnFlag)
                    Input = Input.Replace("\n", DecodedFlag ? "\\r" : "\r");
                if (!ReturnFlag && DecodedFlag)
                    Input = Input.Replace("\n", "\\n");
            } else {
                DecodedFlag = Input.Contains("\\n") || Input.Contains("\\r");
                ReturnFlag = Input.Contains(DecodedFlag ? "\\r" : "\r") && !Input.Contains(DecodedFlag ? "\\n" : "\n");;
                if (ReturnFlag)
                    Input = Input.Replace(DecodedFlag ? "\\r" : "\r", "\n");
                if (DecodedFlag)
                    Input = Input.Replace("\\n", "\n");
            }
        }

        string Prefix;
        string Sufix;
        private void PrefixAndSufix(ref string Str, bool Mode, bool ReadOnly = false) {
            List<string> TrimData = new List<string>(new string[] { "\"", "[", "“", "［", "《", "«", "「", "『", "【", "]", "”", "］", "》", "»", "」", "』", "】", "～" });

            if (VM != null) {
                string[] TD = VM.Call("Main", "Trim");
                foreach (string Trim in TD) {
                    TrimData.Add(Trim);
                }
            }
            if (Mode) {
                Str = Prefix + Str + Sufix;
            } else {
                if (!ReadOnly) {
                    Prefix = string.Empty;
                    Sufix = string.Empty;
                }
                string Before = string.Empty;
                while (Before != Str) {
                    Before = Str;
                    foreach (string Trim in TrimData) {
                        if (Str.StartsWith(Trim)) {
                            if (!ReadOnly)
                                Prefix += Trim;
                            Str = Str.Substring(Trim.Length, Str.Length - Trim.Length);
                        }

                        if (Str.EndsWith(Trim)) {
                            if (!ReadOnly)
                                Sufix = Trim + Sufix;
                            Str = Str.Substring(0, Str.Length - Trim.Length);
                        }
                    }
                }
            }
        }

        private void Translate(out string Translation, string Input) {
            int tries = -1;
            Translation = null;
            while (tries++ < 5 && Translation == null) {
                if (ckDoubleStep.Checked) {
                    try {
                        Translation =
                            CkOffline.Checked ?
                            LEC.Translate(Input, InputLang.Text, "EN", LEC.Gender.Male, LEC.Formality.Formal, Port.Text) :
                            Google.Translate(Input, InputLang.Text, "EN");
                        Translation =
                            CkOffline.Checked ?
                            LEC.Translate(Translation, "EN", OutLang.Text, LEC.Gender.Male, LEC.Formality.Formal, Port.Text) :
                            Google.Translate(Translation, "EN", OutLang.Text);
                    }
                    catch { }
                } else
                    try {
                        Translation =
                            CkOffline.Checked ?
                            LEC.Translate(Input, InputLang.Text, OutLang.Text, LEC.Gender.Male, LEC.Formality.Formal, Port.Text) :
                            Google.Translate(Input, InputLang.Text, OutLang.Text);
                    }
                    catch { }
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
                string text = StringList.Items[i].ToString().Replace("\\n", "\n").Replace("\\r", "\r");
                PrefixAndSufix(ref text, false, true);
                bool Status = !string.IsNullOrWhiteSpace(text);
                int Process = 0;
                bool Asian = InputLang.Text == "JA" || InputLang.Text == "CH";
                bool Russian = InputLang.Text == "RU";
                while (Status) {
                    switch (Process) {
                        default:
                            goto ExitWhile;
                        case 0:
                            if (Russian) {
                                Status = MinimiumFound(text, Properties.Resources.RusCommon, text.Length / 4);
                                if (Status)
                                    goto ExitWhile;
                            }
                            bool Alternate = text.StartsWith("<") && text.EndsWith(">");
                            Status = !ContainsOR(text, Alternate ? "@,§,$,\\,|,_,/" : "@,§,$,\\,|,_,<,>,/");
                            break;
                        case 1:
                            Status = NumberLimiter(text, text.Length / 4);
                            break;
                        case 2:
                            Status = text.Length >= 3 || EndsWithOr(text, ".,!,?");
                            break;
                        case 3:
                            if (Asian)
                                Status = MinimiumFound(text, Properties.Resources.JapCommon, text.Length / 4);
                            else 
                                Status = text.Contains(((char)32).ToString()) || EndsWithOr(text, ".\",!\",?\",.,!,?");
                            break;
                        case 4:
                            if (!Asian && !Russian)
                                break;
                            Status = !ContainsOR(text, "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,x,w,y,z,▽,★,♪");
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
            return found >= min || found == text.Length;
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
                Text = string.Format("TLBOT - {2} ({0}/{1} - {3}%)", StringList.SelectedIndex, StringList.Items.Count, System.IO.Path.GetFileName(LastScript), (int)(((double)StringList.SelectedIndex / StringList.Items.Count) * 100));

                string txt = StringList.SelectedItem.ToString();
                Encode(ref txt, true);
                SearchTB.Text = txt;
            }
            catch { }
        }
        private static void Encode(ref string String, bool Enable) {
            if (Enable) {
                string Result = string.Empty;
                foreach (char c in String) {
                    if (c == '\n')
                        Result += "\\n";
                    else if (c == '\\')
                        Result += "\\\\";
                    else if (c == '\t')
                        Result += "\\t";
                    else if (c == '\r')
                        Result += "\\r";
                    else
                        Result += c;
                }
                String = Result;
            } else {
                string Result = string.Empty;
                bool Special = false;
                foreach (char c in String) {
                    if (c == '\\' & !Special) {
                        Special = true;
                        continue;
                    }
                    if (Special) {
                        switch (c.ToString().ToLower()[0]) {
                            case '\\':
                                Result += '\\';
                                break;
                            case 'n':
                                Result += '\n';
                                break;
                            case 't':
                                Result += '\t';
                                break;
                            case 'r':
                                Result += '\r';
                                break;
                            default:
                                throw new Exception("\\" + c + " Isn't a valid string escape.");
                        }
                        Special = false;
                    } else
                        Result += c;
                }
                String = Result;
            }
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
                    byte[] Script = File.ReadAllBytes(FD.FileNames[i]);
                    Wrapper Temp = new Wrapper();
                    string[] find = Temp.Import(Script, Path.GetExtension(FD.FileNames[i]), true);

                    foreach (string Str in find)
                        if (Str.Contains(content)) {
                            Founds += Path.GetFileName(FD.FileNames[i]) + "\n";
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
        private List<string> BlackSplitList = new List<string>(
        new string[] {
          "auto", "me", "se", "lhe", "tes", "te", "ti", "a", "bem", "mal", "bens",
          "recem", "recém", "line", "como", "like", "pré", "vice", "anti", "pró",
          "pós", "ante", "porta", "tipo", "guarda", "mail", "o"
        });

        public string FixTLAlgo2(string TL, string Ori) {
            if (!Ori.Contains("-") && !Ori.Contains("—"))
                return TL;
            string[] NewWords = TL.Split(' ');
            for (int i = 0; i < NewWords.Length; i++) {
                string Word = NewWords[i];
                if (Word.Contains("-") || Word.Contains("—")) {
                    string[] Splited = Word.Split('-', '—');
                    bool Repeat = true;
                    try {
                        for (int x = 0; x < Splited.Length - 1; x++) {
                            if (Splited[x].ToLower() != Splited[0].ToLower())
                                Repeat = false;
                        }
                        for (int x = 0; x < Splited.Length; x++) {
                            if (string.IsNullOrEmpty(Splited[0]) || BlackSplitList.Contains(Splited[0].ToLower()) || Splited[x].Length == 0) {
                                Repeat = false;
                                throw new Exception();
                            }
                        }
                        string last = Splited[Splited.Length - 1].ToLower();
                        foreach (string sufix in new string[] { "sama", "san", "chan", "kun", "chi", "senpai", "sensei", "dono" }) {
                            if (RepeatCheck(last, sufix)) {
                                Repeat = false;
                                break;
                            }
                        }
                        if ((Ori.ToLower().Contains(Splited[0].ToLower() + "-"))) {
                            string MergedWord = string.Empty;
                            foreach (string str in Splited)
                                MergedWord += str;
                            string Trie = string.Empty;
                            MergedWord = MergedWord.Replace("?", "").Replace("!", "").Replace(".", "");
                            Translate(out Trie, MergedWord);
                            if (!string.IsNullOrEmpty(Trie) && Trie != MergedWord) {
                                Repeat = false;
                                NewWords[i] = Trie;
                            }
                        }
                        for (int x = 0; x < Splited.Length - 1; x++) {
                            if (last.Length <= Splited[x].Length || (Splited[0][0].ToString().ToLower() != Splited[1][0].ToString().ToLower() && Splited.Length >= 3))
                                Repeat = false;
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
            bool Trigger = !AllowMissMatch;
            Str = Str.ToLower();
            Word = Word.ToLower();
            for (int i = 0, x = 0; i < Word.Length; i++) {
                char Atual = Word[i];
                while (x < Str.Length && Str[x] == '.' || Str[x] == '?' || Str[x] == '!')
                    x++;
                if (x >= Str.Length)
                    return false;
                if (Str[x] != Atual && Trigger)
                    return false;
                else if (Str[x] != Atual) {
                    Trigger = true;
                    while (x < Str.Length && Str[x] != Atual)
                        x++;
                    if (x >= Str.Length)
                        return false;
                }
                while (x < Str.Length && Str[x] == Atual)
                    x++;
            }
            return true;
        }

        private void BntBathProc_Click(object sender, EventArgs e) {
            OpenFileDialog FD = new OpenFileDialog();
            FD.Multiselect = true;
            FD.Filter = Filter;
            FD.Title = "Select all files or directorries to find the string";
            FD.Multiselect = true;
            string log = string.Empty;

            DialogResult dr = FD.ShowDialog();
            if (dr != DialogResult.OK)
                return;

            bool AutoSelect = MessageBox.Show("You want the TLBOT select the string automatically?\n\nIf not, all strings are selected.", "TLBOT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            BM = true;
            Error = false;
            foreach (string File in FD.FileNames) {
                try { AutoProcess(File, AutoSelect); }
                catch {
                    log += "\nError: " + Path.GetFileName(File);
                }
            }

            BM = false;
            MessageBox.Show(string.Empty == log ? "Operation Cleared!" : "Sucess, but that files have a problem:" + log, "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AutoProcess(string File, bool AutoSelect) {
            Abort = CanAbort = false;
            Open(File);
            if (StringList.Items.Count == 0)
                throw new Exception("Failed.");
            if (AutoSelect)
                BotSelect_Click(null, null);
            else
                MassSelect_Click(null, null);
            Application.DoEvents();
            BntProc_Click(null, null);
            if (Error)
                return;
            Save(File);
        }

        int LastSearchIndex = -2;
        private void SearchKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\n' || e.KeyChar == '\r') {
                string bak = Text;
                e.Handled = true;
                string text = SearchTB.Text.ToLower();

                int i = (int)Begin.Value;
                if (LastSearchIndex == StringList.SelectedIndex)
                    i = LastSearchIndex + 1;

                for (; i < End.Value; i++) {
                    Text = "Searching... " + i + "/" + (int)End.Value;
                    string Str = StringList.Items[i].ToString().ToLower();
                    if (Str.Contains(text)) {
                        Text = bak;
                        LastSearchIndex = i;
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

        string lf = string.Empty;
        private void FoldTrans_Click(object sender, EventArgs e) {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Folder to translate all files.";
            folder.SelectedPath = lf;
            string log = string.Empty;
            if (folder.ShowDialog() != DialogResult.OK)
                return;

            SelectExtensions exts = new SelectExtensions();
            exts.Default = "*";
            if (exts.ShowDialog() != DialogResult.OK)
                return;


            lf = folder.SelectedPath;
            bool AutoSelect = MessageBox.Show("You want the TLBOT select the string automatically?\n\nIf not, all strings are selected.", "TLBOT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            BM = true;
            string[] Files = Directory.GetFiles(folder.SelectedPath, "*.*", SearchOption.AllDirectories);
            Files = (from string f in Files where exts.Extensions.Contains(Path.GetExtension(f).ToLower()) select f).ToArray();

            foreach (string File in Files)
                try { AutoProcess(File, AutoSelect); }
                catch {
                    log += "\nError: " + System.IO.Path.GetFileName(File);
                }
            BM = false;


            if (log == string.Empty) {
                MessageBox.Show("Sucess!", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                MessageBox.Show("LOG:\n" + log, "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string DBPath = AppDomain.CurrentDomain.BaseDirectory + "Cache.bin";
        private string LastScript;

        private void SaveDB_Click(object sender, EventArgs e) {
            StructWriter Writer = new StructWriter(new StreamWriter(DBPath).BaseStream, false, Encoding.UTF8);
            Writer.Write(Cache.Keys.Count);
            string[] Ori = new string[Cache.Keys.Count];
            Cache.Keys.CopyTo(Ori, 0);
            string[] TL = new string[Cache.Values.Count];
            Cache.Values.CopyTo(TL, 0);

            for (int i = 0; i < Cache.Keys.Count; i++){
                Entry entry = new Entry() {
                    Original = Ori[i],
                    Translation = TL[i]
                };
                Writer.WriteStruct(ref entry);
            }
            Writer.Close();
            MessageBox.Show("Cache Translation Saved.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        

        struct Entry {
            [PString(PrefixType = Const.INT32)]
            internal string Original;
            [PString(PrefixType = Const.INT32)]
            internal string Translation;
        }

        private void LoadDB_Click(object sender, EventArgs e) {
            StructReader Reader = new StructReader(new StreamReader(DBPath).BaseStream, false, Encoding.UTF8);
            int max = Reader.ReadInt32();
            for (int i = 0; i < max; i++) {
                Entry Entry = new Entry();
                Reader.ReadStruct(ref Entry);

                if (!Cache.ContainsKey(Entry.Original))
                    Cache.Add(Entry.Original, Entry.Translation);
            }
            MessageBox.Show("Cache Translation Loaded.\nTotal Cache Entries: " + Cache.Keys.Count, "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void ProgramOpen(object sender, EventArgs e) {
            Again:
            ;
            string PluginDir = HighLevelCodeProcessator.AssemblyDirectory + "\\Plugins";
            if (!Directory.Exists(PluginDir) || Directory.GetFiles(PluginDir, "*.ini").Concat(Directory.GetFiles(PluginDir, "*.inf")).ToArray().Length == 0) {
                if (MessageBox.Show("No plugins Detected...\nPlugins Dir:\n" + PluginDir, "TLBOT", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    goto Again;
                else
                    Close();
            }

            string PATH = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", PluginDir + ";" + PATH);
            Port.Text = LEC.TryDiscoveryPort();
            OpenBinary.Filter = Filter;
            SaveBinary.Filter = Filter;
            LblInfo.Text = string.Format(LblInfo.Text, "SacanaWrapper");
            string TLFilter = AppDomain.CurrentDomain.BaseDirectory + "Filter.cs";
            if (File.Exists(TLFilter)) {
                reload:;
                try {
                    VM = new DotNetVM(File.ReadAllText(TLFilter));
                } catch {
                    DialogResult dr = MessageBox.Show("Failed to Initialize the Filter.", "TLBOT", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (dr == DialogResult.Retry)
                        goto reload;
                }
                BlackList = new List<string>(VM.Call("Main", "GetBlackList"));
                    string[] List = VM.Call("Main", "GetReplaces");
                    for (int i = 0; i < List.Length; i += 2)
                        Replace.Add(List[i], List[i + 1]);
            }
        }

        private void OverwriteBnt_Click(object sender, EventArgs e) {
            if (StringList.SelectedIndex == -1) {
                MessageBox.Show("Before Overwrite a line, you need select he in the list.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string txt = SearchTB.Text;
            Encode(ref txt, false);
            StringList.Items[StringList.SelectedIndex] = txt;
        }

        private void VerifyLang(object sender, EventArgs e) {
            bool DoubleStepPossible = InputLang.Text != "EN" && OutLang.Text != "EN";
            if (!DoubleStepPossible)
                ckDoubleStep.Checked = false;
            ckDoubleStep.Enabled = DoubleStepPossible;
        }
        
        private void ExportClicked(object sender, EventArgs e) {
            string Output = AppDomain.CurrentDomain.BaseDirectory + "Strings.strs";
            if (File.Exists(Output))
                File.Delete(Output);

            StructWriter Writer = new StructWriter(Output);
            Writer.Write(StringList.Items.Count);
            foreach (string String in StringList.Items) {
                Writer.Write(String, StringStyle.PString);
            }
            Writer.Close();
            MessageBox.Show("Strings Exported.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ImportClick(object sender, EventArgs e) {
            string Input = AppDomain.CurrentDomain.BaseDirectory + "Strings.strs";
            if (!File.Exists(Input))
                return;
            StructReader Reader = new StructReader(Input);
            int Max = Reader.ReadInt32();
            if (End.Value > Max) {
                Max = (int)End.Value;
            }
            for (int i = (int)Begin.Value; i < Max; i++) {
                if (i < StringList.Items.Count)
                    StringList.Items[i] = Reader.ReadString(StringStyle.PString);
                else
                    StringList.Items.Add(Reader.ReadString(StringStyle.PString));
            }
            MessageBox.Show("Strings Imported.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OfflineChanged(object sender, EventArgs e) {
            MassMode.Checked = !CkOffline.Checked;
            MassMode.Enabled = !CkOffline.Checked;
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
}