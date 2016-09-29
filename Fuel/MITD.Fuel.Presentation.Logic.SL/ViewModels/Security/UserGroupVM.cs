using MITD.Core;
using MITD.Fuel.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events.Security;
using MITD.Main.Presentation.Logic.SL.ServiceWrapper;
using MITD.Presentation;

using System.Collections.ObjectModel;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Security
{
    public class UserGroupVM : WorkspaceViewModel, IEventHandler<UpdatePartyCustomActionsArgs>
    { 
        #region Fields

        private readonly IFuelController appController;
        private readonly IUserController _userController;
        private readonly IUserSecurityServiceWrapper userService;
  //      private ActionType actionType;

        #endregion

        #region Properties & BackField

        private UserGroupDto userGroup;
        public UserGroupDto UserGroup
        {
            get { return userGroup; }
            set { this.SetField(vm => vm.UserGroup, ref userGroup, value); }
        }

        private CommandViewModel saveCommand;
        public CommandViewModel SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new CommandViewModel("تایید", new DelegateCommand(save));
                }
                return saveCommand;
            }
        }

        private CommandViewModel cancelCommand;
        public CommandViewModel CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new CommandViewModel("انصراف",new DelegateCommand(OnRequestClose));
                }
                return cancelCommand;
            }
        }

        private CommandViewModel customActionsCommand;
        public CommandViewModel CustomActionsCommand
        {
            get
            {
                customActionsCommand = new CommandViewModel("تعیین دسترسی های کاربر", new DelegateCommand(() =>
                {

                    _userController.ShowManageUserAccessList(new PartyDto(),true,UserGroup.Id);
                }));
                //  if (customActionsCommand == null)
                 //   customActionsCommand = CommandHelper.GetControlCommands(this, appController, (int)ActionType.ManageGroupCustomActions);
                return customActionsCommand;
            }
        }

        //public bool IsModifyMode
        //{
        //    get { return (actionType == ActionType.ModifyUserGroup); }
        //}

        //public bool IsCreateMode
        //{
        //    get { return (actionType == ActionType.AddUserGroup); }
        //}

        #endregion

        #region Constructors

        public UserGroupVM()
        {

            UserGroup = new UserGroupDto() { PartyName = "ugroupname1",Description= "UserGroup1" };
        }

        public UserGroupVM(IUserSecurityServiceWrapper userService, 
                           IFuelController appController,
            IUserController userController)//,
                        //   IBasicInfoAppLocalizedResources basicInfoAppLocalizedResources)
        {
           
            this.userService = userService;
            this.appController = appController;
            this._userController = userController;
            //BasicInfoAppLocalizedResources = basicInfoAppLocalizedResources;
            UserGroup = new UserGroupDto();
            //DisplayName = BasicInfoAppLocalizedResources.UserGroupViewTitle;
        } 

        #endregion

        #region Methods



        public void Load(UserGroupDto customFieldParam)//,ActionType actionTypeParam)
        {
            //actionType = actionTypeParam;
            UserGroup = customFieldParam;
        }

        public void Load(string partyName)
        {
            userService.GetUserGroup((res, exp) => appController.BeginInvokeOnDispatcher(() =>
            {
                if (exp == null)
                {
                    UserGroup = res;
                }
                else
                    appController.HandleException(exp);

            }), partyName);
        }


        private void save()
        {
            //user.TypeId = "string";
            if (!userGroup.Validate()) return;
            ShowBusyIndicator();
            //if (actionType==ActionType.AddUserGroup)
            if (UserGroup.Id==0)
            {
                userService.AddUserGroup((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                    {
                        HideBusyIndicator();
                        if (exp != null)
                            appController.HandleException(exp);
                        else
                            FinalizeAction();
                    }), userGroup);
            }
            else //if (actionType == ActionType.ModifyUserGroup)
            {
                userService.UpdateUserGroup((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                    {
                        HideBusyIndicator();
                        if (exp != null)
                            appController.HandleException(exp);
                        else
                            FinalizeAction();
                    }), userGroup);
            }
        }

        private void FinalizeAction()
        {
            appController.Publish(new UpdateUserGroupListArgs());
            OnRequestClose();
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        }


        public void Handle(UpdatePartyCustomActionsArgs eventData)
        {
            if (eventData.PartyName == userGroup.PartyName)
                userGroup.CustomActions = eventData.CustomActions;
        }

        #endregion
    }
}

