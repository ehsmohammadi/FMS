using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
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
    public class VesselListVM : WorkspaceViewModel, IEventHandler<VesselListChangeArg>
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IVesselController vesselController;
        private readonly IVesselServiceWrapper vesselServiceWrapper;
        private readonly IVesselInCompanyServiceWrapper vesselInCompanyServiceWrapper;
        private readonly ICompanyServiceWrapper companyServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string INVALID_RECORD_MESSAGE = "رکورد مورد نظر یافت نشد.";
        private const string READONLY_RECORD_MESSAGE = "رکورد مورد نظر قابل حذف و یا ویرایش نمی باشد.";
        private const string SEARCH_COMMAND_TEXT = "جستجو";
        private const string CLEAR_SEARCH_COMMAND_TEXT = "سعی مجدد";
        private const string APPROVE_COMMAND_TEXT = "تأیید";
        private const string DISAPPROVE_COMMAND_TEXT = "لغو تأیید";
        private const string ADD_HEADER_COMMAND_TEXT = "افزودن";
        private const string ACTIVATE_HEADER_COMMAND_TEXT = "فعالسازی";

        //================================================================================

        private VesselListFilteringVM filtering;
        public VesselListFilteringVM Filtering
        {
            get { return filtering; }
            set { this.SetField(p => p.Filtering, ref filtering, value); }
        }

        private PagedSortableCollectionView<VesselDto> pagedVesselData;
        public PagedSortableCollectionView<VesselDto> PagedVesselData
        {
            get { return pagedVesselData; }
            set { this.SetField(p => p.PagedVesselData, ref pagedVesselData, value); }
        }

        private VesselDto _selectedVessel;
        public VesselDto SelectedVessel
        {
            get { return _selectedVessel; }
            set { this.SetField(p => p.SelectedVessel, ref _selectedVessel, value); }
        }

        private PagedSortableCollectionView<VesselInCompanyDto> pagedVesselInCompanyData;
        public PagedSortableCollectionView<VesselInCompanyDto> PagedVesselInCompanyData
        {
            get { return pagedVesselInCompanyData; }
            set { this.SetField(p => p.PagedVesselInCompanyData, ref pagedVesselInCompanyData, value); }
        }

        #region Activation Info section

        public string ActivationDateToDisplay
        {
            get
            {
                if (SelectedVessel == null)
                {
                    return string.Empty;
                }
                else
                {
                    if (VesselActivation != null)
                    {
                        return VesselActivation.ActivationDate.ToString();
                    }
                    else
                    {
                        return "غیر فعال";
                    }
                }
            }

        }

        private VesselActivationDto vesselActivation;
        public VesselActivationDto VesselActivation
        {
            get { return this.vesselActivation; }
            set { this.SetField(p => p.VesselActivation, ref this.vesselActivation, value); }
        }

        #endregion

        //================================================================================

        private CommandViewModel searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                if (searchCommand == null)
                    searchCommand = new CommandViewModel(SEARCH_COMMAND_TEXT, new DelegateCommand(this.searchVessels));

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

        private CommandViewModel addVesselCommand;
        public CommandViewModel AddVesselCommand
        {
            get
            {
                if (addVesselCommand == null)
                    addVesselCommand = new CommandViewModel(ADD_HEADER_COMMAND_TEXT, new DelegateCommand(this.addVessel));

                return addVesselCommand;
            }
        }

        private CommandViewModel activateVesselCommand;
        public CommandViewModel ActivateVesselCommand
        {
            get
            {
                if (activateVesselCommand == null)
                    activateVesselCommand = new CommandViewModel(ACTIVATE_HEADER_COMMAND_TEXT, new DelegateCommand(this.activateVessel));

                return activateVesselCommand;
            }
        }

        //================================================================================

        public VesselListVM()
        {
            this.DisplayName = "شناورها";

            this.PagedVesselData = new PagedSortableCollectionView<VesselDto>();
            this.PagedVesselData.PageChanged += PagedVesselDtos_PageChanged;

            this.PagedVesselInCompanyData = new PagedSortableCollectionView<VesselInCompanyDto>();
            this.PagedVesselInCompanyData.PageChanged += PagedVesselInCompanyDtos_PageChanged;

            this.PropertyChanged += VesselListVM_PropertyChanged;
        }

        public VesselListVM(
            IFuelController fuelMainController,
            IVesselController vesselController,
            IVesselServiceWrapper vesselServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper,
            IInventoryOperationServiceWrapper inventoryOperationServiceWrapper,
            VesselListFilteringVM filtering, IVesselInCompanyServiceWrapper vesselInCompanyServiceWrapper)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.vesselController = vesselController;
            this.vesselServiceWrapper = vesselServiceWrapper;
            this.companyServiceWrapper = companyServiceWrapper;
            this.vesselInCompanyServiceWrapper = vesselInCompanyServiceWrapper;
            this.Filtering = filtering;
            this.Filtering.PropertyChanged += Filtering_PropertyChanged;
        }

        //================================================================================

        #region Event Handlers

        //================================================================================

        void PagedVesselDtos_PageChanged(object sender, EventArgs e)
        {
            this.searchVessels();
        }

        void PagedVesselInCompanyDtos_PageChanged(object sender, EventArgs e)
        {
            // this.searchVessels();
        }

        //================================================================================

        void Filtering_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.clearVesselData();
            this.clearVesselInCompanayData();
        }

        //================================================================================

        public void Handle(VesselListChangeArg eventData)
        {
            this.searchVessels();
        }

        //================================================================================


        void VesselListVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.SelectedVessel))
            {
                if (SelectedVessel != null)
                {
                    LoadActivationInfo();
                    LoadVesselInCompanies(10, 0);
                }

                this.OnPropertyChanged(this.GetPropertyName(p => p.ActivationDateToDisplay));
            }

            if (e.PropertyName == this.GetPropertyName(p => p.VesselActivation))
            {
                this.OnPropertyChanged(this.GetPropertyName(p => p.ActivationDateToDisplay));
            }
        }

        #endregion

        //================================================================================

        protected override void OnRequestClose()
        {
            this.fuelMainController.Close(this);
        }

        //================================================================================

        public void Load(int pageSize = 10, int pageIndex = 0)
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.companyServiceWrapper.GetAll((result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    this.Filtering.Initialize(result);

                                    if (this.Filtering.SelectedOwner != null)
                                        this.vesselServiceWrapper.GetPagedVesselDataByFilter(
                                            (vesselResult, vesselException) => this.fuelMainController.BeginInvokeOnDispatcher(
                                                () =>
                                                {
                                                    if (vesselException == null)
                                                    {
                                                        if (vesselResult != null)
                                                        {
                                                            PagedVesselData.Clear();
                                                            PagedVesselData.SourceCollection = vesselResult.Result;
                                                            PagedVesselData.TotalItemCount = vesselResult.TotalCount;
                                                            PagedVesselData.PageIndex = Math.Max(0, vesselResult.CurrentPage - 1);
                                                            PagedVesselData.PageSize = vesselResult.PageSize;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        this.fuelMainController.HandleException(vesselException);
                                                    }
                                            
                                                    this.HideBusyIndicator();
                                                }), this.Filtering.SelectedOwner.Id, pageSize, pageIndex);

                                }
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                                this.HideBusyIndicator();
                            }
                        }), true);
        }

        //================================================================================

        private void LoadActivationInfo()
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            VesselActivation = null;
            this.vesselInCompanyServiceWrapper.GetActivationInfo((result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                VesselActivation = result;
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }), SelectedVessel.Code);

        }
        
        //================================================================================

        private void LoadVesselInCompanies(int pageSize = 10, int pageIndex = 0)
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);
            this.vesselInCompanyServiceWrapper.GetPagedDataByFilter((result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                                                                                                                           () =>
                                                                                                                                           {
                                                                                                                                               if (exception == null)
                                                                                                                                               {
                                                                                                                                                   if (result != null)
                                                                                                                                                   {
                                                                                                                                                       this.PagedVesselInCompanyData.Clear();
                                                                                                                                                       this.PagedVesselInCompanyData.SourceCollection = result.Result;
                                                                                                                                                       this.PagedVesselInCompanyData.TotalItemCount = result.TotalCount;
                                                                                                                                                       this.PagedVesselInCompanyData.PageIndex = Math.Max(0, result.CurrentPage - 1);
                                                                                                                                                       this.PagedVesselInCompanyData.PageSize = result.PageSize;
                                                                                                                                                   }
                                                                                                                                               }
                                                                                                                                               else
                                                                                                                                               {
                                                                                                                                                   this.fuelMainController.HandleException(exception);
                                                                                                                                               }
                                                                                                                                               this.HideBusyIndicator();
                                                                                                                                           }), this.SelectedVessel.Code, pageSize, pageIndex, false);

        }

        private void searchVessels()
        {
            clearVesselData();

            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.vesselServiceWrapper.GetPagedVesselDataByFilter(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                            () =>
                            {
                                if (exception == null)
                                {
                                    this.PagedVesselData.SetPagedDataCollection(result);
                                }
                                else
                                {
                                    this.fuelMainController.HandleException(exception);
                                }

                                this.HideBusyIndicator();
                            })
                ,
                this.Filtering.SelectedOwnerId,
                this.PagedVesselData.PageSize,
                this.PagedVesselData.PageIndex);
        }

        //================================================================================

        private void clearVesselData()
        {
            this.PagedVesselData.Clear();
            this.SelectedVessel = null;
            this.VesselActivation = null;
        }

        private void clearVesselInCompanayData()
        {
            this.PagedVesselInCompanyData.Clear();
        }

        //================================================================================

        private bool isVesselSelected()
        {
            if (SelectedVessel == null)
            {
                this.fuelMainController.ShowMessage("از لیست Vessel ها ردیف مورد نظر را انتخاب نمایید.");
                return false;
            }

            return true;
        }

        //================================================================================

        private void addVessel()
        {
            this.vesselController.Add();
        }

        private void activateVessel()
        {
            if (isVesselSelected())
            {
                this.vesselController.ActivateVessel(SelectedVessel);
            }
        }

    }



}

