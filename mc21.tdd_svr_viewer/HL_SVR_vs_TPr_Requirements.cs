using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class HL_SVR_vs_TPr_Requirements
    {

        private mc21.svr_viewer.Model.Main_Model m;
        private mc21.svr_viewer.ViewModel.Main_ViewModel main_vm;
        private mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model vm;

        FileInfo fi;
        DirectoryInfo di;

        [TestInitialize]
        public void Init() 
        {
            m = new svr_viewer.Model.Main_Model();
            main_vm = new svr_viewer.ViewModel.Main_ViewModel(m);
            vm = new svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model();
        }

        [TestMethod]
        public void TestMethod1()
        {
            // загрузить svr
            fi = new FileInfo(Directory.GetCurrentDirectory() + "\\..\\..\\..\\mc21.tdd_svr_viewer_test_data\\For_SVR_TPr_Requirements_01\\test_svr.xls");
            Assert.IsTrue(fi.Exists);
            mc21.svr_viewer.Excel.Load_SVR loaded_svr = new svr_viewer.Excel.Load_SVR(fi.FullName);
            foreach (System.Data.DataTable dt in loaded_svr.Worksheets)
                m.Fill_Model(dt);
            m.Obtain_Statictics();

            // загрузить верификационные примеры
            di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\..\\..\\..\\mc21.tdd_svr_viewer_test_data\\For_SVR_TPr_Requirements_01\\");
            Assert.IsTrue(di.Exists);
            m.Testing_Procedures_Directory = di.FullName;

            // создать вью модель отображения затрассированных требований
            vm = new svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model();

            // сериализация
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model));
            TextWriter tw = new StreamWriter("SVR_Reqs_vs_TPr_Reqs_01.xml");
            ser.Serialize(tw, vm);
            tw.Close();

            // 150 уникальных идентификаторов верификационных примеров
            Assert.AreEqual(150, vm.Display_By_Cases.Count);

             Query_by_Case query = vm.Display_By_Cases.Find(x => x.Case_Id.Equals("PROJ_05_PROC_008_CASE_001"));
             Assert.IsNotNull(query);

        }

        [TestMethod]
        public void TestMethod2()
        {
            // загрузить svr
            fi = new FileInfo(Directory.GetCurrentDirectory() + "\\..\\..\\..\\mc21.tdd_svr_viewer_test_data\\For_SVR_TPr_Requirements_02\\test_svr.xls");
            Assert.IsTrue(fi.Exists);
            mc21.svr_viewer.Excel.Load_SVR loaded_svr = new svr_viewer.Excel.Load_SVR(fi.FullName);
            foreach (System.Data.DataTable dt in loaded_svr.Worksheets)
                m.Fill_Model(dt);
            m.Obtain_Statictics();

            // загрузить верификационные примеры
            di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\..\\..\\..\\mc21.tdd_svr_viewer_test_data\\For_SVR_TPr_Requirements_02\\");
            Assert.IsTrue(di.Exists);
            m.Testing_Procedures_Directory = di.FullName;

            // создать вью модель отображения затрассированных требований
            vm = new svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model();

            // сериализация
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model));
            TextWriter tw = new StreamWriter("SVR_Reqs_vs_TPr_Reqs_02.xml");
            ser.Serialize(tw, vm);
            tw.Close();

            // 16 уникальных идентификаторов верификационных примеров
            Assert.AreEqual(16, vm.Display_By_Cases.Count);
        }

    }
}
