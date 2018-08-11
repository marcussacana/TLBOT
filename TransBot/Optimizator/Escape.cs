using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    class Escape : IOptimizator {
        public void AfterOpen(ref string Line, uint ID) {
            Line = Line.Unescape();
        }

        public void AfterTranslate(ref string Line, uint ID) {

        }

        public void BeforeSave(ref string Line, uint ID) {
            Line = Line.Escape();
        }

        public void BeforeTranslate(ref string Line, uint ID) {
        }

        public string GetName() {
            return "String Escape";
        }
    }
}
