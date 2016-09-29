using System;
using System.Collections.Generic;
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
    public class ScrapListVM : WorkspaceViewModel, IEventHandler<ScrapListChangedArg>, IEventHandler<ScrapDetailListChangedArg>
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IScrapController scrapController;
        private readonly IScrapServiceWrapper scrapServiceWrapper;
        private readonly IApprovalFlowServiceWrapper approvalFlowServiceWrapper;
        private readonly ICompanyServiceWrapper companyServiceWrapper;
        private readonly IInventoryOperationServiceWrapper inventoryOperationServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string INVALID_RECORD_MESSAGE = "رکورد مورد نظر یافت نشد.";
        private const string READONLY_RECORD_MESSAGE = "رکورد مورد نظر قابل حذف و یا ویرایش نمی باشد.";
        private const string SEARCH_COMMAND_TEXT = "جستجو";
        private const string CLEAR_SEARCH_COMMAND_TEXT = "سعی مجدد";
        private const string APPROVE_COMMAND_TEXT = "تأیید";
        private const string DISAPPROVE_COMMAND_TEXT = "لغو تأیید";
        private const string EDIT_HEADER_COMMAND_TEXT = "ویرایش";
        private const string ADD_HEADER_COMMAND_TEXT = "افزودن";
        private const string DELETE_HEADER_COMMAND_TEXT = "حذف";
        private const string EDIT_DETAIL_COMMAND_TEXT = "ویرایش";
        private const string ADD_DETAIL_COMMAND_TEXT = "افزودن";
        private const string DELETE_DETAIL_COMMAND_TEXT = "حذف";
        private const string DELETE_SCRAP_QUESTION_TEXT = "مورد نظر اطمینان دارید؟ Scrap آیا از حذف";
        //================================================================================

        private ScrapListFilteringVM filtering;
        public ScrapListFilteringVM Filtering
        {
            get { return filtering; }
            set { this.SetField(p => p.Filtering, ref filtering, value); }
        }

        private PagedSortableCollectionView<ScrapDto> pagedScrapData;
        public PagedSortableCollectionView<ScrapDto> PagedScrapData
        {
            get { return pagedScrapData; }
            set { this.SetField(p => p.PagedScrapData, ref pagedScrapData, value); }
        }

        private PagedSortableCollectionView<ScrapDetailDto> pagedScrapDetailData;
        public PagedSortableCollectionView<ScrapDetailDto> PagedScrapDetailData
        {
            get { return pagedScrapDetailData; }
            set { this.SetField(p => p.PagedScrapDetailData, ref pagedScrapDetailData, value); }
        }

        private ScrapDto selectedScrap;
        public ScrapDto SelectedScrap
        {
            get { return selectedScrap; }
            set { this.SetField(p => p.SelectedScrap, ref selectedScrap, value); }
        }

        private ScrapDetailDto selectedScrapDetail;
        public ScrapDetailDto SelectedScrapDetail
        {
            get { return this.selectedScrapDetail; }
            set { this.SetField(p => p.SelectedScrapDetail, ref this.selectedScrapDetail, value); }
        }

        private PagedSortableCollectionView<FuelReportInventoryOperationDto> pagedInventoryOperationData;
        public PagedSortableCollectionView<FuelReportInventoryOperationDto> PagedInventoryOperationData
        {
            get { return this.pagedInventoryOperationData; }
            set { this.pagedInventoryOperationData = value; }
        }

        //================================================================================

        private CommandViewModel searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                if (searchCommand == null)
                    searchCommand = new CommandViewModel(SEARCH_COMMAND_TEXT, new DelegateCommand(this.searchScraps));

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

        private CommandViewModel approveCommand;
        public CommandViewModel ApproveCommand
        {
            get
            {
                if (approveCommand == null)
                    approveCommand = new CommandViewModel(APPROVE_COMMAND_TEXT, new DelegateCommand(this.approveScrap));

                return approveCommand;
            }
        }

        private CommandViewModel rejectCommand;
        public CommandViewModel RejectCommand
        {
            get
            {
                if (rejectCommand == null)
                    rejectCommand = new CommandViewModel(DISAPPROVE_COMMAND_TEXT, new DelegateCommand(this.rejectScrap));

                return rejectCommand;
            }
        }

        private CommandViewModel editScrapCommand;
        public CommandViewModel EditScrapCommand
        {
            get
            {
                if (editScrapCommand == null)
                    editScrapCommand = new CommandViewModel(EDIT_HEADER_COMMAND_TEXT, new DelegateCommand(this.editScrap));

                return editScrapCommand;
            }
        }

        private CommandViewModel addScrapCommand;
        public CommandViewModel AddScrapCommand
        {
            get
            {
                if (addScrapCommand == null)
                    addScrapCommand = new CommandViewModel(ADD_HEADER_COMMAND_TEXT, new DelegateCommand(this.addScrap));

                return addScrapCommand;
            }
        }


        private CommandViewModel deleteScrapCommand;
        public CommandViewModel DeleteScrapCommand
        {
            get
            {
                if (deleteScrapCommand == null)
                    deleteScrapCommand = new CommandViewModel(DELETE_HEADER_COMMAND_TEXT, new DelegateCommand(this.deleteScrap));

                return deleteScrapCommand;
            }
        }


        private CommandViewModel addScrapDetailCommand;
        public CommandViewModel AddScrapDetailCommand
        {
            get
            {
                if (addScrapDetailCommand == null)
                    addScrapDetailCommand = new CommandViewModel(ADD_DETAIL_COMMAND_TEXT, new DelegateCommand(this.addScrapDetail));

                return addScrapDetailCommand;
            }
        }


        private CommandViewModel editScrapDetailCommand;
        public CommandViewModel EditScrapDetailCommand
        {
            get
            {
                if (editScrapDetailCommand == null)
                    editScrapDetailCommand = new CommandViewModel(EDIT_DETAIL_COMMAND_TEXT, new DelegateCommand(this.editScrapDetail));

                return editScrapDetailCommand;
            }
        }

        private CommandViewModel deleteScrapDetailCommand;
        public CommandViewModel DeleteScrapDetailCommand
        {
            get
            {
                if (deleteScrapDetailCommand == null)
                    deleteScrapDetailCommand = new CommandViewModel(DELETE_DETAIL_COMMAND_TEXT, new DelegateCommand(this.deleteScrapDetail));

                return deleteScrapDetailCommand;
            }
        }

        //================================================================================

        public ScrapListVM()
        {
            this.DisplayName = "Scrap";

            this.PagedScrapData = new PagedSortableCollectionView<ScrapDto>();
            this.PagedScrapData.PageChanged += PagedScrapDtos_PageChanged;

            this.PagedScrapDetailData = new PagedSortableCollectionView<ScrapDetailDto>();
            this.PagedScrapDetailData.PageChanged += PagedScrapDetailData_PageChanged;

            this.PropertyChanged += ScrapListVM_PropertyChanged;

            this.PagedInventoryOperationData = new PagedSortableCollectionView<FuelReportInventoryOperationDto>();
            this.pagedInventoryOperationData.PageChanged += pagedInventoryOperationData_PageChanged;
        }

        public ScrapListVM(
            IFuelController fuelMainController,
            IScrapController scrapController,
            IScrapServiceWrapper scrapServiceWrapper,
            IApprovalFlowServiceWrapper approvalFlowServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper,
            IInventoryOperationServiceWrapper inventoryOperationServiceWrapper,
            ScrapListFilteringVM filtering)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.scrapController = scrapController;
            this.scrapServiceWrapper = scrapServiceWrapper;
            this.approvalFlowServiceWrapper = approvalFlowServiceWrapper;
            this.companyServiceWrapper = companyServiceWrapper;
            this.inventoryOperationServiceWrapper = inventoryOperationServiceWrapper;

            this.Filtering = filtering;
            this.Filtering.PropertyChanged += Filtering_PropertyChanged;
        }

        //================================================================================

        #region Event Handlers

        //================================================================================

        void PagedScrapDtos_PageChanged(object sender, EventArgs e)
        {
            this.searchScraps();
        }

        //================================================================================

        void PagedScrapDetailData_PageChanged(object sender, EventArgs e)
        {
            this.loadScrapDetails();
        }

        //================================================================================

        void Filtering_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.clearScrapData();
        }

        //================================================================================

        public void Handle(ScrapListChangedArg eventData)
        {
            this.searchScraps();
        }

        //================================================================================

        public void Handle(ScrapDetailListChangedArg eventData)
        {
            this.loadScrapDetails();
        }

        //================================================================================


        void ScrapListVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.SelectedScrap))
            {
                this.loadScrapDetails();
                this.loadInventoryOperations();
            }

            //if (e.PropertyName == this.GetPropertyName(p => p.SelectedScrapDetail))
            //this.loadInventoryOperations();
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
                        }), true);
        }

        //================================================================================

        private void searchScraps()
        {
            clearScrapData();

            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.scrapServiceWrapper.GetPagedScrapDataByFilter(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                if (exception == null)
                                {
                                    this.PagedScrapData.SetPagedDataCollection(result);
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }

                                this.HideBusyIndicator();
                            })
                ,
                this.Filtering.SelectedCompanyId,
                this.Filtering.FromDate,
                this.Filtering.ToDate,
                this.PagedScrapData.PageSize,
                this.PagedScrapData.PageIndex);
        }

        //================================================================================

        private void clearScrapData()
        {
            this.PagedScrapData.Clear();
            this.SelectedScrap = null;
        }

        //================================================================================

        private void clearScrapDetailData()
        {
            this.PagedScrapDetailData.Clear();
            this.SelectedScrapDetail = null;
        }

        //================================================================================

        private void clearInventoryOperationData()
        {
            this.PagedInventoryOperationData.Clear();
        }

        //================================================================================

        private void approveScrap()
        {
            if (isScrapSelected())
            {
                if (this.fuelMainController.ShowConfirmationBox("آیا از تأیید اطلاعات اطمینان دارید؟", "تأیید Scrap"))
                {
                    this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

                    this.approvalFlowServiceWrapper.ActApproveFlow(
                        (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                () =>
                                {
                                    this.HideBusyIndicator();
                                    if (exception == null)
                                    {
                                        this.searchScraps();
                                    }
                                    else
                                    {
                                        this.fuelMainController.HandleException(exception);
                                    }
                                }), SelectedScrap.Id, ActionEntityTypeEnum.Scrap);
                }
            }
        }

        //================================================================================

        private void rejectScrap()
        {
            if (isScrapSelected())
            {
                if (this.fuelMainController.ShowConfirmationBox("آیا از لغو تأیید اطلاعات اطمینان دارید؟", "لغو تأیید نمودن Scrap"))
                {
                    this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

                    this.approvalFlowServiceWrapper.ActRejectFlow(
                        (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                () =>
                                {
                                    this.HideBusyIndicator();
                                    if (exception == null)
                                    {
                                        this.searchScraps();
                                    }
                                    else
                                    {
                                        this.fuelMainController.HandleException(exception);
                                    }
                                }), SelectedScrap.Id, ActionEntityTypeEnum.Scrap);

                }
            }
        }

        //================================================================================

        private bool isScrapSelected()
        {
            if (SelectedScrap == null)
            {
                this.fuelMainController.ShowMessage("از لیست Scrap ها ردیف مورد نظر را انتخاب نمایید.");
                return false;
            }

            return true;
        }

        //================================================================================

        private bool isScrapDetailSelected()
        {
            if (SelectedScrapDetail == null)
            {
                this.fuelMainController.ShowMessage("از لیست جزئیات ردیف مورد نظر را انتخاب نمایید");
                return false;
            }

            return true;
        }

        //================================================================================

        private void editScrap()
        {
            if (isScrapSelected())
            {
                this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

                this.scrapServiceWrapper.GetById(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                this.HideBusyIndicator();
                                if (exception == null)
                                {
                                    if (result != null)
                                    {
                                        if (result.IsScrapEditPermitted)
                                            this.scrapController.Edit(result);
                                        else
                                            this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
                                    }
                                    else
                                        this.fuelMainController.ShowMessage(INVALID_RECORD_MESSAGE);
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }
                            }), SelectedScrap.Id);
            }
        }

        //================================================================================

        private void addScrap()
        {
            this.scrapController.Add();
        }

        //================================================================================

        private void deleteScrap()
        {
            if (isScrapSelected())
                if (SelectedScrap.IsScrapDeletePermitted)
                {
                    if (this.fuelMainController.ShowConfirmationBox(DELETE_SCRAP_QUESTION_TEXT, "حذف Scrap"))
                    {
                        this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

                        this.scrapServiceWrapper.Delete(
                            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                () =>
                                {
                                    this.HideBusyIndicator();

                                    if (exception == null)
                                    {
                                        this.fuelMainController.Publish(new ScrapListChangedArg());
                                    }
                                    else
                                    {
                                        this.fuelMainController.HandleException(exception);
                                    }
                                }), SelectedScrap.Id);

                    }
                }
                else
                    this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        }

        //================================================================================

        private void loadScrapDetails()
        {
            this.clearScrapDetailData();

            if (SelectedScrap == null)
                return;
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.scrapServiceWrapper.GetPagedScrapDetailData
                (
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher
                        (
                            () =>
                            {
                                this.HideBusyIndicator();
                                if (exception == null)
                                {
                                    this.PagedScrapDetailData.SetPagedDataCollection(result);
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }
                            }), this.SelectedScrap.Id, this.PagedScrapDetailData.PageSize, this.PagedScrapDetailData.PageIndex);
        }
        //================================================================================

        private void editScrapDetail()
        {
            if (isScrapSelected() && isScrapDetailSelected())
            {
                this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

                this.scrapServiceWrapper.GetScrapDetail(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(() =>
                        {
                            this.HideBusyIndicator();

                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    if (result.Scrap.IsScrapEditDetailPermitted)
                                        this.scrapController.EditScrapDetail(result.Scrap, result);
                                    else
                                        this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
                                }
                                else
                                    this.fuelMainController.ShowMessage(INVALID_RECORD_MESSAGE);
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                        }), this.SelectedScrap.Id, this.SelectedScrapDetail.Id);
            }
        }

        //================================================================================

        private void addScrapDetail()
        {
            if (isScrapSelected())
            {
                this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

                this.scrapServiceWrapper.GetById(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                this.HideBusyIndicator();

                                if (exception == null)
                                {
                                    if (result != null)
                                    {
                                        if (result.IsScrapAddDetailPermitted)
                                            this.scrapController.AddScrapDetail(result);
                                        else
                                            this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
                                    }
                                    else
                                        this.fuelMainController.ShowMessage(INVALID_RECORD_MESSAGE);
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }
                            }), SelectedScrap.Id);
            }
        }

        //================================================================================

        private void deleteScrapDetail()
        {
            if (isScrapSelected() && isScrapDetailSelected())
                if (this.SelectedScrap.IsScrapDeleteDetailPermitted)
                {
                    if (this.fuelMainController.ShowConfirmationBox("آیا از حذف رکورد مورد نظر اطمینان دارید؟", "حذف جزئیات"))
                    {
                        this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

                        this.scrapServiceWrapper.DeleteScrapDetail(
                                (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(() =>
                                {
                                    this.HideBusyIndicator();

                                    if (exception == null)
                                    {
                                        this.fuelMainController.Publish(new ScrapDetailListChangedArg());
                                    }
                                    else
                                    {
                                        this.fuelMainController.HandleException(exception);
                                    }
                                }), this.SelectedScrap.Id, this.SelectedScrapDetail.Id);
                    }
                }
                else
                    this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        }

        //================================================================================

        private void loadInventoryOperations()
        {
            clearInventoryOperationData();

            if (this.SelectedScrap != null)
            {
                ShowBusyIndicator("در حال دریافت اطلاعات حواله و رسیدها");

                this.inventoryOperationServiceWrapper.GetScrapInventoryOperations((result, exception) =>
                    this.fuelMainController.BeginInvokeOnDispatcher(() =>
                    {
                        if (exception == null)
                        {
                            this.PagedInventoryOperationData.SetPagedDataCollection(result);
                        }
                        else
                        {
                            this.fuelMainController.HandleException(exception);
                        }
                        HideBusyIndicator();
                    }), SelectedScrap.Id);
            }
        }

        //================================================================================


        //================================================================================
    }



}

