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
    public class VesselVM : WorkspaceViewModel
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IVesselServiceWrapper vesselServiceWrapper;
        private readonly ICompanyServiceWrapper companyServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string SUBMIT_COMMAND_TEXT = "ذخیره";
        private const string CANCEL_COMMAND_TEXT = "خروج";
        private const string SUCCESSFUL_OPERATION_MESSAGE = ".عملیات با موفقیت انجام پذیرفت";

        //================================================================================

        private bool isInEditMode;

        private VesselDto entity;

        public VesselDto Entity
        {
            get { return entity; }
            set
            {
                this.SetField(p => p.Entity, ref this.entity, value);
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

        public VesselVM()
        {
            this.isInEditMode = false;
            this.Entity = null;
            this.OwningCompanies = new ObservableCollection<CompanyDto>();

            this.PropertyChanged += VesselVM_PropertyChanged;
        }

        public VesselVM(
            IFuelController fuelMainController,
            IVesselServiceWrapper vesselServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper,
            IFileServiceWrapper fileServiceWrapper)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.vesselServiceWrapper = vesselServiceWrapper;
            this.companyServiceWrapper = companyServiceWrapper;
        }

        //================================================================================

        #region Event Handlers

        void VesselVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
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
            initialize(new VesselDto());
        }

        //================================================================================

        public void Edit(VesselDto _vesselDto)
        {
            this.isInEditMode = true;
            initialize(_vesselDto);
        }

        //================================================================================

        private void initialize(VesselDto _vesselDto)
        {
            loadOwningCompanies();
            this.Entity = _vesselDto;
        }

        //================================================================================

        private void submitForm()
        {
            this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

            if (this.isInEditMode)
            {
                throw new NotImplementedException();
            }
            else
            {
                this.Entity.Owner = this.SelectedOwningCompany;
                this.Entity.OwnerId = this.SelectedOwningCompany.Id;
                this.vesselServiceWrapper.AddVessel(submitActionCallback, this.Entity);
            }
        }

        //================================================================================

        private void submitActionCallback(VesselDto result, Exception exception)
        {
            this.fuelMainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();

                if (exception == null)
                {
                    this.fuelMainController.Publish(new VesselListChangeArg());

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
    }
}
