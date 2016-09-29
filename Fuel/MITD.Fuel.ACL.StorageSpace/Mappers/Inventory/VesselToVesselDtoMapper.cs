using System.Linq;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class VesselToVesselDtoMapper : BaseFacadeMapper<VesselInCompany, VesselDto>, IVesselToVesselDtoMapper
    {
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;

        public VesselToVesselDtoMapper(IFacadeMapper<Company, CompanyDto> companyDtoMapper)
        {
            this.companyDtoMapper = companyDtoMapper;
        }

        public override VesselDto MapToModel(VesselInCompany entity)
        {
            var dto = base.MapToModel(entity);
            dto.Id = entity.VesselInInventory.Id;
            //            dto.VesselState = entity.VesselStateCode.ToString();
            dto.VesselState = (VesselStateEnum)(int)entity.VesselStateCode;
            dto.Company = companyDtoMapper.MapToModel(entity.Company);
            
            return dto;
        }


    }


    public class VesselInInventoryToVesselDtoMapper : BaseFacadeMapper<VesselInInventory, VesselDto>, IVesselInInventoryToVesselDtoMapper
    {
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;

        public VesselInInventoryToVesselDtoMapper(IFacadeMapper<Company, CompanyDto> companyDtoMapper)
        {
            this.companyDtoMapper = companyDtoMapper;
        }

        public override VesselDto MapToModel(VesselInInventory entity)
        {
            var dto = base.MapToModel(entity);

            var vesselInCompany = entity.Company.VesselsOperationInCompany.SingleOrDefault(v => v.CompanyId == entity.CompanyId && v.Code == entity.Code);

            //dto.VesselState = vesselInCompany.VesselStateCode.ToString();
            dto.VesselState = (VesselStateEnum)(int)vesselInCompany.VesselStateCode;
            dto.Company = companyDtoMapper.MapToModel(entity.Company);

            return dto;
        }
    }
}