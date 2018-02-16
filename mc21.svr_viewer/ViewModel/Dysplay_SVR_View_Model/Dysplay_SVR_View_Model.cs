using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mc21.svr_viewer;

namespace mc21.svr_viewer.ViewModel.Dysplay_SVR_View_Model
{
    /// <summary>
    /// Вью модель отображения содержимого SVR
    /// </summary>
    public class Dysplay_SVR_View_Model : ViewModel.ViewModelBase
    {
        // ссылка на модель
        private Model.Main_Model m;

        #region Данные для фильтров

        #region Чекбоксы позиций исполнения

        // Источник данных для отображения чекбоксов выбора позиций
        public List<Checkbox> Query_Dysplay_Checkboxes_Positions { get; set; }

        // Функция инициализации чекбоксов позиций
        private void Initialize_CheckBox_Positions()
        {
            Query_Dysplay_Checkboxes_Positions = new List<Checkbox>();
            foreach (string str in m.Positions)
            {
                Checkbox added_checkbox = new Checkbox(str);
                added_checkbox.CheckboxChanged += CheckboxChanged_Handler;
                Query_Dysplay_Checkboxes_Positions.Add(added_checkbox);

            }

            // получение статистики
            foreach (Checkbox checkbox in Query_Dysplay_Checkboxes_Positions)
            {
                IEnumerable<Model.Verification_Results_by_Case> results = m.Results_by_Case.Where(x => x.Side.Equals(checkbox.Name)).Select(x => x);
                checkbox.Amount = results.Count();
            }
        }

        #endregion

        #region Чекбоксы вариантов результатов

        // Источник данных для отображения чекбоксов выриантов результатов
        public List<Checkbox> Query_Dysplay_Checkboxes_Results { get; set; }

        // Функция инициализации чекбоксов результатов
        private void Initialize_CheckBox_Results()
        {
            Query_Dysplay_Checkboxes_Results = new List<Checkbox>();
            Checkbox added_checkbox;

            added_checkbox = new Checkbox("OK");
            added_checkbox.CheckboxChanged += CheckboxChanged_Handler;
            Query_Dysplay_Checkboxes_Results.Add(added_checkbox);
            added_checkbox = new Checkbox("KO");
            added_checkbox.CheckboxChanged += CheckboxChanged_Handler;
            Query_Dysplay_Checkboxes_Results.Add(added_checkbox);
            added_checkbox = new Checkbox("NA");
            added_checkbox.CheckboxChanged += CheckboxChanged_Handler;
            Query_Dysplay_Checkboxes_Results.Add(added_checkbox);

            // получение статистики
            foreach (Checkbox checkbox in Query_Dysplay_Checkboxes_Results)
            {
                IEnumerable<Model.Verification_Results_by_Case> results = m.Results_by_Case.Where(x => x.Result.Equals(checkbox.Name)).Select(x => x);
                checkbox.Amount = results.Count();
            }

        }

        #endregion

        #region Чекбоксы версии ПО

        /// <summary>
        /// Источник данных для отображения чекбоксов версии ПО
        /// </summary>
        public List<Checkbox> Query_Dysplay_Checkboxes_Software_Label { get; set; }

        /// <summary>
        /// Функция инициализации чекбоксов версии ПО
        /// </summary>
        private void Initialize_CheckBox_Software_Labels()
        {
            Query_Dysplay_Checkboxes_Software_Label = new List<Checkbox>();
            foreach (string str in m.Software_Labels)
            {
                Checkbox added_checkbox = new Checkbox(str);
                added_checkbox.CheckboxChanged += CheckboxChanged_Handler;
                Query_Dysplay_Checkboxes_Software_Label.Add(added_checkbox);
            }

            // получение статистики
            foreach (Checkbox checkbox in Query_Dysplay_Checkboxes_Software_Label)
            {
                IEnumerable<Model.Verification_Results_by_Case> results = m.Results_by_Case.Where(x => x.Software_Label.Equals(checkbox.Name)).Select(x => x);
                checkbox.Amount = results.Count();
            }

        }

