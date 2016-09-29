using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Service.Host.ServiceContracts.Facade.Mappers
{
    public interface IFuelReportDetailToFuelReportDetailDtoMapper : IFacadeMapper<FuelReportDetail, FuelReportDetailDto>
    {
        FuelReportDetailDto MapEntityToDto(FuelReportDetail entity);
        List<FuelReportDetailDto> MapEntityToDto(List<FuelReportDetail> entity);
//        FuelReportDetail MapDtoToEntity(FuelReportDetailDto entity);
//        List<FuelReportDetail> MapDtoToEntity(List<FuelReportDetailDto> entity);
    }
}
