using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Specifications
{
    //CS_1
    public class IsCharterInValid : SpecificationBase<CharterIn>
    {
        public IsCharterInValid():base( charter => charter.VesselInCompanyId>0 
                                     && charter.OwnerId>0 && 
                                        charter.ActionDate!=null )
        {
            
        }

    }

}
