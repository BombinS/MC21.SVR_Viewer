using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mc21.svr_viewer.ViewModel
{
    /// <summary>
    /// Работа с коллекцией отображения кейсов в части информации о процедурах
    /// </summary>
    public class Procedure_List
    {
        /// <summary>
        /// Количество процедур
        /// </summary>
        public int Amount_of_Procedures { get; set; }

        /// <summary>
        /// Коллекция наименований о процедурах
        /// </summary>
        public List<string> Names_Of_Procedures { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="displayed_result_by_case"></param>
        public Procedure_List(List<Display_Result> displayed_result_by_case) 
        {
            List<string> result = new List<string>();

            if (displayed_result_by_case != null) 
            {
                foreach (Display_Result case_result in displayed_result_by_case)
                {
                    // валидным идентификатором вер.примера считается строка содержащая 'PROC_' 
                    int start_index = is_valid_case_result_id(case_result.ID);
                    if (start_index != -1) 
                    {
                        // содержание до следующего вхождения '_' считается идентификационным номером процедуры
                        int end_index = case_result.ID.IndexOf("_", start_index + 5); 
                        // идентификационный номер должен представлять собой число
                        if (end_index != -1) 
                            result.Add(case_result.ID.Substring(0,end_index));
                    }
                }
            }

            Names_Of_Procedures = result.Distinct().ToList();
            Amount_of_Procedures = Names_Of_Procedures.Count;
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
