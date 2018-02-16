using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mc21.svr_viewer;
using System.IO;
using System.Xml.Serialization;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class HL
    {
        private string full_path;
        private string file_path;

        private mc21.svr_viewer.Model.Main_Model m;
        private mc21.svr_viewer.ViewModel.Main_ViewModel vm;
        private mc21.svr_viewer.ViewModel.Dysplay_SVR_View_Model.Dysplay_SVR_View_Model vm_dysplay_svr;

        [TestInitialize]
        public void Init() 
        {
            m = new svr_viewer.Model.Main_Model();
            vm = new svr_viewer.ViewModel.Main_ViewModel(m);
        }

        [TestMethod]
        public void TestMethod1()
        {
            file_path = "simple_svr_one_row_one_test.xls";
            full_path = obtain_test_path(file_path);
            Assert.IsTrue(new FileInfo(full_path).Exists);

            // загрузка книги svr
            mc21.svr_viewer.Excel.Load_SVR loaded_svr = new svr_viewer.Excel.Load_SVR( full_path);

            // в книге svr должно быть 14 листов
            Assert.AreEqual(14, loaded_svr.Worksheets.Count);

            // загрузка данных svr
            foreach (System.Data.DataTable dt in loaded_svr.Worksheets)
                m.Fill_Model(dt);

            // получение статистики по данным svr
            m.Obtain_Statictics();
            // инициализация фильтров

            // число позиций в модели должно быть равно 6
            Assert.AreEqual(6, m.Positions.Count);
            
            // инициализация вью модели отображения SVR
            vm_dysplay_svr = new svr_viewer.ViewModel.Dysplay_SVR_View_Model.Dysplay_SVR_View_Model();            

            // число позиций во вьюмодели должно быть равно 6
            Assert.AreEqual(6, vm_dysplay_svr.Query_Dysplay_Checkboxes_Positions.Count);

            // после инициализации фильтров все позиции должын быть включены
            bool isPositionCheckBoxesTurnON = true;
            foreach (mc21.svr_viewer.ViewModel.Checkbox checkbox in vm_dysplay_svr.Query_Dysplay_Checkboxes_Positions)
            {
                if (checkbox.isChecked == false)
                    isPositionCheckBoxesTurnON = false;
                break;
            }
            Assert.IsTrue(isPositionCheckBoxesTurnON);

            // число вариантов результатов выполнения должно быть равно 3
            Assert.AreEqual(3, vm_dysplay_svr.Query_Dysplay_Checkboxes_Results.Count);

            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(mc21.svr_viewer.ViewModel.Main_ViewModel));
            TextWriter tw = new StreamWriter("blend_data_1.xml");
            ser.Serialize(tw, vm);
            tw.Close();
        }

        [TestMethod]
        public void TestMethod2()
        {
            file_path = "test_cases_statuses.xls";
            full_path = obtain_test_path(file_path);
            Assert.IsTrue(new FileInfo(full_path).Exists);

            // загрузка книги svr
            mc21.svr_viewer.Excel.Load_SVR loaded_svr = new svr_viewer.Excel.Load_SVR(full_path);

            // в книге svr должно быть 3 листа
            Assert.AreEqual(3, loaded_svr.Worksheets.Count);

            // загрузка данных svr
            foreach (System.Data.DataTable dt in loaded_svr.Worksheets)
                m.Fill_Model(dt);

            // получение статистики по данным svr
            m.Obtain_Statictics();

            // инициализация вью модели отображения SVR
            vm_dysplay_svr = new svr_viewer.ViewModel.Dysplay_SVR_View_Model.Dysplay_SVR_View_Model();            

            // число отображаемых результатов должно быть равно 21
            Assert.AreEqual(21, vm_dysplay_svr.Query_Display_Results.Count);
            Assert.AreEqual(21, vm_dysplay_svr.Amount_Displayed_Results_by_Case);

            svr_viewer.ViewModel.Display_Result r;

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_001_CASE_001");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.OK, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_002_CASE_001");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.OK_PCR_Exist, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_003_CASE_001");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.OK_PCR_Exist, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_004_CASE_001");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.OK_PCR_Exist, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_005_CASE_001");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.KO, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_021_CASE_001");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.KO, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_021_CASE_002");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.KO_Bag_Report_Exist, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_021_CASE_003");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.KO_PCR_Exist, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_021_CASE_004");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.NA, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_021_CASE_005");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.KO, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_021_CASE_006");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.NA_Bag_Report_Exist, r.Status);

            r = get_result(vm_dysplay_svr.Query_Display_Results, "PROJ_01_PROC_021_CASE_007");
            Assert.AreEqual(svr_viewer.Model.Test_Case_Status.NA_PCR_Exist, r.Status);


            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(mc21.svr_viewer.ViewModel.Main_ViewModel));
            TextWriter tw = new StreamWriter("blend_data_2.xml");
            ser.Serialize(tw, vm);
            tw.Close();
        }

        private svr_viewer.ViewModel.Display_Result get_result(System.Collections.Generic.List<svr_viewer.ViewModel.Display_Result> list, string str)
        {
            foreach (var item in list)
                if (item.ID.Contains(str))
                    return item;
            return null;
        }

        private string obtain_test_path(string path)
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\mc21.tdd_svr_viewer_test_data\\" + path;
        }

    }
}
