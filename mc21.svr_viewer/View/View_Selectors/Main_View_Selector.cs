using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace mc21.svr_viewer.View.View_Selectors
{
    /// <summary>
    /// класс выбора дататемплейта главного окна
    /// </summary>
    public class Main_View_Selector : DataTemplateSelector
    {
        public DataTemplate empty_content { get; set; }

        public DataTemplate svr_content { get; set; }

        public DataTemplate svr_reqs_vs_tpr_reqs { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null) 
            {
                if ((View_Types)item == View_Types.Not_Defined)
                    return empty_content;
                if ((View_Types)item == View_Types.SVR_Content)
                    return svr_content;
                if ((View_Types)item == View_Types.SVR_Reqs_vs_TPr_Reqs)
                    return svr_reqs_vs_tpr_reqs;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
