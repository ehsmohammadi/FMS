using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IVesselToVesselDtoMapper : IFacadeMapper<VesselInCompany, VesselDto>
    {
        
    }

    public interface IVesselInInventoryToVesselDtoMapper : IFacadeMapper<VesselInInventory, VesselDto>
    {

    }
}