using SacanaWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TLBOT.DataManager;

namespace TLBOT {
    public partial class Search : Form {
        public Search() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            OpenFile.ShowDialog();
        }

        private void OpenFile_FileOk(object sender, CancelEventArgs e) {
            Wrapper Wrapper = new Wrapper();
            MatchList.Items.Clear();
            string Content = tbContent.Text;
            if (ckUnescape.Checked)
                Content = Content.Unescape();
            if (ckBetterSensitivy.Checked)
                Content = Minify(Content);
            Text = "Searching...";
            foreach (string File in OpenFile.FileNames) {
                try {
                    string[] Lines = Wrapper.Import(File);
                    bool Match = false;
                    foreach (string Line in Lines) {
                        if (ckBetterSensitivy.Checked) {
                            if (Minify(Line).Contains(Content)) {
                                Match = true;
                                break;
                            }
                        } else {
                            if (Line.Contains(Content)) {
                                Match = true;
                                break;
                            }
                        }

                    }
                    if (Match) {
                        MatchList.Items.Add(System.IO.Path.GetFileName(File));
                        if (!ckSearchAll.Checked)
                            break;
                    }
                } catch { }
                Application.DoEvents();
            }
            Text = "Search";
        }

        private string Minify(string content) {
            return content.ToLower().Replace(" ", "").Trim();
        }
    }
}
