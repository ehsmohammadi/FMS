using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.Contracts.ServiceWrappers
{
    public interface ICompanyAntiCorruptionServiceWrapper : IAntiCorruptionServiceWrapper<EnterprisePartyDto>
    {
         bool CanBePoGood(int goodId, int companyId);
    }
}
