namespace TLBOT.Optimizator {
    public interface IOptimizator {
        string GetName();

        void PreProcess(ref string[] Lines);

        void BeforeTranslate(ref string Line, uint ID);
        void AfterTranslate(ref string Line, uint ID);

        void AfterOpen(ref string Line, uint ID);

        void BeforeSave(ref string Line, uint ID);

        void PostProcess(ref string[] Lines);
    }
}
