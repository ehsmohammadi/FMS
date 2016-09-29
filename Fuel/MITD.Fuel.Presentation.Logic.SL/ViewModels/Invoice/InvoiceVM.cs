using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Castle.Core.Internal;
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
    public class InvoiceVM : WorkspaceViewModel,
                             IEventHandler<RefrencedInvoiceEvent>,
                             IEventHandler<RefrencedOrderEvent>,
                             IEventHandler<InvoiceAdditionalPriceListChangedArg>
    {
        #region props


        public UploaderVM UploaderVm { get; set; }
        private readonly IInvoiceController controller;
        private readonly ICurrencyServiceWrapper currencyServiceWrapper;
        private readonly IFuelController mainController;

        private readonly IInvoiceServiceWrapper serviceWrapper;

        private ObservableCollection<CompanyDto> companies;
        private bool companyIsEditable;
        private ObservableCollection<CurrencyDto> currencies;
        public CurrencyDto currentCurrency;
        private bool divisionMethodIsEnable;
        private List<ComboBoxItm> divisionMethods;
        private InvoiceDto entity;
        private OrderDto firstOrder;
        private ObservableCollection<InvoiceItemDto> invoiceItems;
        private bool invoiceReferenceVisible;

        private List<ComboBoxItm> invoiceTypes;
        private bool isCriditeEnable;
        private bool orderReferenceVisible;
        private long selectedCurrencyId;
        private InvoiceItemDto selectedInvoiceItem;

        private CommandViewModel submitCommand;

        #region column visibility

        #endregion

        #region Command

        private CommandViewModel cancelCommand;
        private CommandViewModel deleteItemCommand;
        private CommandViewModel editItemCommand;
        private CommandViewModel execDivision;
        private long invoiceTypeId;
        private CommandViewModel manageFactors;
        private CommandViewModel referenceCommand;

        public CommandViewModel SubmitCommand
        {
            get { return submitCommand ?? (submitCommand = new CommandViewModel("ذخیره", new DelegateCommand(Save))); }
        }

        public CommandViewModel EditItemCommand
        {
            get
            {
                return editItemCommand ?? (editItemCommand = new CommandViewModel
                    (
                    "ویرایش", new DelegateCommand
                                  (
                                  () =>
                                  {
                                      if (CheckIsSelected())
                                      {
                                          controller.EditItem
                                              (
                                                  SelectedInvoiceItem, Entity.DivisionMethod, currentCurrency.CurrencyToMainCurrencyRate,
                                                  Entity.InvoiceType);
                                      }
                                  })));
            }
        }

        public CommandViewModel DeleteItemCommand
        {
            get
            {
                return deleteItemCommand ?? (deleteItemCommand = new CommandViewModel
                    (
                    "حذف", new DelegateCommand
                               (
                               () =>
                               {
                                   if (!CheckIsSelected())
                                       return;

                                   // if (mainController.ShowConfirmationBox("آیا برای حذف مطمئن هستید ", "اخطار"))
                                   Entity.InvoiceItems.Remove(SelectedInvoiceItem);
                               })));
            }
        }

        public CommandViewModel CancelCommand
        {
            get { return cancelCommand ?? (cancelCommand = new CommandViewModel("خروج", new DelegateCommand(() => mainController.Close(this)))); }
        }

        public CommandViewModel ReferenceCommand
        {
            get { return referenceCommand ?? (referenceCommand = new CommandViewModel("انتخاب", new DelegateCommand(OpenReference))); }
        }

        public CommandViewModel ManageFactors
        {
            get
            {
                return manageFactors ?? (manageFactors = new CommandViewModel
                    (
                    "انتخاب/ویرایش", new DelegateCommand
                                            (
                                            () =>
                                            {
                                                if (currentCurrency == null)
                                                {
                                                    MessageBox.Show("لطفا نوع ارز را انتخاب کنید");
                                                    return;
                                                }
                                                controller.ManageAdditionalPrice(Entity, currentCurrency.CurrencyToMainCurrencyRate, UniqId);
                                            })));
            }
        }

        public CommandViewModel ExecDivision
        {
            get { return execDivision ?? (execDivision = new CommandViewModel("محاسبه/ثبت", new DelegateCommand(DoDivision))); }
        }

        private void DoDivision()
        {
            if (Entity.DivisionMethod != DivisionMethodEnum.None && (Entity.AdditionalPrices == null || Entity.AdditionalPrices.Count(ap => ap.Divisionable) == 0))
            {
                //MessageBox.Show("لطفاً عوامل تأثیرگذار را انتخاب نمایید");
                return;
            }
            if (Entity.DivisionMethod == DivisionMethodEnum.None && !(Entity.AdditionalPrices == null || Entity.AdditionalPrices.Count(ap => ap.Divisionable) == 0))
            {
                //MessageBox.Show("لطفاً نوع تسهیم پذیری را مشخص نمایید");
                return;
            }

            //if (Entity.DivisionMethod == DivisionMethodEnum.Direct && !(Entity.AdditionalPrices == null || Entity.AdditionalPrices.Count == 0))
            if (Entity.DivisionMethod == DivisionMethodEnum.Direct) //A.H: Above condition is simplified due to checking by earlier "if" condition.
            {
                return;
            }

            clearDivisionPrices();

            //NOTE: The calculation request will be sent to server in both selected or both not selected DevisionMethod and AdditionalPrices.
            serviceWrapper.CalculateAdditionalPrice
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                if (exp == null)
                                {
                                    Entity = res;
                                }
                                else
                                    mainController.HandleException(exp);
                                HideBusyIndicator();
                            }), Entity);
        }

        private void clearDivisionPrices()
        {
            if (Entity.DivisionMethod == DivisionMethodEnum.None)
            {
                Entity.InvoiceItems.ForEach(item =>{
                    item.DivisionPrice = 0;
                    item.DivisionPriceInMainCurrency = 0;
                });
            }
        }

        #endregion

        public InvoiceDto Entity
        {
            get { return entity; }
            set
            {
                this.SetField(p => p.Entity, ref entity, value);
            }
        }

        public OrderDto FirstOrder
        {
            get { return firstOrder; }
            set { this.SetField(p => p.FirstOrder, ref firstOrder, value); }
        }


        public List<ComboBoxItm> InvoiceTypes
        {
            get { return invoiceTypes; }
            set { this.SetField(p => p.InvoiceTypes, ref invoiceTypes, value); }
        }


        public long InvoiceTypeId
        {
            get { return (long)Entity.InvoiceType; }
            set
            {
                InvoiceTypeChanging((InvoiceTypeEnum)value);
                Entity.InvoiceType = (InvoiceTypeEnum)value;
                this.SetField(p => p.InvoiceTypeId, ref invoiceTypeId, value);
            }
        }

        public List<ComboBoxItm> DivisionMethods
        {
            get { return divisionMethods; }
            set { this.SetField(p => p.DivisionMethods, ref divisionMethods, value); }
        }

        public ObservableCollection<CurrencyDto> Currencies
        {
            get { return currencies; }
            set { this.SetField(p => p.Currencies, ref currencies, value); }
        }


        public ObservableCollection<CompanyDto> Companies
        {
            get { return companies; }
            set { this.SetField(p => p.Companies, ref companies, value); }
        }

        public bool DivisionMethodIsEnable
        {
            get { return divisionMethodIsEnable; }
            set { this.SetField(p => p.DivisionMethodIsEnable, ref divisionMethodIsEnable, value); }
        }

        public long SelectedCurrencyId
        {
            get { return Entity.CurrencyId; }
            set
            {
                Entity.CurrencyId = value;
                currencyServiceWrapper.GetById(
                        (result, exception) => mainController.BeginInvokeOnDispatcher(() =>{
                            if (exception == null)
                                currentCurrency = result;
                            else
                                mainController.HandleException(exception);
                        }), value);

                this.SetField(p => p.SelectedCurrencyId, ref selectedCurrencyId, value);
            }
        }


        public InvoiceItemDto SelectedInvoiceItem
        {
            get { return selectedInvoiceItem; }
            set { this.SetField(vm => vm.SelectedInvoiceItem, ref selectedInvoiceItem, value); }
        }

        public bool CompanyIsEditable
        {
            get { return companyIsEditable; }
            set { this.SetField(p => p.CompanyIsEditable, ref companyIsEditable, value); }
        }

        public bool OrderReferenceVisible
        {
            get { return orderReferenceVisible; }
            set { this.SetField(p => p.OrderReferenceVisible, ref orderReferenceVisible, value); }
        }

        public bool InvoiceReferenceVisible
        {
            get { return invoiceReferenceVisible; }
            set { this.SetField(p => p.InvoiceReferenceVisible, ref invoiceReferenceVisible, value); }
        }

        public bool IsCriditeEnable
        {
            get { return isCriditeEnable; }
            set { this.SetField(p => p.IsCriditeEnable, ref isCriditeEnable, value); }
        }

        public CompanyDto GetOwnerCompany()
        {
            if (Entity.OwnerId == 0)
            {
                MessageBox.Show("اول شرکت را انتخاب کنید");
                return null;
            }
            return Companies.Count > 0 ? Companies.Single(c => c.Id == Entity.OwnerId) : null;
        }

        public long DivisionMethodId
        {
            get { return (int)Entity.DivisionMethod; }
            set
            {
                Entity.DivisionMethod = (DivisionMethodEnum)value;
                this.SetField(p => p.DivisionMethodId, ref divisionMethodId, value);
            }
        }

        private bool isInEditMode;

        protected Guid UniqId { get; set; }

        #endregion

        #region ctor

        public InvoiceVM()
        {
            isInEditMode = false;
        }


        public InvoiceVM(IFuelController mainController,
                         IInvoiceController controller,
                         IInvoiceServiceWrapper serviceWrapper,
                         ICurrencyServiceWrapper currencyServiceWrapper,
                         IFileServiceWrapper fileServiceWrapper):this()
        {

            UploaderVm = new UploaderVM(mainController, fileServiceWrapper);
            UploaderVm.AttachmentType = AttachmentType.Invoice;
            Entity = new InvoiceDto();
            DivisionMethodIsEnable = true;
            CompanyIsEditable = true;
            Entity.AdditionalPrices = new ObservableCollection<InvoiceAdditionalPriceDto>();

            this.mainController = mainController;
            this.controller = controller;
            this.serviceWrapper = serviceWrapper;
            this.currencyServiceWrapper = currencyServiceWrapper;
            Entity.OrderRefrences = new ObservableCollection<OrderDto>();
            GetCurrencies();


            DisplayName = "افزودن/اصلاح صورتحساب ";
            UniqId = Guid.NewGuid();

            RequestClose += InvoiceVM_RequestClose;
            this.Entity.PropertyChanged += EntityPropertyChanged;
            this.PropertyChanged += InvoiceVM_PropertyChanged;
        }

        void InvoiceVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.DivisionMethodId))
            {
                DoDivision();
            }
        }

        private void EntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "InvoiceRefrence":
                    if (Entity.InvoiceRefrence != null)
                        Entity.DivisionMethod = Entity.InvoiceRefrence.DivisionMethod;
                    break;

                case "InvoiceType":
                    UpdateInvoiceType();

                    break;
                default:
                    break;
            }
            base.OnPropertyChanged(e.PropertyName);
        }

        #endregion

        #region methods

        private void Save()
        {
            if (!Entity.Validate())
                return;

            if (Entity.DivisionMethod != DivisionMethodEnum.None && (Entity.AdditionalPrices == null || Entity.AdditionalPrices.Count(ap => ap.Divisionable) == 0))
            {
                MessageBox.Show("لطفاً عوامل تأثیرگذار و تسهیم پذیر را انتخاب نمایید");
                return;
            }
            if (Entity.DivisionMethod == DivisionMethodEnum.None && !(Entity.AdditionalPrices == null || Entity.AdditionalPrices.Count(ap => ap.Divisionable) == 0))
            {
                MessageBox.Show("لطفاً نوع تسهیم پذیری را مشخص نمایید");
                return;
            }


            ShowBusyIndicator("در حال ذخیره سازی ");

            if (Entity.Id == 0)
            {
                Entity.OwnerId = mainController.GetCurrentUser().User.CompanyDto.Id;

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
                                        mainController.Publish(new InvoiceListChangeArg());
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
                                        mainController.Publish(new InvoiceListChangeArg());
                                        Entity = res;
                                        mainController.Close(this);
                                    }
                                }), entity);
            }
            HideBusyIndicator();
        }


        private void OpenReference()
        {
            ValidateInvoiceDate();

            if (GetOwnerCompany() != null)
                if (Entity.InvoiceType == InvoiceTypeEnum.Attach)
                    controller.ShowInvoiceReference(GetOwnerCompany(), Entity);
                else
                {
                    //OrderDto firstOrder = null;
                    //if (Entity.OrderRefrences != null && Entity.OrderRefrences.Count > 0)
                    //    firstOrder = Entity.OrderRefrences.First();
                    controller.ShowOrderReference(GetOwnerCompany(), Entity);
                }
        }

        private void ValidateInvoiceDate()
        {
            if (Entity.InvoiceDate == DateTime.MinValue)
                MessageBox.Show("لطفا تاریخ صورتحساب را انتخاب نمایید");
        }


        public void Edit(InvoiceDto ent, List<CompanyDto> allCompanies)
        {
            this.isInEditMode = true;
            ShowBusyIndicator("در حال دریافت اطلاعات سفارش ...");
            setCollection(allCompanies);
            GetInvoice(ent);
            UpdateInvoiceType();
            HideBusyIndicator();
            UniqId = Guid.NewGuid();
            
            this.setUploaderVmStatus();
        }

        private void GetInvoice(InvoiceDto ent)
        {
            ShowBusyIndicator("در حال دریافت اطلاعات  ...");
            serviceWrapper.GetById
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                if (exp == null)
                                {
                                    Entity = res;
                                    SelectedCurrencyId = Entity.CurrencyId;
                                    InvoiceTypeId = (int)Entity.InvoiceType;
                                    DivisionMethodId = (long)Entity.DivisionMethod;
                                }
                                else
                                    mainController.HandleException(exp);
                            }), ent.Id);
            HideBusyIndicator();
        }

        private void GetCurrencies()
        {
            currencyServiceWrapper.GetAllCurrency
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                if (exp == null)
                                    Currencies = new ObservableCollection<CurrencyDto>(res);
                                else
                                    mainController.HandleException(exp);
                            }));
        }

        public void Add(List<CompanyDto> userCompanies)
        {
            this.isInEditMode = false;

            this.setCollection(userCompanies);

            UpdateInvoiceType();

            setUploaderVmStatus();
        }

        private void setCollection(List<CompanyDto> userCompanies)
        {
            //InvoiceTypes = EnumHelper.GetItems(typeof(InvoiceTypeEnum));
            InvoiceTypes = (typeof (InvoiceTypeEnum)).ToComboItemList().Where(i => i.Id != (long) InvoiceTypeEnum.SupplyForDeliveredVessel).ToList();
            DivisionMethods = typeof (DivisionMethodEnum).ToComboItemList();

            Companies = new ObservableCollection<CompanyDto>(userCompanies);
            
            if (Companies.Count == 1)
            {
                Entity.OwnerId = Companies[0].Id;
            }

            //DivisionMethods = EnumHelper.GetItems(typeof(DivisionMethodEnum));
            Entity.InvoiceDate = DateTime.Now;
        }

        private void InvoiceVM_RequestClose(object sender, EventArgs e)
        {
            mainController.Close(this);
        }

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

        //================================================================================

        #endregion

        #region IEventHandlers

        private long divisionMethodId;

        #region IEventHandler<InvoiceAdditionalPriceListChangedArg> Members

        public void Handle(InvoiceAdditionalPriceListChangedArg eventData)
        {
            Entity.AdditionalPrices.Clear();
            eventData.Data.ForEach(Entity.AdditionalPrices.Add);

            this.OnPropertyChanged(this.GetPropertyName(p=>p.Entity));

            //CalculateSumOfAdditionalPrice();

            DoDivision();
        }

        private void CalculateSumOfAdditionalPrice()
        {
            if (Entity.AdditionalPrices == null || Entity.AdditionalPrices.Count==0)
                Entity.TotalOfDivisionPrice = 0;
            Entity.TotalOfDivisionPrice = Entity.AdditionalPrices.Sum(c => c.Price * (int)c.EffectiveFactorType);
        }

        #endregion

        #region IEventHandler<RefrencedInvoiceEvent> Members

        public void Handle(RefrencedInvoiceEvent eventData)
        {
            if (eventData.ReferencedInvoice == null)
                return;
            if (Entity.InvoiceRefrence != null && Entity.InvoiceRefrence.Id == eventData.ReferencedInvoice.Id)
                return;
            Entity.InvoiceRefrence = eventData.ReferencedInvoice;
            Entity.InvoiceItems = Entity.InvoiceRefrence.InvoiceItems;
            DivisionMethodId = (long)eventData.ReferencedInvoice.DivisionMethod;

            foreach (var invoiceItem in Entity.InvoiceItems)
            {
                invoiceItem.Fee = 0;
                invoiceItem.DivisionPrice = 0;
                Entity.SupplierId = null;
                Entity.TransporterId = null;
            }
        }

        #endregion

        #region IEventHandler<RefrencedOrderEvent> Members

        public void Handle(RefrencedOrderEvent eventData)
        {
            if (!eventData.Changed)
                return;
            var orderIdList = eventData.ReferencedOrders.Select(c => c.Id).ToList();
            if (Entity.OrderRefrences == null || Entity.OrderRefrences.Count(c => orderIdList.Contains(c.Id)) == orderIdList.Count)
                return;
            if (orderIdList.Count > 0)
            {
                FirstOrder = eventData.ReferencedOrders.First();
                if (Entity.InvoiceType == InvoiceTypeEnum.PurchaseOperations)
                {
                    if (firstOrder.Transporter != null)
                    {
                        Entity.TransporterId = firstOrder.Transporter.Id;
                        Entity.TransporterName = firstOrder.Transporter.Name;
                    }

                }

                if (firstOrder.Supplier != null)
                {
                    Entity.SupplierId = firstOrder.Supplier.Id;
                    Entity.SupplierName = firstOrder.Supplier.Name;
                }

                Entity.OrderRefrences = new ObservableCollection<OrderDto>(eventData.ReferencedOrders);
            }

            GetInvoiceItems(orderIdList);
        }

        #endregion

        #endregion

        private bool InvoiceTypeChanging(InvoiceTypeEnum newInvoiceType)
        {
            if (Entity.InvoiceType == newInvoiceType)
                return false;

            Entity.InvoiceItems = new PagedSortableCollectionView<InvoiceItemDto>();

            FirstOrder = null;
            Entity.TransporterId = null;
            Entity.TransporterName = "";
            Entity.SupplierId = null;
            Entity.SupplierName = "";
            return true;
        }

        private void UpdateInvoiceType()
        {
            if (Entity.InvoiceType == InvoiceTypeEnum.Attach)
            {
                DivisionMethodIsEnable = false;
                IsCriditeEnable = true;
                InvoiceReferenceVisible = true;
                OrderReferenceVisible = false;
                Entity.OrderRefrences = new ObservableCollection<OrderDto>();
            }
            else
            {
                DivisionMethodIsEnable = true;
                IsCriditeEnable = false;
                InvoiceReferenceVisible = false;
                OrderReferenceVisible = true;
                Entity.InvoiceRefrence = null;
                Entity.IsCreditor = false;
            }
        }

        private void GetInvoiceItems(List<long> orderIdList)
        {
            ShowBusyIndicator("در حال دریافت اطلاعات");
            serviceWrapper.GenerateItemByFilter
                (
                    (res, exp) => mainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                if (exp == null)
                                {
                                    Entity.InvoiceItems = new PagedSortableCollectionView<InvoiceItemDto> { SourceCollection = res };
                                    if (Entity.InvoiceItems.Count == 0)
                                        mainController.ShowMessage(" برای سفارش مورد نظر رسید انبار جدیدی وارد سیستم نشده است ");
                                }
                                else
                                    mainController.HandleException(exp);

                                HideBusyIndicator();
                            }), orderIdList);
        }

        #region IvoiceItem

        private bool CheckIsSelected()
        {
            if (SelectedInvoiceItem == null)
            {
                mainController.ShowMessage("لطفا قلم صورتحساب را انتخاب نمائید");
                return false;
            }
            if (SelectedCurrencyId == 0)
            {
                mainController.ShowMessage("لطفا نوع ارز را انتخاب نمائید");
                return false;
            }

            return true;
        }

        #endregion
    }

    public class InvoiceTypeToReferencesTitleTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is long)
            {
                var selectedValue = (InvoiceTypeEnum) (long)value;

                switch (selectedValue)
                {
                    case InvoiceTypeEnum.Purchase:
                    case InvoiceTypeEnum.PurchaseOperations:
                    case InvoiceTypeEnum.SupplyForDeliveredVessel:
                        return "سفارش (ها)";
                        
                    case InvoiceTypeEnum.Attach:
                        return "صورتحساب اصلی";
                }
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }

    public class InvoiceTypeToDebitCreditVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long)
            {
                var selectedValue = (InvoiceTypeEnum)(long)value;

                switch (selectedValue)
                {
                    case InvoiceTypeEnum.Purchase:
                    case InvoiceTypeEnum.PurchaseOperations:
                        return Visibility.Collapsed;

                    case InvoiceTypeEnum.Attach:
                        return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }

    public class OrderReferencesToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<OrderDto>)
            {
                var references = (ObservableCollection<OrderDto>)value;
                return string.Join(", ", references.Select(p=>p.Code));
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }

    public class InvoiceAdditionalPricesToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<InvoiceAdditionalPriceDto>)
            {
                var additionalPrices = (ObservableCollection<InvoiceAdditionalPriceDto>)value;
                return string.Join(", ", additionalPrices.Select(p => p.EffectiveFactorName));
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
    public class InvoiceAdditionalPricesToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is InvoiceDto)
            {
                var entity = value as InvoiceDto;
                return !(entity.DivisionMethod != DivisionMethodEnum.None && (entity.AdditionalPrices == null || entity.AdditionalPrices.Count(ap => ap.Divisionable) == 0))
                        && !((entity.DivisionMethod == DivisionMethodEnum.None && !(entity.AdditionalPrices == null || entity.AdditionalPrices.Count(ap => ap.Divisionable) == 0)));
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
