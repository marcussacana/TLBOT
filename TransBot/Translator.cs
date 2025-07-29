#define NODEBUG
using System;
using System.Threading.Tasks;
using TLBOT.DataManager;
using TLBOT.Optimizator;

namespace TLBOT
{
    public class TranslationTask : ITask
    {
        string SourceLanguage;
        string TargetLanguage;
        public string[] Lines { get; set; }

        public enum Status
        {
            IDLE, PreProcessing, Translating, PostProcessing, Finished
        }

        public Status TaskStatus = Status.IDLE;
        public string CurrentStatus => TaskStatus.ToString();
        public uint Progress { private set; get; }

        public bool Finished => TaskStatus == Status.Finished;

        IOptimizator[] Optimizators;
        public TranslationTask(string SourceLanguage, string TargetLanguage, IOptimizator[] Optimizators)
        {
            this.SourceLanguage = SourceLanguage;
            this.TargetLanguage = TargetLanguage;
            this.Optimizators = Optimizators;
        }

        public void UpdateLines(string[] Lines)
        {
            this.Lines = Lines;
        }

        public Task Build(Action OnFinish = null)
        {
            return new Task(() =>
            {
                TaskStatus = Status.PreProcessing;

                if (Program.Settings.Multithread)
                {
                    Parallel.For(0, Lines.LongLength, new Action<long>((a) =>
                    {
                        uint i = (uint)a;
                        foreach (IOptimizator Optimizator in Optimizators)
                        {
#if !DEBUG || NODEBUG
                            try
                            {
#endif
                                Optimizator.BeforeTranslate(ref Lines[i], i);
#if !DEBUG || NODEBUG
                            }
                            catch { }
#endif
                        }
                        Progress = i;
                    }));
                }
                else
                {
                    for (uint i = 0; i < Lines.LongLength; i++)
                    {
                        foreach (IOptimizator Optimizator in Optimizators)
                        {
#if !DEBUG || NODEBUG
                            try
                            {
#endif
                                Optimizator.BeforeTranslate(ref Lines[i], i);
#if !DEBUG || NODEBUG
                            }
                            catch { }
#endif
                        }
                        Progress = i;

                    }
                }

                bool canRetry = true;

                TaskStatus = Status.Translating;
                switch (Program.TLMode)
                {
                    case TransMode.Massive:
                        Lines = Lines.TranslateMassive(SourceLanguage, TargetLanguage, Program.TLClient);
                        break;
                    case TransMode.Multithread:
                        Lines = Lines.TranslateMultithread(SourceLanguage, TargetLanguage, Program.TLClient, x => Progress = x);
                        break;

                    case TransMode.Normal:
                        for (uint i = 0; i < Lines.Length; i++)
                        {
                            var newLine = Lines[i].Translate(SourceLanguage, TargetLanguage, canRetry ? Program.TLClient : Translator.OllamaAlt);

                            if (Program.TLClient == Translator.Ollama & !Attestation(newLine))
                            {
                                if (canRetry)
                                {
                                    canRetry = false;
                                    if (Program.Cache.ContainsKey(Lines[i]))
                                        Program.Cache.Remove(Lines[i]);

                                    i--;
                                    continue;
                                } 
                                else
                                {
                                    if (Program.Cache.ContainsKey(Lines[i]))
                                        Program.Cache.Remove(Lines[i]);

                                    newLine = Lines[i].Translate("AUTO", TargetLanguage, Translator.Google);
                                }
                            }

                            Lines[i] = newLine;                            
                            canRetry = true;
                            Progress = i;
                        }
                        break;
                }

                TaskStatus = Status.PostProcessing;
                Progress = 0;

                if (Program.Settings.Multithread)
                {
                    Parallel.For(0, Lines.LongLength, new Action<long>((a) =>
                    {
                        Progress++;
                        uint i = (uint)a;
                        foreach (IOptimizator Optimizator in Optimizators)
#if !DEBUG || NODEBUG
                            try
                            {
#endif
                                Optimizator.AfterTranslate(ref Lines[i], i);
#if !DEBUG || NODEBUG
                            }
                            catch { }
#endif
                        Progress = i;
                    }));
                }
                else
                {
                    for (uint i = 0; i < Lines.LongLength; i++)
                    {
                        foreach (IOptimizator Optimizator in Optimizators)
#if !DEBUG || NODEBUG
                            try
                            {
#endif
                                Optimizator.AfterTranslate(ref Lines[i], i);
#if !DEBUG || NODEBUG
                            }
                            catch { }
#endif
                        Progress = i;
                    }
                }

                TaskStatus = Status.Finished;
                OnFinish?.Invoke();
            });
        }

        private bool Attestation(string Result)
        {
            if (Result.KanjiCount() > 0)
            {
                var TargLang = TargetLanguage.ToLowerInvariant().Trim();
                if (!TargLang.StartsWith("jap") && !TargLang.StartsWith("ch"))
                {
                    return false;
                }
            }
            return true;
        }

    }
}