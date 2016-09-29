using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class GoodUnitDto : DTOBase
    {
        private long id;
        private string name;

        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) { this.id = value; } }
        }
        [DataMember]
        public string Name
        {
            get { return this.name; }
            set { if ((object.ReferenceEquals(this.Name, value) != true)) { this.name = value; } }
        }

    }
}
