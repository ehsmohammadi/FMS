using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MITD.Core;
using MITD.Core.Config;
using MITD.DataAccess.Config;
using MITD.Domain.Repository;


namespace MITD.Main.Services.Host.Infrastructure
{
    public class HostBootstrapper
    {
        public void Execute()
        {
           
            //create container and locator 
            var container = new WindsorContainer();
            container.Register(Component.For<IWindsorContainer>().Instance(container).LifestyleSingleton());
            var serviceLocator = new WindsorServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);

            //register areas, filters, routes, boundles
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //register data container and repositories
            //DataAccessConfigHelper.ConfigureContainer<PerHttpContextUnitOfWorkScope, DataContainer>(container,
            //   () =>
            //   {
            //       var ctx = new DataContainer();
            //       return ctx;
            //   });

            //
            //register api controllers 
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<ApiController>()
                                   .WithService.Self()
                                   .LifestyleTransient());
            //register controllers 
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<Controller>()
                                   .WithService.Self()
                                   .LifestyleTransient());

            //run DataBootstrapper
            //(new DataBootstrapper()).Execute();

            ////run ApplicationBootstrapper   
            //(new ApplicationBootstrapper()).Execute();

            ////run facadeBootstrapper
            //(new FacadeBootstrapper()).Execute();

            //set default resolver 
            System.Web.Mvc.IDependencyResolver resolver = new MITD.Core.IocDependencyResolver();
            DependencyResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.DependencyResolver = resolver.ToServiceResolver();

            //}
            //catch (Exception EX)
            //{

            //    throw EX;
            //}
        }

         
    }
}