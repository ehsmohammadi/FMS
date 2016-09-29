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
    public class UserListVM : WorkspaceViewModel, IEventHandler<UpdateUserListArgs>
    {
        #region Fields

        private readonly IFuelController appController;
        private readonly IUserController _userController;
        private readonly IUserSecurityServiceWrapper userService;
        
        #endregion

        #region Properties & Back fields

        private PagedSortableCollectionView<UserDTOWithActions> users;
        public PagedSortableCollectionView<UserDTOWithActions> Users
        {
            get { return users; }
            set { this.SetField(p => p.Users, ref users, value); }
        }

        private UserDTOWithActions selectedUser;
        public UserDTOWithActions SelectedUser
        {
            get { return selectedUser; }
            set
            {
                this.SetField(p => p.SelectedUser, ref selectedUser, value);
                if (selectedUser == null) return;
               // UserCommands = createCommands();
              
            }
        }

        private DataGridCommandViewModel selectedCommand;
        public DataGridCommandViewModel SelectedCommand
        {
            get { return selectedCommand; }
            set { this.SetField(p => p.SelectedCommand, ref selectedCommand, value); }
        }

        
        private CommandViewModel filterCommand;
        public CommandViewModel FilterCommand
        {
            get
            {
                if (filterCommand == null)
                {
                    filterCommand = new CommandViewModel("جستجو",new DelegateCommand(() =>
                    {
                        refresh(0);
                    }));
                }
                return filterCommand;
            }
        }
        private CommandViewModel addCommand;
        public CommandViewModel AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new CommandViewModel("افزودن", new DelegateCommand(() =>
                    {
                        _userController.ShowUserView(new UserDto());
                    }));
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
                        UserDto u=new UserDto()
                        {
                          
                         FirstName=SelectedUser.FirstName,
                         LastName=SelectedUser.LastName,
                         PartyName = SelectedUser.PartyName,
                         
                        };
                        _userController.ShowUserView(u);
                    }));
                }
                return editCommand;
            }
        }


        private List<DataGridCommandViewModel> userCommands;
        public List<DataGridCommandViewModel> UserCommands
        {
            get { return userCommands; }
            private set { 
                this.SetField(p => p.UserCommands, ref userCommands, value);
                if (UserCommands.Count > 0) SelectedCommand = UserCommands[0];
            }

        }

        private UserCriteria userCriteria;
        public UserCriteria UserCriteria
        {
            get { return userCriteria; }
            set { this.SetField(p => p.UserCriteria, ref userCriteria, value); }
        }


        #endregion

        #region Constructors

        public UserListVM()
        {
            init();
            Users.Add(new UserDTOWithActions{PartyName = "ehsan"});
           
        }

        public UserListVM(IFuelController appController,
            IUserSecurityServiceWrapper userService, IUserController userController)
        {
            this.appController = appController;
            this.userService = userService;
            this._userController = userController;
            this.DisplayName = "تنظیمات کاربران";
            init();
        }

        #endregion

        #region Methods

        void init()
        {
            UserCriteria = new UserCriteria();
            Users = new PagedSortableCollectionView<UserDTOWithActions> { PageSize = 20 };
            Users.OnRefresh += (s, args) => refresh(users.PageIndex+1);
           

        }
       
      

        public void Load(int pageIndex)
        {
            refresh(pageIndex);
        }


        private void refresh(int pageIndex)
        {
            var sortBy = Users.SortDescriptions.ToDictionary(sortDesc => sortDesc.PropertyName, sortDesc =>
                (sortDesc.Direction == ListSortDirection.Ascending ? "ASC" : "DESC"));
            ShowBusyIndicator("در حال دریافت اطلاعات...");

            userService.GetAllUsers(
                   (res, exp) => appController.BeginInvokeOnDispatcher(() =>
                  {
                      HideBusyIndicator();
                      if (exp == null)
                      {
                          if(res.Result!=null)
                            Users.SourceCollection = res.Result;
                          else
                              Users.SourceCollection = new Collection<UserDTOWithActions>();
                          Users.TotalItemCount = res.TotalCount;
                          Users.PageIndex = Math.Max(0, res.CurrentPage - 1);
                      }
                      else appController.HandleException(exp);
                  }), Users.PageSize, pageIndex, sortBy, UserCriteria);
        }


        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        }
     
        public void Handle(UpdateUserListArgs eventData)
        {
            refresh(0);
        }

        #endregion


    }
}
