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
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class CharterController : BaseController, ICharterController
    {

        #region Ctor
        public CharterController(IViewManager viewManager,IEventPublisher eventPublisher,IDeploymentManagement deploymentManagement)
        :base(viewManager,eventPublisher,deploymentManagement)
        {

        }

        #endregion


        #region Method
        public void ShowCharterInList()
        {
            var view = viewManager.ShowInTabControl<ICharterListView>();
            (view.ViewModel as CharterListVM).SetCharterType(CharterType.In);
        }

        public void AddCharterIn()
        {
            var view = viewManager.ShowInDialog<ICharterView>();
            (view.ViewModel as CharterVM).SetCharterType(CharterType.In);
            (view.ViewModel as CharterVM).Load(-1);

        }

        public void UpdateCharterIn(long id)
        {
            var view = viewManager.ShowInDialog<ICharterView>();
            (view.ViewModel as CharterVM).SetCharterType(CharterType.In);
            (view.ViewModel as CharterVM).Load(id);
        }

        public void AddCharterInItem(CharterStateTypeEnum charterStateTypeEnum ,long charterId,long charterItemId,long vesselId)
        {
            var view = viewManager.ShowInDialog<ICharterItemView>();
            (view.ViewModel as CharterItemVM).SetCharterType(CharterType.In, charterStateTypeEnum, charterId, charterItemId, vesselId);
            (view.ViewModel as CharterItemVM).LoadGeneralItems(null);
        }

        public void UpdateCharterInItem(CharterStateTypeEnum charterStateTypeEnum, long charterId, long charterItemId,long vesselId)
        {
            var view = viewManager.ShowInDialog<ICharterItemView>();
            (view.ViewModel as CharterItemVM).SetCharterType(CharterType.In, charterStateTypeEnum,charterId, charterItemId, vesselId);
            (view.ViewModel as CharterItemVM).Load();
        }

        public void ShowCharterOutList()
        {
            var view = viewManager.ShowInTabControl<ICharterListView>();
            (view.ViewModel as CharterListVM).SetCharterType(CharterType.Out);
        }

        public void AddCharterOut()
        {
            var view = viewManager.ShowInDialog<ICharterView>();
            (view.ViewModel as CharterVM).SetCharterType(CharterType.Out);
            (view.ViewModel as CharterVM).Load(-1);
        }

        public void UpdateCharterOut(long id)
        {
             var view = viewManager.ShowInDialog<ICharterView>();
            (view.ViewModel as CharterVM).SetCharterType(CharterType.Out);
            (view.ViewModel as CharterVM).Load(id);
        }

        public void AddCharterOutItem(CharterStateTypeEnum charterStateTypeEnum, long charterId, long charterItemId,long vesselId)
        {
            var view = viewManager.ShowInDialog<ICharterItemView>();
            (view.ViewModel as CharterItemVM).SetCharterType(CharterType.Out, charterStateTypeEnum, charterId, charterItemId,vesselId);
            (view.ViewModel as CharterItemVM).LoadGeneralItems(null);
        }

        public void UpdateCharterOutItem(CharterStateTypeEnum charterStateTypeEnum, long charterId, long charterItemId,long vesselId)
        {
            var view = viewManager.ShowInDialog<ICharterItemView>();
            (view.ViewModel as CharterItemVM).SetCharterType(CharterType.Out, charterStateTypeEnum, charterId, charterItemId, vesselId);
            (view.ViewModel as CharterItemVM).Load();
        }

        #endregion
    }
}
