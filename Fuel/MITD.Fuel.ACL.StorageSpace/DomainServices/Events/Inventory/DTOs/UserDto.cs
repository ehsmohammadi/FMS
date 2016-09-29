using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class UserDto : DTOBase
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

        private CompanyDto _companyDto;
        [DataMember]
        public CompanyDto CompanyDto 
        {
            get { return this._companyDto; }
            set { if ((object.ReferenceEquals(this.CompanyDto, value) != true)) {this._companyDto= value;}}
        }
    }
}
