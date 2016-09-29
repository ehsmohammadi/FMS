using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Contracts
{
    public interface IVoyageLogToVoyageLogDtoMapper : IFacadeMapper<VoyageLog, VoyageLogDto>
    {

    }
}