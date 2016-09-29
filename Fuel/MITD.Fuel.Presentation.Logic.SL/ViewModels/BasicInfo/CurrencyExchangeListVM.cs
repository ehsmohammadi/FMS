using System;
using System.ComponentModel;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class CurrencyExchangeListVM : WorkspaceViewModel
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly ICurrencyController currencyController;
        private readonly ICurrencyServiceWrapper currencyServiceWrapper;
        private readonly IFiscalYearServiceWrapper fiscalYearServiceWrapper;
        private readonly ICompanyServiceWrapper companyServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string INVALID_RECORD_MESSAGE = "رکورد مورد نظر یافت نشد.";
        private const string READONLY_RECORD_MESSAGE = "رکورد مورد نظر قابل حذف و یا ویرایش نمی باشد.";
        private const string SEARCH_COMMAND_TEXT = "جستجو";
        private const string CLEAR_SEARCH_COMMAND_TEXT = "سعی مجدد";
        private const string UPDATE_COMMAND_TEXT = "به روز رسانی";
        private const string DISAPPROVE_COMMAND_TEXT = "لغو تأیید";
        private const string EDIT_HEADER_COMMAND_TEXT = "ویرایش";
        private const string ADD_HEADER_COMMAND_TEXT = "افزودن";
        private const string DELETE_HEADER_COMMAND_TEXT = "حذف";
        private const string EDIT_DETAIL_COMMAND_TEXT = "ویرایش";
        private const string ADD_DETAIL_COMMAND_TEXT = "افزودن";
        private const string DELETE_DETAIL_COMMAND_TEXT = "حذف";
        private const string DELETE_SCRAP_QUESTION_TEXT = "مورد نظر اطمینان دارید؟ Scrap آیا از حذف";
        //================================================================================

        private CurrencyExchangeListFilteringVM filtering;
        public CurrencyExchangeListFilteringVM Filtering
        {
            get { return filtering; }
            set { this.SetField(p => p.Filtering, ref filtering, value); }
        }

        private PagedSortableCollectionView<CurrencyExchangeDto> pagedCurrencyExchangeData;
        public PagedSortableCollectionView<CurrencyExchangeDto> PagedCurrencyExchangeData
        {
            get { return pagedCurrencyExchangeData; }
            set { this.SetField(p => p.PagedCurrencyExchangeData, ref pagedCurrencyExchangeData, value); }
        }

        private CurrencyExchangeDto selectedCurrencyExchange;
        public CurrencyExchangeDto SelectedCurrencyExchange
        {
            get { return selectedCurrencyExchange; }
            set { this.SetField(p => p.SelectedCurrencyExchange, ref selectedCurrencyExchange, value); }
        }


        //================================================================================

        private CommandViewModel searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                if (searchCommand == null)
                    searchCommand = new CommandViewModel(SEARCH_COMMAND_TEXT, new DelegateCommand(this.searchCurrencyExchanges));

                return searchCommand;
            }
        }

        private CommandViewModel clearSearchCommand;
        public CommandViewModel ClearSearchCommand
        {
            get
            {
                if (clearSearchCommand == null)
                    clearSearchCommand = new CommandViewModel(CLEAR_SEARCH_COMMAND_TEXT, new DelegateCommand(this.filtering.ResetToDefaults));

                return clearSearchCommand;
            }
        }


        private CommandViewModel updateCommand;
        public CommandViewModel UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                    updateCommand = new CommandViewModel(UPDATE_COMMAND_TEXT, new DelegateCommand(this.updateCurrencyData));

                return updateCommand;
            }
        }

        //private CommandViewModel approveCommand;
        //public CommandViewModel ApproveCommand
        //{
        //    get
        //    {
        //        if (approveCommand == null)
        //            approveCommand = new CommandViewModel(APPROVE_COMMAND_TEXT, new DelegateCommand(this.approveScrap));

        //        return approveCommand;
        //    }
        //}

        //private CommandViewModel rejectCommand;
        //public CommandViewModel RejectCommand
        //{
        //    get
        //    {
        //        if (rejectCommand == null)
        //            rejectCommand = new CommandViewModel(DISAPPROVE_COMMAND_TEXT, new DelegateCommand(this.rejectScrap));

        //        return rejectCommand;
        //    }
        //}

        //private CommandViewModel editScrapCommand;
        //public CommandViewModel EditScrapCommand
        //{
        //    get
        //    {
        //        if (editScrapCommand == null)
        //            editScrapCommand = new CommandViewModel(EDIT_HEADER_COMMAND_TEXT, new DelegateCommand(this.editScrap));

        //        return editScrapCommand;
        //    }
        //}

        //private CommandViewModel addScrapCommand;
        //public CommandViewModel AddScrapCommand
        //{
        //    get
        //    {
        //        if (addScrapCommand == null)
        //            addScrapCommand = new CommandViewModel(ADD_HEADER_COMMAND_TEXT, new DelegateCommand(this.addScrap));

        //        return addScrapCommand;
        //    }
        //}

        //================================================================================

        public CurrencyExchangeListVM()
        {
            this.DisplayName = "نرخ تبدیل ارزها";

            this.PagedCurrencyExchangeData = new PagedSortableCollectionView<CurrencyExchangeDto>();
            this.PagedCurrencyExchangeData.PageChanged += PagedScrapDtos_PageChanged;

            this.PropertyChanged += CurrencyExchangeListVM_PropertyChanged;
        }

        public CurrencyExchangeListVM(
            IFuelController fuelMainController,
            ICurrencyController currencyController,
            ICurrencyServiceWrapper currencyServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper,
            CurrencyExchangeListFilteringVM filtering, IFiscalYearServiceWrapper fiscalYearServiceWrapper)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.currencyController = currencyController;
            this.currencyServiceWrapper = currencyServiceWrapper;
            this.companyServiceWrapper = companyServiceWrapper;
            this.fiscalYearServiceWrapper = fiscalYearServiceWrapper;

            this.Filtering = filtering;
            this.Filtering.PropertyChanged += Filtering_PropertyChanged;
        }

        //================================================================================

        #region Event Handlers

        //================================================================================

        void PagedScrapDtos_PageChanged(object sender, EventArgs e)
        {
            this.searchCurrencyExchanges();
        }

        //================================================================================

        //void PagedScrapDetailData_PageChanged(object sender, EventArgs e)
        //{
        //    this.loadScrapDetails();
        //}

        //================================================================================

        void Filtering_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.clearCurrencyExchangeData();
        }

        //================================================================================

        public void Handle(ScrapListChangedArg eventData)
        {
            this.searchCurrencyExchanges();
        }

        //================================================================================

        void CurrencyExchangeListVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == this.GetPropertyName(p => p.SelectedScrap))
            //{
            //    this.loadScrapDetails();
            //    this.loadInventoryOperations();
            //}
        }

        //================================================================================

        void pagedInventoryOperationData_PageChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //================================================================================

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

            this.currencyServiceWrapper.GetAllCurrency(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    this.Filtering.SetFromCurrencies(result);
                                    this.Filtering.SetToCurrencies(result);
                                }
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }));

            this.fiscalYearServiceWrapper.GetFiscalYears(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    this.filtering.SetFiscalYears(result);
                                }
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }));
        }

        //================================================================================

        private void searchCurrencyExchanges()
        {
            clearCurrencyExchangeData();

            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.currencyServiceWrapper.GetExchangeRates(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                if (exception == null)
                                {
                                    this.PagedCurrencyExchangeData.SetPagedDataCollection(result);
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }

                                this.HideBusyIndicator();
                            })
                ,
                this.Filtering.SelectedFromCurrencyId,
                this.Filtering.SelectedToCurrencyId,
                this.Filtering.SelectedFiscalYearNumber,
                this.PagedCurrencyExchangeData.PageSize,
                this.PagedCurrencyExchangeData.PageIndex);
        }

        //================================================================================

        private void updateCurrencyData()
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.currencyServiceWrapper.UpdateCurrencies(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                this.HideBusyIndicator();

                                if (exception == null)
                                {
                                    Load();
                                    searchCurrencyExchanges();
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }
                            }));
        }

        //================================================================================

        private void clearCurrencyExchangeData()
        {
            this.PagedCurrencyExchangeData.Clear();
            this.SelectedCurrencyExchange = null;
        }

        //================================================================================

        //private void clearScrapDetailData()
        //{
        //    this.PagedScrapDetailData.Clear();
        //    this.SelectedScrapDetail = null;
        //}

        //================================================================================

        //private void clearInventoryOperationData()
        //{
        //    this.PagedInventoryOperationData.Clear();
        //}

        //================================================================================

        private bool isScrapSelected()
        {
            if (SelectedCurrencyExchange == null)
            {
                this.fuelMainController.ShowMessage("از لیست تبدیل نرخ ارزها ردیف مورد نظر را انتخاب نمایید.");
                return false;
            }

            return true;
        }

        //================================================================================

        //private bool isScrapDetailSelected()
        //{
        //    if (SelectedScrapDetail == null)
        //    {
        //        this.fuelMainController.ShowMessage("از لیست جزئیات ردیف مورد نظر را انتخاب نمایید");
        //        return false;
        //    }

        //    return true;
        //}

        //================================================================================

        //private void editScrap()
        //{
        //    if (isScrapSelected())
        //    {
        //        this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //        this.scrapServiceWrapper.GetById(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
        //                    () =>
        //                    {
        //                        this.HideBusyIndicator();
        //                        if (exception == null)
        //                        {
        //                            if (result != null)
        //                            {
        //                                if (result.IsScrapEditPermitted)
        //                                    this.scrapController.Edit(result);
        //                                else
        //                                    this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        //                            }
        //                            else
        //                                this.fuelMainController.ShowMessage(INVALID_RECORD_MESSAGE);
        //                        }
        //                        else
        //                        {
        //                            this.fuelMainController.HandleException(exception);
        //                        }
        //                    }), SelectedScrap.Id);
        //    }
        //}

        //================================================================================

        //private void addScrap()
        //{
        //    this.scrapController.Add();
        //}

        //================================================================================

        //private void deleteScrap()
        //{
        //    if (isScrapSelected())
        //        if (SelectedScrap.IsScrapDeletePermitted)
        //        {
        //            if (this.fuelMainController.ShowConfirmationBox(DELETE_SCRAP_QUESTION_TEXT, "حذف Scrap"))
        //            {
        //                this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

        //                this.scrapServiceWrapper.Delete(
        //                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
        //                        () =>
        //                        {
        //                            this.HideBusyIndicator();

        //                            if (exception == null)
        //                            {
        //                                this.fuelMainController.Publish(new ScrapListChangedArg());
        //                            }
        //                            else
        //                            {
        //                                this.fuelMainController.HandleException(exception);
        //                            }
        //                        }), SelectedScrap.Id);

        //            }
        //        }
        //        else
        //            this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        //}

        //================================================================================

        //private void loadScrapDetails()
        //{
        //    this.clearScrapDetailData();

        //    if (SelectedScrap == null)
        //        return;
        //    this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //    this.scrapServiceWrapper.GetPagedScrapDetailData
        //        (
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher
        //                (
        //                    () =>
        //                    {
        //                        this.HideBusyIndicator();
        //                        if (exception == null)
        //                        {
        //                            this.PagedScrapDetailData.SetPagedDataCollection(result);
        //                        }
        //                        else
        //                        {
        //                            this.fuelMainController.HandleException(exception);
        //                        }
        //                    }), this.SelectedScrap.Id, this.PagedScrapDetailData.PageSize, this.PagedScrapDetailData.PageIndex);
        //}
        //================================================================================

        //private void editScrapDetail()
        //{
        //    if (isScrapSelected() && isScrapDetailSelected())
        //    {
        //        this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //        this.scrapServiceWrapper.GetScrapDetail(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(() =>
        //                {
        //                    this.HideBusyIndicator();

        //                    if (exception == null)
        //                    {
        //                        if (result != null)
        //                        {
        //                            if (result.Scrap.IsScrapEditDetailPermitted)
        //                                this.scrapController.EditScrapDetail(result.Scrap, result);
        //                            else
        //                                this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        //                        }
        //                        else
        //                            this.fuelMainController.ShowMessage(INVALID_RECORD_MESSAGE);
        //                    }
        //                    else
        //                    {
        //                        this.fuelMainController.HandleException(exception);
        //                    }
        //                }), this.SelectedScrap.Id, this.SelectedScrapDetail.Id);
        //    }
        //}

        ////================================================================================

        //private void addScrapDetail()
        //{
        //    if (isScrapSelected())
        //    {
        //        this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //        this.scrapServiceWrapper.GetById(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
        //                    () =>
        //                    {
        //                        this.HideBusyIndicator();

        //                        if (exception == null)
        //                        {
        //                            if (result != null)
        //                            {
        //                                if (result.IsScrapAddDetailPermitted)
        //                                    this.scrapController.AddScrapDetail(result);
        //                                else
        //                                    this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        //                            }
        //                            else
        //                                this.fuelMainController.ShowMessage(INVALID_RECORD_MESSAGE);
        //                        }
        //                        else
        //                        {
        //                            this.fuelMainController.HandleException(exception);
        //                        }
        //                    }), SelectedScrap.Id);
        //    }
        //}

        ////================================================================================

        //private void deleteScrapDetail()
        //{
        //    if (isScrapSelected() && isScrapDetailSelected())
        //        if (this.SelectedScrap.IsScrapDeleteDetailPermitted)
        //        {
        //            if (this.fuelMainController.ShowConfirmationBox("آیا از حذف رکورد مورد نظر اطمینان دارید؟", "حذف جزئیات"))
        //            {
        //                this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

        //                this.scrapServiceWrapper.DeleteScrapDetail(
        //                        (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(() =>
        //                        {
        //                            this.HideBusyIndicator();

        //                            if (exception == null)
        //                            {
        //                                this.fuelMainController.Publish(new ScrapDetailListChangedArg());
        //                            }
        //                            else
        //                            {
        //                                this.fuelMainController.HandleException(exception);
        //                            }
        //                        }), this.SelectedScrap.Id, this.SelectedScrapDetail.Id);
        //            }
        //        }
        //        else
        //            this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        //}

        ////================================================================================

        //private void loadInventoryOperations()
        //{
        //    clearInventoryOperationData();

        //    if (this.SelectedScrap != null)
        //    {
        //        ShowBusyIndicator("در حال دریافت اطلاعات حواله و رسیدها");

        //        this.inventoryOperationServiceWrapper.GetScrapInventoryOperations((result, exception) =>
        //            this.fuelMainController.BeginInvokeOnDispatcher(() =>
        //            {
        //                if (exception == null)
        //                {
        //                    this.PagedInventoryOperationData.SetPagedDataCollection(result);
        //                }
        //                else
        //                {
        //                    this.fuelMainController.HandleException(exception);
        //                }
        //                HideBusyIndicator();
        //            }), SelectedScrap.Id);
        //    }
        //}

        //================================================================================


        //================================================================================
    }



}

