using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using CharterEndTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.CharterEndTypeEnum;
using CharterStateTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.CharterStateTypeEnum;
using CharterType = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.CharterType;
using OffHirePricingType = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.OffHirePricingType;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public class CharterDto : DTOBase
    {
        private string _CurrentStateName;
        private CharterStateTypeEnum _charterStateType;
        private CharterType _charterType;
        private CharterEndTypeEnum _charterTypeEndEnum;
        private CompanyDto _charterer;
        private CurrencyDto _currencyDto;
        private DateTime _endDate;
        private string _endDateStr;
        private ObservableCollection<FuelReportInventoryOperationDto> _inventoryOperationDtos;
        private bool _isFinalApprove;
        private OffHirePricingType _offHirePricingType;
        private CompanyDto _owner;
        private DateTime _startDate;
        private ObservableCollection<CharterItemDto> charterItems;
        private long id;
        private VesselDto vessel;

        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((ReferenceEquals(this.Id, value) != true)) { this.id = value; } }
        }

        [DataMember]
        public CharterType CharterType
        {
            get { return this._charterType; }
            set { if ((ReferenceEquals(this.CharterType, value) != true)) { this._charterType = value; } }
        }

        [DataMember]
        public CompanyDto Charterer
        {
            get { return this._charterer; }
            set { if ((ReferenceEquals(this.Charterer, value) != true)) { this._charterer = value; } }
        }

        [DataMember]
        public CompanyDto Owner
        {
            get { return this._owner; }
            set { if ((ReferenceEquals(this.Owner, value) != true)) { this._owner = value; } }
        }

        [DataMember]
        public DateTime StartDate
        {
            get { return this._startDate; }
            set { if ((ReferenceEquals(this.StartDate, value) != true)) { this._startDate = value; } }
        }

        [DataMember]
        public DateTime EndDate
        {
            get { return this._endDate; }
            set { if ((ReferenceEquals(this.EndDate, value) != true)) { this._endDate = value; } }
        }


        [DataMember]
        public string EndDateStr
        {
            get { return this._endDateStr; }
            set { if ((ReferenceEquals(this.EndDateStr, value) != true)) { this._endDateStr = value; } }
        }

        [DataMember]
        public VesselDto Vessel
        {
            get { return this.vessel; }
            set { if ((ReferenceEquals(this.Vessel, value) != true)) { this.vessel = value; } }
        }

        [DataMember]
        public CurrencyDto Currency
        {
            get { return this._currencyDto; }
            set { if ((ReferenceEquals(this.Currency, value) != true)) { this._currencyDto = value; } }
        }

        [DataMember]
        public CharterStateTypeEnum CharterStateType
        {
            get { return this._charterStateType; }
            set { if ((ReferenceEquals(this.CharterStateType, value) != true)) { this._charterStateType = value; } }
        }

        [DataMember]
        public OffHirePricingType OffHirePricingType
        {
            get { return this._offHirePricingType; }
            set { if ((ReferenceEquals(this.OffHirePricingType, value) != true)) { this._offHirePricingType = value; } }
        }

        [DataMember]
        public CharterEndTypeEnum CharterEndType
        {
            get { return this._charterTypeEndEnum; }
            set { if ((ReferenceEquals(this.CharterEndType, value) != true)) { this._charterTypeEndEnum = value; } }
        }

        [DataMember]
        public string CurrentStateName
        {
            get { return this._CurrentStateName; }
            set { if ((ReferenceEquals(this.CurrentStateName, value) != true)) { this._CurrentStateName = value; } }
        }


        [DataMember]
        public bool IsFinalApproveVisiblity
        {
            get { return this._isFinalApprove; }
            set { if ((ReferenceEquals(this.IsFinalApproveVisiblity, value) != true)) { this._isFinalApprove = value; } }
        }

        [DataMember]
        public ObservableCollection<CharterItemDto> CharterItems
        {
            get { return this.charterItems; }
            set { if ((ReferenceEquals(this.CharterItems, value) != true)) { this.charterItems = value; } }
        }

        [DataMember]
        public ObservableCollection<FuelReportInventoryOperationDto> InventoryOperationDtos
        {
            get { return this._inventoryOperationDtos; }
            set { if ((ReferenceEquals(this.InventoryOperationDtos, value) != true)) { this._inventoryOperationDtos = value; } }
        }
    }
}
