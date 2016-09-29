using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
   public class AsgnSegmentTypeVoucherSetingDetail
    {
       public int Id { get;private set; }

       public int SegmentTypeId { get; set; }

       public long VoucherSetingDetailId { get; set; }

       public virtual VoucherSetingDetail   VoucherSetingDetail { get; set; }
       public int Typ { get; set; }
       public bool IsDebit
       {
           get { return (Typ == 1) ? true : false; }
       }
       public bool IsCredit { get { return (Typ == 2) ? true : false; } }
       public AsgnSegmentTypeVoucherSetingDetail()
       {
           
       }

    }
}
