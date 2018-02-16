using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mc21.svr_viewer.Diagnostics
{
    public class Timestamps
    {
        public static long parsing_testing_procedures_general_time;

        public static long parsing_testing_procedures_general_time_2;

        public static long parsing_testing_procedures_obtain_files_info;

        public static void Write_Timestamps() 
        {
            StreamWriter sw = new StreamWriter("timestamps.info");
            sw.WriteLine("--1-------------------");
            sw.WriteLine("General generation time: " + parsing_testing_procedures_general_time.ToString());
            sw.WriteLine("Obtain file info: " + parsing_testing_procedures_obtain_files_info.ToString());
            sw.WriteLine();
            sw.WriteLine("--2-------------------");
            sw.WriteLine("General generation time 2: " + parsing_testing_procedures_general_time_2.ToString());
            sw.Close();
        }
    }
}
