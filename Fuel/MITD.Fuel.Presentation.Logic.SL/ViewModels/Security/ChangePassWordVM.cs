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
    public class ChangePassWordVM : WorkspaceViewModel
    {
        #region Fields

        private readonly IFuelController appController;
        private readonly IUserSecurityServiceWrapper userService;
   
        #endregion

        #region Properties & BackField

        private string rnewPass;
        public string RNewPass
        {
            get { return rnewPass; }
            set { this.SetField(vm => vm.RNewPass, ref rnewPass, value); }
        }


        private string newPass;
        public string NewPass
        {
            get { return newPass; }
            set { this.SetField(vm => vm.NewPass, ref newPass, value); }
        }

        private string oldPass;
        public string OldPass
        {
            get { return oldPass; }
            set { this.SetField(vm => vm.OldPass, ref oldPass, value); }
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

   
        #endregion

        #region Constructors

        public ChangePassWordVM()
        {

        }

        public ChangePassWordVM(IUserSecurityServiceWrapper userService,
                           IFuelController appController,
                           IUserController userController,
            ICompanyServiceWrapper companyServiceWrapper)
        {

            this.userService = userService;
            this.appController = appController;
          
        }

        #endregion

        #region Methods

        private void save()
        {
            if (string.IsNullOrEmpty(NewPass) || string.IsNullOrEmpty(RNewPass) || string.IsNullOrEmpty(OldPass))
            {
                this.appController.ShowMessage("لطفا تمامی فیلدها را پرنمایید");
            }
            else
            {
                if (NewPass != RNewPass)
                {
                    this.appController.ShowMessage("رمز عبور و تکرار رمزعبور می بایست یکسان باشد");
                }
                else
                {
                    ShowBusyIndicator();
                    userService.ChangePassWord((res,exp)=>appController.BeginInvokeOnDispatcher(() =>
                    {
                        if (exp!=null)
                        {
                            this.appController.HandleException(exp);
                        }
                        else
                        {
                            if (this.appController.ShowMessage("تغییر رمز با موفقیت انجام شد", "پیام", MessageBoxButton.OK)==MessageBoxResult.OK)
                            {
                                OnRequestClose();
                            }
                           
                        }
                        
                    }),NewPass,OldPass);
                }
            }
        }

     

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        }


      


        #endregion


    }
}

