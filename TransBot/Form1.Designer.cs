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
            this.Port = new System.Windows.Forms.MaskedTextBox();
            this.BotSelect = new System.Windows.Forms.Button();
            this.BntBathFind = new System.Windows.Forms.Button();
            this.BntBathProc = new System.Windows.Forms.Button();
            this.LblInfo = new System.Windows.Forms.Label();
            this.CkOffline = new System.Windows.Forms.CheckBox();
            this.Shutdown = new System.Windows.Forms.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.Begin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.End)).BeginInit();
            this.SuspendLayout();
            // 
            // StringList
            // 
            this.StringList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StringList.FormattingEnabled = true;
            this.StringList.Location = new System.Drawing.Point(18, 19);
            this.StringList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StringList.Name = "StringList";
            this.StringList.Size = new System.Drawing.Size(1045, 466);
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
            this.InputLang.Location = new System.Drawing.Point(252, 626);
            this.InputLang.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InputLang.Name = "InputLang";
            this.InputLang.Size = new System.Drawing.Size(58, 28);
            this.InputLang.TabIndex = 1;
            this.InputLang.Text = "JA";
            this.InputLang.TextChanged += new System.EventHandler(this.VerifyLang);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 631);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Translate selected values from:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 631);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 20);
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
            this.OutLang.Location = new System.Drawing.Point(364, 626);
            this.OutLang.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutLang.Name = "OutLang";
            this.OutLang.Size = new System.Drawing.Size(58, 28);
            this.OutLang.TabIndex = 4;
            this.OutLang.Text = "EN";
            this.OutLang.TextChanged += new System.EventHandler(this.VerifyLang);
            // 
            // BntProc
            // 
            this.BntProc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BntProc.Location = new System.Drawing.Point(603, 625);
            this.BntProc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BntProc.Name = "BntProc";
            this.BntProc.Size = new System.Drawing.Size(112, 35);
            this.BntProc.TabIndex = 5;
            this.BntProc.Text = "Translate!";
            this.BntProc.UseVisualStyleBackColor = true;
            this.BntProc.Click += new System.EventHandler(this.BntProc_Click);
            // 
            // BntOpen
            // 
            this.BntOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BntOpen.Location = new System.Drawing.Point(844, 625);
            this.BntOpen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BntOpen.Name = "BntOpen";
            this.BntOpen.Size = new System.Drawing.Size(104, 35);
            this.BntOpen.TabIndex = 6;
            this.BntOpen.Text = "Open";
            this.BntOpen.UseVisualStyleBackColor = true;
            this.BntOpen.Click += new System.EventHandler(this.BntOpen_Click);
            // 
            // BntSave
            // 
            this.BntSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BntSave.Location = new System.Drawing.Point(956, 625);
            this.BntSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BntSave.Name = "BntSave";
            this.BntSave.Size = new System.Drawing.Size(104, 35);
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
            this.label3.Location = new System.Drawing.Point(434, 631);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "With Port: ";
            // 
            // Port
            // 
            this.Port.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Port.Location = new System.Drawing.Point(526, 628);
            this.Port.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Port.Mask = "00000";
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(67, 26);
            this.Port.TabIndex = 10;
            this.Port.Text = "29511";
            // 
            // BotSelect
            // 
            this.BotSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BotSelect.Location = new System.Drawing.Point(143, 535);
            this.BotSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BotSelect.Name = "BotSelect";
            this.BotSelect.Size = new System.Drawing.Size(141, 35);
            this.BotSelect.TabIndex = 11;
            this.BotSelect.Text = "Text Recog.";
            this.BotSelect.UseVisualStyleBackColor = true;
            this.BotSelect.Click += new System.EventHandler(this.BotSelect_Click);
            // 
            // BntBathFind
            // 
            this.BntBathFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BntBathFind.Location = new System.Drawing.Point(17, 538);
            this.BntBathFind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BntBathFind.Name = "BntBathFind";
            this.BntBathFind.Size = new System.Drawing.Size(118, 35);
            this.BntBathFind.TabIndex = 12;
            this.BntBathFind.Text = "Batch Find";
            this.BntBathFind.UseVisualStyleBackColor = true;
            this.BntBathFind.Click += new System.EventHandler(this.button1_Click);
            // 
            // BntBathProc
            // 
            this.BntBathProc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BntBathProc.Location = new System.Drawing.Point(18, 495);
            this.BntBathProc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BntBathProc.Name = "BntBathProc";
            this.BntBathProc.Size = new System.Drawing.Size(183, 35);
            this.BntBathProc.TabIndex = 13;
            this.BntBathProc.Text = "Batch Trans.";
            this.BntBathProc.UseVisualStyleBackColor = true;
            this.BntBathProc.Click += new System.EventHandler(this.BntBathProc_Click);
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LblInfo.Location = new System.Drawing.Point(690, 542);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LblInfo.Size = new System.Drawing.Size(374, 30);
            this.LblInfo.TabIndex = 14;
            this.LblInfo.Text = "TLBOT by Marcussacana - {0} Build";
            this.LblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CkOffline
            // 
            this.CkOffline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CkOffline.AutoSize = true;
            this.CkOffline.Location = new System.Drawing.Point(376, 543);
            this.CkOffline.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CkOffline.Name = "CkOffline";
            this.CkOffline.Size = new System.Drawing.Size(103, 24);
            this.CkOffline.TabIndex = 15;
            this.CkOffline.Text = "Offline TL";
            this.CkOffline.UseVisualStyleBackColor = true;
            // 
            // Shutdown
            // 
            this.Shutdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Shutdown.AutoSize = true;
            this.Shutdown.Location = new System.Drawing.Point(377, 503);
            this.Shutdown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Shutdown.Name = "Shutdown";
            this.Shutdown.Size = new System.Drawing.Size(145, 24);
            this.Shutdown.TabIndex = 16;
            this.Shutdown.Text = "Auto Shutdown";
            this.Shutdown.UseVisualStyleBackColor = true;
            // 
            // Begin
            // 
            this.Begin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Begin.Increment = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.Begin.Location = new System.Drawing.Point(237, 588);
            this.Begin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Begin.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Begin.Name = "Begin";
            this.Begin.Size = new System.Drawing.Size(127, 26);
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
            this.End.Location = new System.Drawing.Point(404, 588);
            this.End.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.End.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(114, 26);
            this.End.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 591);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(211, 20);
            this.label4.TabIndex = 19;
            this.label4.Text = "Process Only the string from ";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(372, 591);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "At";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(556, 590);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 20);
            this.label6.TabIndex = 21;
            this.label6.Text = "Line:";
            // 
            // SearchTB
            // 
            this.SearchTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchTB.Location = new System.Drawing.Point(603, 588);
            this.SearchTB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SearchTB.Name = "SearchTB";
            this.SearchTB.Size = new System.Drawing.Size(322, 26);
            this.SearchTB.TabIndex = 22;
            this.SearchTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchKeyPress);
            // 
            // MassSelect
            // 
            this.MassSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MassSelect.Location = new System.Drawing.Point(292, 535);
            this.MassSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MassSelect.Name = "MassSelect";
            this.MassSelect.Size = new System.Drawing.Size(78, 35);
            this.MassSelect.TabIndex = 23;
            this.MassSelect.Text = "Sel. All";
            this.MassSelect.UseVisualStyleBackColor = true;
            this.MassSelect.Click += new System.EventHandler(this.MassSelect_Click);
            // 
            // FoldTrans
            // 
            this.FoldTrans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FoldTrans.Location = new System.Drawing.Point(208, 495);
            this.FoldTrans.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FoldTrans.Name = "FoldTrans";
            this.FoldTrans.Size = new System.Drawing.Size(162, 35);
            this.FoldTrans.TabIndex = 24;
            this.FoldTrans.Text = "Folder Trans.";
            this.FoldTrans.UseVisualStyleBackColor = true;
            this.FoldTrans.Click += new System.EventHandler(this.FoldTrans_Click);
            // 
            // SaveDB
            // 
            this.SaveDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveDB.Location = new System.Drawing.Point(524, 499);
            this.SaveDB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SaveDB.Name = "SaveDB";
            this.SaveDB.Size = new System.Drawing.Size(111, 29);
            this.SaveDB.TabIndex = 25;
            this.SaveDB.Text = "Save TL DB";
            this.SaveDB.UseVisualStyleBackColor = true;
            this.SaveDB.Click += new System.EventHandler(this.SaveDB_Click);
            // 
            // LoadDB
            // 
            this.LoadDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LoadDB.Location = new System.Drawing.Point(642, 499);
            this.LoadDB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoadDB.Name = "LoadDB";
            this.LoadDB.Size = new System.Drawing.Size(111, 29);
            this.LoadDB.TabIndex = 26;
            this.LoadDB.Text = "Load TL DB";
            this.LoadDB.UseVisualStyleBackColor = true;
            this.LoadDB.Click += new System.EventHandler(this.LoadDB_Click);
            // 
            // OverwriteBnt
            // 
            this.OverwriteBnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OverwriteBnt.Location = new System.Drawing.Point(933, 588);
            this.OverwriteBnt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OverwriteBnt.Name = "OverwriteBnt";
            this.OverwriteBnt.Size = new System.Drawing.Size(127, 29);
            this.OverwriteBnt.TabIndex = 27;
            this.OverwriteBnt.Text = "Overwrite Line";
            this.OverwriteBnt.UseVisualStyleBackColor = true;
            this.OverwriteBnt.Click += new System.EventHandler(this.OverwriteBnt_Click);
            // 
            // ckDoubleStep
            // 
            this.ckDoubleStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckDoubleStep.AutoSize = true;
            this.ckDoubleStep.Location = new System.Drawing.Point(482, 543);
            this.ckDoubleStep.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckDoubleStep.Name = "ckDoubleStep";
            this.ckDoubleStep.Size = new System.Drawing.Size(113, 24);
            this.ckDoubleStep.TabIndex = 28;
            this.ckDoubleStep.Text = "2-Steps TL";
            this.ckDoubleStep.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 686);
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
            this.Controls.Add(this.Shutdown);
            this.Controls.Add(this.CkOffline);
            this.Controls.Add(this.LblInfo);
            this.Controls.Add(this.BntBathProc);
            this.Controls.Add(this.BntBathFind);
            this.Controls.Add(this.BotSelect);
            this.Controls.Add(this.Port);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BntSave);
            this.Controls.Add(this.BntOpen);
            this.Controls.Add(this.BntProc);
            this.Controls.Add(this.OutLang);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InputLang);
            this.Controls.Add(this.StringList);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1092, 731);
            this.Name = "Form1";
            this.Text = "TLBOT - In Game Machine Transation";
            this.Shown += new System.EventHandler(this.ProgramOpen);
            ((System.ComponentModel.ISupportInitialize)(this.Begin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.End)).EndInit();
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
        private System.Windows.Forms.MaskedTextBox Port;
        private System.Windows.Forms.Button BotSelect;
        private System.Windows.Forms.Button BntBathFind;
        private System.Windows.Forms.Button BntBathProc;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.CheckBox CkOffline;
        private System.Windows.Forms.CheckBox Shutdown;
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
    }
}

