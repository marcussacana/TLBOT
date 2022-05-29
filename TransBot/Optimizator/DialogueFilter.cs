namespace TLBOT.Optimizator {
    /// <summary>
    /// Fake plugin that enable the internal string filter
    /// This is a fake plugin to force the higest priority over all other plugins
    /// and allows the user enable/disable easy.
    /// </summary>
    class DialogueFilter : IOptimizator {
        public void AfterTranslate(ref string Line, uint ID) { }
        public void BeforeTranslate(ref string Line, uint ID) { }
        public void AfterOpen(ref string Line, uint ID) { }
        public void BeforeSave(ref string Line, uint ID) { }
        public void PreProcess(ref string[] Lines) { }
        public void PostProcess(ref string[] Lines) { }
        public string GetName() {
            return "Dialogue Filter";
        }
    }
}
