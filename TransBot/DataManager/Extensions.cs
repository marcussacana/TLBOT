#define SINGLE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TLIB;

namespace TLBOT.DataManager {
    public static partial class Extensions {

        internal static T[] AppendArray<T>(this T[] Array, T Item) =>
            Array.AppendArray(new T[] { Item });

        internal static T[] AppendArray<T>(this T[] Array, T[] Items) {
            T[] NewArray = new T[Array.Length + Items.Length];
            Array.CopyTo(NewArray, 0);
            Items.CopyTo(NewArray, Array.Length);

            return NewArray;
        }
#if SINGLE
        internal static string Translate(this string String, string SourceLanguage, string TargetLanguage, Translator Client) {
            if (Program.Cache.ContainsKey(String))
                return Program.Cache[String];

            for (int i = 0; i < 3; i++) {
                try {
                    string Result = string.Empty;
                    switch (Client) {
                        case Translator.Bing:
                        case Translator.BingNeural:
                            Result = Bing.Translate(String, SourceLanguage, TargetLanguage);
                            break;
                        case Translator.Google:
                            Result = Google.Translate(String, SourceLanguage, TargetLanguage);
                            break;
                        case Translator.LEC:
                            Result = LEC.Translate(String, SourceLanguage, TargetLanguage, LEC.Gender.Male, LEC.Formality.Informal, Program.LECPort);
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(Result))
                        continue;

                    Program.Cache[String] = Result;
                    return Result;
                } catch {
                    Thread.Sleep(100);
                }
            }

            return String;
        }
#else
        internal static string Translate(this string String, string SourceLang, string TargetLang, Translator Client) => TranslateMassive(new string[] { String }, SourceLang, TargetLang, Client).First();
#endif

        internal static string[] TranslateMassive(this string[] Strings, string SourceLanguage, string TargetLanguage, Translator Client) {
            string[] NoCached = (from x in Strings where !Program.Cache.ContainsKey(x) select x).ToArray();
            string[] Result;

            for (int i = 0; i < 5; i++) {
                try {
                    if (NoCached?.Length == 0)
                        break;

                    Result = new string[0];
                    switch (Client) {
                        case Translator.Bing:
                        case Translator.BingNeural:
                            Result = Bing.Translate(NoCached, SourceLanguage, TargetLanguage, 300);
                            break;

                        case Translator.LEC:
                        case Translator.Google:
                            Result = Google.Translate(NoCached, SourceLanguage, TargetLanguage, 300);
                            break;
                    }

                    if (Result.Length != NoCached.Length)
                        continue;

                    for (uint x = 0; x < NoCached.Length; x++) {
                        if (NoCached[x] != Result[x])
                            Program.Cache[NoCached[x]] = Result[x];
                    }

                    int Misseds = NoCached.Length;
                    NoCached = (from x in Strings where !Program.Cache.ContainsKey(x) select x).ToArray();
                    if (Misseds == NoCached.Length)
                        break;
#if DEBUG
                } catch (Exception ex){
                    if (System.Diagnostics.Debugger.IsAttached)
                        System.Diagnostics.Debugger.Break();
#else
                } catch { 
#endif
                    Thread.Sleep(100);
                }
            }

            Result = new string[Strings.LongLength];
            for (uint i = 0; i < Strings.Length; i++) {
                if (Program.Cache.ContainsKey(Strings[i]))
                    Result[i] = Program.Cache[Strings[i]];
                else
                    Result[i] = Strings[i];
            }

            return Result;
        }

        internal static string[] TranslateMultithread(this string[] Strings, string SourceLanguage, string TargetLanguage, Translator Client, Action<uint> ProgressChanged = null) {
            string[] NoCached = (from x in Strings where !Program.Cache.ContainsKey(x) select x).ToArray();
            string[] Result;

            for (int i = 0; i < 5; i++) {
                try {
                    uint Finished = 0;
                    Result = new string[NoCached.Length];

                    Thread Thread = new Thread(() => {
                        var Options = new ParallelOptions() {
                            MaxDegreeOfParallelism = 20
                        };

                        Parallel.For(0, NoCached.Length, Options, x => {
                            if (Result[x] != null)
                                return;

                            Result[x] = NoCached[x].Translate(SourceLanguage, TargetLanguage, Client);
                            Finished++;
                        });
                    });
                    Thread.Start();

                    DateTime LastUpdateTime = DateTime.Now;
                    uint LastUpdate = 0;
                    while (Finished < Result.Length) {
                        if (ProgressChanged != null && LastUpdate != Finished) {
                            LastUpdate = Finished;
                            ProgressChanged(LastUpdate);
                            LastUpdateTime = DateTime.Now;
                        }

                        //Timeout Progress
                        if ((DateTime.Now - LastUpdateTime).TotalSeconds > 15) {
                            if (Thread.IsRunning()) {
                                Thread.Abort();
                            }
                            LastUpdateTime = DateTime.Now;
                            Thread.Start();
                        }

                        Thread.Sleep(10);
                    }
                } catch {
                    Thread.Sleep(100);
                }
            }

            Result = new string[Strings.LongLength];
            for (uint i = 0; i < Strings.Length; i++) {
                if (Program.Cache.ContainsKey(Strings[i]))
                    Result[i] = Program.Cache[Strings[i]];
                else
                    Result[i] = Strings[i];
            }

            return Result;
        }

