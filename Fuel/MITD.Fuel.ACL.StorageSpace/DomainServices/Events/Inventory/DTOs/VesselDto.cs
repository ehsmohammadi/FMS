using System.Collections.Generic;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using VesselStateEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.VesselStateEnum;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public class VesselDto : DTOBase
    {
        private string _code;
        private string _description;
        private string _name;
        private List<TankDto> _tankDtos;
        private CompanyDto company;
        private long companyId;
        private long id;
        private VesselStateEnum vesselState;

        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((ReferenceEquals(this.Id, value) != true)) { this.id = value; } }
        }

        [DataMember]
        public string Code
        {
            get { return this._code; }
            set { if ((ReferenceEquals(this.Code, value) != true)) { this._code = value; } }
        }

        [DataMember]
        public string Name
        {
            get { return this._code; }
            set { if ((ReferenceEquals(this.Name, value) != true)) { this._code = value; } }
        }

        [DataMember]
        public string Description
        {
            get { return this._description; }
            set { if ((ReferenceEquals(this.Description, value) != true)) { this._description = value; } }
        }

        [DataMember]
        public CompanyDto Company
        {
            get { return this.company; }
            set { if ((ReferenceEquals(this.Company, value) != true)) { this.company = value; } }
        }

        [DataMember]
        public long CompanyId
        {
            get { return this.companyId; }
            set { if ((ReferenceEquals(this.CompanyId, value) != true)) { this.companyId = value; } }
        }

        [DataMember]
        public VesselStateEnum VesselState
        {
            get { return this.vesselState; }
            set { if ((ReferenceEquals(this.VesselState, value) != true)) { this.vesselState = value; } }
        }

        [DataMember]
        public List<TankDto> TankDtos
        {
            get { return this._tankDtos; }
            set { if ((ReferenceEquals(this.TankDtos, value) != true)) { this._tankDtos = value; } }
        }
    }
}
