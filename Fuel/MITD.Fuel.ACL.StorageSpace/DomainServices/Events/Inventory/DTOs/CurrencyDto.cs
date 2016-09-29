using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class CurrencyDto : DTOBase
    {
        private long id;
        private string abbreviation;
        private string name;
       private decimal currencyToMainCurrencyRate;


        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [DataMember]
        public string Abbreviation
        {
            get { return this.abbreviation; }
            set { if ((object.ReferenceEquals(this.Abbreviation, value) != true)) {this.abbreviation= value;}}
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        [DataMember]
        public string Name
        {
            get { return this.name; }
            set { if ((object.ReferenceEquals(this.Name, value) != true)) {this.name= value;}}
        }

        [DataMember]
        public decimal CurrencyToMainCurrencyRate
        {
            get { return this.currencyToMainCurrencyRate; }
            set { if ((object.ReferenceEquals(this.CurrencyToMainCurrencyRate, value) != true)) {this.currencyToMainCurrencyRate= value;}}
        }

    }
}
