using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static TLBOT.OllamaTranslationTask;

namespace TLBOT.Items
{
    internal class CharacterDisplay : FlowLayoutPanel
    {

        static Random rnd = new Random();

        private TextBox OriginalName;
        private TextBox LocalizedName;
        private ComboBox GenderCombo;
        private TextBox SpecieBox;
        private ComboBox RoleCombo;
        private List<TextBox> AffiliationBoxes = new List<TextBox>();

        private List<Character> characters;
        private Character This;

        public int FieldCount { get; private set; }
        public CharacterDisplay(ref List<Character> Characters, Character This)
        {
            this.AutoSize = true;
            this.WrapContents = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.VerticalScroll.Enabled = true;
            this.FlowDirection = FlowDirection.TopDown;
            this.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.characters = Characters;
            this.This = This;
            BuildController();

            SizeChanged += (s, e) =>
            {
                resize();
            };

            BackColor = Color.FromArgb(rnd.Next(150, 255), rnd.Next(150, 255), rnd.Next(150, 255));
        }

        public List<Character> GetList()
        {
            return characters;
        }
        private void resize()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Width = ClientSize.Width - ctrl.Margin.Horizontal;
            }
        }

        private void BuildController()
        {
            // Nome original e localizado
            OriginalName = new TextBox { AutoSize = false, Text = This.OriginalName };
            LocalizedName = new TextBox { AutoSize = false, Text = This.LocalizedName };
            OriginalName.TextChanged += onOriginalNameChanged;
            LocalizedName.TextChanged += onLocalizedNameChanged;

            this.Controls.Add(OriginalName);
            this.Controls.Add(LocalizedName);

            FieldCount += 2;

            // Aliases
            if (This.Aliases != null)
            {
                for (int i = 0; i < This.Aliases.Length; i++)
                {
                    var aliasTextBox = new TextBox
                    {
                        AutoSize = false,
                        Text = This.Aliases[i],
                        Name = $"Alias_{i}"
                    };
                    aliasTextBox.TextChanged += onAliasChanged;
                    this.Controls.Add(aliasTextBox);
                    FieldCount++;
                }
            }

            // Gender ComboBox
            GenderCombo = new ComboBox { AutoSize = false, DropDownStyle = ComboBoxStyle.DropDownList };
            GenderCombo.Items.AddRange(Enum.GetNames(typeof(Gender)));
            GenderCombo.SelectedItem = This.Gender.ToString();
            GenderCombo.SelectedIndexChanged += onGenderChanged;
            this.Controls.Add(GenderCombo);
            FieldCount++;

            // Specie TextBox
            if (!string.IsNullOrWhiteSpace(This.Specie)) { 
                SpecieBox = new TextBox { AutoSize = false, Text = This.Specie };
                SpecieBox.TextChanged += onSpecieChanged;
                this.Controls.Add(SpecieBox);
                FieldCount++;
            }

            // Role ComboBox
            RoleCombo = new ComboBox { AutoSize = false, DropDownStyle = ComboBoxStyle.DropDownList };
            RoleCombo.Items.AddRange(Enum.GetNames(typeof(Role)));
            RoleCombo.SelectedItem = This.PlotRole.ToString();
            RoleCombo.SelectedIndexChanged += onRoleChanged;
            this.Controls.Add(RoleCombo);
            FieldCount++;

            // Affiliations
            if (This.Affiliations != null)
            {
                for (int i = 0; i < This.Affiliations.Length; i++)
                {
                    var affBox = new TextBox
                    {
                        AutoSize = false,
                        Text = This.Affiliations[i],
                        Name = $"Aff_{i}"
                    };
                    affBox.TextChanged += onAffiliationChanged;
                    AffiliationBoxes.Add(affBox);
                    this.Controls.Add(affBox);
                    FieldCount++;
                }
            }

            resize();
        }

        private void onGenderChanged(object sender, EventArgs e)
        {
            if (Enum.TryParse(GenderCombo.SelectedItem.ToString(), out Gender selected))
            {
                This.Gender = selected;
                update();
            }
        }

        private void onSpecieChanged(object sender, EventArgs e)
        {
            This.Specie = SpecieBox.Text;
            update();
        }

        private void onRoleChanged(object sender, EventArgs e)
        {
            if (Enum.TryParse(RoleCombo.SelectedItem.ToString(), out Role selected))
            {
                This.PlotRole = selected;
                update();
            }
        }

        private void onAffiliationChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < AffiliationBoxes.Count; i++)
            {
                This.Affiliations[i] = AffiliationBoxes[i].Text;
            }
            update();
        }

        private void onOriginalNameChanged(object sender, EventArgs e)
        {
            int index = GetIndexByName(This.OriginalName);
            if (index != -1)
            {
                This.OriginalName = OriginalName.Text;
                characters[index] = This;
                return;
            }

            showError();
        }
        private void onLocalizedNameChanged(object sender, EventArgs e)
        {
            This.LocalizedName = LocalizedName.Text;
            update();
        }

        private void onAliasChanged(object sender, EventArgs e)
        {
            var tb = (sender as TextBox);

            This.Aliases[int.Parse(tb.Name)] = tb.Text;
            update();
        }

        private void update()
        {
            var index = GetIndexByName(This.OriginalName);

            if (index == -1)
            {
                showError();
                return;
            }

            characters[index] = This;
        }

        private int GetIndexByName(string OriginalName)
        {
            var indexedList = characters.Select((item, index) => (item, index));
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
