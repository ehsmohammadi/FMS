using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class InvoiceListVM : WorkspaceViewModel, IEventHandler<InvoiceListChangeArg>
    {
        #region props

        #region injected fields

        private IApprovalFlowServiceWrapper approcalServiceWrapper;
        private ICompanyServiceWrapper companyServiceWrapper;
        private IInvoiceController controller;
        private IFuelController mainController;
        private IInvoiceServiceWrapper serviceWrapper;
        private IUserServiceWrapper userServiceWrapper;

        #endregion

        #region filter


        //filter props
        private ObservableCollection<CompanyDto> companiesFilter;
        public ObservableCollection<CompanyDto> CompaniesFilter
        {
            get { return this.companiesFilter; }
            set { this.SetField(this.GetPropertyName(p => p.CompaniesFilter), ref this.companiesFilter, value); }
        }

        private CompanyDto companiesFilterSelected;
        public CompanyDto CompaniesFilterSelected
        {
            get { return this.companiesFilterSelected; }
            set { this.SetField(d => d.CompaniesFilterSelected, ref this.companiesFilterSelected, value); }
        }

        private VesselInCompanyDto vesselFilterSelected;
        public VesselInCompanyDto VesselFilterSelected
        {
            get { return this.vesselFilterSelected; }
            set { this.SetField(d => d.VesselFilterSelected, ref this.vesselFilterSelected, value); }
        }

        public ObservableCollection<UserDto> InvoiceCreatorsFilter { get; set; }

        private UserDto invoiceCreatorsFilterSelected;
        public UserDto InvoiceCreatorsFilterSelected
        {
            get { return this.invoiceCreatorsFilterSelected; }
            set { this.SetField(d => d.InvoiceCreatorsFilterSelected, ref this.invoiceCreatorsFilterSelected, value); }
        }

        // public EnumVM<InvoiceTypeEnum> InvoiceTypesVM { get; private set; }

        private DateTime? fromDateFilter;
        public DateTime? FromDateFilter
        {
            get { return this.fromDateFilter; }
            set { this.SetField(v => v.FromDateFilter, ref this.fromDateFilter, value); }
        }

        private DateTime? toDateFilter;
        public DateTime? ToDateFilter
        {
            get { return this.toDateFilter; }
            set { this.SetField(v => v.ToDateFilter, ref this.toDateFilter, value); }
        }

        #endregion

        #region selected & main data

        private PagedSortableCollectionView<InvoiceDto> data;
        private InvoiceDto selectedInvoice;

        public InvoiceDto SelectedInvoice
        {
            get { return this.selectedInvoice; }
            set
            {
                this.SetField(p => p.SelectedInvoice, ref this.selectedInvoice, value);
                this.mainController.Publish(new InvoiceListSelectedIndexChangeEvent { Entity = value, UniqId = this.UniqId });
            }
        }

        public PagedSortableCollectionView<InvoiceDto> Data
        {
            get { return this.data; }
            set { this.SetField(p => p.Data, ref this.data, value); }
        }

        #endregion

        #region commands

        private CommandViewModel addCommand;
        private CommandViewModel approveCommand;
        private CommandViewModel cancelCommand;
        private CommandViewModel deleteCommand;
        private CommandViewModel editCommand;
        private CommandViewModel nextPageCommand;
        private CommandViewModel rejectCommand;
        private CommandViewModel searchCommand;
        //command props


        public CommandViewModel SearchCommand
        {
            get
            { return this.searchCommand ?? (this.searchCommand = new CommandViewModel("جستجو", new DelegateCommand(() => this.LoadInvoicesByFilters()))); }
        }

        public CommandViewModel EditCommand
        {
            get
            {
                return this.editCommand ?? (this.editCommand = new CommandViewModel
                    (
                    "ویرایش", new DelegateCommand
                                  (
                                  () =>
                                  {
                                      if (!this.CheckIsSelected()) return;
                                      this.controller.Edit(this.SelectedInvoice, new List<CompanyDto>(this.CompaniesFilter));
                                  })));
            }
        }

        public CommandViewModel AddCommand
        {
            get
            {
                return this.addCommand ?? (this.addCommand = new CommandViewModel("افزودن",
                        new DelegateCommand(
                            () => this.controller.Add(new List<CompanyDto>(this.CompaniesFilter)))));
            }
        }

        public CommandViewModel DeleteCommand
        {
            get
            {
                return this.deleteCommand ?? (this.deleteCommand = new CommandViewModel
                    (
                    "حذف", new DelegateCommand
                               (
                               () =>
                               {
                                   if (!this.CheckIsSelected()) return;


                                   if (!this.mainController.ShowConfirmationBox("آیا برای حذف مطمئن هستید ", "اخطار")) return;

                                   this.ShowBusyIndicator("درحال انجام حذف");

                                   this.serviceWrapper.Delete
                                       (
                                           (res, exp) => this.mainController.BeginInvokeOnDispatcher
                                               (
                                                   () =>
                                                   {
                                                       if (exp != null) this.mainController.HandleException(exp);

                                                       this.HideBusyIndicator();
                                                       this.LoadInvoicesByFilters();
                                                       this.mainController.Publish(new InvoiceListSelectedIndexChangeEvent() { Entity = null, UniqId = this.UniqId });

                                                   }), this.SelectedInvoice.Id);
                               })));
            }
        }


        public CommandViewModel ApproveCommand
        {
            get
            {
                return this.approveCommand ?? (this.approveCommand = new CommandViewModel
                    (
                    "تأیید", new DelegateCommand
                                 (
                                 () =>
                                 {
                                     if (!this.CheckIsSelected()) return;
                                     this.ShowBusyIndicator();
                                     if (MessageBox.Show("آیا مطمئن هستید ؟", "Approve Confirm", MessageBoxButton.OKCancel)
                                         == MessageBoxResult.Cancel)
                                     {
                                         this.HideBusyIndicator();
                                         return;
                                     }
                                     this.approcalServiceWrapper.ActApproveFlow
                                         (
                                             (res, exp) => this.mainController.BeginInvokeOnDispatcher
                                                 (
                                                     () =>
                                                     {
                                                         this.HideBusyIndicator();
                                                         if (exp != null)
                                                             this.mainController.HandleException(exp);
                                                         else
                                                             this.LoadInvoicesByFilters();
                                                     }), this.SelectedInvoice.Id, ActionEntityTypeEnum.Invoice);
                                 })));
            }
        }

        public CommandViewModel RejectCommand
        {
            get
            {
                return this.rejectCommand ?? (this.rejectCommand = new CommandViewModel
                    (
                    "لغو تأیید", new DelegateCommand
                              (
                              () =>
                              {
                                  if (!this.CheckIsSelected()) return;
                                  this.ShowBusyIndicator();
                                  if (MessageBox.Show("آیا مطمئن هستید ؟", "Reject Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                                  {
                                      this.HideBusyIndicator();
                                      return;
                                  }
                                  this.approcalServiceWrapper.ActRejectFlow
                                      (
                                          (res, exp) => this.mainController.BeginInvokeOnDispatcher
                                              (
                                                  () =>
                                                  {
                                                      this.HideBusyIndicator();
                                                      if (exp != null) this.mainController.HandleException(exp);
                                                      else this.LoadInvoicesByFilters();

                                                  }), this.SelectedInvoice.Id, ActionEntityTypeEnum.Invoice);
                              })));
            }
        }

        public CommandViewModel CancelCommand
        {
            get
            {
                return this.cancelCommand ?? (this.cancelCommand = new CommandViewModel
                    (
                    "کنسل نمودن", new DelegateCommand
                              (
                              () =>
                              {
                                  if (!this.CheckIsSelected()) return;
                                  this.ShowBusyIndicator();
                                  //if (MessageBox.Show("آیا مطمئن هستید ؟", "Cancellation Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                                  //{
                                  //    this.HideBusyIndicator();
                                  //    return;
                                  //}

                                  if (MessageBox.Show("هشدار :\nصورتحساب انتخاب شده دارای عملیات قیمت گذاری می باشد.\nدر صورت کنسل شدن، کلیه عملیات قیمت گذاری برگشت داده می شود.\nآیا از انجام کار، اطمینان دارید؟", "Cancellation Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                                  {
                                      this.HideBusyIndicator();
                                      return;
                                  }

                                  this.approcalServiceWrapper.ActCancelFlow(
                                          (res, exp) => this.mainController.BeginInvokeOnDispatcher
                                              (
                                                  () =>
                                                  {
                                                      this.HideBusyIndicator();
                                                      if (exp != null) this.mainController.HandleException(exp);
                                                      else this.LoadInvoicesByFilters();

                                                  }), this.SelectedInvoice.Id, ActionEntityTypeEnum.Invoice);
                              })));
            }
        }

        private CommandViewModel viewFuelReportDetailsReferencesCommand;
        public CommandViewModel ViewFuelReportDetailsReferencesCommand
        {
            get
            {
                return viewFuelReportDetailsReferencesCommand ?? (viewFuelReportDetailsReferencesCommand = new CommandViewModel("عملیات سوختگیری", new DelegateCommand(() =>
                {
                    if (CheckIsSelected() && !string.IsNullOrWhiteSpace(this.SelectedInvoice.FuelReportDetailIds))
                    {
                        controller.ViewFuelReportDetailsReferences(this.SelectedInvoice.FuelReportDetailIds);
                    }
                })));
            }
        }

        private CommandViewModel viewOrdersReferencesCommand;
        public CommandViewModel ViewOrdersReferencesCommand
        {
            get
            {
                return viewOrdersReferencesCommand ?? (viewOrdersReferencesCommand = new CommandViewModel("سفارش (ها)", new DelegateCommand(() =>
                {
                    if (CheckIsSelected() && !string.IsNullOrWhiteSpace(this.SelectedInvoice.OrderNumbers))
                    {
                        controller.ViewOrdersReferences(this.SelectedInvoice.OrderNumbers);
                    }
                })));
            }
        }

        #endregion

        #region inline editing

        #endregion

        #region column visibility

        private bool isFromVesselVisible;
        private bool _isReceiverVisible;
        private bool _isSupplierVisible;
        private bool _isToVesselVisible;
        private bool isVisibleFilter;



        public bool IsVisibleFilter
        {
            get { return this.isVisibleFilter; }
            set { this.SetField(p => p.IsVisibleFilter, ref this.isVisibleFilter, value); }
        }



        public bool IsSupplierVisible
        {
            get { return this._isSupplierVisible; }
            set { this.SetField(p => p.IsSupplierVisible, ref this._isSupplierVisible, value); }
        }


        public bool IsReceiverVisible
        {
            get { return this._isReceiverVisible; }
            set { this.SetField(p => p.IsReceiverVisible, ref this._isReceiverVisible, value); }
        }

        public bool IsFromVesselVisible
        {
            get { return this.isFromVesselVisible; }
            set { this.SetField(p => p.IsFromVesselVisible, ref this.isFromVesselVisible, value); }
        }

        public bool IsToVesselVisible
        {
            get { return this._isToVesselVisible; }
            set { this.SetField(p => p.IsToVesselVisible, ref this._isToVesselVisible, value); }
        }

        private string invoiceIdsFilterValue;
        public string InvoiceIdsFilterValue
        {
            get { return this.invoiceIdsFilterValue; }
            set { this.SetField(d => d.InvoiceIdsFilterValue, ref this.invoiceIdsFilterValue, value); }
        }

        private string invoiceItemIdsFilterValue;
        public string InvoiceItemIdsFilterValue
        {
            get { return this.invoiceItemIdsFilterValue; }
            set { this.SetField(d => d.InvoiceItemIdsFilterValue, ref this.invoiceItemIdsFilterValue, value); }
        }

        private string invoiceNumberFilterValue;
        public string InvoiceNumberFilterValue
        {
            get { return this.invoiceNumberFilterValue; }
            set { this.SetField(d => d.InvoiceNumberFilterValue, ref this.invoiceNumberFilterValue, value); }
        }

        private string orderNumberFilterValue;
        public string OrderNumberFilterValue
        {
            get { return this.orderNumberFilterValue; }
            set { this.SetField(d => d.OrderNumberFilterValue, ref this.orderNumberFilterValue, value); }
        }

        #endregion

        #endregion

        #region ctor

        public InvoiceListVM()
        {
            //FromDateFilter = DateTime.Now;
        }

        // vesselServiceWrapper must be added ***********************
        public InvoiceListVM(IInvoiceController controller,
                             IFuelController mainController,
                             IInvoiceServiceWrapper serviceWrapper,
                             ICompanyServiceWrapper companyServiceWrapper,
                             IUserServiceWrapper userServiceWrapper,

                             // EnumVM<InvoiceTypeEnum> InvoiceTypeEnum
                              IApprovalFlowServiceWrapper approcalServiceWrapper)
        {

            this.controller = controller;
            this.serviceWrapper = serviceWrapper;
            this.mainController = mainController;
            this.companyServiceWrapper = companyServiceWrapper;
            this.userServiceWrapper = userServiceWrapper;
            this.approcalServiceWrapper = approcalServiceWrapper;

            // InvoiceTypesVM = new EnumVM<InvoiceTypeEnum>();

            this.DisplayName = "صورتحساب";
            this.Data = new PagedSortableCollectionView<InvoiceDto>();
            this.Data.PageChanged += this.Data_PageChanged;

            //filters
            this.CompaniesFilter = new ObservableCollection<CompanyDto>();
            this.InvoiceCreatorsFilter = new ObservableCollection<UserDto>();

            this.Load();
        }

        void Data_PageChanged(object sender, EventArgs e)
        {
            this.LoadInvoicesByFilters();
        }

        #endregion

        #region methods

        private bool CheckIsSelected()
        {
            if (this.SelectedInvoice == null)
            {
                this.mainController.ShowMessage("لطفا سفارش مورد نظر را انتخاب فرمائید");
                return false;
            }
            return true;
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            this.mainController.Close(this);
        }

        #region loading data

        public void Load()
        {
            this.ShowBusyIndicator("درحال دريافت اطلاعات ............");

            #region Companies for filter

            this.companyServiceWrapper.GetAll
                (
                    (res, exp) => this.mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                this.HideBusyIndicator();
                                if (exp == null)
                                {
                                    this.CompaniesFilter.Clear();
                                    foreach (var dto in res)
                                    {
                                        dto.VesselInCompanies.Insert(0, FilteringUtils.EmptyVesselDto);

                                        this.CompaniesFilter.Add(dto);
                                    }

                                    //CompaniesFilter.Insert(0, FilteringUtils.EmptyCompanyDto);

                                    this.CompaniesFilterSelected = this.CompaniesFilter.FirstOrDefault();
                                }
                                else
                                {
                                    this.mainController.HandleException(exp);
                                }
                            }), true);

            #endregion

            #region InvoiceCreators for filter

            //userServiceWrapper.GetAll
            //    (
            //        (res, exp) => mainController.BeginInvokeOnDispatcher
            //            (
            //                () =>
            //                {
            //                    HideBusyIndicator();
            //                    if (exp == null)
            //                    {
            //                        foreach (var dto in res) InvoiceCreatorsFilter.Add(dto);

            //                        InvoiceCreatorsFilter.Insert(0, FilteringUtils.EmptyUserDto);

            //                        InvoiceCreatorsFilterSelected = InvoiceCreatorsFilter.FirstOrDefault();
            //                    }
            //                    else
            //                    {
            //                        mainController.HandleException(exp);
            //                    }
            //                }), "GetAll");

            #endregion
        }

        public Guid UniqId { get; set; }

        private void LoadInvoicesByFilters()
        {
            if (this.InvoiceIdsFilterValue == null && this.InvoiceItemIdsFilterValue == null &&
                (this.CompaniesFilterSelected == null || this.CompaniesFilterSelected == FilteringUtils.EmptyCompanyDto) /*
                string.IsNullOrWhiteSpace(InvoiceNumberFilterValue) ||
                (FromDateFilter == DateTime.MinValue || ToDateFilter == DateTime.MinValue))*/
            )
                return;

            this.ShowBusyIndicator("درحال دريافت اطلاعات ............");
            this.serviceWrapper.GetByFilter(
                    (res, exp) => this.mainController.BeginInvokeOnDispatcher
                        (
                         () =>
                         {
                             if (exp == null)
                             {
                                 this.Data.SourceCollection = res.Result.ToList();
                                 this.Data.TotalItemCount = res.TotalCount;

                                 if (this.Data.SourceCollection.Count() == 1)
                                     this.SelectedInvoice = this.Data.SourceCollection.First();
                             }
                             else
                             {
                                 this.mainController.HandleException(exp);
                             }

                             if (this.Data.PageIndex > this.Data.PageCount - 1)
                             {
                                 this.Data.PageIndex = this.Data.PageCount - 1;
                             }

                             this.HideBusyIndicator();
                         }),
                            (this.CompaniesFilterSelected == null || this.CompaniesFilterSelected == FilteringUtils.EmptyCompanyDto) ? null : (long?)this.CompaniesFilterSelected.Id,
                            this.FromDateFilter,
                            this.ToDateFilter,
                            this.InvoiceIdsFilterValue,
                            this.InvoiceItemIdsFilterValue,
                            this.InvoiceNumberFilterValue,

                            this.VesselFilterSelected == null || this.VesselFilterSelected == FilteringUtils.EmptyVesselDto ? null : (long?)this.VesselFilterSelected.Id, this.OrderNumberFilterValue, null, this.Data.PageSize, this.Data.PageIndex, false);
        }

        #endregion

        #endregion

        public void LoadByFilter(string invoiceIds, string invoiceItemIds)
        {
            this.Load();

            this.CompaniesFilterSelected = FilteringUtils.EmptyCompanyDto;
            this.InvoiceCreatorsFilterSelected = FilteringUtils.EmptyUserDto;

            this.FromDateFilter = null;
            this.ToDateFilter = null;

            this.InvoiceIdsFilterValue = invoiceIds;
            this.InvoiceItemIdsFilterValue = invoiceItemIds;

            this.LoadInvoicesByFilters();
        }

        public void Handle(InvoiceListChangeArg eventData)
        {
            LoadInvoicesByFilters();
        }
    }
}