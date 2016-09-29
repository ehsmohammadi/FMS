using System;
using System.Collections.ObjectModel;
using Castle.Core.Internal;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class ScrapVM : WorkspaceViewModel
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IScrapServiceWrapper scrapServiceWrapper;
        private readonly ICompanyServiceWrapper companyServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string SUBMIT_COMMAND_TEXT = "تأیید";
        private const string CANCEL_COMMAND_TEXT = "خروج";
        private const string SUCCESSFUL_OPERATION_MESSAGE = ".عملیات با موفقیت انجام پذیرفت";

        //================================================================================


        public UploaderVM UploaderVm { get; set; }

        private bool isInEditMode;

        private ScrapDto entity;

        public ScrapDto Entity
        {
            get { return entity; }
            set
            {
                this.SetField(p => p.Entity, ref this.entity, value);
                if (UploaderVm != null)
                    if (value != null && value.Id > 0)
                    {
                        UploaderVm.EntityId = value.Id;
                        UploaderVm.Visible();
                    }
                    else
                    {
                        UploaderVm.EntityId = 0;
                        UploaderVm.InVisible();
                    }
            }
        }

        private CompanyDto selectedOwningCompany;
        public CompanyDto SelectedOwningCompany
        {
            get { return this.selectedOwningCompany; }
            set { this.SetField(p => p.SelectedOwningCompany, ref this.selectedOwningCompany, value); }
        }

        private ObservableCollection<CompanyDto> owningCompanies;
        public ObservableCollection<CompanyDto> OwningCompanies
        {
            get { return this.owningCompanies; }
            set { this.SetField(p => p.OwningCompanies, ref this.owningCompanies, value); }
        }

        //private VesselInCompanyDto selectedVessel;
        //public VesselInCompanyDto SelectedVessel
        //{
        //    get { return this.selectedVessel; }
        //    set { this.SetField(p => p.SelectedVessel, ref this.selectedVessel, value); }
        //}

        private ObservableCollection<VesselInCompanyDto> vessels;
        public ObservableCollection<VesselInCompanyDto> Vessels
        {
            get { return this.vessels; }
            set { this.SetField(p => p.Vessels, ref this.vessels, value); }
        }

        //private CompanyDto selectedSecondPartyCompany;
        //public CompanyDto SelectedSecondPartyCompany
        //{
        //    get { return this.selectedSecondPartyCompany; }
        //    set { this.SetField(p => p.SelectedSecondPartyCompany, ref this.selectedSecondPartyCompany, value); }
        //}


        private ObservableCollection<CompanyDto> secondPartyCompanies;
        public ObservableCollection<CompanyDto> SecondPartyCompanies
        {
            get { return this.secondPartyCompanies; }
            set { this.SetField(p => p.SecondPartyCompanies, ref this.secondPartyCompanies, value); }
        }


        //================================================================================

        private CommandViewModel submitCommand;
        public CommandViewModel SubmitCommand
        {
            get
            {
                if (submitCommand == null)
                    submitCommand = new CommandViewModel(SUBMIT_COMMAND_TEXT, new DelegateCommand(this.submitForm));

                return submitCommand;
            }
        }

        private CommandViewModel cancelCommand;
        public CommandViewModel CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new CommandViewModel(CANCEL_COMMAND_TEXT, new DelegateCommand(this.cancelForm));

                return cancelCommand;
            }
        }

        //================================================================================

        public ScrapVM()
        {
            this.isInEditMode = false;
            this.Entity = null;
            this.OwningCompanies = new ObservableCollection<CompanyDto>();
            this.Vessels = new ObservableCollection<VesselInCompanyDto>();
            this.SecondPartyCompanies = new ObservableCollection<CompanyDto>();

            this.PropertyChanged += ScrapVM_PropertyChanged;
        }

        public ScrapVM(
            IFuelController fuelMainController,
            IScrapServiceWrapper scrapServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper,
            IFileServiceWrapper fileServiceWrapper)
            : this()
        {
            UploaderVm = new UploaderVM(fuelMainController, fileServiceWrapper);
            UploaderVm.AttachmentType = AttachmentType.Scrap;
            this.fuelMainController = fuelMainController;
            this.scrapServiceWrapper = scrapServiceWrapper;
            this.companyServiceWrapper = companyServiceWrapper;
        }

        //================================================================================

        #region Event Handlers

        void ScrapVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.GetPropertyName(p => p.SelectedOwningCompany))
                loadOwnedVessels();

            if (e.PropertyName == this.GetPropertyName(p => p.Entity))
            {
                if (this.Entity != null && this.Entity.VesselInCompany != null)
                    this.SelectedOwningCompany = this.Entity.VesselInCompany.Company;
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
            this.isInEditMode = false;
            initialize(new ScrapDto());
        }

        //================================================================================

        public void Edit(ScrapDto scrapDto)
        {
            this.isInEditMode = true;
            initialize(scrapDto);
        }

        //================================================================================

        private void initialize(ScrapDto scrapDto)
        {
            loadOwningCompanies();

            loadSecondPartyCompanies();

            this.Entity = scrapDto;

            setUploaderVmStatus();
        }

        //================================================================================

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

        private void submitForm()
        {
            this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

            if (this.isInEditMode)
            {
                this.scrapServiceWrapper.UpdateScrap(submitActionCallback, this.Entity);
            }
            else
            {
                this.scrapServiceWrapper.AddScrap(submitActionCallback, this.Entity);
            }
        }

        //================================================================================

        private void submitActionCallback(ScrapDto result, Exception exception)
        {
            this.fuelMainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();

                if (exception == null)
                {
                    this.fuelMainController.Publish(new ScrapListChangedArg());

                    this.fuelMainController.ShowMessage(SUCCESSFUL_OPERATION_MESSAGE);

                    this.fuelMainController.Close(this);
                }
                else
                {
                    this.fuelMainController.HandleException(exception);
                }
            });
        }

        //================================================================================

        private void cancelForm()
        {
            this.fuelMainController.Close(this);
        }

        //================================================================================

        private void loadOwningCompanies()
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.companyServiceWrapper.GetAll(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    this.OwningCompanies.Clear();

                                    result.ForEach(c => this.OwningCompanies.Add(c));
                                }
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }), true);
        }

        //================================================================================

        private void loadSecondPartyCompanies()
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.companyServiceWrapper.GetAll(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    this.SecondPartyCompanies.Clear();

                                    result.ForEach(c => this.SecondPartyCompanies.Add(c));
                                }
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }), false);
        }

        //================================================================================

        private void loadOwnedVessels()
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.companyServiceWrapper.GetOwnedVessels(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    this.Vessels.Clear();

                                    result.Result.ForEach(c => this.Vessels.Add(c));
                                }
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }), this.SelectedOwningCompany.Id);
        }

        //================================================================================
    }
}
