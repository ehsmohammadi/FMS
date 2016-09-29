using System.Collections.ObjectModel;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels
{
    public class VoyageVM : WorkspaceViewModel
    {
        #region properties

        private readonly IVoyageServiceWrapper voyageServiceWrapper;
        private ObservableCollection<FuelReportVM> _fuelReportDetailVms;
        private VoyageDto _selectedVoyage;
        private ObservableCollection<VoyageDto> _voyages;
        private CommandViewModel cancelCommand;
        private IFuelController mainController;
        private string reportType;
        private IFuelReportServiceWrapper serviceWrapper;

        //private CommandViewModel submitCommand;

        //public CommandViewModel SubmitCommand
        //{
        //    get
        //    {
        //        if (this.submitCommand == null)
        //        {
        //            this.submitCommand = new CommandViewModel("تأیید", new DelegateCommand(this.Save));
        //        }
        //        return this.submitCommand;
        //    }
        //}

        //public CommandViewModel CancelCommand
        //{
        //    get
        //    {
        //        if (this.cancelCommand == null)
        //        {
        //            this.cancelCommand = new CommandViewModel("خروج", new DelegateCommand(() => { this.mainController.Close(this); }));
        //        }
        //        return this.cancelCommand;
        //    }
        //}


        //public ObservableCollection<FuelReportDetailVM> FuelReportDetailVms
        //{
        //    get { return this._fuelReportDetailVms; }
        //    set { this.SetField(vm => vm.FuelReportDetailVms, ref this._fuelReportDetailVms, value); }
        //}


        private VoyageDto entity;
        public VoyageDto Entity
        {
            get { return this.entity; }
            private set { this.SetField(p => p.Entity, ref this.entity, value); }
        }

        ////public string ReportType
        ////{
        ////    get { return this.reportType; }
        ////    set { this.SetField(p => p.ReportType, ref this.reportType, value); }
        ////}


        //public VoyageDto SelectedVoyage
        //{
        //    get { return this._selectedVoyage; }
        //    set
        //    {
        //        this.SetField(p => p.SelectedVoyage, ref this._selectedVoyage, value);

        //        this.Entity.Voyage = value;
        //    }
        //}

        //public ObservableCollection<VoyageDto> Voyages
        //{
        //    get { return this._voyages; }
        //    set { this.SetField(p => p.Voyages, ref this._voyages, value); }
        //}

        #endregion

        #region ctor

        public VoyageVM()
        {
            this.Entity = new VoyageDto();
        }

        public VoyageVM(IFuelController mainController, IFuelReportServiceWrapper serviceWrapper, IVoyageServiceWrapper voyageServiceWrapper)
            : this()
        {
            this.mainController = mainController;
            this.serviceWrapper = serviceWrapper;
            this.voyageServiceWrapper = voyageServiceWrapper;

            this.DisplayName = "سفر";
        }

        #endregion

        #region methods

        //private void Save()
        //{
        //    if (!this.Entity.Validate())
        //    {
        //        return;
        //    }

        //    this.ShowBusyIndicator();

        //    this.serviceWrapper.Update((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
        //                                                                                         {
        //                                                                                             this.HideBusyIndicator();
        //                                                                                             if (exp != null)
        //                                                                                             {
        //                                                                                                 this.mainController.HandleException(exp);
        //                                                                                             }
        //                                                                                             else
        //                                                                                             {
        //                                                                                                 this.Entity = res;
        //                                                                                                 this.mainController.Publish(new FuelReportListChangeArg());
        //                                                                                                 this.mainController.Close(this);
        //                                                                                             }
        //                                                                                             this.Entity = res;
        //                                                                                         }), this.Entity);
        //}

        //public void Load(VoyageDto ent)
        //{
        //    this.ShowBusyIndicator("درحال دریافت اطلاعات گزارش");
        //    this.serviceWrapper.GetById((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
        //                                                                                          {
        //                                                                                              if (exp == null)
        //                                                                                              {
        //                                                                                                  this.Entity = res;
        //                                                                                                  //this.ReportType = this.Entity.FuelReportType.ToString();
        //                                                                                                  //this.GetVoyages();
        //                                                                                              }
        //                                                                                              else
        //                                                                                              {
        //                                                                                                  this.mainController.HandleException(exp);
        //                                                                                              }

        //                                                                                              this.HideBusyIndicator();
        //                                                                                          }),
        //                                ent.Id);
        //}

        public void SetMainController(IFuelController fuelController)
        {
            this.mainController = fuelController;
        }

        public void SetServiceWrapper(IFuelReportServiceWrapper fuelReportServiceWrapper)
        {
            this.serviceWrapper = fuelReportServiceWrapper;
        }

        //public void SetVoyages(ObservableCollection<VoyageDto> voyages)
        //{
        //    this.Voyages = voyages;
        //}

        //private void GetVoyages()
        //{
        //    this.Voyages = new ObservableCollection<VoyageDto>();
        //    this.voyageServiceWrapper.GetByFilter(
        //                                          (res, exp) =>
        //                                          {
        //                                              this.mainController.BeginInvokeOnDispatcher(() =>
        //                                                                                          {
        //                                                                                              if (exp == null)
        //                                                                                              {
        //                                                                                                  this.Voyages.Clear();
        //                                                                                                  foreach (VoyageDto voyage in res)
        //                                                                                                  {
        //                                                                                                      this.Voyages.Add(voyage);
        //                                                                                                  }
        //                                                                                              }
        //                                                                                              else
        //                                                                                              {
        //                                                                                                  this.mainController.HandleException(exp);
        //                                                                                              }
        //                                                                                          });
        //                                          }, this.Entity.Company.Id, this.Entity.Id);
        //}

        public void SetEntity(VoyageDto entity)
        {
            this.Entity = entity;
            //this.GetVoyages();
        }

        #endregion
    }
}
