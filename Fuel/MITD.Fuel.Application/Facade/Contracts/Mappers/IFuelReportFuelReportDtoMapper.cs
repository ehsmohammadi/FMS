using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;


namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IFuelReportFuelReportDtoMapper : IFacadeMapper<FuelReport, FuelReportDto>
    {
        FuelReportTypeEnum MapEntityFuelReportTypeToDtoFuelReportType(FuelReportTypes fuelReportType);

        FuelReportTypes MapDtoFuelReportTypeToEntityFuelReportType(FuelReportTypeEnum fuelReportType);

        FuelReportDto MapToModel(FuelReport entity, Action<FuelReportDetail, FuelReportDetailDto> detailsAction);

        IEnumerable<FuelReportDto> MapToModel(IEnumerable<FuelReport> entities, Action<FuelReportDetail, FuelReportDetailDto> detailsAction);
    }
}