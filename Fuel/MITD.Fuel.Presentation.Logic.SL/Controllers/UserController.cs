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
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views.Security;
using MITD.Fuel.Presentation.Logic.SL.ViewModels.Security;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class UserController : BaseController, IUserController
    {
        public UserController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {
        }

        public void ShowUserList()
        {
            var view = ServiceLocator.Current.GetInstance<IUserListView>();
            ((UserListVM)view.ViewModel).Load(0);
            viewManager.ShowInTabControl(view);
        }

        public void ShowGroupList()
        {
            var view = ServiceLocator.Current.GetInstance<IUserGroupListView>();
            ((UserGroupListVM)view.ViewModel).Load();
            viewManager.ShowInTabControl(view);
        }

        public void ShowGroupView(UserGroupDto userGroupDto)
        {
            var view = ServiceLocator.Current.GetInstance<IUserGroupView>();
            ((UserGroupVM)view.ViewModel).Load(userGroupDto);
            viewManager.ShowInDialog(view);
        }


        public void ShowManageUserAccessList(PartyDto partyDto,bool isgroup,long groupId)
        {
            var view = ServiceLocator.Current.GetInstance<IPartyCustomActionsView>();
            ((PartyCustomActionsVM)view.ViewModel).Load(partyDto, isgroup, groupId);
            viewManager.ShowInDialog(view);
        }




        public void ShowUserView(UserDto user)
        {
            var view = ServiceLocator.Current.GetInstance<IUserView>();
            ((UserVM)view.ViewModel).Load(user);
            viewManager.ShowInDialog(view);
        }



        public void ShowGroupView(string partyName)
        {
            var view = ServiceLocator.Current.GetInstance<IUserGroupView>();
            ((UserGroupVM)view.ViewModel).Load(partyName);
            viewManager.ShowInDialog(view);
        }

        public void ShowChangePassWord()
        {
            var view = ServiceLocator.Current.GetInstance<IChangePassView>();
            viewManager.ShowInDialog(view);
        }
    }
}
