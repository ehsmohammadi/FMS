using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class NullValidationHeader : BusinessRuleBase<VoucherSeting>
    {
        

        public NullValidationHeader( Expression<Func<string, bool>> predicate ,VoucherSeting voucherSeting)
            : base(predicate, voucherSeting)
        {
           


        }

        public override void Validate(string typeAction)
        {

            if (IsValidExpression(typeAction))
                if (Entity.CompanyId == 0 || Entity.VoucherDetailTypeId == 0 ||
                    Entity.VoucherTypeId == 0)
                {
                    throw new BusinessRuleException("0909", "Compnay or VoucherType Is Empty");
                }
        }
    }
}
