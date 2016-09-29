using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using OrderTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.OrderTypeEnum;
using WorkflowStageEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.WorkflowStageEnum;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class OrderDto : DTOBase
    {
        #region props

        private string userInChargName;
        private string code;
        private string currentStateName;
        private string description;
        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto fromVessel;
        private long id;
        private DateTime? orderDate;
        private OrderTypeEnum orderType;
        private CompanyDto _owner;
        private CompanyDto _receiver;
        private CompanyDto _supplier;
        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto _toVessel;
        private CompanyDto _transporter;
        private WorkflowStageEnum approveStatus;

        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        [DataMember]
        public string Code
        {
            get { return this.code; }
            set { if ((object.ReferenceEquals(this.Code, value) != true)) {this.code= value;}}
        }

        [DataMember]
        public string UserInChargName
        {
            get { return this.userInChargName; }
            set { if ((object.ReferenceEquals(this.UserInChargName, value) != true)) {this.userInChargName= value;}}
        }

        [DataMember]
        public string CurrentStateName
        {
            get { return this.currentStateName; }
            set { if ((object.ReferenceEquals(this.CurrentStateName, value) != true)) {this.currentStateName= value;}}
        }

//        [Required(AllowEmptyStrings = false, ErrorMessage = "error")]
        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { if ((object.ReferenceEquals(this.Description, value) != true)) {this.description= value;}}
        }

        [DataMember]
        public WorkflowStageEnum ApproveStatus
        {
            get { return this.approveStatus; }
            set { if ((object.ReferenceEquals(this.ApproveStatus, value) != true)) {this.approveStatus= value;}}
        }
        [DataMember]
        public string ApproveStatusString
        {
            //get { return approveStatus.GetDescription(); }
            get { return this.approveStatus.ToString(); }
        }

        [DataMember]
        public OrderTypeEnum OrderType
        {
            get { return this.orderType; }
            set { if ((object.ReferenceEquals(this.OrderType, value) != true)) {this.orderType= value;}}
        }

        [DataMember]
        public string OrderTypeString
        {
            //get { return OrderType.GetDescription(); }
            get { return this.OrderType.ToString(); }
        }

        [DataMember]
        public DateTime? OrderDate
        {
            get { return this.orderDate; }
            set { if ((object.ReferenceEquals(this.OrderDate, value) != true)) {this.orderDate= value;}}
        }

        [DataMember]
        public CompanyDto Supplier
        {
            get { return this._supplier; }
            set { if ((object.ReferenceEquals(this.Supplier, value) != true)) {this._supplier= value;}}
        }


        [DataMember]
        public CompanyDto Receiver
        {
            get { return this._receiver; }
            set
            {
                if ((object.ReferenceEquals(this.Receiver, value) != true)) {this._receiver= value;}
            }
        }

        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto FromVessel
        {
            get { return this.fromVessel; }
            set { if ((object.ReferenceEquals(this.FromVessel, value) != true)) {this.fromVessel= value;}}
        }

        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto ToVessel
        {
            get { return this._toVessel; }
            set { if ((object.ReferenceEquals(this.ToVessel, value) != true)) {this._toVessel= value;}}
        }

        [DataMember]
        public CompanyDto Owner
        {
            get { return this._owner; }
            set { if ((object.ReferenceEquals(this.Owner, value) != true)) {this._owner= value;}}
        }

        [DataMember]
        public CompanyDto Transporter
        {
            get { return this._transporter; }
            set { if ((object.ReferenceEquals(this.Transporter, value) != true)) {this._transporter= value;}}
        }

        #endregion

        private ObservableCollection<ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.OrderItemDto> orderItemDto;
        private long _receiverId;

        [DataMember]
        public ObservableCollection<ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.OrderItemDto> OrderItems
        {
            get { return this.orderItemDto; }
            set { if ((object.ReferenceEquals(this.OrderItems, value) != true)) {this.orderItemDto= value;}}
        }

    }
}