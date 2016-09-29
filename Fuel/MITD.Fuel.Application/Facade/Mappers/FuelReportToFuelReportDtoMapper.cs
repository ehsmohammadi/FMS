using System;
using System.Collections.ObjectModel;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Services.Facade;
using System.Linq;
using System.Collections.Generic;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class FuelReportToFuelReportDtoMapper : BaseFacadeMapper<FuelReport, FuelReportDto>, IFuelReportFuelReportDtoMapper
    {

        private readonly IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailMapper;
        private readonly IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper;

        public FuelReportToFuelReportDtoMapper(
            IFuelReportDetailToFuelReportDetailDtoMapper fuelReportDetailToFuelReportDetailDtoMapper, IInventoryOperationToInventoryOperationDtoMapper inventoryOperationDtoMapper)
        {
            this.fuelReportDetailMapper = fuelReportDetailToFuelReportDetailDtoMapper;
            this.inventoryOperationDtoMapper = inventoryOperationDtoMapper;
        }


        public FuelReportDto MapToModel(FuelReport entity)
        {
            //var dto = new FuelReportDto();

            //dto.FuelReportDetail = new ObservableCollection<FuelReportDetailDto>();

            //if (entity.FuelReportDetails != null && entity.FuelReportDetails.Count > 0)
            //{
            //    var list = fuelReportDetailMapper.MapToModel(entity.FuelReportDetails.ToList());
            //    dto.FuelReportDetail = new ObservableCollection<FuelReportDetailDto>(list);
            //}

            //base.Map(dto, entity);
            //dto.FuelReportType = MapEntityFuelReportTypeToDtoFuelReportType(entity.FuelReportType);

            //var vDto = new VoyageDto();
            //if (entity.Voyage != null)
            //{
            //    base.Map(vDto, entity.Voyage);
            //}
            //dto.Voyage = vDto;


            //var vesselInCompanyDto = new VesselInCompanyDto();
            //base.Map(vesselInCompanyDto, entity.VesselInCompany);
            //dto.VesselInCompanyDto = vesselInCompanyDto;


            //var companyDto = new CompanyDto();
            //base.Map(companyDto, entity.VesselInCompany.Company);
            //dto.VesselInCompanyDto.Company = companyDto;

            //dto.EnableCommercialEditing = entity.IsActive() && !entity.IsSubmittedByCommercial();

            //dto.CurrentStateName = entity.State == States.Closed 
            //    ? "Closed" 
            //    : entity.ApproveWorkFlows.Single(log=>log.Active).CurrentWorkflowStep.CurrentWorkflowStage.ToString();

            //dto.UserInChargName = entity.ApproveWorkFlows.Single(log => log.Active).ActorUser.Name;

            //return dto;

            return this.MapToModel(entity, null);
        }

        public override IEnumerable<FuelReportDto> MapToModel(IEnumerable<FuelReport> entities)
        {
            return entities.Select(this.MapToModel);
        }

        public FuelReportTypeEnum MapEntityFuelReportTypeToDtoFuelReportType(FuelReportTypes fuelReportType)
        {
            switch (fuelReportType)
            {
                case FuelReportTypes.NoonReport:
                    return FuelReportTypeEnum.NoonReport;
                case FuelReportTypes.EndOfVoyage:
                    return FuelReportTypeEnum.EndOfVoyage;
                case FuelReportTypes.ArrivalReport:
                    return FuelReportTypeEnum.ArrivalReport;
                case FuelReportTypes.DepartureReport:
                    return FuelReportTypeEnum.DepartureReport;
                case FuelReportTypes.Berthing:
                    return FuelReportTypeEnum.Berthing;
                case FuelReportTypes.Unberthing:
                    return FuelReportTypeEnum.Unberthing;
                case FuelReportTypes.CharterInEnd:
                    return FuelReportTypeEnum.CharterInEnd;
                case FuelReportTypes.CharterOutStart:
                    return FuelReportTypeEnum.CharterOutStart;
                case FuelReportTypes.BeginOfDryDock:
                    return FuelReportTypeEnum.BeginOfDryDock;
                case FuelReportTypes.BeginOfOffHire:
                    return FuelReportTypeEnum.BeginOfOffHire;
                case FuelReportTypes.BeginOfLayUp:
                    return FuelReportTypeEnum.BeginOfLayUp;
                case FuelReportTypes.EndOfOffhire:
                    return FuelReportTypeEnum.EndOfOffhire;
                case FuelReportTypes.BunkeringCommenced:
                    return FuelReportTypeEnum.BunkeringCommenced;
                case FuelReportTypes.DebunkeringCommenced:
                    return FuelReportTypeEnum.DebunkeringCommenced;
                case FuelReportTypes.Anchoring:
                    return FuelReportTypeEnum.Anchoring;
                case FuelReportTypes.HeavingAnchor:
                    return FuelReportTypeEnum.HeavingAnchor;

                case FuelReportTypes.EndOfDryDock:
                    return FuelReportTypeEnum.EndOfDryDock;
                case FuelReportTypes.BunkeringCompleted:
                    return FuelReportTypeEnum.BunkeringCompleted;
                case FuelReportTypes.DebunkeringCompleted:
                    return FuelReportTypeEnum.DebunkeringCompleted;

                default:
                    throw new ArgumentOutOfRangeException("fuelReportType");
            }
        }

        public FuelReportTypes MapDtoFuelReportTypeToEntityFuelReportType(FuelReportTypeEnum fuelReportType)
        {
            switch (fuelReportType)
            {
                case FuelReportTypeEnum.NoonReport:
                    return FuelReportTypes.NoonReport;
                case FuelReportTypeEnum.EndOfVoyage:
                    return FuelReportTypes.EndOfVoyage;
                case FuelReportTypeEnum.ArrivalReport:
                    return FuelReportTypes.ArrivalReport;
                case FuelReportTypeEnum.DepartureReport:
                    return FuelReportTypes.DepartureReport;
                case FuelReportTypeEnum.Berthing:
                    return FuelReportTypes.Berthing;
                case FuelReportTypeEnum.Unberthing:
                    return FuelReportTypes.Unberthing;
                case FuelReportTypeEnum.CharterInEnd:
                    return FuelReportTypes.CharterInEnd;
                case FuelReportTypeEnum.CharterOutStart:
                    return FuelReportTypes.CharterOutStart;
                case FuelReportTypeEnum.BeginOfDryDock:
                    return FuelReportTypes.BeginOfDryDock;
                case FuelReportTypeEnum.BeginOfOffHire:
                    return FuelReportTypes.BeginOfOffHire;
                case FuelReportTypeEnum.BeginOfLayUp:
                    return FuelReportTypes.BeginOfLayUp;
                case FuelReportTypeEnum.EndOfOffhire:
                    return FuelReportTypes.EndOfOffhire;
                case FuelReportTypeEnum.BunkeringCommenced:
                    return FuelReportTypes.BunkeringCommenced;
                case FuelReportTypeEnum.DebunkeringCommenced:
                    return FuelReportTypes.DebunkeringCommenced;
                case FuelReportTypeEnum.Anchoring:
                    return FuelReportTypes.Anchoring;
                case FuelReportTypeEnum.HeavingAnchor:
                    return FuelReportTypes.HeavingAnchor;

                case FuelReportTypeEnum.EndOfDryDock:
                    return FuelReportTypes.EndOfDryDock;
                case FuelReportTypeEnum.BunkeringCompleted:
                    return FuelReportTypes.BunkeringCompleted;
                case FuelReportTypeEnum.DebunkeringCompleted:
                    return FuelReportTypes.DebunkeringCompleted;
                default:
                    throw new ArgumentOutOfRangeException("fuelReportType");
            }
        }

        public FuelReportDto MapToModel(FuelReport entity, Action<FuelReportDetail, FuelReportDetailDto> detailsAction)
        {
            //var dto = this.MapToModel(entity);

            //foreach (var detailDto in dto.FuelReportDetail)
            //{

            //    detailsAction(entity.FuelReportDetails.FirstOrDefault(
            //        frd => frd.Id == detailDto.Id),
            //        detailDto);

            //}

            //return dto;

            var dto = new FuelReportDto();

            base.Map(dto, entity);

            dto.FuelReportDetail = new ObservableCollection<FuelReportDetailDto>();

            if (entity.FuelReportDetails != null && entity.FuelReportDetails.Count > 0)
            {
                var list = fuelReportDetailMapper.MapToModel(entity.FuelReportDetails, detailsAction);
                dto.FuelReportDetail = new ObservableCollection<FuelReportDetailDto>(list);
            }

            dto.FuelReportType = MapEntityFuelReportTypeToDtoFuelReportType(entity.FuelReportType);

            var vDto = new VoyageDto();
            if (entity.Voyage != null)
            {
                base.Map(vDto, entity.Voyage);
            }
            dto.Voyage = vDto;


            var vesselInCompanyDto = new VesselInCompanyDto();
            base.Map(vesselInCompanyDto, entity.VesselInCompany);
            dto.VesselInCompanyDto = vesselInCompanyDto;


            var companyDto = new CompanyDto();
            base.Map(companyDto, entity.VesselInCompany.Company);
            dto.VesselInCompanyDto.Company = companyDto;

            dto.EnableCommercialEditing = entity.IsActive() && !entity.IsSubmittedByCommercial();

            dto.CurrentStateName = entity.State == States.Closed
                ? "Closed"
                : entity.ApproveWorkFlows.Single(log => log.Active).CurrentWorkflowStep.CurrentWorkflowStage.ToString();

            dto.ApproveStatus = (WorkflowStageEnum)(int) entity.ApproveWorkFlows.Single(log => log.Active).CurrentWorkflowStep.CurrentWorkflowStage;

            dto.UserInChargName = entity.ApproveWorkFlows.Single(log => log.Active).ActorUser.Name;

            dto.InventoryOperationDtos = inventoryOperationDtoMapper.MapToModel(entity.ConsumptionInventoryOperations).ToList();

            return dto;
        }

        public System.Collections.Generic.IEnumerable<FuelReportDto> MapToModel(System.Collections.Generic.IEnumerable<FuelReport> entities, Action<FuelReportDetail, FuelReportDetailDto> detailsAction)
        {
            return entities.Select(entity => this.MapToModel(entity, detailsAction)).ToList();
        }
    }
}