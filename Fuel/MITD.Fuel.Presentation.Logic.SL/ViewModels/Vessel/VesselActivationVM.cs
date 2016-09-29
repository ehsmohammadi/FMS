using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Castle.Core.Internal;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class VesselActivationVM : WorkspaceViewModel
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IVesselActivationController vesselActivationController;
        private readonly IVesselInCompanyServiceWrapper vesselInCompanyServiceWrapper;
        private readonly ICompanyServiceWrapper companyServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string SUBMIT_COMMAND_TEXT = "ذخیره";
        private const string CANCEL_COMMAND_TEXT = "خروج";
        private const string SUCCESSFUL_OPERATION_MESSAGE = ".عملیات با موفقیت انجام پذیرفت";

        //================================================================================

        private VesselDto entity;

        public VesselDto Entity
        {
            get { return entity; }
            set
            {
                this.SetField(p => p.Entity, ref this.entity, value);
            }
        }

        private DateTime activationDate;
        public DateTime ActivationDate
        {
            get { return this.activationDate; }
            set { this.SetField(p=>p.ActivationDate, ref activationDate,value); }
        }

        private ObservableCollection<VesselActivationItemDto> vesselActivationItems;
        public ObservableCollection<VesselActivationItemDto> VesselActivationItems
        {
            get { return this.vesselActivationItems; }
            set { this.SetField(p => p.VesselActivationItems, ref this.vesselActivationItems, value); }
        }

        private VesselActivationItemDto selectedVesselActivationItem;
        public VesselActivationItemDto SelectedVesselActivationItem
        {
            get { return this.selectedVesselActivationItem; }
            set { this.SetField(p => p.SelectedVesselActivationItem, ref this.selectedVesselActivationItem, value); }
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

        public VesselActivationVM()
        {
            this.Entity = null;
            this.VesselActivationItems = new ObservableCollection<VesselActivationItemDto>();
            this.IsActivationDateDataEntryInPersianCalendar = Visibility.Visible;
            this.ActivationDate = DateTime.Now;
        }

        public VesselActivationVM(
            IFuelController fuelMainController,
            IVesselActivationController vesselActivationController,
            IVesselInCompanyServiceWrapper vesselInCompanyServiceWrapper)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.vesselActivationController = vesselActivationController;
            this.vesselInCompanyServiceWrapper = vesselInCompanyServiceWrapper;
        }

        //================================================================================

        protected override void OnRequestClose()
        {
            this.fuelMainController.Close(this);
        }

        //================================================================================

        public void Load(VesselDto _vesselDto)
        {
            this.Entity = _vesselDto;
            //this.IsActivationDateDataEntryInPersianCalendar = Visibility.Visible;
            //this.ActivationDate = DateTime.Now;
        }

        //================================================================================

        private void submitForm()
        {
            this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);
            this.vesselInCompanyServiceWrapper.ActivateWarehouseIncludingRecieptsOperation(submitActionCallback, Entity.Code, Entity.OwnerId, ActivationDate, vesselActivationItems.ToList());
        }

        //================================================================================

        private void submitActionCallback(VesselInCompanyDto result, Exception exception)
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

        private void vesselActivationItemAdded(VesselActivationItemDto newVesselActivationItem)
        {
            vesselActivationItems.Add(newVesselActivationItem);
        }

        //================================================================================

        private void cancelForm()
        {
            this.fuelMainController.Close(this);
        }

        //================================================================================

        private CommandViewModel addCommand;
        public CommandViewModel AddCommand
        {
            get
            {
                addCommand = new CommandViewModel("افزودن سوخت", new DelegateCommand(() =>
                {
                    vesselActivationController.AddVesselActivationItem(Entity, new Action<VesselActivationItemDto>(vesselActivationItemAdded));
                }));
                return addCommand;
            }
        }

        private CommandViewModel deleteCommand;
        public CommandViewModel DeleteCommand
        {
            get
            {
                deleteCommand = new CommandViewModel("حذف سوخت", new DelegateCommand(() =>
                {
                    if (SelectedVesselActivationItem != null)
                        vesselActivationItems.Remove(SelectedVesselActivationItem);
                    else
                    {
                        fuelMainController.ShowMessage("لطفا جهت حذف ابتدا یک سوخت از لیست انتخاب نمایید");
                    }
                }));
                return deleteCommand;
            }

        }


        private DelegateCommand activationDateDataEntryToggleCommand;
        public DelegateCommand ActivationDateDataEntryToggleCommand
        {
            get
            {
                activationDateDataEntryToggleCommand = new DelegateCommand(() =>
                {
                    this.IsActivationDateDataEntryInPersianCalendar = this.IsActivationDateDataEntryInPersianCalendar == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                });

                return activationDateDataEntryToggleCommand;
            }
        }

        private string activationDateDataEntryToggleDisplayName;
        public string ActivationDateDataEntryToggleDisplayName
        {
            set
            {
                this.SetField(p => p.ActivationDateDataEntryToggleDisplayName, ref activationDateDataEntryToggleDisplayName, value);
            }
            get { return activationDateDataEntryToggleDisplayName; }
        }

        private Visibility isActivationDateDataEntryInPersianCalendar;
        public Visibility IsActivationDateDataEntryInPersianCalendar
        {
            get { return this.isActivationDateDataEntryInPersianCalendar; }
            set
            {
                this.SetField(p=>p.IsActivationDateDataEntryInPersianCalendar, ref isActivationDateDataEntryInPersianCalendar, value);
                this.OnPropertyChanged(p => p.IsActivationDateDataEntryInGregorianCalendar);
                ActivationDateDataEntryToggleDisplayName = value == Visibility.Visible ? "تاریخ فعالسازی شمسی" : "تاریخ فعالسازی میلادی";
            }
        }

        public Visibility IsActivationDateDataEntryInGregorianCalendar
        {
            get { return this.isActivationDateDataEntryInPersianCalendar == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
        }
    }
}
