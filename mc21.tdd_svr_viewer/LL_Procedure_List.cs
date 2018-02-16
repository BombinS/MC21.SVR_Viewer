using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mc21.svr_viewer.ViewModel;
using System.Collections.Generic;

namespace mc21.tdd_svr_viewer
{
    [TestClass]
    public class LL_Procedure_List
    {
        Procedure_List tested_class;
        List<Display_Result> input_data;

        /// <summary>
        /// На входе не инициализированная коллекция верификационных примеров
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            input_data = null;
            tested_class = new Procedure_List(input_data);

            // коллекция наименований процедур проинициализирована
            Assert.IsNotNull(tested_class.Names_Of_Procedures);

            // количество процедур равно нулю
            Assert.AreEqual(0, tested_class.Amount_of_Procedures);
        }

        /// <summary>
        /// На входе один валидный результат выполнения верификационного примера
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            input_data = new List<Display_Result>();
            input_data.Add(new Display_Result { ID = "PROJ_02_PROC_296_CASE_002", Date = "", PCR = "", Result = "", Side = "" });

            tested_class = new Procedure_List(input_data);

            // количество идентификаторов процедур равно 1
            Assert.AreEqual(1, tested_class.Amount_of_Procedures);

            // идентификатор процедуры должен быть равен PROJ_02_PROC_296
            Assert.AreEqual("PROJ_02_PROC_296",tested_class.Names_Of_Procedures[0]);
        }

        /// <summary>
        /// На входе один результат выполнения верификационного примера c неинициализированными свойствами
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            input_data = new List<Display_Result>();
            input_data.Add(new Display_Result { ID = null, Date = null, PCR = null, Result = null, Side = null });

            tested_class = new Procedure_List(input_data);

            // количество идентификаторов процедур равно 0
            Assert.AreEqual(0, tested_class.Amount_of_Procedures);
        }

        /// <summary>
        /// На входе два валидных результата выполнения верификационного примера одной процедуры
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            input_data = new List<Display_Result>();
            input_data.Add(new Display_Result { ID = "PROJ_02_PROC_296_CASE_002", Date = "", PCR = "", Result = "", Side = "" });
            input_data.Add(new Display_Result { ID = "PROJ_02_PROC_296_CASE_001", Date = "", PCR = "", Result = "", Side = "" });

            tested_class = new Procedure_List(input_data);

            // количество идентификаторов процедур равно 1
            Assert.AreEqual(1, tested_class.Amount_of_Procedures);

            // идентификатор процедуры должен быть равен PROJ_02_PROC_296
            Assert.AreEqual("PROJ_02_PROC_296", tested_class.Names_Of_Procedures[0]);
        }


        /// <summary>
        /// На входе инициализированные но невалидные результаты выполнения верификационного примеров
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            input_data = new List<Display_Result>();
            input_data.Add(new Display_Result { ID = "PROJ_02_PROC_", Date = "", PCR = "", Result = "", Side = "" });
            input_data.Add(new Display_Result { ID = "PROJ_02_PROC296001", Date = "", PCR = "", Result = "", Side = "" });

            tested_class = new Procedure_List(input_data);

            // количество идентификаторов процедур равно 0
            Assert.AreEqual(0, tested_class.Amount_of_Procedures);

        }

    }
}
