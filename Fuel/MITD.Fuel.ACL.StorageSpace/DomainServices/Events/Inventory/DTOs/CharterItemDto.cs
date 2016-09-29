using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public class CharterItemDto : DTOBase
    {
        private decimal _fee;
        private decimal _feeOffhire;
        private decimal _rob;
        private TankDto _tankDto;
        private long _tankId;
        private long charterId;
        private GoodDto good;
        private long id;

        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((ReferenceEquals(this.Id, value) != true)) { this.id = value; } }
        }

        [DataMember]
        public long CharterId
        {
            get { return this.charterId; }
            set { if ((ReferenceEquals(this.CharterId, value) != true)) { this.charterId = value; } }
        }

        [DataMember]
        public virtual GoodDto Good
        {
            get { return this.good; }
            set { if ((ReferenceEquals(this.Good, value) != true)) { this.good = value; } }
        }


        [DataMember]
        public virtual TankDto TankDto
        {
            get { return this._tankDto; }
            set { if ((ReferenceEquals(this.TankDto, value) != true)) { this._tankDto = value; } }
        }

        [DataMember]
        public virtual long TankId
        {
            get { return this._tankId; }
            set { if ((ReferenceEquals(this.TankId, value) != true)) { this._tankId = value; } }
        }


        //[CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
        [DataMember]
        public decimal Rob
        {
            get { return this._rob; }
            set { if ((ReferenceEquals(this.Rob, value) != true)) { this._rob = value; } }
        }


        //[CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
        [DataMember]
        public decimal Fee
        {
            get { return this._fee; }
            set { if ((ReferenceEquals(this.Fee, value) != true)) { this._fee = value; } }
        }

        //[CustomValidation(typeof(ValidationDto), "IsGreaterZero")]
        [DataMember]
        public decimal FeeOffhire
        {
            get { return this._feeOffhire; }
            set { if ((ReferenceEquals(this.FeeOffhire, value) != true)) { this._feeOffhire = value; } }
        }
    }
}
