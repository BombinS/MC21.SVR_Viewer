using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace mc21.svr_viewer.Model
{
    /// <summary>
    ///  Класс коллекция описаний верификационных примеров
    /// </summary>
    public class Testing_Procedures
    {

#region Свойства

        // Коллекция файловых описаний верификационных процедур
        public List<FileInfo> Procedure_File_Infos { get; set; }

        // Коллеция описаний верификационных процедур
        public List<Testing_Procedure> Procedure_Infos { get; set; }

        // Коллеция описаний верификационных примеров
        public List<Testing_Case> Cases_Infos { get; set; }

#endregion

#region Конструктор

        public Testing_Procedures(string path) 
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // инициализировать директорию тестовых процедур
            DirectoryInfo di = new DirectoryInfo(path);

            //  если директория существует
            if (di.Exists)
            {
                // получить коллекцию файловых описаний верификационных процедур
                Procedure_File_Infos = obtain_procedures_file_info(di);

                // получить коллекцию описаний верификационных процедур
                Procedure_Infos = new List<Testing_Procedure>();
                foreach (FileInfo fi in Procedure_File_Infos)
                    Procedure_Infos.Add(new Testing_Procedure(fi));

                // получить коллекцию описаний верификационных примеров
                Cases_Infos = new List<Testing_Case>();
                foreach (Testing_Procedure tpr in Procedure_Infos)
                    foreach (Testing_Case tprc in tpr.Cases)
                        Cases_Infos.Add(tprc);
            }

            sw.Stop();
            Diagnostics.Timestamps.parsing_testing_procedures_general_time_2 = sw.ElapsedMilliseconds;
        }

#endregion

#region Приватные функции

        // получение коллекции файловых описаний верификационных процедур
        private List<FileInfo> obtain_procedures_file_info(DirectoryInfo testing_procedures_directory)
        {
            List<FileInfo> all_file_infos = new List<FileInfo>();
            IEnumerable<FileInfo> pts_file_infos = testing_procedures_directory.EnumerateFiles("*.pts", SearchOption.AllDirectories);
            IEnumerable<FileInfo> ptu_file_infos = testing_procedures_directory.EnumerateFiles("*.ptu", SearchOption.AllDirectories);
            foreach (FileInfo fi in pts_file_infos)
                all_file_infos.Add(fi);
            foreach (FileInfo fi in ptu_file_infos)
                all_file_infos.Add(fi);
            return all_file_infos;
        }

#endregion

    }
}
