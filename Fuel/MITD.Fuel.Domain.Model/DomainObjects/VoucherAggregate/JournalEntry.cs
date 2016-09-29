using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq.Visitors.ResultOperatorProcessors;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class JournalEntry
    {
        #region Prop
        public long Id { get;  set; }

        public long? InventoryItemId { get; set; }
        public long CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        public long VoucherId { get;  set; }
        public int Typ { get; set; }
        public string AccountNo { get; set; }

        public string Description { get; set; }

        public decimal ForeignAmount { get; set; }
        public decimal IrrAmount { get; set; }

        public bool IsDebit
        {
            get { return (Typ == 1) ? true : false; }
        }
        public bool IsCredit { get { return (Typ == 2) ? true : false; } }

        public virtual List<Segment> Segments { get; set; }

        //public long SegmentId { get; set; }

        public virtual Voucher Voucher { get; set; }

        public string VoucherRef { get; set; }


        public byte[] TimeStamp { get; set; }

        #endregion

        #region ctor

        public JournalEntry()
        {

        }

        public JournalEntry(long id,long voucherId, int typ, string accountNo,
                            string description, decimal foreignAmount,
                             decimal irrAmount, string voucherRef)
        {
            Id = id;
            VoucherId = voucherId;
            Typ = typ;
            AccountNo = accountNo;
            Description = description;
            ForeignAmount = foreignAmount;
            IrrAmount = irrAmount;
          //  SegmentId = segmentId;
            VoucherRef = voucherRef;
        }
        #endregion


        
    }
}
