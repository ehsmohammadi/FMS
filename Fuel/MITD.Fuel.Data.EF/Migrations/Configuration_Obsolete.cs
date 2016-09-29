using System.Reflection;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.FakeDomainServices;

namespace MITD.Fuel.Data.EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MITD.Fuel.Data.EF.Context.DataContainer>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContainer context)
        {
            //return;
            DbContextTransaction transaction = context.Database.BeginTransaction();

            try
            {
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                context.Database.Connection.Close();

                //Database.Delete("DataContainer");
                throw ex;

            }
        }

        private static T Construct<T>(Type[] paramTypes, object[] paramValues)
        {
            //A method to Construct objects via private constructors to bypass business rules checkings.
            Type t = typeof(T);

            ConstructorInfo ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, paramTypes, null);

            return (T)ci.Invoke(paramValues);
        }

        private static T Construct<T>(params object[] paramValues)
        {
            //A method to Construct objects via private constructors to bypass business rules checkings.
            var paramTypes = paramValues.Select(v => v.GetType()).ToArray();
            return Construct<T>(paramTypes, paramValues);
        }
    }
}
