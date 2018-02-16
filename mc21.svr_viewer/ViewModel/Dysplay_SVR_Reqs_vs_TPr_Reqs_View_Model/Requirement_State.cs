using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model
{
    
    public class Requirement_State
    {
        public string Requirement_ID { get; set; }

        public Requirement_States State { get; set; }
    }


    public enum Requirement_States
    {
        Identical,
        Unique
    }
}
