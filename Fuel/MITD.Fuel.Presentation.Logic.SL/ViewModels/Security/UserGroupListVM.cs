using System.ComponentModel;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events.Security;
using MITD.Fuel.Presentation.Contracts.SL.Views.Security;
using MITD.Main.Presentation.Logic.SL.ServiceWrapper;
using MITD.Presentation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Security
{
    public class UserGroupListVM : WorkspaceViewModel, IEventHandler<UpdateUserGroupListArgs>
    {
        #region Fields

        private readonly IFuelController appController;
        private readonly IUserController _userController;
        private readonly IUserSecurityServiceWrapper userService;
        
        #endregion

        #region Properties & Back fields
         
        private List<UserGroupDtoWithActions> userGroups;
        public List<UserGroupDtoWithActions> UserGroups
        {
            get { return userGroups; }
            set { this.SetField(p => p.UserGroups, ref userGroups, value); }
        }

        private UserGroupDtoWithActions selectedUserGroup;
        public UserGroupDtoWithActions SelectedUserGroup
        {
            get { return selectedUserGroup; }
            set
            {
                this.SetField(p => p.SelectedUserGroup, ref selectedUserGroup, value);
                if (selectedUserGroup == null) return;
                //UserGroupCommands = createCommands();
                //if (View != null)
                //    ((IUserGroupListView)View).CreateContextMenu(new ReadOnlyCollection<DataGridCommandViewModel>(UserGroupCommands));
            }
        }

        private DataGridCommandViewModel selectedCommand;
        public DataGridCommandViewModel SelectedCommand
        {
            get { return selectedCommand; }
            set { this.SetField(p => p.SelectedCommand, ref selectedCommand, value); }
        }


        private CommandViewModel addCommand;
        public CommandViewModel AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand=new CommandViewModel("افزودن",new DelegateCommand(() =>
                    {
                        _userController.ShowGroupView(new UserGroupDto());
                    }));
                    //  filterCommand = new CommandViewModel(BasicInfoAppLocalizedResources.UserGroupListViewTitle,new DelegateCommand(refresh));
                }
                return addCommand;
            }
        }
        private CommandViewModel editCommand;
        public CommandViewModel EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new CommandViewModel("ویرایش", new DelegateCommand(() =>
                    {
                        _userController.ShowGroupView(SelectedUserGroup.PartyName);
                    }));
                }
                return editCommand;
            }
        }
        
        private CommandViewModel filterCommand;
        public CommandViewModel FilterCommand
        {
            get
            {
                if (filterCommand == null)
                {
                  //  filterCommand = new CommandViewModel(BasicInfoAppLocalizedResources.UserGroupListViewTitle,new DelegateCommand(refresh));
                }
                return filterCommand;
            }
        }

        private List<DataGridCommandViewModel> userCommands;
        public List<DataGridCommandViewModel> UserGroupCommands
        {
            get { return userCommands; }
            private set { 
                this.SetField(p => p.UserGroupCommands, ref userCommands, value);
                if (UserGroupCommands.Count > 0) SelectedCommand = UserGroupCommands[0];
            }

        }


        #endregion

        #region Constructors

        public UserGroupListVM()
        {
            init();
            UserGroups.Add(new UserGroupDtoWithActions(){PartyName = "ehsan"});
          //  BasicInfoAppLocalizedResources=new BasicInfoAppLocalizedResources();
        }

        public UserGroupListVM( IFuelController appController,
            IUserController userController,
            IUserSecurityServiceWrapper userService)//, IBasicInfoAppLocalizedResources localizedResources)
        {
            this.appController = appController;
            this.userService = userService;
            this._userController = userController;
            this.DisplayName = "تنظیمات گروه کاربران";
        //    BasicInfoAppLocalizedResources = localizedResources;
          //  DisplayName = BasicInfoAppLocalizedResources.UserGroupListViewTitle;
            init();
        }

        #endregion

        #region Methods

        void init()
        {
            UserGroups = new List<UserGroupDtoWithActions>();
            //UserGroupCommands = new List<DataGridCommandViewModel>
            //{
            //       CommandHelper.GetControlCommands(this, appController, new List<int>{ (int) ActionType.AddUserGroup }).FirstOrDefault()
            //};
        }
       
        //private List<DataGridCommandViewModel> createCommands()
        //{
        //    return CommandHelper.GetControlCommands(this, appController, SelectedUserGroup.ActionCodes);
        //}

        public void Load()
        {
            refresh();
        }

        private void refresh()
        {
           ShowBusyIndicator("در حال دریافت اطلاعات...");
           userService.GetAllUserGroups(
                  (res, exp) => appController.BeginInvokeOnDispatcher(() =>
                  {
                      HideBusyIndicator();
                      if (exp == null)
                        UserGroups = res;
                      else appController.HandleException(exp);
                  }));
        }


        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        }
     
        public void Handle(UpdateUserGroupListArgs eventData)
        {
            refresh();
        }

        #endregion


    }
}
