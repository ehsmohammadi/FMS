using System.Linq;
using System.Windows;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events.Security;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Main.Presentation.Logic.SL.ServiceWrapper;
using MITD.Presentation;

using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Security
{
    public class UserVM : WorkspaceViewModel, IEventHandler<UpdatePartyCustomActionsArgs>
    {
        #region Fields

        private readonly IFuelController appController;
        private readonly IUserSecurityServiceWrapper userService;
        private ICompanyServiceWrapper _companyServiceWrapper;
        private IUserController _userController;
        //  private ActionType actionType;

        #endregion

        #region Properties & BackField

        private UserDto user;
        public UserDto User
        {
            get { return user; }
            set { this.SetField(vm => vm.User, ref user, value); }
        }

        private CommandViewModel saveCommand;
        public CommandViewModel SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new CommandViewModel("ذخیره", new DelegateCommand(save));
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
                    cancelCommand = new CommandViewModel("انصراف", new DelegateCommand(OnRequestClose));
                }
                return cancelCommand;
            }
        }

        private CommandViewModel customActionsCommand;
        public CommandViewModel CustomActionsCommand
        {
            get
            {
                if (customActionsCommand == null)
                {
                    customActionsCommand = new CommandViewModel("تعیین دسترسی های کاربر", new DelegateCommand(() =>
                    {
                        PartyDto p = new PartyDto()
                        {
                            Id = User.Id,
                            PartyName = User.UserName,

                        };
                        _userController.ShowManageUserAccessList(p, false, 0);

                    }));
                }
                return customActionsCommand;
            }
        }

        private List<CompanyDto> _companyDtos;
        public List<CompanyDto> CompanyDtos
        {
            get { return _companyDtos; }
            set { this.SetField(vm => vm.CompanyDtos, ref _companyDtos, value); }
        }

        private List<UserGroupDescriptionDto> userGroupList;
        public List<UserGroupDescriptionDto> UserGroupList
        {
            get { return userGroupList; }
            set { this.SetField(vm => vm.UserGroupList, ref userGroupList, value); }
        }


        #endregion

        #region Constructors

        public UserVM()
        {

            User = new UserDto() { PartyName = "uname1", FirstName = "User1" };
        }

        public UserVM(IUserSecurityServiceWrapper userService,
                           IFuelController appController,
                           IUserController userController,
            ICompanyServiceWrapper companyServiceWrapper)
        {

            this.userService = userService;
            this.appController = appController;
            this._companyServiceWrapper = companyServiceWrapper;
            _userController = userController;
            UserGroupList = new List<UserGroupDescriptionDto>();
            User = new UserDto();
            CompanyDtos = new List<CompanyDto>();

        }

        #endregion

        #region Methods

        public void Load(UserDto userDto)
        {

            User = userDto;
            // preload();
            loadUser();

        }

        private void loadcompany()
        {
            ShowBusyIndicator();
            _companyServiceWrapper.GetAll((res, exp) => appController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    CompanyDtos = res.Result.ToList();

                }
                else
                    appController.HandleException(exp);

            }));
        }

        private void preload()
        {
            ShowBusyIndicator();
            userService.GetAllUserGroupsDescriptions((res, exp) => appController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    UserGroupList = res;
                    if (User.Id > 0)
                        loadUser();
                }
                else
                    appController.HandleException(exp);

            }));
        }

        private void loadUser()
        {
            ShowBusyIndicator();
            if (!string.IsNullOrEmpty(User.PartyName))
            {
                userService.GetUser((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                  {
                      HideBusyIndicator();
                      if (exp == null)
                      {
                          User = res;
                          loadcompany();
                          // UserGroupList.Where(allGroups => User.Groups.Select(g => g.PartyName).Contains(allGroups.PartyName))
                          //  .ToList();//.ForEach(g => g.IsChecked = true);
                      }
                      else
                          appController.HandleException(exp);

                  }), User.PartyName);
            }
            else
            {
                loadcompany();
            }

        }


        private void save()
        {
            //user.TypeId = "string";
            //if (!user.Validate()) return;
            ShowBusyIndicator();
            User.Groups = userGroupList.ToList();//.Where(f => f.IsChecked).ToList();

            if (User.Id == 0)
            {
                User.UserName = User.PartyName;
                userService.AddUser((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                    {
                        HideBusyIndicator();
                        if (exp != null)
                            appController.HandleException(exp);
                        else
                            FinalizeAction();
                    }), user);
            }
            else if (User.Id > 0)
            {
                userService.UpdateUser((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                    {
                        HideBusyIndicator();
                        if (exp != null)
                            appController.HandleException(exp);
                        else
                            FinalizeAction();
                    }), user);
            }
        }

        private void FinalizeAction()
        {
            appController.Publish(new UpdateUserListArgs());
            OnRequestClose();
        }


        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        }


        public void Handle(UpdatePartyCustomActionsArgs eventData)
        {
            if (eventData.PartyName == User.PartyName)
                User.CustomActions = eventData.CustomActions;

        }



        #endregion


    }
}

