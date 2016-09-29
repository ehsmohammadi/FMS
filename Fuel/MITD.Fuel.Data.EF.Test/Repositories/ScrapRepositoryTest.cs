using System;
using System.Linq;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Data.EF.Test.Repositories
{
    [TestClass]
    public class ScrapRepositoryTest
    {
        [TestMethod]
        public void Get()
        {
            using (var c = new DataContainer(DbConnectionHelper.GetConnectionString()))
            {
                var g = new EFUnitOfWork(c);
                //var rep = new ScrapRepository(g, new ScrapConfigurator(new ScrapStateFactory(new ApprovableScrapDomainService(new VesselDomainService))));

                
                //var res = rep.Find(r => true);
            }



        }
    }
}