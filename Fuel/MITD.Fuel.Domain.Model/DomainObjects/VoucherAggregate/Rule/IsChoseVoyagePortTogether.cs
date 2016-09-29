using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule
{
   public class IsChoseVoyagePortTogether:BusinessRuleBase<VoucherSetingDetail>
   {
       public IsChoseVoyagePortTogether(Expression<Func<string, bool>> expression, VoucherSetingDetail t) : base(expression, t)
       {
       }

       public override void Validate(string typeAction)
       {
           if (IsValidExpression(typeAction))
           {
               if (Entity.CreditSegmentTypes.Exists(c=>c.SegmentTypeId==3))
                   if (!Entity.CreditSegmentTypes.Exists(c => c.SegmentTypeId == 2))
                    throw new BusinessRuleException("010","Port & Voyage Must Seleted");

               if (Entity.DebitSegmentTypes.Exists(c => c.SegmentTypeId == 3))
                   if (!Entity.DebitSegmentTypes.Exists(c => c.SegmentTypeId == 2))
                   throw new BusinessRuleException("010", "Port & Voyage Must Seleted");
           }
       }
   }
}
