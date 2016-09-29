using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class OrderListVM : WorkspaceViewModel, IEventHandler<OrderListChangeArg>
    {
        #region props

        #region injected fields

        private IApprovalFlowServiceWrapper approcalServiceWrapper;
        private IGoodServiceWrapper goodServiceWrapper;

        private IOrderController controller
        {
            get;
            set;
        }

        private IFuelController mainController
        {
            get;
            set;
        }

        private IOrderServiceWrapper serviceWrapper
        {
            get;
            set;
        }

        private ICompanyServiceWrapper orderCompanyServiceWrapper
        {
            get;
            set;
        }

        private IUserServiceWrapper userServiceWrapper
        {
            get;
            set;
        }

        private IVesselInCompanyServiceWrapper vesselServiceWrapper
        {
            get;
            set;
        }

        private IResolver<OrderVM> orderVMResolver
        {
            get;
            set;
        }

        public OrderItemListVM OrderItemListVM
        {
            get;
            set;
        }

        #endregion

        #region filter

        private CompanyDto companiesFilterSelected;
        private UserDto orderCreatorsFilterSelected;
        private DateTime? fromDateFilter;
        private DateTime? toDateFilter;

        //filter props
        public ObservableCollection<CompanyDto> CompaniesFilter
        {
            get { return this.companiesFilter; }
            set { this.SetField(this.GetPropertyName(p=>p.CompaniesFilter), ref this.companiesFilter, value); }
        }

        public CompanyDto CompaniesFilterSelected
        {
            get { return companiesFilterSelected; }
            set { this.SetField(d => d.CompaniesFilterSelected, ref companiesFilterSelected, value); }
        }

        public VesselInCompanyDto VesselFilterSelected
        {
            get { return this.vesselFilterSelected; }
            set { this.SetField(this.GetPropertyName(p => p.VesselFilterSelected), ref this.vesselFilterSelected, value); }
        }

        public ObservableCollection<UserDto> OrderCreatorsFilter
        {
            get;
            set;
        }

        public UserDto OrderCreatorsFilterSelected
        {
            get { return orderCreatorsFilterSelected; }
            set { this.SetField(d => d.OrderCreatorsFilterSelected, ref orderCreatorsFilterSelected, value); }
        }

        public EnumVM<OrderTypeEnum> OrderTypesVM
        {
            get;
            private set;
        }

        public DateTime? FromDateFilter
        {
            get { return fromDateFilter; }
            set { this.SetField(v => v.FromDateFilter, ref fromDateFilter, value); }
        }

        public DateTime? ToDateFilter
        {
            get { return toDateFilter; }
            set { this.SetField(v => v.ToDateFilter, ref toDateFilter, value); }
        }

        private long? orderIdFilterValue;
        public long? OrderIdFilterValue
        {
            get { return this.orderIdFilterValue; }
            set { this.SetField(d => d.OrderIdFilterValue, ref this.orderIdFilterValue, value); }
        }

        private long? orderItemIdFilterValue;
        public long? OrderItemIdFilterValue
        {
            get { return this.orderItemIdFilterValue; }
            set { this.SetField(d => d.OrderItemIdFilterValue, ref this.orderItemIdFilterValue, value); }
        }

        private string orderNumberFilterValue;
        public string OrderNumberFilterValue
        {
            get { return this.orderNumberFilterValue; }
            set { this.SetField(d => d.OrderNumberFilterValue, ref this.orderNumberFilterValue, value); }
        }

        #endregion

        #region selected & main data

        private PagedSortableCollectionView<OrderVM> data;
        private OrderVM selected;

        public OrderVM Selected
        {
            get { return selected; }
            set
            {
                this.SetField(p => p.Selected, ref selected, value);
                OrderItemListVM.OrderVMSelected = selected;
            }
        }

        public PagedSortableCollectionView<OrderVM> Data
        {
            get { return data; }
            set { this.SetField(p => p.Data, ref data, value); }
        }

        #endregion

        #region commands

        private CommandViewModel addCommand;
        private CommandViewModel approveCommand;
        private CommandViewModel closeOrderCommand;
        private CommandViewModel canceledCommand;
        private CommandViewModel deleteCommand;
        private CommandViewModel editCommand;
        private CommandViewModel nextPageCommand;
        private CommandViewModel rejectCommand;
        private CommandViewModel searchCommand;
        //command props

        public CommandViewModel SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new CommandViewModel("جستجو", new DelegateCommand(() => LoadOrdersByFilters()));
                }
                return searchCommand;
            }
        }

        public CommandViewModel EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new CommandViewModel
                    (
                    "ویرایش", new DelegateCommand
                                  (
                                  () =>
                                  {
                                      if (!CheckIsSelected())
                                          return;
                                      controller.Edit(Selected.Entity, Suppliers, FromVessels);
                                  })));
            }
        }

        public CommandViewModel AddCommand
        {
            get { return addCommand ?? (addCommand = new CommandViewModel("افزودن", new DelegateCommand(() => controller.Add(Suppliers, FromVessels)))); }
        }

        public CommandViewModel DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new CommandViewModel
                    (
                    "حذف", new DelegateCommand
                               (
                               () =>
                               {
                                   if (!CheckIsSelected())
                                       return;


                                   if (!mainController.ShowConfirmationBox("آیا برای حذف مطمئن هستید ", "اخطار"))
                                       return;
                                   ShowBusyIndicator("درحال انجام حذف");

                                   serviceWrapper.Delete
                                       (
                                           (res, exp) => mainController.BeginInvokeOnDispatcher
                                               (
                                                   () =>
                                                   {
                                                       if (exp != null)
                                                       {
                                                           mainController.HandleException(exp);
                                                       }
                                                       HideBusyIndicator();
                                                       LoadOrdersByFilters();
                                                   }), Selected.Entity.Id);
                               })));
            }
        }

        public CommandViewModel NextPageCommand
        {
            get
            {
                return nextPageCommand ?? (nextPageCommand = new CommandViewModel(
                    "صفحه بعد", new DelegateCommand
                                    (
                                    () =>
                                    {
                                        Data.PageIndex++;
                                        Data.Refresh();
                                    })));
            }
        }

        public CommandViewModel ApproveCommand
        {
            get
            {
                return approveCommand ?? (approveCommand = new CommandViewModel
                    (
                    "تأیید", new DelegateCommand
                                 (
                                 () =>
                                 {
                                     if (!CheckIsSelected())
                                         return;
                                     ShowBusyIndicator();
                                     if (MessageBox.Show("آیا مطمئن هستید ؟", "Approve Confirm", MessageBoxButton.OKCancel)
                                         == MessageBoxResult.Cancel)
                                     {
                                         HideBusyIndicator();
                                         return;
                                     }
                                     approcalServiceWrapper.ActApproveFlow
                                         (
                                             (res, exp) => mainController.BeginInvokeOnDispatcher
                                                 (
                                                     () =>
                                                     {
                                                         HideBusyIndicator();
                                                         if (exp != null)
                                                         {
                                                             mainController.HandleException(exp);
                                                         }
                                                         else
                                                             LoadOrdersByFilters();
                                                     }), Selected.Entity.Id, ActionEntityTypeEnum.Order);
                                 })));
            }
        }

        public CommandViewModel RejectCommand
        {
            get
            {
                if (rejectCommand == null)
                {
                    rejectCommand = new CommandViewModel
                        (
                        "لغو تأیید", new DelegateCommand
                                  (
                                  () =>
                                  {
                                      if (!CheckIsSelected())
                                          return;
                                      ShowBusyIndicator();
                                      if (MessageBox.Show("آیا مطمئن هستید ؟", "Reject Confirm", MessageBoxButton.OKCancel)
                                          == MessageBoxResult.Cancel)
                                      {
                                          HideBusyIndicator();
                                          return;
                                      }
                                      approcalServiceWrapper.ActRejectFlow
                                          (
                                              (res, exp) => mainController.BeginInvokeOnDispatcher
                                                  (
                                                      () =>
                                                      {
                                                          HideBusyIndicator();
                                                          if (exp != null)
                                                          {
                                                              mainController.HandleException(exp);
                                                          }
                                                          else
                                                              LoadOrdersByFilters();
                                                      }), Selected.Entity.Id, ActionEntityTypeEnum.Order);
                                  }));
                }
                return rejectCommand;
            }
        }

        public CommandViewModel CloseOrderCommand
        {
            get
            {
                return closeOrderCommand ?? (closeOrderCommand = new CommandViewModel
                    (
                    "بستن", new DelegateCommand
                                (() =>
                                {
                                    if (!CheckIsSelected())
                                        return;
                                    ShowBusyIndicator();
                                    if (MessageBox.Show("آیا از بستن سفارش اطمینان دارید؟", "Close Confirmation", MessageBoxButton.OKCancel)
                                        == MessageBoxResult.Cancel)
                                    {
                                        HideBusyIndicator();
                                        return;
                                    }
                                    approcalServiceWrapper.ActCloseFlow
                                        (
                                            (res, exp) => mainController.BeginInvokeOnDispatcher
                                                (
                                                    () =>
                                                    {
                                                        HideBusyIndicator();
                                                        if (exp != null)
                                                        {
                                                            mainController.HandleException(exp);
                                                        }
                                                        else
                                                            LoadOrdersByFilters();
                                                    }), Selected.Entity.Id, ActionEntityTypeEnum.Order);
                                })));
            }
        }

        public CommandViewModel CancelCommand
        {
            get
            {
                return canceledCommand ?? (canceledCommand = new CommandViewModel
                    (
                    "ابطال", new DelegateCommand
                                (
                                () =>
                                {
                                    if (!CheckIsSelected())
                                        return;
                                    ShowBusyIndicator();
                                    if (MessageBox.Show("آیا از ابطال سفارش اطمینان دارید؟", "Cancel Confirm", MessageBoxButton.OKCancel)
                                        == MessageBoxResult.Cancel)
                                    {
                                        HideBusyIndicator();
                                        return;
                                    }
                                    approcalServiceWrapper.ActCancelFlow
                                        (
                                            (res, exp) => mainController.BeginInvokeOnDispatcher
                                                (
                                                    () =>
                                                    {
                                                        HideBusyIndicator();
                                                        if (exp != null)
                                                        {
                                                            mainController.HandleException(exp);
                                                        }
                                                        else
                                                            LoadOrdersByFilters();
                                                    }), Selected.Entity.Id, ActionEntityTypeEnum.Order);
                                })));
            }
        }

        private CommandViewModel viewAssignedFuelReportDetailReferencesCommand;
        public CommandViewModel ViewAssignedFuelReportReferencesCommand
        {
            get
            {
                return viewAssignedFuelReportDetailReferencesCommand ?? (viewAssignedFuelReportDetailReferencesCommand = new CommandViewModel("عملیات سوختگیری / سوخت رسانی", new DelegateCommand(() =>
                {
                    if (CheckIsSelected() && this.Selected.Entity.DestinationReferences.Any(d => d.DestinationType == OrderAssignementReferenceTypeEnum.FuelReportDetail))
                    {
                        controller.ViewAssignedReferences(OrderAssignementReferenceTypeEnum.FuelReportDetail, this.Selected.Entity);
                    }
                })));
            }
        }

        private CommandViewModel viewAssignedInvoiceReferencesCommand;
        public CommandViewModel ViewAssignedInvoiceReferencesCommand
        {
            get
            {
                return viewAssignedInvoiceReferencesCommand ?? (viewAssignedInvoiceReferencesCommand = new CommandViewModel("صورتحساب (ها)", new DelegateCommand(() =>
                {
                    if (CheckIsSelected() && this.Selected.Entity.DestinationReferences.Any(d => d.DestinationType == OrderAssignementReferenceTypeEnum.Invoice))
                    {
                        controller.ViewAssignedReferences(OrderAssignementReferenceTypeEnum.Invoice, this.Selected.Entity);
                    }
                })));
            }
        }

        #endregion

        #region inline editing

        //inline editing 
        private List<VesselInCompanyDto> _fromVessels;
        private List<VesselInCompanyDto> _toVessels;
        private List<CompanyDto> receivers;
        private List<CompanyDto> suppliers;

        public List<CompanyDto> Suppliers
        {
            get { return suppliers; }
            set { this.SetField(p => p.Suppliers, ref suppliers, value); }
        }

        public List<CompanyDto> Receivers
        {
            get { return receivers; }
            set { this.SetField(p => p.Receivers, ref receivers, value); }
        }

        public List<VesselInCompanyDto> FromVessels
        {
            get { return _fromVessels; }
            set { this.SetField(p => p.FromVessels, ref _fromVessels, value); }
        }

        public List<VesselInCompanyDto> ToVessels
        {
            get { return _toVessels; }
            set { this.SetField(p => p.ToVessels, ref _toVessels, value); }
        }

        #endregion

        #region column visibility

        private bool _isFromVesselVisible;
        private bool _isReceiverVisible;
        private bool _isSupplierVisible;
        private bool _isToVesselVisible;
        private VesselInCompanyDto vesselFilterSelected;
        private ObservableCollection<CompanyDto> companiesFilter;

        public bool IsSupplierVisible
        {
            get { return _isSupplierVisible; }
            set { this.SetField(p => p.IsSupplierVisible, ref _isSupplierVisible, value); }
        }

        public bool IsReceiverVisible
        {
            get { return _isReceiverVisible; }
            set { this.SetField(p => p.IsReceiverVisible, ref _isReceiverVisible, value); }
        }

        public bool IsFromVesselVisible
        {
            get { return _isFromVesselVisible; }
            set { this.SetField(p => p.IsFromVesselVisible, ref _isFromVesselVisible, value); }
        }

        public bool IsToVesselVisible
        {
            get { return _isToVesselVisible; }
            set { this.SetField(p => p.IsToVesselVisible, ref _isToVesselVisible, value); }
        }

        #endregion

        #endregion

        #region ctor

        public OrderListVM()
        {
            //FromDateFilter = DateTime.Now;

            this.PropertyChanged += OrderListVM_PropertyChanged;
        }

        // vesselServiceWrapper must be added ***********************
        public OrderListVM(IOrderController controller,
                           IFuelController mainController,
                           IOrderServiceWrapper serviceWrapper,
                           ICompanyServiceWrapper orderCompanyServiceWrapper,
                           IUserServiceWrapper userServiceWrapper,
                           IVesselInCompanyServiceWrapper vesselServiceWrapper,
                           IResolver<OrderVM> orderVMResolver
            // EnumVM<OrderTypeEnum> orderTypeEnum
                           ,
                           OrderItemListVM orderItemListVM,
                           IApprovalFlowServiceWrapper _approcalServiceWrapper,
                           IGoodServiceWrapper goodServiceWrapper)
        {
            Init
                (
                    controller, mainController, serviceWrapper, orderCompanyServiceWrapper, userServiceWrapper, vesselServiceWrapper, orderVMResolver,
                    orderItemListVM, _approcalServiceWrapper, goodServiceWrapper);
        }

        #endregion

        #region methods

        public void Handle(OrderListChangeArg eventData)
        {
            LoadOrdersByFilters();
        }

        private void Init(IOrderController controller,
                          IFuelController mainController,
                          IOrderServiceWrapper serviceWrapper,
                          ICompanyServiceWrapper orderCompanyServiceWrapper,
                          IUserServiceWrapper userServiceWrapper,
                          IVesselInCompanyServiceWrapper vesselServiceWrapper,
                          IResolver<OrderVM> orderVMResolver,
                          OrderItemListVM orderItemListVM,
                          IApprovalFlowServiceWrapper _approcalServiceWrapper,
                          IGoodServiceWrapper goodServiceWrapper)
        {
            this.controller = controller;
            this.serviceWrapper = serviceWrapper;
            this.goodServiceWrapper = goodServiceWrapper;
            this.mainController = mainController;
            this.orderCompanyServiceWrapper = orderCompanyServiceWrapper;
            this.userServiceWrapper = userServiceWrapper;
            this.vesselServiceWrapper = vesselServiceWrapper;
            this.orderVMResolver = orderVMResolver;

            OrderTypesVM = new EnumVM<OrderTypeEnum>();
            OrderItemListVM = orderItemListVM;

            DisplayName = "سفارش";


            Data = new PagedSortableCollectionView<OrderVM>() { PageSize = 10 };
            Data.PageChanged += DataPageChanged;


            //filters
            CompaniesFilter = new ObservableCollection<CompanyDto>();
            OrderCreatorsFilter = new ObservableCollection<UserDto>();

            OrderTypesVM.Items.Remove(OrderTypesVM.Items.ToList().FirstOrDefault(i=>i.EnumName == OrderTypeEnum.SupplyForDeliveredVessel));

            OrderTypesVM.SelectedItem = OrderTypesVM.Items.FirstOrDefault();
            OrderTypesVM.PropertyChanged += OrderTypeFilterChanged;
            //FromDateFilter = DateTime.Now.AddMonths(-1);
            //ToDateFilter = DateTime.Now;

            //inline editing 
            Suppliers = new List<CompanyDto>();
            Receivers = new List<CompanyDto>();
            FromVessels = new List<VesselInCompanyDto>();
            ToVessels = new List<VesselInCompanyDto>();
            approcalServiceWrapper = _approcalServiceWrapper;

            //            Data.OnRefresh += (s, args) => Load();
        }

        void DataPageChanged(object sender, EventArgs e)
        {
            LoadOrdersByFilters();
        }

        private void OrderTypeFilterChanged(object s, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == OrderTypesVM.GetPropertyName(d => d.SelectedItem))
            {
                if (OrderTypesVM.SelectedItem.EnumName != OrderTypeEnum.PurchaseForVessel ||
                    OrderTypesVM.SelectedItem.EnumName != OrderTypeEnum.SupplyForDeliveredVessel)
                {
                    this.VesselFilterSelected = FilteringUtils.EmptyVesselDto;
                }
            }

            #region Useful but Commented
            //if (e.PropertyName == OrderTypesVM.GetPropertyName(d => d.SelectedItem))
            //    LoadOrdersByFilters();

            ////internal transfer
            //if (OrderTypesVM.SelectedItem.EnumName == OrderTypeEnum.InternalTransfer)
            //{
            //    IsSupplierVisible = true;
            //    IsReceiverVisible = false;
            //    IsFromVesselVisible = true;
            //    IsToVesselVisible = true;
            //}
            ////Purchase Transfer
            //else if (OrderTypesVM.SelectedItem.EnumName == OrderTypeEnum.PurchaseWithTransfer)
            //{
            //    IsSupplierVisible = true;
            //    IsReceiverVisible = true;
            //    IsFromVesselVisible = false;
            //    IsToVesselVisible = false;
            //}
            ////Purchase
            //else if (OrderTypesVM.SelectedItem.EnumName == OrderTypeEnum.Purchase)
            //{
            //    IsSupplierVisible = false;
            //    IsReceiverVisible = false;
            //    IsFromVesselVisible = false;
            //    IsToVesselVisible = true;
            //}
            //else if (OrderTypesVM.SelectedItem.EnumName == OrderTypeEnum.PurchaseForVessel)
            //{
            //    IsSupplierVisible = false;
            //    IsReceiverVisible = false;
            //    IsFromVesselVisible = false;
            //    IsToVesselVisible = true;
            //}
            //else // others
            //{
            //    IsSupplierVisible = true;
            //    IsReceiverVisible = true;
            //    IsFromVesselVisible = true;
            //    IsToVesselVisible = true;
            //}

            #endregion
        }

        void OrderListVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.Selected))
            {

            }
        }


        private OrderVM CreateItem(OrderDto dto)
        {
            var result = orderVMResolver.Resolve();
            result.Entity = dto;
            result.Suppliers = Suppliers;
            result.Receivers = Receivers;
            result.SetMainController(mainController);
            result.SetServiceWrapper(serviceWrapper);
            result.OrderItemVms = createOrderItem(dto);
            result.FromVessels = FromVessels;
            result.ToVessels = ToVessels;

            return result;
        }

        private ObservableCollection<OrderItemVM> createOrderItem(OrderDto dto)
        {
            var result = new ObservableCollection<OrderItemVM>();
            if (dto.OrderItems != null && dto.OrderItems.Count > 0)
            {
                dto.OrderItems.ToList().ForEach(c => result.Add(new OrderItemVM(mainController, serviceWrapper, goodServiceWrapper) { Entity = c, }));
            }
            return result;
        }

        private bool CheckIsSelected()
        {
            if (Selected == null)
            {
                mainController.ShowMessage("لطفا سفارش مورد نظر را انتخاب فرمائید");
                return false;
            }
            return true;
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            mainController.Close(this);
            this.Dispose();
        }

        #region loading data

        public void Load()
        {
            ShowBusyIndicator("درحال دريافت اطلاعات ............");

            #region Companies for filter

            orderCompanyServiceWrapper.GetAll
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                HideBusyIndicator();
                                if (exp == null)
                                {
                                    CompaniesFilter.Clear();
                                    //Suppliers.Clear();
                                    //Receivers.Clear();

                                    foreach (var dto in res)
                                    {
                                        dto.VesselInCompanies.Insert(0, FilteringUtils.EmptyVesselDto);

                                        CompaniesFilter.Add(dto);
                                        //Suppliers.Add(dto);
                                        //Receivers.Add(dto);
                                    }

                                    //CompaniesFilter.Insert(0, FilteringUtils.EmptyCompanyDto);

                                    CompaniesFilterSelected = CompaniesFilter.FirstOrDefault();
                                }
                                else
                                {
                                    mainController.HandleException(exp);
                                }
                            }), true);

            orderCompanyServiceWrapper.GetAll
               (
                   (res, exp) => mainController.BeginInvokeOnDispatcher
                       (
                           () =>
                           {
                               HideBusyIndicator();
                               if (exp == null)
                               {
                                   Suppliers.Clear();
                                   Receivers.Clear();

                                   foreach (var dto in res)
                                   {
                                       Suppliers.Add(dto);
                                       Receivers.Add(dto);
                                   }
                               }
                               else
                               {
                                   mainController.HandleException(exp);
                               }
                           }), false);

            #endregion

            #region orderCreators for filter

            //userServiceWrapper.GetAll
            //    (
            //        (res, exp) => mainController.BeginInvokeOnDispatcher
            //            (
            //                () =>
            //                {
            //                    HideBusyIndicator();
            //                    if (exp == null)
            //                    {
            //                        foreach (var dto in res)
            //                        {
            //                            OrderCreatorsFilter.Add(dto);
            //                        }
            //                        OrderCreatorsFilterSelected = OrderCreatorsFilter.FirstOrDefault();
            //                    }
            //                    else
            //                    {
            //                        mainController.HandleException(exp);
            //                    }
            //                }), "GetAll");

            #endregion

            //#region Vessels

            //vesselServiceWrapper.GetAll
            //    (
            //        (res, exp) => mainController.BeginInvokeOnDispatcher
            //            (
            //                () =>
            //                {
            //                    HideBusyIndicator();
            //                    if (exp == null)
            //                    {
            //                        FromVessels.Clear();
            //                        ToVessels.Clear();

            //                        foreach (var dto in res)
            //                        {
            //                            FromVessels.Add(dto);
            //                            ToVessels.Add(dto);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        mainController.HandleException(exp);
            //                    }
            //                }), new
            //                    {
            //                        CompanyId = 5,
            //                        State = "5,6,7"
            //                    });

            //#endregion
        }

        private void LoadOrdersByFilters()
        {
            if (
                OrderIdFilterValue == null && OrderItemIdFilterValue == null && string.IsNullOrEmpty(OrderNumberFilterValue ) && 
                (VesselFilterSelected == null || VesselFilterSelected == FilteringUtils.EmptyVesselDto) &&
                (CompaniesFilterSelected == null || CompaniesFilterSelected == FilteringUtils.EmptyCompanyDto) && OrderTypesVM.SelectedItem == null)
                return;

            ShowBusyIndicator("درحال دريافت اطلاعات ............");
            serviceWrapper.GetByFilter(
                    (res, exp) => this.mainController.BeginInvokeOnDispatcher
                        (
                         () =>
                         {
                             if (exp == null)
                             {
                                 this.Data.SourceCollection = res.Result.Select(this.CreateItem).ToList();
                                 this.Data.TotalItemCount = res.TotalCount;

                                 if (this.Data.SourceCollection.Count() == 1)
                                     this.Selected = this.Data.SourceCollection.First();
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
                    (CompaniesFilterSelected == null || CompaniesFilterSelected == FilteringUtils.EmptyCompanyDto)  ? null : (long?)this.CompaniesFilterSelected.Id,
                    this.FromDateFilter, this.ToDateFilter,
                    this.OrderIdFilterValue,
                    this.OrderItemIdFilterValue,
                    this.OrderNumberFilterValue,
                    OrderTypesVM.SelectedItem == null ? string.Empty : this.OrderTypesVM.SelectedItem.EnumValue.ToString(),
                    this.Data.PageSize,
                    this.Data.PageIndex, 
                    this.VesselFilterSelected == null || this.VesselFilterSelected == FilteringUtils.EmptyVesselDto ? null : (long?)this.VesselFilterSelected.Id);
        }

        public void LoadByFilter(string orderNumbers)
        {
            Load();

            this.VesselFilterSelected = FilteringUtils.EmptyVesselDto;

            this.FromDateFilter = null;
            this.ToDateFilter = null;

            this.OrderNumberFilterValue = orderNumbers;

            LoadOrdersByFilters();
        }

        #endregion

        #endregion
    }
}