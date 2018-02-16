using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model
{
    /// <summary>
    /// Вью модель отображения сравнения требований в SVR и тестовых процедурах
    /// </summary>
    public class Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model
    {

        private Query_by_Case added_item;

        // запрос на отображение списка идентификаторов верификационных примеров из SVR и TPr
        public List<Query_by_Case> Display_By_Cases {
            get 
            {
                List<Query_by_Case> to_display = new List<Query_by_Case>();

                foreach (string str in list_case_ids)
                {
                    // получение идентификатора верификационного примера
                    string svr_case_id = "";
                    if (list_svr_case_ids.Contains(str))
                        svr_case_id = str;
                    string tpr_case_id = "";
                    if (list_tpr_case_ids.Contains(str))
                        tpr_case_id = str;

                    // если идентификаторы приемов определены, анализ затрассированных требований
                    if (svr_case_id.Equals("") == false && tpr_case_id.Equals("") == false)
                    {
                        // получить список требований SVR
                        List<string> svr_requirements = m.Results_by_Case.Where(x => x.ID.Equals(svr_case_id)).First().Reqs;

                        // получить список требований TPr
                        List<string> tpr_requirements = m.Procedures.Cases_Infos.Where(x => x.Name.Equals(tpr_case_id)).First().Requirements;

                        // проанализировать списки
                        Requirements_Analisys analisys = new Requirements_Analisys(svr_requirements, tpr_requirements);

                        // создать экземпляр отображения
                        added_item = new Query_by_Case(svr_case_id, tpr_case_id, analisys);
                    }
                    else
                        added_item = new Query_by_Case(svr_case_id, tpr_case_id);

                    to_display.Add(added_item);
                }
                return to_display;
            }
            set { } 
        }
        
        // ссылка на модель
        private Model.Main_Model m;

        private List<string> list_case_ids;
        private List<string> list_svr_case_ids;
        private List<string> list_tpr_case_ids;

#region Конструктор

        public Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model() 
        {
            // инициализировать ссылку на модель
            m = Main_ViewModel.m;

            // получить список идентификаторов кейсов указанных как в SVR, так и TPr
            obtain_list_case_ids();
        }

#endregion

        // получение списка идентификаторов кейсов указанных как в SVR, так и TPr
        private void obtain_list_case_ids()
        {
            // идентификаторы кейсов указанные в svr
            IEnumerable<string> query_svr_case_ids = m.Results_by_Case.Select(x => x.ID);
            list_svr_case_ids = query_svr_case_ids.ToList();

            // идентификаторы кейсов указанные в tpr
            list_tpr_case_ids = new List<string>();
            if (m.Procedures != null)
            {
                foreach (Model.Testing_Procedure procedure_info in m.Procedures.Procedure_Infos)
                    foreach (Model.Testing_Case case_info in procedure_info.Cases)
                        list_tpr_case_ids.Add(case_info.Name);
            }

            // общий список
            list_case_ids = query_svr_case_ids.Union(list_tpr_case_ids.Select(x => x)).Distinct().OrderBy(x => x).ToList();

        }

    }
}

