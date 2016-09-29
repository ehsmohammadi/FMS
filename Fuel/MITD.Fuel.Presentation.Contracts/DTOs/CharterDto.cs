using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class CharterDto
    {
        private long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        private CompanyDto _charterer;

        public CompanyDto Charterer
        {
            get { return _charterer; }
            set { this.SetField(p => p.Charterer, ref _charterer, value); }
        }

        private CompanyDto _owner;
        public CompanyDto Owner
        {
            get { return _owner; }
            set { this.SetField(p => p.Owner, ref _owner, value); }
        }

        DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { this.SetField(p => p.StartDate, ref _startDate, value); }
        }

        DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { this.SetField(p => p.EndDate, ref _endDate, value); }
        }


        string _endDateStr;
        public string EndDateStr
        {
            get { return _endDateStr; }
            set { this.SetField(p => p.EndDateStr, ref _endDateStr, value); }
        }

        private VesselInCompanyDto vesselInCompany;
        public VesselInCompanyDto VesselInCompany
        {
            get { return vesselInCompany; }
            set { this.SetField(p => p.VesselInCompany, ref vesselInCompany, value); }
        }

        private CurrencyDto _currencyDto;
        public CurrencyDto Currency
        {
            get { return _currencyDto; }
            set { this.SetField(p => p.Currency, ref _currencyDto, value); }
        }

        private CharterStateTypeEnum _charterStateType;
        public CharterStateTypeEnum CharterStateType
        {
            get { return _charterStateType; }
            set { this.SetField(p => p.CharterStateType, ref _charterStateType, value); }
        }

        private OffHirePricingType _offHirePricingType;
        public OffHirePricingType OffHirePricingType
        {
            get { return _offHirePricingType; }
            set { this.SetField(p => p.OffHirePricingType, ref _offHirePricingType, value); }
        }
        
        private CharterEndTypeEnum _charterTypeEndEnum;
        public CharterEndTypeEnum CharterEndType
        {
            get { return _charterTypeEndEnum; }
            set { this.SetField(p => p.CharterEndType, ref _charterTypeEndEnum, value); }
        }

        string _CurrentStateName;
        public string CurrentStateName
        {
            get { return _CurrentStateName; }
            set { this.SetField(p => p.CurrentStateName, ref _CurrentStateName, value); }
        }




        bool _isFinalApprove;
        public bool IsFinalApproveVisiblity
        {
            get { return _isFinalApprove; }
            set { this.SetField(p => p.IsFinalApproveVisiblity, ref _isFinalApprove, value); }
        }

        bool _isRejectedApprove;
        public bool IsRejectedApproveVisiblity
        {
            get { return _isRejectedApprove; }
            set { this.SetField(p => p.IsRejectedApproveVisiblity, ref _isRejectedApprove, value); }
        }


        private ObservableCollection<CharterItemDto> charterItems;
        public ObservableCollection<CharterItemDto> CharterItems
        {
            get { return this.charterItems; }
            set { this.SetField(p => p.CharterItems, ref this.charterItems, value); }
        }
        private ObservableCollection<FuelReportInventoryOperationDto> _inventoryOperationDtos;
        public ObservableCollection<FuelReportInventoryOperationDto> InventoryOperationDtos
        {
            get { return _inventoryOperationDtos; }
            set { this.SetField(p => p.InventoryOperationDtos, ref _inventoryOperationDtos, value); }
        }

        private WorkflowStageEnum approveStatus;
        public WorkflowStageEnum ApproveStatus
        {
            get { return approveStatus; }
            set { this.SetField(p => p.ApproveStatus, ref approveStatus, value); }
        }
    }
}
