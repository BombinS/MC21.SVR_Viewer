using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mc21.svr_viewer.Model
{
    public class Verification_Results_By_Part
    {
        public string SVCP_Part_Name { get; set; }

        public List<Verification_Results_by_Case> Results { get; set; }

        public Verification_Results_By_Part() 
        {
            SVCP_Part_Name = "";
            Results = new List<Verification_Results_by_Case>();
        }
    }
}
