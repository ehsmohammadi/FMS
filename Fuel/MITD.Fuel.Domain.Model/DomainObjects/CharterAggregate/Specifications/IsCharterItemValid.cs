using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Specifications
{
  public class IsCharterItemValid :SpecificationBase<CharterItem>
  {
      //B6,7
      public IsCharterItemValid():base(item =>
          item.GoodId>0 &&
          item.GoodUnitId>0 &&
          item.Fee>0 &&
          item.Rob>0 
         
          
          )
      {
          
      }
    }
}
