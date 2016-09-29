using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface IFuelReportInventoryResultFacadeService : IFacadeService
    {
        void IsHandleResultPossible(long fuelReportId);

        void HandleResult(FuelReportInventoryResultDto dto);
    }
}