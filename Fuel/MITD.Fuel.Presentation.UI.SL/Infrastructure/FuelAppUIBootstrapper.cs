extern alias MITDCoreSL;

using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.Controllers;
using MITD.Presentation;
using MITD.Fuel.Presentation.Logic.SL.Controllers;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation.Config;

namespace MITD.Fuel.Presentation.UI.SL.Infrastructure
{
    public class FuelAppUIBootstrapper : IBootStrapper
    {
        public void Execute()
        {
            //  ApplicationConfigHelper.ConfigureModule<IFuelReportController, FuelReportController>(null);


            var container = MITDCoreSL::MITD.Core.ServiceLocator.Current.GetInstance<IWindsorContainer>();


            //controllers 
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IView>()
                //.WithServiceDefaultInterfaces()
                                   .WithService.FromInterface().LifestyleTransient());

            (new FuelAppLogicBootstrapper()).Execute();
        }
    }
}