using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mc21.svr_viewer.ViewModel
{
    public class Display_Result
    {
        /// <summary>
        /// Статус верификационного примера
        /// </summary>
        public Model.Test_Case_Status Status { get; set; }
        
        /// <summary>
        /// Позиция выполнения верификационного примера
        /// </summary>
        public string Side { get; set; }
        
        /// <summary>
        /// Идентификатор верификационного примера
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Результат выполнения верификационного примера
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Описание несоответсвия
        /// </summary>
        public string PCR { get; set; }

        /// <summary>
        /// Дата выполнения верификационного примера
        /// </summary>
        public string Date { get; set; }

    }
}
