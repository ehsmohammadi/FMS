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
    public class FuelReportController : BaseController, IFuelReportController
    {

        public FuelReportController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void Add()
        {
            // viewManager.ShowInTabControl<IFuelReportView>();
            viewManager.ShowInDialog<IFuelReportView>();
        }

        public void Edit(FuelReportDto dto)
        {

            var view = viewManager.ShowInDialog<IFuelReportView>();
            (view.ViewModel as FuelReportVM).Load(dto);
        }

        public void ShowList()
        {
            var view = this.viewManager.ShowInTabControl<IFuelReportListView>();
            (view.ViewModel as FuelReportListVM).Load();
        }

        /// <summary>
        /// Displays FuelReport View filtered by given FuelReport Ids or FuelReportDetail Ids
        /// </summary>
        /// <param name="fuelReportIds">Comma separated Id list</param>
        /// <param name="fuelReportDetailIds">Comma separated Id list</param>
        public void ShowListByFilter(string fuelReportIds, string fuelReportDetailIds)
        {
            var view = this.viewManager.ShowInTabControl<IFuelReportListView>();
            (view.ViewModel as FuelReportListVM).LoadByFilter(fuelReportIds, fuelReportDetailIds);
        }


        public void ShowOriginalVesselReport(string reportCode)
        {
            var windowOption = new HtmlPopupWindowOptions
            {
                Location = false,
                Menubar = false,
                Toolbar = false,
                Directories = false,
                Resizeable = true,
                Scrollbars = true,
                Status = false,
                Width = 1000,
                Height = 1000,
            };

            var hostingSiteBaseAddress = Application.Current.Host.Source.GetHostingSiteBaseAddress();
            //var hostingSiteBaseAddress = Application.Current.Host.Source.AbsoluteUri.Replace(Application.Current.Host.Source.AbsolutePath, string.Empty);

            var reportViewerPageUri = new Uri(hostingSiteBaseAddress + "/Reports/ReportViewer.aspx?ItemPath=FuelOriginalDataReport&ReportCode=" + reportCode, UriKind.Absolute);

            //System.Windows.MessageBox.Show(reportViewerPageUri.AbsoluteUri);

            System.Windows.Browser.HtmlPage.PopupWindow(reportViewerPageUri, "_blank", windowOption);
        }
    }
}
