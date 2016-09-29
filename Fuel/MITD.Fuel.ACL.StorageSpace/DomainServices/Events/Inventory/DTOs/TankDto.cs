using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class TankDto : DTOBase
    {
        private long id;
        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        private string code;
        [DataMember]
        public string Code
        {
            get { return this.code; }
            set { if ((object.ReferenceEquals(this.Code, value) != true)) {this.code= value;}}
        }


        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto vesselDto;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto VesselDto
        {
            get { return this.vesselDto; }
            set { if ((object.ReferenceEquals(this.VesselDto, value) != true)) {this.vesselDto= value;}}
        }

    }
}
