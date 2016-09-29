using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Presentation.Config;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Main.Presentation.Logic.SL;
using MITD.Main.Presentation.Logic.SL.Infrastructure;
//using MITD.StorageSpace.Presentation.Contracts.SL.Controllers;
//using MITD.StorageSpace.Presentation.Contracts.SL.Views;
using Omu.ValueInjecter.Silverlight;

namespace MITD.Main.Presentation.UI.SL.Infrastructure
{
    public class UIBootstrapper
    {
        public void Execute()
        {

            //  MITD.Presentation.Config.ApplicationConfigHelper.
            ApplicationConfigHelper.Configure<MainWindowVM, MainPage>(new Dictionary<string, List<Type>> 
            {
            {"MITD.Main.Presentation.UI.SL.xap",new List<Type>()},   
            //{"MITD.StorageSpace.Presentation.BasicInfoApp.UI.SL.xap",new List<Type>{
            //                                                                  typeof (IStorageSpaceTypeListView),
            //                                                                  typeof (IStorageSpaceTypeController),
            //                                                                  typeof(IUnitListView),
            //                                                                  typeof(IUnitController),
            //                                                                  typeof(IGoodTypeListView),
            //                                                                  typeof(IGoodTypeController),
            //                                                                  typeof(IGoodListView),
            //                                                                  typeof(IGoodController),
            //                                                                  typeof(IGoodCategoryController),
            //                                                                  typeof(IGoodCategoryListView),
            //                                                                  typeof(IEffectiveFactorListView),
            //                                                                  typeof(IEffectiveFactorController),
            //                                                                  typeof(IReceiptTypeListView),
            //                                                                  typeof(IReceiptTypeController),
            //                                                                  typeof(IBrandListView),
            //                                                                  typeof(IBrandController),
            //                                                                   typeof(IIssueTypeListView),
            //                                                                   typeof(IIssueTypeController),
            //                                                                  typeof(IGoodCategoryListView),
            //                                                                  typeof(IGoodCategoryController),
                                                                             

            //                                                               }},
        {"MITD.Fuel.Presentation.UI.SL.xap",new List<Type>
                {
                    typeof(IFuelController),
                    typeof(ICharterController),
                    typeof(IOrderController),
                    typeof(IInvoiceController),
                    typeof(IOrderItemController),
                    typeof(ICharterView),
                    typeof(IFuelReportListView),
                    typeof(IFuelReportController),
                    typeof(IFuelReportDetailController),  
                    typeof(IVoyageController),
                    typeof(IScrapController),
                    typeof(IVesselController),
                    typeof(IInventoryTransactionController),
                    typeof(IOffhireController),
                    typeof(ICardexReportController),
                    typeof(IVoucherController),
                    typeof(IVoucherSetingController),
                    typeof(IVoucherReportController),
                    typeof(IInventoryQuantityReportController),
                    typeof(IVesselDataReportController),
                    typeof(IFinancialAccountController),
                    typeof(IPeriodicalFuelStatisticsReportController),
                    typeof(IUserController),
                    typeof(ICurrencyController)
                }}
                
            });
            //todo: when ApplicationConfigHelper.Configure is revised , this execute method must be revised too

            //value injecter 
            var container = ServiceLocator.Current.GetInstance<IWindsorContainer>();
            container.Register(Component.For<IValueInjecter>()
                                   .ImplementedBy<ValueInjecter>()
                                   .LifestyleSingleton());

            container.Register(


                          Component.For<IUserProvider>()
                           .ImplementedBy<UserProvider>()
                          .LifestyleTransient());

            //logic bootstrapper 
            (new LogicBootstrapper()).Execute();

            ApplicationConfigHelper.Start();
        }
    }
}
