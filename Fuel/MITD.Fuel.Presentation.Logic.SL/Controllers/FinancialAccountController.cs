using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels.BasicInfo;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class FinancialAccountController :BaseController, IFinancialAccountController
    {
        public FinancialAccountController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
        :base(viewManager,eventPublisher,deploymentManagement)
        {

        }
        public void ShowList()
        {
            var view = viewManager.ShowInTabControl<IFinancialAccountListView>();
            (view.ViewModel as FinancialAccountListVM).Load();
        }
    }
}
