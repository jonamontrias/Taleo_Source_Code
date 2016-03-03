using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TaleoAutomation
{
    class Property
    {
        public Dictionary<string, List<Action>> PageList = new Dictionary<string, List<Action>>();
        string file = Properties.Resources.main2;

        public Property()
        {
            convertFile();
        }

        private void convertFile()
        {
            //try
            //{
                if (!string.IsNullOrWhiteSpace(file))
                { SplitLine(file); }
            //}
            //catch (Exception)
            //{
            //    System.Windows.MessageBox.Show("The File is Empty");
            //}
        }

        private void SplitLine(string file)
        {
            string key = "";
            string[] enter = {"\r\n"};
            string[] line = file.Split(enter, StringSplitOptions.None);
            List<Action> list = new List<Action>();
            foreach (string L in line)
            {
                if (L.Contains("//"))
                {
                    key = getName(L);
                    list = new List<Action>();
                    PageList[key] = list;
                }
                else if (!string.IsNullOrWhiteSpace(L))
                {
                    string[] code = remBar(L);
                    list.Add(new Action(code[0], code[1], code[2], code[3]));
                }
            }
        }

        private string[] remBar(string L)
        {
            string mod_line = L.Replace(" | ", "|");
            string[] code = mod_line.Split('|');

            return code;
        }

        private string getName(string L)
        {
            string mod_name = L.Replace("//", "");
            return mod_name;
        }
    }
}
