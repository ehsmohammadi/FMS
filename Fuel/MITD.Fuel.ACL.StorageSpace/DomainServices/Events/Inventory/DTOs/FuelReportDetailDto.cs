using System.Collections.Generic;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using CorrectionTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.CorrectionTypeEnum;
using ReceiveTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.ReceiveTypeEnum;
using TransferTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.TransferTypeEnum;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public class FuelReportDetailDto : DTOBase
    {
        private decimal? _consumption;
        private decimal? _correction;
        private double? _correctionPrice;
        private CorrectionTypeEnum _correctionType;
        private FuelReportDto _fuelReport;
        private long _fuelReportId;
        private long? _goodId;
        private long? _goodUnitId;
        private long _id;
        private ReceiveTypeEnum _receiveType;
        private decimal? _recieve;
        private decimal _rob;
        private TankDto _tank;
        private long? _tankId;
        private decimal? _transfer;
        private TransferTypeEnum _transferType;
        private CurrencyDto currencyDto;
        private FuelReportCorrectionReferenceNoDto fuelReportCorrectionReferenceNoDto;
        private List<FuelReportCorrectionReferenceNoDto> fuelReportCorrectionReferenceNoDtos;
        private FuelReportReciveReferenceNoDto fuelReportReciveReferenceNoDto;
        private List<FuelReportReciveReferenceNoDto> fuelReportReciveReferenceNoDtos;
        private FuelReportTransferReferenceNoDto fuelReportTransferReferenceNoDto;
        private List<FuelReportTransferReferenceNoDto> fuelReportTransferReferenceNoDtos;
        private GoodDto good;
        private List<FuelReportTransferReferenceNoDto> rejectedTransferReferenceNoDtos;

        [DataMember]
        public GoodDto Good
        {
            get { return this.good; }
            set { if ((ReferenceEquals(this.Good, value) != true)) { this.good = value; } }
        }

        /// <summary>
        ///     There are no comments for ReceiveTypeId in the schema.
        /// </summary>
        [DataMember]
        public long Id
        {
            get { return this._id; }
            set { if ((ReferenceEquals(this.Id, value) != true)) { this._id = value; } }
        }


        /// <summary>
        ///     There are no comments for Consumption in the schema.
        /// </summary>
        [DataMember]
        public decimal? Consumption
        {
            get { return this._consumption; }
            set { if ((ReferenceEquals(this.Consumption, value) != true)) { this._consumption = value; } }
        }


        /// <summary>
        ///     There are no comments for Correction in the schema.
        /// </summary>
        [DataMember]
        public decimal? Correction
        {
            get { return this._correction; }
            set { if ((ReferenceEquals(this.Correction, value) != true)) { this._correction = value; } }
        }


        /// <summary>
        ///     There are no comments for CorrectionPrice in the schema.
        /// </summary>
        [DataMember]
        public double? CorrectionPrice
        {
            get { return this._correctionPrice; }
            set { if ((ReferenceEquals(this.CorrectionPrice, value) != true)) { this._correctionPrice = value; } }
        }


        /// <summary>
        ///     There are no comments for CorrectionType in the schema.
        /// </summary>
        [DataMember]
        public CorrectionTypeEnum CorrectionType
        {
            get { return this._correctionType; }
            set { if ((ReferenceEquals(this.CorrectionType, value) != true)) { this._correctionType = value; } }
        }


        /// <summary>
        ///     There are no comments for Recieve in the schema.
        /// </summary>
        [DataMember]
        public decimal? Recieve
        {
            get { return this._recieve; }
            set { if ((ReferenceEquals(this.Recieve, value) != true)) { this._recieve = value; } }
        }


        /// <summary>
        ///     There are no comments for ROB in the schema.
        /// </summary>
        [DataMember]
        public decimal ROB
        {
            get { return this._rob; }
            set { if ((ReferenceEquals(this.ROB, value) != true)) { this._rob = value; } }
        }


        /// <summary>
        ///     There are no comments for Transfer in the schema.
        /// </summary>
        [DataMember]
        public decimal? Transfer
        {
            get { return this._transfer; }
            set { if ((ReferenceEquals(this.Transfer, value) != true)) { this._transfer = value; } }
        }


        /// <summary>
        ///     There are no comments for FuelReportId in the schema.
        /// </summary>
        [DataMember]
        public long FuelReportId
        {
            get { return this._fuelReportId; }
            set { if ((ReferenceEquals(this.FuelReportId, value) != true)) { this._fuelReportId = value; } }
        }

        [DataMember]
        public FuelReportDto FuelReport
        {
            get { return this._fuelReport; }
            set { if ((ReferenceEquals(this.FuelReport, value) != true)) { this._fuelReport = value; } }
        }

        /// <summary>
        ///     There are no comments for ReceiveTypeId in the schema.
        /// </summary>
        [DataMember]
        public ReceiveTypeEnum ReceiveType
        {
            get { return this._receiveType; }
            set { if ((ReferenceEquals(this.ReceiveType, value) != true)) { this._receiveType = value; } }
        }


        /// <summary>
        ///     There are no comments for TransferTypeId in the schema.
        /// </summary>
        [DataMember]
        public TransferTypeEnum TransferType
        {
            get { return this._transferType; }
            set { if ((ReferenceEquals(this.TransferType, value) != true)) { this._transferType = value; } }
        }


        [DataMember]
        public long? GoodId
        {
            get { return this._goodId; }
            set { if ((ReferenceEquals(this.GoodId, value) != true)) { this._goodId = value; } }
        }

        [DataMember]
        public long? TankId
        {
            get { return this._tankId; }
            set { if ((ReferenceEquals(this.TankId, value) != true)) { this._tankId = value; } }
        }

        [DataMember]
        public TankDto Tank
        {
            get { return this._tank; }
            set { if ((ReferenceEquals(this.Tank, value) != true)) { this._tank = value; } }
        }

        [DataMember]
        public long? GoodUnitId
        {
            get { return this._goodUnitId; }
            set { if ((ReferenceEquals(this.GoodUnitId, value) != true)) { this._goodUnitId = value; } }
        }

        [DataMember]
        public FuelReportTransferReferenceNoDto FuelReportTransferReferenceNoDto
        {
            get { return this.fuelReportTransferReferenceNoDto; }
            set { if ((ReferenceEquals(this.FuelReportTransferReferenceNoDto, value) != true)) { this.fuelReportTransferReferenceNoDto = value; } }
        }

        [DataMember]
        public List<FuelReportTransferReferenceNoDto> FuelReportTransferReferenceNoDtos
        {
            get { return this.fuelReportTransferReferenceNoDtos; }
            set { if ((ReferenceEquals(this.FuelReportTransferReferenceNoDtos, value) != true)) { this.fuelReportTransferReferenceNoDtos = value; } }
        }

        [DataMember]
        public List<FuelReportTransferReferenceNoDto> RejectedTransferReferenceNoDtos
        {
            get { return this.rejectedTransferReferenceNoDtos; }
            set { if ((ReferenceEquals(this.RejectedTransferReferenceNoDtos, value) != true)) { this.rejectedTransferReferenceNoDtos = value; } }
        }

        [DataMember]
        public FuelReportCorrectionReferenceNoDto FuelReportCorrectionReferenceNoDto
        {
            get { return this.fuelReportCorrectionReferenceNoDto; }
            set { if ((ReferenceEquals(this.FuelReportCorrectionReferenceNoDto, value) != true)) { this.fuelReportCorrectionReferenceNoDto = value; } }
        }

        [DataMember]
        public List<FuelReportCorrectionReferenceNoDto> FuelReportCorrectionReferenceNoDtos
        {
            get { return this.fuelReportCorrectionReferenceNoDtos; }
            set { if ((ReferenceEquals(this.FuelReportCorrectionReferenceNoDtos, value) != true)) { this.fuelReportCorrectionReferenceNoDtos = value; } }
        }


        [DataMember]
        public FuelReportReciveReferenceNoDto FuelReportReciveReferenceNoDto
        {
            get { return this.fuelReportReciveReferenceNoDto; }
            set { if ((ReferenceEquals(this.FuelReportReciveReferenceNoDto, value) != true)) { this.fuelReportReciveReferenceNoDto = value; } }
        }

        [DataMember]
        public List<FuelReportReciveReferenceNoDto> FuelReportReciveReferenceNoDtos
        {
            get { return this.fuelReportReciveReferenceNoDtos; }
            set { if ((ReferenceEquals(this.FuelReportReciveReferenceNoDtos, value) != true)) { this.fuelReportReciveReferenceNoDtos = value; } }
        }

        [DataMember]
        public CurrencyDto CurrencyDto
        {
            get { return this.currencyDto; }
            set { if ((ReferenceEquals(this.CurrencyDto, value) != true)) { this.currencyDto = value; } }
        }
    }
}