        public static void Translate(this Control Control, string TargetLanguage, Translator Client) {
            try {
                string TL = Control.Text.Translate("PT", TargetLanguage, Client);
                Control.Invoke(new MethodInvoker(() => {
                    try {
                        Control.Text = TL;
                    } catch { }
                }));
            } catch { }
        }

        public static void Translate(this Form Form, string TargetLanguage, Translator Client) {
            foreach (Control Control in GetControlHierarchy(Form)) {
                if (Control is TextBox || Control is RichTextBox || Control is ComboBox)
                    continue;

                Control.Translate(TargetLanguage, Client);
            }
        }

        private static IEnumerable<Control> GetControlHierarchy(Control root) {
            var queue = new Queue<Control>();

            queue.Enqueue(root);

            do {
                var control = queue.Dequeue();

                yield return control;

                foreach (var child in control.Controls.OfType<Control>())
                    queue.Enqueue(child);

            } while (queue.Count > 0);

        }

        private static bool IsRunning(this Thread Thread) {
            return Thread.ThreadState == ThreadState.Running || Thread.ThreadState == ThreadState.Background
                || Thread.ThreadState == ThreadState.WaitSleepJoin;
        }

        public static string Unescape(this string String) {
            if (string.IsNullOrWhiteSpace(String))
                return String;

            string Result = string.Empty;
            bool Special = false;
            foreach (char c in String) {
                if (c == '\\' & !Special) {
                    Special = true;
                    continue;
                }
                if (Special) {
                    switch (c) {
                        case '\\':
                            Result += '\\';
                            break;
                        case 'N':
                        case 'n':
                            Result += '\n';
                            break;
                        case 'T':
                        case 't':
                            Result += '\t';
                            break;
                        case 'R':
                        case 'r':
                            Result += '\r';
                            break;
                        case '"':
                            Result += '"';
                            break;
                        default:
                            Result += "\\" + c;
                            break;
                    }
                    Special = false;
                } else
                    Result += c;
            }

            return Result;
        }

        public static string Escape(this string String) {
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
                else if (c == '"')
                    Result += "\\\"";
                else
                    Result += c;
            }
            return Result;
        }

        private static bool VerifingDialog = false;
        public static bool IsDialogue(this string String) {
            if (string.IsNullOrWhiteSpace(String))
                return false;

            if (Program.ForceDialogues.ContainsKey(String))
                return Program.ForceDialogues[String];

            if (Program.FilterSettings.UseDB && Program.Cache.ContainsKey(String))
                return true;


            string[] DenyList = Program.FilterSettings.DenyList.Unescape().Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] IgnoreList = Program.FilterSettings.IgnoreList.Unescape().Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            Quote[] Quotes = Program.FilterSettings.QuoteList.Unescape().Split('\n')
                .Where(x => x.Length == 2)
                .Select(x => {
                    return new Quote() { Start = x[0], End = x[1] };
                }).ToArray();

            string Str = String;
            foreach (string Ignore in IgnoreList)
                Str = Str.Replace(Ignore, "");

            if (!VerifingDialog)
                foreach (var Otimizator in Program.ExternalPlugins) {
                    Otimizator.BeforeTranslate(ref Str, uint.MaxValue);
                }

            VerifingDialog = true;
            foreach (string Deny in DenyList)
                if (Str.ToLower().Contains(Deny.ToLower())) {
                    VerifingDialog = false;
                    return false;
                }

            Str = Str.Replace(Program.WordwrapSettings.LineBreaker, "\n");


            string[] Words = Str.Split(' ');

            char[] PontuationJapList = new char[] { '。', '？', '！', '…', '、', '―' };
            char[] SpecialList = new char[] { '_', '=', '+', '#', ':', '$', '@' };
            char[] PontuationList = new char[] { '.', '?', '!', '…', ',' };
            int Spaces = Str.Where(x => x == ' ' || x == '\t').Count();
            int Pontuations = Str.Where(x => PontuationList.Contains(x)).Count();
            int WordCount = Words.Where(x => x.Length >= 2 && !string.IsNullOrWhiteSpace(x)).Count();
            int Specials = Str.Where(x => char.IsSymbol(x)).Count();
            Specials += Str.Where(x => char.IsPunctuation(x)).Count() - Pontuations;
            int SpecialsStranges = Str.Where(x => SpecialList.Contains(x)).Count();

