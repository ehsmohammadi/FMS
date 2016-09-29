using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;



namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IInventoryOperationServiceWrapper : IServiceWrapper
    {
        void GetFuelReportInventoryOperations(Action<List<FuelReportInventoryOperationDto>, Exception> action, long fuelReportId);

        void GetFuelReportDetailInventoryOperations(Action<List<FuelReportInventoryOperationDto>, Exception> action, long fuelReportId, long fuelReportDetailId);

        void GetScrapInventoryOperations(Action<PageResultDto<FuelReportInventoryOperationDto>, Exception> action, long scrapId);
    }
}
