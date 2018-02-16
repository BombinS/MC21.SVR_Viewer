using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace mc21.svr_viewer.Excel
{
    public class Load_SVR
    {

        /// <summary>
        /// Коллекция таблиц данных представляющих собой листы SVR с детальными результатами
        /// </summary>
        public List<DataTable> Worksheets 
        {
            get 
            {
                if (worksheets == null)
                    worksheets = new List<DataTable>();
                return worksheets;
            }
        }
        private List<DataTable> worksheets = null;

        // путь к выбранному файлу
        private string path = null;

        // строка подключения к xls файлу
        private string connection_string = null;


        #region Конструкторы

        // конструктор класса загрузки SVR посредством вызова диалогового окна
        public Load_SVR() 
        {
            // вызвать форму выбора файла
            get_path_to_svr();

            if (path != null)
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    Dictionary<string, string> props = new Dictionary<string, string>();
                    // XLSX - Excel 2007, 2010, 2012, 2013
                    props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
                    props["Data Source"] = path;
                    props["Extended Properties"] = "\"Excel 12.0 XML;HDR=NO;IMEX=1\"";

                    StringBuilder sb = new StringBuilder();
                    foreach (KeyValuePair<string, string> prop in props)
                    {
                        sb.Append(prop.Key);
                        sb.Append('=');
                        sb.Append(prop.Value);
                        sb.Append(';');
                    }

                    connection_string = sb.ToString();
                    worksheets = new List<DataTable>();
                }
            }

            if (connection_string != null) 
            {
                try
                {

                // подключение в xlsx как к базе данных
                OleDbConnection connection = new OleDbConnection(connection_string);
                
                // открыть подключение
                connection.Open();
                
                // запрос структуры бызы данных
                System.Data.DataTable dtSheet = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow dr in dtSheet.Rows)
                {
                    // формирование команды выборки данных
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM [" + dr["TABLE_NAME"].ToString() + "]";
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                    string table_name = dr["TABLE_NAME"].ToString();
                    if (table_name.EndsWith("$\'") || table_name.EndsWith("$"))
                    {

                    DataTable datatable;

                    //выборка данных
                    datatable = new DataTable();
                    da.Fill(datatable);

                        foreach (DataRow drow in datatable.Rows)
                        {
                            int number_columns = drow.ItemArray.Count();
                            for (int i = 0; i < number_columns; i++)
                            {
                                if (drow.ItemArray[i] != DBNull.Value)
                                    if (drow.ItemArray[i].ToString().ToLower().Contains("идентификатор верификационной процедуры"))
                                    {
                                        worksheets.Add(datatable);
                                        break;
                                    }
                            }
                        }
                    }
                }

                // закрыть подключение
                connection.Close();
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("error due Excel parse");
                }

            }

        }

        // (tdd) конструктор класса загрузки SVR посредством указания в качестве аргумента пути к файлу xls
        public Load_SVR(string test_path)
        {
            path = test_path;

            if (path != null)
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    Dictionary<string, string> props = new Dictionary<string, string>();
                    // XLSX - Excel 2007, 2010, 2012, 2013
                    props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
                    props["Data Source"] = path;
                    props["Extended Properties"] = "\"Excel 12.0 XML;HDR=NO;IMEX=1\"";

                    StringBuilder sb = new StringBuilder();
                    foreach (KeyValuePair<string, string> prop in props)
                    {
                        sb.Append(prop.Key);
                        sb.Append('=');
                        sb.Append(prop.Value);
                        sb.Append(';');
                    }

                    connection_string = sb.ToString();
                    worksheets = new List<DataTable>();
                }
            }

            if (connection_string != null)
            {
                // подключение в xlsx как к базе данных
                OleDbConnection connection = new OleDbConnection(connection_string);

                // открыть подключение
                connection.Open();

                // запрос структуры бызв данных
                System.Data.DataTable dtSheet = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow dr in dtSheet.Rows)
                {
                    // формирование команды выборки данных
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM [" + dr["TABLE_NAME"].ToString() + "]";
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                    DataTable datatable;

                    //выборка данных
                    datatable = new DataTable();
                    da.Fill(datatable);

                    string table_name = dr["TABLE_NAME"].ToString();
                    if (table_name.EndsWith("$\'") || table_name.EndsWith("$"))
                    {
                        foreach (DataRow drow in datatable.Rows)
                        {
                            int number_columns = drow.ItemArray.Count();
                            for (int i = 0; i < number_columns; i++)
                            {
                                if (drow.ItemArray[i] != DBNull.Value)
                                    if (drow.ItemArray[i].ToString().ToLower().Contains("идентификатор верификационной процедуры"))
                                    {
                                        worksheets.Add(datatable);
                                        break;
                                    }
                            }
                        }
                    }
                }

                // закрыть подключение
                connection.Close();
            }

        }


        #endregion


        /// <summary>
        /// Вызов диалогового окна выбора xls файла
        /// </summary>
        private void get_path_to_svr()
        {
            System.Windows.Forms.OpenFileDialog fg = new System.Windows.Forms.OpenFileDialog();
            fg.InitialDirectory = Directory.GetCurrentDirectory();
            fg.Filter = "Excel files (*.xls, *.xlsx) | *.xls; *.xlsx";
            if (fg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                path = fg.FileName;
        }
    }
}
