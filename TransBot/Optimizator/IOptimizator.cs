namespace TLBOT.Optimizator {
    public interface IOptimizator {
        string GetName();
        void BeforeTranslate(ref string Line, uint ID);
        void AfterTranslate(ref string Line, uint ID);

        void AfterOpen(ref string Line, uint ID);

        void BeforeSave(ref string Line, uint ID);
    }
}
