using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Service.Host.ServiceContracts.Facade.Mappers
{
    public interface IVesselToVesselDtoMapper : IFacadeMapper<Vessel, VesselDto>
    {
         
    }
}