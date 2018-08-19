using System.Collections.Generic;
using System.Linq;
using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    public class StutterFixer : IOptimizator {

        static Quote[] Quotes = Program.FilterSettings.QuoteList.Unescape().Split('\n')
            .Where(x => x.Length == 2)
            .Select(x => {
                return new Quote() { Start = x[0], End = x[1] };
            }).ToArray();


        static Dictionary<uint, List<Quote>> QuoteDB = new Dictionary<uint, List<Quote>>();
        static Dictionary<uint, string> DB = new Dictionary<uint, string>();
        static Dictionary<uint, bool> EmptyPrefix = new Dictionary<uint, bool>();
        static Dictionary<uint, bool> EmptySufix = new Dictionary<uint, bool>();
        public void AfterOpen(ref string Line, uint ID) { }

        public void AfterTranslate(ref string Line, uint ID) {
            if (!EmptyPrefix[ID])
                Line = Line.TrimStart();
            if (!EmptySufix[ID])
                Line = Line.TrimEnd();
            if ((!ContainsBadStuttered(Line) && !DB.ContainsKey(ID)) || !DB.ContainsKey(ID)) {
                Line = RestoreQuotes(Line, ID);
                return;
            }
            Line = FixPharseStutter(Line, DB[ID]);
            Line = RestoreQuotes(Line, ID);
        }

        private string FixPharseStutter(string Pharse, string OriPharse) {
            string RPharse = string.Empty;
            string[] Words = Pharse.Split(' ');
            for (int i = 0; i < Words.Length; i++) {
                string Word = Words[i];
                string[] Parts = Word.Replace("ー", "-").Split('-');
                if (Parts.Length == 1) {
                    RPharse += Word + " ";
                    continue;
                }
                string Shutter = Word.Substring(0, Word.Length - Parts.Last().Length - 1);
                bool IsAStutter = OriPharse.ToLower().Contains(Shutter.ToLower());
                if (!IsBadStuttered(Word) || !IsAStutter) {
                    RPharse += Word + " ";
                    continue;
                }
                Word = Parts.Last();
                int Len = 0;
                string Prefix = string.Empty;
                foreach (string Part in Parts) {
                    if (Len + 1 >= Word.Length)
                        continue;
                    Prefix += Word.Substring(0, Len++) + "-";
                }

                RPharse += Prefix.TrimStart('-') + Word + " ";
            }

            return RPharse.Substring(0, RPharse.Length);
        }

        public void BeforeSave(ref string Line, uint ID) { }

        public void BeforeTranslate(ref string Line, uint ID) {
            Line = TrimQuotes(Line, ID);
            EmptyPrefix[ID] = Line.TrimStart() != Line;
            EmptySufix[ID] = Line.TrimEnd() != Line;
            if (!ContainsStuttered(Line))
                return;
            DB.Add(ID, Line);
        }

        private string RestoreQuotes(string Line, uint ID) {
            if (!QuoteDB.ContainsKey(ID))
                return Line;
            string Result = Line;
            foreach (Quote Quote in QuoteDB[ID].ToArray().Reverse()) {
                Result = Quote.Start + Result + Quote.End;
            }

            return Result;
        }

        private string TrimQuotes(string Line, uint ID) {
            if (string.IsNullOrWhiteSpace(Program.FilterSettings.QuoteList))
                return Line;
            QuoteDB[ID] = new List<Quote>();
            string Result = Line;
            string bak = string.Empty;
            while (Result != bak) {
                bak = Result;
                foreach (Quote Quote in Quotes) {
                    if (Line.StartsWith(Quote.Start.ToString()) && Result.EndsWith(Quote.End.ToString())) {
                        QuoteDB[ID].Add(Quote);
                        Result = Result.Substring(1, Result.Length - 2);
                    }
                }
            }
            return Result;
        }

        private bool ContainsStuttered(string Pharse) {
            foreach (string Word in Pharse.Split(' '))
                if (IsStuttered(Word))
                    return true;
            return false;
        }
        private bool ContainsBadStuttered(string Pharse) {
            foreach (string Word in Pharse.Split(' '))
                if (IsStuttered(Word))
                    return true;
            return false;
        }
        //B-BA-BAAKAAA! = TRUE
        //BAKA! = FALSE
        private bool IsStuttered(string Word) {
            int Matchs = 0;
            string[] Parts = Word.ToLower().Replace("ー", "-").Split('-');
            for (int i = 0; i < Parts.Length; i++) {
                bool IsFrist = i == 0;
                bool IsLast = i + 1 >= Parts.Length;
                string Bef = string.Empty, Nxt = string.Empty, Cur = Parts[i];
                if (!IsFrist)
                    Bef = Parts[i - 1];
                if (!IsLast)
                    Nxt = Parts[i + 1];
                if (IsFrist) {
                    if (Cur.StartsWith(Nxt) || Nxt.StartsWith(Cur))
                        Matchs++;
                    continue;
                }
                if (IsLast) {
                    if (Cur.StartsWith(Bef) || Bef.StartsWith(Cur))
                        Matchs++;

                    return Matchs >= Parts.Length - 1;
                }
                if (Cur.StartsWith(Nxt) || Nxt.StartsWith(Cur)) {
                    Matchs++;
                    continue;
                }
                Matchs--;
            }
            return false;
        }
        private bool IsBadStuttered(string Word) {
            int Matchs = 0;
            string[] Parts = Word.ToLower().Replace("ー", "-").Split('-');
            for (int i = 0; i < Parts.Length; i++) {
                bool IsFrist = i == 0;
                bool IsLast = i + 1 >= Parts.Length;
                string Bef = string.Empty, Nxt = string.Empty, Cur = Parts[i];
                if (!IsFrist)
                    Bef = Parts[i - 1];
                if (!IsLast)
                    Nxt = Parts[i + 1];
                if (IsFrist) {
                    if (Cur.StartsWith(Nxt) || Nxt.StartsWith(Cur))
                        Matchs++;
                    continue;
                }
                if (IsLast) {
                    if (Cur.StartsWith(Bef) || Bef.StartsWith(Cur))
                        Matchs--;
                    else
                        Matchs += 2;

                    return Matchs >= Parts.Length - 1;
                }
                if (Cur.StartsWith(Nxt) || Nxt.StartsWith(Cur)) {
                    Matchs++;
                    continue;
                }
                Matchs--;
            }
            return false;
        }
        public string GetName() {
            return "Stutter Fixer";
        }
    }
}
