using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels
{
    public class FuelReportListVM : WorkspaceViewModel, IEventHandler<FuelReportListChangeArg>, IEventHandler<FuelReportChangedArg>
    {

        #region props

        private CompanyDto companiesFilterSelected;
        private VesselInCompanyDto vesselFilterSelected;
        private IApprovalFlowServiceWrapper approcalServiceWrapper;
        private CommandViewModel approveCommand;
        private IFuelReportController controller;
        private PagedSortableCollectionView<FuelReportDto> data;
        private CommandViewModel deleteCommand;
        private CommandViewModel editCommand;
        private IFuelController mainController;
        private CommandViewModel rejectCommand;
        private CommandViewModel revertConsumptionCommand;
        private CommandViewModel revertBatchCommand;
        private CommandViewModel approveBatchCommand;
        private CommandViewModel searchCommand;
        private CommandViewModel refreshVoyagesCommand;
        private CommandViewModel viewOriginalReportCommand;
        private FuelReportDto selected;
        private VesselEventReportViewDto currentVesselEventReportViewDto;
        private IFuelReportServiceWrapper serviceWrapper;
        private IResolver<FuelReportVM> FuelReportVMResolver { get; set; }
        private ICompanyServiceWrapper CompanyServiceWrapper { get; set; }
        //private IInventoryOperationServiceWrapper InventoryOperationServiceWrapper { get; set; }


        public PagedSortableCollectionView<FuelReportDto> Data
        {
            get { return this.data; }
            set { this.SetField(p => p.Data, ref this.data, value); }
        }


        public FuelReportDto Selected
        {
            get { return this.selected; }
            set
            {
                this.SetField(p => p.Selected, ref this.selected, value);
            }
        }

        public CommandViewModel EditCommand
        {
            get
            {
                if (this.editCommand == null)
                {
                    this.editCommand = new CommandViewModel("ویرایش", new DelegateCommand(
                                                                          () =>
                                                                          {
                                                                              if (!this.checkIsSelected())
                                                                              {
                                                                                  return;
                                                                              }
                                                                              this.controller.Edit(this.Selected);
                                                                          }
                                                                          ));
                }
                return this.editCommand;
            }
        }


        public CommandViewModel DeleteCommand
        {
            get
            {
                if (this.deleteCommand == null)
                {
                    this.deleteCommand = new CommandViewModel("حذف",
                                                              new DelegateCommand(() =>
                                                                                  {
                                                                                      if (!this.checkIsSelected())
                                                                                      {
                                                                                          return;
                                                                                      }

                                                                                      if (MessageBox.Show("آیا مطمئن هستید ؟", "Delete Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                                                                                      {
                                                                                          this.HideBusyIndicator();
                                                                                          return;
                                                                                      }

                                                                                      this.ShowBusyIndicator();
                                                                                      this.serviceWrapper.Delete(
                                                                                                (res, exp) =>
                                                                                                    this.mainController.
                                                                                                         BeginInvokeOnDispatcher(
                                                                                                                                 () =>
                                                                                                                                 {
                                                                                                                                     this.HideBusyIndicator
                                                                                                                                         ();
                                                                                                                                     if (exp != null)
                                                                                                                                     {
                                                                                                                                         this.mainController.HandleException(exp);
                                                                                                                                     }
                                                                                                                                     else
                                                                                                                                     {
                                                                                                                                         this.LoadFuelReportsByFilters();
                                                                                                                                     }
                                                                                                                                 }), this.Selected.Id);
                                                                                  }));
                }
                return this.deleteCommand;
            }
        }

        public CommandViewModel ApproveCommand
        {
            get
            {
                string caption = "تأیید";

                if (this.approveCommand == null)
                {
                    this.approveCommand = new CommandViewModel(caption, new DelegateCommand(executeApproveCommandMethod));
                }

                return this.approveCommand;
            }
        }

        private void executeApproveCommandMethod()
        {
            if (!this.checkIsSelected())
            {
                return;
            }

            if (this.Selected.ApproveStatus != WorkflowStageEnum.Initial && MessageBox.Show("آیا مطمئن هستید ؟", "Approve Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                this.HideBusyIndicator();
                return;
            }

            this.ShowBusyIndicator();

            this.approcalServiceWrapper.ActApproveFlow((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();
                if (exp != null)
                {
                    this.mainController.HandleException(exp);
                }
                else
                {
                    this.LoadFuelReportsByFilters();
                }
            }), this.Selected.Id, ActionEntityTypeEnum.FuelReport);
        }

        public CommandViewModel RejectCommand
        {
            get
            {
                if (this.rejectCommand == null)
                {
                    this.rejectCommand = new CommandViewModel("لغو تأیید",
                                                              new DelegateCommand(() =>
                                                                                  {
                                                                                      if (!this.checkIsSelected())
                                                                                      {
                                                                                          return;
                                                                                      }
                                                                                      this.ShowBusyIndicator();
                                                                                      if (MessageBox.Show("آیا مطمئن هستید ؟", "Reject Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                                                                                      {
                                                                                          this.HideBusyIndicator();
                                                                                          return;
                                                                                      }
                                                                                      this.approcalServiceWrapper.ActRejectFlow(
                                                                                                                                (res, exp) =>
                                                                                                                                    this.mainController.
                                                                                                                                         BeginInvokeOnDispatcher(
                                                                                                                                                                 () =>
                                                                                                                                                                 {
                                                                                                                                                                     this.HideBusyIndicator
                                                                                                                                                                         ();
                                                                                                                                                                     if (exp != null)
                                                                                                                                                                     {
                                                                                                                                                                         this.mainController
                                                                                                                                                                             .
                                                                                                                                                                              HandleException
                                                                                                                                                                             (exp);
                                                                                                                                                                     }
                                                                                                                                                                     else
                                                                                                                                                                     {
                                                                                                                                                                         this.LoadFuelReportsByFilters();
                                                                                                                                                                     }
                                                                                                                                                                 }),
                                                                                                                                this.Selected.Id, ActionEntityTypeEnum.FuelReport);
                                                                                  }));
                }
                return this.rejectCommand;
            }
        }

        public CommandViewModel RevertConsumptionCommand
        {
            get
            {
                if (this.revertConsumptionCommand == null)
                {
                    this.revertConsumptionCommand = new CommandViewModel("برگشت مصرف",
                                                              new DelegateCommand(revertConsumption));
                }
                return this.revertConsumptionCommand;
            }
        }

        private void revertConsumption()
        {
            if (!this.checkIsSelected())
            {
                return;
            }
            this.ShowBusyIndicator();
            if (MessageBox.Show("آیا مطمئن هستید ؟", "Reject Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                this.HideBusyIndicator();
                return;
            }
            this.serviceWrapper.RevertFuelReportInventoryOperations((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();
                if (exp != null)
                {
                    this.mainController.HandleException(exp);
                }
                else
                {
                    this.LoadFuelReportsByFilters();
                }
            }), this.Selected.Id);
        }

        public CommandViewModel RevertBatchCommand
        {
            get
            {
                if (this.revertBatchCommand == null)
                {
                    this.revertBatchCommand = new CommandViewModel("لغو تأیید کلی",
                                                              new DelegateCommand(revertBatch));
                }
                return this.revertBatchCommand;
            }
        }

        public CommandViewModel ApproveBatchCommand
        {
            get
            {
                if (this.approveBatchCommand == null)
                {
                    this.approveBatchCommand = new CommandViewModel("تأیید نهایی کلی",
                                                              new DelegateCommand(approveBatch));
                }
                return this.approveBatchCommand;
            }
        }
        private void revertBatch()
        {
            if (!this.checkIsSelected())
            {
                return;
            }
            this.ShowBusyIndicator();
            if (MessageBox.Show("آیا مطمئن هستید ؟\nتوجه:\nکلیه رکورد ها پس از رکورد انتخاب شده، از تأیید نهایی برگشت داده می شوند.", "Batch Reject Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                this.HideBusyIndicator();
                return;
            }
            this.approcalServiceWrapper.RejectAllFuelReportsFromReportId((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();
                if (exp != null)
                {
                    this.mainController.HandleException(exp);
                }
                else
                {
                    this.LoadFuelReportsByFilters();
                }
            }), this.Selected.Id);
        }

        private void approveBatch()
        {
            if (!this.checkIsSelected())
            {
                return;
            }
            this.ShowBusyIndicator();

            if (MessageBox.Show("آیا مطمئن هستید ؟\nتوجه:\nکلیه رکورد ها پس از رکورد انتخاب شده، تأیید نهایی می گردند.", "Batch Approve Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                this.HideBusyIndicator();
                return;
            }
            this.approcalServiceWrapper.ApproveAllFuelReportsFromReportId((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();
                if (exp != null)
                {
                    this.mainController.HandleException(exp);
                }
                else
                {
                    this.LoadFuelReportsByFilters();
                }
            }), this.Selected.Id);
        }

        public CommandViewModel SearchCommand
        {
            get
            {
                if (this.searchCommand == null)
                {
                    this.searchCommand = new CommandViewModel("جستجو", new DelegateCommand(
                                                                               () => { this.LoadFuelReportsByFilters(); }
                                                                               ));
                }
                return this.searchCommand;
            }
        }

        public CommandViewModel ViewOriginalReportCommand
        {
            get
            {
                if (this.viewOriginalReportCommand == null)
                {
                    this.viewOriginalReportCommand = new CommandViewModel("گزارش کشتی", new DelegateCommand(
                        () => controller.ShowOriginalVesselReport(this.Selected.Code)));
                }

                return this.viewOriginalReportCommand;
            }
        }

        public CommandViewModel RefreshVoyagesCommand
        {
            get
            {
                if (this.refreshVoyagesCommand == null)
                {
                    this.refreshVoyagesCommand = new CommandViewModel("مقداردهی سفرها", new DelegateCommand(
                                                                               this.refreshFuelReportsVoyage
                                                                               ));
                }
                return this.refreshVoyagesCommand;
            }
        }

        public FuelReportDetailListVM FuelReportDetailListVm { get; set; }

        public CompanyDto CompaniesFilterSelected
        {
            get { return this.companiesFilterSelected; }
            set { this.SetField(d => d.CompaniesFilterSelected, ref this.companiesFilterSelected, value); }
        }

        public ObservableCollection<CompanyDto> CompaniesFilter { get; set; }

        public VesselInCompanyDto VesselFilterSelected
        {
            get { return this.vesselFilterSelected; }
            set { this.SetField(p => p.VesselFilterSelected, ref this.vesselFilterSelected, value); }
        }

        private string fuelReportIdFilterValue;
        public string FuelReportIdFilterValue
        {
            get { return this.fuelReportIdFilterValue; }
            set { this.SetField(d => d.FuelReportIdFilterValue, ref this.fuelReportIdFilterValue, value); }
        }

        private string fuelReportDetailIdFilterValue;
        public string FuelReportDetailIdFilterValue
        {
            get { return this.fuelReportDetailIdFilterValue; }
            set { this.SetField(d => d.FuelReportDetailIdFilterValue, ref this.fuelReportDetailIdFilterValue, value); }
        }

        #region inline editing

        public EnumVM<FuelReportTypeEnum> FuelReportTypesVM { get; private set; }

        private string vesselReportCode;
        public string VesselReportCode
        {
            get { return this.vesselReportCode; }
            set { this.SetField(v => v.VesselReportCode, ref vesselReportCode, value); }
        }

        private DateTime? fromDateFilter;
        public DateTime? FromDateFilter
        {
            get { return fromDateFilter; }
            set { this.SetField(v => v.FromDateFilter, ref fromDateFilter, value); }
        }

        private DateTime? toDateFilter;

        public DateTime? ToDateFilter
        {
            get { return toDateFilter; }
            set { this.SetField(v => v.ToDateFilter, ref toDateFilter, value); }
        }


        #endregion

        #endregion

        #region ctor

        public FuelReportListVM()
        {
            this.Data = new PagedSortableCollectionView<FuelReportDto>();
            this.Data.PageChanged += Data_PageChanged;

            this.PropertyChanged += FuelReportListVM_PropertyChanged;
        }

        public FuelReportListVM(IFuelReportController controller,
                                IFuelController mainController,
                                IFuelReportServiceWrapper serviceWrapper,
                                ICompanyServiceWrapper companyServiceWrapper,
            //IInventoryOperationServiceWrapper inventoryOperationServiceWrapper,
                                IApprovalFlowServiceWrapper approcalServiceWrapper,
                                IResolver<FuelReportVM> fuelReportVMResolver,
                                FuelReportDetailListVM fuelReportDetailListVm,
                                IGoodServiceWrapper goodServiceWrapper,
                                IInventoryTransactionController inventoryTransactionController
            )
            : this()
        {
            this.Init(controller, mainController, serviceWrapper, companyServiceWrapper,
                      fuelReportVMResolver,
                //inventoryOperationServiceWrapper, 
                      fuelReportDetailListVm, approcalServiceWrapper);
        }

        #endregion

        #region methods

        public void Handle(FuelReportListChangeArg eventData)
        {
            this.LoadFuelReportsByFilters();
        }

        void Data_PageChanged(object sender, EventArgs e)
        {
            this.LoadFuelReportsByFilters();
        }

        private void Init(IFuelReportController controller, IFuelController mainController,
                          IFuelReportServiceWrapper serviceWrapper,
                          ICompanyServiceWrapper companyServiceWrapper,
                          IResolver<FuelReportVM> fuelReportVmResolver,
            //IInventoryOperationServiceWrapper inventoryOperationServiceWrapper,
                          FuelReportDetailListVM fuelReportDetailListVm,
                          IApprovalFlowServiceWrapper approcalServiceWrapper)
        {
            this.controller = controller;
            this.serviceWrapper = serviceWrapper;
            this.mainController = mainController;
            this.CompanyServiceWrapper = companyServiceWrapper;

            //this.InventoryOperationServiceWrapper = inventoryOperationServiceWrapper;
            this.CompaniesFilter = new ObservableCollection<CompanyDto>();
            this.FuelReportVMResolver = fuelReportVmResolver;
            this.approcalServiceWrapper = approcalServiceWrapper;
            this.DisplayName = "گزارش سوخت";

            //inline editing 
            this.FuelReportDetailListVm = fuelReportDetailListVm;
        }

        private FuelReportVM CreateItem(FuelReportDto dto)
        {
            FuelReportVM result = this.FuelReportVMResolver.Resolve();
            result.SetMainController(this.mainController);
            result.SetServiceWrapper(this.serviceWrapper);
            result.SetEntity(dto);

            return result;
        }

        private bool checkIsSelected()
        {
            if (this.Selected == null)
            {
                this.mainController.ShowMessage("لطفا گزارش مورد نظر را انتخاب نمایید");
                return false;
            }

            return true;
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();

            this.mainController.Close(this);
        }

        #region loading data

        public void Load()
        {
            this.ShowBusyIndicator("درحال دریافت اطلاعات ....");
            this.CompanyServiceWrapper.GetAll(
                    (res, exp) =>
                    {
                        this.mainController.BeginInvokeOnDispatcher(() =>
                                {
                                    this.HideBusyIndicator();
                                    if (exp == null)
                                    {
                                        this.CompaniesFilter.Clear();

                                        foreach (CompanyDto dto in res)
                                        {
                                            dto.VesselInCompanies.Insert(0, FilteringUtils.EmptyVesselDto);

                                            this.CompaniesFilter.Add(dto);
                                        }

                                        this.CompaniesFilterSelected = this.CompaniesFilter.FirstOrDefault();
                                        this.VesselFilterSelected = this.CompaniesFilterSelected.VesselInCompanies.FirstOrDefault();
                                    }
                                    else
                                    {
                                        this.mainController.HandleException(exp);
                                    }
                                });
                    }, true, true);
        }

        private void LoadFuelReportsByFilters()
        {
            if (string.IsNullOrWhiteSpace(FuelReportIdFilterValue) && string.IsNullOrWhiteSpace(FuelReportDetailIdFilterValue) &&
                (this.CompaniesFilterSelected == null || this.CompaniesFilterSelected == FilteringUtils.EmptyCompanyDto ||
                    this.VesselFilterSelected == null || this.VesselFilterSelected == FilteringUtils.EmptyVesselDto) &&
                    string.IsNullOrEmpty(VesselReportCode))
            {
                this.mainController.ShowMessage("لطفاً کشتی و یا سریالهای  مورد نظر را در فیلدهای جستجو، وارد نمایید");
                return;
            }

            this.ShowBusyIndicator("درحال دريافت اطلاعات ............");
            this.serviceWrapper.GetByFilter(
                        (res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
                                            {
                                                if (exp == null)
                                                {
                                                    var selectedId = this.Selected == null ? null : (long?)this.Selected.Id;

                                                    this.Data.SourceCollection = res.Result;
                                                    this.Data.TotalItemCount = res.TotalCount;

                                                    if (this.Data.SourceCollection.Count() == 1)
                                                        this.Selected = this.Data.SourceCollection.FirstOrDefault();
                                                    else
                                                    {
                                                        if (selectedId.HasValue)
                                                        {
                                                            var selectedRecord = this.Data.SourceCollection.SingleOrDefault(d => d.Id == selectedId.Value);
                                                            if (selectedRecord == null && this.Selected != null)
                                                                selectedRecord = this.Data.SourceCollection.Where(er => er.EventDate >= this.Selected.EventDate).OrderBy(er => er.EventDate).FirstOrDefault();

                                                            this.Selected = selectedRecord;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    this.mainController.HandleException(exp);
                                                }

                                                if (Data.PageIndex > Data.PageCount - 1)
                                                {
                                                    Data.PageIndex = Data.PageCount - 1;
                                                }

                                                this.HideBusyIndicator();
                                            }),
                                (this.CompaniesFilterSelected == null || this.CompaniesFilterSelected == FilteringUtils.EmptyCompanyDto) ? null : (long?)this.CompaniesFilterSelected.Id,
                                (this.VesselFilterSelected == null || this.VesselFilterSelected == FilteringUtils.EmptyVesselDto) ? null : (long?)this.VesselFilterSelected.Id,
                                VesselReportCode,
                                FromDateFilter,
                                ToDateFilter,
                                this.FuelReportIdFilterValue,
                                this.FuelReportDetailIdFilterValue,
                                Data.PageSize, Data.PageIndex);
        }

        #endregion

        private void FuelReportListVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.Selected))
            {
                this.FuelReportDetailListVm.FuelReportVMSelected = this.Selected == null ? null : this.CreateItem(this.Selected);
            }
        }

        private void refreshFuelReportsVoyage()
        {
            if (this.CompaniesFilterSelected == null || this.CompaniesFilterSelected == FilteringUtils.EmptyCompanyDto)
            {
                this.mainController.ShowMessage("شرکت و در صورت نیاز کشتی مورد نظر را انتخاب نمایید");

                return;
            }

            this.ShowBusyIndicator("در حال به روز رسانی سفرها");

            this.serviceWrapper.RefreshFuelReportsVoyage((res, exp) =>
                                                             this.mainController.BeginInvokeOnDispatcher(() =>
                                                             {
                                                                 this.HideBusyIndicator();

                                                                 if (exp != null)
                                                                 {
                                                                     this.mainController.HandleException(exp);
                                                                 }
                                                                 else
                                                                 {
                                                                     this.mainController.Publish(new FuelReportListChangeArg());
                                                                 }
                                                             }), this.CompaniesFilterSelected.Id,
                                                             (this.VesselFilterSelected == null || this.VesselFilterSelected == FilteringUtils.EmptyVesselDto) ? null : (long?)this.VesselFilterSelected.Id);
        }

        #endregion

        public void LoadByFilter(string fuelReportId, string fuelReportDetailId)
        {
            Load();

            this.CompaniesFilterSelected = FilteringUtils.EmptyCompanyDto;
            this.VesselFilterSelected = FilteringUtils.EmptyVesselDto;
            this.VesselReportCode = null;
            this.FromDateFilter = null;
            this.ToDateFilter = null;
            this.FuelReportIdFilterValue = fuelReportId;
            this.FuelReportDetailIdFilterValue = fuelReportDetailId;

            LoadFuelReportsByFilters();
        }

        public void Handle(FuelReportChangedArg eventData)
        {
            if (eventData.FuelReport != null)
            {
                resetFuelReport(eventData.FuelReport);
            }
            else if (eventData.FuelReportId.HasValue)
            {
                this.serviceWrapper.GetById((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
                {
                    if (exp == null)
                    {
                        resetFuelReport(res);
                    }
                    else
                    {
                        this.mainController.HandleException(exp);
                    }
                }), eventData.FuelReportId.Value);
            }
        }

        private void resetFuelReport(FuelReportDto fuelReportDto)
        {
            var foundReport = this.Data.SourceCollection.FirstOrDefault(fr => fr.Id == fuelReportDto.Id);

            if (foundReport == null) return;

            foundReport.SetValues(fuelReportDto);

            if (this.Selected != null && this.Selected.Id == foundReport.Id)
            {
                this.Selected = foundReport;
                this.OnPropertyChanged(this.GetPropertyName(p => p.Selected));
            }
        }
    }
}
