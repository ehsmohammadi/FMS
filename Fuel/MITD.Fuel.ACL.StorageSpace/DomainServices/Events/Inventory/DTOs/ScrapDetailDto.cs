using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class ScrapDetailDto : DTOBase
    {
        public ScrapDetailDto()
        {
        }

        long id;
        [DataMember]
        public long Id
        {
            get { return this.id; }
            set {if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }


        private double rob;
        [DataMember]
        public double ROB
        {
            get { return this.rob; }
            set { if ((object.ReferenceEquals(this.ROB, value) != true)) {this.rob= value;}}
        }

        private double price;
        [DataMember]
        public double Price
        {
            get { return this.price; }
            set { if ((object.ReferenceEquals(this.Price, value) != true)) {this.price= value;}}
        }

        private CurrencyDto currency;
        [DataMember]
        public CurrencyDto Currency
        {
            get { return this.currency; }
            set { if ((object.ReferenceEquals(this.Currency, value) != true)) {this.currency= value;}}
        }

        private GoodDto good;
        [DataMember]
        public GoodDto Good
        {
            get { return this.good; }
            set { if ((object.ReferenceEquals(this.Good, value) != true)) {this.good= value;}}
        }

        private GoodUnitDto unit;
        [DataMember]
        public GoodUnitDto Unit
        {
            get { return this.unit; }
            set { if ((object.ReferenceEquals(this.Unit, value) != true)) {this.unit= value;}}
        }

        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.TankDto tank;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.TankDto Tank
        {
            get { return this.tank; }
            set { if ((object.ReferenceEquals(this.Tank, value) != true)) {this.tank= value;}}
        }

        private ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.ScrapDto scrap;
        [DataMember]
        public ACL.StorageSpace.DomainServices.Events.Inventory.DTOs.ScrapDto Scrap
        {
            get { return this.scrap; }
            set { if ((object.ReferenceEquals(this.Scrap, value) != true)) {this.scrap= value;}}
        }
    }
}
