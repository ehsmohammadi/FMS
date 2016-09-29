#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IGoodPartyAssignmentDomainService : IDomainService<GoodPartyAssignment>
    {
        bool IsNotOverMaximumOrder(double quantity, long company, long goodId, long wareHouseId);
        bool IsEqualFixOrder(double quantity, long company, long goodId, long wareHouseId);
        bool CanBeOrderWithReOrderLevelCheck(long company, long goodId, long wareHouseId);


        //  bool GoodAssignIdValidForCompanyAndGood(long goodPartyAssignmentId, long companyId, long goodId);
        //bool IsValid(long goodPartyAssignmentId);
    }
}