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
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.BasicInfo
{
    public class FinancialAccountListVM : WorkspaceViewModel
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

        private string _orgName;
        public string OrgName
        {
            get { return _orgName; }
            set { this.SetField(p => p.OrgName, ref _orgName, value); }
        }
        private string _orgCode;
        public string OrgCode
        {
            get { return _orgCode; }
            set { this.SetField(p => p.OrgCode, ref _orgCode, value); }
        }
        private int _typ;
        public int Typ
        {
            get { return _typ; }
            set { this.SetField(p => p.Typ, ref _typ, value); }
        }
        private IFuelController _fuelController;

        private IAccountServiceWrapper _accountServiceWrapper;

        private IOriginalAccountServiceWrapper _originalAccountServiceWrapper;

        private PagedSortableCollectionView<AccountDto> _accountDtos;
        public PagedSortableCollectionView<AccountDto> AccountDtos
        {
            get { return _accountDtos; }
            set { this.SetField(p => p.AccountDtos, ref _accountDtos, value); }
        }

        private PagedSortableCollectionView<AccountDto> _originalAccountDtos;
        public PagedSortableCollectionView<AccountDto> OriginalAccountDtos
        {
            get { return _originalAccountDtos; }
            set { this.SetField(p => p.OriginalAccountDtos, ref _originalAccountDtos, value); }
        }

        private AccountDto _accountDto;
        public AccountDto AccountDto
        {
            get { return _accountDto; }
            set { this.SetField(p => p.AccountDto, ref _accountDto, value); }
        }

        private AccountDto _originalAccountDto;
        public AccountDto OriginalAccountDto
        {
            get { return _originalAccountDto; }
            set { this.SetField(p => p.OriginalAccountDto, ref _originalAccountDto, value); }
        }

        private CommandViewModel _searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                _searchCommand = new CommandViewModel("جستجو", new DelegateCommand(() =>
                {

                    LoadAccountDtos();

                }));
                return _searchCommand;
            }

        }

        private CommandViewModel _searchOrgCommand;
        public CommandViewModel SearchOrgCommand
        {
            get
            {
                _searchOrgCommand = new CommandViewModel("جستجو", new DelegateCommand(() =>
                {

                    LoadOriginalAccountDtos(this.Typ);

                }));
                return _searchOrgCommand;
            }

        }


        private CommandViewModel _submitCommand;
        public CommandViewModel SubmitCommand
        {
            get
            {
                _submitCommand = new CommandViewModel("افزودن", new DelegateCommand(() =>
                {
                    if (string.IsNullOrEmpty(OriginalAccountDto.Code))
                    {
                        _fuelController.ShowMessage("لطفا حساب را انتخاب نمایید");
                    }
                    else
                    {
                        Add(OriginalAccountDto);
                    }


                }));
                return _submitCommand;
            }

        }



        #endregion

        public FinancialAccountListVM(IFuelController fuelController, IAccountServiceWrapper accountServiceWrapper, IOriginalAccountServiceWrapper originalAccountServiceWrapper)
        {
            _fuelController = fuelController;
            _accountServiceWrapper = accountServiceWrapper;
            _originalAccountServiceWrapper = originalAccountServiceWrapper;
            this.DisplayName = "افزودن به حسابهای معین";
            AccountDtos = new PagedSortableCollectionView<AccountDto>();
            AccountDto = new AccountDto();
            OriginalAccountDtos = new PagedSortableCollectionView<AccountDto>();
            OriginalAccountDto = new AccountDto();
            //this.AccountDtos.OnRefresh += (a, args) => LoadAccountDtos(AccountDtos.PageIndex + 1);
            this.AccountDtos.PageChanged += (a, args) => LoadAccountDtos();
            //this.OriginalAccountDtos.OnRefresh += (a, args) => LoadOriginalAccountDtos(OriginalAccountDtos.PageIndex + 1,0);
            this.OriginalAccountDtos.PageChanged += (a, args) => LoadOriginalAccountDtos(0);
        }

        #region Method

        public void Load()
        {
            LoadAccountDtos();
            LoadOriginalAccountDtos(0);

        }
        void LoadOriginalAccountDtos(int typ)
        {
            this.Typ = typ;
            ShowBusyIndicator("درحال دریافت اطلاعات");
            _originalAccountServiceWrapper.GetByFilter((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    OriginalAccountDtos.Clear();


                    OriginalAccountDtos.SourceCollection = res.Result;

                    OriginalAccountDtos.TotalItemCount = res.TotalCount;

                    //OriginalAccountDtos.PageIndex = Math.Max(0, res.CurrentPage - 1);

                    OriginalAccountDtos.PageSize = res.PageSize;

                    OriginalAccountDtos.PageIndex = Math.Min(OriginalAccountDtos.PageIndex, OriginalAccountDtos.PageCount - 1);
                }
                else
                {
                    _fuelController.HandleException(exp);
                }

            }), OrgName, OrgCode, OriginalAccountDtos.PageIndex, 10);
            //}), OrgName, OrgCode, pageIndex, 10);


        }
        void LoadAccountDtos()
        {

            ShowBusyIndicator("درحال دریافت اطلاعات");
            _accountServiceWrapper.GetByFilter((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    AccountDtos.Clear();


                    AccountDtos.SourceCollection = res.Result;

                    AccountDtos.TotalItemCount = res.TotalCount;

                    //AccountDtos.PageIndex = Math.Max(0, res.CurrentPage - 1);

                    AccountDtos.PageSize = res.PageSize;

                    AccountDtos.PageIndex = Math.Min(AccountDtos.PageIndex, AccountDtos.PageCount - 1);

                }
                else
                {
                    _fuelController.HandleException(exp);
                }

            }), Name, Code, AccountDtos.PageIndex, 10);
            //}), Name, Code, pageIndex , 10);


        }
        void Add(AccountDto accountDto)
        {

            ShowBusyIndicator("درحال دریافت اطلاعات");
            _accountServiceWrapper.Add((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {
                    // Load(AccountDtos.PageIndex);
                    LoadAccountDtos();
                }
                else
                {
                    _fuelController.HandleException(exp);
                }

            }), accountDto);


        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            _fuelController.Close(this);
        }



        #endregion

    }
}
