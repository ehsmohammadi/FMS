namespace MITD.Fuel.Data.EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Reflection;

    internal sealed class Configuration : DbMigrationsConfiguration<MITD.Fuel.Data.EF.Context.DataContainer>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MITD.Fuel.Data.EF.Context.DataContainer context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
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
