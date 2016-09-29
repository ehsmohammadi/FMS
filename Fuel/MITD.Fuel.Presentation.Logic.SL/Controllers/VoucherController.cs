using System;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Logic.SL.ViewModels.Voucher;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class VoucherController : BaseController, IVoucherController
    {
        #region Ctor
        public VoucherController
            (IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }

        #endregion

        public void ShowVoucherList()
        {
            var view = viewManager.ShowInTabControl<IVoucherListView>();
            (view.ViewModel as VoucherListVM).Load(0);

        }

        public void ShowRefrence(string referenceType,long entityId)
        {

            switch (referenceType)
            {
                //CharterIn
                case "CharterIn":
                    {
                        var view = viewManager.ShowInDialog<ICharterView>();
                        (view.ViewModel as CharterVM).SetCharterType(CharterType.In);
                        (view.ViewModel as CharterVM).Load(entityId);
                        break;
                    }
                //CharterOut
                case "CharterOut":
                    {
                        var view = viewManager.ShowInDialog<ICharterView>();
                        (view.ViewModel as CharterVM).SetCharterType(CharterType.Out);
                        (view.ViewModel as CharterVM).Load(entityId);
                        break;
                    }
                //FuelReport
                case "FuelReport":
                    {
                        var fuelReportListViewForDetails = this.viewManager.ShowInDialog<IFuelReportListView>();
                        (fuelReportListViewForDetails.ViewModel as FuelReportListVM).LoadByFilter(entityId.ToString(), null);

                        break;
                    }
                case "FuelReportDetail":
                    {
                        var fuelReportListViewForDetails = this.viewManager.ShowInDialog<IFuelReportListView>();
                        (fuelReportListViewForDetails.ViewModel as FuelReportListVM).LoadByFilter(null, entityId.ToString());

                        break;
                    }
                //PurchesInvoice
                case "PurchesInvoice":
                    {
                        break;
                    }
                //Offhire    
                case "Offhire":
                    {
                        break;
                    }
                //Invoice
                case "Invoice":
                    {
                        break;
                    }
                //Invoice
                case "Scrap":
                    {
                        break;
                    }
            }


        }

        public void ShowPrint(string no)
        {
            //Added by Hatefi on 1394-10-21
            var hostingSiteBaseAddress = Application.Current.Host.Source.AbsoluteUri.Replace(Application.Current.Host.Source.AbsolutePath, string.Empty);

            var reportViewerPageUri = new Uri(hostingSiteBaseAddress + "/Reports/ReportViewer.aspx?ItemPath=VoucherReport&LocalVoucherNo=" + no, UriKind.Absolute);

            System.Windows.Browser.HtmlPage.Window.Navigate(reportViewerPageUri, "_blank", HtmlWindowOptions.WINDOW_OPEN_OPTIONS);
        }
    }
}
