using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLBOT.Optimizator {
    public class StutterFixer : IOptimizator {

        Dictionary<uint, string> DB = new Dictionary<uint, string>();
        public void AfterOpen(ref string Line, uint ID) { }

        public void AfterTranslate(ref string Line, uint ID) {
            if (!ContainsBadStuttered(Line) || !DB.ContainsKey(ID))
                return;
            Line = FixPharseStutter(Line, DB[ID]);
        }

        private string FixPharseStutter(string Pharse, string OriPharse) {
            string RPharse = string.Empty;
            string[] Words = Pharse.Split(' ');
            for (int i = 0; i < Words.Length; i++) {
                string Word = Words[i];
                string[] Parts = Word.Replace("ー", "-").Split('-');
                if (Parts.Length == 1)
                    continue;
                bool IsAStutter = OriPharse.ToLower().Contains(Word.ToLower().Substring(0, Word.Length - Parts.Last().Length - 1));
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
            if (!ContainsStuttered(Line))
                return;
            DB.Add(ID, Line);
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
