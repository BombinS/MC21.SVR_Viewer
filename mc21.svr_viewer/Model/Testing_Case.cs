using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mc21.svr_viewer.Model
{
    /// <summary>
    /// Класс описания верификационного примера
    /// </summary>
    public class Testing_Case
    {
        // имя 
        public string Name { get; set; }

        // первая строка
        public int Row_Start { get; set; }

        // последняя строка
        public int Row_End { get; set; }

        // затрассированные требования
        public List<string> Requirements { get; set; }

        // путь к файлу процедуры
        public string Path { get; set; }

    }
}
