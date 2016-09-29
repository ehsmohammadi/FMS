using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.AntiCorruption.Contracts;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.Contracts.Mappers
{
    public interface IGoodAntiCorruptionMapper : IAntiCorruptionMapper<Good,GoodDto> 
    {
    }
}
