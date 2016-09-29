using System;
using System.Linq;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.DomainObjects;
using NUnit.Framework;

namespace MITD.Fuel.Data.EF.Test.MappingTests
{
    [TestFixture]
    public class OrderMappingTest
    {
        [TestFixtureSetUp]
        public void TestSetup()
        {
            // HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
        }

        [Test]
        public void Can_include_related_fromVessel_in_order_query()
        {
            using (var rep = new DataContainer(DbConnectionHelper.GetConnectionString()))
            {

                var data = rep.Orders
                    .Include("FromVesselInCompany")
                    .Where(o => o.FromVesselInCompanyId.HasValue).ToList();//

                Assert.That(data.Count > 0);

                var ent = data.First();
                Assert.That(ent.FromVesselInCompany != null);
                Assert.That(ent.FromVesselInCompany.Id == ent.FromVesselInCompanyId.Value);

                Assert.That(!string.IsNullOrEmpty(ent.FromVesselInCompany.Code));
                Assert.That(!string.IsNullOrEmpty(ent.FromVesselInCompany.Name));
            }
        }

        [Test]
        public void Can_include_related_toVessel_in_order_query()
        {
            using (var rep = new DataContainer(DbConnectionHelper.GetConnectionString()))
            {

                var data = rep.Orders
                    .Include("toVessel")
                    .Where(o => o.FromVesselInCompanyId.HasValue).ToList();//


                Assert.That(data.Count > 0);

                var ent = data.First();
                Assert.That(ent.ToVesselInCompany != null);
                Assert.That(ent.ToVesselInCompany.Id == ent.ToVesselInCompanyId.Value);

                Assert.That(!string.IsNullOrEmpty(ent.ToVesselInCompany.Code));
                Assert.That(!string.IsNullOrEmpty(ent.ToVesselInCompany.Name));
            }
        }
    }
}