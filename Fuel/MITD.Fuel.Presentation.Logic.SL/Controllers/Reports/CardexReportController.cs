using System;
using System.Windows;
using System.Windows.Browser;
using MITD.Core;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Logic.SL.Controllers;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.Controllers
{
    public class CardexReportController : BaseController, ICardexReportController
    {

        public CardexReportController(IViewManager _viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(_viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void ShowInventoryCardexReport()
        {
            var hostingSiteBaseAddress = Application.Current.Host.Source.GetHostingSiteBaseAddress();

            var reportViewerPageUri = new Uri(hostingSiteBaseAddress + "/Reports/ReportViewer.aspx?ItemPath=InventoryCardex", UriKind.Absolute);

            System.Windows.Browser.HtmlPage.Window.Navigate(reportViewerPageUri, "_blank", HtmlWindowOptions.WINDOW_OPEN_OPTIONS);
            //System.Windows.Browser.HtmlPage.PopupWindow(reportViewerPageUri, "_blank", HtmlWindowOptions.PopupWindowOptions);
        }

        public void ShowAccountingCardexReport()
        {
            var hostingSiteBaseAddress = Application.Current.Host.Source.GetHostingSiteBaseAddress();

            var reportViewerPageUri = new Uri(hostingSiteBaseAddress + "/Reports/ReportViewer.aspx?ItemPath=AccountingCardex", UriKind.Absolute);

            System.Windows.Browser.HtmlPage.Window.Navigate(reportViewerPageUri, "_blank", HtmlWindowOptions.WINDOW_OPEN_OPTIONS);
            //System.Windows.Browser.HtmlPage.PopupWindow(reportViewerPageUri, "_blank", HtmlWindowOptions.PopupWindowOptions);
        }
    }
}
