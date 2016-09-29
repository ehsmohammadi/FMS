using System;
using System.Windows;
using System.Windows.Browser;
using MITD.Core;
using MITD.Fuel.Presentation.Logic.SL.Controllers;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.Controllers
{
    public class VesselDataReportController : BaseController, IVesselDataReportController
    {

        public VesselDataReportController(IViewManager _viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(_viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void ShowReport()
        {
            var hostingSiteBaseAddress = Application.Current.Host.Source.GetHostingSiteBaseAddress();

            var reportViewerPageUri = new Uri(hostingSiteBaseAddress + "/Reports/ReportViewer.aspx?ItemPath=VesselReportDataReport", UriKind.Absolute);

            System.Windows.Browser.HtmlPage.Window.Navigate(reportViewerPageUri, "_blank", HtmlWindowOptions.WINDOW_OPEN_OPTIONS);
            //System.Windows.Browser.HtmlPage.PopupWindow(reportViewerPageUri, "_blank", HtmlWindowOptions.PopupWindowOptions);
        }

        public void ShowVesselRunningValuesReport()
        {
            var hostingSiteBaseAddress = Application.Current.Host.Source.GetHostingSiteBaseAddress();

            var reportViewerPageUri = new Uri(hostingSiteBaseAddress + "/Reports/ReportViewer.aspx?ItemPath=VesselRunningValuesReport", UriKind.Absolute);

            System.Windows.Browser.HtmlPage.Window.Navigate(reportViewerPageUri, "_blank", HtmlWindowOptions.WINDOW_OPEN_OPTIONS);
            //System.Windows.Browser.HtmlPage.PopupWindow(reportViewerPageUri, "_blank", HtmlWindowOptions.PopupWindowOptions);
        }
    }
}
