using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using mc21.svr_viewer.Model;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class LL_Verification_Result_by_Case
    {
        Verification_Results_by_Case t_class;


        [TestMethod]
        public void TestMethod1()
        {
            t_class = new Verification_Results_by_Case();

            t_class.ID = null;
            Assert.IsNotNull(t_class.Procedure_ID);

            t_class.ID = "test";
            Assert.AreEqual("", t_class.Procedure_ID);

            t_class.ID = "test_proc";
            Assert.AreEqual("", t_class.Procedure_ID);

            t_class.ID = "test_proc_0";
            Assert.AreEqual("", t_class.Procedure_ID);

            t_class.ID = "test_proc_001";
            Assert.AreEqual("", t_class.Procedure_ID);

            t_class.ID = "test_proc_001_";
            Assert.AreEqual("test_proc_001", t_class.Procedure_ID);

            t_class.ID = "test_PROC_1_122134";
            Assert.AreEqual("test_PROC_1", t_class.Procedure_ID);
        }

        [TestMethod]
        public void TestMethod2()
        {
            t_class = new Verification_Results_by_Case();

            t_class.Reqs = null;
            Assert.AreEqual(false,t_class.is_Requirement_traced("test"));

            t_class.Reqs = new List<string>();
            Assert.AreEqual(false, t_class.is_Requirement_traced("test"));

            t_class.Reqs.Add("test1");
            t_class.Reqs.Add("test2");
            Assert.AreEqual(false, t_class.is_Requirement_traced("test"));
            Assert.AreEqual(true, t_class.is_Requirement_traced("test1"));
            Assert.AreEqual(true, t_class.is_Requirement_traced("test2"));

        }

    }
}
