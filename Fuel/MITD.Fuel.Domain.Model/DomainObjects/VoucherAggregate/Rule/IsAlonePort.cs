using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule
{
  public  class IsAlonePort:BusinessRuleBase<VoucherSetingDetail>
  {
      public IsAlonePort(Expression<Func<string, bool>> expression, VoucherSetingDetail t) : base(expression, t)
      {
      }

      public override void Validate(string typeAction)
      {
          if (IsValidExpression(typeAction))
          {
              if(Entity.CreditSegmentTypes.Exists(c=>c.SegmentTypeId==2))
                if(Entity.CreditSegmentTypes.Count(c=>c.SegmentTypeId!=2)==0)
                    throw new BusinessRuleException("010","Segment Type Port Must Have Another Segment Type");

              if (Entity.DebitSegmentTypes.Exists(c => c.SegmentTypeId == 2))
                  if (Entity.DebitSegmentTypes.Count(c => c.SegmentTypeId != 2) == 0)
                      throw new BusinessRuleException("010", "Segment Type Port Must Have Another Segment Type");

          }
      }
  }
}
