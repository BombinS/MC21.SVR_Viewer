using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using mc21.svr_viewer.ViewModel.Dysplay_SVR_Reqs_vs_TPr_Reqs_View_Model;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class LL_Requirements_Analisys
    {
        Requirements_Analisys t_class;
        List<string> arg1, arg2;


        // должна быть предусмотрена защита от null
        [TestMethod]
        public void TestMethod1()
        {
            arg1 = null;
            arg2 = null;
            t_class = new Requirements_Analisys(arg1,arg2);

            Assert.AreEqual(0, t_class.SVR_Requirements.Count);
            Assert.AreEqual(0, t_class.TPR_Requirements.Count);
        }

        // req1 req1 
        // req2 req2
        // req3 req3
        [TestMethod]
        public void TestMethod2()
        {
            arg1 = new List<string>();
            arg1.Add("req1");
            arg1.Add("req2");
            arg1.Add("req3");
            arg2 = new List<string>();
            arg2.Add("req1");
            arg2.Add("req2");
            arg2.Add("req3");

            t_class = new Requirements_Analisys(arg1, arg2);

            Assert.AreEqual(3, t_class.SVR_Requirements.Count);
            Assert.AreEqual(3, t_class.TPR_Requirements.Count);

            Assert.AreEqual("req1", t_class.SVR_Requirements[0].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.SVR_Requirements[0].State);
            Assert.AreEqual("req2", t_class.SVR_Requirements[1].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.SVR_Requirements[1].State);
            Assert.AreEqual("req3", t_class.SVR_Requirements[2].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.SVR_Requirements[2].State);

            Assert.AreEqual("req1", t_class.TPR_Requirements[0].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.TPR_Requirements[0].State);
            Assert.AreEqual("req2", t_class.TPR_Requirements[1].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.TPR_Requirements[1].State);
            Assert.AreEqual("req3", t_class.TPR_Requirements[2].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.TPR_Requirements[2].State);
        }

        // req1 req1 
        // req2 req4
        // req3 req3
        [TestMethod]
        public void TestMethod3()
        {
            arg1 = new List<string>();
            arg1.Add("req1");
            arg1.Add("req2");
            arg1.Add("req3");
            arg2 = new List<string>();
            arg2.Add("req1");
            arg2.Add("req4");
            arg2.Add("req3");

            t_class = new Requirements_Analisys(arg1, arg2);

            Assert.AreEqual(3, t_class.SVR_Requirements.Count);
            Assert.AreEqual(3, t_class.TPR_Requirements.Count);

            Assert.AreEqual("req1", t_class.SVR_Requirements[0].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.SVR_Requirements[0].State);
            Assert.AreEqual("req3", t_class.SVR_Requirements[1].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.SVR_Requirements[1].State);
            Assert.AreEqual("req2", t_class.SVR_Requirements[2].Requirement_ID);
            Assert.AreEqual(Requirement_States.Unique, t_class.SVR_Requirements[2].State);

            Assert.AreEqual("req1", t_class.TPR_Requirements[0].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.TPR_Requirements[0].State);
            Assert.AreEqual("req3", t_class.TPR_Requirements[1].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.TPR_Requirements[1].State);
            Assert.AreEqual("req4", t_class.TPR_Requirements[2].Requirement_ID);
            Assert.AreEqual(Requirement_States.Unique, t_class.TPR_Requirements[2].State);
        }

        // req1 req1 
        // req4 req2
        // req3 req3
        [TestMethod]
        public void TestMethod4()
        {
            arg1 = new List<string>();
            arg1.Add("req1");
            arg1.Add("req4");
            arg1.Add("req3");
            arg2 = new List<string>();
            arg2.Add("req1");
            arg2.Add("req2");
            arg2.Add("req3");

            t_class = new Requirements_Analisys(arg1, arg2);

            Assert.AreEqual(3, t_class.SVR_Requirements.Count);
            Assert.AreEqual(3, t_class.TPR_Requirements.Count);

            Assert.AreEqual("req1", t_class.SVR_Requirements[0].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.SVR_Requirements[0].State);
            Assert.AreEqual("req3", t_class.SVR_Requirements[1].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.SVR_Requirements[1].State);
            Assert.AreEqual("req4", t_class.SVR_Requirements[2].Requirement_ID);
            Assert.AreEqual(Requirement_States.Unique, t_class.SVR_Requirements[2].State);

            Assert.AreEqual("req1", t_class.TPR_Requirements[0].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.TPR_Requirements[0].State);
            Assert.AreEqual("req3", t_class.TPR_Requirements[1].Requirement_ID);
            Assert.AreEqual(Requirement_States.Identical, t_class.TPR_Requirements[1].State);
            Assert.AreEqual("req2", t_class.TPR_Requirements[2].Requirement_ID);
            Assert.AreEqual(Requirement_States.Unique, t_class.TPR_Requirements[2].State);
        }


    }
}
