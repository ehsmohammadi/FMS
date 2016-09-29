using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class AsgnVoucherAcont
    {
        #region Prop


        public int Id { get; set; }
        public int Typ { get; set; }

        public int AccountId { get; set; }

        

        public bool IsDebit
        {
            get { return (Typ == 1) ? true : false; }
        }
        public bool IsCredit { get { return (Typ == 2) ? true : false; } }

        public byte[] TimeStamp { get; set; }

        public virtual  VoucherSetingDetail VoucherSetingDetail { get; set; }
        public long VoucherSetingDetailId { get; set; }
       

        public virtual Account  Account { get; set; }

        #endregion


        #region ctor

        public AsgnVoucherAcont()
        {

        }


        public AsgnVoucherAcont(int id, int typ, int accountId, int voucherSetingDetailId)
        {
            this.Id = id;
            this.Typ = typ;
            this.AccountId = accountId;
            this.VoucherSetingDetailId = voucherSetingDetailId;
        }
        #endregion
    }
}
