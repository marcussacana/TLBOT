using System.Collections.Generic;
using System.Linq;
using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    class QuoteTrim : IOptimizator {

        static Quote[] Quotes = Program.FilterSettings.QuoteList.Unescape().Split('\n')
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
            Line = QuoteMap[ID]?.Start + Line + QuoteMap[ID]?.End;
        }

        public void BeforeSave(ref string Line, uint ID) {

        }

        public void BeforeTranslate(ref string Line, uint ID) {
            QuoteMap[ID] = null;
            foreach (Quote Quote in Quotes) {
                if (Line.StartsWith(Quote.Start.ToString()) && Line.EndsWith(Quote.End.ToString())) {
                    Line = Line.Substring(1, Line.Length - 2);
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
