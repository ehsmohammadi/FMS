using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MITD.Fuel.Application.Facade;

namespace MITD.Fuel.Application.Test
{
    [TestClass]
    public class FuelReportFacadeServiceTests
    {
        private TransactionScope transactionScope;

        private FuelReportFacadeService target;

        [TestInitialize]
        public void TestInitialize()
        {

            //target = new FuelReportFacadeService();

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
