namespace TLBOT {
    partial class Form1 {
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
            this.StringList = new System.Windows.Forms.CheckedListBox();
            this.InputLang = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OutLang = new System.Windows.Forms.ComboBox();
            this.BntProc = new System.Windows.Forms.Button();
            this.BntOpen = new System.Windows.Forms.Button();
            this.BntSave = new System.Windows.Forms.Button();
            this.OpenBinary = new System.Windows.Forms.OpenFileDialog();
            this.SaveBinary = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.BotSelect = new System.Windows.Forms.Button();
            this.BntBathFind = new System.Windows.Forms.Button();
            this.BntBathProc = new System.Windows.Forms.Button();
            this.LblInfo = new System.Windows.Forms.Label();
            this.MassMode = new System.Windows.Forms.CheckBox();
            this.Begin = new System.Windows.Forms.NumericUpDown();
            this.End = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SearchTB = new System.Windows.Forms.TextBox();
            this.MassSelect = new System.Windows.Forms.Button();
            this.FoldTrans = new System.Windows.Forms.Button();
            this.SaveDB = new System.Windows.Forms.Button();
            this.LoadDB = new System.Windows.Forms.Button();
            this.OverwriteBnt = new System.Windows.Forms.Button();
            this.ckDoubleStep = new System.Windows.Forms.CheckBox();
            this.ExportTextBnt = new System.Windows.Forms.Button();
            this.ImportTextBnt = new System.Windows.Forms.Button();
            this.MaxPerLine = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ckUseOriLen = new System.Windows.Forms.CheckBox();
            this.Client = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.Begin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.End)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxPerLine)).BeginInit();
            this.SuspendLayout();
            // 
            // StringList
            // 
            this.StringList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StringList.FormattingEnabled = true;
            this.StringList.Location = new System.Drawing.Point(12, 12);
            this.StringList.Name = "StringList";
            this.StringList.Size = new System.Drawing.Size(680, 289);
            this.StringList.TabIndex = 0;
            this.StringList.SelectedIndexChanged += new System.EventHandler(this.StringList_SelectedIndexChanged);
            // 
            // InputLang
            // 
            this.InputLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InputLang.FormattingEnabled = true;
            this.InputLang.Items.AddRange(new object[] {
            "JA",
            "EN",
            "ES",
            "PT",
            "CH",
            "RU"});
            this.InputLang.Location = new System.Drawing.Point(172, 413);
            this.InputLang.Name = "InputLang";
            this.InputLang.Size = new System.Drawing.Size(40, 21);
            this.InputLang.TabIndex = 1;
            this.InputLang.Text = "JA";
            this.InputLang.TextChanged += new System.EventHandler(this.VerifyLang);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 416);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Translate selected values from:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 416);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To:";
            // 
            // OutLang
            // 
            this.OutLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OutLang.FormattingEnabled = true;
            this.OutLang.Items.AddRange(new object[] {
            "JA",
            "EN",
            "ES",
            "PT",
            "CH",
            "RU"});
            this.OutLang.Location = new System.Drawing.Point(247, 413);
            this.OutLang.Name = "OutLang";
            this.OutLang.Size = new System.Drawing.Size(40, 21);
            this.OutLang.TabIndex = 4;
            this.OutLang.Text = "EN";
            this.OutLang.TextChanged += new System.EventHandler(this.VerifyLang);
            // 
            // BntProc
            // 
            this.BntProc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BntProc.Location = new System.Drawing.Point(416, 412);
            this.BntProc.Name = "BntProc";
            this.BntProc.Size = new System.Drawing.Size(75, 23);
            this.BntProc.TabIndex = 5;
            this.BntProc.Text = "Translate!";
            this.BntProc.UseVisualStyleBackColor = true;
            this.BntProc.Click += new System.EventHandler(this.BntProc_Click);
            // 
            // BntOpen
            // 
            this.BntOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BntOpen.Location = new System.Drawing.Point(549, 412);
            this.BntOpen.Name = "BntOpen";
            this.BntOpen.Size = new System.Drawing.Size(69, 23);
            this.BntOpen.TabIndex = 6;
            this.BntOpen.Text = "Open";
            this.BntOpen.UseVisualStyleBackColor = true;
            this.BntOpen.Click += new System.EventHandler(this.BntOpen_Click);
            // 
            // BntSave
            // 
            this.BntSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BntSave.Location = new System.Drawing.Point(623, 412);
            this.BntSave.Name = "BntSave";
            this.BntSave.Size = new System.Drawing.Size(69, 23);
            this.BntSave.TabIndex = 7;
            this.BntSave.Text = "Save";
            this.BntSave.UseVisualStyleBackColor = true;
            this.BntSave.Click += new System.EventHandler(this.BntSave_Click);
            // 
            // OpenBinary
            // 
            this.OpenBinary.DefaultExt = "scn";
            this.OpenBinary.Filter = "All Eshully Script Files | *.bin";
            this.OpenBinary.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenBinary_FileOk);
            // 
            // SaveBinary
            // 
            this.SaveBinary.DefaultExt = "scb";
            this.SaveBinary.Filter = "All Eshully Script Files | *.bin";
            this.SaveBinary.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveBinary_FileOk);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(293, 417);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "With: ";
            // 
            // BotSelect
            // 
            this.BotSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BotSelect.Location = new System.Drawing.Point(95, 333);
            this.BotSelect.Name = "BotSelect";
            this.BotSelect.Size = new System.Drawing.Size(94, 23);
            this.BotSelect.TabIndex = 11;
            this.BotSelect.Text = "Text Recog.";
            this.BotSelect.UseVisualStyleBackColor = true;
            this.BotSelect.Click += new System.EventHandler(this.BotSelect_Click);
            // 
            // BntBathFind
            // 
            this.BntBathFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BntBathFind.Location = new System.Drawing.Point(12, 333);
            this.BntBathFind.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BntBathFind.Name = "BntBathFind";
            this.BntBathFind.Size = new System.Drawing.Size(79, 23);
            this.BntBathFind.TabIndex = 12;
            this.BntBathFind.Text = "Batch Find";
            this.BntBathFind.UseVisualStyleBackColor = true;
            this.BntBathFind.Click += new System.EventHandler(this.button1_Click);
            // 
            // BntBathProc
            // 
            this.BntBathProc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BntBathProc.Location = new System.Drawing.Point(12, 307);
            this.BntBathProc.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BntBathProc.Name = "BntBathProc";
            this.BntBathProc.Size = new System.Drawing.Size(122, 23);
            this.BntBathProc.TabIndex = 13;
            this.BntBathProc.Text = "Batch Trans.";
            this.BntBathProc.UseVisualStyleBackColor = true;
            this.BntBathProc.Click += new System.EventHandler(this.BntBathProc_Click);
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LblInfo.Location = new System.Drawing.Point(443, 335);
            this.LblInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LblInfo.Size = new System.Drawing.Size(249, 19);
            this.LblInfo.TabIndex = 14;
            this.LblInfo.Text = "TLBOT by Marcussacana - {0} Build";
            this.LblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MassMode
            // 
            this.MassMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MassMode.AutoSize = true;
            this.MassMode.Checked = true;
            this.MassMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MassMode.Location = new System.Drawing.Point(251, 309);
            this.MassMode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MassMode.Name = "MassMode";
            this.MassMode.Size = new System.Drawing.Size(95, 17);
            this.MassMode.TabIndex = 16;
            this.MassMode.Text = "Massive Mode";
            this.MassMode.UseVisualStyleBackColor = true;
            // 
            // Begin
            // 
            this.Begin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Begin.Increment = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.Begin.Location = new System.Drawing.Point(162, 388);
            this.Begin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Begin.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Begin.Name = "Begin";
            this.Begin.Size = new System.Drawing.Size(85, 20);
            this.Begin.TabIndex = 17;
            // 
            // End
            // 
            this.End.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.End.Increment = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.End.Location = new System.Drawing.Point(273, 388);
            this.End.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.End.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(76, 20);
            this.End.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 390);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Process Only the string from ";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(252, 390);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "At";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(380, 390);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Line:";
            // 
            // SearchTB
            // 
            this.SearchTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchTB.Location = new System.Drawing.Point(414, 388);
            this.SearchTB.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SearchTB.Name = "SearchTB";
            this.SearchTB.Size = new System.Drawing.Size(190, 20);
            this.SearchTB.TabIndex = 22;
            this.SearchTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchKeyPress);
            // 
            // MassSelect
            // 
            this.MassSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MassSelect.Location = new System.Drawing.Point(195, 333);
            this.MassSelect.Name = "MassSelect";
            this.MassSelect.Size = new System.Drawing.Size(52, 23);
            this.MassSelect.TabIndex = 23;
            this.MassSelect.Text = "Sel. All";
            this.MassSelect.UseVisualStyleBackColor = true;
            this.MassSelect.Click += new System.EventHandler(this.MassSelect_Click);
            // 
            // FoldTrans
            // 
            this.FoldTrans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FoldTrans.Location = new System.Drawing.Point(139, 307);
            this.FoldTrans.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.FoldTrans.Name = "FoldTrans";
            this.FoldTrans.Size = new System.Drawing.Size(108, 23);
            this.FoldTrans.TabIndex = 24;
            this.FoldTrans.Text = "Folder Trans.";
            this.FoldTrans.UseVisualStyleBackColor = true;
            this.FoldTrans.Click += new System.EventHandler(this.FoldTrans_Click);
            // 
            // SaveDB
            // 
            this.SaveDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveDB.Location = new System.Drawing.Point(350, 307);
            this.SaveDB.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SaveDB.Name = "SaveDB";
            this.SaveDB.Size = new System.Drawing.Size(74, 19);
            this.SaveDB.TabIndex = 25;
            this.SaveDB.Text = "Save TL DB";
            this.SaveDB.UseVisualStyleBackColor = true;
            this.SaveDB.Click += new System.EventHandler(this.SaveDB_Click);
            // 
            // LoadDB
            // 
            this.LoadDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LoadDB.Location = new System.Drawing.Point(428, 307);
            this.LoadDB.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.LoadDB.Name = "LoadDB";
            this.LoadDB.Size = new System.Drawing.Size(74, 19);
            this.LoadDB.TabIndex = 26;
            this.LoadDB.Text = "Load TL DB";
            this.LoadDB.UseVisualStyleBackColor = true;
            this.LoadDB.Click += new System.EventHandler(this.LoadDB_Click);
            // 
            // OverwriteBnt
            // 
            this.OverwriteBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OverwriteBnt.Location = new System.Drawing.Point(608, 388);
            this.OverwriteBnt.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.OverwriteBnt.Name = "OverwriteBnt";
            this.OverwriteBnt.Size = new System.Drawing.Size(85, 19);
            this.OverwriteBnt.TabIndex = 27;
            this.OverwriteBnt.Text = "Overwrite Line";
            this.OverwriteBnt.UseVisualStyleBackColor = true;
            this.OverwriteBnt.Click += new System.EventHandler(this.OverwriteBnt_Click);
            // 
            // ckDoubleStep
            // 
            this.ckDoubleStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckDoubleStep.AutoSize = true;
            this.ckDoubleStep.Location = new System.Drawing.Point(251, 334);
            this.ckDoubleStep.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ckDoubleStep.Name = "ckDoubleStep";
            this.ckDoubleStep.Size = new System.Drawing.Size(78, 17);
            this.ckDoubleStep.TabIndex = 28;
            this.ckDoubleStep.Text = "2-Steps TL";
            this.ckDoubleStep.UseVisualStyleBackColor = true;
            // 
            // ExportTextBnt
            // 
            this.ExportTextBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ExportTextBnt.Location = new System.Drawing.Point(585, 307);
            this.ExportTextBnt.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ExportTextBnt.Name = "ExportTextBnt";
            this.ExportTextBnt.Size = new System.Drawing.Size(74, 19);
            this.ExportTextBnt.TabIndex = 29;
            this.ExportTextBnt.Text = "Export Text";
            this.ExportTextBnt.UseVisualStyleBackColor = true;
            this.ExportTextBnt.Click += new System.EventHandler(this.ExportClicked);
            // 
            // ImportTextBnt
            // 
            this.ImportTextBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImportTextBnt.Location = new System.Drawing.Point(507, 307);
            this.ImportTextBnt.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ImportTextBnt.Name = "ImportTextBnt";
            this.ImportTextBnt.Size = new System.Drawing.Size(74, 19);
            this.ImportTextBnt.TabIndex = 30;
            this.ImportTextBnt.Text = "Import Text";
            this.ImportTextBnt.UseVisualStyleBackColor = true;
            this.ImportTextBnt.Click += new System.EventHandler(this.ImportClick);
            // 
            // MaxPerLine
            // 
            this.MaxPerLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MaxPerLine.Location = new System.Drawing.Point(139, 362);
            this.MaxPerLine.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaxPerLine.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.MaxPerLine.Name = "MaxPerLine";
            this.MaxPerLine.Size = new System.Drawing.Size(85, 20);
            this.MaxPerLine.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 364);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Wordwrap with max of:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(229, 364);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "chars per line. (0=Off)";
            // 
            // ckUseOriLen
            // 
            this.ckUseOriLen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckUseOriLen.AutoSize = true;
            this.ckUseOriLen.Location = new System.Drawing.Point(343, 363);
            this.ckUseOriLen.Name = "ckUseOriLen";
            this.ckUseOriLen.Size = new System.Drawing.Size(117, 17);
            this.ckUseOriLen.TabIndex = 34;
            this.ckUseOriLen.Text = "Use Orig. Line Len.";
            this.ckUseOriLen.UseVisualStyleBackColor = true;
            // 
            // Client
            // 
            this.Client.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Client.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Client.FormattingEnabled = true;
            this.Client.Items.AddRange(new object[] {
            "Google",
            "Bing Neural",
            "Bing Statical",
            "LEC"});
            this.Client.Location = new System.Drawing.Point(328, 414);
            this.Client.Name = "Client";
            this.Client.Size = new System.Drawing.Size(82, 21);
            this.Client.TabIndex = 35;
            this.Client.TextChanged += new System.EventHandler(this.ClientChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 447);
            this.Controls.Add(this.Client);
            this.Controls.Add(this.ckUseOriLen);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.MaxPerLine);
            this.Controls.Add(this.ImportTextBnt);
            this.Controls.Add(this.ExportTextBnt);
            this.Controls.Add(this.ckDoubleStep);
            this.Controls.Add(this.OverwriteBnt);
            this.Controls.Add(this.LoadDB);
            this.Controls.Add(this.SaveDB);
            this.Controls.Add(this.FoldTrans);
            this.Controls.Add(this.MassSelect);
            this.Controls.Add(this.SearchTB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.End);
            this.Controls.Add(this.Begin);
            this.Controls.Add(this.MassMode);
            this.Controls.Add(this.LblInfo);
            this.Controls.Add(this.BntBathProc);
            this.Controls.Add(this.BntBathFind);
            this.Controls.Add(this.BotSelect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BntSave);
            this.Controls.Add(this.BntOpen);
            this.Controls.Add(this.BntProc);
            this.Controls.Add(this.OutLang);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InputLang);
            this.Controls.Add(this.StringList);
            this.MinimumSize = new System.Drawing.Size(715, 486);
            this.Name = "Form1";
            this.Text = "TLBOT - In Game Machine Transation";
            this.Shown += new System.EventHandler(this.ProgramOpen);
            ((System.ComponentModel.ISupportInitialize)(this.Begin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.End)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxPerLine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox StringList;
        private System.Windows.Forms.ComboBox InputLang;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox OutLang;
        private System.Windows.Forms.Button BntProc;
        private System.Windows.Forms.Button BntOpen;
        private System.Windows.Forms.Button BntSave;
        private System.Windows.Forms.OpenFileDialog OpenBinary;
        private System.Windows.Forms.SaveFileDialog SaveBinary;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BotSelect;
        private System.Windows.Forms.Button BntBathFind;
        private System.Windows.Forms.Button BntBathProc;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.CheckBox MassMode;
        private System.Windows.Forms.NumericUpDown Begin;
        private System.Windows.Forms.NumericUpDown End;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox SearchTB;
        private System.Windows.Forms.Button MassSelect;
        private System.Windows.Forms.Button FoldTrans;
        private System.Windows.Forms.Button SaveDB;
        private System.Windows.Forms.Button LoadDB;
        private System.Windows.Forms.Button OverwriteBnt;
        private System.Windows.Forms.CheckBox ckDoubleStep;
        private System.Windows.Forms.Button ExportTextBnt;
        private System.Windows.Forms.Button ImportTextBnt;
        private System.Windows.Forms.NumericUpDown MaxPerLine;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ckUseOriLen;
        private System.Windows.Forms.ComboBox Client;
    }
}

