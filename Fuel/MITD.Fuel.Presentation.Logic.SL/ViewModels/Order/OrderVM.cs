#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.Extensions;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;

#endregion

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class OrderVM : WorkspaceViewModel
    {
        #region props

        private List<VesselInCompanyDto> _fromVessels;
        private ObservableCollection<OrderItemVM> _orderItemVms;
        private List<CompanyDto> _receivers;
        private List<CompanyDto> _suppliers;
        private List<VesselInCompanyDto> _toVessels;
        private IOrderView _view;
        private CommandViewModel cancelCommand;
        private OrderDto entity;
        private IFuelController mainController;
        private long orderTypeId;
        private List<ComboBoxItm> orderTypes;
        private CommandViewModel saveCommand;
        private IOrderServiceWrapper serviceWrapper;
        private IFileServiceWrapper _fileServiceWrapper;
        private IVesselInCompanyServiceWrapper vesselServiceWrapper;
        public UploaderVM UploaderVm { get; set; }

        private CommandViewModel submitCommand;

        #region column visibility

        private bool _isFromVesselVisible;
        private bool _isReceiverVisible;
        private bool _isSupplierVisible;
        private bool _isToVesselVisible;
        private bool _IsTransporterVisible;
        private List<VesselInCompanyDto> allVessels;
        private long receiverId;
        private List<CompanyDto> transporters;

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


        public bool IsTransporterVisible
        {
            get { return _IsTransporterVisible; }
            set { this.SetField(p => p.IsTransporterVisible, ref _IsTransporterVisible, value); }
        }

        #endregion

        public CommandViewModel SubmitCommand
        {
            get { return submitCommand ?? (submitCommand = new CommandViewModel("ذخیره", new DelegateCommand(Save))); }
        }

        public CommandViewModel CancelCommand
        {
            get { return cancelCommand ?? (cancelCommand = new CommandViewModel("خروج", new DelegateCommand(() => mainController.Close(this)))); }
        }

        public OrderDto Entity
        {
            get { return entity; }
            set
            {
                this.SetField(p => p.Entity, ref entity, value);
            }
        }

        public long OrderTypeId
        {
            get { return (long)Entity.OrderType; }
            //get { return orderTypeId; }
            set
            {
                if (OrderTypeChanging((OrderTypeEnum)value, false))
                {
                    Entity.OrderType = (OrderTypeEnum)value;
                    this.SetField(p => p.OrderTypeId, ref orderTypeId, value);
                }


            }
        }

        public long ReceiverId
        {
            get { return receiverId; }
            set
            {
                ChangeReciver(value);
                this.SetField(p => p.ReceiverId, ref receiverId, value);
            }
        }

        private void ChangeReciver(long value)
        {
            Entity.Receiver = Receivers.Single(c => c.Id == value);
        }

        public List<ComboBoxItm> OrderTypes
        {
            get { return orderTypes; }
            set { this.SetField(p => p.OrderTypes, ref orderTypes, value); }
        }


        public List<CompanyDto> Suppliers
        {
            get { return _suppliers; }
            set { this.SetField(p => p.Suppliers, ref _suppliers, value); }
        }
        public List<CompanyDto> Transporters
        {
            get { return transporters; }
            set { this.SetField(p => p.Transporters, ref transporters, value); }
        }

        public List<CompanyDto> Receivers
        {
            get { return _receivers; }
            set { this.SetField(p => p.Receivers, ref _receivers, value); }
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

        public List<VesselInCompanyDto> AllVessels
        {
            get { return allVessels; }
            set { this.SetField(p => p.AllVessels, ref allVessels, value); }
        }


        public ObservableCollection<OrderItemVM> OrderItemVms
        {
            get { return _orderItemVms; }
            set { this.SetField(vm => vm.OrderItemVms, ref _orderItemVms, value); }
        }

        private bool isInEditMode;

        #endregion

        #region ctor

        public OrderVM(IFuelController mainController, IOrderServiceWrapper serviceWrapper, IFileServiceWrapper fileServiceWrapper, IVesselInCompanyServiceWrapper vesselServiceWrapper)
        {
            this.vesselServiceWrapper = vesselServiceWrapper;
            this.isInEditMode = false;

            UploaderVm = new UploaderVM(mainController, fileServiceWrapper);
            UploaderVm.AttachmentType = AttachmentType.Order;
            Entity = new OrderDto { Id = -1 };

            this._fileServiceWrapper = fileServiceWrapper;
            this.mainController = mainController;
            this.serviceWrapper = serviceWrapper;

            Suppliers = new List<CompanyDto>();
            Receivers = new List<CompanyDto>();
            Transporters = new List<CompanyDto>();
            DisplayName = "افزودن/اصلاح سفارش ";

            RequestClose += OrderVM_RequestClose;
            this.Entity.PropertyChanged += EntityPropertyChanged;

        }

        void EntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Entity.GetPropertyName(o => o.Receiver) == e.PropertyName)
            {
                if (Entity.OrderType == OrderTypeEnum.Transfer)
                {
                    if (Entity.Receiver != null)
                        //ToVessels = UpdateVessels(Entity.Receiver.Id);
                        UpdateVessels(Entity.Receiver.Id, (res) => ToVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);
                    Entity.ToVesselInCompany = new VesselInCompanyDto();
                }
            }

            //if (Entity.GetPropertyName(o => o.Receiver) == e.PropertyName)
            //{
            //    if (Entity.OrderType == OrderTypeEnum.Transfer)
            //    {
            //        if (Entity.Receiver != null)
            //            ToVessels = UpdateVessels(Entity.Receiver.Id);
            //        Entity.ToVessel = new VesselInCompanyDto();
            //    }
            //}

            base.OnPropertyChanged(e.PropertyName);
        }

        #endregion

        #region methods

        private void Save()
        {
            if (!entity.Validate())
                return;

            ShowBusyIndicator("در حال ذخیره سازی ");

            if (Entity.Id == -1)
            {

                serviceWrapper.Add
                    (
                        (res, exp) => mainController.BeginInvokeOnDispatcher
                            (
                                () =>
                                {
                                    if (exp != null)
                                    {
                                        mainController.HandleException(exp);
                                    }
                                    else
                                    {
                                        mainController.Publish(new OrderListChangeArg());
                                        Entity = res;
                                        mainController.Close(this);
                                    }
                                }), entity);
            }
            else
            {
                serviceWrapper.Update
                    (
                        (res, exp) => mainController.BeginInvokeOnDispatcher
                            (
                                () =>
                                {
                                    if (exp != null)
                                        mainController.HandleException(exp);
                                    else
                                    {
                                        mainController.Publish(new OrderListChangeArg());
                                        Entity = res;
                                        mainController.Close(this);
                                    }
                                }), entity);
            }
            HideBusyIndicator();
        }

        public void Load(OrderDto ent, List<CompanyDto> dtos, List<VesselInCompanyDto> vesselInCompanyDtos)
        {
            this.isInEditMode = true;

            SetCollection(dtos, vesselInCompanyDtos);

            ShowBusyIndicator("در حال دریافت اطلاعات سفارش ...");
            serviceWrapper.GetById
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                if (exp == null)
                                {
                                    Entity = res;
                                    this.Entity.PropertyChanged += EntityPropertyChanged;
                                    
                                    PrepareUIForEntity();

                                    OnPropertyChanged(this.GetPropertyName(vm => vm.OrderTypeId));
                                    //OrderTypeId = (int)res.OrderType;

                                    //Entity.FromVesselInCompany = res.FromVesselInCompany;
                                    //Entity.ToVesselInCompany = res.ToVesselInCompany;
                                    //Entity.Supplier = res.Supplier;
                                    //Entity.Receiver = res.Receiver;
                                    //Entity.Transporter = res.Transporter;
                                }
                                else
                                {
                                    mainController.HandleException(exp);
                                }

                                setUploaderVmStatus();

                                HideBusyIndicator();
                            }), ent.Id);
        }

        public void SetCollection(List<CompanyDto> dtos, List<VesselInCompanyDto> vesselInCompanyDtos)
        {
            Entity.Supplier = new CompanyDto();
            Entity.Receiver = new CompanyDto();
            Entity.Transporter = new CompanyDto();
            Entity.Owner = new CompanyDto();
            Entity.FromVesselInCompany = new VesselInCompanyDto();
            Entity.ToVesselInCompany = new VesselInCompanyDto();

            FromVessels = new List<VesselInCompanyDto>();
            ToVessels = new List<VesselInCompanyDto>();
            AllVessels = vesselInCompanyDtos;

            OrderTypes = (typeof(OrderTypeEnum)).ToComboItemList().Where(i=>i.Id != (long)OrderTypeEnum.None 
                && i.Id != (long)OrderTypeEnum.SupplyForDeliveredVessel
                ).ToList();

            Suppliers = dtos;
            Receivers = dtos;
            Transporters = dtos;
        }


        private bool OrderTypeChanging(OrderTypeEnum orderType, bool init)
        {
            if (!init && Entity.Id > 0 && orderType != Entity.OrderType)
                mainController.ShowMessage("در صورت تغییر، آیتم های سفارش حذف خواهد شد", "اخطار", MessageBoxButton.OK);

            switch (orderType)
            {
                case OrderTypeEnum.InternalTransfer:

                    Entity.Supplier = new CompanyDto();
                    Entity.Transporter = new CompanyDto();
                    Entity.Receiver = new CompanyDto();

                    IsSupplierVisible = false;
                    IsTransporterVisible = true;  //The Transporter is enabled just to select the Transporter Company.
                    IsReceiverVisible = false;

                    //The vessels of Owner of Order should be selected by user.
                    IsFromVesselVisible = true;
                    IsToVesselVisible = true;

                    //Loading Vessels of Owner who is registering the InternalTransfer order for itself.
                    //FromVessels = UpdateVessels(Entity.Owner.Id);
                    //ToVessels = UpdateVessels(Entity.Owner.Id);
                    UpdateVessels(Entity.Owner.Id, (res) => FromVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);
                    UpdateVessels(Entity.Owner.Id, (res) => ToVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);

                    //Entity.FromVessel = new VesselInCompanyDto();
                    //Entity.ToVessel = new VesselInCompanyDto();

                    break;

                case OrderTypeEnum.PurchaseWithTransferOperations:
                    IsSupplierVisible = true;
                    IsTransporterVisible = true;

                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto(); //The Receiver is the same as Owner of Order.

                    //The Transporter and Supplier companies leave unchanged but selectible.

                    //No need to Select Vessels in this Order Type.
                    FromVessels = new List<VesselInCompanyDto>();
                    ToVessels = new List<VesselInCompanyDto>();

                    IsFromVesselVisible = false;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();

                    IsToVesselVisible = false;
                    Entity.ToVesselInCompany = new VesselInCompanyDto();

                    break;

                case OrderTypeEnum.Purchase:
                    IsSupplierVisible = true;  //Must be selectible.

                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto();  //The Receiver is the same as Owner of Order.

                    IsTransporterVisible = false;
                    Entity.Transporter = new CompanyDto();  //The Transporter is unknown at the time of Order Registration.

                    //No need to Select Vessels in this Order Type.
                    FromVessels = new List<VesselInCompanyDto>();
                    ToVessels = new List<VesselInCompanyDto>();

                    IsFromVesselVisible = false;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();

                    IsToVesselVisible = false;
                    Entity.ToVesselInCompany = new VesselInCompanyDto();
                    break;

                case OrderTypeEnum.Transfer:
                    IsReceiverVisible = true;   //The Receiver Company is selectible by User.
                    IsTransporterVisible = true;  //The Transporter company. Has nothing to do with FromVessel

                    IsSupplierVisible = false;
                    Entity.Supplier = new CompanyDto(); //The Supplier is the same as Owner of Order.

                    IsToVesselVisible = true;  //The receiving vessel should be selected by changing the Receiver Company
                    Entity.ToVesselInCompany = new VesselInCompanyDto();
                    if (Entity.Receiver != null)
                        UpdateVessels(Entity.Receiver.Id, (res) => ToVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);
                        //ToVessels = UpdateVessels(Entity.Receiver.Id);


                    //Loading vessels of Owner of Order which will provide the bunker for Receiving Company.
                    IsFromVesselVisible = true;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();
                    UpdateVessels(Entity.Owner.Id, (res) => FromVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);
                    //FromVessels = UpdateVessels(Entity.Owner.Id);
                    //Entity.FromVessel = new VesselInCompanyDto();

                    break;

                case OrderTypeEnum.PurchaseForVessel:
                    IsSupplierVisible = true;  //Must be selectible.
                    Entity.Supplier = new CompanyDto(); //The Supplier is the same as Owner of Order.

                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto();  //The Receiver is the same as Owner of Order.

                    IsTransporterVisible = false;
                    Entity.Transporter = new CompanyDto();  //The Transporter is unknown at the time of Order Registration.

                    //No need to Select Vessels in this Order Type.
                    FromVessels = new List<VesselInCompanyDto>();
                    IsFromVesselVisible = false;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();

                    IsToVesselVisible = true;
                    Entity.ToVesselInCompany = new VesselInCompanyDto();
                    UpdateVessels(Entity.Owner.Id, (res) => ToVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut); //Required to be selected
                    //ToVessels = UpdateVessels(Entity.Owner.Id); //Required to be selected
                    break;

                case OrderTypeEnum.SupplyForDeliveredVessel:
                    IsSupplierVisible = true;  //Must be selectible.
                    Entity.Supplier = new CompanyDto(); //The Supplier is the same as Owner of Order.

                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto();  //The Receiver is the same as Owner of Order.

                    IsTransporterVisible = false;
                    Entity.Transporter = new CompanyDto();  //The Transporter is unknown at the time of Order Registration.

                    //No need to Select Vessels in this Order Type.
                    FromVessels = new List<VesselInCompanyDto>();
                    IsFromVesselVisible = false;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();

                    IsToVesselVisible = true;
                    Entity.ToVesselInCompany = new VesselInCompanyDto();
                    UpdateVessels(Entity.Owner.Id, (res) => ToVessels = res, VesselStateEnum.CharterOut); //Required to be selected
                    //ToVessels = UpdateVessels(Entity.Owner.Id); //Required to be selected
                    break;
                default:
                    Entity.Supplier = new CompanyDto();
                    Entity.Transporter = new CompanyDto();
                    Entity.Receiver = new CompanyDto();

                    IsSupplierVisible = false;
                    IsTransporterVisible = false;  //The Transporter is enabled just to select the Transporter Company.
                    IsReceiverVisible = false;

                    //The vessels of Owner of Order should be selected by user.
                    IsFromVesselVisible = false;
                    IsToVesselVisible = false;

                    FromVessels = new List<VesselInCompanyDto>();
                    ToVessels = new List<VesselInCompanyDto>();

                    Entity.FromVesselInCompany = new VesselInCompanyDto();
                    Entity.ToVesselInCompany = new VesselInCompanyDto();

                    break;
            }
            return true;
        }

        private bool PrepareUIForEntity()
        {
            switch (Entity.OrderType)
            {
                case OrderTypeEnum.InternalTransfer:

                    IsSupplierVisible = false;
                    Entity.Supplier = new CompanyDto();

                    IsTransporterVisible = true;  //The Transporter is enabled just to select the Transporter Company.
                    //Entity.Transporter = new CompanyDto();
                    
                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto();

                    //The vessels for Owner of Order should be selected by user.
                    IsFromVesselVisible = true;
                    IsToVesselVisible = true;

                    //Loading Vessels of Owner who is registering the InternalTransfer order for itself.
                    UpdateVessels(Entity.Owner.Id, (res) => FromVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);
                    UpdateVessels(Entity.Owner.Id, (res) => ToVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);

                    break;

                case OrderTypeEnum.PurchaseWithTransferOperations:
                    IsSupplierVisible = true;
                    IsTransporterVisible = true;
                    //The Transporter and Supplier companies leave unchanged but selectible.

                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto(); //The Receiver is the same as Owner of Order.

                    //No need to Select Vessels in this Order Type.
                    FromVessels = new List<VesselInCompanyDto>();
                    ToVessels = new List<VesselInCompanyDto>();

                    IsFromVesselVisible = false;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();

                    IsToVesselVisible = false;
                    Entity.ToVesselInCompany = new VesselInCompanyDto();

                    break;

                case OrderTypeEnum.Purchase:
                    IsSupplierVisible = true;  //Must be selectible.

                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto();  //The Receiver is the same as Owner of Order.

                    IsTransporterVisible = false;
                    Entity.Transporter = new CompanyDto();  //The Transporter is unknown at the time of Order Registration.

                    //No need to Select Vessels in this Order Type.
                    FromVessels = new List<VesselInCompanyDto>();
                    ToVessels = new List<VesselInCompanyDto>();

                    IsFromVesselVisible = false;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();

                    IsToVesselVisible = false;
                    Entity.ToVesselInCompany = new VesselInCompanyDto();
                    break;

                case OrderTypeEnum.Transfer:
                    IsReceiverVisible = true;   //The Receiver Company is selectible by User.
                    IsTransporterVisible = true;  //The Transporter company. Has nothing to do with FromVessel

                    IsSupplierVisible = false;
                    Entity.Supplier = new CompanyDto(); //The Supplier is the same as Owner of Order.

                    IsToVesselVisible = true;  //The receiving vessel should be selected by changing the Receiver Company
                    if (Entity.Receiver != null)
                        UpdateVessels(Entity.Receiver.Id, (res) => ToVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);


                    //Loading vessels for Owner of Order which will provide the bunker for Receiving Company.
                    IsFromVesselVisible = true;
                    UpdateVessels(Entity.Owner.Id, (res) => FromVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut);

                    break;

                case OrderTypeEnum.PurchaseForVessel:
                    IsSupplierVisible = true;  //Must be selectible.

                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto();  //The Receiver is the same as Owner of Order.

                    IsTransporterVisible = false;
                    Entity.Transporter = new CompanyDto();  //The Transporter is unknown at the time of Order Registration.

                    //No need to Select Vessels in this Order Type.
                    FromVessels = new List<VesselInCompanyDto>();

                    IsFromVesselVisible = false;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();

                    IsToVesselVisible = true;
                    UpdateVessels(Entity.Owner.Id, (res) => ToVessels = res, VesselStateEnum.CharterIn, VesselStateEnum.Owned, VesselStateEnum.CharterOut); //Required to be selected
                    break;

                case OrderTypeEnum.SupplyForDeliveredVessel:
                    IsSupplierVisible = true;  //Must be selectible.

                    IsReceiverVisible = false;
                    Entity.Receiver = new CompanyDto();  //The Receiver is the same as Owner of Order.

                    IsTransporterVisible = false;
                    Entity.Transporter = new CompanyDto();  //The Transporter is unknown at the time of Order Registration.

                    //No need to Select Vessels in this Order Type.
                    FromVessels = new List<VesselInCompanyDto>();

                    IsFromVesselVisible = false;
                    Entity.FromVesselInCompany = new VesselInCompanyDto();

                    IsToVesselVisible = true;
                    UpdateVessels(Entity.Owner.Id, (res) => ToVessels = res, VesselStateEnum.CharterOut); //Required to be selected
                    break;
                default:
                    Entity.Supplier = new CompanyDto();
                    Entity.Transporter = new CompanyDto();
                    Entity.Receiver = new CompanyDto();

                    IsSupplierVisible = false;
                    IsTransporterVisible = false;  //The Transporter is enabled just to select the Transporter Company.
                    IsReceiverVisible = false;

                    //The vessels of Owner of Order should be selected by user.
                    IsFromVesselVisible = false;
                    IsToVesselVisible = false;

                    FromVessels = new List<VesselInCompanyDto>();
                    ToVessels = new List<VesselInCompanyDto>();

                    Entity.FromVesselInCompany = new VesselInCompanyDto();
                    Entity.ToVesselInCompany = new VesselInCompanyDto();

                    break;
            }
            return true;
        }

        private void UpdateVessels(long companyId, Action<List<VesselInCompanyDto>> resultAction, params VesselStateEnum[] vesselStates)
        {
            vesselServiceWrapper.GetAll
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                if (exp == null)
                                {
                                    resultAction(res);
                                }
                                else
                                {
                                    mainController.HandleException(exp);
                                }
                            }), new
                            {
                                companyId,
                                vesselStates = string.Join(",", vesselStates.Select(s=>(int)s))
                            });

            //var vessels = AllVessels.Where(c => c.CompanyId == companyId && (c.VesselState == VesselStateEnum.CharterIn || c.VesselState == VesselStateEnum.Owned || c.VesselState == VesselStateEnum.CharterOut)).ToList();
            //return vessels;
        }


        private void OrderVM_RequestClose(object sender, EventArgs e)
        {
            mainController.Close(this);
        }

        public void SetServiceWrapper(IOrderServiceWrapper serviceWrapper)
        {
            this.serviceWrapper = serviceWrapper;
        }

        public void SetMainController(IFuelController mainController)
        {
            this.mainController = mainController;
        }

        #endregion

        public void AddNewOrder(List<CompanyDto> dtos, List<VesselInCompanyDto> vesselInCompanyDtos)
        {
            this.isInEditMode = false;

            SetCollection(dtos, vesselInCompanyDtos);

            Entity.Owner = mainController.GetCurrentUser().User.CompanyDto;
            //Entity.OrderDate = DateTime.Now;

            setUploaderVmStatus();
        }

        //================================================================================

        public void setUploaderVmStatus()
        {
            if (UploaderVm != null)
            {
                UploaderVm.IsVisible = this.isInEditMode &&
                    (this.Entity.ApproveStatus == WorkflowStageEnum.Initial ||
                    this.Entity.ApproveStatus == WorkflowStageEnum.Approved ||
                    this.Entity.ApproveStatus == WorkflowStageEnum.SubmitRejected);
                UploaderVm.EntityId = this.Entity.Id <= 0 ? 0 : this.Entity.Id;
            }
        }
    }
}