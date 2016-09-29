using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using System.Collections.ObjectModel;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class ScrapToScrapDtoMapper : BaseFacadeMapper<Scrap, ScrapDto>, IScrapToScrapDtoMapper
    {
        private readonly IVesselToVesselDtoMapper vesselDtoMapper;
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;
        private readonly IScrapDetailToScrapDetailDtoMapper scrapDetailMapper;
        private readonly ITankToTankDtoMapper tankDtoMapper;
        private readonly IFacadeMapper<FuelUser, UserDto> userDtoMapper;

        public ScrapToScrapDtoMapper(
            IVesselToVesselDtoMapper vesselDtoMapper,
            IFacadeMapper<Company, CompanyDto> companyDtoMapper,
            IScrapDetailToScrapDetailDtoMapper scrapDetailMapper,
            ITankToTankDtoMapper tankDtoMapper,
            IFacadeMapper<FuelUser, UserDto> userDtoMapper)
        {
            this.vesselDtoMapper = vesselDtoMapper;
            this.companyDtoMapper = companyDtoMapper;
            this.scrapDetailMapper = scrapDetailMapper;
            this.tankDtoMapper = tankDtoMapper;
            this.userDtoMapper = userDtoMapper;
        }

        public override ScrapDto MapToModel(Scrap entity)
        {
            var dto = base.MapToModel(entity);

            dto.Vessel = this.vesselDtoMapper.MapToModel(entity.VesselInCompany);
            dto.Vessel.TankDtos = this.tankDtoMapper.MapToModel(entity.VesselInCompany.Tanks).ToList();

            dto.SecondParty = this.companyDtoMapper.MapToModel(entity.SecondParty);
            dto.ScrapDetails = new ObservableCollection<ScrapDetailDto>(scrapDetailMapper.MapToModel(entity.ScrapDetails));

            var lastWorkflowLog = entity.ApproveWorkflows.LastOrDefault();

            if (lastWorkflowLog != null)
            {
                dto.UserInCharge = userDtoMapper.MapToModel(lastWorkflowLog.ActorUser);
                dto.CurrentState = lastWorkflowLog.CurrentWorkflowStep.CurrentWorkflowStage.ToString();
            }

            return dto;
        }

        public ScrapDto MapToModel(Scrap entity, Action<Scrap, ScrapDto> action)
        {
            var dto = this.MapToModel(entity);

            if (action != null) action(entity, dto);

            return dto;
        }

        public IEnumerable<ScrapDto> MapToModel(IEnumerable<Scrap> entities, Action<Scrap, ScrapDto> action)
        {
            var result = entities.Select(e => this.MapToModel(e, action));
            return result;
        }
    }
}