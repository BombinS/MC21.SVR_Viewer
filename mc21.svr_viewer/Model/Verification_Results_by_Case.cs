using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mc21.svr_viewer.Model
{
    public class Verification_Results_by_Case
    {
        public string ID { get; set; }

        public string Side { get; set; }

        public string Result { get; set; }

        public string Software_Label { get; set; }

        public string PCR { get; set; }

        public string Date { get; set; }

        public Test_Case_Status Status { get; set; }

        public List<string> Reqs { get; set; }

        // Идентификатор процедуры
        public string Procedure_ID 
        {
            get 
            {
                if (ID != null) 
                {
                    // валидным идентификатором вер.примера считается строка содержащая 'PROC_' 
                    int start_index = is_valid_case_result_id(ID);
                    if (start_index != -1)
                    {
                        // содержание до следующего вхождения '_' считается идентификационным номером процедуры
                        int end_index = ID.IndexOf("_", start_index + 5);
                        // идентификационный номер должен представлять собой число
                        if (end_index != -1)
                            return ID.Substring(0, end_index).Trim();
                    }
                }
                return "";
            }
        }

        // признак трассирования требования
        public bool is_Requirement_traced(string requirement_id) 
        {
            if (Reqs != null) 
                return Reqs.Contains(requirement_id);
            return false;
        }

        // валидным идентификатором вер.примера считается строка содержащая 'PROC_' 
        private int is_valid_case_result_id(string str)
        {
            if (str == null)
                return -1;
            return str.ToLower().IndexOf("proc_");
        }
    }
}
