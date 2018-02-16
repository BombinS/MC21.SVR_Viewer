using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mc21.svr_viewer.Model
{
    /// <summary>
    /// Парсинг строки верикационной процедуры
    /// </summary>
    public class Parce_Testing_Procedure_String
    {

#region Свойства

        // Признак вхождения "SCENARIO " в строку
        public bool isScenario { get; set; }

        // Признак вхождения "END " в строку
        public bool isEnd { get; set; }

        // Признак вхождения "Tested Requirement" в строку
        public bool isTestedRequirements { get; set; }

        // Признак вхождения "PCR" в строку
        public bool isPCR { get; set; }

#endregion

        private string parsing_string;

#region Конструктор - анализ строки и заполнение свойств

        public Parce_Testing_Procedure_String(string parsing_string) 
        {
            this.parsing_string = parsing_string;

            if (parsing_string.Contains("SCENARIO "))
                isScenario = true;
            else
                isScenario = false;

            if (parsing_string.Contains("END "))
                isEnd = true;
            else
                isEnd = false;

            if (parsing_string.Contains("Tested Requirement:"))
                isTestedRequirements = true;
            else
                isTestedRequirements = false;

            if (parsing_string.Contains("PCR:"))
                isPCR = true;
            else
                isPCR = false;
        }

#endregion

#region Приватные функции

        // Определение идентификатора верификационной процедуры
        public string Obtain_Procedure_Name() 
        {
            try
            {
                // разбить строку
                string[] parts_of_procedure_name = parsing_string.Split('_');
                // найти элемент начинающийся на "PJ"
                int index_pj = -1;
                IEnumerable<string> string_pj = parts_of_procedure_name.Where(x => x.StartsWith("PJ"));
                if (string_pj.Count() != 0)
                    index_pj = parts_of_procedure_name.ToList().IndexOf(string_pj.ElementAt(0));
                if (index_pj != -1 && parts_of_procedure_name[index_pj + 1].StartsWith("PC") == true)
                    return "PROJ_" + parts_of_procedure_name[index_pj].Substring(2) + "_PROC_" + parts_of_procedure_name[index_pj + 1].Substring(2);
                }
            catch (Exception)
            {

                System.Windows.MessageBox.Show("Невалидный идентификатор верификационной процедуры: " + parsing_string);
            }

            return "";
        }

        // Определение идентификатора верификационного примера
        public string Obtain_Case_Name()
        {
            try
            {
                string[] parts_of_case_name = parsing_string.Split('_');
                // найти элемент начинающийся на "PJ"
                int index_pj = -1;
                IEnumerable<string> string_pj = parts_of_case_name.Where(x => x.StartsWith("PJ"));
                if (string_pj.Count() != 0)
                    index_pj = parts_of_case_name.ToList().IndexOf(string_pj.ElementAt(0));
                if (index_pj != -1 && parts_of_case_name[index_pj + 1].StartsWith("PC") && (parts_of_case_name[index_pj + 2].StartsWith("C") || parts_of_case_name[index_pj + 2].StartsWith("S")))
                    return ("PROJ_" + parts_of_case_name[index_pj].Substring(2) + "_PROC_" + parts_of_case_name[index_pj + 1].Substring(2) + "_CASE_" + parts_of_case_name[index_pj + 2].Substring(1)).Trim();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Невалидный идентификатор верификационного примера: " + parsing_string);
            }

            return "";
        }

#endregion 

    }
}
