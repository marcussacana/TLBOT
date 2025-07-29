using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static TLBOT.OllamaTranslationTask;

namespace TLBOT.Items
{
    internal class AffiliationDisplay : FlowLayoutPanel
    {
        static Random rnd = new Random();

        private TextBox OriginalName;
        private TextBox LocalizedName;
        private List<Affiliation> affiliations;
        private Affiliation This;
        public int FieldCount { get; private set; }

        public AffiliationDisplay(ref List<Affiliation> Affiliations, Affiliation This)
        {
            this.AutoSize = true;
            this.WrapContents = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.VerticalScroll.Enabled = true;
            this.FlowDirection = FlowDirection.TopDown;
            this.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.affiliations = Affiliations;
            this.This = This;
            BuildController();


            SizeChanged += (s, e) =>
            {
                resize();
            };

            BackColor = Color.FromArgb(rnd.Next(90, 150), rnd.Next(90, 150), rnd.Next(90, 150));
        }

        private void resize()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Width = ClientSize.Width - ctrl.Margin.Horizontal;
            }
        }

        public List<Affiliation> GetList()
        {
            return affiliations;
        }

        private void BuildController()
        {
            OriginalName = new TextBox
            {
                AutoSize = false,
                Text = This.OriginalName
            };

            LocalizedName = new TextBox
            {
                AutoSize = false,
                Text = This.LocalizedName
            };

            OriginalName.TextChanged += onOriginalNameChanged;
            LocalizedName.TextChanged += onLocalizedNameChanged;

            OriginalName.Text = This.OriginalName;
            LocalizedName.Text = This.LocalizedName;

            Controls.Add(OriginalName);
            Controls.Add(LocalizedName);

            FieldCount += 2;

            if (This.Aliases != null) 
            {
                for (int i = 0; i < This.Aliases.Length; i++)
                {
                    var aliasTextBox = new TextBox
                    {
                        AutoSize = false,
                        Text = This.Aliases[i],
                    };
                    aliasTextBox.Name = i.ToString();
                    aliasTextBox.TextChanged += onAliasChanged;

                    Controls.Add(aliasTextBox);
                    FieldCount++;
                }
            }

            resize();
        }

        private void onOriginalNameChanged(object sender, EventArgs e)
        {
            int index = GetAffiliationIndexByName(This.OriginalName);
            if (index != -1)
            {
                This.OriginalName = OriginalName.Text;
                affiliations[index] = This;
                return;
            }

            showError();
        }
        private void onLocalizedNameChanged(object sender, EventArgs e)
        {
            This.LocalizedName = LocalizedName.Text;
            updateAffiliation();
        }

        private void onAliasChanged(object sender, EventArgs e)
        {
            var tb = (sender as TextBox);

            This.Aliases[int.Parse(tb.Name)] = tb.Text;
            updateAffiliation();
        }

        private void updateAffiliation()
        {
            var index = GetAffiliationIndexByName(This.OriginalName);

            if (index == -1)
            {
                showError();
                return;
            }

            affiliations[index] = This;
        }

        private int GetAffiliationIndexByName(string OriginalName)
        {
            var indexedList = affiliations.Select((item, index) => (item, index));
            if (indexedList.Any(x => x.item.OriginalName == This.OriginalName))
            {
                return indexedList.First(x => x.item.OriginalName == This.OriginalName).index;
            }
            return -1;
        }
        void showError()
        {
            MessageBox.Show("Failed To update the field", "TLBOT", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
