using AdvancedBinary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TLBOT.DataManager;
using TLBOT.Optimizator;

namespace TLBOT {
    static class Program {

        internal static Font WordwrapFont => new Font(WordwrapSettings.FontName, WordwrapSettings.FontSize, WordwrapSettings.Bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
        internal static string TaskPath => AppDomain.CurrentDomain.BaseDirectory + "Task.tbt";
        internal static string INIPath => AppDomain.CurrentDomain.BaseDirectory + "TLBOT.ini";
        private static string CachePath => AppDomain.CurrentDomain.BaseDirectory + string.Format("TLBOT-{0}.tbc", Settings.TargetLang);

        public static bool CacheReady = false;

        public static Translator TLClient { get {
                switch (Settings.TLClient.ToLower()) {
                    default:
                        return Translator.Google;

                    case "bing":
                        return Translator.Bing;

                    case "bing neural":
                    case "bingneural":
                        return Translator.BingNeural;

                    case "powertranslator":
                    case "lec":
                        return Translator.LEC;

                    case "cacheonly":
                    case "cache":
                    case "none":
                        return Translator.CacheOnly;
                }
            }
        }

        public static TransMode TLMode {
            get {
                switch (Settings.TransMode.ToLower()) {
                    default:
                        return TransMode.Massive;

                    case "multithread":
                    case "multithreaded":
                    case "multi thread":
                    case "multi threaded":
                        return TransMode.Multithread;

                    case "normal":
                        return TransMode.Normal;
                }
            }
        }

        public static bool FromAsian { get {
                string Lang = Settings.SourceLang.ToUpper().Trim().Replace("-", "");
                switch (Lang) {
                    case "JA":
                    case "JP":
                    case "ZHCN":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public static bool ProxyInitialized = false;

        public static Dictionary<string, bool> ForceDialogues = new Dictionary<string, bool>();
        public static Dictionary<string, string> Cache = new Dictionary<string, string>();

        public static Settings Settings;
        public static FilterSettings FilterSettings;
        public static WordwrapSettings WordwrapSettings;
        public static OptimizatorSelector OptimizatorSettings;
        public static TaskInfo TaskInfo = new TaskInfo();

        public static IOptimizator[] ExternalPlugins = new IOptimizator[0];

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            LoadSettings();
            LoadCache(new Action(() => { CacheReady = true; }));
            ExternalPlugins = SearchOptimizators("*-TBPlugin.cs");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }


        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            var Exception = e.ExceptionObject as Exception;
            if (e.IsTerminating)
                MessageBox.Show("A unhandled Excpetion has occured\n" + Exception.Message + "\nCall Stack:\n\n" + Exception.StackTrace, "TLBOT 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void LoadSettings() {
            try {
                AdvancedIni.FastOpen(out Settings, INIPath);
                AdvancedIni.FastOpen(out OptimizatorSettings, INIPath);
                AdvancedIni.FastOpen(out FilterSettings, INIPath);
                AdvancedIni.FastOpen(out WordwrapSettings, INIPath);

            } catch { }            
        }

        public static void LoadCache(Action OnFinish) {
            new Thread(() => {
                try {
                    TLBCache Cache = new TLBCache();
                    using (StructReader Reader = new StructReader(CachePath)) {
                        Reader.ReadStruct(ref Cache);
                        Reader.Close();
                    }

                    Program.Cache = new Dictionary<string, string>();
                    for (uint i = 0; i < Cache.Entries; i++)
                        Program.Cache[Cache.Original[i]] = Cache.Translations[i];

                    ForceDialogues = new Dictionary<string, bool>();
                    for (uint i = 0; i < Cache.SEntries; i++)
                        ForceDialogues[Cache.ManualString[i]] = Cache.ManualChecked[i];
                } catch { }

                try {
                    if (File.Exists(TaskPath)) {
                        TaskInfo = new TaskInfo();
                        using (StructReader Reader = new StructReader(TaskPath)) {
                            Reader.ReadStruct(ref TaskInfo);
                            Reader.Close();
                        }
                    }
                } catch { }

                OnFinish?.Invoke();
            }).Start();
        }
        public static void SaveSettings() {
            try {
                AdvancedIni.FastSave(Settings, INIPath);
                AdvancedIni.FastSave(OptimizatorSettings, INIPath);
                AdvancedIni.FastSave(FilterSettings, INIPath);
                AdvancedIni.FastSave(WordwrapSettings, INIPath);
            } catch { }
        }

        public static void SaveCache() {
            if (!CacheReady)
                return;
            try {
                TLBCache Cache = new TLBCache() {
                    Signature = "TLBC",
                    Original = Program.Cache.Keys.OfType<string>().ToArray(),
                    Translations = Program.Cache.Values.OfType<string>().ToArray(),
                    ManualString = ForceDialogues.Keys.OfType<string>().ToArray(),
                    ManualChecked = ForceDialogues.Values.OfType<bool>().ToArray()
                };

                if (File.Exists(CachePath))
                    File.Delete(CachePath);

                using (StructWriter Writer = new StructWriter(CachePath)) {
                    Writer.WriteStruct(ref Cache);
                    Writer.Close();
                }
            } catch { }

            SaveTask();
        }

        internal static void SaveTask() {
            try {
                if (File.Exists(TaskPath))
                    File.Delete(TaskPath);

                if (TaskInfo.LastTaskPos > 0) {
                    using (StructWriter Writer = new StructWriter(TaskPath)) {
                        Writer.WriteStruct(ref TaskInfo);
                        Writer.Close();
                    }
                }
            } catch { }

        }

        internal static IOptimizator[] SearchOptimizators(string Filter) {
            var Optimizators = new IOptimizator[0];
            foreach (string PluginPath in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, Filter)) {
                try {
                    string Content = File.ReadAllText(PluginPath, System.Text.Encoding.UTF8);
                    Content = Content.Replace("IPlugin", "IOptimizator");

                    int IO = Content.Substring(0, Content.IndexOf("class")).IndexOf("using ");
                    if (IO < 0)
                        IO = 0;

                    Content = Content.Substring(0, IO) +
                        "using TLBOT.Optimizator;\n" +
                        Content.Substring(IO);
                    try {
                        DotNetVM VM = new DotNetVM(Content, DotNetVM.Language.CSharp);
                        Type[] Classes = VM.Assembly.GetTypes().Where(x => typeof(IOptimizator).IsAssignableFrom(x)).ToArray();

                        foreach (Type Class in Classes) {
                            IOptimizator Plugin = (IOptimizator)Activator.CreateInstance(Class);
                            Optimizators = Optimizators.AppendArray(Plugin);
                        }
                    } catch (Exception ex){
                        MessageBox.Show(ex.ToString(), string.Format("TLBOT 2 - {0} Error", System.IO.Path.GetFileName(PluginPath)), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } catch { }
            }
            return Optimizators;
        }
    }

#pragma warning disable 649
    [FieldParmaters(Name = "TLBOT")]
    internal struct Settings {
        [FieldParmaters(DefaultValue = "", Name = "LastPath")]
        public string LastSelectedPath;
        [FieldParmaters(DefaultValue = "", Name = "Client")]
        public string TLClient;
        [FieldParmaters(DefaultValue = "", Name = "Mode")]
        public string TransMode;
        [FieldParmaters(DefaultValue = "", Name = "SourceLanguage")]
        public string SourceLang;
        [FieldParmaters(DefaultValue = "", Name = "TargetLanguage")]
        public string TargetLang;
        [FieldParmaters(DefaultValue = false, Name = "DoubleStep")]
        public bool DoubleStep;
        [FieldParmaters(DefaultValue = false, Name = "TranslateWindow")]
        public bool TranslateWindow;
        [FieldParmaters(DefaultValue = 1000, Name = "DBViewPageLimit")]
        public int DBViewPageLimit;
        [FieldParmaters(DefaultValue = false, Name = "Multithread")]
        public bool Multithread;
        [FieldParmaters(DefaultValue = false, Name = "LSTMode")]
        public bool LSTMode;
    }

    [FieldParmaters(Name = "Optimizator")]
    internal struct OptimizatorSelector {
        [FieldParmaters(DefaultValue = false, Name = "EnableWordWrap")]
        public bool EnableWordWrap;
        [FieldParmaters(DefaultValue = false, Name = "CaseFixer")]
        public bool CaseFixer;
        [FieldParmaters(DefaultValue = false, Name = "CustomReplaces")]
        public bool CustomReplaces;
        [FieldParmaters(DefaultValue = false, Name = "JapaneseFixer")]
        public bool JapaneseFixer;
        [FieldParmaters(DefaultValue = false, Name = "DialogueFilter")]
        public bool DialogueFilter;
        [FieldParmaters(DefaultValue = false, Name = "MassiveFixer")]
        public bool MassiveFixer;
    }
    
    internal struct TaskInfo {
        [PArray(PrefixType = Const.UINT32), CString()]
        public string[] LastTask;
        public uint LastTaskPos;
    }


    [FieldParmaters(Name = "Filter")]
    internal struct FilterSettings {
        [FieldParmaters(DefaultValue = "", Name = "DenyList")]
        public string DenyList;
        [FieldParmaters(DefaultValue = "", Name = "IgnoreList")]
        public string IgnoreList;
        [FieldParmaters(DefaultValue = "", Name = "QuoteList")]
        public string QuoteList;
        [FieldParmaters(DefaultValue = 2, Name = "Sensitivity")]
        public int Sensitivity;
        [FieldParmaters(DefaultValue = false, Name = "UseDB")]
        public bool UseDB;
        [FieldParmaters(DefaultValue = false, Name = "UsePos")]
        public bool UsePos;
        [FieldParmaters(DefaultValue = false, Name = "UsePosCaution")]
        public bool UsePosCaution;
    }

    [FieldParmaters(Name = "WordWrap")]
    internal struct WordwrapSettings {
        [FieldParmaters(DefaultValue = false, Name = "FakeBreakLine")]
        public bool FakeBreakLine;
        [FieldParmaters(DefaultValue = false, Name = "Monospaced")]
        public bool Monospaced;
        [FieldParmaters(DefaultValue = false, Name = "Bold")]
        public bool Bold;
        [FieldParmaters(DefaultValue = 0, Name = "MaxWidth")]
        public int MaxWidth;
        [FieldParmaters(DefaultValue = 0, Name = "MaxLines")]
        public int MaxLines;
        [FieldParmaters(DefaultValue = "Consolas", Name = "FaceName")]
        public string FontName;
        [FieldParmaters(DefaultValue = 12.0f, Name = "FontSize")]
        public float FontSize;
        [FieldParmaters(DefaultValue = "\\n", Name = "LineBreaker")]
        public string LineBreaker;
        [FieldParmaters(DefaultValue = false, Name = "DynamicWidthDiscardSetenceEnd")]
        public bool DynamicWidthDiscardSetenceEnd;
        [FieldParmaters(DefaultValue = false, Name = "DynamicWidthPerScript")]
        public bool DynamicWidthPerScript;
    }

    internal struct TLBCache {
        [FString(Length = 4)]
        public string Signature;

        public uint Entries;

        [RArray(FieldName = "Entries"), PString(PrefixType = Const.UINT16)]
        public string[] Original;
        [RArray(FieldName = "Entries"), PString(PrefixType = Const.UINT16)]
        public string[] Translations;

        public uint SEntries;
        [RArray(FieldName = "SEntries"), CString()]
        public string[] ManualString;
        [RArray(FieldName = "SEntries")]
        public bool[] ManualChecked;
    }

    internal struct Quote {
        public char? Start;
        public char? End;
    }
#pragma warning restore 649
}
