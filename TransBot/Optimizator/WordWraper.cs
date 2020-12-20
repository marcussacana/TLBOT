using System;
using System.Collections.Generic;
using System.Linq;
using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    class WordWraper : IOptimizator {
        bool Dynamic => Program.WordwrapSettings.MaxWidth <= 0;
        int MaxWidth => Math.Abs(Program.WordwrapSettings.MaxWidth);
        Dictionary<uint, int> WidthMap = new Dictionary<uint, int>(); 
        public void AfterTranslate(ref string Line, uint ID) {
            Line = Line.WordWrap(Dynamic ? (int?)WidthMap[ID] : null);
        }

        public void BeforeTranslate(ref string Line, uint ID) {
            if (Dynamic)
                WidthMap[ID] = GetMaxSize(Line);

            Line = Line.MergeLines();
        }

        public void AfterOpen(ref string Line, uint ID) {
        }

        public void BeforeSave(ref string Line, uint ID) {
        }

        public int GetMaxSize(string String) {
            string[] Lines = String.Replace(Program.WordwrapSettings.LineBreaker, "\n").Split('\n');
            if (Lines.Length == 1 && MaxWidth != 0)
                return MaxWidth;

            if (Program.WordwrapSettings.Monospaced) {
                int[] Sizes = (from x in Lines orderby x.Length select x.Length).ToArray();
                return Sizes.Last();
            } else {
                int[] Widths = (from x in Lines select Extensions.GetTextWidth(Program.WordwrapFont, x)).ToArray();
                return (from x in Widths orderby x select x).Last();
            }
        }
        public string GetName() {
            return "WordWraper";
        }
    }
}
