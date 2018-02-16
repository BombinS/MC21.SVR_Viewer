using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace mc21.svr_viewer.Model
{
    public class Testing_Procedure
    {

#region Публичные свойста

        // Тип процедуры
        public Procedure_Type Type { get; set; }

        // Имя процедуры
        public string Name { get; set; }

        // Месторасположение процедуры
        public string Path { get; set; }

        // Описание примеров
        public List<Testing_Case> Cases { get; set; }

#endregion        

#region Конструктор
        
        public Testing_Procedure(FileInfo fi) 
        {
            // Инициализация коллекции описания верификационных примеров
            Cases = new List<Testing_Case>();

            // определить месторасположение файла верификационной процедуры
            Path = fi.FullName;

            // определить тип верификационного примера (системный/компонентный)
            if (fi.FullName.EndsWith("pts"))
                Type = Procedure_Type.System;
            else
                Type = Procedure_Type.Component;

            // считать содержимое
            string[] procedure_content = File.ReadAllLines(fi.FullName);
            
            // анализ содержимого

            bool is_procedure_name_obtained = false;
            bool is_case_started = false;
            int start_line = -1;
            int end_line = -1;
            string case_name = "";
            List<string> tested_requirements = new List<string>();
            int start_tested_requirement = -1;
            int start_pcr = -1; 

            for (int i = 0; i < procedure_content.Length; i++)
            {
                Parce_Testing_Procedure_String parsing_string = new Parce_Testing_Procedure_String(procedure_content[i]);
                // значимыми строками являются строки содержащие следующие тэги: "SCENARIO ", "END ", "Tested Requirement:", "PCR:" 
                if (parsing_string.isScenario || parsing_string.isEnd || parsing_string.isTestedRequirements || parsing_string.isPCR)
                {
                    // если в строке тэг "SCENARIO" и имя процедуры еще не определено, то определяем имя процедуры
                    if (parsing_string.isScenario && is_procedure_name_obtained == false)
                    {
                        Name = parsing_string.Obtain_Procedure_Name();
                        is_procedure_name_obtained = true;
                        continue;
                    }
                    // если в строке тэг "Tested Requirement", сохраняем индекс строки
                    if (parsing_string.isTestedRequirements) 
                    {
                        start_tested_requirement = i;
                        continue;
                    }
                    // если в строке тэг "PCR" и строка с требования была обнаружена, определяем затрассированные требования
                    if (parsing_string.isPCR && start_tested_requirement != -1) 
                    {
                        start_pcr = i;
                        List<string> temp_requirements_strings = new List<string>();
                        for (int j = start_tested_requirement; j < start_pcr; j++)
                            foreach (string str in procedure_content[j].Replace("Tested Requirement", "").Split('-', ' ', ',', ':'))
                                if (str.Equals("") == false)
                                    temp_requirements_strings.Add(str);
                        tested_requirements = temp_requirements_strings.OrderBy(x => x).ToList();
                        continue;
                    }
                    // если в строке тэг "SCENARIO", признак начала кейса в false и имя процедуры определено, то это начало тестового кейса
                    if (parsing_string.isScenario && is_procedure_name_obtained && is_case_started == false) 
                    {
                        case_name = parsing_string.Obtain_Case_Name();
                        start_line = i;
                        is_case_started = true;
                        continue;
                    }
                    // если в строке тэг "END" и признак начала сценария установлен, то это конец тестового кейса
                    if (parsing_string.isEnd && is_case_started)
                    {
                        end_line = i;
                        // если определено имя кейса и найдены позиции начала и конца, то заносим определение кейса
                        if (case_name.Equals("") == false && start_line != -1 && end_line != -1)
                            Cases.Add(new Testing_Case
                            {
                                Path = this.Path,
                                Name = case_name,
                                Row_Start = start_line,
                                Row_End = end_line,
                                Requirements = tested_requirements
                            });

                        // сброс кейсовых атрибутов
                        start_line = -1;
                        end_line = -1;
                        case_name = "";
                        tested_requirements = new List<string>();
                        start_tested_requirement = -1;
                        start_pcr = -1;
                        is_case_started = false;
                    }
                }
            }
        }

#endregion

    }

    public enum Procedure_Type 
    {
        System,
        Component
    }
}
