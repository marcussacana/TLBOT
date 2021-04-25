using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    class WordWraper : IOptimizator {

        bool NewScript = true;

        bool DynamicLines => Program.WordwrapSettings.MaxLines <= 0;
        bool Dynamic => Program.WordwrapSettings.MaxWidth <= 0;
        int MaxWidth => Math.Abs(Program.WordwrapSettings.MaxWidth);
        int MaxLines => Math.Abs(Program.WordwrapSettings.MaxLines);

        int DynamicMaxLines => LineMap.Values.OrderByDescending(x => x).First();

        int? DynamicMaxWidth => Program.WordwrapSettings.DynamicWidthPerScript ? (int?)WidthMap.Values.OrderByDescending(x => x).First() : null;

        static Dictionary<uint, int> WidthMap = new Dictionary<uint, int>();
        static Dictionary<uint, int> LineMap = new Dictionary<uint, int>();
        public void AfterTranslate(ref string Line, uint ID) {
            var NewLine = Line.WordWrap(Dynamic ? (int?)WidthMap[ID] : null);

            if (MaxLines > 0 && DynamicLines && NewLine.SplitLines().Length > DynamicMaxLines)
                NewLine = WordWrapMaxLines(Line, ID, DynamicMaxLines);

            if (MaxLines > 0 && !DynamicLines && NewLine.SplitLines().Length > MaxLines)
                NewLine = WordWrapMaxLines(Line, ID, MaxLines);

            Line = NewLine;
        }

        string WordWrapMaxLines(string Line, uint ID, int MaxLines)
        {
            int DefMaxWidth = this.MaxWidth;
            
            int MaxWidth;
            if (Dynamic)
                MaxWidth = DynamicMaxWidth ?? WidthMap[ID];
            else
                MaxWidth = DefMaxWidth;

            int OverflowMaxWidth = (int)((double)(25 * DefMaxWidth) / 100) + DefMaxWidth;

            string NewLine;
            do
            {
                NewLine = Line.WordWrap(MaxWidth++);
            } while (NewLine.SplitLines().Length > MaxLines && MaxWidth < OverflowMaxWidth);

            return NewLine;
        }

        public void BeforeTranslate(ref string Line, uint ID)
        {
            if (NewScript || ID == 0)
            {
                NewScript = false;
                WidthMap = new Dictionary<uint, int>();
                LineMap = new Dictionary<uint, int>();
            }

            if (Dynamic)
                WidthMap[ID] = GetMaxSize(Line);

            if (DynamicLines)
                LineMap[ID] = Line.SplitLines().Length;

            Line = Line.MergeLines();
        }

        public void AfterOpen(ref string Line, uint ID) {
        }

        public void BeforeSave(ref string Line, uint ID) {
        }

        public int GetMaxSize(string String) {
            string[] Lines = String.Replace(Program.WordwrapSettings.LineBreaker, "\n").Split('\n');
            
            if (Program.WordwrapSettings.DynamicWidthDiscardSetenceEnd)
                Lines = Lines.Where(x => !string.IsNullOrEmpty(x.TrimEnd()) && 
                                         !new char[] { '.', ',', '｡', '，', '．', '!', '?', ':' }
                                         .Contains(x.TrimEnd().Last())).ToArray();
            
            if (Lines.Length <= 1 && MaxWidth != 0)
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
