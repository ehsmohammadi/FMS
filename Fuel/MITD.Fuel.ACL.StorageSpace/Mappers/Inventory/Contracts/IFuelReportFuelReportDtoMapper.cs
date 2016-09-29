using System;
using System.Collections.Generic;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.Facade;


namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IFuelReportFuelReportDtoMapper : IFacadeMapper<FuelReport, FuelReportDto>
    {
        FuelReportTypeEnum MapEntityFuelReportTypeToDtoFuelReportType(FuelReportTypes fuelReportType);

        //FuelReportTypes MapDtoFuelReportTypeToEntityFuelReportType(string fuelReportType);

        FuelReportDto MapToModel(FuelReport entity, Action<FuelReportDetail, FuelReportDetailDto> action);

        IEnumerable<FuelReportDto> MapToModel(IEnumerable<FuelReport> entities, Action<FuelReportDetail, FuelReportDetailDto> action);
    }
}