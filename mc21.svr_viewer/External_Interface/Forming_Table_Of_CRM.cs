using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using mc21.svr_viewer.Model;


namespace mc21.svr_viewer.External_Interface
{
    public class Forming_Table_Of_CRM
    {

        // наименование временного файла для формирования таблицы анализа непрошедших процедур
        private string filename = "tables_with_crm.xlsx";

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

        public Forming_Table_Of_CRM(List<Verification_Results_by_Case> results) 
        {
            // проверить на существование временный файл, если есть удалить
            check_temporary_file();

            // получить имя ресурса xlsx файла для отображения непрошедших процедур
            string resource_name = get_embedded_resource_name(filename);

            // создать временный файл из ресурса
            create_temporary_file(resource_name);

            // выбрать уникальные идентификаторы требований
            List<string> unique_requirements = get_unique_requirements(results);

            // если требования существуют
            if (unique_requirements.Count != 0)
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
                        if (!sheetname.EndsWith("CRM$'") && !sheetname.EndsWith("CRM$"))
                            continue;

                        // для каждого идентификатора требования 
                        foreach (string requirement in unique_requirements)
                        {
                            // получить список идентификаторов процедур
                            List<string> procedures = results.Where(x => x.is_Requirement_traced(requirement)).Select(x => x.Procedure_ID).Distinct().OrderBy(x => x).ToList();

                            // занести информацию в xlsx файл
                            int start_row = 1;
                            while (true)
                            {
                                if (start_row > procedures.Count)
                                    break;

                                string sql_requirements_id;
                                if (start_row == 1)
                                    sql_requirements_id = requirement;
                                else
                                    sql_requirements_id = "";

                                string sql_procedure_id = procedures[start_row - 1];

                                try
                                {
                                    OleDbCommand command = new OleDbCommand();
                                    command.Connection = connection;
                                    command.CommandText = "INSERT INTO [" + sheetname + "] (F1, F6) values('" + sql_requirements_id + "','" + sql_procedure_id + "')";
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
                    }

                    // закрыть подключение
                    connection.Close();
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

        // выбрать уникальные идентификаторы требований
        private List<string> get_unique_requirements(List<Verification_Results_by_Case> results)
        {
            List<string> requirements = new List<string>();
            foreach (Verification_Results_by_Case result in results)
                foreach (string req in result.Reqs)
                    requirements.Add(req);
            return requirements.Distinct().OrderBy(x => x).ToList();
        }

        // открыть временный xlsx файл CRM
        private void open_xls_action()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "EXCEL.EXE";
            process.StartInfo.Arguments = Filename;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(close_xls_action);
            process.Start();
        }

        // действие при закрытии файла CRM
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
