using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IInventoryOperationDomainService : IDomainService
    {
        InventoryOperation GetEndOfVoyageFuelReportConsumptionInventoryOperation(FuelReport fuelReport);
        InventoryOperation GetEndOfMonthFuelReportConsumptionInventoryOperation(FuelReport fuelReport);
        InventoryOperation GetEndOfYearFuelReportConsumptionInventoryOperation(FuelReport fuelReport);
        List<InventoryOperation> GetFuelReportInventoryOperations(FuelReport fuelReport);

        List<InventoryOperation> GetFuelReportHeaderInventoryOperations(long fuelReportId);
        List<InventoryOperation> GetFuelReportDetailInventoryOperations(long fuelReportId, long fuelReportDetailId);

        List<InventoryOperation> GetVoyageRegisteredInventoryOperations(long voyageId);

        bool HasOrderAnyReceipt(long orderId);
        PageResult<InventoryOperation> GetScrapInventoryOperations(long scrapId, int? pageSize, int? pageIndex);
    }
}
