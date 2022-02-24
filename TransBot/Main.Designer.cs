﻿namespace TLBOT {
    partial class Main {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.ViewTab = new System.Windows.Forms.TabPage();
            this.bntSearch = new System.Windows.Forms.Button();
            this.bntViewScript = new System.Windows.Forms.Button();
            this.StringList = new System.Windows.Forms.CheckedListBox();
            this.bntNewTask = new System.Windows.Forms.Button();
            this.lblState = new System.Windows.Forms.Label();
            this.lblInfoPrefix = new System.Windows.Forms.Label();
            this.TaskProgress = new System.Windows.Forms.ProgressBar();
            this.OptionsTab = new System.Windows.Forms.TabPage();
            this.ckLstMode = new System.Windows.Forms.CheckBox();
            this.bntTestClient = new System.Windows.Forms.Button();
            this.SensentiveGB = new System.Windows.Forms.GroupBox();
            this.ckUsePos = new System.Windows.Forms.CheckBox();
            this.ckUseDB = new System.Windows.Forms.CheckBox();
            this.lblBarVal = new System.Windows.Forms.Label();
            this.lblMoreSensetive = new System.Windows.Forms.Label();
            this.lblLessSensentive = new System.Windows.Forms.Label();
            this.SensetiveBar = new System.Windows.Forms.TrackBar();
            this.ckTransTLBot = new System.Windows.Forms.CheckBox();
            this.LineBreaker = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblMaxLines = new System.Windows.Forms.GroupBox();
            this.MaxLines = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.ckBold = new System.Windows.Forms.CheckBox();
            this.FontSize = new System.Windows.Forms.MaskedTextBox();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.FaceName = new System.Windows.Forms.TextBox();
            this.lblFaceName = new System.Windows.Forms.Label();
            this.ckMonospaced = new System.Windows.Forms.CheckBox();
            this.LineLimit = new System.Windows.Forms.NumericUpDown();
            this.lblLineLimit = new System.Windows.Forms.Label();
            this.ckFakeBreakLine = new System.Windows.Forms.CheckBox();
            this.TargetStepMode = new System.Windows.Forms.ComboBox();
            this.lblTargetSteps = new System.Windows.Forms.Label();
            this.OptimizatorList = new System.Windows.Forms.CheckedListBox();
            this.TargetLangSelector = new System.Windows.Forms.ComboBox();
            this.SourceLangSelector = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TransModeMenu = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TLCLientMenu = new System.Windows.Forms.ComboBox();
            this.lblClientPrefix = new System.Windows.Forms.Label();
            this.TranslationDB = new System.Windows.Forms.TabPage();
            this.GenDBBnt = new System.Windows.Forms.Button();
            this.OptimizeDbBnt = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.DBPageSelector = new System.Windows.Forms.NumericUpDown();
            this.ImportLstBnt = new System.Windows.Forms.Button();
            this.ExportLstBnt = new System.Windows.Forms.Button();
            this.ClearDbBnt = new System.Windows.Forms.Button();
            this.ShowDBBnt = new System.Windows.Forms.Button();
            this.DBStrList = new System.Windows.Forms.ListView();
            this.OriCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TransCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DBPageCounter = new System.Windows.Forms.Timer(this.components);
            this.CacheLoadedVerify = new System.Windows.Forms.Timer(this.components);
            this.MainTabControl.SuspendLayout();
            this.ViewTab.SuspendLayout();
            this.OptionsTab.SuspendLayout();
            this.SensentiveGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SensetiveBar)).BeginInit();
            this.lblMaxLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineLimit)).BeginInit();
            this.TranslationDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DBPageSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.ViewTab);
            this.MainTabControl.Controls.Add(this.OptionsTab);
            this.MainTabControl.Controls.Add(this.TranslationDB);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MainTabControl.MinimumSize = new System.Drawing.Size(1087, 609);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1088, 610);
            this.MainTabControl.TabIndex = 0;
            // 
            // ViewTab
            // 
            this.ViewTab.Controls.Add(this.bntSearch);
            this.ViewTab.Controls.Add(this.bntViewScript);
            this.ViewTab.Controls.Add(this.StringList);
            this.ViewTab.Controls.Add(this.bntNewTask);
            this.ViewTab.Controls.Add(this.lblState);
            this.ViewTab.Controls.Add(this.lblInfoPrefix);
            this.ViewTab.Controls.Add(this.TaskProgress);
            this.ViewTab.Location = new System.Drawing.Point(4, 25);
            this.ViewTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ViewTab.Name = "ViewTab";
            this.ViewTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ViewTab.Size = new System.Drawing.Size(1080, 581);
            this.ViewTab.TabIndex = 0;
            this.ViewTab.Text = "Visualização";
            this.ViewTab.UseVisualStyleBackColor = true;
            // 
            // bntSearch
            // 
            this.bntSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntSearch.Location = new System.Drawing.Point(720, 540);
            this.bntSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bntSearch.Name = "bntSearch";
            this.bntSearch.Size = new System.Drawing.Size(100, 28);
            this.bntSearch.TabIndex = 19;
            this.bntSearch.Text = "Pesquisar";
            this.bntSearch.UseVisualStyleBackColor = true;
            this.bntSearch.Click += new System.EventHandler(this.bntSearch_Click);
            // 
            // bntViewScript
            // 
            this.bntViewScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntViewScript.Location = new System.Drawing.Point(828, 540);
            this.bntViewScript.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bntViewScript.Name = "bntViewScript";
            this.bntViewScript.Size = new System.Drawing.Size(125, 28);
            this.bntViewScript.TabIndex = 6;
            this.bntViewScript.Text = "Visualizar Script";
            this.bntViewScript.UseVisualStyleBackColor = true;
            this.bntViewScript.Click += new System.EventHandler(this.PreviewDialoguesClick);
            // 
            // StringList
            // 
            this.StringList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StringList.FormattingEnabled = true;
            this.StringList.Location = new System.Drawing.Point(8, 7);
            this.StringList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StringList.Name = "StringList";
            this.StringList.Size = new System.Drawing.Size(1057, 514);
            this.StringList.TabIndex = 5;
            this.StringList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.StringListManualChecked);
            this.StringList.Click += new System.EventHandler(this.StringListClicked);
            // 
            // bntNewTask
            // 
            this.bntNewTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntNewTask.Location = new System.Drawing.Point(961, 540);
            this.bntNewTask.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bntNewTask.Name = "bntNewTask";
            this.bntNewTask.Size = new System.Drawing.Size(105, 28);
            this.bntNewTask.TabIndex = 4;
            this.bntNewTask.Text = "Nova Tarefa";
            this.bntNewTask.UseVisualStyleBackColor = true;
            this.bntNewTask.Click += new System.EventHandler(this.NewTask_Click);
            // 
            // lblState
            // 
            this.lblState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblState.Location = new System.Drawing.Point(480, 546);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(232, 16);
            this.lblState.TabIndex = 3;
            this.lblState.Text = "Loading Cache...";
            // 
            // lblInfoPrefix
            // 
            this.lblInfoPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInfoPrefix.AutoSize = true;
            this.lblInfoPrefix.Location = new System.Drawing.Point(415, 546);
            this.lblInfoPrefix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfoPrefix.Name = "lblInfoPrefix";
            this.lblInfoPrefix.Size = new System.Drawing.Size(56, 17);
            this.lblInfoPrefix.TabIndex = 2;
            this.lblInfoPrefix.Text = "Estado:";
            // 
            // TaskProgress
            // 
            this.TaskProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TaskProgress.Location = new System.Drawing.Point(11, 540);
            this.TaskProgress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TaskProgress.Name = "TaskProgress";
            this.TaskProgress.Size = new System.Drawing.Size(396, 28);
            this.TaskProgress.TabIndex = 1;
            // 
            // OptionsTab
            // 
            this.OptionsTab.Controls.Add(this.ckLstMode);
            this.OptionsTab.Controls.Add(this.bntTestClient);
            this.OptionsTab.Controls.Add(this.SensentiveGB);
            this.OptionsTab.Controls.Add(this.ckTransTLBot);
            this.OptionsTab.Controls.Add(this.LineBreaker);
            this.OptionsTab.Controls.Add(this.label4);
            this.OptionsTab.Controls.Add(this.lblMaxLines);
            this.OptionsTab.Controls.Add(this.TargetStepMode);
            this.OptionsTab.Controls.Add(this.lblTargetSteps);
            this.OptionsTab.Controls.Add(this.OptimizatorList);
            this.OptionsTab.Controls.Add(this.TargetLangSelector);
            this.OptionsTab.Controls.Add(this.SourceLangSelector);
            this.OptionsTab.Controls.Add(this.label3);
            this.OptionsTab.Controls.Add(this.label2);
            this.OptionsTab.Controls.Add(this.TransModeMenu);
            this.OptionsTab.Controls.Add(this.label1);
            this.OptionsTab.Controls.Add(this.TLCLientMenu);
            this.OptionsTab.Controls.Add(this.lblClientPrefix);
            this.OptionsTab.Location = new System.Drawing.Point(4, 25);
            this.OptionsTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OptionsTab.Name = "OptionsTab";
            this.OptionsTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OptionsTab.Size = new System.Drawing.Size(1080, 581);
            this.OptionsTab.TabIndex = 1;
            this.OptionsTab.Text = "Opções";
            this.OptionsTab.UseVisualStyleBackColor = true;
            // 
            // ckLstMode
            // 
            this.ckLstMode.AutoSize = true;
            this.ckLstMode.Location = new System.Drawing.Point(176, 158);
            this.ckLstMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckLstMode.Name = "ckLstMode";
            this.ckLstMode.Size = new System.Drawing.Size(56, 21);
            this.ckLstMode.TabIndex = 19;
            this.ckLstMode.Text = "LST";
            this.ckLstMode.UseVisualStyleBackColor = true;
            this.ckLstMode.CheckedChanged += new System.EventHandler(this.LstModeCheckedChanged);
            // 
            // bntTestClient
            // 
            this.bntTestClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.bntTestClient.Location = new System.Drawing.Point(19, 154);
            this.bntTestClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bntTestClient.Name = "bntTestClient";
            this.bntTestClient.Size = new System.Drawing.Size(56, 28);
            this.bntTestClient.TabIndex = 17;
            this.bntTestClient.Text = "Test Client";
            this.bntTestClient.UseVisualStyleBackColor = true;
            this.bntTestClient.Click += new System.EventHandler(this.bntTestClient_Click);
            // 
            // SensentiveGB
            // 
            this.SensentiveGB.Controls.Add(this.ckUsePos);
            this.SensentiveGB.Controls.Add(this.ckUseDB);
            this.SensentiveGB.Controls.Add(this.lblBarVal);
            this.SensentiveGB.Controls.Add(this.lblMoreSensetive);
            this.SensentiveGB.Controls.Add(this.lblLessSensentive);
            this.SensentiveGB.Controls.Add(this.SensetiveBar);
            this.SensentiveGB.Location = new System.Drawing.Point(247, 86);
            this.SensentiveGB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SensentiveGB.Name = "SensentiveGB";
            this.SensentiveGB.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SensentiveGB.Size = new System.Drawing.Size(303, 96);
            this.SensentiveGB.TabIndex = 16;
            this.SensentiveGB.TabStop = false;
            this.SensentiveGB.Text = "Sensibilidade do Filtro";
            // 
            // ckUsePos
            // 
            this.ckUsePos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckUsePos.AutoSize = true;
            this.ckUsePos.Checked = true;
            this.ckUsePos.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.ckUsePos.Location = new System.Drawing.Point(188, 68);
            this.ckUsePos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckUsePos.Name = "ckUsePos";
            this.ckUsePos.Size = new System.Drawing.Size(101, 21);
            this.ckUsePos.TabIndex = 21;
            this.ckUsePos.Text = "Usar Índice";
            this.ckUsePos.ThreeState = true;
            this.ckUsePos.UseVisualStyleBackColor = true;
            this.ckUsePos.CheckStateChanged += new System.EventHandler(this.UsePosCheckedChanged);
            // 
            // ckUseDB
            // 
            this.ckUseDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckUseDB.AutoSize = true;
            this.ckUseDB.Location = new System.Drawing.Point(8, 68);
            this.ckUseDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckUseDB.Name = "ckUseDB";
            this.ckUseDB.Size = new System.Drawing.Size(83, 21);
            this.ckUseDB.TabIndex = 20;
            this.ckUseDB.Text = "Usar DB";
            this.ckUseDB.UseVisualStyleBackColor = true;
            this.ckUseDB.CheckedChanged += new System.EventHandler(this.UseDBChanged);
            // 
            // lblBarVal
            // 
            this.lblBarVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBarVal.Location = new System.Drawing.Point(84, 69);
            this.lblBarVal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBarVal.Name = "lblBarVal";
            this.lblBarVal.Size = new System.Drawing.Size(137, 23);
            this.lblBarVal.TabIndex = 19;
            this.lblBarVal.Text = "2";
            this.lblBarVal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblMoreSensetive
            // 
            this.lblMoreSensetive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMoreSensetive.Location = new System.Drawing.Point(272, 23);
            this.lblMoreSensetive.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMoreSensetive.Name = "lblMoreSensetive";
            this.lblMoreSensetive.Size = new System.Drawing.Size(23, 22);
            this.lblMoreSensetive.TabIndex = 18;
            this.lblMoreSensetive.Text = "+";
            this.lblMoreSensetive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLessSensentive
            // 
            this.lblLessSensentive.Location = new System.Drawing.Point(8, 23);
            this.lblLessSensentive.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLessSensentive.Name = "lblLessSensentive";
            this.lblLessSensentive.Size = new System.Drawing.Size(21, 22);
            this.lblLessSensentive.TabIndex = 17;
            this.lblLessSensentive.Text = "-";
            this.lblLessSensentive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SensetiveBar
            // 
            this.SensetiveBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SensetiveBar.BackColor = System.Drawing.Color.White;
            this.SensetiveBar.LargeChange = 1;
            this.SensetiveBar.Location = new System.Drawing.Point(37, 23);
            this.SensetiveBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SensetiveBar.Maximum = 6;
            this.SensetiveBar.Minimum = -6;
            this.SensetiveBar.Name = "SensetiveBar";
            this.SensetiveBar.Size = new System.Drawing.Size(227, 56);
            this.SensetiveBar.TabIndex = 15;
            this.SensetiveBar.TickFrequency = 2;
            this.SensetiveBar.ValueChanged += new System.EventHandler(this.SensetiveChanged);
            // 
            // ckTransTLBot
            // 
            this.ckTransTLBot.AutoSize = true;
            this.ckTransTLBot.Location = new System.Drawing.Point(88, 158);
            this.ckTransTLBot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckTransTLBot.Name = "ckTransTLBot";
            this.ckTransTLBot.Size = new System.Drawing.Size(75, 21);
            this.ckTransTLBot.TabIndex = 14;
            this.ckTransTLBot.Text = "Self TL";
            this.ckTransTLBot.UseVisualStyleBackColor = true;
            this.ckTransTLBot.CheckedChanged += new System.EventHandler(this.TransWindowChanged);
            // 
            // LineBreaker
            // 
            this.LineBreaker.Location = new System.Drawing.Point(373, 54);
            this.LineBreaker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LineBreaker.Name = "LineBreaker";
            this.LineBreaker.Size = new System.Drawing.Size(152, 22);
            this.LineBreaker.TabIndex = 13;
            this.LineBreaker.Text = "\\n";
            this.LineBreaker.TextChanged += new System.EventHandler(this.LineBreakerChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(243, 58);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Quebra de linha:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMaxLines
            // 
            this.lblMaxLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxLines.Controls.Add(this.MaxLines);
            this.lblMaxLines.Controls.Add(this.label6);
            this.lblMaxLines.Controls.Add(this.ckBold);
            this.lblMaxLines.Controls.Add(this.FontSize);
            this.lblMaxLines.Controls.Add(this.lblFontSize);
            this.lblMaxLines.Controls.Add(this.FaceName);
            this.lblMaxLines.Controls.Add(this.lblFaceName);
            this.lblMaxLines.Controls.Add(this.ckMonospaced);
            this.lblMaxLines.Controls.Add(this.LineLimit);
            this.lblMaxLines.Controls.Add(this.lblLineLimit);
            this.lblMaxLines.Controls.Add(this.ckFakeBreakLine);
            this.lblMaxLines.Location = new System.Drawing.Point(557, 17);
            this.lblMaxLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblMaxLines.Name = "lblMaxLines";
            this.lblMaxLines.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblMaxLines.Size = new System.Drawing.Size(509, 165);
            this.lblMaxLines.TabIndex = 11;
            this.lblMaxLines.TabStop = false;
            this.lblMaxLines.Text = "Word Wrap";
            // 
            // MaxLines
            // 
            this.MaxLines.Location = new System.Drawing.Point(311, 57);
            this.MaxLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaxLines.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MaxLines.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.MaxLines.Name = "MaxLines";
            this.MaxLines.Size = new System.Drawing.Size(171, 22);
            this.MaxLines.TabIndex = 11;
            this.MaxLines.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.MaxLines.ValueChanged += new System.EventHandler(this.MaxLinesChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(169, 57);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Max. de Linhas";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckBold
            // 
            this.ckBold.AutoSize = true;
            this.ckBold.Location = new System.Drawing.Point(8, 80);
            this.ckBold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckBold.Name = "ckBold";
            this.ckBold.Size = new System.Drawing.Size(76, 21);
            this.ckBold.TabIndex = 9;
            this.ckBold.Text = "Negrito";
            this.ckBold.UseVisualStyleBackColor = true;
            this.ckBold.CheckedChanged += new System.EventHandler(this.BoldChanged);
            // 
            // FontSize
            // 
            this.FontSize.Enabled = false;
            this.FontSize.Location = new System.Drawing.Point(311, 91);
            this.FontSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FontSize.Mask = "00.0";
            this.FontSize.Name = "FontSize";
            this.FontSize.Size = new System.Drawing.Size(169, 22);
            this.FontSize.TabIndex = 8;
            this.FontSize.Text = "120";
            this.FontSize.TextChanged += new System.EventHandler(this.FontSizeChanged);
            // 
            // lblFontSize
            // 
            this.lblFontSize.Location = new System.Drawing.Point(169, 95);
            this.lblFontSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(133, 20);
            this.lblFontSize.TabIndex = 7;
            this.lblFontSize.Text = "Tamanho da Fonte:";
            this.lblFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FaceName
            // 
            this.FaceName.Location = new System.Drawing.Point(311, 123);
            this.FaceName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FaceName.Name = "FaceName";
            this.FaceName.Size = new System.Drawing.Size(169, 22);
            this.FaceName.TabIndex = 6;
            this.FaceName.Text = "Consolas";
            this.FaceName.TextChanged += new System.EventHandler(this.FontNameChanged);
            // 
            // lblFaceName
            // 
            this.lblFaceName.Location = new System.Drawing.Point(169, 124);
            this.lblFaceName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFaceName.Name = "lblFaceName";
            this.lblFaceName.Size = new System.Drawing.Size(133, 20);
            this.lblFaceName.TabIndex = 5;
            this.lblFaceName.Text = "Nome da Fonte:";
            this.lblFaceName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckMonospaced
            // 
            this.ckMonospaced.AutoSize = true;
            this.ckMonospaced.Checked = true;
            this.ckMonospaced.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckMonospaced.Location = new System.Drawing.Point(8, 22);
            this.ckMonospaced.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckMonospaced.Name = "ckMonospaced";
            this.ckMonospaced.Size = new System.Drawing.Size(150, 21);
            this.ckMonospaced.TabIndex = 4;
            this.ckMonospaced.Text = "Modo Monospaced";
            this.ckMonospaced.UseVisualStyleBackColor = true;
            this.ckMonospaced.CheckedChanged += new System.EventHandler(this.MonospacedChanged);
            // 
            // LineLimit
            // 
            this.LineLimit.Location = new System.Drawing.Point(311, 21);
            this.LineLimit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LineLimit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.LineLimit.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.LineLimit.Name = "LineLimit";
            this.LineLimit.Size = new System.Drawing.Size(171, 22);
            this.LineLimit.TabIndex = 3;
            this.LineLimit.ValueChanged += new System.EventHandler(this.MaxWidthChanged);
            // 
            // lblLineLimit
            // 
            this.lblLineLimit.Location = new System.Drawing.Point(169, 21);
            this.lblLineLimit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLineLimit.Name = "lblLineLimit";
            this.lblLineLimit.Size = new System.Drawing.Size(133, 20);
            this.lblLineLimit.TabIndex = 2;
            this.lblLineLimit.Text = "Limite por Linha:";
            this.lblLineLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckFakeBreakLine
            // 
            this.ckFakeBreakLine.AutoSize = true;
            this.ckFakeBreakLine.Location = new System.Drawing.Point(8, 52);
            this.ckFakeBreakLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckFakeBreakLine.Name = "ckFakeBreakLine";
            this.ckFakeBreakLine.Size = new System.Drawing.Size(154, 21);
            this.ckFakeBreakLine.TabIndex = 1;
            this.ckFakeBreakLine.Text = "Wordwrap simulado";
            this.ckFakeBreakLine.UseVisualStyleBackColor = true;
            this.ckFakeBreakLine.CheckedChanged += new System.EventHandler(this.FakeBreakLineChanged);
            // 
            // TargetStepMode
            // 
            this.TargetStepMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TargetStepMode.FormattingEnabled = true;
            this.TargetStepMode.Items.AddRange(new object[] {
            "Single Step",
            "Double Step"});
            this.TargetStepMode.Location = new System.Drawing.Point(373, 21);
            this.TargetStepMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TargetStepMode.Name = "TargetStepMode";
            this.TargetStepMode.Size = new System.Drawing.Size(152, 24);
            this.TargetStepMode.TabIndex = 10;
            this.TargetStepMode.SelectedValueChanged += new System.EventHandler(this.StepModeChanged);
            // 
            // lblTargetSteps
            // 
            this.lblTargetSteps.Location = new System.Drawing.Point(243, 21);
            this.lblTargetSteps.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTargetSteps.Name = "lblTargetSteps";
            this.lblTargetSteps.Size = new System.Drawing.Size(123, 22);
            this.lblTargetSteps.TabIndex = 9;
            this.lblTargetSteps.Text = "Transição:";
            this.lblTargetSteps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OptimizatorList
            // 
            this.OptimizatorList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OptimizatorList.FormattingEnabled = true;
            this.OptimizatorList.Location = new System.Drawing.Point(15, 190);
            this.OptimizatorList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OptimizatorList.Name = "OptimizatorList";
            this.OptimizatorList.Size = new System.Drawing.Size(1051, 361);
            this.OptimizatorList.Sorted = true;
            this.OptimizatorList.TabIndex = 8;
            this.OptimizatorList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ItemChecked);
            // 
            // TargetLangSelector
            // 
            this.TargetLangSelector.FormattingEnabled = true;
            this.TargetLangSelector.Items.AddRange(new object[] {
            "JA",
            "EN",
            "CH",
            "RU",
            "PT",
            "ES",
            "IT",
            "FR",
            "PL",
            "DE"});
            this.TargetLangSelector.Location = new System.Drawing.Point(149, 123);
            this.TargetLangSelector.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TargetLangSelector.Name = "TargetLangSelector";
            this.TargetLangSelector.Size = new System.Drawing.Size(83, 24);
            this.TargetLangSelector.TabIndex = 7;
            this.TargetLangSelector.Text = "EN";
            this.TargetLangSelector.SelectedValueChanged += new System.EventHandler(this.TargetLangChanged);
            this.TargetLangSelector.TextChanged += new System.EventHandler(this.TargetLangChanged);
            // 
            // SourceLangSelector
            // 
            this.SourceLangSelector.FormattingEnabled = true;
            this.SourceLangSelector.Items.AddRange(new object[] {
            "JA",
            "EN",
            "CH",
            "RU",
            "PT",
            "ES",
            "IT",
            "FR",
            "PL",
            "DE",
            "AUTO"});
            this.SourceLangSelector.Location = new System.Drawing.Point(149, 90);
            this.SourceLangSelector.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SourceLangSelector.Name = "SourceLangSelector";
            this.SourceLangSelector.Size = new System.Drawing.Size(83, 24);
            this.SourceLangSelector.TabIndex = 6;
            this.SourceLangSelector.Text = "JA";
            this.SourceLangSelector.SelectedValueChanged += new System.EventHandler(this.SourceLangChanged);
            this.SourceLangSelector.TextChanged += new System.EventHandler(this.SourceLangChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 123);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 22);
            this.label3.TabIndex = 5;
            this.label3.Text = "Língua Alvo:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Língua de Origem:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TransModeMenu
            // 
            this.TransModeMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TransModeMenu.FormattingEnabled = true;
            this.TransModeMenu.Items.AddRange(new object[] {
            "Massive",
            "Multithread",
            "Normal"});
            this.TransModeMenu.Location = new System.Drawing.Point(95, 50);
            this.TransModeMenu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TransModeMenu.Name = "TransModeMenu";
            this.TransModeMenu.Size = new System.Drawing.Size(137, 24);
            this.TransModeMenu.TabIndex = 3;
            this.TransModeMenu.SelectedValueChanged += new System.EventHandler(this.TransModeChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Modo:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TLCLientMenu
            // 
            this.TLCLientMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TLCLientMenu.FormattingEnabled = true;
            this.TLCLientMenu.Items.AddRange(new object[] {
            "Google",
            "Cache Only"});
            this.TLCLientMenu.Location = new System.Drawing.Point(95, 17);
            this.TLCLientMenu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TLCLientMenu.Name = "TLCLientMenu";
            this.TLCLientMenu.Size = new System.Drawing.Size(137, 24);
            this.TLCLientMenu.TabIndex = 1;
            this.TLCLientMenu.SelectedValueChanged += new System.EventHandler(this.ClientChanged);
            // 
            // lblClientPrefix
            // 
            this.lblClientPrefix.Location = new System.Drawing.Point(11, 21);
            this.lblClientPrefix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClientPrefix.Name = "lblClientPrefix";
            this.lblClientPrefix.Size = new System.Drawing.Size(76, 22);
            this.lblClientPrefix.TabIndex = 0;
            this.lblClientPrefix.Text = "Cliente:";
            this.lblClientPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TranslationDB
            // 
            this.TranslationDB.Controls.Add(this.GenDBBnt);
            this.TranslationDB.Controls.Add(this.OptimizeDbBnt);
            this.TranslationDB.Controls.Add(this.label5);
            this.TranslationDB.Controls.Add(this.DBPageSelector);
            this.TranslationDB.Controls.Add(this.ImportLstBnt);
            this.TranslationDB.Controls.Add(this.ExportLstBnt);
            this.TranslationDB.Controls.Add(this.ClearDbBnt);
            this.TranslationDB.Controls.Add(this.ShowDBBnt);
            this.TranslationDB.Controls.Add(this.DBStrList);
            this.TranslationDB.Location = new System.Drawing.Point(4, 25);
            this.TranslationDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TranslationDB.Name = "TranslationDB";
            this.TranslationDB.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TranslationDB.Size = new System.Drawing.Size(1080, 581);
            this.TranslationDB.TabIndex = 2;
            this.TranslationDB.Text = "Banco de Dados";
            this.TranslationDB.UseVisualStyleBackColor = true;
            // 
            // GenDBBnt
            // 
            this.GenDBBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GenDBBnt.Location = new System.Drawing.Point(348, 540);
            this.GenDBBnt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GenDBBnt.Name = "GenDBBnt";
            this.GenDBBnt.Size = new System.Drawing.Size(137, 28);
            this.GenDBBnt.TabIndex = 8;
            this.GenDBBnt.Text = "Gerar Database";
            this.GenDBBnt.UseVisualStyleBackColor = true;
            this.GenDBBnt.Click += new System.EventHandler(this.GenDBBnt_Click);
            // 
            // OptimizeDbBnt
            // 
            this.OptimizeDbBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OptimizeDbBnt.Location = new System.Drawing.Point(784, 540);
            this.OptimizeDbBnt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OptimizeDbBnt.Name = "OptimizeDbBnt";
            this.OptimizeDbBnt.Size = new System.Drawing.Size(137, 28);
            this.OptimizeDbBnt.TabIndex = 7;
            this.OptimizeDbBnt.Text = "Otimizar Database";
            this.OptimizeDbBnt.UseVisualStyleBackColor = true;
            this.OptimizeDbBnt.Click += new System.EventHandler(this.OptimizeDbBnt_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(156, 540);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 28);
            this.label5.TabIndex = 6;
            this.label5.Text = "Página:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DBPageSelector
            // 
            this.DBPageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DBPageSelector.Location = new System.Drawing.Point(255, 544);
            this.DBPageSelector.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DBPageSelector.Name = "DBPageSelector";
            this.DBPageSelector.Size = new System.Drawing.Size(73, 22);
            this.DBPageSelector.TabIndex = 5;
            // 
            // ImportLstBnt
            // 
            this.ImportLstBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportLstBnt.Location = new System.Drawing.Point(493, 540);
            this.ImportLstBnt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ImportLstBnt.Name = "ImportLstBnt";
            this.ImportLstBnt.Size = new System.Drawing.Size(137, 28);
            this.ImportLstBnt.TabIndex = 4;
            this.ImportLstBnt.Text = "Importar Database";
            this.ImportLstBnt.UseVisualStyleBackColor = true;
            this.ImportLstBnt.Click += new System.EventHandler(this.ImportLstBnt_Click);
            // 
            // ExportLstBnt
            // 
            this.ExportLstBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportLstBnt.Location = new System.Drawing.Point(639, 540);
            this.ExportLstBnt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExportLstBnt.Name = "ExportLstBnt";
            this.ExportLstBnt.Size = new System.Drawing.Size(137, 28);
            this.ExportLstBnt.TabIndex = 3;
            this.ExportLstBnt.Text = "Exportar Database";
            this.ExportLstBnt.UseVisualStyleBackColor = true;
            this.ExportLstBnt.Click += new System.EventHandler(this.ExportLstBnt_Click);
            // 
            // ClearDbBnt
            // 
            this.ClearDbBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearDbBnt.Location = new System.Drawing.Point(929, 540);
            this.ClearDbBnt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ClearDbBnt.Name = "ClearDbBnt";
            this.ClearDbBnt.Size = new System.Drawing.Size(137, 28);
            this.ClearDbBnt.TabIndex = 2;
            this.ClearDbBnt.Text = "Limpar Database";
            this.ClearDbBnt.UseVisualStyleBackColor = true;
            this.ClearDbBnt.Click += new System.EventHandler(this.ClearDbBnt_Click);
            // 
            // ShowDBBnt
            // 
            this.ShowDBBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowDBBnt.Location = new System.Drawing.Point(11, 540);
            this.ShowDBBnt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ShowDBBnt.Name = "ShowDBBnt";
            this.ShowDBBnt.Size = new System.Drawing.Size(137, 28);
            this.ShowDBBnt.TabIndex = 1;
            this.ShowDBBnt.Text = "Exibir Database";
            this.ShowDBBnt.UseVisualStyleBackColor = true;
            this.ShowDBBnt.Click += new System.EventHandler(this.ShowDBBnt_Click);
            // 
            // DBStrList
            // 
            this.DBStrList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DBStrList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.OriCol,
            this.TransCol});
            this.DBStrList.HideSelection = false;
            this.DBStrList.Location = new System.Drawing.Point(8, 4);
            this.DBStrList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DBStrList.Name = "DBStrList";
            this.DBStrList.Size = new System.Drawing.Size(1057, 525);
            this.DBStrList.TabIndex = 0;
            this.DBStrList.UseCompatibleStateImageBehavior = false;
            this.DBStrList.View = System.Windows.Forms.View.Details;
            // 
            // OriCol
            // 
            this.OriCol.Text = "Original";
            this.OriCol.Width = 343;
            // 
            // TransCol
            // 
            this.TransCol.Text = "Tradução";
            this.TransCol.Width = 446;
            // 
            // DBPageCounter
            // 
            this.DBPageCounter.Enabled = true;
            this.DBPageCounter.Interval = 1000;
            this.DBPageCounter.Tick += new System.EventHandler(this.DBPageCounter_Tick);
            // 
            // CacheLoadedVerify
            // 
            this.CacheLoadedVerify.Enabled = true;
            this.CacheLoadedVerify.Interval = 200;
            this.CacheLoadedVerify.Tick += new System.EventHandler(this.CacheLoadedVerifier);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 610);
            this.Controls.Add(this.MainTabControl);
            this.Enabled = false;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(1103, 648);
            this.Name = "Main";
            this.Text = "TLBOT 2022";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TLBClosing);
            this.MainTabControl.ResumeLayout(false);
            this.ViewTab.ResumeLayout(false);
            this.ViewTab.PerformLayout();
            this.OptionsTab.ResumeLayout(false);
            this.OptionsTab.PerformLayout();
            this.SensentiveGB.ResumeLayout(false);
            this.SensentiveGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SensetiveBar)).EndInit();
            this.lblMaxLines.ResumeLayout(false);
            this.lblMaxLines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineLimit)).EndInit();
            this.TranslationDB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DBPageSelector)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage ViewTab;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblInfoPrefix;
        private System.Windows.Forms.ProgressBar TaskProgress;
        private System.Windows.Forms.TabPage OptionsTab;
        private System.Windows.Forms.Button bntNewTask;
        private System.Windows.Forms.ComboBox TLCLientMenu;
        private System.Windows.Forms.Label lblClientPrefix;
        private System.Windows.Forms.ComboBox TransModeMenu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox TargetLangSelector;
        private System.Windows.Forms.ComboBox SourceLangSelector;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox TargetStepMode;
        private System.Windows.Forms.Label lblTargetSteps;
        private System.Windows.Forms.CheckedListBox OptimizatorList;
        private System.Windows.Forms.GroupBox lblMaxLines;
        private System.Windows.Forms.CheckBox ckMonospaced;
        private System.Windows.Forms.NumericUpDown LineLimit;
        private System.Windows.Forms.Label lblLineLimit;
        private System.Windows.Forms.CheckBox ckFakeBreakLine;
        private System.Windows.Forms.TextBox LineBreaker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblFaceName;
        private System.Windows.Forms.TextBox FaceName;
        private System.Windows.Forms.MaskedTextBox FontSize;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.CheckBox ckBold;
        private System.Windows.Forms.CheckBox ckTransTLBot;
        private System.Windows.Forms.CheckedListBox StringList;
        private System.Windows.Forms.Button bntViewScript;
        private System.Windows.Forms.TabPage TranslationDB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown DBPageSelector;
        private System.Windows.Forms.Button ImportLstBnt;
        private System.Windows.Forms.Button ExportLstBnt;
        private System.Windows.Forms.Button ClearDbBnt;
        private System.Windows.Forms.Button ShowDBBnt;
        private System.Windows.Forms.ListView DBStrList;
        private System.Windows.Forms.ColumnHeader OriCol;
        private System.Windows.Forms.ColumnHeader TransCol;
        private System.Windows.Forms.Timer DBPageCounter;
        private System.Windows.Forms.Button OptimizeDbBnt;
        private System.Windows.Forms.GroupBox SensentiveGB;
        private System.Windows.Forms.Label lblMoreSensetive;
        private System.Windows.Forms.Label lblLessSensentive;
        private System.Windows.Forms.TrackBar SensetiveBar;
        private System.Windows.Forms.Label lblBarVal;
        private System.Windows.Forms.Timer CacheLoadedVerify;
        private System.Windows.Forms.CheckBox ckUseDB;
        private System.Windows.Forms.Button bntTestClient;
        private System.Windows.Forms.Button GenDBBnt;
        private System.Windows.Forms.CheckBox ckLstMode;
        private System.Windows.Forms.CheckBox ckUsePos;
        private System.Windows.Forms.Button bntSearch;
        private System.Windows.Forms.NumericUpDown MaxLines;
        private System.Windows.Forms.Label label6;
    }
}