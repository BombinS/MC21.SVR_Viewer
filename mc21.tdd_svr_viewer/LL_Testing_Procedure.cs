using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mc21.svr_viewer.Model;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class LL_Testing_Procedure
    {
        [TestMethod]
        public void TestMethod1()
        {
            FileInfo fi = new FileInfo("..\\..\\..\\mc21.tdd_svr_viewer_test_data\\Testing_Procedures_01\\Directory\\Another_directory\\SCN_MC21_FWS_PJ31_PC001_side_1C.pts");
            Assert.IsTrue(fi.Exists);
            Testing_Procedure t_class = new Testing_Procedure(fi);

            // тип тестовой процедуры - системный
            Assert.AreEqual(Procedure_Type.System, t_class.Type);

            // имя тестовой процедуры - PROJ_31_PROC_001
            Assert.AreEqual("PROJ_31_PROC_001", t_class.Name);

            // тестовая процедура содержит описания десяти примеров
            Assert.AreEqual(10, t_class.Cases.Count);

            Testing_Case t_case;
            t_case = t_class.Cases[6];
            Assert.AreEqual("PROJ_31_PROC_001_CASE_007", t_case.Name);
            Assert.AreEqual(220, t_case.Row_Start);
            Assert.AreEqual(237, t_case.Row_End);

            t_case = t_class.Cases[0];
            Assert.AreEqual("PROJ_31_PROC_001_CASE_001", t_case.Name);
            Assert.AreEqual(85, t_case.Row_Start);
            Assert.AreEqual(101, t_case.Row_End);

            t_case = t_class.Cases[9];
            Assert.AreEqual("PROJ_31_PROC_001_CASE_010", t_case.Name);
            Assert.AreEqual(297, t_case.Row_Start);
            Assert.AreEqual(324, t_case.Row_End);

            // формат тест креатора. Нет тэга Tested requirent
            t_case = t_class.Cases[0];
            Assert.IsNotNull(t_case.Requirements);
            Assert.AreEqual(0, t_case.Requirements.Count);

            // формат тест креатора. требований нет
            t_case = t_class.Cases[2];
            Assert.IsNotNull(t_case.Requirements);
            Assert.AreEqual(0, t_case.Requirements.Count);

            // формат тест креатора. нет тэга PCR
            t_case = t_class.Cases[3];
            Assert.IsNotNull(t_case.Requirements);
            Assert.AreEqual(0, t_case.Requirements.Count);

            // формат тест креатора. два требования
            t_case = t_class.Cases[5];
            Assert.IsNotNull(t_case.Requirements);
            Assert.AreEqual(2, t_case.Requirements.Count);
            Assert.AreEqual("MC21.FWS.CAS.REQ.0007", t_case.Requirements[0]);
            Assert.AreEqual("MC21.FWS.CAS.REQ.2007", t_case.Requirements[1]);

            // формат тест креатора. PCR выше требований
            t_case = t_class.Cases[6];
            Assert.IsNotNull(t_case.Requirements);
            Assert.AreEqual(0, t_case.Requirements.Count);

        }

        [TestMethod]
        public void TestMethod2()
        {
            FileInfo fi = new FileInfo("..\\..\\..\\mc21.tdd_svr_viewer_test_data\\Testing_Procedures_01\\Directory\\Another_directory\\SCN_MC21_IDS_GPSY_PJ02_PC004_side_OL.pts");
            Assert.IsTrue(fi.Exists);
            Testing_Procedure t_class = new Testing_Procedure(fi);

            // тип тестовой процедуры - системный
            Assert.AreEqual(Procedure_Type.System, t_class.Type);

            // имя тестовой процедуры - PROJ_02_PROC_004
            Assert.AreEqual("PROJ_02_PROC_004", t_class.Name);

            // тестовая процедура содержит описания пяти примеров
            Assert.AreEqual(5, t_class.Cases.Count);

            // два требования строкой ниже тэга
            Testing_Case t_case;
            t_case = t_class.Cases[0];
            Assert.AreEqual("PROJ_02_PROC_004_CASE_001", t_case.Name);
            Assert.AreEqual(47, t_case.Row_Start);
            Assert.AreEqual(89, t_case.Row_End);
            Assert.AreEqual(2, t_case.Requirements.Count);
            Assert.AreEqual(t_case.Requirements[0], "MC21.IDS.SRD.GPSY.REQ.0004");
            Assert.AreEqual(t_case.Requirements[1], "MC21.IDS.SRD.GPSY.REQ.0005");

            // три требования в строке тэга и два ниже по одному в каждой строке
            t_case = t_class.Cases[1];
            Assert.AreEqual("PROJ_02_PROC_004_CASE_002", t_case.Name);
            Assert.AreEqual(93, t_case.Row_Start);
            Assert.AreEqual(139, t_case.Row_End);
            Assert.AreEqual(3, t_case.Requirements.Count);
            Assert.AreEqual(t_case.Requirements[0], "MC21.IDS.SRD.GPSY.REQ.0004");
            Assert.AreEqual(t_case.Requirements[1], "MC21.IDS.SRD.GPSY.REQ.0006");
            Assert.AreEqual(t_case.Requirements[2], "MC21.IDS.SRD.GPSY.REQ.0010");

            // два тэга требований
            t_case = t_class.Cases[2];
            Assert.AreEqual("PROJ_02_PROC_004_CASE_003", t_case.Name);
            Assert.AreEqual(143, t_case.Row_Start);
            Assert.AreEqual(196, t_case.Row_End);
            Assert.AreEqual(0, t_case.Requirements.Count);

            // тэг СПИ  над тэгом требований
            t_case = t_class.Cases[4];
            Assert.AreEqual("PROJ_02_PROC_004_CASE_005", t_case.Name);
            Assert.AreEqual(256, t_case.Row_Start);
            Assert.AreEqual(312, t_case.Row_End);
            Assert.AreEqual(0, t_case.Requirements.Count);

        }



    }
}