        #endregion

        #region Чекбоксы несоответсвий

        /// <summary>
        /// Источник данных для отображения чекбоксов версии ПО
        /// </summary>
        public List<Checkbox> Query_Dysplay_Checkboxes_PCR { get; set; }

        /// <summary>
        /// Функция инициализации чекбоксов версии ПО
        /// </summary>
        private void Initialize_CheckBox_PCR()
        {
            Query_Dysplay_Checkboxes_PCR = new List<Checkbox>();
            foreach (string str in m.PCR)
            {
                Checkbox added_checkbox = new Checkbox(str);
                added_checkbox.CheckboxChanged += CheckboxChanged_Handler;
                Query_Dysplay_Checkboxes_PCR.Add(added_checkbox);
            }

            // получение статистики
            foreach (Checkbox checkbox in Query_Dysplay_Checkboxes_PCR)
            {
                IEnumerable<Model.Verification_Results_by_Case> results = m.Results_by_Case.Where(x => x.PCR.Equals(checkbox.Name)).Select(x => x);
                checkbox.Amount = results.Count();
            }

        }

        #endregion

        private void CheckboxChanged_Handler(object sender)
        {
            OnPropertyChanged("Query_Display_Results");
            OnPropertyChanged("Amount_Displayed_Results_by_Case");
        }

        /// <summary>
        /// Функция инициализации фильтров
        /// </summary>
        public void Initialize_View()
        {
            Initialize_CheckBox_Positions();
            OnPropertyChanged("Query_Dysplay_Checkboxes_Positions");

            Initialize_CheckBox_Results();
            OnPropertyChanged("Query_Dysplay_Checkboxes_Results");

            Initialize_CheckBox_Software_Labels();
            OnPropertyChanged("Query_Dysplay_Checkboxes_Software_Label");

            Initialize_CheckBox_PCR();
            OnPropertyChanged("Query_Dysplay_Checkboxes_PCR");

            OnPropertyChanged("Query_Display_Results");
            OnPropertyChanged("Amount_Displayed_Results_by_Case");

            Amount_Critical_Results = m.Critical_Result_by_Case.Count;
        }

        #endregion


        #region Отображение результатов запросов

        /// <summary>
        /// Коллекция отображаемых результатов выполнения примеров
        /// </summary>
        public List<Display_Result> Query_Display_Results
        {
            get
            {
                List<Display_Result> query_display_results = new List<Display_Result>();

                foreach (Model.Verification_Results_by_Case result in m.Results_by_Case)
                {
                    // получить состояние чекбокса результата выполнения верификационного примера
                    Checkbox checkbox_by_result = Query_Dysplay_Checkboxes_Results.Where(x => x.Name.Equals(result.Result)).Select(x => x).FirstOrDefault();
                    if (checkbox_by_result == null)
                        System.Windows.MessageBox.Show("1");

                    // получить состояние чекбокса позиции выполнения верификацонного примера
                    Checkbox checkbox_by_position = Query_Dysplay_Checkboxes_Positions.Where(x => x.Name.Equals(result.Side)).Select(x => x).First();
                    if (checkbox_by_position == null)
                        System.Windows.MessageBox.Show("2");

                    // получить состояние чекбокса версии ПО выполнения верификацонного примера
                    Checkbox checkbox_by_software_label = Query_Dysplay_Checkboxes_Software_Label.Where(x => x.Name.Equals(result.Software_Label)).Select(x => x).First();
                    if (checkbox_by_software_label == null)
                        System.Windows.MessageBox.Show("3");

                    // получить состояние чекбокса СПИ
                    Checkbox checkbox_by_pcr = Query_Dysplay_Checkboxes_PCR.Where(x => x.Name.Equals(result.PCR)).Select(x => x).First();
                    if (checkbox_by_pcr == null)
                        System.Windows.MessageBox.Show("4");

                    if (checkbox_by_result.isChecked &&
                        checkbox_by_position.isChecked &&
                        checkbox_by_software_label.isChecked &&
                        checkbox_by_pcr.isChecked)
                    {
                        Display_Result added_result = new Display_Result();
                        added_result.ID = result.ID;
                        added_result.Result = result.Result;
                        added_result.Side = result.Side;
                        added_result.PCR = result.PCR;
                        added_result.Date = result.Date;
                        added_result.Status = result.Status;
                        query_display_results.Add(added_result);
                    }
                }

                // обновить информацию о примерах
                Amount_Displayed_Results_by_Case = query_display_results.Count();
                Cases_IDs = new List<string>();
                foreach (Display_Result item in query_display_results)
                    Cases_IDs.Add(item.ID);

                // обновить информацию о критических результатах выполнения примеров


                // обновить информацию о процедурах
                Procedure_List info_procedures = new Procedure_List(query_display_results);
                Amount_Displayed_Results_by_Procedure = info_procedures.Amount_of_Procedures;
                Procedures_IDs = info_procedures.Names_Of_Procedures;

                return query_display_results.OrderBy(x => x.ID).ToList();
            }
        }

