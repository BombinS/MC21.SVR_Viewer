using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mc21.svr_viewer.Model
{
    public enum Test_Case_Status
    {

        OK,                  // кейс выполнен успешно
        OK_PCR_Exist,        // кейс выполнен успешно, но приведено СПИ
        NA,                  // кейс не выполнен
        NA_PCR_Exist,        // кейс не выполнен, но выпущено СПИ
        NA_Bag_Report_Exist, // кейс не выполнен, но открыто несоответствие в багзилле
        KO_PCR_Exist,        // кейс провалился, но выпущено СПИ
        KO_Bag_Report_Exist, // кейс провалился, но открыто несоответствие в багзилле
        KO,                  // кейс провалился, объяснений нет
        Not_Defined
    }
}
