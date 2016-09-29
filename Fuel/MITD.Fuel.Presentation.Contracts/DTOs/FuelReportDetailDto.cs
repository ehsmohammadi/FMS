using System.Collections.Generic;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class FuelReportDetailDto
    {
        private GoodDto good;
        public virtual GoodDto Good
        {
            get { return good; }
            set
            {
                this.SetField(p => p.Good, ref good, value);
            }
        }

        /// <summary>
        /// There are no comments for ReceiveTypeId in the schema.
        /// </summary>
        public virtual long Id
        {
            get
            {
                return _id;
            }
            set
            {
                this.SetField(c => c.Id, ref _id, value);
            }
        }
        private long _id;



        /// <summary>
        /// There are no comments for Consumption in the schema.
        /// </summary>
        public virtual decimal? Consumption
        {
            get
            {
                return _consumption;
            }
            set
            {
                this.SetField(c => c.Consumption, ref _consumption, value);
            }
        }
        private decimal? _consumption;


        /// <summary>
        /// There are no comments for Correction in the schema.
        /// </summary>
        public virtual decimal? Correction
        {
            get
            {
                return _correction;
            }
            set
            {
                this.SetField(c => c.Correction, ref _correction, value);
            }
        }
        private decimal? _correction;


        /// <summary>
        /// There are no comments for CorrectionPrice in the schema.
        /// </summary>
        public virtual decimal? CorrectionPrice
        {
            get
            {
                return _correctionPrice;
            }
            set
            {
                this.SetField(c => c.CorrectionPrice, ref _correctionPrice, value);
            }
        }
        private decimal? _correctionPrice;


        /// <summary>
        /// There are no comments for CorrectionType in the schema.
        /// </summary>
        public virtual CorrectionTypeEnum CorrectionType
        {
            get
            {
                return _correctionType;
            }
            set
            {
                this.SetField(c => c.CorrectionType, ref _correctionType, value);
            }
        }
        private CorrectionTypeEnum _correctionType;

        public virtual CorrectionPricingTypeEnum CorrectionPricingType
        {
            get
            {
                return _correctionPricingType;
            }
            set
            {
                this.SetField(c => c.CorrectionPricingType, ref _correctionPricingType, value);
            }
        }
        private CorrectionPricingTypeEnum _correctionPricingType;


        /// <summary>
        /// There are no comments for Recieve in the schema.
        /// </summary>
        public virtual decimal? Recieve
        {
            get
            {
                return _recieve;
            }
            set
            {
                this.SetField(c => c.Recieve, ref _recieve, value);
            }
        }
        private decimal? _recieve;


        /// <summary>
        /// There are no comments for ROB in the schema.
        /// </summary>


        //[CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
        public virtual decimal ROB
        {
            get
            {
                return _rob;
            }
            set
            {
                this.SetField(c => c.ROB, ref _rob, value);
            }
        }
        private decimal _rob;

        public virtual decimal? PreviousROB
        {
            get
            {
                return _PreviousROB;
            }
            set
            {
                this.SetField(c => c.PreviousROB, ref _PreviousROB, value);
            }
        }
        private decimal? _PreviousROB;


        /// <summary>
        /// There are no comments for Transfer in the schema.
        /// </summary>
        public virtual decimal? Transfer
        {
            get
            {
                return _transfer;
            }
            set
            {
                this.SetField(c => c.Transfer, ref _transfer, value);
            }
        }
        private decimal? _transfer;


        /// <summary>
        /// There are no comments for FuelReportId in the schema.
        /// </summary>
        public virtual long FuelReportId
        {
            get
            {
                return _fuelReportId;
            }
            set
            {
                this.SetField(c => c.FuelReportId, ref _fuelReportId, value);
            }
        }
        private long _fuelReportId;


        /// <summary>
        /// There are no comments for ReceiveTypeId in the schema.
        /// </summary>
        public virtual ReceiveTypeEnum ReceiveType
        {
            get
            {
                return _receiveType;
            }
            set
            {
                this.SetField(c => c.ReceiveType, ref _receiveType, value);
            }
        }
        private ReceiveTypeEnum _receiveType;


        /// <summary>
        /// There are no comments for TransferTypeId in the schema.
        /// </summary>
        public virtual TransferTypeEnum TransferType
        {
            get
            {
                return _transferType;
            }
            set
            {
                this.SetField(c => c.TransferType, ref _transferType, value);
            }
        }
        private TransferTypeEnum _transferType;




        public virtual long? GoodId
        {
            get
            {
                return _goodId;
            }
            set
            {
                this.SetField(c => c.GoodId, ref _goodId, value);
            }
        }
        private long? _goodId;

        public virtual long? TankId
        {
            get
            {
                return _tankId;
            }
            set
            {
                this.SetField(c => c.TankId, ref _tankId, value);
            }
        }
        private long? _tankId;

        public virtual long? GoodUnitId
        {
            get
            {
                return _goodUnitId;
            }
            set
            {
                this.SetField(c => c.GoodUnitId, ref _goodUnitId, value);
            }
        }
        private long? _goodUnitId;




        private FuelReportTransferReferenceNoDto fuelReportTransferReferenceNoDto;
        public FuelReportTransferReferenceNoDto FuelReportTransferReferenceNoDto
        {
            get { return fuelReportTransferReferenceNoDto; }
            set { this.SetField(p => p.FuelReportTransferReferenceNoDto, ref fuelReportTransferReferenceNoDto, value); }
        }

        //private List<FuelReportTransferReferenceNoDto> fuelReportTransferReferenceNoDtos;
        //public List<FuelReportTransferReferenceNoDto> FuelReportTransferReferenceNoDtos
        //{
        //    get { return fuelReportTransferReferenceNoDtos; }
        //    set { this.SetField(p => p.FuelReportTransferReferenceNoDtos, ref fuelReportTransferReferenceNoDtos, value); }
        //}

        private Dictionary<TransferTypeEnum, List<FuelReportTransferReferenceNoDto>> transferReferenceNoDtos;
        public Dictionary<TransferTypeEnum, List<FuelReportTransferReferenceNoDto>> TransferReferenceNoDtos
        {
            get { return transferReferenceNoDtos; }
            set { this.SetField(p => p.TransferReferenceNoDtos, ref transferReferenceNoDtos, value); }
        }

        //private List<FuelReportTransferReferenceNoDto> internalTransferTransferReferenceNoDtos;
        //public List<FuelReportTransferReferenceNoDto> InternalTransferTransferReferenceNoDtos
        //{
        //    get { return internalTransferTransferReferenceNoDtos; }
        //    set { this.SetField(p => p.InternalTransferTransferReferenceNoDtos, ref internalTransferTransferReferenceNoDtos, value); }
        //}

        //private List<FuelReportTransferReferenceNoDto> saleTransferTransferReferenceNoDtos;
        //public List<FuelReportTransferReferenceNoDto> SaleTransferTransferReferenceNoDtos
        //{
        //    get { return saleTransferTransferReferenceNoDtos; }
        //    set { this.SetField(p => p.SaleTransferTransferReferenceNoDtos, ref saleTransferTransferReferenceNoDtos, value); }
        //}

        //private List<FuelReportTransferReferenceNoDto> rejectedTransferReferenceNoDtos;
        //public List<FuelReportTransferReferenceNoDto> RejectedTransferReferenceNoDtos
        //{
        //    get { return rejectedTransferReferenceNoDtos; }
        //    set { this.SetField(p => p.RejectedTransferReferenceNoDtos, ref rejectedTransferReferenceNoDtos, value); }
        //}

        private FuelReportCorrectionReferenceNoDto fuelReportCorrectionReferenceNoDto;
        public FuelReportCorrectionReferenceNoDto FuelReportCorrectionReferenceNoDto
        {
            get { return fuelReportCorrectionReferenceNoDto; }
            set { this.SetField(p => p.FuelReportCorrectionReferenceNoDto, ref fuelReportCorrectionReferenceNoDto, value); }
        }

        private List<FuelReportCorrectionReferenceNoDto> correctionReferenceNoDtos;
        public List<FuelReportCorrectionReferenceNoDto> CorrectionReferenceNoDtos
        {
            get { return correctionReferenceNoDtos; }
            set { this.SetField(p => p.CorrectionReferenceNoDtos, ref correctionReferenceNoDtos, value); }
        }


        private FuelReportReceiveReferenceNoDto fuelReportReceiveReferenceNoDto;
        public FuelReportReceiveReferenceNoDto FuelReportReceiveReferenceNoDto
        {
            get { return fuelReportReceiveReferenceNoDto; }
            set { this.SetField(p => p.FuelReportReceiveReferenceNoDto, ref fuelReportReceiveReferenceNoDto, value); }
        }

        //private List<FuelReportReceiveReferenceNoDto> FuelReportReceiveReferenceNoDtos;
        //public List<FuelReportReceiveReferenceNoDto> FuelReportReceiveReferenceNoDtos
        //{
        //    get { return FuelReportReceiveReferenceNoDtos; }
        //    set { this.SetField(p => p.FuelReportReceiveReferenceNoDtos, ref FuelReportReceiveReferenceNoDtos, value); }
        //}

        private Dictionary<ReceiveTypeEnum, List<FuelReportReceiveReferenceNoDto>> receiveReferenceNoDtos;
        public Dictionary<ReceiveTypeEnum, List<FuelReportReceiveReferenceNoDto>> ReceiveReferenceNoDtos
        {
            get { return receiveReferenceNoDtos; }
            set { this.SetField(p => p.ReceiveReferenceNoDtos, ref receiveReferenceNoDtos, value); }
        }

        //private List<FuelReportReceiveReferenceNoDto> internalTransferReceiveReferenceNoDtos;
        //public List<FuelReportReceiveReferenceNoDto> InternalTransferReceiveReferenceNoDtos
        //{
        //    get { return internalTransferReceiveReferenceNoDtos; }
        //    set { this.SetField(p => p.InternalTransferReceiveReferenceNoDtos, ref internalTransferReceiveReferenceNoDtos, value); }
        //}

        //private List<FuelReportReceiveReferenceNoDto> transferPurchaseReceiveReferenceNoDtos;
        //public List<FuelReportReceiveReferenceNoDto> TransferPurchaseReceiveReferenceNoDtos
        //{
        //    get { return transferPurchaseReceiveReferenceNoDtos; }
        //    set { this.SetField(p => p.TransferPurchaseReceiveReferenceNoDtos, ref transferPurchaseReceiveReferenceNoDtos, value); }
        //}
 
        //private List<FuelReportReceiveReferenceNoDto> purchaseReceiveReferenceNoDtos;
        //public List<FuelReportReceiveReferenceNoDto> PurchaseReceiveReferenceNoDtos
        //{
        //    get { return purchaseReceiveReferenceNoDtos; }
        //    set { this.SetField(p => p.TransferPurchaseReceiveReferenceNoDtos, ref purchaseReceiveReferenceNoDtos, value); }
        //}

        private CurrencyDto currencyDto;
        public CurrencyDto CurrencyDto
        {
            get { return currencyDto; }
            set { this.SetField(p => p.CurrencyDto, ref currencyDto, value); }
        }

        public bool EnableCommercialEditing
        {
            get { return enableCommercialEditing; }
            set { this.SetField(p => p.EnableCommercialEditing, ref enableCommercialEditing, value); }
        }

        private bool enableCommercialEditing;

        public bool EnableFinancialEditing
        {
            get { return enableFinancialEditing; }
            set { this.SetField(p => p.EnableFinancialEditing, ref enableFinancialEditing, value); }
        }

        private bool enableFinancialEditing;


        private bool isTheTrustIssueQuantityAssignedTo;
        public bool IsTheTrustIssueQuantityAssignedTo
        {
            get { return this.isTheTrustIssueQuantityAssignedTo; }
            set { this.SetField(p => p.IsTheTrustIssueQuantityAssignedTo, ref this.isTheTrustIssueQuantityAssignedTo, value); }
        }

        private bool isTrustIssueQuantityAssignmentPossible;
        public bool IsTrustIssueQuantityAssignmentPossible
        {
            get { return this.isTrustIssueQuantityAssignmentPossible; }
            set { this.SetField(p => p.IsTrustIssueQuantityAssignmentPossible, ref this.isTrustIssueQuantityAssignmentPossible, value); }
        }

        private InventoryResultItemDto trustIssueInventoryResultItem;
        public InventoryResultItemDto TrustIssueInventoryResultItem
        {
            get { return this.trustIssueInventoryResultItem; }
            set { this.SetField(p => p.TrustIssueInventoryResultItem, ref this.trustIssueInventoryResultItem, value); }
        }

        private long? trustIssueInventoryTransactionItemId;
        public long? TrustIssueInventoryTransactionItemId
        {
            get { return this.trustIssueInventoryTransactionItemId; }
            set { this.SetField(p => p.TrustIssueInventoryTransactionItemId, ref this.trustIssueInventoryTransactionItemId, value); }
        }

        public List<FuelReportInventoryOperationDto> InventoryOperationDtos
        {
            get { return this.inventoryOperationDtos; }
            set { this.SetField(p => p.InventoryOperationDtos, ref this.inventoryOperationDtos, value); }
        }

        private List<FuelReportInventoryOperationDto> inventoryOperationDtos;
    }
}
