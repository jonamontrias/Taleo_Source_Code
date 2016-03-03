using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace TaleoAutomation
{
    static class ExcelToDataTable
    {
        public static DataTable Convert(string path)
        {
            DataTable dt = new DataTable();
            OleDbCommand DBCommand = new OleDbCommand();
            OleDbConnection DBConnect = new OleDbConnection();
            OleDbDataAdapter DBadap = new OleDbDataAdapter();
           // bool headers = false;
            string HDR = "No";
            string constring = "";
            if (path.Substring(path.LastIndexOf('.')).ToLower().Equals(".xlsx"))
                constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                constring = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            DBConnect = new OleDbConnection();
            DBConnect = new OleDbConnection(constring);
            DBConnect.Open();
            DBadap = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", DBConnect);
            var ds = new DataSet();
            DBadap.Fill(ds, "Table");
            dt = ds.Tables["Table"];
            return dt;
        }

    }
}
