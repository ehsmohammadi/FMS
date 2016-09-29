using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Integration.Inventory
{
   public class ExceptionChain:IChain
   {
       private BusinessRuleException _businessRuleException;
       public ExceptionChain(BusinessRuleException businessRuleException )
       {
           _businessRuleException = businessRuleException;
       }
       public void HandleRequest()
       {
           throw _businessRuleException;
       }

       public string Name { get; set; }
       public ChainType ChainType { get; set; }
    }
}
