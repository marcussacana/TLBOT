namespace TLBOT {
    partial class FilesSelector {
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
            this.FileList = new System.Windows.Forms.CheckedListBox();
            this.bntAddFolder = new System.Windows.Forms.Button();
            this.bntAddFiles = new System.Windows.Forms.Button();
            this.bntCheckAll = new System.Windows.Forms.Button();
            this.bntUncheckAll = new System.Windows.Forms.Button();
            this.bntOkay = new System.Windows.Forms.Button();
            this.bntAbort = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FileList
            // 
            this.FileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileList.FormattingEnabled = true;
            this.FileList.Location = new System.Drawing.Point(12, 12);
            this.FileList.Name = "FileList";
            this.FileList.Size = new System.Drawing.Size(463, 289);
            this.FileList.TabIndex = 0;
            // 
            // bntAddFolder
            // 
            this.bntAddFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bntAddFolder.Location = new System.Drawing.Point(12, 306);
            this.bntAddFolder.Name = "bntAddFolder";
            this.bntAddFolder.Size = new System.Drawing.Size(104, 23);
            this.bntAddFolder.TabIndex = 1;
            this.bntAddFolder.Text = "Adicionar Pasta";
            this.bntAddFolder.UseVisualStyleBackColor = true;
            this.bntAddFolder.Click += new System.EventHandler(this.bntAddFolder_Click);
            // 
            // bntAddFiles
            // 
            this.bntAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bntAddFiles.Location = new System.Drawing.Point(122, 306);
            this.bntAddFiles.Name = "bntAddFiles";
            this.bntAddFiles.Size = new System.Drawing.Size(104, 23);
            this.bntAddFiles.TabIndex = 2;
            this.bntAddFiles.Text = "Adicionar Arquivos";
            this.bntAddFiles.UseVisualStyleBackColor = true;
            this.bntAddFiles.Click += new System.EventHandler(this.bntAddFiles_Click);
            // 
            // bntCheckAll
            // 
            this.bntCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntCheckAll.Location = new System.Drawing.Point(249, 306);
            this.bntCheckAll.Name = "bntCheckAll";
            this.bntCheckAll.Size = new System.Drawing.Size(110, 23);
            this.bntCheckAll.TabIndex = 3;
            this.bntCheckAll.Text = "Selecionar Tudo";
            this.bntCheckAll.UseVisualStyleBackColor = true;
            this.bntCheckAll.Click += new System.EventHandler(this.bntCheckAll_Click);
            // 
            // bntUncheckAll
            // 
            this.bntUncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntUncheckAll.Location = new System.Drawing.Point(365, 306);
            this.bntUncheckAll.Name = "bntUncheckAll";
            this.bntUncheckAll.Size = new System.Drawing.Size(110, 23);
            this.bntUncheckAll.TabIndex = 4;
            this.bntUncheckAll.Text = "Desselecionar Tudo";
            this.bntUncheckAll.UseVisualStyleBackColor = true;
            this.bntUncheckAll.Click += new System.EventHandler(this.bntUncheckAll_Click);
            // 
            // bntOkay
            // 
            this.bntOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bntOkay.Location = new System.Drawing.Point(12, 335);
            this.bntOkay.Name = "bntOkay";
            this.bntOkay.Size = new System.Drawing.Size(214, 23);
            this.bntOkay.TabIndex = 5;
            this.bntOkay.Text = "Pronto";
            this.bntOkay.UseVisualStyleBackColor = true;
            this.bntOkay.Click += new System.EventHandler(this.bntOkay_Click);
            // 
            // bntAbort
            // 
            this.bntAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntAbort.Location = new System.Drawing.Point(249, 335);
            this.bntAbort.Name = "bntAbort";
            this.bntAbort.Size = new System.Drawing.Size(226, 23);
            this.bntAbort.TabIndex = 6;
            this.bntAbort.Text = "Cancelar";
            this.bntAbort.UseVisualStyleBackColor = true;
            this.bntAbort.Click += new System.EventHandler(this.bntAbort_Click);
            // 
            // FilesSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 370);
            this.Controls.Add(this.bntAbort);
            this.Controls.Add(this.bntOkay);
            this.Controls.Add(this.bntUncheckAll);
            this.Controls.Add(this.bntCheckAll);
            this.Controls.Add(this.bntAddFiles);
            this.Controls.Add(this.bntAddFolder);
            this.Controls.Add(this.FileList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FilesSelector";
            this.Text = "Selecione os Arquivos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingSelector);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox FileList;
        private System.Windows.Forms.Button bntAddFolder;
        private System.Windows.Forms.Button bntAddFiles;
        private System.Windows.Forms.Button bntCheckAll;
        private System.Windows.Forms.Button bntUncheckAll;
        private System.Windows.Forms.Button bntOkay;
        private System.Windows.Forms.Button bntAbort;
    }
}