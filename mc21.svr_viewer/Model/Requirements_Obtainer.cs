using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace mc21.svr_viewer.Model
{
    public class Requirements_Obtainer
    {
        public List<string> List_OF_Reqs { get; set; }

        private string parsing_string;

        public Requirements_Obtainer(int rigth_border, DataRow row) 
        {
            List_OF_Reqs = new List<string>();

            if (row.ItemArray[rigth_border + 4] != DBNull.Value) 
            {
                parsing_string = row.ItemArray[rigth_border + 4].ToString();
                char[] delimiterChars = { ' ', ',', ';', ':', '\t', '\r', '\n' };
                string[] words = parsing_string.Split(delimiterChars);
                foreach (string str in words)
                    if (str.Length != 0)
                        List_OF_Reqs.Add(str);
            }
        }
        
    }
}
