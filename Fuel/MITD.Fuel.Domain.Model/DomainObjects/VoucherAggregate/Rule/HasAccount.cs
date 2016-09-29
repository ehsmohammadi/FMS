using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule
{
   public class HasAccount:BusinessRuleBase<VoucherSetingDetail>
   {
       public HasAccount(Expression<Func<string, bool>> expression, VoucherSetingDetail t) : base(expression, t)
       {
       }

       public override void Validate(string typeAction)
       {
           if (IsValidExpression(typeAction))
           {
               if (!(Entity.AsgnVoucherAconts.Exists(c => c.IsCredit && c.AccountId > 0) && Entity.AsgnVoucherAconts.Exists(c => c.IsDebit && c.AccountId > 0)))
               {
                  throw new BusinessRuleException("010","Select Account"); 
               }
           } 
       }
   }
}
