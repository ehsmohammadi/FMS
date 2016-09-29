using System.Collections.Generic;
using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class CompanyDto : DTOBase
    {
        long id;
        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        string code;
        [DataMember]
        public string Code
        {
            get { return this.code; }
            set { if ((object.ReferenceEquals(this.Code, value) != true)) {this.code= value;}}
        }

        string name;
        [DataMember]
        public string Name
        {
            get { return this.name; }
            set { if ((object.ReferenceEquals(this.Name, value) != true)) {this.name= value;}}
        }

        List<ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto> _vessels;
        [DataMember]
        public List<ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto> Vessels
        {
            get { return this._vessels; }
            set { if ((object.ReferenceEquals(this.Vessels, value) != true)) {this._vessels= value;}}
        }

        public CompanyDto()
        {
            this._vessels = new List<ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.VesselDto>();
        }
    }
}