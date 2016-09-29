using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.Contracts.Adapters
{
    public interface ICompanyAntiCorruptionAdapter : IAntiCorruptionAdapter<Company, EnterprisePartyDto>
    {
        bool CanBePoGood(int goodId, int companyId);
    }
}
