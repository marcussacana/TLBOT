﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TLBOT.Optimizator {
    class CaseFixer : IOptimizator {
        Dictionary<uint, Case> CaseMap = new Dictionary<uint, Case>();
        public void AfterTranslate(ref string Line, uint ID) {
            if (GetLineCase(Line) != CaseMap[ID])
                Line = SetCase(Line, CaseMap[ID]);
        }

        public void BeforeTranslate(ref string Line, uint ID) {
            CaseMap[ID] = GetLineCase(Line);
        }

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
                    string[] nWords = String.Split(' ');
                    for (int x = 0; x < nWords.Length; x++) {
                        for (int i = 0; i < nWords[x].Length; i++) {
                            bool Upper = i == 0;
                            if (!Upper && char.IsPunctuation(nWords[i - 1].Last())) {
                                Upper = true;
                            }
                            
                            char c = nWords[x][i];
                            nResult += Upper ? char.ToUpper(c) : c;
                        }
                        nResult += ' ';
                    }
                    nResult = nResult.Substring(0, nResult.Length - 1);
                    return nResult;
                case Case.Title:
                    string tResult = string.Empty;
                    string[] tWords = String.Split(' ');
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
            string[] Words = String.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            Case[] WordsCase = new Case[Words.Length];
            for (int x = 0; x < Words.Length; x++) {
                string Word = Words[x];
                for (int i = 0; i < Word.Length; i++) {
                    char Char = Word[i];
                    if (Char > 0x8000)
                        return Case.Normal;

                    if (i == 0) {
                        if (char.IsLetter(Char) && char.IsUpper(Char)) {
                            WordsCase[x] = (x == 0 || (char.IsPunctuation(Words[x - 1].Last()))) ? Case.Normal : Case.Title;
                        } else {
                            WordsCase[x] = Case.Lower;
                        }
                    } else {
                        if (char.IsUpper(Char))
                            WordsCase[x] = Case.Upper;
                        break;
                    }
                }
            }

            int Titles = (from x in WordsCase where x == Case.Title  select x).Count();
            int Upper  = (from x in WordsCase where x == Case.Upper  select x).Count();
            int Normal = (from x in WordsCase where x == Case.Normal select x).Count();
            int Lower  = (from x in WordsCase where x == Case.Lower  select x).Count();

            if (Titles > Normal && Titles > Upper && Titles > Lower)
                return Case.Title;
            if (Upper > Titles && Upper > Normal && Upper > Lower)
                return Case.Upper;
            if (Lower > Titles && Lower > Upper && Lower > Normal)
                return Case.Normal;

            return Case.Normal;
        }

        public void AfterOpen(ref string Line, uint ID) {
        }

        public void BeforeSave(ref string Line, uint ID) {
        }
        public string GetName() {
            return "Case Fixer";
        }
    }
}
