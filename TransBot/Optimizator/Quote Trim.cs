using System.Collections.Generic;
using System.Linq;
using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    class QuoteTrim : IOptimizator {

        public void PreProcess(ref string[] Lines) { }

        public void PostProcess(ref string[] Lines) { }

        public static Quote[] Quotes = Program.FilterSettings.QuoteList.Unescape().Split('\n')
                    .Where(x => x.Length == 2)
                    .Select(x => {
                        return new Quote() { Start = x[0], End = x[1] };
                    }).ToArray();

        Dictionary<uint, Quote?> QuoteMap = new Dictionary<uint, Quote?>(); 

        public void AfterOpen(ref string Line, uint ID) {

        }

        public void AfterTranslate(ref string Line, uint ID) {
            if (!QuoteMap.ContainsKey(ID) || !QuoteMap[ID].HasValue)
                return;
            Line = QuoteMap[ID]?.Start + Line.Trim() + QuoteMap[ID]?.End;
        }

        public void BeforeSave(ref string Line, uint ID) {

        }

        public void BeforeTranslate(ref string Line, uint ID) {
            QuoteMap[ID] = null;
            Line = Line.Trim();
            foreach (Quote LQuote in Quotes) {
                var Quote = new Quote() {
                    Start = LQuote.Start,
                    End = LQuote.End
                };

                if (Line.StartsWith(Quote.Start.ToString()))
                {
                    Line = Line.Substring(1).Trim();

                    if (Line.EndsWith(Quote.End.ToString())) {
                        Line = Line.Substring(0, Line.Length - 1).Trim();
                    }
                    else Quote.End = null;

                    QuoteMap[ID] = Quote;
                    break;
                }
                else Quote.Start = null;

                if (Line.EndsWith(Quote.End.ToString()))
                {
                    Line = Line.Substring(0, Line.Length - 1).Trim();
                    QuoteMap[ID] = Quote;
                    break;
                }
            }
        }

        public string GetName() {
            return "Quote Trim";
        }
    }
}
