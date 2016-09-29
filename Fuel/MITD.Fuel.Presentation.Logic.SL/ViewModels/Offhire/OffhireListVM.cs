using System;
using System.Collections.Generic;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class OffhireListVM : WorkspaceViewModel, IEventHandler<OffhireListChangedArg>, IEventHandler<OffhireDetailListChangedArg>
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IOffhireController offhireController;
        private readonly IOffhireServiceWrapper offhireServiceWrapper;
        private readonly IApprovalFlowServiceWrapper approvalFlowServiceWrapper;
        private readonly IVesselInCompanyServiceWrapper vesselServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string INVALID_RECORD_MESSAGE = ".رکورد مورد نظر یافت نشد";
        private const string READONLY_RECORD_MESSAGE = ".رکورد مورد نظر قابل حذف و یا ویرایش نمی باشد";
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
        private const string DELETE_OFFHIRE_QUESTION_TEXT = "مورد نظر اطمینان دارید؟ Offhire آیا از حذف";
        private const string SELECT_FROM_HEADER_LIST_MESSAGE = ".ها ردیف مورد نظر را انتخاب نمایید Offhire از لیست";
        private const string SELECT_FROM_DETAIL_LIST_MESSAGE = ".از لیست جزئیات ردیف مورد نظر را انتخاب نمایید";

        //================================================================================

        private OffhireListFilteringVM filtering;
        public OffhireListFilteringVM Filtering
        {
            get { return filtering; }
            set { this.SetField(p => p.Filtering, ref filtering, value); }
        }

        private PagedSortableCollectionView<OffhireDto> pagedOffhireData;
        public PagedSortableCollectionView<OffhireDto> PagedOffhireData
        {
            get { return pagedOffhireData; }
            set { this.SetField(p => p.PagedOffhireData, ref pagedOffhireData, value); }
        }

        private PagedSortableCollectionView<OffhireDetailDto> pagedOffhireDetailData;
        public PagedSortableCollectionView<OffhireDetailDto> PagedOffhireDetailData
        {
            get { return pagedOffhireDetailData; }
            set { this.SetField(p => p.PagedOffhireDetailData, ref pagedOffhireDetailData, value); }
        }

        private OffhireDto selectedOffhire;
        public OffhireDto SelectedOffhire
        {
            get { return selectedOffhire; }
            set { this.SetField(p => p.SelectedOffhire, ref selectedOffhire, value); }
        }

        private OffhireDetailDto selectedOffhireDetail;
        public OffhireDetailDto SelectedOffhireDetail
        {
            get { return this.selectedOffhireDetail; }
            set { this.SetField(p => p.SelectedOffhireDetail, ref this.selectedOffhireDetail, value); }
        }

        //================================================================================

        private CommandViewModel searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                if (searchCommand == null)
                    searchCommand = new CommandViewModel(SEARCH_COMMAND_TEXT, new DelegateCommand(this.searchOffhires));

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
                    approveCommand = new CommandViewModel(APPROVE_COMMAND_TEXT, new DelegateCommand(this.approveOffhire));

                return approveCommand;
            }
        }

        private CommandViewModel rejectCommand;
        public CommandViewModel RejectCommand
        {
            get
            {
                if (rejectCommand == null)
                    rejectCommand = new CommandViewModel(DISAPPROVE_COMMAND_TEXT, new DelegateCommand(this.rejectOffhire));

                return rejectCommand;
            }
        }

        private CommandViewModel editOffhireCommand;
        public CommandViewModel EditOffhireCommand
        {
            get
            {
                if (editOffhireCommand == null)
                    editOffhireCommand = new CommandViewModel(EDIT_HEADER_COMMAND_TEXT, new DelegateCommand(this.editOffhire));

                return editOffhireCommand;
            }
        }

        private CommandViewModel addOffhireCommand;
        public CommandViewModel AddOffhireCommand
        {
            get
            {
                if (addOffhireCommand == null)
                    addOffhireCommand = new CommandViewModel(ADD_HEADER_COMMAND_TEXT, new DelegateCommand(this.addOffhire));

                return addOffhireCommand;
            }
        }


        private CommandViewModel deleteOffhireCommand;
        public CommandViewModel DeleteOffhireCommand
        {
            get
            {
                if (deleteOffhireCommand == null)
                    deleteOffhireCommand = new CommandViewModel(DELETE_HEADER_COMMAND_TEXT, new DelegateCommand(this.deleteOffhire));

                return deleteOffhireCommand;
            }
        }

        //================================================================================

        public OffhireListVM()
        {
            this.DisplayName = "Offhire";

            this.PagedOffhireData = new PagedSortableCollectionView<OffhireDto>();
            this.PagedOffhireData.PageChanged += PagedOffhireDtos_PageChanged;

            this.PagedOffhireDetailData = new PagedSortableCollectionView<OffhireDetailDto>();
            this.PagedOffhireDetailData.PageChanged += PagedOffhireDetailData_PageChanged;

            this.PropertyChanged += OffhireListVM_PropertyChanged;
        }

        public OffhireListVM(
            IFuelController fuelMainController,
            IOffhireController offhireController,
            IOffhireServiceWrapper offhireServiceWrapper,
            IApprovalFlowServiceWrapper approvalFlowServiceWrapper,
            IVesselInCompanyServiceWrapper vesselServiceWrapper,
            OffhireListFilteringVM filtering)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.offhireController = offhireController;
            this.offhireServiceWrapper = offhireServiceWrapper;
            this.approvalFlowServiceWrapper = approvalFlowServiceWrapper;
            this.vesselServiceWrapper = vesselServiceWrapper;

            this.Filtering = filtering;
            this.Filtering.PropertyChanged += Filtering_PropertyChanged;
        }

        //================================================================================

        #region Event Handlers

        //================================================================================

        void PagedOffhireDtos_PageChanged(object sender, EventArgs e)
        {
            this.searchOffhires();
        }

        //================================================================================

        void PagedOffhireDetailData_PageChanged(object sender, EventArgs e)
        {
            this.loadOffhireDetails();
        }

        //================================================================================

        void Filtering_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.clearOffhireData();
        }

        //================================================================================

        public void Handle(OffhireListChangedArg eventData)
        {
            this.searchOffhires();
        }

        //================================================================================

        public void Handle(OffhireDetailListChangedArg eventData)
        {
            this.loadOffhireDetails();
        }

        //================================================================================

        void OffhireListVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.SelectedOffhire))
                this.loadOffhireDetails();

            //if (e.PropertyName == this.GetPropertyName(p => p.SelectedOffhireDetail))
            //    this.loadInventoryOperations();
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

            this.vesselServiceWrapper.GetPagedDataByFilter(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                                                                           () =>
                                                                                           {
                                                                                               if (exception == null)
                                                                                               {
                                                                                                   if (result != null)
                                                                                                       this.Filtering.Initialize(result.Result);
                                                                                               }
                                                                                               else
                                                                                               {
                                                                                                   this.fuelMainController.HandleException(exception);
                                                                                               }
                                                                                               this.HideBusyIndicator();
                                                                                           }),
                        this.fuelMainController.GetCurrentUser().User.CompanyDto.Id,
                        null,
                        null, true);
        }

        //================================================================================

        private void searchOffhires()
        {
            clearOffhireData();

            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.offhireServiceWrapper.GetPagedOffhireDataByFilter(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                if (exception == null)
                                {
                                    setPagedDataCollection(this.PagedOffhireData, result);
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }

                                this.HideBusyIndicator();
                            })
                ,
                this.fuelMainController.GetCurrentUser().User.CompanyDto.Id,
                this.Filtering.SelectedVesselId,
                this.Filtering.FromDate,
                this.Filtering.ToDate,
                this.PagedOffhireData.PageSize,
                this.PagedOffhireData.PageIndex);
        }

        //================================================================================

        private void clearOffhireData()
        {
            this.PagedOffhireData.Clear();
            this.SelectedOffhire = null;
        }

        //================================================================================

        private void clearOffhireDetailData()
        {
            this.PagedOffhireDetailData.Clear();
            this.SelectedOffhireDetail = null;
        }

        //================================================================================

        private void approveOffhire()
        {
            if (isOffhireSelected())
            {
                if (this.fuelMainController.ShowConfirmationBox("آیا از تأیید اطلاعات اطمینان دارید؟", "تأیید Offhire"))
                {
                    this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

                    this.approvalFlowServiceWrapper.ActApproveFlow(
                        (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                () =>
                                {
                                    this.HideBusyIndicator();
                                    if (exception == null)
                                    {
                                        this.searchOffhires();
                                    }
                                    else
                                    {
                                        this.fuelMainController.HandleException(exception);
                                    }
                                }), SelectedOffhire.Id, ActionEntityTypeEnum.Offhire);
                }
            }
        }

        //================================================================================

        private void rejectOffhire()
        {
            if (isOffhireSelected())
            {
                if (this.fuelMainController.ShowConfirmationBox("آیا از لغو تأیید اطلاعات اطمینان دارید؟", "لغو تأیید نمودن Offhire"))
                {
                    this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

                    this.approvalFlowServiceWrapper.ActRejectFlow(
                        (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                () =>
                                {
                                    this.HideBusyIndicator();
                                    if (exception == null)
                                    {
                                        this.searchOffhires();
                                    }
                                    else
                                    {
                                        this.fuelMainController.HandleException(exception);
                                    }
                                }), SelectedOffhire.Id, ActionEntityTypeEnum.Offhire);

                }
            }
        }

        //================================================================================

        private bool isOffhireSelected()
        {
            if (SelectedOffhire == null)
            {
                this.fuelMainController.ShowMessage(SELECT_FROM_HEADER_LIST_MESSAGE);
                return false;
            }

            return true;
        }

        //================================================================================

        private bool isOffhireDetailSelected()
        {
            if (SelectedOffhireDetail == null)
            {
                this.fuelMainController.ShowMessage(SELECT_FROM_DETAIL_LIST_MESSAGE);
                return false;
            }

            return true;
        }

        //================================================================================

        private void editOffhire()
        {
            if (isOffhireSelected())
            {
                this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

                this.offhireServiceWrapper.GetById(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                this.HideBusyIndicator();
                                if (exception == null)
                                {
                                    if (result != null)
                                    {
                                        if (result.IsOffhireEditPermitted)
                                            this.offhireController.Edit(result);
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
                            }), SelectedOffhire.Id);
            }
        }

        //================================================================================

        private void addOffhire()
        {
            this.offhireController.ShowOffhireManagementSystemList();

            //this.fuelMainController.ShowMessage("Hard Coded!");
        }

        //================================================================================

        private void deleteOffhire()
        {
            if (isOffhireSelected())
                if (SelectedOffhire.IsOffhireDeletePermitted)
                {
                    if (this.fuelMainController.ShowConfirmationBox(DELETE_OFFHIRE_QUESTION_TEXT, "حذف Offhire"))
                    {
                        this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

                        this.offhireServiceWrapper.Delete(
                            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                () =>
                                {
                                    this.HideBusyIndicator();

                                    if (exception == null)
                                    {
                                        this.fuelMainController.Publish(new OffhireListChangedArg());
                                    }
                                    else
                                    {
                                        this.fuelMainController.HandleException(exception);
                                    }
                                }), SelectedOffhire.Id);

                    }
                }
                else
                    this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        }

        //================================================================================

        private void loadOffhireDetails()
        {
            this.clearOffhireDetailData();

            if (this.SelectedOffhire != null)
            {
                this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

                this.offhireServiceWrapper.GetPagedOffhireDetailData(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(() =>
                        {
                            this.HideBusyIndicator();

                            if (exception == null)
                            {
                                setPagedDataCollection(this.PagedOffhireDetailData, result);
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                        }),
                        this.SelectedOffhire.Id,
                        this.PagedOffhireDetailData.PageSize,
                        this.PagedOffhireDetailData.PageIndex);
            }
        }

        //================================================================================

        //private void editOffhireDetail()
        //{
        //    if (isOffhireSelected() && isOffhireDetailSelected())
        //    {
        //        this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //        this.offhireServiceWrapper.GetOffhireDetail(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(() =>
        //                {
        //                    this.HideBusyIndicator();

        //                    if (exception == null)
        //                    {
        //                        if (result != null)
        //                        {
        //                            if (result.Offhire.IsOffhireEditDetailPermitted)
        //                                this.offhireController.EditOffhireDetail(result.Offhire, result);
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
        //                }), this.SelectedOffhire.Id, this.SelectedOffhireDetail.Id);
        //    }
        //}

        //================================================================================

        //private void addOffhireDetail()
        //{
        //    if (isOffhireSelected())
        //    {
        //        this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //        this.offhireServiceWrapper.GetById(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
        //                    () =>
        //                    {
        //                        this.HideBusyIndicator();

        //                        if (exception == null)
        //                        {
        //                            if (result != null)
        //                            {
        //                                if (result.IsOffhireAddDetailPermitted)
        //                                    this.offhireController.AddOffhireDetail(result);
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
        //                    }), SelectedOffhire.Id);
        //    }
        //}

        //================================================================================

        //private void deleteOffhireDetail()
        //{
        //    if (isOffhireSelected() && isOffhireDetailSelected())
        //        if (this.SelectedOffhire.IsOffhireDeleteDetailPermitted)
        //        {
        //            if (this.fuelMainController.ShowConfirmationBox("آیا از حذف رکورد مورد نظر اطمینان دارید؟", "حذف جزئیات"))
        //            {
        //                this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

        //                this.offhireServiceWrapper.DeleteOffhireDetail(
        //                        (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(() =>
        //                        {
        //                            this.HideBusyIndicator();

        //                            if (exception == null)
        //                            {
        //                                this.fuelMainController.Publish(new OffhireDetailListChangedArg());
        //                            }
        //                            else
        //                            {
        //                                this.fuelMainController.HandleException(exception);
        //                            }
        //                        }), this.SelectedOffhire.Id, this.SelectedOffhireDetail.Id);
        //            }
        //        }
        //        else
        //            this.fuelMainController.ShowMessage(READONLY_RECORD_MESSAGE);
        //}

        //================================================================================

        //private void loadInventoryOperations()
        //{
        //    clearInventoryOperationData();

        //    if (this.SelectedOffhire != null && this.SelectedOffhireDetail != null)
        //    {
        //        ShowBusyIndicator("در حال دریافت اطلاعات حواله و رسیدها");

        //        this.inventoryOperationServiceWrapper.GetOffhireInventoryOperations((result, exception) =>
        //            this.fuelMainController.BeginInvokeOnDispatcher(() =>
        //            {
        //                if (exception == null)
        //                {
        //                    setPagedDataCollection(this.PagedInventoryOperationData, result);
        //                }
        //                else
        //                {
        //                    this.fuelMainController.HandleException(exception);
        //                }
        //                HideBusyIndicator();
        //            }), SelectedOffhire.Id, SelectedOffhireDetail.Id);
        //    }
        //}

        //================================================================================

        private void setPagedDataCollection<T>(PagedSortableCollectionView<T> collection, PageResultDto<T> source) where T : class
        {
            collection.Clear();

            if (source != null)
            {
                collection.SourceCollection = source.Result;

                collection.ItemCount = source.TotalCount;
            }
        }

        //================================================================================
    }
}

