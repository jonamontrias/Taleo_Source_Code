using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace TaleoAutomation
{
    class TxtToDataTable
    {
        public List<string[]> returnConvertedList(string f)
        {
            using (StreamReader r = new StreamReader(f))
            {
                string line;
                List<string[]> dt = new List<string[]>();
                while ((line = r.ReadLine()) != null)
                {
                    string[] words = line.Split('>');
                    List<string> conf = new List<string>();
                    foreach (string word in words)
                    {
                        conf.Add(word);
                    }
                    dt.Add(conf.ToArray());
                }
                return dt;
            }
        }
    }
}
