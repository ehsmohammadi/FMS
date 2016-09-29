using Castle.Windsor;
using MITD.Core;

namespace MITD.Fuel.Data.EF.Infrastructure
{
    public class DataBootstrapper : IBootstrapper
    {
        public void Execute()
        {
            var container = ServiceLocator.Current.GetInstance<IWindsorContainer>();

            //fetchStrategyFactory
            //container.Register(Component.For<IFetchStrategyFactory>()
            //                       .ImplementedBy<FetchStrategyFactory>()
            //                       .LifestyleSingleton());
            ////repositories
            //container.Register(Component.For(typeof (IRepository<>))
            //                       .ImplementedBy(typeof (EFRepository<>))
            //                       .LifestyleTransient());

            //container.Register(Classes.FromThisAssembly()
            //                       .BasedOn<IRepository>()
            //                       .WithServiceFromInterface()
            //                       .LifestyleTransient());

            //var test = ServiceLocator.Current.GetInstance<IUserRepository>();
        }
    }
}
