using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Data;
using System.IO;

namespace mc21.svr_viewer.ViewModel
{
    public class Main_ViewModel : ViewModelBase
    {
        // статический экземляр ссылки на модель
        public static Model.Main_Model m;

#region Выбор вида отображения главного окна

        // вид отображения главного окна
        public View.View_Selectors.View_Types Type_Of_View 
        {
            get 
            {
                return type_of_view; 
            }
            set 
            {
                if (value != type_of_view) 
                {
                    type_of_view = value;
                    OnPropertyChanged("Type_Of_View");
                }
            } 
        }
        private View.View_Selectors.View_Types type_of_view;

#endregion

#region Команды меню

    #region Загрузка SVR

        // команда для элемента меню "Загрузить SVR"
        private DelegateCommand load_svr_command;
        public ICommand Load_SVR_Command 
        {
            get 
            {
                if (load_svr_command == null)
                    load_svr_command = new DelegateCommand(load_svr_action);
                return load_svr_command;
            }
        }

        // действие при активации команды для элемента меню "Загрузить SVR"
        private void load_svr_action()
        {
            // выбрать таблицы данных представляющих собой результаты верификации
            Excel.Load_SVR loaded_worksheets = new Excel.Load_SVR();

            // сбрось текущие данные модели
            m.Clear_Model();

            // заполнить модель
            foreach (DataTable dt in loaded_worksheets.Worksheets)
                m.Fill_Model(dt);

            // обновить статистику данных модели
            m.Obtain_Statictics();

            // сменить вид отображения на SVR вью
            Type_Of_View = View.View_Selectors.View_Types.SVR_Content;

        }

    #endregion

    #region Загрузка тестовых процедур

        // Команда элемента меню "Загрузить тестовые процедуры"
        private DelegateCommand load_testing_procedures;
        public ICommand Load_Testing_Procedures 
        {
            get 
            {
                if (load_testing_procedures == null)
                    load_testing_procedures = new DelegateCommand(load_testing_procedures_action);
                return load_testing_procedures;
            }
        }

        // действия команды элемента меню "Загрузить тестовые процедуры"
        private void load_testing_procedures_action()
        {
            // вызвать диалоговое окно выбора директории
            System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog();
            fd.SelectedPath = Directory.GetCurrentDirectory();
            System.Windows.Forms.DialogResult result = fd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                m.Testing_Procedures_Directory = fd.SelectedPath;
        }

    #endregion

    #region Выбрать вид отображения SVR

        // команда элемента меню "Отобразить SVR"
        private DelegateCommand dysplay_svr_content_command;
        public ICommand Dysplay_SVR_Content_Command
        {
            get
            {
                if (dysplay_svr_content_command == null) 
                    dysplay_svr_content_command = new DelegateCommand(dysplay_svr_content_command_action);
                return dysplay_svr_content_command;
            }
        }

        // действие команды элемента меню "Отобразить SVR"
        private void dysplay_svr_content_command_action()
        {
            Type_Of_View = View.View_Selectors.View_Types.SVR_Content;
        }

    #endregion

    #region Выбрать вид отображения затрассированных требований

        // команда элемента меню "Отобразить затрассированные требования"
        private DelegateCommand dysplay_svr_reqs_vs_tpr_reqs_command;
        public ICommand Dysplay_SVR_Reqs_vs_TPr_Reqs_Command
        {
            get
            {
                if (dysplay_svr_reqs_vs_tpr_reqs_command == null)
                    dysplay_svr_reqs_vs_tpr_reqs_command = new DelegateCommand(dysplay_svr_reqs_vs_tpr_reqs_command_action);
                return dysplay_svr_reqs_vs_tpr_reqs_command;
            }
        }

        // действие команды элемента меню "Отобразить SVR"
        private void dysplay_svr_reqs_vs_tpr_reqs_command_action()
        {
            Type_Of_View = View.View_Selectors.View_Types.SVR_Reqs_vs_TPr_Reqs;
        }

    #endregion

#endregion

#region Конструкторы

        public Main_ViewModel() 
        {

        }

        public Main_ViewModel(Model.Main_Model model) 
        {
            m = model;
        }

#endregion

    }
}
