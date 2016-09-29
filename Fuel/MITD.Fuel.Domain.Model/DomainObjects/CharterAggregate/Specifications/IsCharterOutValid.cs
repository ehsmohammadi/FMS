using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Specifications
{
   public class IsCharterOutValid :SpecificationBase<CharterOut>
   {
       ////CS_1
       public IsCharterOutValid()
           : base(charter => charter.VesselInCompanyId > 0 
                             && charter.ChartererId>0 && 
                                        charter.ActionDate!=null )
        {
            
        }
    }
}
