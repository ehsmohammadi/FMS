using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class Account
    {

        #region prop

        public int Id { get; private set; }
        public string Code { get; private set; }
        public string  Name{ get; private set; }
        public virtual List<AsgnVoucherAcont> AsgnVoucherAconts { get; set; }
        public virtual List<EffectiveFactor> EffectiveFactors { get; set; }
        public byte[] TimeStamp { get; set; }
        #endregion


        #region ctor

        public Account()
        {
                
        }
        public Account(int id,string code,string name)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
        }
        #endregion
    }
}
