#region

using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IVoyageLogDomainService : IDomainService
    {
        PageResult<VoyageLog> GetPagedDataByFilter(long voyageId, int pageSize, int pageIndex);
    }
}