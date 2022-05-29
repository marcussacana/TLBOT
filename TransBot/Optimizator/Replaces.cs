using System;
using System.IO;
using TLBOT.DataManager;

namespace TLBOT.Optimizator {
    class Replaces : IOptimizator {
        string ListPath = AppDomain.CurrentDomain.BaseDirectory + "Replace.lst";
        string[] Source = null;
        string[] Target = null;

        public void PreProcess(ref string[] Lines) { }

        public void PostProcess(ref string[] Lines) { }

        public void AfterTranslate(ref string Line, uint ID) {
            if (string.IsNullOrWhiteSpace(Line))
                return;

            if (Source == null) {
                Source = new string[0];
                Target = new string[0];
                try {
                    if (File.Exists(ListPath)) {
                        using (StreamReader Reader = new StreamReader(File.OpenRead(ListPath))) {
                            while (Reader.Peek() >= 0) {
                                string L1 = Reader.ReadLine();
                                string L2 = Reader.ReadLine();
                                if (string.IsNullOrEmpty(L1))
                                    continue;
                                Source = Source.AppendArray(L1.Unescape());
                                Target = Target.AppendArray(L2.Unescape());
                            }
                            Reader.Close();
                        }
                    }
                } catch { }
            }

            for (int i = 0; i < Target.Length; i++) {
                Line = Line.Replace(Source[i], Target[i]);
            }
        }

        public void BeforeTranslate(ref string Line, uint ID) {

        }

        public void AfterOpen(ref string Line, uint ID) {
        }

        public void BeforeSave(ref string Line, uint ID) {
        }
        public string GetName() {
            return "Custom Replaces";
        }
    }
}
