using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    class WordWraper : IOptimizator {
        public void AfterTranslate(ref string Line, uint ID) {
            Line = Line.WordWrap();
        }

        public void BeforeTranslate(ref string Line, uint ID) {
            Line = Line.MergeLines();
        }

        public void AfterOpen(ref string Line, uint ID) {
        }

        public void BeforeSave(ref string Line, uint ID) {
        }
        public string GetName() {
            return "WordWraper";
        }
    }
}