        /// <summary>
        /// Коллекция идентификаторов отображаемых примеров
        /// </summary>
        public List<string> Cases_IDs { get; set; }

        /// <summary>
        /// Количество отображаемых кейсов
        /// </summary>
        public int Amount_Displayed_Results_by_Case
        {
            get
            {
                return amount_displayed_results_by_case;
            }
            set
            {
                if (value != amount_displayed_results_by_case)
                {
                    amount_displayed_results_by_case = value;
                    OnPropertyChanged("Button_Content_Copy_Cases_to_Clipboard");
                }
            }
        }
        private int amount_displayed_results_by_case;

        /// <summary>
        /// Коллекция идентификаторов отображаемых процедур
        /// </summary>
        public List<string> Procedures_IDs { get; set; }

        /// <summary>
        /// Количество отображаемых процедур
        /// </summary>
        public int Amount_Displayed_Results_by_Procedure
        {
            get
            {
                return amount_displayed_results_by_procedure;
            }
            set
            {
                if (value != amount_displayed_results_by_procedure)
                {
                    amount_displayed_results_by_procedure = value;
                    OnPropertyChanged("Button_Content_Copy_Procedures_to_Clipboard");
                }
            }
        }
        private int amount_displayed_results_by_procedure;

        /// <summary>
        /// Количество критических важных предупреждений
        /// </summary>
        public int Amount_Critical_Results
        {
            get
            {
                return amount_critical_results;
            }
            set
            {
                if (value != amount_critical_results)
                {
                    amount_critical_results = value;
                    OnPropertyChanged("Button_Content_Show_Critical_Results");
                }
            }
        }
        private int amount_critical_results;



        #endregion

        #region Управление

        #region Кнопка отображения критически важных предупреждений

        /// <summary>
        /// Текст кнопки показа критических важных предупреждений
        /// </summary>
        public string Button_Content_Show_Critical_Results
        {
            get
            {
                return "Количество предупреждений: " + Amount_Critical_Results.ToString() + "\nОтобразить невалидные результаты";
            }
            set { }
        }

        /// <summary>
        /// Команда кнопки отображения предупреждений
        /// </summary>
        private DelegateCommand show_critical_results_command;


        #endregion

        #region Кнопка копирования идентификаторов процедур в буфер обмена

        /// <summary>
        /// Текст кнопки копирования названий процедур в буфер обмена
        /// </summary>
        public string Button_Content_Copy_Procedures_to_Clipboard
        {
            get
            {
                if (Amount_Displayed_Results_by_Procedure != 0)
                    return "Количество отображаемых верификационных процедур: " + Amount_Displayed_Results_by_Procedure.ToString() + "\nСкопировать в буфер обмена";
                else
                    return "Количество отображаемых верификационных процедур: " + Amount_Displayed_Results_by_Procedure.ToString();
            }
            set { }
        }

