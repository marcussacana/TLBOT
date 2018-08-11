using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TLBOT.DataManager;

namespace TLBOT {
    public partial class FilesSelector : Form {
        public string[] SelectedFiles;
        public string Filter = "*.*";
        public FilesSelector() {
            InitializeComponent();

            if (Program.Settings.TranslateWindow)
                new Thread(() => this.Translate(Program.Settings.TargetLang, Program.TLClient)).Start();

            DialogResult = DialogResult.None;
        }

        private void bntAddFiles_Click(object sender, EventArgs e) {
            var FileDialog = new CommonOpenFileDialog() {
                Multiselect = true,
                IsFolderPicker = false,
                EnsurePathExists = true,
                InitialDirectory = Program.Settings.LastSelectedPath
            };
            FileDialog.Multiselect = true;

            if (FileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            Program.Settings.LastSelectedPath = Path.GetDirectoryName(FileDialog.FileNames.First());

            foreach (string FileName in FileDialog.FileNames)
                FileList.Items.Add(FileName, true);
        }

        private void bntAddFolder_Click(object sender, EventArgs e) {
            var FileDialog = new CommonOpenFileDialog() {
                Multiselect = true,
                IsFolderPicker = true,
                EnsurePathExists = true,
                InitialDirectory = Program.Settings.LastSelectedPath
            };
            FileDialog.Multiselect = true;

            if (FileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            Program.Settings.LastSelectedPath = Path.GetDirectoryName(FileDialog.FileNames.First());

            foreach (string DirectoryName in FileDialog.FileNames) {
                string[] Files = Directory.GetFiles(DirectoryName, Filter, SearchOption.AllDirectories);
                foreach (string File in Files)
                    FileList.Items.Add(File, true);
            }
        }

        private void bntCheckAll_Click(object sender, EventArgs e) {
            for (int i = 0; i < FileList.Items.Count; i++)
                FileList.SetItemChecked(i, true);
        }

        private void bntUncheckAll_Click(object sender, EventArgs e) {
            for (int i = 0; i < FileList.Items.Count; i++)
                FileList.SetItemChecked(i, false);
        }

        private void bntAbort_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void bntOkay_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ClosingSelector(object sender, FormClosingEventArgs e) {
            if (DialogResult == DialogResult.None)
                DialogResult = DialogResult.Cancel;
            else
                SelectedFiles = FileList.CheckedItems.OfType<string>().ToArray();            
        }
    }
}
