using System.Collections.Generic;
using System.Linq;
using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    class JapFixer : IOptimizator {
        Dictionary<uint, string> Minified = new Dictionary<uint, string>();
        public void AfterTranslate(ref string Line, uint ID) {
            string MinifiedTL = Minify(Line);

            if (MinifiedTL == Minified[ID] && !string.IsNullOrEmpty(MinifiedTL)) {
                var TempClient = AlternativeClient(Program.TLClient);
                string NewResult = Line.Translate(Program.Settings.SourceLang, Program.Settings.TargetLang, TempClient);
                if (MinifiedTL != Minify(NewResult) && NewResult.IsDialogue())
                    Line = NewResult;
            }
        }

        public void BeforeTranslate(ref string Line, uint ID) {
            Minified[ID] = Minify(Line);
        }
        private Translator AlternativeClient(Translator Client) {
            switch (Client) {
                case Translator.BingNeural:
                case Translator.Bing:
                    return Translator.Google;
                case Translator.Google:
                    if (Program.LECPort == null)
                        return Translator.Google;
                    else
                        return Translator.LEC;
                case Translator.CacheOnly:
                    return Translator.CacheOnly;

                default:
                    return Translator.Google;
            }
        }

        public static string Minify(string String) {
            string Minified = string.Empty;
            foreach (char c in String)
                if (char.IsLetter(c) && Minified.LastOrDefault() != c)
                    Minified += c;

            return Minified.ToLower();
        }

        public void AfterOpen(ref string Line, uint ID) {
        }

        public void BeforeSave(ref string Line, uint ID) {
        }
        public string GetName() {
            return "Japanese Fixer";
        }
    }
}
