using MITD.Fuel.ACL.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.AntiCorruption;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mapper
{
    public class VesselAntiCorruptionMapper : BaseAntiCorruptionMapper<VesselInCompany, WarehouseDto>, IVesselAntiCorruptionMapper
    {
        //public override VesselInCompany MapToEntity(WarehouseDto model)
        //{
        //    var ent = new VesselInCompany(model.Id, model.Code, model.Description, model.Description, 1, VesselStates.Idle, false);
        //    return ent;

        //}
    }
}