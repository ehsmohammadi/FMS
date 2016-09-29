using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Data.EF.ExternalServices
{
    public interface IEnterprisePartyExternalService
    {
        bool CanBePoGood(int goodId, int enterprisePartyId);
        EnterpriseParty GetEnterpriseParty(int? enterprisePartyFromId);
        List<EnterpriseParty> Get(List<int> ids);
        List<EnterpriseParty> GetAll();
    }
}