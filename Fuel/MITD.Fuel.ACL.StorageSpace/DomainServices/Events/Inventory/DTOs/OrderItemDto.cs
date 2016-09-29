using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class OrderItemDto : DTOBase
    {
        
        private long id;
        private string description;
        private decimal quantity;
        private long orderdId;
        private GoodDto good;

       


        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }
        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { if ((object.ReferenceEquals(this.Description, value) != true)) {this.description= value;}}
        }
        //[CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
        [DataMember]
        public decimal Quantity
        {
            get { return this.quantity; }
            set { if ((object.ReferenceEquals(this.Quantity, value) != true)) {this.quantity= value;}}
        }
        [DataMember]
        public long OrderId
        {
            get { return this.orderdId; }
            set { if ((object.ReferenceEquals(this.OrderId, value) != true)) {this.orderdId= value;}}
        }

        [DataMember]
        public virtual GoodDto Good
        {
            get { return this.good; }
            set
            {
                if ((object.ReferenceEquals(this.Good, value) != true)) {this.good= value;}
            }
        }

        [DataMember]
        public long? AssigneBuessinessPartyForGoodId { get; set; }

        [DataMember]
        public string AssigneBuessinessPartyForGoodName { get; set; }

        public virtual ObservableCollection<OrderItemBalanceDto> OrderItemBalances { get; set; }

        public OrderItemDto()
        {
            OrderItemBalances = new ObservableCollection<OrderItemBalanceDto>();
        }

    }
}
