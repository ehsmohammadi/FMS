using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class ScrapToScrapDtoMapper : BaseFacadeMapper<Scrap, ScrapDto>, IScrapToScrapDtoMapper
    {
        private readonly IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyDtoMapper;
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;
        private readonly IScrapDetailToScrapDetailDtoMapper scrapDetailMapper;
        private readonly ITankToTankDtoMapper tankDtoMapper;
        private readonly IFacadeMapper<FuelUser, UserDto> userDtoMapper;

        public ScrapToScrapDtoMapper(
            IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyDtoMapper,
            IFacadeMapper<Company, CompanyDto> companyDtoMapper,
            IScrapDetailToScrapDetailDtoMapper scrapDetailMapper,
            ITankToTankDtoMapper tankDtoMapper,
            IFacadeMapper<FuelUser, UserDto> userDtoMapper)
        {
            this.vesselInCompanyDtoMapper = vesselInCompanyDtoMapper;
            this.companyDtoMapper = companyDtoMapper;
            this.scrapDetailMapper = scrapDetailMapper;
            this.tankDtoMapper = tankDtoMapper;
            this.userDtoMapper = userDtoMapper;
        }

        public override ScrapDto MapToModel(Scrap entity)
        {
            var dto = base.MapToModel(entity);

            dto.VesselInCompany = this.vesselInCompanyDtoMapper.MapToModel(entity.VesselInCompany);
            dto.VesselInCompany.TankDtos = this.tankDtoMapper.MapToModel(entity.VesselInCompany.Tanks).ToList();

            dto.SecondParty = this.companyDtoMapper.MapToModel(entity.SecondParty);
            dto.ScrapDetails = new ObservableCollection<ScrapDetailDto>(scrapDetailMapper.MapToModel(entity.ScrapDetails));

            var lastWorkflowLog = entity.ApproveWorkflows.LastOrDefault(log => log.Active);

            if (lastWorkflowLog != null)
            {
                dto.UserInCharge = userDtoMapper.MapToModel(lastWorkflowLog.ActorUser);
                dto.CurrentState = lastWorkflowLog.CurrentWorkflowStep.CurrentWorkflowStage.ToString();
                dto.ApproveStatus = WorkflowStagesToDto(lastWorkflowLog.CurrentWorkflowStep.CurrentWorkflowStage);
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

        public WorkflowStageEnum WorkflowStagesToDto(WorkflowStages workflowStage)
        {
            switch (workflowStage)
            {
                case WorkflowStages.None:
                    return WorkflowStageEnum.None;
                    break;
                case WorkflowStages.Initial:
                    return WorkflowStageEnum.Initial;
                    break;
                case WorkflowStages.Approved:
                    return WorkflowStageEnum.Approved;
                    break;
                case WorkflowStages.FinalApproved:
                    return WorkflowStageEnum.FinalApproved;
                    break;
                case WorkflowStages.Submited:
                    return WorkflowStageEnum.Submited;
                    break;
                case WorkflowStages.Closed:
                    return WorkflowStageEnum.Closed;
                    break;
                case WorkflowStages.Canceled:
                    return WorkflowStageEnum.Canceled;
                    break;
                case WorkflowStages.SubmitRejected:
                    return WorkflowStageEnum.SubmitRejected;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("workflowStage");
            }
        }
    }
}