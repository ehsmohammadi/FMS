using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule
{
   public class HasSegment:BusinessRuleBase<VoucherSetingDetail>
   {
       public HasSegment(Expression<Func<string, bool>> expression, VoucherSetingDetail t) : base(expression, t)
       {
       }

       public override void Validate(string typeAction)
       {
           if (IsValidExpression(typeAction))
           {
               if(Entity.CreditSegmentTypes.Count==0)
                   throw new BusinessRuleException("0901","Segment Type Must Selected");

               if (Entity.DebitSegmentTypes.Count == 0)
                   throw new BusinessRuleException("0901", "Segment Type Must Selected");
           }
       }
   }
}
