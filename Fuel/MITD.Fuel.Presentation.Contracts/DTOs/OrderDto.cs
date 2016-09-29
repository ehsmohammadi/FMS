#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts;
#endregion

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    // [DataContract]
    public partial class OrderDto
    {
        #region props

        private string userInChargName;
        private string code;
        private string currentStateName;
        private string description;
        private VesselInCompanyDto fromVesselInCompany;
        private long id;
        private DateTime orderDate;
        private OrderTypeEnum orderType;
        private CompanyDto _owner;
        private CompanyDto _receiver;
        private CompanyDto _supplier;
        private VesselInCompanyDto _toVesselInCompany;
        private CompanyDto _transporter;
        private WorkflowStageEnum approveStatus;
        private long _receiverId;

        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        public string Code
        {
            get { return code; }
            set { this.SetField(p => p.Code, ref code, value); }
        }

        public string UserInChargName
        {
            get { return userInChargName; }
            set { this.SetField(p => p.UserInChargName, ref userInChargName, value); }
        }

        public string CurrentStateName
        {
            get { return currentStateName; }
            set { this.SetField(p => p.CurrentStateName, ref currentStateName, value); }
        }

//        [Required(AllowEmptyStrings = false, ErrorMessage = "error")]
        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }

        public WorkflowStageEnum ApproveStatus
        {
            get { return approveStatus; }
            set { this.SetField(p => p.ApproveStatus, ref approveStatus, value); }
        }
        public string ApproveStatusString
        {
            get { return approveStatus.GetDescription(); }
        }

        public OrderTypeEnum OrderType
        {
            get { return orderType; }
            set { this.SetField(p => p.OrderType, ref orderType, value); }
        }

        public string OrderTypeString
        {
            get { return OrderType.GetDescription(); }
        }

        public DateTime OrderDate
        {
            get { return orderDate; }
            set { this.SetField(p => p.OrderDate, ref orderDate, value); }
        }

        public CompanyDto Supplier
        {
            get { return _supplier; }
            set { this.SetField(p => p.Supplier, ref _supplier, value); }
        }


        public CompanyDto Receiver
        {
            get { return _receiver; }
            set
            {
                this.SetField(p => p.Receiver, ref _receiver, value);
            }
        }

        public VesselInCompanyDto FromVesselInCompany
        {
            get { return fromVesselInCompany; }
            set { this.SetField(p => p.FromVesselInCompany, ref fromVesselInCompany, value); }
        }

        public VesselInCompanyDto ToVesselInCompany
        {
            get { return _toVesselInCompany; }
            set { this.SetField(p => p.ToVesselInCompany, ref _toVesselInCompany, value); }
        }

        public CompanyDto Owner
        {
            get { return _owner; }
            set { this.SetField(p => p.Owner, ref _owner, value); }
        }

        public CompanyDto Transporter
        {
            get { return _transporter; }
            set { this.SetField(p => p.Transporter, ref _transporter, value); }
        }

        #endregion

        private ObservableCollection<OrderItemDto> orderItemDto;
        public ObservableCollection<OrderItemDto> OrderItems
        {
            get { return orderItemDto; }
            set { this.SetField(p => p.OrderItems, ref orderItemDto, value); }
        }

        private ObservableCollection<OrderAssignmentReferenceDto> destinationReferences;
        public ObservableCollection<OrderAssignmentReferenceDto> DestinationReferences
        {
            get { return destinationReferences; }
            set { this.SetField(p => p.DestinationReferences, ref this.destinationReferences , value); }
        }
    }
}