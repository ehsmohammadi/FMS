using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.ReportObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IVesselEventReportViewToVesselEventReportViewDtoMapper : IFacadeMapper<VesselEventReportsView, VesselEventReportViewDto>
    {
        
    }
}