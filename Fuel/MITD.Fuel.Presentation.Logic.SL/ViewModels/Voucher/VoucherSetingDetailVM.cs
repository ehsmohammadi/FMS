using System;
using System.Collections.Generic;
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
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Events;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Voucher
{
    public class VoucherSetingDetailVM : WorkspaceViewModel, IEventHandler<AccountChangeArg>
    {
        #region Prop
        private IFuelController _fuelController;
        private IVoucherSetingController _voucherSetingController;

        private IGoodServiceWrapper _goodServiceWrapper;
        private IVoucherSetingServiceWrapper _voucherSetingServiceWrapper;


        private ObservableCollection<GoodDto> _goodDtos;
        public ObservableCollection<GoodDto> GoodDtos
        {
            get { return _goodDtos; }
            set { this.SetField(p => p.GoodDtos, ref _goodDtos, value); }
        }

        private VoucherSetingDetailDto _voucherSetingDto;
        public VoucherSetingDetailDto VoucherSetingDetailDto
        {
            get { return _voucherSetingDto; }
            set { this.SetField(p => p.VoucherSetingDetailDto, ref _voucherSetingDto, value); }
        }



        private long _selectedGoodId;
        public long SelectedGoodId
        {
            get { return _selectedGoodId; }
            set { this.SetField(p => p.SelectedGoodId, ref _selectedGoodId, value); }
        }


        private long _voucherSetingId;
        public long VoucherSetingId
        {
            get { return _voucherSetingId; }
            set { this.SetField(p => p.VoucherSetingId, ref _voucherSetingId, value); }
        }


        private string _debitCode;
        public string DebitCode
        {
            get { return _debitCode; }
            set
            {
                this.SetField(p => p.DebitCode, ref _debitCode, value);

            }
        }

        private string _creditCode;
        public string CreditCode
        {
            get { return _creditCode; }
            set
            {
                this.SetField(p => p.CreditCode, ref _creditCode, value);

            }
        }





        private bool _isDebitVessel;
        public bool IsDebitVessel
        {
            get { return _isDebitVessel; }
            set
            {
                this.SetField(p => p.IsDebitVessel, ref _isDebitVessel, value);

            }
        }

        private bool _isDebitVoyage;
        public bool IsDebitVoyage
        {
            get { return _isDebitVoyage; }
            set
            {
                this.SetField(p => p.IsDebitVoyage, ref _isDebitVoyage, value);

            }
        }

        private bool _isDebitPort;
        public bool IsDebitPort
        {
            get { return _isDebitPort; }
            set
            {
                this.SetField(p => p.IsDebitPort, ref _isDebitPort, value);

            }
        }

        private bool _isDebitCompany;
        public bool IsDebitCompany
        {
            get { return _isDebitCompany; }
            set
            {
                this.SetField(p => p.IsDebitCompany, ref _isDebitCompany, value);

            }
        }



        private bool _isCreditVessel;
        public bool IsCreditVessel
        {
            get { return _isCreditVessel; }
            set
            {
                this.SetField(p => p.IsCreditVessel, ref _isCreditVessel, value);

            }
        }

        private bool _isCreditVoyage;
        public bool IsCreditVoyage
        {
            get { return _isCreditVoyage; }
            set
            {
                this.SetField(p => p.IsCreditVoyage, ref _isCreditVoyage, value);

            }
        }

        private bool _isCreditPort;
        public bool IsCreditPort
        {
            get { return _isCreditPort; }
            set
            {
                this.SetField(p => p.IsCreditPort, ref _isCreditPort, value);

            }
        }

        private bool _isCreditCompany;
        public bool IsCreditCompany
        {
            get { return _isCreditCompany; }
            set
            {
                this.SetField(p => p.IsCreditCompany, ref _isCreditCompany, value);

            }
        }



        private CommandViewModel _showDebitAccountCommand;
        public CommandViewModel ShowDebitAccountCommand
        {
            get
            {
                _showDebitAccountCommand = new CommandViewModel("...", new DelegateCommand(() =>
                {

                    _voucherSetingController.ShowLookUpAccount(1);
                }));
                return _showDebitAccountCommand;
            }

        }
        private CommandViewModel _showAccountCommand;
        public CommandViewModel ShowAccountCommand
        {
            get
            {
                _showAccountCommand = new CommandViewModel("...", new DelegateCommand(() =>
                {

                    _voucherSetingController.ShowLookUpAccount(2);
                }));
                return _showAccountCommand;
            }

        }

        private CommandViewModel _submitCommand;
        public CommandViewModel SubmitCommand
        {
            get
            {
                _submitCommand = new CommandViewModel("تأیید", new DelegateCommand(() =>
                {
                    Submit();

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


        public VoucherSetingDetailVM(IFuelController fuelController,
                               IGoodServiceWrapper goodServiceWrapper,
                               IVoucherSetingController voucherSetingController,
                               IVoucherSetingServiceWrapper voucherSetingServiceWrapper)
        {
            _fuelController = fuelController;
            _voucherSetingController = voucherSetingController;
            this.DisplayName = "افزودن/ویرایش تنظیمات حساب های اسناد مالی";
            _goodServiceWrapper = goodServiceWrapper;
            _voucherSetingServiceWrapper = voucherSetingServiceWrapper;
            GoodDtos = new ObservableCollection<GoodDto>();
            VoucherSetingDetailDto = new VoucherSetingDetailDto();
            VoucherSetingDetailDto.CreditSegmentTypes = new List<int>();
            VoucherSetingDetailDto.DebitSegmentTypes = new List<int>();
            VoucherSetingDetailDto.DebitAccountDto = new AccountDto();
            VoucherSetingDetailDto.CreditAccountDto = new AccountDto();


        }

        #region Method
        public void Load(long vocherSetingId, long voucherSetingDetailId)
        {

            this.VoucherSetingId = vocherSetingId;
            _voucherSetingServiceWrapper.GetDetailById(
                (res, exp) =>
                    _fuelController.BeginInvokeOnDispatcher(() =>
                                                            {
                                                                if (exp == null)
                                                                {
                                                                    VoucherSetingDetailDto = res;
                                                                    SelectedGoodId = res.GoodDto.Id;
                                                                    DebitCode = res.DebitAccountDto.Code;
                                                                    CreditCode = res.CreditAccountDto.Code;
                                                                    SetSegmentCredit(res.CreditSegmentTypes);
                                                                    SetSegmentDebit(res.DebitSegmentTypes);
                                                                }
                                                                else
                                                                {
                                                                    _fuelController.HandleException(exp);
                                                                }
                                                            }
                    )
                , vocherSetingId, voucherSetingDetailId);


            Load(vocherSetingId);

        }

        public void Load(long vocherSetingId)
        {
            this.VoucherSetingId = vocherSetingId;

            ShowBusyIndicator("درحال دریافت اطلاعات ...");
            _goodServiceWrapper.GetAll((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {
                HideBusyIndicator();
                if (exp == null)
                {

                    foreach (var goodDto in res)
                    {
                        GoodDtos.Add(goodDto);
                    }

                }
                else
                {
                    _fuelController
                        .
                        HandleException
                        (exp);
                }


            }), "", _fuelController.GetCurrentUser().User.CompanyDto.Id);

        }
        void Submit()
        {
            if (VoucherSetingDetailDto.Id == 0)
            {
                VoucherSetingDetailDto.GoodDto = new GoodDto() { Id = SelectedGoodId };
                VoucherSetingDetailDto.VoucherSetingId = VoucherSetingId;

                GetSegment();

                _voucherSetingServiceWrapper.AddVoucherSetingDetail(
                    (res, exp) =>
                    _fuelController.BeginInvokeOnDispatcher(() =>
                                                            {
                                                                if (exp == null)
                                                                {
                                                                    _fuelController.Publish<VoucherSetingArg>(new VoucherSetingArg());
                                                                    _fuelController.Close(this);
                                                                }
                                                                else
                                                                {
                                                                    _fuelController.HandleException(exp);
                                                                }
                                                            }), VoucherSetingDetailDto);

            }
            else
            {
                VoucherSetingDetailDto.GoodDto = new GoodDto() { Id = SelectedGoodId };
                GetSegment();
                _voucherSetingServiceWrapper.UpdateVoucherSetingDetail(
                    (res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                        {
                            if (exp == null)
                            {
                                _fuelController.Publish<VoucherSetingArg>(new VoucherSetingArg());
                                _fuelController.Close(this);
                            }
                            else
                            {
                                _fuelController.HandleException(exp);
                            }

                        }),VoucherSetingDetailDto.Id,VoucherSetingDetailDto);


            }
        }
        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            _fuelController.Close(this);
        }

        public void Handle(AccountChangeArg eventData)
        {
            if (eventData.Type == 1)
            {
                DebitCode = eventData.AccountDto.Code;
                VoucherSetingDetailDto.DebitAccountDto = eventData.AccountDto;
            }
            else if (eventData.Type == 2)
            {
                CreditCode = eventData.AccountDto.Code;
                VoucherSetingDetailDto.CreditAccountDto = eventData.AccountDto;
            }

        }



        void SetSegmentDebit(List<int> segments)
        {
            IsDebitCompany = false;
            IsDebitPort = false;
            IsDebitVessel = false;
            IsDebitVoyage = false;

            if (segments.Contains(1))
                IsDebitVessel = true;
            if (segments.Contains(2))
                IsDebitPort = true;
            if (segments.Contains(3))
                IsDebitVoyage = true;
            if (segments.Contains(4))
                IsDebitCompany = true;

        }
        void SetSegmentCredit(List<int> segments)
        {
            IsCreditCompany = false;
            IsCreditPort = false;
            IsCreditVessel = false;
            IsCreditVoyage = false;

            if (segments.Contains(1))
                IsCreditVessel = true;
            if (segments.Contains(2))
                IsCreditPort = true;
            if (segments.Contains(3))
                IsCreditVoyage = true;
            if (segments.Contains(4))
                IsCreditCompany = true;

        }

        private void GetSegment()
        {
            VoucherSetingDetailDto.CreditSegmentTypes.Clear();
            VoucherSetingDetailDto.DebitSegmentTypes.Clear();

            if (IsCreditVessel == true)
                VoucherSetingDetailDto.CreditSegmentTypes.Add(1);
            if (IsCreditPort == true)
                VoucherSetingDetailDto.CreditSegmentTypes.Add(2);
            if (IsCreditVoyage == true)
                VoucherSetingDetailDto.CreditSegmentTypes.Add(3);
            if (IsCreditCompany == true)
                VoucherSetingDetailDto.CreditSegmentTypes.Add(4);

            if (IsDebitVessel == true)
                VoucherSetingDetailDto.DebitSegmentTypes.Add(1);
            if (IsDebitPort == true)
                VoucherSetingDetailDto.DebitSegmentTypes.Add(2);
            if (IsDebitVoyage == true)
                VoucherSetingDetailDto.DebitSegmentTypes.Add(3);
            if (IsDebitCompany == true)
                VoucherSetingDetailDto.DebitSegmentTypes.Add(4);
        }

        #endregion

    }
}
