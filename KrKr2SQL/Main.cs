using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KrKr2SQL
{
    public class SQLOpen
    {
        public Action ProgressChange;
        public string ExportProgress { get; private set; }
        public bool DoEvents = true;
        string[] Text;
        SQLiteConnection SQL;

        public SQLOpen(string DB) {
            SQL =  new SQLiteConnection(string.Format("Data Source='{0}';Version=3;", DB));
            SQL.Open();
        }

        public string[] Import() {
            const string Command = "select text from text order by idx asc";
            SQLiteCommand command = new SQLiteCommand(Command, SQL);
            SQLiteDataReader Reader = command.ExecuteReader();
            List<string> Strings = new List<string>();
            while (Reader.Read())
                Strings.Add(Reader["text"].ToString());
            Text = Strings.ToArray();
            return Strings.ToArray();
        }

        public void Export(string[] Lines) {
            if (Lines.Length != Text.Length)
                throw new Exception("You cannot add or delete string entries...");

            const string BASE = "update text set text=:New where text=:Ori";
            for (int i = 0; i < Lines.Length; i++) {
                ExportProgress = string.Format("{0}/{1} - {2}%", i, Lines.Length, (int)(((double)i / Lines.Length) * 100));
                if (DoEvents)
                    Application.DoEvents();
                if (Lines[i] == Text[i])
                    continue;
                ProgressChange?.Invoke();
                using (SQLiteCommand command = new SQLiteCommand(BASE, SQL)) {
                    command.Parameters.Add("New", System.Data.DbType.String).Value = Lines[i];
                    command.Parameters.Add("Ori", System.Data.DbType.String).Value = Text[i];
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Close() {
            SQL?.Close();
        }
    }
}
