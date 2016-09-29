using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using Castle.Core.Internal;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels.Voucher
{
    public class VoucherSetingListVM : WorkspaceViewModel, IEventHandler<VoucherSetingArg>
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



        private ObservableCollection<ComboBoxItm> voucherTypsColl;
        public ObservableCollection<ComboBoxItm> VoucherTypsColl
        {
            get { return voucherTypsColl; }
            set { this.SetField(p => p.VoucherTypsColl, ref voucherTypsColl, value); }
        }

        private ObservableCollection<ComboBoxItm> _voucherDetailTypsColl;
        public ObservableCollection<ComboBoxItm> VoucherDetailTypsColl
        {
            get { return _voucherDetailTypsColl; }
            set { this.SetField(p => p.VoucherDetailTypsColl, ref _voucherDetailTypsColl, value); }
        }



        private CompanyDto _companyDto;
        public CompanyDto CompanyDto
        {
            get { return _companyDto; }
            set { this.SetField(p => p.CompanyDto, ref _companyDto, value); }
        }


        private PagedSortableCollectionView<VoucherSetingDto> _voucherSetingDtos;
        public PagedSortableCollectionView<VoucherSetingDto> VoucherSetingDtos
        {
            get { return _voucherSetingDtos; }
            set { this.SetField(p => p.VoucherSetingDtos, ref _voucherSetingDtos, value); }
        }



        private VoucherSetingDto _voucherSetingDto;
        public VoucherSetingDto VoucherSetingDto
        {
            get { return _voucherSetingDto; }
            set
            {
                this.SetField(p => p.VoucherSetingDto, ref _voucherSetingDto, value);
            }
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


        private CommandViewModel _searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                _searchCommand = new CommandViewModel("جستجو", new DelegateCommand(() =>
                {

                    LoadVoucherSeting();
                }));
                return _searchCommand;
            }

        }

        private CommandViewModel _addCommand;
        public CommandViewModel AddCommand
        {
            get
            {
                _addCommand = new CommandViewModel("افزودن", new DelegateCommand(() =>
                {

                    _voucherSetingController.ShowAddVoucherSeting();
                }));
                return _addCommand;
            }

        }

        private CommandViewModel _updateCommand;
        public CommandViewModel UpdateCommand
        {
            get
            {
                _updateCommand = new CommandViewModel("ویرایش", new DelegateCommand(() =>
                                                                                    {
                                                                                        if (VoucherSetingDto == null || VoucherSetingDto.Id == 0)
                                                                                            _fuelController
                                                                                                .ShowMessage("لطفا ایتم مورد نظر را انتخاب نمایید");
                                                                                        else
                                                                                        {
                                                                                            _voucherSetingController.ShowUpdateVoucherSeting(VoucherSetingDto.Id);
                                                                                        }

                                                                                    }));
                return _updateCommand;
            }

        }

        private CommandViewModel _deleteCommand;
        public CommandViewModel DeleteCommand
        {
            get
            {
                _deleteCommand = new CommandViewModel("حذف", new DelegateCommand(() =>
                {

                    LoadVoucherSeting();
                }));
                return _deleteCommand;
            }

        }


        #endregion

        #region Ctor

        public VoucherSetingListVM(IFuelController fuelController,
                               IVoucherSetingController voucherSetingController,
                               IVoucherSetingServiceWrapper voucherSetingServiceWrapper)
        {
            _fuelController = fuelController;
            _voucherSetingController = voucherSetingController;
            this.DisplayName = "تنظیمات اسناد مالی";

            _voucherSetingServiceWrapper = voucherSetingServiceWrapper;
            CompanyDto = new CompanyDto();

            VoucherSetingDtos = new PagedSortableCollectionView<VoucherSetingDto>();
            VoucherDetailTypsColl=new ObservableCollection<ComboBoxItm>();
           voucherTypsColl=new ObservableCollection<ComboBoxItm>();
            SetDetailCollection();
        }
        #endregion

        #region Method


        public void Load()
        {
            VoucherTyps = new ObservableCollection<ComboBoxItm>()
                        {
                            
                            new ComboBoxItm(){Id = 0,Name ="<--انتخاب نمایید-->"},
                            new ComboBoxItm(){Id = 1,Name ="Invoice"},
                            new ComboBoxItm(){Id = 2,Name ="Receipt"},
                            new ComboBoxItm(){Id = 3,Name ="Issue"},
                            new ComboBoxItm(){Id = 4,Name ="SaleInvoice"},
                            new ComboBoxItm() {Id = 5, Name = "OffHire"}
                         
                        };
            VoucherDetailTyps = new ObservableCollection<ComboBoxItm>()
                        {
                            
                            new ComboBoxItm(){Id = 0,Name ="<--انتخاب نمایید-->"},
                        
                        };
            CompanyDto = _fuelController.GetCurrentUser().User.CompanyDto;



        }

        void LoadVoucherSeting()
        {


            ShowBusyIndicator("درحال دریافت اطلاعات ...");
            _voucherSetingServiceWrapper.GetByFilter(
                (res, exp) =>
                    _fuelController.BeginInvokeOnDispatcher(
                    () =>
                    {
                        HideBusyIndicator();
                        if (exp == null)
                        {
                            VoucherSetingDtos.Clear();

                      
                                res.Result.ForEach(c =>
                                                   {
                                                       c.VoucherTypeName =
                                                           VoucherDetailTypsColl.First(
                                                               d => d.Id == c.VoucherDetailTypeId).Name + "  " +
                                                           VoucherTypsColl.First(e => e.Id == c.VoucherTypeId).Name;

                                                   });


                            VoucherSetingDtos.SourceCollection = res.Result;
                            VoucherSetingDtos.TotalItemCount = res.TotalCount;
                            VoucherSetingDtos.PageSize = res.PageSize;
                            VoucherSetingDtos.PageIndex = Math.Max(0, res.CurrentPage - 1);



                        }
                        else
                        {
                            _fuelController.HandleException(exp);
                        }
                    }),
                                   _fuelController.GetCurrentUser().User.CompanyDto.Id,
                                  Convert.ToInt32(SelectedVoucherTypeId),
                                   Convert.ToInt32(SelectedVoucherDetailTypeId),
                                   0,
                                  100);

        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            _fuelController.Close(this);
        }
        #endregion

        public void Handle(VoucherSetingArg eventData)
        {
            LoadVoucherSeting();
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

        void SetDetailCollection()
        {
            VoucherDetailTypsColl.Clear();

            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 3, Name = " مصرف پایان سفر" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 4, Name = " مصرف پایان سال" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 5, Name = " مصرف پایان ماه" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 2, Name = " Charter Out Start" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 7, Name = " Charter In End" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 12, Name = "MinusCorrection" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 15, Name = "Transfer" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 9, Name = "Offhire CharterIn" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 10, Name = "Offhire CharterOut" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 1, Name = " Charter In Start" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 8, Name = " Charter Out End" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 11, Name = "PlusCorrection" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 14, Name = "CharterInEndVariance" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 13, Name = "CharterOutStartVariance" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 6, Name = " خرید" });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 16, Name = "Barging " });
            VoucherDetailTypsColl.Add(new ComboBoxItm() { Id = 17, Name = "CharterInStartVariance" });

            VoucherTypsColl = new ObservableCollection<ComboBoxItm>()
            {


                new ComboBoxItm() {Id = 1, Name = "Invoice"},
                new ComboBoxItm() {Id = 2, Name = "Receipt"},
                new ComboBoxItm() {Id = 3, Name = "Issue"},
                new ComboBoxItm() {Id = 4, Name = "SaleInvoice"},
                new ComboBoxItm() {Id = 5, Name = "OffHire"}
            };

        }
    }
}
