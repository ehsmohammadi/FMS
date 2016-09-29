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
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Voucher
{
    public class VoucherSetingVM : WorkspaceViewModel, IEventHandler<VoucherSetingArg>
    {

        #region Prop
        private IFuelController _fuelController;
        private IVoucherSetingController _voucherSetingController;


        private IVoucherSetingServiceWrapper _voucherSetingServiceWrapper;



        private ObservableCollection<ComboBoxItm> voucherTyps;
        public ObservableCollection<ComboBoxItm> VoucherTyps
        {
            get { return voucherTyps; }
            set { this.SetField(p => p.VoucherTyps, ref voucherTyps, value); }
        }

        private ObservableCollection<ComboBoxItm> _voucherDetailTyps;
        public ObservableCollection<ComboBoxItm> VoucherDetailTyps
        {
            get { return _voucherDetailTyps; }
            set { this.SetField(p => p.VoucherDetailTyps, ref _voucherDetailTyps, value); }
        }

        private CompanyDto _companyDto;
        public CompanyDto CompanyDto
        {
            get { return _companyDto; }
            set { this.SetField(p => p.CompanyDto, ref _companyDto, value); }
        }


        private VoucherSetingDto _voucherSetingDto;
        public VoucherSetingDto VoucherSetingDto
        {
            get { return _voucherSetingDto; }
            set { this.SetField(p => p.VoucherSetingDto, ref _voucherSetingDto, value); }
        }

        private VoucherSetingDetailDto _voucherSetingDetailDto;
        public VoucherSetingDetailDto VoucherSetingDetailDto
        {
            get { return _voucherSetingDetailDto; }
            set { this.SetField(p => p.VoucherSetingDetailDto, ref _voucherSetingDetailDto, value); }
        }


        private long _selectedCompanyId;
        public long SelectedCompanyId
        {
            get { return _selectedCompanyId; }
            set { this.SetField(p => p.SelectedCompanyId, ref _selectedCompanyId, value); }
        }


        private long _selectedVoucherTypeId;
        public long SelectedVoucherTypeId
        {
            get { return _selectedVoucherTypeId; }
            set
            {
                this.SetField(p => p.SelectedVoucherTypeId, ref _selectedVoucherTypeId, value);
                SetComboBox(value);

            }
        }

        private long _selectedVoucherDetailTypeId;
        public long SelectedVoucherDetailTypeId
        {
            get { return _selectedVoucherDetailTypeId; }
            set { this.SetField(p => p.SelectedVoucherDetailTypeId, ref _selectedVoucherDetailTypeId, value); }
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

        private CommandViewModel _addDetailCommand;
        public CommandViewModel AddDetailCommand
        {
            get
            {
                _addDetailCommand = new CommandViewModel("افزودن", new DelegateCommand(() =>
                {

                    _voucherSetingController.ShowAddVoucherSetingDetail(VoucherSetingDto.Id);
                }));
                return _addDetailCommand;
            }

        }

        private CommandViewModel _updateDetailCommand;
        public CommandViewModel UpdateDetailCommand
        {
            get
            {
                _updateDetailCommand = new CommandViewModel("ویرایش", new DelegateCommand(() =>
                {
                    if (VoucherSetingDetailDto == null || VoucherSetingDetailDto.Id == 0)
                        _fuelController
                            .ShowMessage("لطفا ایتم مورد نظر را انتخاب نمایید");
                    else
                    {
                        _voucherSetingController.ShowUpdateVoucherSetingDetail(VoucherSetingDetailDto.VoucherSetingId, VoucherSetingDetailDto.Id);
                    }

                }));
                return _updateDetailCommand;
            }

        }
        private CommandViewModel _deleteDetailCommand;
        public CommandViewModel DeleteDetailCommand
        {
            get
            {
                _deleteDetailCommand = new CommandViewModel("حذف", new DelegateCommand(() =>
                {
                    if (VoucherSetingDetailDto == null || VoucherSetingDetailDto.Id == 0)
                        _fuelController
                            .ShowMessage("لطفا ایتم مورد نظر را انتخاب نمایید");
                    else
                    {
                        ShowBusyIndicator("درحال انجام عملیات");
                        _voucherSetingServiceWrapper.DeleteVoucherSetingDetail(
                   (res, exp) =>
                   _fuelController.BeginInvokeOnDispatcher(() =>
                                                           {
                                                               {
                                                                   HideBusyIndicator();
                                                                   if (exp == null)
                                                                   {
                                                                       Load(VoucherSetingDetailDto.Id);
                                                                   }
                                                                   else
                                                                   {
                                                                       _fuelController.HandleException(exp);
                                                                   }
                                                               }
                                                           }), VoucherSetingDetailDto.VoucherSetingId, VoucherSetingDetailDto.Id);


                    }

                }));
                return _deleteDetailCommand;
            }

        }

        #endregion


        #region ctor
        public VoucherSetingVM(IFuelController fuelController,
                               ICompanyServiceWrapper companyServiceWrapper,
                               IGoodServiceWrapper goodServiceWrapper,
                               IVoucherSetingController voucherSetingController,
                               IVoucherSetingServiceWrapper voucherSetingServiceWrapper)
        {
            _fuelController = fuelController;
            _voucherSetingController = voucherSetingController;
            this.DisplayName = "افزودن/ویرایش تنظیمات اسناد مالی";
            _voucherSetingServiceWrapper = voucherSetingServiceWrapper;
            CompanyDto = new CompanyDto();
            VoucherTyps = new ObservableCollection<ComboBoxItm>();
            VoucherSetingDto = new VoucherSetingDto();
            VoucherSetingDto.Company = new CompanyDto();
        }
        #endregion

        #region Method
        public void Load()
        {
            VoucherTyps.Add(new ComboBoxItm() { Id = 0, Name = "<--انتخاب نمایید-->" });
            VoucherTyps.Add(new ComboBoxItm() { Id = 1, Name = "Invoice" });
            VoucherTyps.Add(new ComboBoxItm() { Id = 2, Name = "Receipt" });
            VoucherTyps.Add(new ComboBoxItm() { Id = 3, Name = "Issue" });
            VoucherTyps.Add(new ComboBoxItm() { Id = 4, Name = "SaleInvoice" });
            VoucherTyps.Add(new ComboBoxItm() { Id = 5, Name = "OffHire" });

            VoucherDetailTyps = new ObservableCollection<ComboBoxItm>()
                        {
                           
                            new ComboBoxItm(){Id = 0,Name ="<--انتخاب نمایید-->"},
                          
                        };
            CompanyDto = _fuelController.GetCurrentUser().User.CompanyDto;

        }



        void Submit()
        {
            if (VoucherSetingDto.Id == 0)
            {
                VoucherSetingDto.VoucherDetailTypeId = Convert.ToInt32(SelectedVoucherDetailTypeId);
                VoucherSetingDto.VoucherMainRefDescription = "x";
                VoucherSetingDto.VoucherMainDescription = "x";
                VoucherSetingDto.VoucherTypeId = Convert.ToInt32(SelectedVoucherTypeId);
                VoucherSetingDto.Company.Id = CompanyDto.Id;

                ShowBusyIndicator("درحال انجام عملیات");

                _voucherSetingServiceWrapper.AddVoucherSeting(
                    (res, exp) =>
                    _fuelController.BeginInvokeOnDispatcher(() =>
                                                            {
                                                                HideBusyIndicator();
                                                                if (exp == null)
                                                                {
                                                                    _fuelController.Publish<VoucherSetingArg>(new VoucherSetingArg());
                                                                    _fuelController.Close(this);
                                                                }
                                                                else
                                                                {
                                                                    _fuelController.HandleException(exp);
                                                                }
                                                            }), VoucherSetingDto);

            }
            else
            {
                VoucherSetingDto.VoucherDetailTypeId = Convert.ToInt32(SelectedVoucherDetailTypeId);
                VoucherSetingDto.VoucherMainRefDescription = "x";
                VoucherSetingDto.VoucherMainDescription = "x";
                VoucherSetingDto.VoucherTypeId = Convert.ToInt32(SelectedVoucherTypeId);
                ShowBusyIndicator("درحال انجام عملیات");

                _voucherSetingServiceWrapper.UpdateVoucherSeting(
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
                                                            }), VoucherSetingDto.Id, VoucherSetingDto);

            }
        }
        public void Load(long id)
        {
            Load();
            _voucherSetingServiceWrapper.GetById(
                (res, exp) =>
                    _fuelController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exp == null)
                            {
                                VoucherSetingDto = res;
                                SelectedVoucherDetailTypeId = res.VoucherDetailTypeId;
                                SelectedVoucherTypeId = res.VoucherTypeId;

                            }
                            else
                            {
                                _fuelController.HandleException(exp);
                            }
                        })
                , id);
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            _fuelController.Close(this);
        }


        void SetComboBox(long id)
        {
            VoucherDetailTyps.Clear();
            VoucherDetailTyps.Add(new ComboBoxItm() { Id = 0, Name = "<--انتخاب نمایید-->" });
            switch (id)
            {
                case 1:
                    {
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 6, Name = " خرید" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 16, Name = "Barging " });

                        break;
                    }
                case 2:
                    {
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 1, Name = " Charter In Start" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 8, Name = " Charter Out End" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 11, Name = "PlusCorrection" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 14, Name = "CharterInEndVariance" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 13, Name = "CharterOutStartVariance" });

                        break;
                    }
                case 3:
                    {
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 3, Name = " مصرف پایان سفر" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 4, Name = " مصرف پایان سال" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 5, Name = " مصرف پایان ماه" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 2, Name = " Charter Out Start" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 7, Name = " Charter In End" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 12, Name = "MinusCorrection" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 13, Name = "CharterOutStartVariance" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 14, Name = "CharterInEndVariance" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 15, Name = "Transfer" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 17, Name = "CharterInStartVariance" });
                        break;
                    }
                case 4:
                    {
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 15, Name = "Transfer" });

                        break;
                    }
                case 5:
                    {
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 9, Name = "Offhire CharterIn" });
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 10, Name = "Offhire CharterOut" });

                        break;
                    }
                default:
                    {
                        VoucherDetailTyps.Clear();
                        VoucherDetailTyps.Add(new ComboBoxItm() { Id = 0, Name = "<--انتخاب نمایید-->" });
                        break;
                    }

            }
        }


        #endregion






        public void Handle(VoucherSetingArg eventData)
        {
            Load(VoucherSetingDto.Id);
        }
    }
}
