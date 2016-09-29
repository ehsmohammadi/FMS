using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.Contracts.Adapters
{
    public interface IGoodPartyAssignmentAntiCorruptionAdapter : IAntiCorruptionAdapter<GoodPartyAssignment, GoodAssignmentDto>
    {
        bool IsNotOverMaximumOrder(double quantity, int companyId, int goodId, int wareHouseId);
        bool IsEqualFixOrder(double quantity, int companyId, int goodId, int wareHouseId);
        bool CanBeOrderWithReOrderLevelCheck(int companyId, int goodId, int wareHouseId);
    }
}
