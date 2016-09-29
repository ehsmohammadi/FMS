using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.Contracts.ServiceWrappers
{
    public interface IGoodPartyAssignmentAntiCorruptionServiceWrapper : IAntiCorruptionServiceWrapper<GoodDto>
    {
        bool IsNotOverMaximumOrder(double quantity, int company, int goodId, int wareHouseId);
        bool IsEqualFixOrder(double quantity, int company, int goodId, int wareHouseId);
        bool CanBeOrderWithReOrderLevelCheck(int company, int goodId, int wareHouseId);
    }
}
