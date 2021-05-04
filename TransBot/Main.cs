//#define DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SacanaWrapper;
using SocketClient;
using TLBOT.DataManager;
using TLBOT.Optimizator;

namespace TLBOT {
    public partial class Main : Form {
        private string AtualLang = "PT";
        private bool ShowingStrings = false;

        public Main() {
            InitializeComponent();

            bntTestClient.Visible = System.Diagnostics.Debugger.IsAttached;

            TranslateWindow();

            TLCLientMenu.Text = Program.Settings.TLClient;
            TransModeMenu.Text = Program.Settings.TransMode;

            SourceLangSelector.Text = Program.Settings.SourceLang;
            TargetLangSelector.Text = Program.Settings.TargetLang;

            TargetStepMode.Text = Program.Settings.DoubleStep ? "Double Step" : "Single Step";
            LineBreaker.Text = Program.WordwrapSettings.LineBreaker;
            ckFakeBreakLine.Checked = Program.WordwrapSettings.FakeBreakLine;
            LineLimit.Value = Program.WordwrapSettings.MaxWidth;
            MaxLines.Value = Program.WordwrapSettings.MaxLines;
            FaceName.Text = Program.WordwrapSettings.FontName;
            ckBold.Checked = Program.WordwrapSettings.Bold;

            SensetiveBar.Value = Program.FilterSettings.Sensitivity;

            ckUseDB.Checked = Program.FilterSettings.UseDB;
            ckLstMode.Checked = Program.Settings.LSTMode;
            ckTransTLBot.Checked = Program.Settings.TranslateWindow;

            if (Program.FilterSettings.UsePos)
                ckUsePos.Checked = true;
            else if (Program.FilterSettings.UsePosCaution)
                ckUsePos.CheckState = CheckState.Indeterminate;
            else
                ckUsePos.Checked = false;


            string Float = Program.WordwrapSettings.FontSize.ToString().Replace(".", ",");
            if (Float.Length == 1)
                Float = '0' + Float;
            if (!Float.Contains(","))
                Float += '0';
            FontSize.Text = Float.Replace(",", "");
            
            ShowOptimizators();
            InitializeToolTips();
            InitializeService();
        }

