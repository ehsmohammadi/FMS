using System.Linq;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MITD.Core;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;
using MITD.Fuel.Presentation.Logic.SL.Controllers;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{
    public class FuelAppLogicBootstrapper : IBootStrapper
    {
        public void Execute()
        {
            var container = ServiceLocator.Current.GetInstance<IWindsorContainer>();

            //controllers 
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<BaseController>()
                                   .WithService.FirstInterface()
                                   .LifestyleTransient());

            //viewModels

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<WorkspaceViewModel>()
                                   .LifestyleTransient());
            //Wrappers
            container.Register(Classes.FromThisAssembly()
                               .BasedOn<IServiceWrapper>()
                               .WithService.FromInterface().LifestyleSingleton());

            //mappers 
            container.Register(Classes.FromThisAssembly().BasedOn(typeof(IMapper<,>))
                                   .WithService.FromInterface()
                                   .LifestyleSingleton());
            
            //Resolver
            container.Register(Component.For(typeof (IResolver<>))
                                   .ImplementedBy(typeof (Resolver<>))
                                   .LifestyleSingleton());

            

        }
    }
}