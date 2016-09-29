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
using MITD.Fuel.Presentation.Logic.SL.ViewModels.Voucher;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class VoucherSetingController:BaseController,IVoucherSetingController
    {
        public VoucherSetingController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement) : base(viewManager, eventPublisher, deploymentManagement)
        {
        }

        public void ShowVoucherSeting()
        {
            var view = viewManager.ShowInTabControl<IVoucherSetingListView>();
            (view.ViewModel as VoucherSetingListVM).Load();
        }


        public void ShowLookUpAccount(int typ)
        {
            var view = viewManager.ShowInDialog<IAccountListView>();
            (view.ViewModel as AccountListVM).Load(0,typ);
        }


        public void ShowAddVoucherSeting()
        {
            var view = viewManager.ShowInDialog<IVoucherSetingView>();
            (view.ViewModel as VoucherSetingVM).Load();
        }

        public void ShowUpdateVoucherSeting(long id)
        {
            var view = viewManager.ShowInDialog<IVoucherSetingView>();
            (view.ViewModel as VoucherSetingVM).Load(id);
        }


        public void ShowAddVoucherSetingDetail(long id)
        {
            var view = viewManager.ShowInDialog<IVoucherSetingDetailView>();
            (view.ViewModel as VoucherSetingDetailVM).Load(id);
        }

        public void ShowUpdateVoucherSetingDetail(long id,long detailId)
        {
            var view = viewManager.ShowInDialog<IVoucherSetingDetailView>();
            (view.ViewModel as VoucherSetingDetailVM).Load(id,detailId);
        }
    }
}
