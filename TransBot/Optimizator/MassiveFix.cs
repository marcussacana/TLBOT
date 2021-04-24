using System.Collections.Generic;
using System.Linq;
using TLBOT.DataManager;

namespace TLBOT.Optimizator
{
    class MassiveFix : IOptimizator
    {
        static Dictionary<uint, string> StrMap = new Dictionary<uint, string>();
        public void AfterOpen(ref string Line, uint ID) { }

        public void AfterTranslate(ref string Line, uint ID)
        {
            var OriLine = StrMap[ID];
            if (Line.Length >= OriLine.Length - (OriLine.Length / 3))
                return;

            Line = new string[] { OriLine }.TranslateMassive(Program.Settings.SourceLang, Program.Settings.TargetLang, Program.TLClient).First();
        }

        public void BeforeSave(ref string Line, uint ID)
        {
            StrMap[ID] = Line;
        }

        public void BeforeTranslate(ref string Line, uint ID) { }

        public string GetName()
        {
            return "Massive Fixer";
        }
    }
}
