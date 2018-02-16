using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model
{
    public class Query_by_Case
    {

        public string Case_Id { get; set; }

        public string SVR_Case_Id { get; set; }

        public List<Requirement_State> SVR_Case_Requirements { get; set; }

        public string TPr_Case_Id { get; set; }

        public List<Requirement_State> TPr_Case_Requirements { get; set; }

        public Case_ID_Statuses Case_ID_Status { get; set; }

        public Query_by_Case(string svr_case_id, string tpr_case_id) 
        {
            SVR_Case_Id = svr_case_id;
            SVR_Case_Requirements = new List<Requirement_State>();
            TPr_Case_Id = tpr_case_id;
            TPr_Case_Requirements = new List<Requirement_State>();
            if (SVR_Case_Id.Equals(""))
            {
                Case_Id = TPr_Case_Id;
                Case_ID_Status = Case_ID_Statuses.Absent_at_SVR;
            }
            else
            {
                Case_Id = SVR_Case_Id;
                Case_ID_Status = Case_ID_Statuses.Absent_at_TPr;
            }
        }

        public Query_by_Case(string svr_case_id, string tpr_case_id, Requirements_Analisys analisys)
        {
            Case_Id = svr_case_id;
            SVR_Case_Id = svr_case_id;
            SVR_Case_Requirements = analisys.SVR_Requirements;
            TPr_Case_Id = tpr_case_id;
            TPr_Case_Requirements = analisys.TPR_Requirements;
            Case_ID_Status = Case_ID_Statuses.Full;
        }

        public Query_by_Case() 
        {
        }
    }

    public enum Case_ID_Statuses
    {
        Undefined,
        Full,
        Absent_at_SVR,
        Absent_at_TPr
    }
}
