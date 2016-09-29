using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Transactions;
using MITD.Fuel.Data.EF.Context;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Data.EF.Test
{
    [TestClass]
    public class BasicInfoMappingTests
    {
        private TransactionScope scope;

        [TestInitialize]
        public void InitTest()
        {
            scope = new TransactionScope();
        }

        //================================================================================

        [TestMethod]
        public void TestAllBasicInfoViews()
        {
            using (var ctx = new DataContainer())
            {
                var nc = ctx.Companies.ToList();

                var vic = ctx.VesselsInInventory.ToList();

                var gc = ctx.SharedGoods.ToList();

                var tc = ctx.Tanks.ToList();

                var uc = ctx.Units.ToList();

                var cc = ctx.Currencies.ToList();
            }
        }

        //================================================================================
        
        [TestCleanup]
        public void Cleanup()
        {
            scope.Dispose();
        }

        //================================================================================
    }
}
