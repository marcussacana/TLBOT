#define NODEBUG
using System;
using System.Threading.Tasks;
using TLBOT.DataManager;
using TLBOT.Optimizator;

namespace TLBOT {
    public class TranslationTask {
        string SourceLanguage;
        string TargetLanguage;
        public string[] Lines;

        public enum Status {
            IDLE, PreProcessing, Translating, PostProcessing, Finished
        }

        public Status TaskStatus = Status.IDLE;
        public uint Progress { private set; get; }

        IOptimizator[] Optimizators;
        public TranslationTask(string[] Lines, string SourceLanguage, string TargetLanguage, IOptimizator[] Optimizators) {
            this.Lines = Lines;
            this.SourceLanguage = SourceLanguage;
            this.TargetLanguage = TargetLanguage;
            this.Optimizators = Optimizators;
        }

        public Task Build(Action OnFinish = null) {
            return new Task(() => {
                TaskStatus = Status.PreProcessing;

                if (Program.Settings.Multithread) {
                    Parallel.For(0, Lines.LongLength, new Action<long>((a) => {
                        uint i = (uint)a;
                        foreach (IOptimizator Optimizator in Optimizators) {
#if !DEBUG
                            try {
#endif
                                Optimizator.BeforeTranslate(ref Lines[i], i);
#if !DEBUG
                            } catch { }
#endif
                        }
                        Progress++;
                    }));
                } else {
                    for (uint i = 0; i < Lines.LongLength; i++) {
                        foreach (IOptimizator Optimizator in Optimizators) {
#if !DEBUG
                            try {
#endif
                                Optimizator.BeforeTranslate(ref Lines[i], i);
#if !DEBUG
                            } catch { }
#endif
                            Progress++;
                        }

                    }
                }

                TaskStatus = Status.Translating;
                switch (Program.TLMode) {
                    case TransMode.Massive:
                        Lines = Lines.TranslateMassive(SourceLanguage, TargetLanguage, Program.TLClient);
                        break;
                    case TransMode.Multithread:
                        Lines = Lines.TranslateMultithread(SourceLanguage, TargetLanguage, Program.TLClient, x => Progress = x);
                        break;

                    case TransMode.Normal:
                        for (uint i = 0; i < Lines.Length; i++) {
                            Lines[i] = Lines[i].Translate(SourceLanguage, TargetLanguage, Program.TLClient);
                            Progress = i;
                        }
                        break;
                }

                TaskStatus = Status.PostProcessing;
                Progress = 0;

                if (Program.Settings.Multithread) {
                    Parallel.For(0, Lines.LongLength, new Action<long>((a) => {
                        Progress++;
                        uint i = (uint)a;
                        foreach (IOptimizator Optimizator in Optimizators)
                            try {
#if DEBUG && !NODEBUG
                            var Begin = DateTime.Now;
                            string Line = Lines[i];
                            Optimizator.AfterTranslate(ref Line, i);
                            if (System.Diagnostics.Debugger.IsAttached) {
                                var ElapsedTime = (DateTime.Now - Begin).TotalMilliseconds;
                                if (ElapsedTime > 300 || Lines[i] != Line)
                                    System.Diagnostics.Debugger.Break();
                            }
                            Lines[i] = Line;
#else
                                Optimizator.AfterTranslate(ref Lines[i], i);
#endif
                            } catch { }

                    }));
                } else {
                    for (uint i = 0; i < Lines.LongLength; i++) {
                        foreach (IOptimizator Optimizator in Optimizators)
                            try {
#if DEBUG && !NODEBUG
                            var Begin = DateTime.Now;
                            string Line = Lines[i];
                            Optimizator.AfterTranslate(ref Line, i);
                            if (System.Diagnostics.Debugger.IsAttached) {
                                var ElapsedTime = (DateTime.Now - Begin).TotalMilliseconds;
                                if (ElapsedTime > 300 || Lines[i] != Line)
                                    System.Diagnostics.Debugger.Break();
                            }
                            Lines[i] = Line;
#else
                                Optimizator.AfterTranslate(ref Lines[i], i);
#endif
                            } catch { }
                        Progress++;
                    }
                }

                TaskStatus = Status.Finished;
                OnFinish?.Invoke();
            });
        }
                
    }
}
