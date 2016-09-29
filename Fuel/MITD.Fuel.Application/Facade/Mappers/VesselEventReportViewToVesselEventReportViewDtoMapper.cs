using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.ReportObjects;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VesselEventReportViewToVesselEventReportViewDtoMapper : BaseFacadeMapper<VesselEventReportsView, VesselEventReportViewDto>, IVesselEventReportViewToVesselEventReportViewDtoMapper
    {
        private readonly IFacadeMapper<VesselEventReportsView, VesselEventReportViewDto> vesselEventReportViewToVesselEventReportViewDtoMapper;

        public VesselEventReportViewToVesselEventReportViewDtoMapper(IFacadeMapper<VesselEventReportsView, VesselEventReportViewDto> vesselEventReportViewToVesselEventReportViewDtoMapper)
        {
            this.vesselEventReportViewToVesselEventReportViewDtoMapper = vesselEventReportViewToVesselEventReportViewDtoMapper;
        }

        public override VesselEventReportViewDto MapToModel(VesselEventReportsView entity)
        {
            var dto = base.MapToModel(entity);
            return dto;
        }
    }
}