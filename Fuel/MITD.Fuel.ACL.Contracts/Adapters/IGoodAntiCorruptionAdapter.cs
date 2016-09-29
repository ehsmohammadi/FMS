using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.Contracts.Adapters
{
    public interface IGoodAntiCorruptionAdapter : IAntiCorruptionAdapter<Good, GoodDto>
    {
        List<Good> GetAll(int companyId);
    }
}
