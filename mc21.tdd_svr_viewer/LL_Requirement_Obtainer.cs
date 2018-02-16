using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class LL_Requirement_Obtainer
    {

        private string full_path;
        private string file_path;

        private mc21.svr_viewer.Model.Main_Model m;
        private mc21.svr_viewer.ViewModel.Main_ViewModel vm;

        private mc21.svr_viewer.Model.Verification_Results_by_Case result;

        private string obtain_test_path(string path)
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\..\\mc21.tdd_svr_viewer_test_data\\" + path;
        }

        [TestInitialize]
        public void Init()
        {
            m = new svr_viewer.Model.Main_Model();
            vm = new svr_viewer.ViewModel.Main_ViewModel(m);
        }
        
        [TestMethod]
        public void TestMethod1()
        {
            file_path = "reqs.xls";
            full_path = obtain_test_path(file_path);
            Assert.IsTrue(new FileInfo(full_path).Exists);

            // загрузка книги svr
            mc21.svr_viewer.Excel.Load_SVR loaded_svr = new svr_viewer.Excel.Load_SVR(full_path);

            // загрузка данных svr
            foreach (System.Data.DataTable dt in loaded_svr.Worksheets)
                m.Fill_Model(dt);

            // в модели должно существовать определение примера PROJ_01_PROC_001_CASE_001
            result = m.Results_by_Case.Find(x => x.ID.Contains("PROJ_01_PROC_001_CASE_001"));
            Assert.IsNotNull(result);
            
            // свойство требования при пустой ячейке должно быть проинициализировано 
            Assert.IsNotNull(result.Reqs);

            // в модели должно существовать определение примера PROJ_01_PROC_001_CASE_002
            result = m.Results_by_Case.Find(x => x.ID.Contains("PROJ_01_PROC_001_CASE_002"));
            // для примера PROJ_01_PROC_001_CASE_002 должно быть прилинковано одно требование
            Assert.AreEqual(1, result.Reqs.Count);
            // первый идентификатор требования примера PROJ_01_PROC_001_CASE_002 должен быть reqs1
            Assert.AreEqual("reqs1", result.Reqs[0]);

            // в модели должно существовать определение примера PROJ_01_PROC_001_CASE_003
            result = m.Results_by_Case.Find(x => x.ID.Contains("PROJ_01_PROC_001_CASE_003"));
            // для примера PROJ_01_PROC_001_CASE_003 должно быть прилинковано два требования
            Assert.AreEqual(2, result.Reqs.Count);
            // первый идентификатор требования примера PROJ_01_PROC_001_CASE_003 должен быть reqs1
            Assert.AreEqual("reqs1", result.Reqs[0]);
            // второй идентификатор требования примера PROJ_01_PROC_001_CASE_003 должен быть reqs2
            Assert.AreEqual("reqs2", result.Reqs[1]);

            // в модели должно существовать определение примера PROJ_01_PROC_001_CASE_004
            result = m.Results_by_Case.Find(x => x.ID.Contains("PROJ_01_PROC_001_CASE_004"));
            // для примера PROJ_01_PROC_001_CASE_004 должно быть прилинковано два требования
            Assert.AreEqual(2, result.Reqs.Count);
            // первый идентификатор требования примера PROJ_01_PROC_001_CASE_004 должен быть reqs1
            Assert.AreEqual("reqs1", result.Reqs[0]);
            // второй идентификатор требования примера PROJ_01_PROC_001_CASE_004 должен быть reqs2
            Assert.AreEqual("reqs2", result.Reqs[1]);

            // в модели должно существовать определение примера PROJ_01_PROC_001_CASE_005
            result = m.Results_by_Case.Find(x => x.ID.Contains("PROJ_01_PROC_001_CASE_005"));
            // для примера PROJ_01_PROC_001_CASE_005 должно быть прилинковано четыре требования
            Assert.AreEqual(4, result.Reqs.Count);
            Assert.AreEqual("reqs1", result.Reqs[0]);
            Assert.AreEqual("reqs2", result.Reqs[1]);
            Assert.AreEqual("reqs3", result.Reqs[2]);
            Assert.AreEqual("reqs5", result.Reqs[3]);

        }
    }
}
