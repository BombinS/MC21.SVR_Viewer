using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model;

namespace mc21.svr_viewer.View.View_Selectors
{
    public class SVR_Reqs_vs_TPr_Reqs_Row_Selector : DataTemplateSelector
    {
        public DataTemplate Full { get; set; }

        public DataTemplate SVR_miss { get; set; }

        public DataTemplate TPr_miss { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) 
        {
            if (item != null)
            {
                Case_ID_Statuses status = (item as Query_by_Case).Case_ID_Status;
                if (status == Case_ID_Statuses.Full)
                    return Full;
                if (status == Case_ID_Statuses.Absent_at_SVR)
                    return SVR_miss;
                if (status == Case_ID_Statuses.Absent_at_TPr)
                    return TPr_miss;
            }
            return base.SelectTemplate(item, container);

        }

    }
}
