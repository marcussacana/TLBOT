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
        CacheOnly
    }

    public enum TransMode {
        Massive,
        Multithread,
        Normal
    }
}
