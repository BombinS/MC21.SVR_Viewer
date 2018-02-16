using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mc21.svr_viewer.Model;
using System.IO;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class HL_Testing_Procedures
    {
        Main_Model m;
        string tdd_path;

        [TestInitialize]
        public void init() 
        {
            m = new Main_Model();
            tdd_path = Directory.GetCurrentDirectory() + "\\..\\..\\..\\mc21.tdd_svr_viewer_test_data\\";
        }

        [TestMethod]
        public void TestMethod1()
        {
            // модель должна содержать публичное свойство "путь к директории тестовых процедур"
            Assert.IsNotNull(m.Testing_Procedures_Directory);

            DirectoryInfo di = new DirectoryInfo(tdd_path + "Testing_Procedures_01");
            Assert.IsTrue(di.Exists);

            // модель должна принимать значение пути к директории тестовых процедур
            m.Testing_Procedures_Directory = di.FullName;
            Assert.AreEqual(m.Testing_Procedures_Directory, di.FullName); 

            // в модели должно быть описание пяти верификационных процедур
            Assert.AreEqual(5, m.Procedures.Procedure_Infos.Count);

        }
    }
}
