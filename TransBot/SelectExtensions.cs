using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TLBOT {
    public partial class SelectExtensions : Form {

        public SelectExtensions() {
            InitializeComponent();
        }

        public string Default {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }
        public string[] Extensions = null;

        
        private delegate void Caller();
        private void Ok_Click(object sender, EventArgs e) {
            if (textBox1.Text.Contains(".")) {
                MessageBox.Show("Invalid Input, see the sample.", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            string[] exts = textBox1.Text.Split(';', ',', '|');
            for (int i = 0; i < exts.Length; i++)
                exts[i] = "." + exts[i].Trim().ToLower();
            Extensions = exts;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
