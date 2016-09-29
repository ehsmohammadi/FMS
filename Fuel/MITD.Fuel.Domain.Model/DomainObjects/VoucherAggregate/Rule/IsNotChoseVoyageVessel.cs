using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule
{
    public class IsNotChoseVoyageVessel : BusinessRuleBase<VoucherSetingDetail>
    {
        public IsNotChoseVoyageVessel(Expression<Func<string, bool>> expression, VoucherSetingDetail t)
            : base(expression, t)
        {
        }

        public override void Validate(string typeAction)
        {
            if (IsValidExpression(typeAction))
            {
              
                if (Entity.CreditSegmentTypes.Count(c => c.SegmentTypeId == 1) > 0
                    && Entity.CreditSegmentTypes.Count(c => c.SegmentTypeId == 3) > 0)
                    throw new BusinessRuleException("010","Vessel & Voyage shouldn't Select Together");

                if (Entity.DebitSegmentTypes.Count(c => c.SegmentTypeId == 1) > 0
                   && Entity.DebitSegmentTypes.Count(c => c.SegmentTypeId == 3) > 0)
                    throw new BusinessRuleException("010", "Vessel & Voyage shouldn't Select Together");
            }
        }
    }
}





