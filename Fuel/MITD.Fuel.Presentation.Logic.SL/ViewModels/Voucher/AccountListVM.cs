using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Castle.Core.Internal;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Voucher
{
    public class AccountListVM : WorkspaceViewModel
    {

        #region Prop

        private string _name;
        public string Name
        {
            get { return _name; }
            set { this.SetField(p => p.Name, ref _name, value); }
        }
        private string _code;
        public string Code
        {
            get { return _code; }
            set { this.SetField(p => p.Code, ref _code, value); }
        }


        private int _typ;
        public int Typ
        {
            get { return _typ; }
            set { this.SetField(p => p.Typ, ref _typ, value); }
        }
        private IFuelController _fuelController;

        private IAccountServiceWrapper _accountServiceWrapper;

        private PagedSortableCollectionView<AccountDto> _accountDtos;
        public PagedSortableCollectionView<AccountDto> AccountDtos
        {
            get { return _accountDtos; }
            set { this.SetField(p => p.AccountDtos, ref _accountDtos, value); }
        }

        private AccountDto _accountDto;
        public AccountDto AccountDto
        {
            get { return _accountDto; }
            set { this.SetField(p => p.AccountDto, ref _accountDto, value); }
        }


        private CommandViewModel _searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                _searchCommand = new CommandViewModel("جستجو", new DelegateCommand(() =>
                {

                    Load(0,this.Typ);

                }));
                return _searchCommand;
            }

        }

        private CommandViewModel _submitCommand;
        public CommandViewModel SubmitCommand
        {
            get
            {
                _submitCommand = new CommandViewModel("تأیید", new DelegateCommand(() =>
                {
                    _fuelController.Publish<AccountChangeArg>(new AccountChangeArg()
                                                              {
                                                                  AccountDto = this.AccountDto,
                                                                  Type = Typ
                                                              });
                    _fuelController.Close(this);
                }));
                return _submitCommand;
            }

        }

        private CommandViewModel _closeCommand;
        public CommandViewModel CloseCommand
        {
            get
            {
                _closeCommand = new CommandViewModel("خروج", new DelegateCommand(() =>
                {
                    _fuelController.Close(this);

                }));
                return _closeCommand;
            }

        }

        #endregion


        public AccountListVM(IFuelController fuelController, IAccountServiceWrapper accountServiceWrapper)
        {
            _fuelController = fuelController;
            _accountServiceWrapper = accountServiceWrapper;
            this.DisplayName = "لیست حسابهای معین";
            AccountDtos=new PagedSortableCollectionView<AccountDto>();
            AccountDto=new AccountDto();
        }

        public void Load(int pageIndex,int typ)
        {
            this.Typ = typ;
            ShowBusyIndicator("درحال دریافت اطلاعات");
            _accountServiceWrapper.GetByFilter((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                        {
                                                  HideBusyIndicator();
                                                  if (exp == null)
                                                  {
                                                      AccountDtos.Clear();

                                                      AccountDtos.SourceCollection = res.Result;

                                                      AccountDtos.TotalItemCount = res.TotalCount;

                                                      AccountDtos.PageSize = res.PageSize;

                                                      AccountDtos.PageIndex = Math.Min(AccountDtos.PageIndex, AccountDtos.PageCount - 1);
                                                  }
                                                  else
                                                  {
                                                      _fuelController.HandleException(exp);
                                                  }
                        }), Name, Code, AccountDtos.PageIndex, 10);


        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            _fuelController.Close(this);
        }
    }
}
