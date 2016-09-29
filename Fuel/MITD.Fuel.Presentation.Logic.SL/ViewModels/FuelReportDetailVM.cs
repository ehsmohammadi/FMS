using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class FuelReportDetailVM : WorkspaceViewModel
    {
        #region Prop

        private IFuelController mainController;
        private IFuelReportServiceWrapper serviceWrapper;
        //private IGoodServiceWrapper goodServiceWrapper;

        private CommandViewModel submitCommand;
        public CommandViewModel SubmitCommand
        {
            get
            {
                if (this.submitCommand == null)
                    this.submitCommand = new CommandViewModel("ذخیره", new DelegateCommand(this.Save));

                return this.submitCommand;
            }
        }

        private CommandViewModel cancelCommand;
        public CommandViewModel CancelCommand
        {
            get
            {
                if (this.cancelCommand == null)
                {
                    this.cancelCommand = new CommandViewModel("خروج", new DelegateCommand(() =>
                    {
                        this.mainController.Close(this);
                    }));
                }
                return this.cancelCommand;
            }
        }


        private FuelReportDetailDto entity;
        public FuelReportDetailDto Entity
        {
            get { return this.entity; }
            set { this.SetField(p => p.Entity, ref this.entity, value); }
        }

        //private bool isFinalApprove;
        //public bool IsFinalApprove
        //{
        //    get { return isFinalApprove; }
        //    set { this.SetField(p => p.IsFinalApprove, ref isFinalApprove, value); }
        //}

        private List<ComboBoxItm> correctionTypes;
        public List<ComboBoxItm> CorrectionTypes
        {
            get { return this.correctionTypes; }
            set
            {
                this.SetField(p => p.CorrectionTypes, ref this.correctionTypes, value);
            }
        }

        private ObservableCollection<ComboBoxItm> correctionPricingTypeItems;
        public ObservableCollection<ComboBoxItm> CorrectionPricingTypeItems
        {
            get { return this.correctionPricingTypeItems; }
            set
            {
                this.SetField(p => p.CorrectionPricingTypeItems, ref this.correctionPricingTypeItems, value);
            }
        }

        private List<ComboBoxItm> transferTypes;
        public List<ComboBoxItm> TransferTypes
        {
            get { return this.transferTypes; }
            set
            {
                this.SetField(p => p.TransferTypes, ref this.transferTypes, value);
            }
        }

        private List<ComboBoxItm> receiveTypes;
        public List<ComboBoxItm> ReceiveTypes
        {
            get { return this.receiveTypes; }
            set
            {
                this.SetField(p => p.ReceiveTypes, ref this.receiveTypes, value);
            }
        }

        //private ObservableCollection<CorrectionTypeEnum> correctionTypes;
        //public ObservableCollection<CorrectionTypeEnum> CorrectionTypes
        //{
        //    get { return correctionTypes; }
        //    set
        //    {
        //        this.SetField(p => p.CorrectionTypes, ref correctionTypes, value);
        //    }
        //}

        //private ObservableCollection<TransferTypeEnum> transferTypes;
        //public ObservableCollection<TransferTypeEnum> TransferTypes
        //{
        //    get { return transferTypes; }
        //    set
        //    {
        //        this.SetField(p => p.TransferTypes, ref transferTypes, value);
        //    }
        //}

        //private ObservableCollection<ReceiveTypeEnum> receiveTypes;
        //public ObservableCollection<ReceiveTypeEnum> ReceiveTypes
        //{
        //    get { return receiveTypes; }
        //    set
        //    {
        //        this.SetField(p => p.ReceiveTypes, ref receiveTypes, value);
        //    }
        //}


        private long correctionTypeId;
        public long CorrectionTypeId
        {
            get { return this.correctionTypeId; }
            set
            {
                this.SetField(p => p.CorrectionTypeId, ref this.correctionTypeId, value);
                this.Entity.CorrectionType = (CorrectionTypeEnum)this.CorrectionTypeId;
            }
        }

        private long transferId;
        public long TransferId
        {
            get { return this.transferId; }
            set
            {
                this.SetField(p => p.TransferId, ref this.transferId, value);
                this.Entity.TransferType = (TransferTypeEnum)this.TransferId;
            }
        }

        private long receiveId;
        public long ReceiveId
        {
            get { return this.receiveId; }
            set
            {
                this.SetField(p => p.ReceiveId, ref this.receiveId, value);
                this.Entity.ReceiveType = (ReceiveTypeEnum)this.ReceiveId;
            }
        }

        private long correctionPricingTypeId;
        public long CorrectionPricingTypeId
        {
            get { return this.correctionPricingTypeId; }
            set
            {
                this.SetField(p => p.CorrectionPricingTypeId, ref this.correctionPricingTypeId, value);
                this.Entity.CorrectionPricingType = (CorrectionPricingTypeEnum)this.CorrectionPricingTypeId;
            }
        }

        private ObservableCollection<FuelReportCorrectionReferenceNoDto> fuelReportCorrectionReferenceNoDtos;
        public ObservableCollection<FuelReportCorrectionReferenceNoDto> FuelReportCorrectionReferenceNoDtos
        {
            get { return this.fuelReportCorrectionReferenceNoDtos; }
            set
            {
                this.SetField(p => p.FuelReportCorrectionReferenceNoDtos, ref this.fuelReportCorrectionReferenceNoDtos, value);

            }
        }

        private ObservableCollection<FuelReportReceiveReferenceNoDto> fuelReportReceiveReferenceNoDtos;
        public ObservableCollection<FuelReportReceiveReferenceNoDto> FuelReportReceiveReferenceNoDtos
        {
            get { return this.fuelReportReceiveReferenceNoDtos; }
            set
            {
                this.SetField(p => p.FuelReportReceiveReferenceNoDtos, ref this.fuelReportReceiveReferenceNoDtos, value);

            }
        }

        private ObservableCollection<FuelReportTransferReferenceNoDto> fuelReportTransferReferenceNoDto;
        public ObservableCollection<FuelReportTransferReferenceNoDto> FuelReportTransferReferenceNoDtos
        {
            get { return this.fuelReportTransferReferenceNoDto; }
            set
            {
                this.SetField(p => p.FuelReportTransferReferenceNoDtos, ref this.fuelReportTransferReferenceNoDto, value);

            }
        }

        private ObservableCollection<GoodUnitDto> unitDtos;
        public ObservableCollection<GoodUnitDto> UnitDtos
        {


            get
            {

                return this.unitDtos;
            }
            set
            {
                this.SetField(vm => vm.UnitDtos, ref this.unitDtos, value);
            }

        }

        private ObservableCollection<CurrencyDto> currencyDtos;
        public ObservableCollection<CurrencyDto> CurrencyDtos
        {
            get { return this.currencyDtos; }
            set
            {
                this.SetField(p => p.CurrencyDtos, ref this.currencyDtos, value);

            }
        }

        public string ReceiveTypeName
        {

            get
            {
                string str = this.Entity.ReceiveType.ToString();
                str = (string.IsNullOrEmpty(str) ? "" : str);

                return str;
            }
        }

        public string TransferTypeName
        {

            get
            {
                string str = this.Entity.TransferType.ToString();
                str = (string.IsNullOrEmpty(str) ? "" : str);


                return str;
            }
        }

        public string CorrectionName
        {
            get
            {
                var str = this.Entity.CorrectionType.ToString();
                str = (string.IsNullOrEmpty(str) ? "" : str);


                return str;
            }
        }


        private bool isTransferTypeActive;
        public bool IsTransferTypeActive
        {
            get { return this.isTransferTypeActive; }
            set { this.SetField(p => p.IsTransferTypeActive, ref this.isTransferTypeActive, value); }
        }

        //private bool isTransferReferenceActive;
        //public bool IsTransferReferenceActive
        //{
        //    get { return isTransferReferenceActive; }
        //    set { this.SetField(p => p.IsTransferReferenceActive, ref isTransferReferenceActive, value); }
        //}

        private Visibility isTransferReferenceActive;
        public Visibility IsTransferReferenceActive
        {
            get { return this.isTransferReferenceActive; }
            set { this.SetField(p => p.IsTransferReferenceActive, ref this.isTransferReferenceActive, value); }
        }

        private bool isReceiveTypeActive;
        public bool IsReceiveTypeActive
        {
            get { return this.isReceiveTypeActive; }
            set { this.SetField(p => p.IsReceiveTypeActive, ref this.isReceiveTypeActive, value); }
        }

        //private bool isReceiveReferenceActive;
        //public bool IsReceiveReferenceActive
        //{
        //    get { return isReceiveReferenceActive; }
        //    set { this.SetField(p => p.IsReceiveReferenceActive, ref isReceiveReferenceActive, value); }
        //}

        private Visibility isReceiveReferenceActive;
        public Visibility IsReceiveReferenceActive
        {
            get { return this.isReceiveReferenceActive; }
            set { this.SetField(p => p.IsReceiveReferenceActive, ref this.isReceiveReferenceActive, value); }
        }

        private bool isCorrectionTypeActive;
        public bool IsCorrectionTypeActive
        {
            get { return this.isCorrectionTypeActive; }
            set { this.SetField(p => p.IsCorrectionTypeActive, ref this.isCorrectionTypeActive, value); }
        }

        private bool isCorrectionPricingTypeActive;
        public bool IsCorrectionPricingTypeActive
        {
            get { return this.isCorrectionPricingTypeActive; }
            set { this.SetField(p => p.IsCorrectionPricingTypeActive, ref this.isCorrectionPricingTypeActive, value); }
        }

        private bool isCorrectionReferenceActive;
        public bool IsCorrectionReferenceActive
        {
            get { return this.isCorrectionReferenceActive; }
            set { this.SetField(p => p.IsCorrectionReferenceActive, ref this.isCorrectionReferenceActive, value); }
        }



        private bool isCorrectionDefaultPricingActive;
        public bool IsCorrectionDefaultPricingActive
        {
            get { return this.isCorrectionDefaultPricingActive; }
            set { this.SetField(p => p.IsCorrectionDefaultPricingActive, ref this.isCorrectionDefaultPricingActive, value); }
        }

        private bool isCorrectionLastIssuedConsumptionPricingActive;
        public bool IsCorrectionLastIssuedConsumptionPricingActive
        {
            get { return this.isCorrectionLastIssuedConsumptionPricingActive; }
            set { this.SetField(p => p.IsCorrectionLastIssuedConsumptionPricingActive, ref this.isCorrectionLastIssuedConsumptionPricingActive, value); }
        }

        private bool isCorrectionLastPurchasePricingActive;
        public bool IsCorrectionLastPurchasePricingActive
        {
            get { return this.isCorrectionLastPurchasePricingActive; }
            set { this.SetField(p => p.IsCorrectionLastPurchasePricingActive, ref this.isCorrectionLastPurchasePricingActive, value); }
        }

        private bool isCorrectionManualPricingActive;
        public bool IsCorrectionManualPricingActive
        {
            get { return this.isCorrectionManualPricingActive; }
            set { this.SetField(p => p.IsCorrectionManualPricingActive, ref this.isCorrectionManualPricingActive, value); }
        }

        private bool isEditingPossible;
        public bool IsEditingPossible
        {
            get { return this.isEditingPossible; }
            set { this.SetField(p => p.IsEditingPossible, ref this.isEditingPossible, value); }
        }

        private bool isTrustQuantityFetchPossible;
        public bool IsTrustQuantityFetchPossible
        {
            get { return this.isTrustQuantityFetchPossible; }
            set { this.SetField(p => p.IsTrustQuantityFetchPossible, ref this.isTrustQuantityFetchPossible, value); }
        }

        private bool isClearReceiveQuantityByTrustVisible;
        public bool IsClearReceiveQuantityByTrustVisible
        {
            get { return this.isClearReceiveQuantityByTrustVisible; }
            set { this.SetField(p => p.IsClearReceiveQuantityByTrustVisible, ref this.isClearReceiveQuantityByTrustVisible, value); }
        }


        private CommandViewModel clearTrustIssueQuantityCommand;
        public CommandViewModel ClearTrustIssueQuantityCommand
        {
            get
            {
                if (this.clearTrustIssueQuantityCommand == null)
                {
                    this.clearTrustIssueQuantityCommand = new CommandViewModel("حذف مقدار امانی", new DelegateCommand(() =>
                    {
                        this.Entity.Recieve = null;
                        this.Entity.TrustIssueInventoryTransactionItemId = null;
                        this.Entity.IsTheTrustIssueQuantityAssignedTo = false;

                        //this.SetEnableStatus();
                    }));
                }
                return this.clearTrustIssueQuantityCommand;
            }
        }

        private CommandViewModel fetchTrustIssueQuantityCommand;
        public CommandViewModel FetchTrustIssueQuantityCommand
        {
            get
            {
                if (this.fetchTrustIssueQuantityCommand == null)
                {
                    this.fetchTrustIssueQuantityCommand = new CommandViewModel("دریافت مقدار امانی", new DelegateCommand(() =>
                    {

                        this.Entity.Recieve = this.Entity.TrustIssueInventoryResultItem.Quantity;
                        this.Entity.TrustIssueInventoryTransactionItemId = this.Entity.TrustIssueInventoryResultItem.Id;
                        this.Entity.IsTheTrustIssueQuantityAssignedTo = true;

                        //this.SetEnableStatus();
                    }));
                }
                return this.fetchTrustIssueQuantityCommand;
            }
        }

        #endregion

        #region Ctor

        public FuelReportDetailVM(IFuelController appController, IFuelReportServiceWrapper fuelReportServiceWrapper/*, IGoodServiceWrapper goodServiceWrapper*/)
        {
            this.mainController = appController;
            this.serviceWrapper = fuelReportServiceWrapper;
            //this.goodServiceWrapper = goodServiceWrapper;

            this.FuelReportTransferReferenceNoDtos = new ObservableCollection<FuelReportTransferReferenceNoDto>();
            this.FuelReportReceiveReferenceNoDtos = new ObservableCollection<FuelReportReceiveReferenceNoDto>();

            this.UnitDtos = new ObservableCollection<GoodUnitDto>();
            this.setEntity(new FuelReportDetailDto());
            this.DisplayName = "جزئیات گزارش ";

            this.CorrectionPricingTypeItems = new ObservableCollection<ComboBoxItm>();

            this.PropertyChanged += this.FuelReportDetailVM_PropertyChanged;
        }

        #endregion

        #region Method

        void FuelReportDetailVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.GetPropertyName(p => p.CorrectionTypeId) == e.PropertyName)
            {

            }
        }

        void GetCurrency()
        {
            this.CurrencyDtos = new ObservableCollection<CurrencyDto>();
            this.serviceWrapper.GetAllCurrency((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
            {
                if (exp == null)
                {
                    res.ForEach(c => this.CurrencyDtos.Add(c));
                }
                else
                {
                    this.mainController.HandleException(exp);
                }
            }));
        }

        public void SetEnableStatus()
        {
            if (this.Entity.Recieve.HasValue && this.Entity.Recieve > 0)
            {
                this.IsReceiveTypeActive = this.Entity.EnableCommercialEditing;

                this.IsReceiveReferenceActive = this.Entity.EnableCommercialEditing ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                this.IsReceiveTypeActive = false;
                this.IsReceiveReferenceActive = Visibility.Collapsed;
                this.ReceiveId = (long)ReceiveTypeEnum.NotDefined;
                this.Entity.FuelReportReceiveReferenceNoDto = null;
            }

            if (this.Entity.Transfer.HasValue && this.Entity.Transfer > 0)
            {
                this.IsTransferTypeActive = this.Entity.EnableCommercialEditing;
                this.IsTransferReferenceActive = this.Entity.EnableCommercialEditing ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                this.IsTransferTypeActive = false;
                this.IsTransferReferenceActive = Visibility.Collapsed;
                this.TransferId = (long)TransferTypeEnum.NotDefined;
                this.Entity.FuelReportTransferReferenceNoDto = null;
            }

            if (this.Entity.Correction.HasValue && this.Entity.Correction > 0)
            {
                this.IsCorrectionTypeActive = this.Entity.EnableCommercialEditing;
                this.IsCorrectionReferenceActive = this.Entity.EnableFinancialEditing;
            }
            else
            {
                this.IsCorrectionTypeActive = this.IsCorrectionReferenceActive = false;
                this.CorrectionTypeId = (long)CorrectionTypeEnum.NotDefined;
                this.Entity.CurrencyDto = null;
                this.Entity.CorrectionPrice = null;
                this.Entity.FuelReportCorrectionReferenceNoDto = null;
            }

            this.IsEditingPossible = this.Entity.EnableCommercialEditing || this.Entity.EnableFinancialEditing;

            this.IsTrustQuantityFetchPossible = this.Entity.EnableCommercialEditing && this.Entity.IsTrustIssueQuantityAssignmentPossible && !this.Entity.IsTheTrustIssueQuantityAssignedTo;

            this.IsClearReceiveQuantityByTrustVisible = this.Entity.EnableCommercialEditing && this.Entity.IsTheTrustIssueQuantityAssignedTo;
        }

        private void Save()
        {
            if (!this.Entity.Validate()) return;

            this.Entity.ReceiveType = (ReceiveTypeEnum)this.ReceiveId;
            this.Entity.TransferType = (TransferTypeEnum)this.TransferId;
            this.Entity.CorrectionType = (CorrectionTypeEnum)this.CorrectionTypeId;
            //Entity.Recieve = ReceiveValue;
            //Entity.Transfer = TransferValue;
            //Entity.Correction = CorrectionValue;

            this.serviceWrapper.UpdateFuelReportDetail((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
            {
                if (exp != null)
                    this.mainController.HandleException(exp);
                else
                {
                    this.setEntity(res);
                    this.mainController.Close(this);
                    this.mainController.Publish(new FuelReportChangedArg() { FuelReportId = res.FuelReportId });
                    this.mainController.Publish(new FuelReportDetailListChangeArg());
                }
            }), this.Entity);
        }

        public void Load(FuelReportDetailDto fuelReportDetailDto)
        {
            this.GetCurrency();
            this.SetCollection();

            this.serviceWrapper.GetFuelReportDetailById((res, exp) => this.mainController.BeginInvokeOnDispatcher(() =>
                {
                    if (exp == null)
                    {
                        this.setEntity(res);

                        //SetEnableStatus();
                    }
                    else
                    {
                        this.mainController.HandleException(exp);
                    }
                }), fuelReportDetailDto.FuelReportId, fuelReportDetailDto.Id, true);
        }

        void SetCollection()
        {
            //CorrectionTypes = Infrastructure.EnumHelper.GetItems(typeof(CorrectionTypeEnum));
            //CorrectionTypes.First(cmbitem => cmbitem.Id == (long)CorrectionTypeEnum.NotDefined).Name = " ";

            //TransferTypes = Infrastructure.EnumHelper.GetItems(typeof(TransferTypeEnum));
            //TransferTypes.First(cmbitem => cmbitem.Id == (long)TransferTypeEnum.NotDefined).Name = " ";

            //ReceiveTypes = Infrastructure.EnumHelper.GetItems(typeof(ReceiveTypeEnum));
            //ReceiveTypes.First(cmbitem => cmbitem.Id == (long)ReceiveTypeEnum.NotDefined).Name = " ";

            this.ReceiveTypes = typeof(ReceiveTypeEnum).ToComboItemList();
            this.TransferTypes = typeof(TransferTypeEnum).ToComboItemList();
            this.CorrectionTypes = typeof(CorrectionTypeEnum).ToComboItemList();
        }

        private void setEntity(FuelReportDetailDto dto)
        {
            this.Entity = dto;
            this.Entity.PropertyChanged += this.Entity_PropertyChanged;

            this.setTransferReferences();
            this.setReceiveReferences();
            this.setCorrectionPricingTypes();
            this.manageChangeCorrectionPricingTypeValue();

            this.ReceiveId = (long)this.Entity.ReceiveType;
            this.TransferId = (long)this.Entity.TransferType;
            this.CorrectionTypeId = (long)this.Entity.CorrectionType;
            this.CorrectionPricingTypeId = (long)this.Entity.CorrectionPricingType;
            //this.Entity = dto;
            //this.Entity.PropertyChanged += this.Entity_PropertyChanged;

            this.OnPropertyChanged(p => p.Entity);
            this.Entity.OnPropertyChanged(p => p.FuelReportReceiveReferenceNoDto);
            this.Entity.OnPropertyChanged(p => p.FuelReportCorrectionReferenceNoDto);
            this.Entity.OnPropertyChanged(p => p.FuelReportTransferReferenceNoDto);
        }

        void Entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetEnableStatus();

            if (e.PropertyName == this.Entity.GetPropertyName(p => p.TransferType))
            {
                this.Entity.FuelReportTransferReferenceNoDto = new FuelReportTransferReferenceNoDto();
                this.setTransferReferences();
            }

            if (e.PropertyName == this.Entity.GetPropertyName(p => p.ReceiveType))
            {
                this.Entity.FuelReportReceiveReferenceNoDto = new FuelReportReceiveReferenceNoDto();
                this.setReceiveReferences();
            }

            if (e.PropertyName == this.Entity.GetPropertyName(p => p.CorrectionType))
            {
                this.setCorrectionPricingTypes();
            }

            if (e.PropertyName == this.Entity.GetPropertyName(p => p.CorrectionPricingType))
            {
                this.manageChangeCorrectionPricingTypeValue();
            }
        }

        private void manageChangeCorrectionPricingTypeValue()
        {
            this.IsCorrectionDefaultPricingActive =
                this.IsCorrectionLastPurchasePricingActive =
                this.IsCorrectionManualPricingActive =
                this.IsCorrectionLastIssuedConsumptionPricingActive = false;

            switch (this.Entity.CorrectionPricingType)
            {
                case CorrectionPricingTypeEnum.Default:
                    this.IsCorrectionDefaultPricingActive = true;//this.Entity.EnableFinancialEditing;

                    this.Entity.CurrencyDto = null;
                    this.Entity.CorrectionPrice = null;
                    this.Entity.FuelReportCorrectionReferenceNoDto = null;

                    break;
                case CorrectionPricingTypeEnum.LastPurchasePrice:
                    this.IsCorrectionLastPurchasePricingActive = true;//this.Entity.EnableFinancialEditing;

                    this.Entity.CurrencyDto = null;
                    this.Entity.CorrectionPrice = null;
                    this.Entity.FuelReportCorrectionReferenceNoDto = null;

                    break;
                case CorrectionPricingTypeEnum.LastIssuedConsumption:
                    this.IsCorrectionLastIssuedConsumptionPricingActive = true;//this.Entity.EnableFinancialEditing;

                    this.Entity.CurrencyDto = null;
                    this.Entity.CorrectionPrice = null;

                    break;
                case CorrectionPricingTypeEnum.ManualPricing:
                    this.IsCorrectionManualPricingActive = true;// this.Entity.EnableFinancialEditing;

                    this.Entity.FuelReportCorrectionReferenceNoDto = null;

                    break;
            }
        }

        private void setCorrectionPricingTypes()
        {
            if (this.CorrectionPricingTypeItems == null)
                this.CorrectionPricingTypeItems = new ObservableCollection<ComboBoxItm>();
            else
                this.CorrectionPricingTypeItems.Clear();

            this.IsCorrectionPricingTypeActive = false;

            var items = typeof(CorrectionPricingTypeEnum).ToComboItemList();

            if (this.Entity.CorrectionType == CorrectionTypeEnum.Plus)
            {
                items.ForEach(i =>
                {
                    if ((i.Id == (long)CorrectionPricingTypeEnum.LastIssuedConsumption) ||
                        (i.Id == (long)CorrectionPricingTypeEnum.LastPurchasePrice) ||
                        (i.Id == (long)CorrectionPricingTypeEnum.ManualPricing))
                        this.CorrectionPricingTypeItems.Add(i);
                });

                this.IsCorrectionPricingTypeActive = this.Entity.EnableFinancialEditing;
            }
            else if (this.Entity.CorrectionType == CorrectionTypeEnum.Minus)
            {
                items.ForEach(i =>
                {
                    if ((i.Id == (long)CorrectionPricingTypeEnum.LastIssuedConsumption) ||
                        (i.Id == (long)CorrectionPricingTypeEnum.Default))
                        this.CorrectionPricingTypeItems.Add(i);
                });

                this.IsCorrectionPricingTypeActive = this.Entity.EnableFinancialEditing;
            }
        }

        private void setTransferReferences()
        {
            this.FuelReportTransferReferenceNoDtos.Clear();

            //if (this.Entity.TransferType == TransferTypeEnum.Rejected)
            //{
            //    if (this.Entity.RejectedTransferReferenceNoDtos != null)
            //        this.Entity.RejectedTransferReferenceNoDtos.ForEach(this.FuelReportTransferReferenceNoDtos.Add);
            //}
            //else
            //{
            //    if (this.Entity.FuelReportTransferReferenceNoDtos != null)
            //        this.Entity.FuelReportTransferReferenceNoDtos.ForEach(this.FuelReportTransferReferenceNoDtos.Add);
            //}

            if (this.Entity.TransferReferenceNoDtos != null && this.Entity.TransferReferenceNoDtos.ContainsKey(this.Entity.TransferType))
                this.Entity.TransferReferenceNoDtos[this.Entity.TransferType].ForEach(this.FuelReportTransferReferenceNoDtos.Add);
        }

        private void setReceiveReferences()
        {
            this.FuelReportReceiveReferenceNoDtos.Clear();

            //if (this.Entity.TransferType == TransferTypeEnum.Rejected)
            //{
            //    if (this.Entity.RejectedTransferReferenceNoDtos != null)
            //        this.Entity.RejectedTransferReferenceNoDtos.ForEach(this.FuelReportTransferReferenceNoDtos.Add);
            //}
            //else
            //{
            //    if (this.Entity.FuelReportTransferReferenceNoDtos != null)
            //        this.Entity.FuelReportTransferReferenceNoDtos.ForEach(this.FuelReportTransferReferenceNoDtos.Add);
            //}

            if (this.Entity.ReceiveReferenceNoDtos != null && this.Entity.ReceiveReferenceNoDtos.ContainsKey(this.Entity.ReceiveType))
                this.Entity.ReceiveReferenceNoDtos[this.Entity.ReceiveType].ForEach(this.FuelReportReceiveReferenceNoDtos.Add);
        }

        #endregion
    }
}
