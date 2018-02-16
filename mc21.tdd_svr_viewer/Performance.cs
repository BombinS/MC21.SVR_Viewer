using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class Performance
    {
        mc21.svr_viewer.Model.Main_Model m;

        [TestInitialize]
        public void Init() 
        {
            m = new svr_viewer.Model.Main_Model();
        }
        
        [TestMethod]
        public void TestMethod1()
        {
            DirectoryInfo di = new DirectoryInfo("D:\\mc21_new\\Testing_Procedures\\SWS_075\\Project_04\\Tests_for_1C\\");
            //DirectoryInfo di = new DirectoryInfo("c:\\Work\\mc21_new\\Testing_Procedures\\SWS_075\\Project_04\\Tests_for_1C\\");
            // DirectoryInfo di = new DirectoryInfo("c:\\Work\\mc21_new\\Testing_Procedures\\SWS_075\\");
            Assert.IsTrue(di.Exists);
            m.Testing_Procedures_Directory = di.FullName;
        }
    }
}
