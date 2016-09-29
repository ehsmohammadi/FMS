using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VesselInCompanyToVesselInCompanyDtoMapper : BaseFacadeMapper<VesselInCompany, VesselInCompanyDto>, IVesselInCompanyToVesselInCompanyDtoMapper
    {
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;

        public VesselInCompanyToVesselInCompanyDtoMapper(IFacadeMapper<Company, CompanyDto> companyDtoMapper)
        {
            this.companyDtoMapper = companyDtoMapper;
        }

        public override VesselInCompanyDto MapToModel(VesselInCompany entity)
        {
            var dto = base.MapToModel(entity);

            dto.VesselState = (VesselStateEnum)(int)entity.VesselStateCode;
            dto.Company = companyDtoMapper.MapToModel(entity.Company);

            return dto;
        }

        //public override VesselInCompany MapToEntity(VesselInCompanyDto model)
        //{
        //    var result = new VesselInCompany(model.Id, model.Code, model.Name, model.Description, 1, VesselStates.Idle,false);
        //    return result;
        //}
    }


    public class VesselInInventoryToVesselDtoMapper : BaseFacadeMapper<VesselInInventory, VesselInCompanyDto>, IVesselInInventoryToVesselDtoMapper
    {
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;

        public VesselInInventoryToVesselDtoMapper(IFacadeMapper<Company, CompanyDto> companyDtoMapper)
        {
            this.companyDtoMapper = companyDtoMapper;
        }

        public override VesselInCompanyDto MapToModel(VesselInInventory entity)
        {
            var dto = base.MapToModel(entity);

            var vesselInCompany= entity.Company.VesselsOperationInCompany.SingleOrDefault (v => v.CompanyId == entity.CompanyId && v.Code == entity.Code);

            dto.VesselState = (VesselStateEnum)(int)vesselInCompany.VesselStateCode;
            dto.Company = companyDtoMapper.MapToModel(entity.Company);

            return dto;
        }
    }
}