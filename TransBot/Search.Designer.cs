namespace TLBOT {
    partial class Search {
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
            this.tbContent = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.MatchList = new System.Windows.Forms.ListBox();
            this.ckUnescape = new System.Windows.Forms.CheckBox();
            this.ckSearchAll = new System.Windows.Forms.CheckBox();
            this.ckBetterSensitivy = new System.Windows.Forms.CheckBox();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // tbContent
            // 
            this.tbContent.Location = new System.Drawing.Point(12, 12);
            this.tbContent.Name = "tbContent";
            this.tbContent.Size = new System.Drawing.Size(473, 20);
            this.tbContent.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(491, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MatchList
            // 
            this.MatchList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MatchList.FormattingEnabled = true;
            this.MatchList.Location = new System.Drawing.Point(117, 39);
            this.MatchList.Name = "MatchList";
            this.MatchList.Size = new System.Drawing.Size(449, 95);
            this.MatchList.TabIndex = 2;
            // 
            // ckUnescape
            // 
            this.ckUnescape.AutoSize = true;
            this.ckUnescape.Location = new System.Drawing.Point(12, 39);
            this.ckUnescape.Name = "ckUnescape";
            this.ckUnescape.Size = new System.Drawing.Size(75, 17);
            this.ckUnescape.TabIndex = 3;
            this.ckUnescape.Text = "Unescape";
            this.ckUnescape.UseVisualStyleBackColor = true;
            // 
            // ckSearchAll
            // 
            this.ckSearchAll.AutoSize = true;
            this.ckSearchAll.Location = new System.Drawing.Point(12, 85);
            this.ckSearchAll.Name = "ckSearchAll";
            this.ckSearchAll.Size = new System.Drawing.Size(74, 17);
            this.ckSearchAll.TabIndex = 4;
            this.ckSearchAll.Text = "Search All";
            this.ckSearchAll.UseVisualStyleBackColor = true;
            // 
            // ckBetterSensitivy
            // 
            this.ckBetterSensitivy.AutoSize = true;
            this.ckBetterSensitivy.Location = new System.Drawing.Point(12, 62);
            this.ckBetterSensitivy.Name = "ckBetterSensitivy";
            this.ckBetterSensitivy.Size = new System.Drawing.Size(99, 17);
            this.ckBetterSensitivy.TabIndex = 5;
            this.ckBetterSensitivy.Text = "Better Sensitivy";
            this.ckBetterSensitivy.UseVisualStyleBackColor = true;
            // 
            // OpenFile
            // 
            this.OpenFile.Filter = "All Files|*.*";
            this.OpenFile.Multiselect = true;
            this.OpenFile.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFile_FileOk);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 138);
            this.Controls.Add(this.ckBetterSensitivy);
            this.Controls.Add(this.ckSearchAll);
            this.Controls.Add(this.ckUnescape);
            this.Controls.Add(this.MatchList);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Search";
            this.Text = "Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbContent;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox MatchList;
        private System.Windows.Forms.CheckBox ckUnescape;
        private System.Windows.Forms.CheckBox ckSearchAll;
        private System.Windows.Forms.CheckBox ckBetterSensitivy;
        private System.Windows.Forms.OpenFileDialog OpenFile;
    }
}