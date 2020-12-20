//#IMPORT System.Linq.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INI {
    public class Plain {
        string[] Script;
        Encoding Eco = Encoding.UTF8;
        bool BOOM = false;
        public Plain(byte[] Script) {
            if (Script[0] == 0xFF && Script[1] == 0xFE) {
                BOOM = true;
                byte[] narr = new byte[Script.Length - 2];
                for (int i = 2; i < Script.Length; i++)
                    narr[i - 2] = Script[i];
                this.Script = Eco.GetString(narr).Replace("\r\n", "\n").Split('\n');
                return;
            }
            this.Script = Eco.GetString(Script).Replace("\r\n", "\n").Split('\n');
        }

        public string[] Import() {
            List<string> Lines = new List<string>();
            for (uint i = 0; i < Script.Length; i += 1) {
                string Line = Script[i];
                if (!IsStr(Line))
                    continue;

                Lines.Add(GetLine(Line));
            }
            return Lines.ToArray();
        }

        public string GetLine(string Line) {
            return string.Join("=", Line.Split('=').Skip(1).ToArray());
        }

        public int StrLen(string Line) {
            return GetLine(Line).Length;
        }

        public bool IsStr(string Line) {
            return Line.ToLower().Contains("=");
        }

        public byte[] Export(string[] Text) {
            StringBuilder Compiler = new StringBuilder();
            for (int i = 0, t = 0; i < Script.Length; i++) {
                string Line = Script[i];
                if (!IsStr(Line)) {
                    Compiler.AppendLine(Line);
                    continue;
                }
                int Len = StrLen(Line);
                if (Len == 0) {
                    Compiler.AppendLine(Line);
                    continue;
                }
                string Begin = Line.Split('=')[0] + "=";
                Compiler.AppendLine(Begin + Text[t++]);
            }

            byte[] barr = Eco.GetBytes(Compiler.ToString());
            if (BOOM) {
                byte[] Out = new byte[barr.Length + 2];
                Out[0] = 0xFF;
                Out[1] = 0xFE;
                barr.CopyTo(Out, 0);
                barr = Out;
            }
            return barr;
        }

    }
}
