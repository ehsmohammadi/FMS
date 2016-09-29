using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class TransactionListVM : WorkspaceViewModel, IEventHandler<TransactionListChangedArg>
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IInventoryTransactionController transactionController;
        private readonly IInventoryTransactionServiceWrapper transactionServiceWrapper;
        private readonly IInventoryCompanyServiceWrapper companyServiceWrapper;
        private readonly IInventoryOperationServiceWrapper inventoryOperationServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string INVALID_RECORD_MESSAGE = "رکورد مورد نظر یافت نشد.";
        private const string READONLY_RECORD_MESSAGE = "رکورد مورد نظر قابل حذف و یا ویرایش نمی باشد.";
        private const string SEARCH_COMMAND_TEXT = "جستجو";
        private const string CLEAR_SEARCH_COMMAND_TEXT = "سعی مجدد";
        private const string VIEW_COMMAND_TEXT = "نمایش";
        private const string PRICING_COMMAND_TEXT = "قیمت گذاری";
        private const string VOUCHER_COMMAND_TEXT = "ایجاد اسناد";
        private const string VIEW_REFERENCE_COMMAND_TEXT = "نمایش مرجع";
        private const string PRICING_QUESTION_TEXT = "آیا برای قیمت گذاری اطمینان دارید؟";
        private const string VOUCHER_QUESTION_TEXT = "آیا برای ایجاد اسناد اطمینان دارید؟";
        //================================================================================

        private TransactionListFilteringVM filtering;
        public TransactionListFilteringVM Filtering
        {
            get { return filtering; }
            set { this.SetField(p => p.Filtering, ref filtering, value); }
        }

        private PagedSortableCollectionView<Inventory_TransactionDto> pagedTransactionData;
        public PagedSortableCollectionView<Inventory_TransactionDto> PagedTransactionData
        {
            get { return pagedTransactionData; }
            set { this.SetField(p => p.PagedTransactionData, ref pagedTransactionData, value); }
        }

        private PagedSortableCollectionView<Inventory_TransactionDetailDto> pagedTransactionDetailData;
        public PagedSortableCollectionView<Inventory_TransactionDetailDto> PagedTransactionDetailData
        {
            get { return pagedTransactionDetailData; }
            set { this.SetField(p => p.PagedTransactionDetailData, ref pagedTransactionDetailData, value); }
        }

        private PagedSortableCollectionView<Inventory_TransactionDetailPriceDto> pagedTransactionDetailPriceData;
        public PagedSortableCollectionView<Inventory_TransactionDetailPriceDto> PagedTransactionDetailPriceData
        {
            get { return pagedTransactionDetailPriceData; }
            set { this.SetField(p => p.PagedTransactionDetailPriceData, ref pagedTransactionDetailPriceData, value); }
        }

        private Inventory_TransactionDto selectedTransaction;

        public Inventory_TransactionDto SelectedTransaction
        {
            get { return selectedTransaction; }
            set { this.SetField(p => p.SelectedTransaction, ref selectedTransaction, value); }
        }

        private Inventory_TransactionDetailDto selectedTransactionDetail;

        public Inventory_TransactionDetailDto SelectedTransactionDetail
        {
            get { return selectedTransactionDetail; }
            set { this.SetField(p => p.SelectedTransactionDetail, ref selectedTransactionDetail, value); }
        }

        private Inventory_TransactionDetailPriceDto selectedTransactionDetailPrice;

        public Inventory_TransactionDetailPriceDto SelectedTransactionDetailPrice
        {
            get { return selectedTransactionDetailPrice; }
            set { this.SetField(p => p.SelectedTransactionDetailPrice, ref selectedTransactionDetailPrice, value); }
        }

        //================================================================================

        private CommandViewModel viewCommand;
        public CommandViewModel ViewCommand
        {
            get
            {
                if (viewCommand == null)
                    viewCommand = new CommandViewModel(SEARCH_COMMAND_TEXT, new DelegateCommand(this.viewTransaction));

                return viewCommand;
            }
        }

        private CommandViewModel clearViewCommand;
        public CommandViewModel ClearViewCommand
        {
            get
            {
                if (clearViewCommand == null)
                    clearViewCommand = new CommandViewModel(CLEAR_SEARCH_COMMAND_TEXT, new DelegateCommand(this.Filtering.ResetToDefaults));

                return clearViewCommand;
            }
        }

        private CommandViewModel pricingCommand;
        public CommandViewModel PricingCommand
        {
            get
            {
                if (pricingCommand == null)
                    pricingCommand = new CommandViewModel(PRICING_COMMAND_TEXT, new DelegateCommand(this.pricingTransaction));

                return pricingCommand;
            }
        }

        private CommandViewModel voucherCommand;
        public CommandViewModel VoucherCommand
        {
            get
            {
                if (voucherCommand == null)
                    voucherCommand = new CommandViewModel(VOUCHER_COMMAND_TEXT, new DelegateCommand(this.voucherTransaction));

                return voucherCommand;
            }
        }

        private CommandViewModel viewInventoryTransactionReferenceCommand;
        public CommandViewModel ViewInventoryTransactionReferenceCommand
        {
            get
            {
                if (viewInventoryTransactionReferenceCommand == null)
                    viewInventoryTransactionReferenceCommand = new CommandViewModel(VIEW_REFERENCE_COMMAND_TEXT, new DelegateCommand(this.viewInventoryTransactionReference));

                return viewInventoryTransactionReferenceCommand;
            }
        }

        private CommandViewModel viewInventoryTransactionDetailPricingReferenceCommand;
        public CommandViewModel ViewInventoryTransactionDetailPricingReferenceCommand
        {
            get
            {
                if (viewInventoryTransactionDetailPricingReferenceCommand == null)
                    viewInventoryTransactionDetailPricingReferenceCommand = new CommandViewModel(VIEW_REFERENCE_COMMAND_TEXT, new DelegateCommand(this.viewInventoryTransactionDetailPricingReference));

                return viewInventoryTransactionDetailPricingReferenceCommand;
            }
        }

        //================================================================================

        public TransactionListVM()
        {
            this.DisplayName = "عملیات انبارداری";

            this.PagedTransactionData = new PagedSortableCollectionView<Inventory_TransactionDto>();

            this.PagedTransactionDetailData = new PagedSortableCollectionView<Inventory_TransactionDetailDto>();

            this.PagedTransactionDetailPriceData =
                new PagedSortableCollectionView<Inventory_TransactionDetailPriceDto>();

            this.PagedTransactionData.PageChanged += PagedTransactionData_PageChanged;

            this.PagedTransactionDetailData.PageChanged += PagedTransactionDetailData_PageChanged;

            this.PagedTransactionDetailPriceData.PageChanged += PagedTransactionDetailPriceData_PageChanged;

            this.PropertyChanged += TransactionListVM_PropertyChanged;
        }

        public TransactionListVM(
            IFuelController fuelMainController,
            IInventoryTransactionController transactionController,
            IInventoryTransactionServiceWrapper transactionServiceWrapper,
            IInventoryCompanyServiceWrapper companyServiceWrapper,
            TransactionListFilteringVM filtering)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.transactionController = transactionController;
            this.transactionServiceWrapper = transactionServiceWrapper;
            this.companyServiceWrapper = companyServiceWrapper;
            this.Filtering = filtering;
            this.Filtering.PropertyChanged += Filtering_PropertyChanged;
        }

        //================================================================================

        #region Event Handlers

        //================================================================================

        void PagedTransactionData_PageChanged(object sender, EventArgs e)
        {
            this.viewTransaction();
        }

        //================================================================================

        void PagedTransactionDetailData_PageChanged(object sender, EventArgs e)
        {
            this.loadTransactionDetails();
        }

        //================================================================================

        void PagedTransactionDetailPriceData_PageChanged(object sender, EventArgs e)
        {
            this.loadTransactionDetailPrices();
        }

        //================================================================================

        private void Filtering_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.clearTransactionData();
            if (e.PropertyName == "SelectedCompany")
            {
                if (Filtering.SelectedCompany != null)
                {
                    this.companyServiceWrapper.GetWarehouse(
                        (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                if (exception == null)
                                {
                                    if (result != null)
                                    {
                                        Filtering.Warehouse.Clear();
                                        //new Inventory_WarehouseDto()
                                        //                        {
                                        //                            Name = "انتخاب نمایید...",
                                        //                            Id = 0,
                                        //                        }
                                        Filtering.Warehouse.Add(null);
                                        result.ForEach(c => Filtering.Warehouse.Add(c));
                                    }
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }
                                this.HideBusyIndicator();
                            }), Filtering.SelectedCompany.Id);
                }
                else
                    Filtering.Warehouse.Clear();
            }
        }

        //================================================================================

        public void Handle(TransactionListChangedArg eventData)
        {
            this.viewTransaction();
        }

        ////================================================================================

        //public void Handle(ScrapDetailListChangedArg eventData)
        //{
        //    this.loadTransactionDetails();
        //}

        //================================================================================


        private void TransactionListVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.SelectedTransaction))
            {
                if (SelectedTransaction != null)
                    this.loadTransactionDetails();
                else clearTransactionDetailData();
            }
            if (e.PropertyName == this.GetPropertyName(p => p.SelectedTransactionDetail))
            {
                if (SelectedTransactionDetail != null)
                    this.loadTransactionDetailPrices();
                else clearTransactionDetailPriceData();
            }
        }

        #endregion

        //================================================================================

        protected override void OnRequestClose()
        {
            this.fuelMainController.Close(this);
        }

        //================================================================================

        public void Load()
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.companyServiceWrapper.GetAll(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                    this.Filtering.Initialize(result);
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }), true
                );
        }

        //================================================================================
        private void viewTransaction()
        {
            clearTransactionData();

            bool isValid = Filtering.Validate();
            if (!isValid || Filtering.SelectedCompanyId == 0)
                return;

            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.transactionServiceWrapper.GetPagedTransactionDataByFilter(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                                                                           () =>
                                                                                           {
                                                                                               if (exception == null)
                                                                                               {
                                                                                                   this.PagedTransactionData.SetPagedDataCollection(result);
                                                                                               }
                                                                                               else
                                                                                               {
                                                                                                   this.fuelMainController.HandleException(exception);
                                                                                               }

                                                                                               this.HideBusyIndicator();
                                                                                           }),
                this.Filtering.SelectedCompanyId,
                this.Filtering.SelectedWarehouseId,
                this.Filtering.FromDate,
                this.Filtering.ToDate,
                (byte)this.Filtering.SelectedTransactionType,
                (byte)this.Filtering.SelectedTransactionStatus,
                null,
                this.PagedTransactionData.PageSize,
                this.PagedTransactionData.PageIndex);
        }

        //================================================================================

        private void clearTransactionData()
        {
            if (PagedTransactionData != null)
                this.PagedTransactionData.Clear();
            this.SelectedTransaction = null;
            clearTransactionDetailData();
        }

        //================================================================================

        private void clearTransactionDetailData()
        {
            if (PagedTransactionDetailData != null)
                this.PagedTransactionDetailData.Clear();
            this.SelectedTransactionDetail = null;
            clearTransactionDetailPriceData();
        }

        //================================================================================

        private void clearTransactionDetailPriceData()
        {
            if (PagedTransactionDetailPriceData != null)
                this.PagedTransactionDetailPriceData.Clear();
            this.SelectedTransactionDetailPrice = null;
        }


        //================================================================================

        private bool isTransactionSelected()
        {
            if (SelectedTransaction == null)
            {
                this.fuelMainController.ShowMessage("از لیست  ردیف مورد نظر را انتخاب نمایید.");
                return false;
            }

            return true;
        }

        //================================================================================

        private bool isTransactionDetailSelected()
        {
            if (SelectedTransactionDetail == null)
            {
                this.fuelMainController.ShowMessage("از لیست جزئیات ردیف مورد نظر را انتخاب نمایید");
                return false;
            }

            return true;
        }

        //================================================================================

        private void loadTransactionDetails()
        {
            return;
            this.clearTransactionDetailData();

            if (SelectedTransaction == null)
                return;

            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.transactionServiceWrapper.GetPagedTransactionDetailDataByFilter
                (
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                this.HideBusyIndicator();
                                if (exception == null)
                                {
                                    this.PagedTransactionDetailData.SetPagedDataCollection(result);
                                    if (result.Result.Count == 1)
                                        this.SelectedTransactionDetail = result.Result[0];
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }
                            }), this.SelectedTransaction.Id, this.PagedTransactionDetailData.PageSize, this.PagedTransactionDetailData.PageIndex);
        }

        //================================================================================

        private void loadTransactionDetailPrices()
        {
            return;

            clearTransactionDetailPriceData();

            if (this.SelectedTransactionDetail != null)
            {
                ShowBusyIndicator("در حال دریافت اطلاعات قیمت گذاری شده");

                this.transactionServiceWrapper.GetPagedTransactionDetailPriceDataByFilter((result, exception) =>
                    this.fuelMainController.BeginInvokeOnDispatcher(() =>
                    {
                        if (exception == null)
                        {
                            this.PagedTransactionDetailPriceData.SetPagedDataCollection(result);
                        }
                        else
                        {
                            this.fuelMainController.HandleException(exception);
                        }
                        HideBusyIndicator();
                    }), SelectedTransaction.Id, SelectedTransactionDetail.Id, this.PagedTransactionDetailPriceData.PageSize, this.PagedTransactionDetailPriceData.PageIndex);
            }
        }

        //================================================================================

        private void pricingTransaction()
        {
            bool isValid = Filtering.Validate();
            if (!isValid || Filtering.SelectedCompanyId == 0)
                return;

            this.ShowBusyIndicator("در حال قیمت گذاری ...");

            this.transactionServiceWrapper.PricingTransaction(
                (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                    () =>
                    {
                        if (exception == null)
                        {
                            viewTransaction();
                        }
                        else
                        {
                            this.fuelMainController.HandleException(exception);
                        }

                        this.HideBusyIndicator();
                    })
                ,
                this.Filtering.SelectedCompanyId,
                this.Filtering.SelectedWarehouseId,
                this.Filtering.FromDate,
                this.Filtering.ToDate,
                (byte)this.Filtering.SelectedTransactionType);
        }

        //================================================================================

        private void voucherTransaction()
        {
            bool isValid = Filtering.Validate();
            if (!isValid || Filtering.SelectedCompanyId == 0)
                return;

            this.ShowBusyIndicator("در حال ایجاد سند ...");

            this.transactionServiceWrapper.CreateVoucherForTransactions(
                (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                    () =>
                    {
                        if (exception == null)
                        {
                            this.viewTransaction();
                        }
                        else
                        {
                            this.fuelMainController.HandleException(exception);
                        }

                        this.HideBusyIndicator();
                    })
                ,
                this.Filtering.SelectedCompanyId,
                this.Filtering.SelectedWarehouseId,
                this.Filtering.FromDate,
                this.Filtering.ToDate,
                (byte)this.Filtering.SelectedTransactionType);
        }

        //================================================================================

        private void viewInventoryTransactionReference()
        {
            transactionController.ShowReference(this.SelectedTransaction.ReferenceType, this.SelectedTransaction.ReferenceNo, this.SelectedTransaction.Warehouse.CompanyId);
        }

        private void viewInventoryTransactionDetailPricingReference()
        {
            transactionController.ShowReference(this.SelectedTransactionDetailPrice.PricingReferenceType, this.SelectedTransactionDetailPrice.PricingReferenceNumber, this.SelectedTransaction.Warehouse.CompanyId, true);
        }


        //================================================================================
        public void LoadByFilter(string inventoryOperationReferenceCode)
        {
            this.Load();

            clearTransactionData();

            TransactionTypeEnum transactionType;
            long warehouseId;
            decimal inventoryCode;
            string warehouseCode;

            extractValues(inventoryOperationReferenceCode, out transactionType, out warehouseId, out inventoryCode, out warehouseCode);

            this.transactionServiceWrapper.GetPagedTransactionDataByFilter(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                                                                           () =>
                                                                                           {
                                                                                               if (exception == null)
                                                                                               {
                                                                                                   this.PagedTransactionData.SetPagedDataCollection(result);
                                                                                                   if (result.Result.Count == 1)
                                                                                                       this.SelectedTransaction = result.Result[0];
                                                                                               }
                                                                                               else
                                                                                               {
                                                                                                   this.fuelMainController.HandleException(exception);
                                                                                               }

                                                                                               this.HideBusyIndicator();
                                                                                           }),
                null,
                warehouseId,
                null,
                null,
                (byte)transactionType,
                null,
                inventoryCode,
                this.PagedTransactionData.PageSize,
                this.PagedTransactionData.PageIndex);

        }

        private void extractValues(string referenceCode, out TransactionTypeEnum transactionType, out long warehouseId, out decimal inventoryCode, out string warehouseCode)
        {
            var parts = referenceCode.Split('|').Last().Split('/').ToList();

            Enum.TryParse(parts[0], true, out transactionType);
            long.TryParse(parts[1], out warehouseId);
            decimal.TryParse(parts[2], out inventoryCode);
            warehouseCode = parts[3];
        }

        public void LoadByFilter(long inventoryTransactionId)
        {
            this.Load();

            clearTransactionData();

            this.transactionServiceWrapper.GetById(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                                                                           () =>
                                                                                           {
                                                                                               if (exception == null)
                                                                                               {
                                                                                                   this.PagedTransactionData.Add(result);
                                                                                                   this.SelectedTransaction = result;
                                                                                               }
                                                                                               else
                                                                                               {
                                                                                                   this.fuelMainController.HandleException(exception);
                                                                                               }

                                                                                               this.HideBusyIndicator();
                                                                                           }),
                inventoryTransactionId);

        }
    }
}
