using System.Linq;
using MITD.Fuel.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events.Security;
using MITD.Main.Presentation.Logic.SL.ServiceWrapper;
using MITD.Presentation;

using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Security
{
    public class PartyCustomActionsVM : WorkspaceViewModel
    {
        #region Fields

        private readonly IFuelController appController;
        private readonly IUserSecurityServiceWrapper userService;
        private Dictionary<int, bool> _userActions;

        public bool IsGroup { get; set; }
        
        public Dictionary<int, bool> UserActions
        {
            get { return _userActions; }
            set { this.SetField(p => p.UserActions, ref _userActions, value); }
        }

        #endregion

        #region Properties

        private PartyDto party ;
        public PartyDto Party
        {
            get { return party; }
            set { this.SetField(vm => vm.Party, ref party, value); }
        }

        private List<Privilege> privilegeList;
        public List<Privilege> PrivilegeList
        {
            get { return privilegeList; }
            set { this.SetField(vm => vm.PrivilegeList, ref privilegeList, value); }
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
                    cancelCommand = new CommandViewModel("انصراف",new DelegateCommand(OnRequestClose));
                }
                return cancelCommand;
            }
        } 

        #endregion

        #region Constructors

        public PartyCustomActionsVM()
        {
           
            init();
            party = new PartyDto();
        }


        public PartyCustomActionsVM(IFuelController appController,
            IUserSecurityServiceWrapper userService
            
            )
        {
            this.appController = appController;
            this.userService = userService;
            init();

        } 

        #endregion

        #region Methods

        private void init()
        {
            party = new PartyDto();
            PrivilegeList = new List<Privilege>();
            UserActions=new Dictionary<int, bool>();
            
        }


        public void Load(PartyDto PartyDto, bool isgroup, long groupId)
        {
            IsGroup = isgroup;
            Party = PartyDto;
            ShowBusyIndicator();
            userService.GetAllActionTypes((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                {
                    HideBusyIndicator();
                    if (exp == null)
                    {
                        PrivilegeList = res.Select(a => new Privilege() { ActionType = a, IsDeny = false, IsGrant = false }).ToList();
                        setPartyCustomActions(isgroup, groupId);
                    }
                    else
                    {
                        appController.HandleException(exp);
                    }
                }));

        }

        private void setPartyCustomActions(bool isgroup,long groupId)
        {

            userService.GetAllUserActionTypes((res, exp) => appController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    res.ForEach(c =>
                    {
                    UserActions.Add((int)c.Id,true);    
                    });
                    

                    PrivilegeList.Where(all => UserActions.Where(c => c.Value).Select(c => c.Key).Contains((int)all.ActionType.Id))
                .ToList().ForEach(p => p.IsGrant = true);

                    PrivilegeList.Where(all => UserActions.Where(c => !c.Value).Select(c => c.Key).Contains((int)all.ActionType.Id))
                        .ToList().ForEach(p => p.IsDeny = true);
                }
                else
                {
                    appController.HandleException(exp);
                }
            }), Party.PartyName, isgroup, groupId);

          



            //PrivilegeList.Where(all => Party.CustomActions.Where(c => c.Value).Select(c => c.Key).Contains((int)all.ActionType.Id))
            //     .ToList().ForEach(p => p.IsGrant = true);

            //PrivilegeList.Where(all => Party.CustomActions.Where(c => !c.Value).Select(c => c.Key).Contains((int)all.ActionType.Id))
            //    .ToList().ForEach(p => p.IsDeny = true);
        }
         
        
        private void save()
        {
            var grants = PrivilegeList.Where(p => p.IsGrant).Select(p => p.ActionType).ToDictionary(p => (int)p.Id, p => true);
            var denies = PrivilegeList.Where(p => p.IsDeny).Select(p => p.ActionType).ToDictionary(p => p.Id, p => false);

            foreach (int k in denies.Keys)
            {
                if (!grants.Keys.Contains(k))
                    grants.Add(k, denies[k]);
            }
            if (!IsGroup)
            {
                userService.UpdateUserAccess((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                {
                    if (exp != null)
                    {
                        appController.HandleException(exp);
                    }
                    else
                    {
                        OnRequestClose();
                    }

                }), Party.Id, grants);
            }
         


           // appController.Publish(new UpdatePartyCustomActionsArgs(grants, Party.PartyName)));
           // OnRequestClose();
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        } 

        #endregion
    }
}

