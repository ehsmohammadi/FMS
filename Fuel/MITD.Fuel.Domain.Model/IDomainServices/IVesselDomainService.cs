#region

using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Domain.Repository;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IVesselDomainService : IDomainService<Vessel>
    {
        PageResult<Vessel> GetPagedData(int pageSize, int pageIndex);

        PageResult<Vessel> GetPagedDataByFilter(long? ownerCompanyId, int pageSize, int pageIndex);
    }
}