        /// <summary>
        /// Команда кнопки копирования названий процедур в буфер обмена
        /// </summary>
        private DelegateCommand copy_procedures_to_clipboard_command;
        public ICommand Copy_Procedures_to_Clipboard_Command
        {
            get
            {
                if (copy_procedures_to_clipboard_command == null)
                    copy_procedures_to_clipboard_command = new DelegateCommand(copy_procedures_to_clipboard_action);
                return copy_procedures_to_clipboard_command;
            }
        }

        private void copy_procedures_to_clipboard_action()
        {
            External_Interface.Copy_IDs_To_Clipboard.Action(Procedures_IDs);
        }

        #endregion

        #region Кнопка копирования идентификаторов примеров в буфер обмена


        // Текст кнопки копирования названий примеров в буфер обмена
        public string Button_Content_Copy_Cases_to_Clipboard
        {
            get
            {
                if (Amount_Displayed_Results_by_Case != 0)
                    return "Количество отображаемых верификационных примеров: " + Amount_Displayed_Results_by_Case.ToString() + "\nСкопировать в буфер обмена";
                else
                    return "Количество отображаемых верификационных примеров: " + Amount_Displayed_Results_by_Case.ToString();
            }
            set { }
        }

        // Команда кнопки копирования идентификаторов примеров в буфер обмена
        private DelegateCommand copy_cases_to_clipboard_command;
        public ICommand Copy_Cases_to_Clipboard_Command
        {
            get
            {
                if (copy_cases_to_clipboard_command == null)
                    copy_cases_to_clipboard_command = new DelegateCommand(copy_cases_to_clipboard_action);
                return copy_cases_to_clipboard_command;
            }
        }

        // действие кнопки копирования идентификаторов примеров в буфер обмена
        private void copy_cases_to_clipboard_action()
        {
            External_Interface.Copy_IDs_To_Clipboard.Action(Cases_IDs);
        }

        #endregion

        #region Кнопка формирования таблицы анализа непрошедших процедур

        // текст кнопки формирования таблицы анализа непрошедших процедур
        public string Button_Content_Create_Failed_Procedures_Table
        {
            get
            {
                return "Сформировать таблицу анализа непрошедших процедур";
            }
            set { }
        }

        // команда кнопки формирования таблицы анализа непрошедших процедур
        private DelegateCommand create_failed_procedures_table_command;
        public ICommand Create_Failed_Procedures_Table_Command
        {
            get
            {
                if (create_failed_procedures_table_command == null)
                    create_failed_procedures_table_command = new DelegateCommand(create_failed_procedures_table_command_action);
                return create_failed_procedures_table_command;
            }
        }

        // действие команды кнопки формирования таблицы анализа непрошедших процедур
        private void create_failed_procedures_table_command_action()
        {
            External_Interface.Forming_Tables_Of_Failed_Procedures action = new External_Interface.Forming_Tables_Of_Failed_Procedures(m.Results_by_Case);
        }

        #endregion

        #region Кнопка формирования матрицы CRM

        // текст кнопки формирования матрицы CRM
        public string Button_Content_Create_CRM_Table
        {
            get
            {
                return "Сформировать матрицу CRM";
            }
            set { }
        }

        // команда кнопки формирования матрицы CRM
        private DelegateCommand create_crm_table_command;
        public ICommand Create_Crm_Table_Command
        {
            get
            {
                if (create_crm_table_command == null)
                    create_crm_table_command = new DelegateCommand(create_crm_table_command_action);
                return create_crm_table_command;
            }
        }

        // действие команды кнопки формирования матрицы CRM
        private void create_crm_table_command_action()
        {
            External_Interface.Forming_Table_Of_CRM action = new External_Interface.Forming_Table_Of_CRM(m.Results_by_Case);
        }

        #endregion


        #endregion


#region Конструктор - получение ссылки на модель

        public Dysplay_SVR_View_Model() 
        {
            this.m = ViewModel.Main_ViewModel.m;

            // инициализировать чекбоксы
            Initialize_View();

            OnPropertyChanged("Query_Display_Results");
        }

#endregion

    }
}
