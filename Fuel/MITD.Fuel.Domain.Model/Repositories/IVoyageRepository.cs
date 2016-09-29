#region

using System.Collections.Generic;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Domain.Model.Repositories
{
    public interface IVoyageRepository : IRepository<Voyage>
    {
        //List<Voyage> GetByFilter(long companyId, long vesselInCompanyId);
        void UpdateVoyagesFromRotationData();
    }
}