            int Uppers = Str.Where(x => char.IsUpper(x)).Count();
            int Latim = Str.Where(x => x >= 'A' && x <= 'z').Count();
            int Numbers = Str.Where(x => x >= '0' && x <= '9').Count();
            int NumbersJap = Str.Where(x=> x >= '０' && x <= '９').Count();
            int JapChars = Str.Where(x => (x >= '、' && x <= 'ヿ') || (x >= '｡' && x <= 'ﾝ')).Count();
            int Kanjis = Str.Where(x => x >= '一' && x <= '龯').Count();


            bool IsCaps = Optimizator.CaseFixer.GetLineCase(Str) == Optimizator.CaseFixer.Case.Upper;
            bool IsJap = JapChars + Kanjis > Latim/2;


            //More Points = Don't Looks a Dialogue
            //Less Points = Looks a Dialogue
            int Points = 0;

            if (Str.Length > 4) {
                string ext = Str.Substring(Str.Length - 4, 4);
                try {
                    if (System.IO.Path.GetExtension(ext).Trim('.').Length == 3)
                        Points += 2;
                } catch { }
            }

            bool BeginQuote = false;
            Quote? LineQuotes = null;
            foreach (Quote Quote in Quotes) {
                BeginQuote |= Str.StartsWith(Quote.Start.ToString());

                if (Str.StartsWith(Quote.Start.ToString()) && Str.EndsWith(Quote.End.ToString())) {
                    Points -= 3;
                    LineQuotes = Quote;
                    break;
                } else if (Str.StartsWith(Quote.Start.ToString()) || Str.EndsWith(Quote.End.ToString())) {
                    Points--;
                    LineQuotes = Quote;
                    break;
                }
            }
            try {
                char Last = (LineQuotes == null ? Str.Last() : Str.TrimEnd(LineQuotes.Value.End).Last());
                if (IsJap && PontuationJapList.Contains(Last))
                    Points -= 3;

                if (!IsJap && (PontuationList).Contains(Last))
                    Points -= 3;

            } catch { }
            try {
                char First = (LineQuotes == null ? Str.First() : Str.TrimEnd(LineQuotes.Value.Start).First());
                if (IsJap && PontuationJapList.Contains(First))
                    Points -= 3;

                if (!IsJap && (PontuationList).Contains(First))
                    Points -= 3;

            } catch { }

            if (!IsJap) {
                foreach (string Word in Words) {
                    int WNumbers = Word.Where(c => char.IsNumber(c)).Count();
                    int WLetters = Word.Where(c => char.IsLetter(c)).Count();
                    if (WLetters > 0 && WNumbers > 0) {
                        Points += 2;
                    }
                    if (Word.Trim(PontuationList).Where(c => PontuationList.Contains(c)).Count() != 0) {
                        Points += 2;
                    }
                }
            }

            if (!BeginQuote && !char.IsLetter(Str.First()))
                Points += 2;

            if (Specials > WordCount)
                Points++;

            if (Specials > Latim + JapChars)
                Points += 2;

            if (SpecialsStranges > 0)
                Points += 2;

            if (SpecialsStranges > 3)
                Points++;

            if ((Pontuations == 0) && (WordCount <= 2) && !IsJap)
                Points++;

            if (Uppers > Pontuations + 2 && !IsCaps)
                Points++;

            if (Spaces > WordCount * 2)
                Points++;

            if (IsJap && Spaces == 0)
                Points--;

            if (!IsJap && Spaces == 0)
                Points += 2;

            if (WordCount <= 2 && Numbers != 0)
                Points += (int)(Str.PercentOf(Numbers) / 10);

            if (Str.Length <= 3 && !IsJap)
                Points++;

            if (Numbers >= Str.Length)
                Points += 3;

            if (IsJap && Kanjis / 2 > JapChars)
                Points--;

            if (IsJap && JapChars > Kanjis)
                Points--;

            if (IsJap && Latim != 0)
                Points += (int)(Str.PercentOf(Latim) / 10) + 2;

            if (IsJap && NumbersJap != 0)
                Points += (int)(Str.PercentOf(NumbersJap) / 10) + 2;

            if (IsJap && Numbers != 0)
                Points += (int)(Str.PercentOf(Numbers) / 10) + 3;

            if (IsJap && Pontuations != 0)
                Points += (int)(Str.PercentOf(Pontuations) / 10) + 2;

            if (Str.Trim() == string.Empty)
                return false;

            if (Str.Trim().Trim(Str.Trim().First()) == string.Empty)
                Points += 2;

            if (IsJap != Program.FromAsian)
                return false;

            VerifingDialog = false;
            return Points < Program.FilterSettings.Sensitivity;
        }

        internal static double PercentOf(this string String, int Value) {
            var Result = Value / (double)String.Length;
            return Result * 100;
        }
    }
}
