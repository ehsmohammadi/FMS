using System;
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
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels
{
    public class VoyageListVM : WorkspaceViewModel, IEventHandler<VoyageListChangeArg>
    {
        #region props

        private IFuelController fuelManagementController;
        private IVoyageController voyageController;

        private ICompanyServiceWrapper companyServiceWrapper;
        //private IFuelReportCompanyServiceWrapper companyServiceWrapper;        //private IFuelReportCompanyServiceWrapper companyServiceWrapper;
        private IVoyageServiceWrapper voyageServiceWrapper;

        private PagedSortableCollectionView<VoyageDto> voyagePagedData;
        public PagedSortableCollectionView<VoyageDto> VoyagePagedData
        {
            get { return this.voyagePagedData; }
            set { this.SetField(p => p.VoyagePagedData, ref this.voyagePagedData, value); }
        }

        private VoyageDto selectedVoyage;
        public VoyageDto SelectedVoyage
        {
            get { return this.selectedVoyage; }
            set
            {
                if (this.selectedVoyage != value) this.clearLogList();

                this.SetField(p => p.SelectedVoyage, ref this.selectedVoyage, value);
            }
        }

        private PagedSortableCollectionView<VoyageLogDto> voyageLogPagedData;
        public PagedSortableCollectionView<VoyageLogDto> VoyageLogPagedData
        {
            get { return this.voyageLogPagedData; }
            set { this.SetField(p => p.VoyageLogPagedData, ref this.voyageLogPagedData, value); }
        }

        private CommandViewModel showChangeHistoryCommand;
        public CommandViewModel ShowChangeHistoryCommand
        {
            get
            {
                if (this.showChangeHistoryCommand == null)
                {
                    this.showChangeHistoryCommand = new CommandViewModel("تاریخچه تغییرات", new DelegateCommand(
                                                                          () =>
                                                                          {
                                                                              if (!this.checkIsSelected())
                                                                              {
                                                                                  return;
                                                                              }

                                                                              ShowLog();
                                                                          }
                                                                          ));
                }
                return this.showChangeHistoryCommand;
            }
        }

        private CommandViewModel searchCommand;
        public CommandViewModel SearchCommand
        {
            get
            {
                if (this.searchCommand == null)
                {
                    this.searchCommand = new CommandViewModel("جستجو", new DelegateCommand(this.LoadVoyagesList));
                }
                return this.searchCommand;
            }
        }

        private CommandViewModel updateVoyagesFromRotationDataCommand;
        public CommandViewModel UpdateVoyagesFromRotationDataCommand
        {
            get
            {
                if (this.updateVoyagesFromRotationDataCommand == null)
                {
                    this.updateVoyagesFromRotationDataCommand = new CommandViewModel("به روز رسانی سفرها", new DelegateCommand(this.UpdateVoyagesFromRotationData));
                }
                return this.updateVoyagesFromRotationDataCommand;
            }
        }

        private CompanyDto selectedCompanyDto;
        public CompanyDto SelectedCompanyDto
        {
            get { return this.selectedCompanyDto; }
            set { this.SetField(d => d.SelectedCompanyDto, ref this.selectedCompanyDto, value); }
        }

        private ObservableCollection<CompanyDto> companyDtosCollection;
        public ObservableCollection<CompanyDto> CompanyDtosCollection
        {
            get { return this.companyDtosCollection; }
            set { this.SetField(p => p.CompanyDtosCollection, ref this.companyDtosCollection, value); }
        }

        private VesselInCompanyDto selectedVesselInCompanyDto;
        public VesselInCompanyDto SelectedVesselInCompanyDto
        {
            get { return this.selectedVesselInCompanyDto; }
            set { this.SetField(p => p.SelectedVesselInCompanyDto, ref this.selectedVesselInCompanyDto, value); }
        }

        #endregion

        #region ctor

        public VoyageListVM()
        {
        }

        public VoyageListVM(
            IVoyageController voyageController,
            IFuelController fuelManagementController,
            IVoyageServiceWrapper voyageServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper)
            : this()
        {
            init(voyageController, fuelManagementController, voyageServiceWrapper, companyServiceWrapper);
        }

        private void init(
            IVoyageController voyageController,
            IFuelController fuelManagementController,
            IVoyageServiceWrapper voyageServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper)
        {

            this.voyageController = voyageController;
            this.voyageServiceWrapper = voyageServiceWrapper;
            this.fuelManagementController = fuelManagementController;
            this.companyServiceWrapper = companyServiceWrapper;

            this.DisplayName = "گزارش سفرها";
            this.VoyagePagedData = new PagedSortableCollectionView<VoyageDto>();

            this.VoyagePagedData.PageChanged += VoyagePagedData_PageChanged;

            this.CompanyDtosCollection = new ObservableCollection<CompanyDto>();

            this.VoyageLogPagedData = new PagedSortableCollectionView<VoyageLogDto>();

            this.VoyageLogPagedData.PageChanged += VoyageLogPagedData_PageChanged;
        }

        #endregion

        #region methods

        private bool checkIsSelected()
        {
            if (this.SelectedVoyage == null)
            {
                this.fuelManagementController.ShowMessage("لطفا ردیف سفر مورد نظر را انتخاب نمایید");
                return false;
            }
            return true;
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            this.fuelManagementController.Close(this);
        }

        #region loading data

        public void Load()
        {
            this.ShowBusyIndicator("درحال دریافت اطلاعات ....");

            this.companyServiceWrapper.GetAll(companyGetAllCallback, true, true);
            //this.companyServiceWrapper.GetAll(companyGetAllCallback, this.fuelManagementController.GetCurrentUser().Id);
        }

        private void companyGetAllCallback(System.Collections.Generic.List<CompanyDto> companyDtos, Exception caughtException)
        {
            this.fuelManagementController.BeginInvokeOnDispatcher(() =>
                {
                    this.HideBusyIndicator();
                    if (caughtException == null)
                    {
                        this.CompanyDtosCollection.Clear();

                        foreach (CompanyDto dto in companyDtos)
                        {
                            this.CompanyDtosCollection.Add(dto);
                        }

                        this.SelectedCompanyDto = this.CompanyDtosCollection.FirstOrDefault();

                        if (this.SelectedCompanyDto != null)
                            this.SelectedVesselInCompanyDto = this.SelectedCompanyDto.VesselInCompanies.FirstOrDefault();
                    }
                    else
                    {
                        this.fuelManagementController.HandleException(caughtException);
                    }
                });
        }

        void VoyagePagedData_PageChanged(object sender, EventArgs e)
        {
            LoadVoyagesList();
        }

        private void LoadVoyagesList()
        {
            if (this.SelectedCompanyDto == null ||
                this.SelectedVesselInCompanyDto == null)
            {
                return;
            }

            this.ShowBusyIndicator("درحال دريافت اطلاعات ............");
            this.voyageServiceWrapper.GetByFilter(
                                            voyageGetByFilterCallback,
                                            this.SelectedCompanyDto.Id,
                                            this.SelectedVesselInCompanyDto.Id,
                                            this.VoyagePagedData.PageSize,
                                            this.VoyagePagedData.PageIndex);
        }

        private void UpdateVoyagesFromRotationData()
        {
            this.ShowBusyIndicator("درحال دريافت اطلاعات ............");
            this.voyageServiceWrapper.UpdateVoyagesFromRotationData(
                (res, exp) => this.fuelManagementController.BeginInvokeOnDispatcher(() =>{
                    this.HideBusyIndicator();
                    
                    if (exp != null)
                    {
                        this.fuelManagementController.HandleException(exp);
                    }
                    else
                    {
                        LoadVoyagesList();
                    }
                })
                );
        }

        private void voyageGetByFilterCallback(PageResultDto<VoyageDto> res, Exception exp)
        {
            this.fuelManagementController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();
                if (exp == null)
                {
                    this.VoyagePagedData.SourceCollection = res.Result;
                    this.VoyagePagedData.ItemCount = res.TotalCount;
                }
                else
                {
                    this.fuelManagementController.HandleException(exp);
                }
            });
        }

        void VoyageLogPagedData_PageChanged(object sender, EventArgs e)
        {
            ShowLog();
        }

        public void ShowLog()
        {
            this.ShowBusyIndicator("درحال دریافت اطلاعات ....");

            if (this.SelectedVoyage == null)
            {
                this.HideBusyIndicator();

                this.fuelManagementController.ShowMessage("از لیست سفرها یک ردیف انتخاب گردد.");

                return;
            }

            this.voyageServiceWrapper.GetChenageHistory(this.getChenageHistoryCallback,
                this.SelectedVoyage.Id, this.VoyageLogPagedData.PageSize, this.VoyageLogPagedData.PageIndex);
        }

        private void getChenageHistoryCallback(PageResultDto<VoyageLogDto> pageResult, Exception caughtException)
        {
            this.fuelManagementController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();
                if (caughtException == null)
                {
                    this.VoyageLogPagedData.SourceCollection = pageResult.Result;
                    this.VoyageLogPagedData.ItemCount = pageResult.TotalCount;
                }
                else
                {
                    this.fuelManagementController.HandleException(caughtException);
                }
            });
        }

        private void clearLogList()
        {
            this.VoyageLogPagedData.Clear();
            this.VoyageLogPagedData.ItemCount = 0;
        }

        #endregion

        #endregion

        public void Handle(VoyageListChangeArg eventData)
        {
            this.LoadVoyagesList();
        }
    }
}
