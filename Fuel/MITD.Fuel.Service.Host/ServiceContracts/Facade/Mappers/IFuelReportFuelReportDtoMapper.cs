using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.Service.Host.ServiceContracts.Facade.Mappers
{
    public interface IFuelReportFuelReportDtoMapper : IFacadeMapper<FuelReport, FuelReportDto>
    {
        FuelReportTypeEnum MapEntityFuelReportTypeToDtoFuelReportType(
            FuelReportTypes fuelReportType);

             FuelReportTypes MapDtoFuelReportTypeToEntityFuelReportType(
        FuelReportTypeEnum fuelReportType);
    }
}