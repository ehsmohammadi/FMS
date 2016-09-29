#region

using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IVesselInInventoryDomainService : IDomainService<VesselInCompany>
    {
        List<VesselInInventory> Get(List<long> idList);
        List<VesselInInventory> CompanyVessels(long companyId);
        List<VesselInCompany> GetActiveVesselsInCompany(long companyId);
        VesselInInventory GetWithTanks(long id);
    }
}