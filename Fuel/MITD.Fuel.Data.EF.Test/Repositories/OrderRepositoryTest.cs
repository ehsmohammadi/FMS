using System;
using System.Linq;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MITD.Fuel.Data.EF.Test.Repositories
{
    [TestClass]
    public class OrderRepositoryTest
    {
        [TestMethod]
        public void Can_get_orders_by_filter()
        {
            try
            {
                using (var c = new DataContainer(DbConnectionHelper.GetConnectionString()))
                {
                    var g = new EFUnitOfWork(c);
                    var rep = new OrderRepository(g);
                    Order result = rep.First(d => d.Id > 0);
                }

            }
            catch (Exception ex)
            {
                
                throw;
            }
            


        }
    }
}