        private void InitializeService()
        {
            API.ConnectionFailed += (sender, args) => {
                MessageBox.Show("Failed to Connect to the Translation API,\nMaybe the server is offline?", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            };
        }

        IOptimizator[] EnabledOptimizators {
            get {
                var Optimizators = new IOptimizator[0];
                if (Program.OptimizatorSettings.JapaneseFixer) {
                    Optimizators = Optimizators.AppendArray(new JapFixer());
                }
                if (Program.OptimizatorSettings.CaseFixer) {
                    Optimizators = Optimizators.AppendArray(new CaseFixer());
                }
                if (Program.OptimizatorSettings.CustomReplaces) {
                   Optimizators = Optimizators.AppendArray(new Replaces());
                }
                if (Program.OptimizatorSettings.DialogueFilter) {
                    Optimizators = Optimizators.AppendArray(new DialogueFilter());
                }
                if (Program.OptimizatorSettings.MassiveFixer) {
                    Optimizators = Optimizators.AppendArray(new MassiveFix());
                }

                if (Program.OptimizatorSettings.EnableWordWrap) {
                    Optimizators = Optimizators.AppendArray(new WordWraper());
                }

                foreach (IOptimizator Optimizator in AllOptimizators) {
                    if ((from x in Optimizators where x.GetName() == Optimizator.GetName() select x).Count() != 0)
                        continue;

                    string Name = CaseFixer.SetCase(Optimizator.GetName(), CaseFixer.Case.Title).Trim().Replace(" ", "");
                    bool Enabled = Ini.GetConfig("Optimizator", Name, Program.INIPath, false).ToLower() == "true";
                    if (Enabled)
                        Optimizators = Optimizators.AppendArray(Optimizator);
                }

                return Optimizators;
            }
        }

        IOptimizator[] AllOptimizators {
            get {
                return new IOptimizator[] {
                    new CaseFixer(),
                    new Replaces(),
                    new WordWraper(),
                    new JapFixer(),
                    new DialogueFilter(),
                    new Escape(),
                    new StutterFixer(),
                    new QuoteTrim(),
                    new MassiveFix()
                }.Concat(Program.ExternalPlugins).ToArray();
            }
        }
        
        private void TranslateWindow() {
            if (Program.Settings.TranslateWindow) {
                if (AtualLang == Program.Settings.TargetLang)
                    return;
                AtualLang = Program.Settings.TargetLang;
                new Thread(() => this.Translate(Program.Settings.TargetLang, Program.TLClient)).Start();
            }
        }
        private void ShowOptimizators() {
            OptimizatorList.Items.Clear();
            foreach (IOptimizator Optimizator in AllOptimizators) {
                OptimizatorList.Items.Add(Optimizator.GetName(), IsEnabled(Optimizator));
            }
        }

        private bool IsEnabled(IOptimizator Optimizator) => (from x in EnabledOptimizators where x.GetName() == Optimizator.GetName() select x).Count() > 0;

        struct ToolTipInfo {
            public string Text;
            public Control Control;
        }
        private void InitializeToolTips() {
            ToolTipInfo[] ToolTips = new ToolTipInfo[] {
                new ToolTipInfo(){
                    Control = ckTransTLBot,
                    Text = "Traduz Automaticamente a Interface do TLBOT para a lingua alvo selecionada."
                },
                new ToolTipInfo() {
                    Control = LineLimit,
                    Text = "Define o espaço limite de uma única linha.\nQuando modo monospaced estiver ligado informe aqui o numero máximo de letras de uma linha.\nCaso o modo monospaced esteja desligado, informe o numero de pixels de uma linha.\n\nPS: Caso o valor dado seja zero, ele irá assumir um limite dinamico tendo como base o comprimento de cada linha de forma individual,\nisto é, o comprimento máximo de uma linha original dita o limite da linha de saída,\ne caso seja menor que zero ele tornará esse valor padrão para diálogos que não tem nenhuma quebra de linha."
                },
                new ToolTipInfo() {
                    Control = MaxLines,
                    Text = "Define o numero de linhas limite a ser almejado.\nSe esse limite for estrapolado o limite por linha, o limite irá ser expandido na medida do possível.\nQuando o valor for zero, o limite de linhas será desconsiderado, quando o limite for menor que zero, ele se tornará um limite padrão,\nque se refere ao máximo de linhas utilizadas no script atual como um todo."
                },
                new ToolTipInfo() {
                    Control = SensetiveBar,
                    Text = "Define a sensibilidade do filtro de comandos.\nQuanto maior mais texto será traduzido.\nQuanto menor menos texto será traduzido.\nMas como nada é de graça nessa vida, com maiores valores aumenta-se as chances se traduzir uma linha indevida\n(comando, nome de arquivos e etc) podendo então causar erros no script.\nPor outro lado reduzindo este valor fará o filtro tomar mais precaução contra comandos evitando possíveis erros no script gerado."
                },
                new ToolTipInfo(){
                    Control = TargetStepMode,
                    Text = "Single Step: Traduz a lingua de origem diretamente para lingua alvo.\nDouble Step: Traduz a lingua de origem para inglês, e do inglês a traduz para lingua alvo."
                },
                new ToolTipInfo() {
                    Control = TransModeMenu,
                    Text = "Massive: Tradução massiva com buffer de 300 linhas por pedido de tradução (Recomendado para o Google)\nMultithreaded: Efetua tradução criando 20 threads simultâneamente e realizando 20 pedidos diferentes. (Recomendado para LEC ou Bing)\nNormal: Traduz uma unica linha por vez (Mais lento porem mais estável)"
                },
                new ToolTipInfo(){
                    Control = OptimizeDbBnt,
                    Text = "Remove do bando de dados quaisquer entradas na qual o texto de origem é igual a sua referente tradução ou então se sua tradução não for considerada um diálogo."
                },
                new ToolTipInfo(){
                    Control = ckFakeBreakLine,
                    Text = "Caso a engine não tenha nenhum comando de quebra de linha,\nuse essa opção para \"simular\" um comando de quebra de linha\npreenchendo o espaço restante da linha com um espaço em branco.\n(Exige uma configuração completamente precisa dos limites da linha.)"
                },
                new ToolTipInfo() {
                    Control = ckUseDB,
                    Text = "Quando Ativo qualquer linha presente no banco de traduções será considerada como um diálogo."
                }
            };

            foreach (ToolTipInfo Info in ToolTips) {
                string Text = Info.Text;
                if (Program.Settings.TranslateWindow)
                    Text = Info.Text.Translate("PT", Program.Settings.TargetLang, Program.TLClient);

                ToolTip TP = new ToolTip() {
                    ToolTipIcon = ToolTipIcon.Info,
                    ToolTipTitle = "TLBOT 2 -  Info",
                    AutoPopDelay = (Text.Length * 35) + 3000
                };

                TP.SetToolTip(Info.Control, Text);
            }
        }

        private void NewTask_Click(object sender, EventArgs e) {
            #region ResumeLastTask
            if (Program.TaskInfo.LastTaskPos > 0) {
                string[] Existents = new string[0];
                bool AllExists = true;
                for (uint i = Program.TaskInfo.LastTaskPos; i < Program.TaskInfo.LastTask.LongLength; i++) {
                    if (!File.Exists(Program.TaskInfo.LastTask[i])) {
                        AllExists = false;
                    } else
                        Existents = Existents.AppendArray(Program.TaskInfo.LastTask[i]);
                }

                if (DialogResult.Yes == MessageBox.Show("You want Continue the last stoped task?", "TLBOT 2", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    if (!AllExists) {
                        string Message = $"Some Files of the last task are not found ({Existents.Length} Founds of {Program.TaskInfo.LastTask.LongLength - Program.TaskInfo.LastTaskPos} Files)\nContinue?";
                        if (DialogResult.Yes != MessageBox.Show(Message, "TLBOT 2", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                            return;

                        Program.TaskInfo = new TaskInfo() {
                            LastTask = Existents,
                            LastTaskPos = 0
                        };

                    }
                    ProcessFiles(Program.TaskInfo.LastTask, Program.TaskInfo.LastTaskPos);
                    return;
                }
            }
            #endregion

            FilesSelector TaskCreator = new FilesSelector();
            if (TaskCreator.ShowDialog() != DialogResult.OK)
                return;

            ProcessFiles(TaskCreator.SelectedFiles);
        }

        private void ProcessFiles(string[] Files, uint Begin = 0) {
            Program.TaskInfo = new TaskInfo() {
                LastTask = Files,
                LastTaskPos = Begin
            };

            DialogResult? dr = null;
            long TotalCount = 0;
            Wrapper Wrapper = new Wrapper();
            for (uint x = Begin; x < Files.LongLength; x++) {
                Program.TaskInfo.LastTaskPos = x;
                Program.SaveTask();

                string FileName = Files[x];
#if !DEBUG
                try {
#endif
                    var Strings = Import(FileName);

                    if (Strings.Length == 0) {
                        if (dr == null || dr == DialogResult.Retry)
                            dr = MessageBox.Show($"Failed to Open the Script \"{Path.GetFileName(FileName)}\"", "TLBOT 2", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                        if (dr == DialogResult.Retry)
                            x--;                        
                        if (dr == DialogResult.Ignore || dr == DialogResult.Retry)
                            continue;
                        if (dr == DialogResult.Abort)
                            break;
                    }

                    TotalCount += (from z in Strings select (long)z.Length).Sum();

                    ShowStrings(Strings, NewFile: true);

                    var TaskCreator = new TranslationTask(Strings, Program.Settings.SourceLang, Program.Settings.TargetLang, EnabledOptimizators);
                    
                    Task Translate = TaskCreator.Build();

                    Translate.Start();
                    Text = $"TLBOT 2 - {Path.GetFileName(FileName)}";
                    int DL = 0;
                    int LP = 0;
                    while (TaskCreator.TaskStatus != TranslationTask.Status.Finished) {
                        try {
                            if (Program.TLMode == TransMode.Normal || Program.TLMode == TransMode.Multithread) {
                                lblState.Text = string.Format("{4}... ({0}/{1} Lines) ({2}/{3} Files)", TaskCreator.Progress, Strings.LongLength, x, Files.LongLength, GetStateName(TaskCreator.TaskStatus));
                                TaskProgress.Maximum = Strings.Length;
                                TaskProgress.Value = (int)TaskCreator.Progress;
                                StringList.SelectedIndex = (int)TaskCreator.Progress;

                                if (++DL == 20) {
                                    DL = 0;
                                    int Progress = (int)(Program.TLMode == TransMode.Normal ? TaskCreator.Progress : 0);
                                    ShowStrings(TaskCreator.Lines, LP, Progress);
                                    LP = Progress;
                                }
                            } else {
                                if (TaskCreator.TaskStatus == TranslationTask.Status.Translating)
                                    lblState.Text = string.Format("Translating... ({0}/{1} Files)", x, Files.LongLength);
                                else {
                                    lblState.Text = string.Format("{4}...  ({2}/{3} Lines) ({0}/{1} Files)", x, Files.LongLength, TaskCreator.Progress, Strings.LongLength, GetStateName(TaskCreator.TaskStatus));
                                    if (++DL == 20) {
                                        DL = 0;
                                        int Progress = (int)TaskCreator.Progress;
                                        ShowStrings(TaskCreator.Lines, LP, Progress);
                                        LP = Progress;
                                    }
                                }
                                TaskProgress.Maximum = Files.Length;
                                TaskProgress.Value = (int)x;
                            }
                        } catch { }
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }


                    for (uint i = 0; i < TaskCreator.Lines.LongLength; i++) {
                        if (i % (Strings.LongLength > 5000 ? 55 : 15) == 0) {
                            lblState.Text = string.Format("Finishing... ({0}/{1} Lines)", i, Strings.LongLength);
                            Application.DoEvents();
                        }

                        foreach (IOptimizator Optimizator in EnabledOptimizators)
                            try {
                                if (Optimizator is DialogueFilter)
                                    continue;

                                Optimizator.BeforeSave(ref TaskCreator.Lines[i], i);
                            } catch { }
                    }

                    bool Changed = false;
                    for (uint i = 0; i < Strings.Length; i++)
                        if (Strings[i] != TaskCreator.Lines[i]) {
                            Changed = true;
                            break;
                        }
                    if (Changed) {
                        if (Program.Settings.LSTMode) {
                            string LstPath = Path.GetDirectoryName(FileName) + "\\Strings-" + Path.GetFileNameWithoutExtension(FileName) + ".lst";
                            Dump(TaskCreator.Lines, LstPath);
                        } else
                            Export(TaskCreator.Lines, FileName);
                    }
#if !DEBUG
            } catch { }
#endif
            }

            TaskProgress.Value = TaskProgress.Maximum;
            Text = "TLBOT 2";
            lblState.Text = "IDLE";
            Program.TaskInfo = new TaskInfo();

            MessageBox.Show($"Task Finished:\n{TotalCount:N0} letters in this game.\nwith ${20 * (TotalCount / 1000000.00):N} of cost", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        string[] OriStrs;
        bool[] ValidList;
        Wrapper Wrapper = new Wrapper();
        private string[] Import(string File) {
            var Ori = Wrapper.Import(File);
            OriStrs = new string[Ori.LongLength];
            Ori.CopyTo(OriStrs, 0);

            for (uint i = 0; i < Ori.LongLength; i++) {
                if (i % (Ori.LongLength > 5000 ? 55 : 15 ) == 0) {
                    lblState.Text = string.Format("Initializing... ({0}/{1} Lines)", i, Ori.LongLength);
                    Application.DoEvents();
                }
                foreach (IOptimizator Optimizator in EnabledOptimizators)
                    try {
                        if (Optimizator is DialogueFilter)
                            continue;

                        Optimizator.AfterOpen(ref Ori[i], i);
                    } catch { }
            }

            var Filter = new DialogueFilter();

            string[] Strs = null;
            if (IsEnabled(Filter)) {
                ValidList = ValidDialogueList(Ori);
                List<string> List = new List<string>();
                for (uint i = 0; i < Ori.LongLength; i++)
                    if (ValidList[i]) {
                        List.Add(Ori[i]);
                    }

                Strs = List.ToArray();
            }

            return Strs ?? Ori;
        }
        const string BreakLineFlag = "::BREAKLINE::";
        const string ReturnLineFlag = "::RETURNLINE::";

        private bool Dump(string[] Translation, string SaveAs) {
            var Filter = new DialogueFilter();
            var Strs = Translation;
            if (IsEnabled(Filter)) {
                Strs = new string[OriStrs.Length];
                for (uint i = 0, x = 0; i < OriStrs.Length; i++)
                    if (ValidList[i])
                        Strs[i] = Translation[x++];
                    else
                        Strs[i] = OriStrs[i];
            }

            if (OriStrs.Length != Strs.Length)
                return false;
            if (OriStrs.Length == 0)
                return true;

            using (StreamWriter Writer = new StreamWriter(SaveAs)) {
                for (uint i = 0; i < OriStrs.Length; i++) {
                    if (OriStrs[i] == Strs[i])
                        continue;
                    if (string.IsNullOrWhiteSpace(OriStrs[i]))
                        continue;

                    Writer.WriteLine(OriStrs[i].Replace("\n", BreakLineFlag).Replace("\r", ReturnLineFlag));
                    Writer.WriteLine(Strs[i].Replace("\n", BreakLineFlag).Replace("\r", ReturnLineFlag));
                }
                Writer.Close();
            }

            return true;
        }
        private void Export(string[] Strings, string SaveAs) {
            var Filter = new DialogueFilter();

            string[] Strs = new string[Strings.Length];
            Strings.CopyTo(Strs, 0);
            if (IsEnabled(Filter)) {
                Strs = new string[OriStrs.Length];
                for (uint i = 0, x = 0; i < OriStrs.Length; i++)
                    if (ValidList[i])
                        Strs[i] = Strings[x++];
                    else
                        Strs[i] = OriStrs[i];
            }

            Wrapper.Export(Strs, SaveAs);
        }

        private bool[] ValidDialogueList(string[] Strings) {
            bool[] Values = new bool[Strings.LongLength];
            for (uint i = 0; i < Values.LongLength; i++)
                Values[i] = Strings[i].IsDialogue();


            if (Program.FilterSettings.UsePos || Program.FilterSettings.UsePosCaution)
                for (uint i = 2; i < Strings.Length - 2; i++) {
                    var BBefore = Values[i - 2];
                    var Before = Values[i - 1];
                    var Current = Values[i];
                    var Next = Values[i + 1];
                    var ANext = Values[i + 2];
                    if (BBefore != Before)
                        continue;
                    if (Next != ANext)
                        continue;
                    if (Next != Before)
                        continue;
                    if (Current == Before)
                        continue;
                    if (!Current)
                        continue;

                    Values[i] = Program.FilterSettings.UsePosCaution ? Strings[i - 1].IsDialogue(-2) : Before;
                }

            return Values;
        }

        private string GetStateName(TranslationTask.Status Status) {
            switch (Status) {
                case TranslationTask.Status.Finished:
                    return "Finishing...";
                case TranslationTask.Status.IDLE:
                    return "Waiting...";
                case TranslationTask.Status.PostProcessing:
                    return "Post-Processing";
                case TranslationTask.Status.PreProcessing:
                    return "Pre-Processing";
                case TranslationTask.Status.Translating:
                    return "Translating";
            }
            throw new Exception("Invalid State");
        }

        private void ShowStrings(string[] Strings, int Begin = 0, int End = 0, bool NewFile = false) {
            ShowingStrings = true;
            if (NewFile)
                StringList.Items.Clear();

            int LEnd = (End > 0 ? End : Strings.Length);
            if (Strings.Length == StringList.Items.Count) {
                for (int i = Begin; i < LEnd; i++) {
                    StringList.Items[i] = Strings[i];
                    if (i % (Strings.Length > 5000 ? 50 : 10) == 0)
                        StringList.SelectedIndex = i;
                }
            } else {
                StringList.Items.Clear();
                StringList.Items.AddRange(Strings);
                bool[] DialogueList = ValidDialogueList(Strings);
                for (int i = Begin; i < LEnd; i++) {
                    StringList.SetItemChecked(i, DialogueList[i]);
                }
            }
            ShowingStrings = false;
        }

        private void ClientChanged(object sender, EventArgs e) {
            Program.Settings.TLClient = TLCLientMenu.Text;
        }

        private void TransModeChanged(object sender, EventArgs e) {
            Program.Settings.TransMode = TransModeMenu.Text;
        }

        private void SourceLangChanged(object sender, EventArgs e) {
            Program.Settings.SourceLang = SourceLangSelector.Text;
            ValidateStep();
        }

        private void TargetLangChanged(object sender, EventArgs e) {
            Program.SaveCache();
            Program.Settings.TargetLang = TargetLangSelector.Text;

            Enabled = false;
            Program.LoadCache(() => {
                ValidateStep();
                TranslateWindow();

                if (!CacheLoadedVerify.Enabled)
                    Invoke(new Action(() => {
                        Enabled = true;
                    }));
            });
        }

        private void ValidateStep() {
            bool CantDoubleStep = Program.Settings.TargetLang?.ToUpper() == "EN";
            CantDoubleStep |= Program.Settings.SourceLang?.ToUpper() == "EN";
            if (CantDoubleStep) {
                TryInvoke(new MethodInvoker(() => {
                    TargetStepMode.Text = "Single Step";
                }));
            }
        }
        
        private void TryInvoke(Delegate Method) {
            try {
                Invoke(Method);
            } catch { }
        }
        private void FakeBreakLineChanged(object sender, EventArgs e) {
            Program.WordwrapSettings.FakeBreakLine = ckFakeBreakLine.Checked;
        }

        private void StepModeChanged(object sender, EventArgs e) {
            Program.Settings.DoubleStep = TargetStepMode.Text == "Double Step";
        }

        private void TLBClosing(object sender, FormClosingEventArgs e) {
            Program.SaveCache();
            Program.SaveSettings();
            try {
                Environment.Exit(0);
            } catch { System.Diagnostics.Process.GetCurrentProcess().Kill(); }
        }
        
        private void MaxWidthChanged(object sender, EventArgs e) {
            Program.WordwrapSettings.MaxWidth = (int)LineLimit.Value;
        }
        private void MaxLinesChanged(object sender, EventArgs e) {
            Program.WordwrapSettings.MaxLines = (int)MaxLines.Value;
        }

        private void LineBreakerChanged(object sender, EventArgs e) {
            try {
                LineBreaker.Text.Unescape();
                Program.WordwrapSettings.LineBreaker = LineBreaker.Text;
            } catch { }
        }

        private void FontSizeChanged(object sender, EventArgs e) {
            string Input = FontSize.MaskedTextProvider.ToDisplayString();
            if (float.TryParse(Input, out float Value)) {
                Program.WordwrapSettings.FontSize = Value;
            }
        }

        private void FontNameChanged(object sender, EventArgs e) {
            Program.WordwrapSettings.FontName = FaceName.Text;
        }

        private void BoldChanged(object sender, EventArgs e) {
            Program.WordwrapSettings.Bold = ckBold.Checked;
        }

        private void MonospacedChanged(object sender, EventArgs e) {
            Program.WordwrapSettings.Monospaced = ckMonospaced.Checked;
            ckBold.Enabled = !Program.WordwrapSettings.Monospaced;
            FaceName.Enabled = !Program.WordwrapSettings.Monospaced;
            FontSize.Enabled = !Program.WordwrapSettings.Monospaced;
        }

        private void TransWindowChanged(object sender, EventArgs e) {
            Program.Settings.TranslateWindow = ckTransTLBot.Checked;
            TranslateWindow();
        }

        private void PreviewDialoguesClick(object sender, EventArgs e) {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "All Files|*.*";
            if (fd.ShowDialog() != DialogResult.OK)
                return;

            Wrapper Wrapper = new Wrapper();
            var Content = Wrapper.Import(fd.FileName, TryLastPluginFirst: true);

            for (uint i = 0; i < Content.LongLength; i++) {
                if (i % (Content.LongLength > 5000 ? 55 : 15) == 0) {
                    lblState.Text = string.Format("Initializing... ({0}/{1} Lines)", i, Content.LongLength);
                    Application.DoEvents();
                }
                foreach (IOptimizator Optimizator in EnabledOptimizators) try {
                        Optimizator.AfterOpen(ref Content[i], i);
                } catch { }
            }

            for (uint i = 0; i < Content.LongLength; i++) {
                if (i % (Content.LongLength > 5000 ? 55 : 15) == 0) {
                    lblState.Text = string.Format("Finishing... ({0}/{1} Lines)", i, Content.LongLength);
                    Application.DoEvents();
                }
                foreach (IOptimizator Optimizator in EnabledOptimizators) try {
                        Optimizator.BeforeTranslate(ref Content[i], i);
                } catch { }
            }

            ShowStrings(Content, NewFile: true);
            lblState.Text = "IDLE";
        }

        private void StringListClicked(object sender, EventArgs e) {
            if (StringList.SelectedIndex < 0 || StringList.SelectedIndex > StringList.Items.Count || ShowingStrings)
                return;

            try {
                Clipboard.SetText(StringList.SelectedItem.ToString());
            } catch { }
        }

        private void StringListManualChecked(object sender, ItemCheckEventArgs e) {
            if (e.Index < 0 || e.Index > StringList.Items.Count || ShowingStrings)
                return;

            string Text = StringList.Items[e.Index].ToString();
            Program.ForceDialogues[Text] = e.NewValue == CheckState.Checked;
        }

        private void ItemChecked(object sender, ItemCheckEventArgs e) {
            IOptimizator Optimizator = AllOptimizators.Where(x=>x.GetName() == OptimizatorList.Items[e.Index].ToString()).First();
            bool Enabled = e.NewValue == CheckState.Checked;

            if (Optimizator is WordWraper) {
                Program.OptimizatorSettings.EnableWordWrap = Enabled;
            } else if (Optimizator is CaseFixer) {
                Program.OptimizatorSettings.CaseFixer = Enabled;
            } else if (Optimizator is Replaces) {
                Program.OptimizatorSettings.CustomReplaces = Enabled;
            } else if (Optimizator is JapFixer) {
                Program.OptimizatorSettings.JapaneseFixer = Enabled;
            } else if (Optimizator is DialogueFilter) {
                Program.OptimizatorSettings.DialogueFilter = Enabled;
            } else if (Optimizator is MassiveFix) {
                Program.OptimizatorSettings.MassiveFixer = Enabled;
            } else {
                foreach (IOptimizator EOptimizator in AllOptimizators) {
                    if (EOptimizator.GetName() != Optimizator.GetName())
                        continue;

                    string Name = CaseFixer.SetCase(EOptimizator.GetName(), CaseFixer.Case.Title).Trim().Replace(" ", "");
                    Ini.SetConfig("Optimizator", Name, Enabled ? "True" : "False", Program.INIPath);
                }
            }

        }

        private void DBPageCounter_Tick(object sender, EventArgs e) {
            if (Program.Cache?.Count == 0)
                return;
            int Count = Program.Cache.Count - (Program.Cache.Count % Program.Settings.DBViewPageLimit);
            int Pages = (Count / Program.Settings.DBViewPageLimit);

            DBPageSelector.Maximum = Pages;
        }

        private void ShowDBBnt_Click(object sender, EventArgs e) {
            DBStrList.Items.Clear();
            if (Program.Cache?.Count == 0)
                return;
            try {
                int Begin = (int)DBPageSelector.Value * Program.Settings.DBViewPageLimit;
                int End = Begin + Program.Settings.DBViewPageLimit;
                if (End > Program.Cache.Count)
                    End = Program.Cache.Count;

                for (int i = Begin; i < End; i++) {
                    DBStrList.Items.Add(new ListViewItem(new string[] {
                    Program.Cache.Keys.ElementAt(i).Escape(),
                    Program.Cache.Values.ElementAt(i).Escape()
                }));
                }
            } catch {
                if (sender != null) {
                    System.Threading.Thread.Sleep(100);
                    ShowDBBnt_Click(null, null);
                }
            }
        }

        private void ClearDbBnt_Click(object sender, EventArgs e) {
            if (Program.ForceDialogues.Count != 0) {
                var Rst = MessageBox.Show("You want clear only the dialogues filter cache?", "TLBOT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                Program.ForceDialogues = new Dictionary<string, bool>();
                if (Rst == DialogResult.Yes)
                    return;
            }

            Program.Cache = new Dictionary<string, string>();
            DBPageCounter_Tick(null, null);
            ShowDBBnt_Click(null, null);
        }

        private void ImportLstBnt_Click(object sender, EventArgs e) {
            FilesSelector Selector = new FilesSelector() {
                Filter = "*.lst"
            };

            if (DialogResult.OK != Selector.ShowDialog())
                return;
            foreach (string FN in Selector.SelectedFiles) {
                using (StreamReader Reader = new StreamReader(FN)) {
                    try {
                        while (Reader.Peek() >= 0) {
                            string L1 = Reader.ReadLine().Unescape();
                            string L2 = Reader.ReadLine().Unescape();
                            if (string.IsNullOrEmpty(L1))
                                continue;
                            if (string.IsNullOrEmpty(L2))
                                continue;
                            if (Program.Cache.ContainsKey(L1))
                                continue;
                            Program.Cache.Add(L1, L2);
                        }
                    } catch { }
                    Reader.Close();
                }
            }

            MessageBox.Show("LST's Imported.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportLstBnt_Click(object sender, EventArgs e) {
            SaveFileDialog FD = new SaveFileDialog();
            FD.Filter = "All LST Files|*.lst";
            if (FD.ShowDialog() != DialogResult.OK)
                return;
            if (File.Exists(FD.FileName))
                File.Delete(FD.FileName);

            using (StreamWriter Writer = new StreamWriter(FD.FileName)) {
                for (int i = 0; i < Program.Cache.Count; i++) {
                    Writer.WriteLine(Program.Cache.Keys.ElementAt(i).Escape());
                    Writer.WriteLine(Program.Cache.Values.ElementAt(i).Escape());
                }
                Writer.Close();
            }

            MessageBox.Show("Database Exported.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OptimizeDbBnt_Click(object sender, EventArgs e) {
            if (Program.Cache?.Count == 0)
                return;

            var ToRemove = (from x in Program.Cache where 
                            !x.Key.IsDialogue() || 
                            JapFixer.Minify(x.Key.Trim()) == JapFixer.Minify(x.Value.Trim())
                            select x.Key).ToArray();
            foreach (string Element in ToRemove) {
                Program.Cache.Remove(Element);
            }

            if (MessageBox.Show("Do you wanna trim the quotes?", "TLBOT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var Pairs = Program.Cache.ToList();
                Program.Cache = new Dictionary<string, string>();
                foreach (var Pair in Pairs)
                {
                    string Key = Pair.Key;
                    string Value = Pair.Value;

                    foreach (var Quote in QuoteTrim.Quotes)
                    {
                        if (Key.First() == Quote.Start.Value)
                            Key = Key.Substring(1);
                        if (Key.Last() == Quote.End.Value)
                            Key = Key.Substring(0, Key.Length - 1);

                        if (Value.First() == Quote.Start.Value)
                            Value = Value.Substring(1);
                        if (Value.Last() == Quote.End.Value)
                            Value = Value.Substring(0, Value.Length - 1);
                    }

                    Program.Cache[Key] = Value;
                }
            }

            MessageBox.Show($"Database Optimized, {ToRemove.Count()} Entries Removed.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SensetiveChanged(object sender, EventArgs e) {
            lblBarVal.Text = SensetiveBar.Value.ToString();
            Program.FilterSettings.Sensitivity = SensetiveBar.Value;
        }

        private void CacheLoadedVerifier(object sender, EventArgs e) {
            if (!Program.CacheReady)
                return;
            CacheLoadedVerify.Enabled = false;

            Enabled = true;
            lblState.Text = "IDLE";
        }

        private void UseDBChanged(object sender, EventArgs e) {
            Program.FilterSettings.UseDB = ckUseDB.Checked;
        }

        private void bntTestClient_Click(object sender, EventArgs e) {
            MessageBox.Show("Copy the text");
            string test = Clipboard.GetText();
            MessageBox.Show(test.Translate("EN", "PT", Translator.Google) + " - google");

        }

        private void bntSearch_Click(object sender, EventArgs e) {
            var Searcher = new Search();
            Searcher.ShowDialog();
        }

        private void GenDBBnt_Click(object sender, EventArgs e) {
            MessageBox.Show("Select all original scripts", "TLBOT 2", MessageBoxButtons.OK);
            FilesSelector Selector = new FilesSelector();
            if (Selector.ShowDialog() != DialogResult.OK)
                return;
            MessageBox.Show("Select the directory with your translated scripts", "TLBOT 2", MessageBoxButtons.OK);
            CommonOpenFileDialog OFD = new CommonOpenFileDialog();
            OFD.IsFolderPicker = true;
            OFD.Multiselect = false;
            OFD.Title = "Select the directory with translated scripts";
            if (OFD.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            string[] Scripts = Selector.SelectedFiles;
            string Directory = OFD.FileName;
            if (!Directory.EndsWith("\\"))
                Directory += "\\";

            string LOG = string.Empty;
            uint Lines = 0;

            Wrapper Wrapper = new Wrapper();
            foreach (string Script in Scripts) {
                string TLFile = Directory + Path.GetFileName(Script);
                if (!File.Exists(TLFile)) {
                    LOG += $"\n{Path.GetFileName(Script)} Not Found";
                    continue;
                }
                string[] OStrings = Wrapper.Import(Script);
                string[] TStrings = Wrapper.Import(TLFile);
                if (OStrings.Length != TStrings.Length) {
                    LOG += $"\n{Path.GetFileName(Script)} Isn't the same script";
                    continue;
                }
                for (uint i = 0; i < OStrings.Length; i++) {
                    if (OStrings[i] == TStrings[i])
                        continue;
                    Program.Cache[OStrings[i]] = TStrings[i];
                    Lines++;
                }
            }

            if (LOG != string.Empty && Lines == 0) {
                MessageBox.Show("Failed to generate the DB" + LOG, "TLBOT 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if (LOG != string.Empty) {
                MessageBox.Show($"\"{Lines}\" Entrys Imported, but...{LOG}", "TLBOT 2", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else if (Lines == 0) {
                MessageBox.Show("Something is wrong...", "TLBOT 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                MessageBox.Show($"\"{Lines}\" Entrys Imported", "TLBOT 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LstModeCheckedChanged(object sender, EventArgs e) {
            Program.Settings.LSTMode = ckLstMode.Checked;
        }

        private void UsePosCheckedChanged(object sender, EventArgs e) {
            Program.FilterSettings.UsePos = ckUsePos.CheckState == CheckState.Checked;
            Program.FilterSettings.UsePosCaution = ckUsePos.CheckState == CheckState.Indeterminate;
        }
    }
}
