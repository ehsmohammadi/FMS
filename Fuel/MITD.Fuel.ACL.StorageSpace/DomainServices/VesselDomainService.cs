using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.FakeDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class VesselInInventoryDomainService : IVesselInInventoryDomainService
    {
        IAntiCorruptionAdapter<VesselInInventory, WarehouseDto> Adapter { get; set; }

        public VesselInInventoryDomainService(
            IAntiCorruptionAdapter<VesselInInventory, WarehouseDto> adapter)
        {
            this.Adapter = adapter;
        }


        List<VesselInCompany> IDomainService<VesselInCompany>.Get(List<long> IDs)
        {
            throw new NotImplementedException();
        }

        public List<VesselInInventory> CompanyVessels(long companyId)
        {
            throw new NotImplementedException();
        }

        public List<VesselInCompany> GetActiveVesselsInCompany(long companyId)
        {
            throw new NotImplementedException();
        }

        public VesselInInventory GetWithTanks(long id)
        {
            throw new NotImplementedException();
        }

        List<VesselInInventory> IVesselInInventoryDomainService.Get(List<long> idList)
        {
            throw new NotImplementedException();
        }

        public VesselInCompany Get(long id)
        {
            throw new NotImplementedException();
        }

        public List<VesselInCompany> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
