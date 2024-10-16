using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pruebas
{
    [TestClass]
    public class TestCallBackService
    {

        private static ServiceHost serviceHost;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            serviceHost = new ServiceHost(typeof(CalculatorService.ImplementacionServicio));
            serviceHost.Open();
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            serviceHost.Close();
        }
        [TestMethod]
        public void TestCallBackServiceSendOperation()
        {
            




        }
    }
}
