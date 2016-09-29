using System.Collections.Generic;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.Contracts.ServiceWrappers
{
    public interface IGoodAntiCorruptionServiceWrapper : IAntiCorruptionServiceWrapper<GoodDto>
    {
         List<GoodDto> GetAll(int companyId);
    }
}
