using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IVesselActivationItemToVesselActivationItemDtoMapper : IFacadeMapper<VesselActivationItem, VesselActivationItemDto>
    {
        
    }
}