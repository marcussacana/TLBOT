using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLBOT {
    public enum Translator {
        Google,
        Bing,
        BingNeural,
        LEC,
        ChatGPT,
        Ollama,
        OllamaAlt,
        DeepL,
        CacheOnly
    }

    public enum TransMode {
        Massive,
        Multithread,
        Normal,
        ContextAware,
        StaticContext
    }
}
