using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MITD.Fuel.Application.Test.FuelReportTests
{
    [TestClass]
    public class VoyageFacadeServiceTests
    {
        #region Test General Variables

        private FuelReportTestObjectsContainer testObjects;
        private TransactionScope transactionScope;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            this.testObjects = new FuelReportTestObjectsContainer();

            //this.transactionScope = new TransactionScope();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //this.transactionScope.Dispose();
        }

        [TestMethod]
        public void TestGetAll()
        {
            #region Arrange

            #endregion

            #region Action

            var voyages = this.testObjects.VoyageFacadeService.GetAll(includeFuelReports: true);

            #endregion

            #region Assert

            Assert.IsNotNull(voyages.FirstOrDefault(v => v.Id == 1).EndOfVoyageFuelReport);

            #endregion
        }

        [TestMethod]
        public void TestGetVoyageLogs()
        {
            #region Arrange

            #endregion

            #region Action

            var voyagesResultPage = this.testObjects.VoyageFacadeService.GetChenageHistory(1, 10, 0);

            #endregion

            #region Assert

            Assert.IsNotNull(voyagesResultPage);

            Assert.IsNotNull(voyagesResultPage.Result);

            Assert.IsTrue(voyagesResultPage.Result.Count > 0);

            #endregion
        }
    }
}
