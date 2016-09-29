using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Castle.Core.Internal;
using Castle.MicroKernel.Handlers;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Voucher
{
    public class VoucherListVM : WorkspaceViewModel
    {
        #region   prop

        public long CompanyId { get; set; }

        private long _voucherTypeId;
        public long VoucherTypeId
        {
            get { return _voucherTypeId; }
            set { this.SetField(p => p.VoucherTypeId, ref _voucherTypeId, value); }

        }

        private long _voucherItmId;
        public long VoucherItmId
        {
            get { return _voucherItmId; }
            set { this.SetField(p => p.VoucherItmId, ref _voucherItmId, value); }

        }


        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { this.SetField(p => p.CompanyName, ref _companyName, value); }
        }


        private long _stateId;
        public long StateId
        {
            get { return _stateId; }
            set { this.SetField(p => p.StateId, ref _stateId, value); }

        }


        private string _refNo;

        public string RefNo
        {
            get { return _refNo; }
            set { this.SetField(p => p.RefNo, ref _refNo, value); }

        }

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { this.SetField(p => p.FromDate, ref _fromDate, value); }

        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get { return _toDate; }
            set { this.SetField(p => p.ToDate, ref _toDate, value); }
        }

        private IFuelController _fuelController;

        private IVoucherServiceWrapper _voucherServiceWrapper;
        private readonly IVoucherController _voucherController;


        private ObservableCollection<JournalEntryDto> _journalEntryDtos;

        public ObservableCollection<JournalEntryDto> JournalEntryDtos
        {
            get { return _journalEntryDtos; }
            set { this.SetField(p => p.JournalEntryDtos, ref _journalEntryDtos, value); }
        }

        private DateTime _selectedVoucherDate;
        public DateTime SelectedVoucherDate
        {
            get { return _selectedVoucherDate; }
            set
            {
                this.SetField(p => p.SelectedVoucherDate, ref _selectedVoucherDate, value);


            }
        }


        private VoucherDto _selectedVoucherDto;
        public VoucherDto SelectedVoucherDto
        {
            get { return _selectedVoucherDto; }
            set
            {
                this.SetField(p => p.SelectedVoucherDto, ref _selectedVoucherDto, value);
                if (SelectedVoucherDto != null)
                    LoadById(SelectedVoucherDto.Id);

            }
        }


        private PagedSortableCollectionView<VoucherDto> _vouchers;

        public PagedSortableCollectionView<VoucherDto> Vouchers
        {
            get { return _vouchers; }
            set
            {
                this.SetField(p => p.Vouchers, ref _vouchers, value);
                Vouchers.Refresh();
            }
        }
        private PagedSortableCollectionView<VoucherTransferLogDto> _logs;

        public PagedSortableCollectionView<VoucherTransferLogDto> Logs
        {
            get { return _logs; }
            set
            {
                this.SetField(p => p.Logs, ref _logs, value);

            }
        }

        private ObservableCollection<ComboBoxItm> voucherItms;
        public ObservableCollection<ComboBoxItm> VoucherItms
        {
            get { return voucherItms; }
            set { this.SetField(p => p.VoucherItms, ref voucherItms, value); }
        }

        private ObservableCollection<ComboBoxItm> stateItms;
        public ObservableCollection<ComboBoxItm> StateItms
        {
            get { return stateItms; }
            set { this.SetField(p => p.StateItms, ref stateItms, value); }
        }

        private ObservableCollection<ComboBoxItm> voucherTyps;
        public ObservableCollection<ComboBoxItm> VoucherTyps
        {
            get { return voucherTyps; }
            set { this.SetField(p => p.VoucherTyps, ref voucherTyps, value); }
        }

        private ComboBoxItm _selectedVoucherType;
        public ComboBoxItm SelectedVoucherType
        {
            get { return _selectedVoucherType; }
            set { this.SetField(p => p.SelectedVoucherType, ref _selectedVoucherType, value); }
        }


        private CommandViewModel _searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                _searchCommand = new CommandViewModel("جستجو", new DelegateCommand(() =>
                {

                    Load(0);

                }));
                return _searchCommand;
            }

        }

        private CommandViewModel _sendCommand;
        public CommandViewModel SendCommand
        {
            get
            {

                _sendCommand = new CommandViewModel("ارسال به مالی", new DelegateCommand(() =>
                {
                    if (VoucherItmId != 0)
                    {

                        var date = MITD.Core.PDateHelper.GregorianToHijri(SelectedVoucherDate, false).Split('/');
                        Send(Vouchers.Where(c => c.IsSelected).Select(d => d.Id).ToList(), date[0] + date[1] + date[2], VoucherItmId.ToString());
                    }
                    else
                    {
                        _fuelController.ShowMessage("لطفا نوع حساب را مشخص نمایید");
                    }

                }));
                return _sendCommand;
        }

        }



        private CommandViewModel _showRefCommand;
        public CommandViewModel ShowRefCommand
        {
            get
            {
                _showRefCommand = new CommandViewModel("نمایش مرجع", new DelegateCommand(() =>
                {
                    if (SelectedVoucherDto==null || SelectedVoucherDto.Id == 0)
                       _fuelController.ShowMessage("لطفا سند موردنظر خودرا انتخاب نمایید");   
                    else
                        ShowRef();

                }));
                return _showRefCommand;
            }

        }

        private CommandViewModel _showPrintCommand;
        public CommandViewModel ShowPrintCommand
        {
            get
            {
                _showPrintCommand = new CommandViewModel("نمایش سند چاپی", new DelegateCommand(() =>
                {
                    if (SelectedVoucherDto == null || SelectedVoucherDto.Id == 0)
                         _fuelController.ShowMessage("لطفا سند موردنظر خودرا انتخاب نمایید");
                       
                    else
                        ShowPrint(SelectedVoucherDto.LocalVoucherNo);

                }));
                return _showPrintCommand;
            }

        }

        private CommandViewModel _updateCommand;
        public CommandViewModel UpdateCommand
        {
            get
            {
                _updateCommand = new CommandViewModel("بروزرسانی", new DelegateCommand(() =>
                {


                }));
                return _updateCommand;
            }

        }

        private CommandViewModel _rejectCommand;
        public CommandViewModel RejectCommand
        {
            get
            {
                _rejectCommand = new CommandViewModel("برگشت سند و صدور مجدد", new DelegateCommand(() =>
                {


                }));
                return _rejectCommand;
            }

        }


        #endregion

        #region   Ctor

        public VoucherListVM(
            IFuelController fuelController,
            IVoucherServiceWrapper voucherServiceWrapper,
            IVoucherController voucherController)
        {
            Vouchers = new PagedSortableCollectionView<VoucherDto>() { PageSize = 20 };
            JournalEntryDtos = new ObservableCollection<JournalEntryDto>();
            Logs = new PagedSortableCollectionView<VoucherTransferLogDto>();
            _fuelController = fuelController;
            _voucherServiceWrapper = voucherServiceWrapper;
            _voucherController = voucherController;
            var user = _fuelController.GetCurrentUser();
            CompanyName = user.User.CompanyDto.Name;
            CompanyId = user.User.CompanyDto.Id;
            SelectedVoucherType = new ComboBoxItm();
            VoucherItms = new ObservableCollection<ComboBoxItm>();
            StateId = 0;
            this.Vouchers.OnRefresh += (a, args) => Load(Vouchers.PageIndex + 1);
            this.DisplayName = "اسناد مالی";
        }

        #endregion

        #region   Method

        public void Load(int pageIndex)
        {

            VoucherTyps = new ObservableCollection<ComboBoxItm>()
                        {
                            
                           new ComboBoxItm(){Id = 0,Name ="<--انتحاب نمایید-->"},
                            new ComboBoxItm(){Id = 1,Name ="رسید Charter In Start"},
                            new ComboBoxItm(){Id = 2,Name ="حواله Charter Out Start"},
                            new ComboBoxItm(){Id = 3,Name ="حواله مصرف پایان سفر"},
                            new ComboBoxItm(){Id = 4,Name ="حواله مصرف پایان سال"},
                            new ComboBoxItm(){Id = 5,Name ="حواله مصرف پایان ماه"},
                            new ComboBoxItm(){Id = 6,Name ="رسید خرید"},
                            new ComboBoxItm(){Id = 7,Name ="حواله Charter In End"},
                            new ComboBoxItm(){Id = 8,Name ="رسید Charter Out End"},
                            new ComboBoxItm(){Id = 9,Name ="Offhire Charter Out"},
                            new ComboBoxItm(){Id = 10,Name ="Offhire Charter In"},
                            new ComboBoxItm(){Id = 11,Name ="Plus Correction"},
                            new ComboBoxItm(){Id = 12,Name ="Mints Correction"},
                            new ComboBoxItm(){Id = 13,Name ="رسید برگشتی CharterOutStart Variance"},
                            new ComboBoxItm(){Id = 13,Name ="حواله مصرف CharterOutStart Variance "},
                            new ComboBoxItm(){Id = 14,Name ="رسید برگشتی CharterInEnd Variance"},
                            new ComboBoxItm(){Id = 14,Name ="حواله مصرف CharterInEnd Variance "},
                            new ComboBoxItm(){Id = 15,Name ="انتقال (فروش انتقالی)"}
                          

                        };


            StateItms = new ObservableCollection<ComboBoxItm>()
                        {
                            new ComboBoxItm(){Id = 0,Name ="<--انتحاب نمایید-->"},
                           new ComboBoxItm(){Id=1,Name="ارسال شده"}
                            ,new ComboBoxItm(){Id=2,Name="ارسال نشده"}
                            ,new ComboBoxItm(){Id=3,Name="خطا در ارسال"}
                        };

            VoucherItms = new ObservableCollection<ComboBoxItm>()
            {

                new ComboBoxItm(){Id = 0,Name ="<--انتحاب نمایید-->"},
                new ComboBoxItm(){Id=11200,Name="حسابداری درآمدها"},
                new ComboBoxItm(){Id=11300,Name="حسابداری مدیریت"},
                new ComboBoxItm(){Id=11100,Name="حسابداری هزینه ها"},
 
            };

            ShowBusyIndicator("درحال دریافت اطلاعات");



            _voucherServiceWrapper.GetByFilter((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                                                                                       {
                                                                                           HideBusyIndicator();
                                                                                           if (exp == null)
                                                                                           {
                                                                                               Vouchers.Clear();
                                                                                               JournalEntryDtos.Clear();
                                                                                               Logs.Clear();
                                                                                               res.Result.ForEach(
                                                                                                   c =>
                                                                                                   {
                                                                                                       if (c.VoucherDetailTypeId != 0)
                                                                                                       {
                                                                                                           c.VoucherDetailTypeName
                                                                                                                        =
                              VoucherTyps.First(d => d.Id == c.VoucherDetailTypeId).Name;
                                                                                                       }

                                                                                                       //Vouchers.Add(c);
                                                                                                   });

                                                                                               Vouchers.SourceCollection = res.Result;

                                                                                               Vouchers.TotalItemCount = res.TotalCount;

                                                                                               Vouchers.PageIndex = Math.Max(0, res.CurrentPage - 1);

                                                                                               Vouchers.PageSize = res.PageSize;


                                                                                           }
                                                                                           else
                                                                                           {
                                                                                               _fuelController.HandleException(exp);
                                                                                           }
                                                                                       }), CompanyId,

                                    (FromDate == new DateTime(1, 1, 1)) ? null : HttpUtil.DateTimeToString(FromDate),
                                    (ToDate == new DateTime(1, 1, 1)) ? null : HttpUtil.DateTimeToString(ToDate),
                                   (int)VoucherTypeId, RefNo, StateId.ToString(), pageIndex, Vouchers.PageSize);

        }


        void LoadById(long id)
        {
            _voucherServiceWrapper.GetById((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
                                                                                                 {
                                                                                                     JournalEntryDtos
                                                                                                         .Clear();
                                                                                                     Logs.Clear();
                                                                                                     if (exp != null)
                                                                                                     {
                                                                                                         _fuelController.HandleException(exp);
                                                                                                     }
                                                                                                     else
                                                                                                     {

                                                                                                         if (res.JournalEntryDtos != null)
                                                                                                             res.JournalEntryDtos.ForEach(c
                                                                                                                 => JournalEntryDtos.Add(c));
                                                                                                         if (res.VoucherTransferLogDto != null)
                                                                                                             res.VoucherTransferLogDto.ForEach(c => Logs.Add(c));
                                                                                                     }
                                                                                                 }), id);
        }

        void Send(List<long> ids, string date, string code)
        {
            ShowBusyIndicator("درحال ارسال اطلاعات");
            _voucherServiceWrapper.SendToFinancial((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
             {
                 HideBusyIndicator();
                 if (exp == null)
                 {
                     Load(0);
                 }
                 else
                 {
                     _fuelController.HandleException(exp);
                 }
             }), ids, date, code);
        }

        void ShowRef()
        {
            _voucherServiceWrapper.GetEntityId((res, exp) => _fuelController.BeginInvokeOnDispatcher(() =>
            {

                if (exp != null)
                {
                    _fuelController.HandleException(exp);
                }
                else
                {

                    _voucherController.ShowRefrence(res.EntityTypeName, res.Id);
                }
            }), SelectedVoucherDto.ReferenceNo);
        }

        void ShowPrint(string no)
        {
            _voucherController.ShowPrint(no);
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            _fuelController.Close(this);
        }

    }

        #endregion


}
