#region

using System.Collections.Generic;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Good
    {

        //        public bool temp { get; private set; }


        public Good()
        {
        }

        public Good(long id, string name, string code, long sharedGoodId, long companyId)
            : this()
        {
            this.Id = id;
            this.Name = name;
            this.Code = code;
            this.SharedGoodId = sharedGoodId;
            this.CompanyId = companyId;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public long CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public long SharedGoodId { get; set; }

        //public virtual GoodUnit GoodUnit { get; set; }

        public virtual SharedGood SharedGood { get; set; }

        public virtual List<GoodUnit> GoodUnits { get; set; }
    }
}
