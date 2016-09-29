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
    public class ScrapDetailVM : WorkspaceViewModel
    {
        //================================================================================

        private readonly IFuelController fuelMainController;
        private readonly IScrapServiceWrapper scrapServiceWrapper;
        private readonly ICompanyServiceWrapper companyServiceWrapper;
        private readonly IGoodServiceWrapper goodServiceWrapper;
        private readonly ICurrencyServiceWrapper currencyServiceWrapper;

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

                OnPropertyChanged(this.GetPropertyName(p => p.IsGoodEditable));
                OnPropertyChanged(this.GetPropertyName(p => p.IsTankEditable));
            }
        }

        private ScrapDetailDto entity;
        public ScrapDetailDto Entity
        {
            get { return entity; }
            set { this.SetField(p => p.Entity, ref this.entity, value); }
        }

        private ScrapDto scrap;
        public ScrapDto Scrap
        {
            get { return scrap; }
            set { this.SetField(p => p.Scrap, ref this.scrap, value); }
        }

        private GoodDto selectedGood;
        public GoodDto SelectedGood
        {
            get { return this.selectedGood; }
            set { this.SetField(p => p.SelectedGood, ref this.selectedGood, value); }
        }

        private ObservableCollection<GoodDto> goods;
        public ObservableCollection<GoodDto> Goods
        {
            get { return this.goods; }
            set { this.SetField(p => p.Goods, ref this.goods, value); }
        }

        private TankDto selectedTank;
        public TankDto SelectedTank
        {
            get { return this.selectedTank; }
            set { this.SetField(p => p.SelectedTank, ref this.selectedTank, value); }
        }

        //private ObservableCollection<TankDto> tanks;
        //public ObservableCollection<TankDto> Tanks
        //{
        //    get { return this.tanks; }
        //    set { this.SetField(p => p.Tanks, ref this.tanks, value); }
        //}

        private GoodUnitDto selectedGoodUnit;
        public GoodUnitDto SelectedGoodUnit
        {
            get { return this.selectedGoodUnit; }
            set { this.SetField(p => p.SelectedGoodUnit, ref this.selectedGoodUnit, value); }
        }

        private ObservableCollection<GoodUnitDto> goodUnits;
        public ObservableCollection<GoodUnitDto> GoodUnits
        {
            get { return this.goodUnits; }
            set { this.SetField(p => p.GoodUnits, ref this.goodUnits, value); }
        }

        private CurrencyDto selectedCurrency;
        public CurrencyDto SelectedCurrency
        {
            get { return this.selectedCurrency; }
            set { this.SetField(p => p.SelectedCurrency, ref this.selectedCurrency, value); }
        }

        private ObservableCollection<CurrencyDto> currencies;
        public ObservableCollection<CurrencyDto> Currencies
        {
            get { return this.currencies; }
            set { this.SetField(p => p.Currencies, ref this.currencies, value); }
        }

        public bool IsTankEditable
        {
            get
            {
                return !this.IsInEditMode || (this.Scrap != null && this.Scrap.IsTankEditable && this.Scrap.VesselInCompany.TankDtos != null && this.Scrap.VesselInCompany.TankDtos.Count != 0);
            }
        }

        public bool IsGoodEditable
        {
            get
            {
                return !this.IsInEditMode || (this.Scrap != null && this.Scrap.IsGoodEditable);
            }
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

        public ScrapDetailVM()
        {
            this.isInEditMode = false;
            this.Entity = null;
            this.Goods = new ObservableCollection<GoodDto>();
            this.Currencies = new ObservableCollection<CurrencyDto>();
            this.GoodUnits = new ObservableCollection<GoodUnitDto>();

            this.PropertyChanged += this.ScrapDetailVM_PropertyChanged;

            this.Goods.CollectionChanged += Goods_CollectionChanged;

        }

        public ScrapDetailVM(
            IFuelController fuelMainController,
            IScrapServiceWrapper scrapServiceWrapper,
            ICompanyServiceWrapper companyServiceWrapper,
            IGoodServiceWrapper goodServiceWrapper,
            ICurrencyServiceWrapper currencyServiceWrapper)
            : this()
        {
            this.fuelMainController = fuelMainController;
            this.scrapServiceWrapper = scrapServiceWrapper;
            this.companyServiceWrapper = companyServiceWrapper;
            this.goodServiceWrapper = goodServiceWrapper;
            this.currencyServiceWrapper = currencyServiceWrapper;
        }

        //================================================================================

        #region Event Handlers

        void ScrapDetailVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == this.GetPropertyName(p => p.SelectedOwningCompany))
            //    loadOwnedVessels();

            //if (e.PropertyName == this.GetPropertyName(p => p.Goods))
            //{
            //    if (this.Entity != null && this.Entity.Good != null && this.Goods != null)
            //        this.SelectedGood = this.Goods.FirstOrDefault(g => g.Id == this.Entity.Good.Id);
            //}

            if (e.PropertyName == this.GetPropertyName(p => p.SelectedGood))
                refillObservableCollection(this.GoodUnits, this.SelectedGood.Units);
            //if (e.PropertyName == this.GetPropertyName(p => p.SelectedGoodUnit))
            //{
            //    if (this.Entity != null)
            //        this.Entity.Unit = SelectedGoodUnit;
            //}
        }

        //================================================================================

        void Goods_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.Entity != null && this.Entity.Good != null && this.Goods != null)
            {
                var scrapDetailGood = this.Goods.FirstOrDefault(g => g.Id == this.Entity.Good.Id);

                if (scrapDetailGood != null)
                {
                    refillObservableCollection(this.GoodUnits, scrapDetailGood.Units);
                }
            }
        }

        //================================================================================

        #endregion

        //================================================================================

        protected override void OnRequestClose()
        {
            this.fuelMainController.Close(this);
        }

        //================================================================================

        public void Load(ScrapDto scrap)
        {
            this.IsInEditMode = false;
            initialize(scrap, new ScrapDetailDto());
        }

        //================================================================================

        public void Edit(ScrapDto scrap, ScrapDetailDto scrapDetailDto)
        {
            this.IsInEditMode = true;
            initialize(scrap, scrapDetailDto);
        }

        //================================================================================

        private void initialize(ScrapDto scrap, ScrapDetailDto scrapDetailDto)
        {
            this.Scrap = scrap;

            this.Entity = scrapDetailDto;
            this.Entity.Scrap = scrap;

            this.Entity.PropertyChanged += Entity_PropertyChanged;

            loadGoods();

            loadCurrencies();
        }

        //================================================================================

        void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.Entity.GetPropertyName(p => p.Good))
            {
                refillObservableCollection(this.GoodUnits, this.Entity.Good.Units);
            }
        }

        //================================================================================

        private void submitForm()
        {
            this.ShowBusyIndicator(IN_OPERATION_BUSY_MESSAGE);

            if (this.isInEditMode)
            {
                this.scrapServiceWrapper.UpdateScrapDetail(submitActionCallback, this.Entity);
            }
            else
            {
                this.scrapServiceWrapper.AddScrapDetail(submitActionCallback, this.Scrap.Id, this.Entity);
            }
        }

        //================================================================================

        private void submitActionCallback(ScrapDetailDto result, Exception exception)
        {
            this.fuelMainController.BeginInvokeOnDispatcher(() =>
            {
                this.HideBusyIndicator();

                if (exception == null)
                {
                    this.fuelMainController.Publish(new ScrapDetailListChangedArg());

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

        private void loadGoods()
        {
            this.ShowBusyIndicator(FETCH_DATA_BUSY_MESSAGE);

            this.goodServiceWrapper.GetAll(
                    (result, exception) => this.fuelMainController.BeginInvokeOnDispatcher(
                        () =>
                        {
                            if (exception == null)
                            {
                                if (result != null)
                                {
                                    refillObservableCollection(this.Goods, result);
                                }
                            }
                            else
                            {
                                this.fuelMainController.HandleException(exception);
                            }
                            this.HideBusyIndicator();
                        }), string.Empty, this.Scrap.VesselInCompany.Company.Id);
        }

        //================================================================================

        private void loadCurrencies()
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
