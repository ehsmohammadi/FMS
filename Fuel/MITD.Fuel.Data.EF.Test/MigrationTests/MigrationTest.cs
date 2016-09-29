using System.Linq;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Migrations;
using NUnit.Framework;

namespace MITD.Fuel.Data.EF.Test.MigrationTests
{
    [TestFixture]
    public class MigrationTest
    {
        [TestFixtureSetUp]
        public void TestSetup()
        {
            // HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
        }

        [Test]
        public void TestMigration()
        {
            using (var rep = new DataContainer(DbConnectionHelper.GetConnectionString()))
            {
                var result = rep.VoyagesLog.Find(0);
            }
        }

        [Test]
        public void TestMigrationSeed()
        {
            //var seedMig = new Migration_Seed();
            //seedMig.Up();
        }


    }
}