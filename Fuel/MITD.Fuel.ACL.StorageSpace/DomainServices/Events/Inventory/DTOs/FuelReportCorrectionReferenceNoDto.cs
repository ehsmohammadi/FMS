using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using ReferenceTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.ReferenceTypeEnum;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class FuelReportCorrectionReferenceNoDto : DTOBase
    {
        private long id;
        private string code;
        private string date;

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
        public string Date
        {
            get { return this.date; }
            set { if ((object.ReferenceEquals(this.Date, value) != true)) {this.date= value;}}
        }

        private ReferenceTypeEnum referenceType;
        [DataMember]
        public ReferenceTypeEnum ReferenceType
        {
            get { return this.referenceType; }
            set { if ((object.ReferenceEquals(this.ReferenceType, value) != true)) {this.referenceType= value;}}
        }
    }
}
