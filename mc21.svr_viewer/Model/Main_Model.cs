using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace mc21.svr_viewer.Model
{
    public class Main_Model
    {

#region Тестовые процедуры

    #region Свойства

        // Путь к директории тестовых процедур. При смене значения инициализируется загрузка и анализ содержимого тестовых процедур
        public string Testing_Procedures_Directory 
        {
            get 
            {
                return testing_procedures_directory;
            }
            set 
            {
                if (value != testing_procedures_directory) 
                {
                    testing_procedures_directory = value;
                    initialize_testing_procedures_analisys();
                }
            } 
        }

        // Класс описания верификационных примеров
        public Testing_Procedures Procedures { get; set; }

    #endregion

    #region Приваты

        private string testing_procedures_directory = "";

        // инициализация анализа содержимого директории тестовых процедур
        private void initialize_testing_procedures_analisys()
        {
            Procedures = new Testing_Procedures(testing_procedures_directory);
            Diagnostics.Timestamps.Write_Timestamps();
        }
    #endregion

#endregion

        // Результаты верификации по частям SVCP 
        public List<Verification_Results_By_Part> Results_by_Parts { get; set; }

        // Результаты верификации по кейсам (всех частей)
        public List<Verification_Results_by_Case> Results_by_Case 
        {
            get 
            {
                List<Verification_Results_by_Case> results_by_case = new List<Verification_Results_by_Case>();

                foreach (Verification_Results_By_Part part in Results_by_Parts)
                    foreach (Verification_Results_by_Case case_result in part.Results)
                        results_by_case.Add(case_result);

                return results_by_case;
            }
        }


        // Критические результаты всех частей
        public List<Verification_Results_by_Case> Critical_Result_by_Case 
        { 
            get 
            {
                List<Verification_Results_by_Case> critical_result_by_case = new List<Verification_Results_by_Case>();

                foreach (Verification_Results_By_Part part in Results_by_Parts)
                    foreach (Verification_Results_by_Case case_result in part.Results)
                        if (case_result.Status == Test_Case_Status.KO || case_result.Status == Test_Case_Status.OK_PCR_Exist)
                            critical_result_by_case.Add(case_result);

                return critical_result_by_case;
            } 
        }

        public List<Reqs_vs_Tests> CRM_Data { get; set; }


        // Коллекция позиций выполнения
        public List<string> Positions
        {
            get 
            {
                if (positions == null)
                    positions = new List<string>();
                return positions;
            }
        }
        private List<string> positions;

        /// <summary>
        /// Коллекция версий ПО
        /// </summary>
        public List<string> Software_Labels 
        { 
            get 
            {
                if (software_labels == null)
                    return new List<string>();
                return software_labels; 
            } 
        }
        private List<string> software_labels;

        /// <summary>
        /// Коллекция идентификаторов СПИ
        /// </summary>
        public List<string> PCR
        {
            get
            {
                if (pcr == null)
                    return new List<string>();
                return pcr;
            }
        }
        private List<string> pcr;


        // Конструктор модели
        public Main_Model() 
        {
            Results_by_Parts = new List<Verification_Results_By_Part>();
        }

        // обнуление модели
        public void Clear_Model() 
        {
            Results_by_Parts = new List<Verification_Results_By_Part>();
        }

        // заполнение результатов одной части
        public void Fill_Model(System.Data.DataTable dt)
        {
            Verification_Results_By_Part results_by_part = new Verification_Results_By_Part();

            int left_border = -1;
            int rigth_border = -1;
            int top_border = -1;

            // поиск первой значимой ячейки (наименование части в SVCP)
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    if (item != DBNull.Value)
                    {
                        results_by_part.SVCP_Part_Name = item.ToString();
                        break;
                    }
                }
                if (results_by_part.SVCP_Part_Name.Equals("") == false)
                    break;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Rows[i].ItemArray.Count(); j++)
                {
                    if (dt.Rows[i].ItemArray[j].ToString().ToLower().Contains("идентификатор верификационной процедуры"))
                    {
                        top_border = i + 1;
                        left_border = j + 1;
                    }
                    if (dt.Rows[i].ItemArray[j].ToString().ToLower().Contains("тип верификационной процедуры") ||
                        dt.Rows[i].ItemArray[j].ToString().ToLower().Contains("категория верификационной процедуры"))
                        rigth_border = j - 1;
                }
                if (left_border != -1 && rigth_border != -1 && top_border != -1)
                    break;
            }

            // по строчное считывание таблицы результатов
            for (int i = top_border + 1; i < dt.Rows.Count; i++)
            {
                // единицей верификационного примера является непустая ячейка в диапазоне столбцов описывающих позицию выполнения
                for (int j = left_border; j < rigth_border+1; j++)
                {
                    if (dt.Rows[i].ItemArray[j] != DBNull.Value)
                    {
                        Verification_Results_by_Case result = new Verification_Results_by_Case();

                        // определение идентификатора
                        result.ID = dt.Rows[i].ItemArray[left_border - 1].ToString().Trim();

                        // определение позиции
                        result.Side = dt.Rows[top_border].ItemArray[j].ToString().Trim();

                        // определение результата
                        string temp_result = dt.Rows[i].ItemArray[j].ToString().Trim();
                        if (temp_result.ToLower().Contains("ok"))
                            result.Result = "OK";
                        else if (temp_result.ToLower().Contains("ko"))
                            result.Result = "KO";
                        else
                            result.Result = "NA";


                        // версия ПО
                        if (dt.Rows[i].ItemArray[rigth_border + 3] == DBNull.Value)
                            result.Software_Label = "";
                        else
                        {
                            string whole_string = dt.Rows[i].ItemArray[rigth_border + 3].ToString();
                            string search_pattern = result.Side + ":";
                            int index_start = whole_string.IndexOf(search_pattern);
                            if (index_start == -1)
                            {
                                result.Software_Label = whole_string.Trim();
                            }
                            else
                            {
                                string partial_string = whole_string.Substring(index_start + search_pattern.Length);
                                int index_end = partial_string.IndexOf("\n");
                                if (index_end == -1)
                                {
                                    result.Software_Label = partial_string.Trim();
                                }
                                else 
                                {
                                    result.Software_Label = partial_string.Substring(0, index_end).Trim();
                                }
                            }
                        }

                        // СПИ
                        if (dt.Rows[i].ItemArray[rigth_border + 8] == DBNull.Value)
                            result.PCR = "";
                        else
                            result.PCR = correct_mc(dt.Rows[i].ItemArray[rigth_border + 8].ToString()).Trim();

                        // дата верификации
                        if (dt.Rows[i].ItemArray[rigth_border + 5] == DBNull.Value)
                            result.Date = "";
                        else
                            result.Date = dt.Rows[i].ItemArray[rigth_border + 5].ToString().Trim();

                        // затрассированные требования
                        result.Reqs = new Requirements_Obtainer(rigth_border, dt.Rows[i]).List_OF_Reqs;


                        results_by_part.Results.Add(result);
                    }
                }
            }

            Results_by_Parts.Add(results_by_part);
        }

        /// <summary>
        /// Корректировка русского вхождения МС 
        /// </summary>
        private string correct_mc(string str)
        {
            if (str.Contains("МС"))  
                return str.Replace("МС", "MC"); 
            if (str.Contains("MС"))  
                return str.Replace("MС", "MC"); 
            if (str.Contains("МC"))  
                return str.Replace("МC", "MC"); 
            return str;
        }

        // получение статистических данных
        public void Obtain_Statictics()
        {
            // получение списка позиций выполнения
            initialize_side_checkboxes();
            
            // получение списка версий ПО
            initialize_software_labels();

            // получение списка СПИ
            initialize_pcr();

            // определение статусов выполнения верификационных примеров
            initialize_statuses();

            // заполнение матрицы CRM
            initialize_crm();
        }

        // Заполнение матрицы CRM
        private void initialize_crm()
        {
            CRM_Data = new List<Reqs_vs_Tests>();

            // получить список всех протрассированных требований
            List<string> all_requirements = new List<string>();
            foreach (var result in Results_by_Case)
                foreach (var req in result.Reqs)
                    all_requirements.Add(req);
            List<string> unique_requirements = all_requirements.Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Определение статусов выполнения верификационных примеров
        /// </summary>
        private void initialize_statuses()
        {
            foreach (Verification_Results_by_Case result in Results_by_Case)
            {
                if (result.Result.ToLower().Equals("ok") && result.PCR.Equals("") == true)
                {
                    result.Status = Test_Case_Status.OK;
                    continue;
                }
                if (result.Result.ToLower().Equals("ok") && result.PCR.Equals("") == false)
                {
                    result.Status = Test_Case_Status.OK_PCR_Exist;
                    continue;
                }
                if (result.Result.ToLower().Equals("ko") && ( result.PCR.ToLower().StartsWith("mc21")))
                {
                    result.Status = Test_Case_Status.KO_PCR_Exist;
                    continue;
                }
                if (result.Result.ToLower().Equals("ko") && result.PCR.ToLower().StartsWith("bug"))
                {
                    result.Status = Test_Case_Status.KO_Bag_Report_Exist;
                    continue;
                }
                if (result.Result.ToLower().Equals("ko") && result.PCR.ToLower().StartsWith("mc21") == false && result.PCR.ToLower().StartsWith("bug") == false)
                {
                    result.Status = Test_Case_Status.KO;
                    continue;
                }
                if (result.Result.ToLower().Equals("na") && result.PCR.Equals("") == true) 
                {
                    result.Status = Test_Case_Status.NA;
                    continue;
                }
                if (result.Result.ToLower().Equals("na") && result.PCR.ToLower().StartsWith("mc21") == false && result.PCR.ToLower().StartsWith("bug") == false)
                {
                    result.Status = Test_Case_Status.KO;
                    continue;
                }
                if (result.Result.ToLower().Equals("na") && result.PCR.ToLower().StartsWith("mc21"))
                {
                    result.Status = Test_Case_Status.NA_PCR_Exist;
                    continue;
                }
                if (result.Result.ToLower().Equals("na") && result.PCR.ToLower().StartsWith("bug"))
                {
                    result.Status = Test_Case_Status.NA_Bag_Report_Exist;
                    continue;
                }
                result.Status = Test_Case_Status.Not_Defined;
            }
        }

        /// <summary>
        /// Получение списка СПИ
        /// </summary>
        private void initialize_pcr()
        {
            pcr = Results_by_Case.Select(x => x.PCR).Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Получение списка версий ПО
        /// </summary>
        private void initialize_software_labels()
        {
            software_labels = Results_by_Case.Select(x => x.Software_Label).Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Инициализация состояний чекбоксов позиций вычислителя
        /// </summary>
        private void initialize_side_checkboxes()
        {
            // Получение списка позиций выполнения
            positions = Results_by_Case.Select(x => x.Side).Distinct().OrderBy(x => x).ToList();

        }
    }
}
