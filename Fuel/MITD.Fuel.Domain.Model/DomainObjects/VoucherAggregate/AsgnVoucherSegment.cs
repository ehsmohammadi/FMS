using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
   public class AsgnVoucherSegment
    {
        
            #region Prop

       public int Id { get; set; }
            public int Typ { get; set; }

            public long SegmentId { get; set; }

            

            public bool IsDebit
            {
                get { return (Typ == 1) ? true : false; }
            }
            public bool IsCredit { get { return (Typ == 2) ? true : false; } }
            public byte[] TimeStamp { get; set; }

            public virtual VoucherSetingDetail VoucherSetingDetail { get; set; }

            public virtual Segment Segment { get; set; }
       
            public long VoucherSetingDetailId { get; set; }
            #endregion


            #region ctor

            public AsgnVoucherSegment()
            {

            }


            public AsgnVoucherSegment(int typ, long segmentId, long voucherSetingDetailId)
            {
                this.Typ = typ;
                this.SegmentId = segmentId;
                this.VoucherSetingDetailId = voucherSetingDetailId;
            }
            #endregion
        }
    
}
