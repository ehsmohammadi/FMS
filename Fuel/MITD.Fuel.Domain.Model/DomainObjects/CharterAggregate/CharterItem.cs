using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Specifications;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate
{
    public class CharterItem
    {
        #region Prop

 

        public long Id { get; private set; }
        public long CharterId { get; private set; }
        public long GoodUnitId { get; private set; }
        public long GoodId { get; private set; }
        public long? TankId { get; private set; }
        public decimal Rob { get; private set; }
        public decimal Fee { get; private set; }
        public decimal OffhireFee { get; private set; }

        public virtual Good Good { get; private set; }
        public virtual Tank Tank { get; private set; }
        public virtual GoodUnit GoodUnit { get; private set; }

        public virtual Charter Charter { get; private set; }
        public byte[] TimeStamp { get; set; }
        #endregion

        #region Ctor

        public CharterItem()
        {

        }

        public CharterItem(long id, long charterId, decimal rob, decimal fee,
                          decimal feeOffhire, long goodId, long tankId, long unitId)
        {
            this.Id = id;
            this.CharterId = charterId;
            this.Rob = rob;
            this.Fee = fee;
            this.OffhireFee = feeOffhire;
            this.GoodId = goodId;
            this.GoodUnitId = unitId;
            this.TankId = tankId;



        }

        #endregion

        #region Opert

        public void Update(long id, long charterId, decimal rob, decimal fee,
                          decimal feeOffhire, long goodId, long tankId, long unitId)
        {


            this.Id = id;
            this.CharterId = charterId;
            this.Rob = rob;
            this.Fee = fee;
            this.OffhireFee = feeOffhire;
            this.GoodId = goodId;
            this.GoodUnitId = unitId;
            this.TankId = tankId;
           
     
            
         

        }


        #endregion


        #region BusinessRule


       

        #endregion

    }
}
