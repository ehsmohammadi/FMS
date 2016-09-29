using System;
using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class FuelReportInventoryOperationDto : DTOBase
    {
        long id;
        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        string code;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "code can't be empty")]
        [DataMember]
        public string Code
        {
            get { return this.code; }
            set { if ((object.ReferenceEquals(this.Code, value) != true)) {this.code= value;}}
        }

        DateTime _actionDate;
        [DataMember]
        public DateTime ActionDate
        {
            get { return this._actionDate; }
            set { if ((object.ReferenceEquals(this.ActionDate, value) != true)) {this._actionDate= value;}}
        }

        string _actionType;
        //[Required(AllowEmptyStrings = false,ErrorMessage = "ActionType can't be empty")]
        [DataMember]
        public string ActionType
        {
            get { return this._actionType; }
            set { if ((object.ReferenceEquals(this.ActionType, value) != true)) {this._actionType= value;}}
        }

        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.GoodDto good;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.GoodDto Good
        {
            get { return this.good; }
            set { if ((object.ReferenceEquals(this.Good, value) != true)) {this.good= value;}}

        }
    }
}
