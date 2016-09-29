using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class OffhireDetailVM : WorkspaceViewModel
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IOffhireServiceWrapper offhireServiceWrapper;
        private readonly IGoodServiceWrapper goodServiceWrapper;

        //================================================================================

        private const string FETCH_DATA_BUSY_MESSAGE = "در حال دریافت اطلاعات ...";
        private const string IN_OPERATION_BUSY_MESSAGE = "در حال انجام عملیات ...";
        private const string SUBMIT_COMMAND_TEXT = "تأیید";
        private const string CANCEL_COMMAND_TEXT = "خروج";
        private const string SUCCESSFUL_OPERATION_MESSAGE = ".عملیات با موفقیت انجام پذیرفت";

        //================================================================================

        private bool isInEditMode;
        public bool IsInEditMode
        {
            get { return isInEditMode; }
            set
            {
                this.SetField(p => p.IsInEditMode, ref this.isInEditMode, value);
            }
        }

        private OffhireDetailDto entity;
        public OffhireDetailDto Entity
        {
            get { return entity; }
            set { this.SetField(p => p.Entity, ref this.entity, value); }
        }

        private OffhireDto offhire;
        public OffhireDto Offhire
        {
            get { return offhire; }
            set { this.SetField(p => p.Offhire, ref this.offhire, value); }
        }


        //private GoodDto selectedGood;
        //public GoodDto SelectedGood
        //{
        //    get { return this.selectedGood; }
        //    set { this.SetField(p => p.SelectedGood, ref this.selectedGood, value); }
        //}

        //private ObservableCollection<GoodDto> goods;
        //public ObservableCollection<GoodDto> Goods
        //{
        //    get { return this.goods; }
        //    set { this.SetField(p => p.Goods, ref this.goods, value); }
        //}

        //private TankDto selectedTank;
        //public TankDto SelectedTank
        //{
        //    get { return this.selectedTank; }
        //    set { this.SetField(p => p.SelectedTank, ref this.selectedTank, value); }
        //}

        //private ObservableCollection<TankDto> tanks;
        //public ObservableCollection<TankDto> Tanks
        //{
        //    get { return this.tanks; }
        //    set { this.SetField(p => p.Tanks, ref this.tanks, value); }
        //}

        //private GoodUnitDto selectedGoodUnit;
        //public GoodUnitDto SelectedGoodUnit
        //{
        //    get { return this.selectedGoodUnit; }
        //    set { this.SetField(p => p.SelectedGoodUnit, ref this.selectedGoodUnit, value); }
        //}

        //private ObservableCollection<GoodUnitDto> goodUnits;
        //public ObservableCollection<GoodUnitDto> GoodUnits
        //{
        //    get { return this.goodUnits; }
        //    set { this.SetField(p => p.GoodUnits, ref this.goodUnits, value); }
        //}

        //private CurrencyDto selectedCurrency;
        //public CurrencyDto SelectedCurrency
        //{
        //    get { return this.selectedCurrency; }
        //    set { this.SetField(p => p.SelectedCurrency, ref this.selectedCurrency, value); }
        //}

        //private ObservableCollection<CurrencyDto> currencies;
        //public ObservableCollection<CurrencyDto> Currencies
        //{
        //    get { return this.currencies; }
        //    set { this.SetField(p => p.Currencies, ref this.currencies, value); }
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

        public OffhireDetailVM()
        {
            this.isInEditMode = false;
            this.Entity = null;

            this.PropertyChanged += this.OffhireDetailVM_PropertyChanged;

            //this.Goods.CollectionChanged += Goods_CollectionChanged;
        }

        public OffhireDetailVM(
            IFuelController fuelMainController,
            IOffhireServiceWrapper offhireServiceWrapper,
            //ICompanyServiceWrapper companyServiceWrapper,
            IGoodServiceWrapper goodServiceWrapper)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.offhireServiceWrapper = offhireServiceWrapper;
            //this.companyServiceWrapper = companyServiceWrapper;
            this.goodServiceWrapper = goodServiceWrapper;
        }

        //================================================================================

        #region Event Handlers

        void OffhireDetailVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == this.GetPropertyName(p => p.SelectedOwningCompany))
            //    loadOwnedVessels();

            //if (e.PropertyName == this.GetPropertyName(p => p.Goods))
            //{
            //    if (this.Entity != null && this.Entity.Good != null && this.Goods != null)
            //        this.SelectedGood = this.Goods.FirstOrDefault(g => g.Id == this.Entity.Good.Id);
            //}

            //if (e.PropertyName == this.GetPropertyName(p => p.SelectedGoodUnit))
            //{
            //    if (this.Entity != null)
            //        this.Entity.Unit = SelectedGoodUnit;
            //}
        }

        //================================================================================

        //void Goods_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (this.Entity != null && this.Entity.Good != null && this.Goods != null)
        //    {
        //        var offhireDetailGood = this.Goods.FirstOrDefault(g => g.Id == this.Entity.Good.Id);

        //        if (offhireDetailGood != null)
        //        {
        //            refillObservableCollection(this.GoodUnits, offhireDetailGood.Units);
        //        }
        //    }
        //}

        //================================================================================

        #endregion

        //================================================================================

        protected override void OnRequestClose()
        {
            this.fuelMainController.Close(this);
        }

        //================================================================================

        public void Load(OffhireDto offhire)
        {
            this.IsInEditMode = false;
            initialize(offhire, new OffhireDetailDto());
        }

        //================================================================================

        public void Edit(OffhireDto offhire, OffhireDetailDto offhireDetailDto)
        {
            this.IsInEditMode = true;
            initialize(offhire, offhireDetailDto);
        }

        //================================================================================

        private void initialize(OffhireDto offhire, OffhireDetailDto offhireDetailDto)
        {
            this.Offhire = offhire;

            this.Entity = offhireDetailDto;

            //loadGoods();

            //loadCurrencies();
        }

        //================================================================================

        private void submitForm()
        {
            //this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

            //if (this.isInEditMode)
            //{
            //    this.offhireServiceWrapper.UpdateOffhireDetail(submitActionCallback, this.Entity);
            //}
            //else
            //{
            //    this.offhireServiceWrapper.AddOffhireDetail(submitActionCallback, this.Entity);
            //}
        }

        //================================================================================

        private void submitActionCallback(OffhireDetailDto result, Exception exception)
        {
            this.fuelMainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();

                if (exception == null)
                {
                    this.fuelMainController.Publish(new OffhireDetailListChangedArg());

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
    }
}
