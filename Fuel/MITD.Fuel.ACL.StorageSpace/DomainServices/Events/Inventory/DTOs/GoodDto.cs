using System.Collections.Generic;
using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class GoodDto : DTOBase
    {
        private long id;
        private string name;
        private string code;

        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }
        [DataMember]
        public string Name
        {
            get { return this.name; }
            set { if ((object.ReferenceEquals(this.Name, value) != true)) {this.name= value;}}
        }

        [DataMember]
        public string Code
        {
            get { return this.code; }
            set { if ((object.ReferenceEquals(this.Code, value) != true)) {this.code= value;}}
        }

        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.GoodUnitDto unit;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.GoodUnitDto Unit
        {
            get { return this.unit; }
            set { if ((object.ReferenceEquals(this.Unit, value) != true)) { this.unit = value; } }
        }

        private long sharedGoodId;
        [DataMember]
        public long SharedGoodId
        {
            get { return this.sharedGoodId; }
            set { if ((object.ReferenceEquals(this.SharedGoodId, value) != true)) { this.sharedGoodId = value; } }
        }

       private List<ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.GoodUnitDto> units;
        //   [Required(AllowEmptyStrings = false, ErrorMessage = "Unit is required")]
       [DataMember]
       public List<ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.GoodUnitDto> Units
        {
            get { return this.units; }
            set { if ((object.ReferenceEquals(this.Units, value) != true)) {this.units= value;}}
        }

        //private List<BrandDto> brands;
        ////   [Required(AllowEmptyStrings = false, ErrorMessage = "Unit is required")]
        //public List<BrandDto> Brands
        //{
        //    get { return brands; }
        //    set { if ((object.ReferenceEquals(this.Brands, value) != true)) {this.brands= value;}}
        //}
      
    }
}
