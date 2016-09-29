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
    public interface IGoodUnitDomainService : IDomainService<GoodUnit>
    {
        bool IsValid(long goodUnitId);
        bool Validate(long goodUnitId, long companyId, long goodId, long unitId);
        List<GoodUnit> GetGoodUnits(long goodId);
    }
}