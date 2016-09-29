using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Castle.Core.Internal;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class OffhireVM : WorkspaceViewModel
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IOffhireServiceWrapper offhireServiceWrapper;
        private readonly ICompanyServiceWrapper companyServiceWrapper;
        private readonly ICurrencyServiceWrapper currencyServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string SUBMIT_COMMAND_TEXT = "تأیید";
        private const string CANCEL_COMMAND_TEXT = "خروج";
        private const string SUCCESSFUL_OPERATION_MESSAGE = ".عملیات با موفقیت انجام پذیرفت";
        private const string UPDATE_OFFHIRE_FEES_QUESTION_TEXT = "آیا قیمتهای سوخت فعلی به روز رسانی گردد؟";
        private const string UPDATE_OFFHIRE_FEES_QUESTION_TITLE = "به روزرسانی قیمتهای جدید";

        //================================================================================

        private bool isInEditMode;

        private OffhireDto entity;
        public OffhireDto Entity
        {
            get { return entity; }
            set { this.SetField(p => p.Entity, ref this.entity, value); }
        }

        //private CompanyDto selectedOwningCompany;
        //public CompanyDto SelectedOwningCompany
        //{
        //    get { return this.selectedOwningCompany; }
        //    set { this.SetField(p => p.SelectedOwningCompany, ref this.selectedOwningCompany, value); }
        //}

        //private ObservableCollection<CompanyDto> owningCompanies;
        //public ObservableCollection<CompanyDto> OwningCompanies
        //{
        //    get { return this.owningCompanies; }
        //    set { this.SetField(p => p.OwningCompanies, ref this.owningCompanies, value); }
        //}

        //private VesselInCompanyDto selectedVessel;
        //public VesselInCompanyDto SelectedVessel
        //{
        //    get { return this.selectedVessel; }
        //    set { this.SetField(p => p.SelectedVessel, ref this.selectedVessel, value); }
        //}

        //private ObservableCollection<VesselInCompanyDto> vessels;
        //public ObservableCollection<VesselInCompanyDto> Vessels
        //{
        //    get { return this.vessels; }
        //    set { this.SetField(p => p.Vessels, ref this.vessels, value); }
        //}

        //private CompanyDto selectedSecondPartyCompany;
        //public CompanyDto SelectedSecondPartyCompany
        //{
        //    get { return this.selectedSecondPartyCompany; }
        //    set { this.SetField(p => p.SelectedSecondPartyCompany, ref this.selectedSecondPartyCompany, value); }
        //}


        //private ObservableCollection<CompanyDto> secondPartyCompanies;
        //public ObservableCollection<CompanyDto> SecondPartyCompanies
        //{
        //    get { return this.secondPartyCompanies; }
        //    set { this.SetField(p => p.SecondPartyCompanies, ref this.secondPartyCompanies, value); }
        //}

        private ObservableCollection<CurrencyDto> currencies;
        public ObservableCollection<CurrencyDto> Currencies
        {
            get { return this.currencies; }
            set { this.SetField(p => p.Currencies, ref this.currencies, value); }
        }

        //private PagedSortableCollectionView<OffhireDetailDto> offhireDetails;
        //public PagedSortableCollectionView<OffhireDetailDto> OffhireDetails
        //{
        //    get { return this.offhireDetails; }
        //    set { this.SetField(p => p.OffhireDetails, ref this.offhireDetails, value); }
        //}

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

        public OffhireVM()
        {
            this.isInEditMode = false;
            this.Entity = null;
            //this.OwningCompanies = new ObservableCollection<CompanyDto>();
            //this.Vessels = new ObservableCollection<VesselInCompanyDto>();
            //this.SecondPartyCompanies = new ObservableCollection<CompanyDto>();
            this.Currencies = new ObservableCollection<CurrencyDto>();
            //this.OffhireDetails = new PagedSortableCollectionView<OffhireDetailDto>();

            //this.PricingValues = new List<PricingValueDto>();

            //this.PropertyChanged += OffhireVM_PropertyChanged;
        }

        public OffhireVM(
            IFuelController fuelMainController,
            IOffhireServiceWrapper offhireServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper, ICurrencyServiceWrapper currencyServiceWrapper)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.offhireServiceWrapper = offhireServiceWrapper;
            this.companyServiceWrapper = companyServiceWrapper;
            this.currencyServiceWrapper = currencyServiceWrapper;
        }

        //================================================================================

        #region Event Handlers

        //void OffhireVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    //if (e.PropertyName == this.GetPropertyName(p => p.SelectedOwningCompany))
        //    //    loadOwnedVessels();

        //    if (e.PropertyName == this.GetPropertyName(p => p.Entity))
        //    {
        //        //recalculateDetailsFee();
        //    }
        //}

        void OffhireDetail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FeeInVoucherCurrency" || e.PropertyName == "FeeInMainCurrency")
            {
                recalculateDetailsFee();
            }
        }

        void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.Entity.GetPropertyName(p => p.VoucherCurrency) ||
                e.PropertyName == this.Entity.GetPropertyName(p => p.VoucherDate))
            {
                recalculateDetailsFee();
            }
        }

        #endregion

        //================================================================================

        protected override void OnRequestClose()
        {
            this.fuelMainController.Close(this);
        }

        //================================================================================

        public void Load(long referenceNumber)
        {
            this.isInEditMode = false;

            loadCurrencies(() => initialize(referenceNumber));
        }

        //================================================================================

        public void Edit(OffhireDto offhireDto)
        {
            this.isInEditMode = true;

            loadCurrencies(() => initialize(offhireDto));
        }

        //================================================================================

        private void initialize(long referenceNumber)
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.offhireServiceWrapper.GetOffhirePreparedData(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                                                                                           () =>
                                                                                           {
                                                                                               if (exception == null)
                                                                                               {
                                                                                                   if (result != null)
                                                                                                   {
                                                                                                       this.Entity = result;

                                                                                                       this.Entity.PropertyChanged += this.Entity_PropertyChanged;
                                                                                                       this.Entity.OffhireDetails.ForEach(od => od.PropertyChanged += this.OffhireDetail_PropertyChanged);

                                                                                                       this.setPricingValues();
                                                                                                   }
                                                                                               }
                                                                                               else
                                                                                               {
                                                                                                   this.fuelMainController.HandleException(exception);
                                                                                                   this.OnRequestClose();
                                                                                               }
                                                                                               this.HideBusyIndicator();
                                                                                           }),
                        referenceNumber,
                        this.fuelMainController.GetCurrentUser().User.CompanyDto.Id);
        }


        //================================================================================

        //private void loadPricingValues()
        //{
        //    this.offhireServiceWrapper.GetPricingValues(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
        //                () =>
        //                {
        //                    if (exception == null)
        //                    {
        //                        if (result != null)
        //                        {
        //                            this.PricingValues = result;

        //                            if (this.isInEditMode)
        //                            {
        //                                if (isPricingAvailable())
        //                                {
        //                                    if (this.fuelMainController.ShowConfirmationBox(UPDATE_OFFHIRE_FEES_QUESTION_TEXT, UPDATE_OFFHIRE_FEES_QUESTION_TITLE))
        //                                        setDetailsPricing();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (isPricingAvailable())
        //                                    setDetailsPricing();
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.fuelMainController.HandleException(exception);
        //                        this.OnRequestClose();
        //                    }
        //                }),
        //                this.fuelMainController.GetCurrentUser().CompanyDto.Id,
        //                this.Entity.Vessel.Id,
        //                this.Entity.StartDateTime);
        //}


        private void setPricingValues()
        {
            if (this.isInEditMode)
            {
                if (isPricingAvailable(this.Entity.PricingValuesInMainCurrency))
                {
                    if (this.fuelMainController.ShowConfirmationBox(UPDATE_OFFHIRE_FEES_QUESTION_TEXT, UPDATE_OFFHIRE_FEES_QUESTION_TITLE))
                        setDetailsPricing(this.Entity.PricingValuesInMainCurrency);
                }
            }
            else
            {
                if (isPricingAvailable(this.Entity.PricingValuesInMainCurrency))
                    setDetailsPricing(this.Entity.PricingValuesInMainCurrency);
            }
        }

        private void recalculateDetailsFee()
        {
            if (this.Entity != null && this.Entity.VoucherCurrency != null && this.Entity.VoucherDate.HasValue && this.Entity.VoucherDate.Value != DateTime.MinValue)
            {
                foreach (var offhireDetail in this.Entity.OffhireDetails)
                {
                    var pricingValueForOffhireDetail = this.Entity.PricingValuesInMainCurrency.SingleOrDefault(pv => pv.Good.Id == offhireDetail.Good.Id);
                    if (pricingValueForOffhireDetail != null && pricingValueForOffhireDetail.Fee.HasValue)
                    {
                        currencyServiceWrapper.ConvertPrice(
                            (result, exception) =>
                                this.fuelMainController.BeginInvokeOnDispatcher(
                                    () =>
                                    {
                                        if (exception == null)
                                        {
                                            offhireDetail.PropertyChanged -= OffhireDetail_PropertyChanged;
                                            offhireDetail.FeeInVoucherCurrency = result;
                                            offhireDetail.PropertyChanged += OffhireDetail_PropertyChanged;
                                            this.fuelMainController.ShowMessage(".به روز رسانی گردید " + offhireDetail.Good.Name + " مبلغ 'فی (ارز سند)' برای ردیف");
                                        }
                                        else
                                        {
                                            this.fuelMainController.HandleException(exception);
                                        }
                                    }),
                                    pricingValueForOffhireDetail.Fee.Value,
                                    pricingValueForOffhireDetail.Currency.Id,
                                    this.Entity.VoucherCurrency.Id,
                                    this.Entity.VoucherDate.Value);
                    }
                    else //There is no pricing value found for current detail, so if the user has entered a fee in voucher currency (as it is editable), 
                    //the fee must be converted and set for fee in main currency too.
                    {
                        if (offhireDetail.FeeInVoucherCurrency.HasValue)
                        {
                            currencyServiceWrapper.GetCurrencyValueInMainCurrency(
                            (result, exception) =>
                                this.fuelMainController.BeginInvokeOnDispatcher(
                                    () =>
                                    {
                                        if (exception == null)
                                        {
                                            offhireDetail.PropertyChanged -= OffhireDetail_PropertyChanged;
                                            offhireDetail.FeeInMainCurrency = result;
                                            offhireDetail.PropertyChanged += OffhireDetail_PropertyChanged;
                                        }
                                        else
                                        {
                                            this.fuelMainController.HandleException(exception);
                                        }
                                    }),
                                    this.Entity.VoucherCurrency.Id,
                                    offhireDetail.FeeInVoucherCurrency.Value,
                                    this.Entity.VoucherDate.Value);
                        }
                    }
                }
            }
        }


        //================================================================================

        private bool isPricingAvailable(List<PricingValueDto> pricingValues)
        {
            return pricingValues.Count > 0;
        }

        //================================================================================

        private void setDetailsPricing(List<PricingValueDto> pricingValues)
        {
            this.Entity.OffhireDetails.ForEach(
                od =>
                {
                    var pricingValueForOffhireDetail = pricingValues.Single(pv => pv.Good.Id == od.Good.Id);
                    if (pricingValueForOffhireDetail != null && pricingValueForOffhireDetail.Fee.HasValue)
                    {
                        od.PropertyChanged -= OffhireDetail_PropertyChanged;
                        od.FeeInMainCurrency = pricingValueForOffhireDetail.Fee;
                        od.PropertyChanged += OffhireDetail_PropertyChanged;
                    }
                });
        }

        //================================================================================

        private void initialize(OffhireDto offhire)
        {
            this.Entity = offhire;
            this.Entity.PropertyChanged += Entity_PropertyChanged;
            this.Entity.OffhireDetails.ForEach(od => od.PropertyChanged += OffhireDetail_PropertyChanged);

            setPricingValues();
        }

        //================================================================================

        private void submitForm()
        {
            this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

            if (validate())
            {
                if (this.isInEditMode)
                {
                    this.offhireServiceWrapper.UpdateOffhire(submitActionCallback, this.Entity);
                }
                else
                {
                    this.offhireServiceWrapper.AddOffhire(submitActionCallback, this.Entity);
                }
            }
        }

        //================================================================================

        private void submitActionCallback(OffhireDto result, Exception exception)
        {
            this.fuelMainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();

                if (exception == null)
                {
                    this.fuelMainController.Publish(new OffhireListChangedArg());

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

        private bool validate()
        {
            var isEntityValid = false;

            try
            {
                isEntityValid = true;

                this.Entity.OffhireDetails.ForEach(od => isEntityValid &= od.Validate());

                isEntityValid = this.Entity.Validate();
            }
            catch (Exception)
            {
                isEntityValid = false;
            }

            return isEntityValid;
        }

        //private void loadOwningCompanies()
        //{
        //    this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //    this.companyServiceWrapper.GetAll(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
        //                () =>
        //                {
        //                    if (exception == null)
        //                    {
        //                        if (result != null)
        //                        {
        //                            this.OwningCompanies.Clear();

        //                            result.Result.ForEach(c => this.OwningCompanies.Add(c));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.fuelMainController.HandleException(exception);
        //                    }
        //                    this.HideBusyIndicator();
        //                }));
        //}

        //================================================================================

        //private void loadSecondPartyCompanies()
        //{
        //    this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //    this.companyServiceWrapper.GetAll(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
        //                () =>
        //                {
        //                    if (exception == null)
        //                    {
        //                        if (result != null)
        //                        {
        //                            this.SecondPartyCompanies.Clear();

        //                            result.Result.ForEach(c => this.SecondPartyCompanies.Add(c));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.fuelMainController.HandleException(exception);
        //                    }
        //                    this.HideBusyIndicator();
        //                })
        //        );
        //}

        //================================================================================

        //private void loadOwnedVessels()
        //{
        //    this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

        //    this.companyServiceWrapper.GetOwnedVessels(
        //            (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
        //                () =>
        //                {
        //                    if (exception == null)
        //                    {
        //                        if (result != null)
        //                        {
        //                            this.Vessels.Clear();

        //                            result.Result.ForEach(c => this.Vessels.Add(c));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.fuelMainController.HandleException(exception);
        //                    }
        //                    this.HideBusyIndicator();
        //                }), this.SelectedOwningCompany.Id);
        //}

        //================================================================================

        private void loadCurrencies(Action succeedAction)
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.currencyServiceWrapper.GetAllCurrency(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    refillObservableCollection(this.Currencies, result);
                                    if (succeedAction != null)
                                        succeedAction();
                                }
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }));
        }

        //================================================================================

        private void refillObservableCollection<T>(ObservableCollection<T> collection, PageResultDto<T> source) where T : class
        {
            collection.Clear();

            source.Result.ToList().ForEach(collection.Add);
        }

        private void refillObservableCollection<T>(ObservableCollection<T> collection, List<T> source) where T : class
        {
            collection.Clear();

            source.ForEach(collection.Add);
        }

        //================================================================================
    }
}
