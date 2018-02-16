using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model
{
    /// <summary>
    /// Класс сравнения двух коллекций строк (идентификаторов требований)
    /// </summary>
    public class Requirements_Analisys
    {
        public List<Requirement_State> SVR_Requirements { get; set; }

        public List<Requirement_State> TPR_Requirements { get; set; }

        public Requirements_Analisys(List<string> svr, List<string> tpr) 
        {

            SVR_Requirements = new List<Requirement_State>();
            TPR_Requirements = new List<Requirement_State>();

            if (svr != null && tpr != null) 
            {
                // если количество требований одинаково
                if (svr.Count == tpr.Count)

                    // вести сбалансированный анализ относительно svr
                    perform_balance_analisys(svr, tpr);

                // определить где требований больше
                else if (svr.Count > tpr.Count)
                {
                    // вести разбалансированный анализ относительно svr
                }
                else
                {
                    // вести разбалансированный анализ относительно tpr
                }

            }
        }

        // сбалансированный анализ относительно svr
        private void perform_balance_analisys(List<string> svr_list, List<string> tpr_list)
        {
            // создаем пулы
            List<string> pool_svr = new List<string>();
            List<string> pool_svr_unique = new List<string>();
            foreach (string item in svr_list)
                pool_svr.Add(item);
            List<string> pool_tpr = new List<string>();
            foreach (string item in tpr_list)
                pool_tpr.Add(item);

            // пока есть значения в пуле svr
            while (pool_svr.Count != 0)
            {
                // первое требование в пуле svr
                string requirement = pool_svr[0];

                int tpr_index = pool_tpr.IndexOf(requirement);
                // такое требование существует в пуле tpr
                if (tpr_index != -1)
                {
                    // создаем экземпляр найденного описания SVR
                    SVR_Requirements.Add(new Requirement_State { Requirement_ID = requirement, State = Requirement_States.Identical });

                    // создаем экземпляр найденного описания TPr
                    TPR_Requirements.Add(new Requirement_State { Requirement_ID = requirement, State = Requirement_States.Identical });

                    // убираем требование из пула tpr
                    pool_tpr.RemoveAt(tpr_index);
                }
                else
                {
                    // добавляем значение в пул уникальных требований svr
                    pool_svr_unique.Add(pool_svr[0].ToString());
                }
                
                // убираем требование из пула svr
                pool_svr.RemoveAt(0);
            }

            // пока есть значения в пуле уникальных значений svr
            while (pool_svr_unique.Count != 0)
            {
                // первое требование в пуле tpr
                string requirement = pool_svr_unique[0];

                // создаем экземпляр не найденного описания TPr
                SVR_Requirements.Add(new Requirement_State { Requirement_ID = requirement, State = Requirement_States.Unique });

                // убираем требование из пула tpr
                pool_svr_unique.RemoveAt(0);
            }

            // пока есть значения в пуле tpr
            while (pool_tpr.Count != 0) 
            {
                // первое требование в пуле tpr
                string requirement = pool_tpr[0];                

                // создаем экземпляр не найденного описания TPr
                TPR_Requirements.Add(new Requirement_State { Requirement_ID = requirement, State = Requirement_States.Unique });

                // убираем требование из пула tpr
                pool_tpr.RemoveAt(0);
            }
        }
    }
}
