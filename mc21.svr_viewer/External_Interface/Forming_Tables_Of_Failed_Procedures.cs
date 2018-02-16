using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using mc21.svr_viewer.Model;


namespace mc21.svr_viewer.External_Interface
{
    /// <summary>
    /// Класс формирования таблицы xls содержащей обоснование непрошедших процедур
    /// </summary>
    public class Forming_Tables_Of_Failed_Procedures
    {
        // наименование временного файла для формирования таблицы анализа непрошедших процедур
        private string filename = "tables_with_failed_results.xlsx";

        // путь к временному xlsx файлу    
        public string Filename 
        {
            get 
            {
                return Directory.GetCurrentDirectory() + "\\" + filename;
            }
        }

        // ресурсы 
        public Assembly Assembly 
        {
            get 
            {
                return this.GetType().Assembly;
            }
        }

        // строка подключения к временному файлу
        public string Oledb_Connection_String 
        {
            get
            {
                Dictionary<string, string> props = new Dictionary<string, string>();
                // XLSX - Excel 2007, 2010, 2012, 2013
                props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
                props["Data Source"] = Filename;
                props["Extended Properties"] = "\"Excel 12.0 XML;HDR=NO;IMEX=3\"";

                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> prop in props)
                {
                    sb.Append(prop.Key);
                    sb.Append('=');
                    sb.Append(prop.Value);
                    sb.Append(';');
                }

                return sb.ToString();
            }
        }

#region Конструктор

        public Forming_Tables_Of_Failed_Procedures(List<Verification_Results_by_Case> results) 
        {

            // проверить на существование временный файл, если есть удалить
            check_temporary_file();

            // получить имя ресурса xlsx файла для отображения непрошедших процедур
            string resource_name = get_embedded_resource_name(filename);

            // создать временный файл из ресурса
            create_temporary_file(resource_name);

            // выбрать результаты выполнения верификационных результатов на c определенными СПИ или открытыми багами в багзилле
            IEnumerable<Verification_Results_by_Case> ko_results = results.Where(x => x.Status == Test_Case_Status.KO_PCR_Exist || 
                                                                                 x.Status == Test_Case_Status.KO_Bag_Report_Exist)
                                                                          .OrderBy(x => x.ID);
            // получить список уникальных СПИ
            IEnumerable<string> pcrs = ko_results.Select(x => x.PCR).Distinct().OrderBy(x => x);

            // если проблемы существуют
            if (pcrs.Count() != 0)
            {
                try
                {
                    // подключиться к xlsx как к базе данных
                    OleDbConnection connection = new OleDbConnection(Oledb_Connection_String);
                    connection.Open();

                    // запрос структуры бызы данных
                    System.Data.DataTable dtSheet = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    // для листа "Не прошедшие процедуры"
                    foreach (DataRow dr in dtSheet.Rows)
                    {
                        string sheetname = dr["TABLE_NAME"].ToString();
                        if (!sheetname.EndsWith("Не прошедшие процедуры$'"))
                            continue;

                        // для каждого идентификатора проблемы
                        foreach (string problem_id in pcrs)
                        {
                            // создать строки перечня процедур
                            List<string> procedures = get_string_of_procedures(results, problem_id);

                            // создать строки перечня затрассированных требований
                            List<string> requirements = get_string_of_requirements(results, problem_id);

                            // занести инфо в xlsx файл
                            int start_row = 1;
                            while(true){

                                if (start_row > procedures.Count && start_row > requirements.Count)
                                    break;

                                string sql_problem_id;
                                if (start_row > 1)
                                    sql_problem_id = "";
                                else
                                    sql_problem_id = problem_id;

                                string sql_procedure;
                                if (start_row > procedures.Count)
                                    sql_procedure = "";
                                else
                                    sql_procedure = procedures[start_row - 1];

                                string sql_requirement = "";
                                if (start_row > requirements.Count)
                                    sql_requirement = "";
                                else
                                    sql_requirement = requirements[start_row - 1];

                                try
                                {
                                    OleDbCommand command = new OleDbCommand();
                                    command.Connection = connection;
                                    command.CommandText = "INSERT INTO [" + sheetname + "] (F2, F3, F4) values('" + sql_problem_id + "','" + sql_procedure + "','" + sql_requirement + "')";
                                    command.ExecuteNonQuery();
                                    command.Dispose();
                                }
                                catch (Exception error)
                                {
                                    StringBuilder error_message = new StringBuilder();
                                    error_message.AppendLine("Ошибка записи в xlsx файл");
                                    error_message.AppendLine("Операция не может быть выполнена");
                                    error_message.AppendLine(error.Message);
                                    System.Windows.Forms.MessageBox.Show(error_message.ToString());                                    
                                }
                                start_row++;
                            }
                        }
                        // закрыть подключение с xlsx
                        connection.Close();
                        connection.Dispose();
                    }

                }
                catch (Exception error)
                {
                    StringBuilder error_message = new StringBuilder();
                    error_message.AppendLine("Ошибка открытия xlsx файла");
                    error_message.AppendLine("Операция не может быть выполнена");
                    error_message.AppendLine(error.Message);
                    System.Windows.Forms.MessageBox.Show(error_message.ToString());
                }
            }

            // открыть временный файл
            open_xls_action();
        }


#endregion

        // удаление временного файла в случае его существования
        private void check_temporary_file()
        {
            FileInfo f = new FileInfo(Filename);
            if (f.Exists)
                f.Delete();
        }

        // создание файла из ресурса
        private void create_temporary_file(string resource_name)
        {
            Stream input = Assembly.GetManifestResourceStream(resource_name);
            Stream output = File.Open(Filename, FileMode.CreateNew);
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, read);

            input.Dispose();
            output.Dispose();
        }

        // получение имени ресурса xlsx файла для отображения непрошедших процедур
        private string get_embedded_resource_name(string resource_name)
        {
            foreach (string str in Assembly.GetManifestResourceNames())
                if (str.Contains(resource_name))
                    return str;
            return null;
        }

        // создание строки перечня процедур по идентификатору проблемы
        private List<string> get_string_of_procedures(List<Verification_Results_by_Case> results, string problem_id)
        {
            return results.Where(x => x.PCR.Equals(problem_id)).Select(x => x.Procedure_ID).Distinct().ToList();
        }

        // создание строки перечня затрассированных требований по идентификатору проблемы
        private List<string> get_string_of_requirements(List<Verification_Results_by_Case> results, string problem_id)
        {

            IEnumerable<Verification_Results_by_Case> case_list = results.Where(x => x.PCR.Equals(problem_id));

            List<string> requirements = new List<string>();
            foreach (Verification_Results_by_Case _case in case_list)
                foreach (string req in _case.Reqs)
                    requirements.Add(req);

            IEnumerable<string> unique_requirements = requirements.Distinct().OrderBy(x => x);

            return unique_requirements.ToList();
        }

        // открыть временный xlsx файл анализа непрошедших процедур
        private void open_xls_action()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "EXCEL.EXE";
            process.StartInfo.Arguments = Filename;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(close_xls_action);
            process.Start();
        }

        // действие призакрытии файла анализа непрошедших процедур
        private void close_xls_action(object sender, System.EventArgs e) 
        {
            // необходимо удалить временный файл анализа непрошедших процедур
            FileInfo f = new FileInfo(Filename);
            if (f.Exists) 
            {
                try
                {
                    f.Delete();
                }
                catch (Exception error)
                {

                    System.Windows.MessageBox.Show(error.Message);
                }
            }
                
        }

    }
}
