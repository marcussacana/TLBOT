using System.Collections.Generic;
using System.Linq;
using TLBOT.DataManager;

namespace TLBOT.Optimizator
{
    class MassiveFix : IOptimizator
    {
        static Dictionary<string, string> Cache = new Dictionary<string, string>();
        static Dictionary<uint, string> StrMap = new Dictionary<uint, string>();
        public void AfterOpen(ref string Line, uint ID) { }

        public void AfterTranslate(ref string Line, uint ID)
        {
            var OriLine = StrMap[ID];
            if (CountWords(Line) >= CountWords(OriLine) - (CountWords(OriLine) / 3))
                return;

            if (Cache.ContainsKey(OriLine))
            {
                Line = Cache[OriLine];
                return;
            }

            var Result = new string[] { OriLine }.TranslateMassive(Program.Settings.SourceLang, Program.Settings.TargetLang, Program.TLClient).First();
            Cache[OriLine] = Result;
        }

        private int CountWords(string Line) => Line.Trim().Split(' ').Length;

        public void BeforeSave(ref string Line, uint ID) { }

        public void BeforeTranslate(ref string Line, uint ID) { StrMap[ID] = Line; }

        public string GetName()
        {
            return "Massive Fixer";
        }
    }
}
