using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Service.AntiCorruption.Contracts.Adapters;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Data.EF.DomainServices
{
    public class EnterprisePartyDomainService:IEnterprisePartyDomainService
    {
        IAntiCorruptionAdapter<EnterpriseParty, EnterprisePartyDto> Adapter { get; set; }
        private IEnterprisePartyAntiCorruptionAdapter EnterprisePartyAntiCorruptionAdapter { get; set; }

        public EnterprisePartyDomainService(IAntiCorruptionAdapter<EnterpriseParty, EnterprisePartyDto> adapter, IEnterprisePartyAntiCorruptionAdapter enterprisePartyAntiCorruptionAdapter)
        {
            this.Adapter = adapter;
            this.EnterprisePartyAntiCorruptionAdapter = enterprisePartyAntiCorruptionAdapter;
        }
         
        public List<EnterpriseParty> Get(List<int> IDs)
        {
            var data = this.Adapter.Get(IDs); 
            return data;
        }




        public List<EnterpriseParty> GetAll()
        {
            var entities = this.Adapter.GetAll();
            return entities;
        }

        public bool CanBePoGood(int goodId, int enterprisePartyId)
        {
            return this.EnterprisePartyAntiCorruptionAdapter.CanBePoGood(goodId, enterprisePartyId);
        }

        public EnterpriseParty Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}