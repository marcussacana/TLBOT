using System;
using System.Collections.Generic;
using System.Linq;

namespace TLBOT.Optimizator {
    class CaseFixer : IOptimizator {
        Dictionary<uint, Case> CaseMap = new Dictionary<uint, Case>();
        public void AfterTranslate(ref string Line, uint ID) {}

        public void BeforeTranslate(ref string Line, uint ID) {}

        public enum Case {
            Lower, Upper, Normal, Title
        }

        public static string SetCase(string String, Case Case) {
            switch (Case) {
                case Case.Upper:
                    return String.ToUpper();
                case Case.Lower:
                    return String.ToLower();
                case Case.Normal:
                    string nResult = string.Empty;
                    string[] nWords = String.Trim().Split(' ');
                    bool FirstUpper = false;
                    for (int x = 0; x < nWords.Length; x++) {
                        bool DotUpper = false;
                        for (int i = 0; i < nWords[x].Length; i++) {
                            bool Upper = !FirstUpper;
                            if (!Upper && x != 0 && !DotUpper && char.IsPunctuation(nWords[x - 1].Last())) {
                                Upper = true;
                                DotUpper = true;
                            }
                            char c = nWords[x][i];
                            if (!char.IsLetter(c))
                                Upper = false;

                            if (Upper) {
                                FirstUpper = true;
                            }

                            nResult += Upper ? char.ToUpper(c) : char.ToLower(c);
                        }
                        nResult += ' ';
                    }
                    nResult = nResult.Substring(0, nResult.Length - 1);
                    return nResult;
                case Case.Title:
                    string tResult = string.Empty;
                    string[] tWords = String.Trim().Split(' ');
                    foreach (string Word in tWords) {
                        for (int i = 0; i < Word.Length; i++) {
                            tResult += i == 0 ? char.ToUpper(Word[i]) : Word[i];
                        }
                        tResult += ' ';
                    }
                    tResult = tResult.Substring(0, tResult.Length - 1);
                    return tResult;

                default:
                    throw new Exception("wtf");
            }
        }

        public static Case GetLineCase(string String) {
            try {
                string[] Words = String.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                Case[] WordsCase = new Case[Words.Length];
                for (int x = 0; x < Words.Length; x++) {
                    string Word = Words[x];
                    uint cLower = 0;
                    uint cUpper = 0;
                    uint cTitle = 0;
                    for (int i = 0; i < Word.Length; i++) {
                        char Char = Word[i];
                        if (Char > 0x8000)
                            return Case.Normal;
                        if (!char.IsLetter(Char))
                            continue;

                        if (i == 0) {
                            if (char.IsLetter(Char) && char.IsUpper(Char)) {
                                if (x == 0 || (char.IsPunctuation(Words[x - 1].Last())))
                                    cTitle++;
                            } else {
                                cLower++;
                            }
                        } else {
                            if (char.IsUpper(Char))
                                cUpper++;
                            break;
                        }
                    }
                    if (cLower == Word.Length)
                        WordsCase[x] = Case.Lower;
                    else if (cUpper == Word.Length)
                        WordsCase[x] = Case.Upper;
                    else if (cUpper + cLower == Word.Length)
                        WordsCase[x] = Case.Normal;
                    else if (cUpper > cLower && cUpper > cTitle)
                        WordsCase[x] = Case.Upper;
                    else if (cLower > cUpper && cLower > cTitle)
                        WordsCase[x] = Case.Lower;
                    else if (cTitle >= Words.Length)
                        WordsCase[x] = Case.Title;
                    else
                        WordsCase[x] = Case.Normal;
                }

                int Titles = (from x in WordsCase where x == Case.Title select x).Count();
                int Upper = (from x in WordsCase where x == Case.Upper select x).Count();
                int Normal = (from x in WordsCase where x == Case.Normal select x).Count();
                int Lower = (from x in WordsCase where x == Case.Lower select x).Count();

                if (Titles > Normal && Titles > Upper && Titles > Lower)
                    return Case.Title;
                if (Upper > Titles && Upper > Normal && Upper > Lower)
                    return Case.Upper;
                if (Lower > Titles && Lower > Upper && Lower > Normal)
                    return Case.Normal;

                return Case.Normal;
            } catch {
                return Case.Normal;
            }
        }

        public void AfterOpen(ref string Line, uint ID) {
            if (!CaseMap.ContainsKey(ID))
                return;
            CaseMap[ID] = GetLineCase(Line);
        }

        public void BeforeSave(ref string Line, uint ID) {
            if (!CaseMap.ContainsKey(ID))
                return;
            if (GetLineCase(Line) != CaseMap[ID])
                Line = SetCase(Line, CaseMap[ID]);
        }
        public string GetName() {
            return "Case Fixer";
        }
    }
}
