using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class OffhireManagementSystemListVM : WorkspaceViewModel, IEventHandler<OffhireListChangedArg>, IEventHandler<OffhireDetailListChangedArg>
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IOffhireController offhireController;
        private readonly IOffhireServiceWrapper offhireServiceWrapper;
        private readonly IVesselInCompanyServiceWrapper vesselServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string SEARCH_COMMAND_TEXT = "جستجو";
        private const string CLEAR_SEARCH_COMMAND_TEXT = "سعی مجدد";
        //private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        //private const string INVALID_RECORD_MESSAGE = "رکورد مورد نظر یافت نشد.";
        //private const string READONLY_RECORD_MESSAGE = "رکورد مورد نظر قابل حذف و یا ویرایش نمی باشد.";
        //private const string APPROVE_COMMAND_TEXT = "تأیید";
        //private const string DISAPPROVE_COMMAND_TEXT = "لغو تأیید";
        //private const string EDIT_HEADER_COMMAND_TEXT = "ویرایش";
        //private const string DELETE_HEADER_COMMAND_TEXT = "حذف";
        //private const string EDIT_DETAIL_COMMAND_TEXT = "ویرایش";
        //private const string ADD_DETAIL_COMMAND_TEXT = "افزودن";
        //private const string DELETE_DETAIL_COMMAND_TEXT = "حذف";
        //private const string SELECT_FROM_DETAIL_LIST_MESSAGE = "از لیست جزئیات ردیف مورد نظر را انتخاب نمایید";
        //private const string DELETE_SCRAP_QUESTION_TEXT = "مورد نظر اطمینان دارید؟ Offhire آیا از حذف";
        private const string IMPORT_COMMAND_TEXT = "شروع عملیات";
        private const string CANCEL_COMMAND_TEXT = "خروج";
        private const string SELECT_FROM_HEADER_LIST_MESSAGE = ".ها ردیف مورد نظر را انتخاب نمایید Offhire از لیست";

        //================================================================================

        private OffhireListFilteringVM filtering;
        public OffhireListFilteringVM Filtering
        {
            get { return filtering; }
            set { this.SetField(p => p.Filtering, ref filtering, value); }
        }

        private PagedSortableCollectionView<OffhireManagementSystemDto> pagedOffhireData;
        public PagedSortableCollectionView<OffhireManagementSystemDto> PagedOffhireData
        {
            get { return pagedOffhireData; }
            set { this.SetField(p => p.PagedOffhireData, ref pagedOffhireData, value); }
        }

        private ObservableCollection<OffhireManagementSystemDetailDto> offhireDetailData;
        public ObservableCollection<OffhireManagementSystemDetailDto> OffhireDetailData
        {
            get { return offhireDetailData; }
            set { this.SetField(p => p.OffhireDetailData, ref offhireDetailData, value); }
        }

        private OffhireManagementSystemDto selectedOffhire;
        public OffhireManagementSystemDto SelectedOffhire
        {
            get { return selectedOffhire; }
            set { this.SetField(p => p.SelectedOffhire, ref selectedOffhire, value); }
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

        private CommandViewModel importCommand;
        public CommandViewModel ImportCommand
        {
            get
            {
                if (importCommand == null)
                    importCommand = new CommandViewModel(IMPORT_COMMAND_TEXT, new DelegateCommand(this.importOffhire));

                return importCommand;
            }
        }

        private CommandViewModel cancelCommand;
        public CommandViewModel CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new CommandViewModel(CANCEL_COMMAND_TEXT, new DelegateCommand(OnRequestClose));

                return cancelCommand;
            }
        }


        //================================================================================

        public OffhireManagementSystemListVM()
        {
            this.DisplayName = "سیستم Offhire";

            this.PagedOffhireData = new PagedSortableCollectionView<OffhireManagementSystemDto>();
            this.PagedOffhireData.PageChanged += PagedOffhireDtos_PageChanged;

            this.OffhireDetailData = new ObservableCollection<OffhireManagementSystemDetailDto>();

            this.PropertyChanged += OffhireListVM_PropertyChanged;
        }

        public OffhireManagementSystemListVM(
            IFuelController fuelMainController,
            IOffhireController offhireController,
            IOffhireServiceWrapper offhireServiceWrapper,
            IVesselInCompanyServiceWrapper vesselServiceWrapper,
            OffhireListFilteringVM filtering)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.offhireController = offhireController;
            this.offhireServiceWrapper = offhireServiceWrapper;
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

        void Filtering_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        void OffhireListVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

            //this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);
            var busyId = this.fuelMainController.ShowBusyIndicatorEx(FETCH_DATA_BUSY_MESSAGE);


            this.offhireServiceWrapper.GetOffhireManagementSystemPagedData(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                                                                           () =>
                                                                                           {
                                                                                               if (exception == null)
                                                                                               {
                                                                                                   this.setPagedDataCollection(this.PagedOffhireData, result);
                                                                                               }
                                                                                               else
                                                                                               {
                                                                                                   this.fuelMainController.HandleException(exception);
                                                                                               }

                                                                                               //this.HideBusyIndicator();
                                                                                               fuelMainController.HideBusyIndicatorEx(busyId);
                                                                                           }),
                    this.fuelMainController.GetCurrentUser().User.CompanyDto.Id,
                    this.Filtering.SelectedVesselId.Value,
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
            if (this.OffhireDetailData != null)
                this.OffhireDetailData.Clear();
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

        private void importOffhire()
        {
            if (isOffhireSelected())
                this.offhireController.Import(this.SelectedOffhire.ReferenceNumber);
        }

        //================================================================================

        private void loadOffhireDetails()
        {
            this.clearOffhireDetailData();

            if (this.SelectedOffhire != null)
            {
                this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

                this.OffhireDetailData = new ObservableCollection<OffhireManagementSystemDetailDto>(this.SelectedOffhire.OffhireDetails);

                this.HideBusyIndicator();
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

