using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IInventoryManagementDomainService : IDomainService
    {
        InventoryResult GetPricedIssueResult(long companyId, long operationId);
        bool CanIssuance(long vesselInCompanyId);
        bool CanRecipt(long VesselInCompanyId);
        //List<Reference> GetVesselPurchaseReceiptNumbers(long companyId, long vesselInCompanyId);

        List<Reference> GetFueReportDetailsReceiveOperationReference(List<FuelReportDetail> previousPurchaseFuelReportDetails);
        InventoryResult GetInventoryResult(long companyId, long operationId, InventoryActionType actionType, bool includePrices);

        InventoryResult GetVoyageConsumptionResult(long companyId, long endOfVoyageInventoryOperationId);

        InventoryResult GetLastNotOperatedIssuedTrustReceive(long companyId, string vesselCode, DateTime comparingDateTime);

        bool IsThereAnyInventoryOperationWithNotEmptyQuantity(List<InventoryOperation> inventoryOperations);
        bool AreInventoryOperationsPartiallyPriced(List<InventoryOperation> inventoryOperations);
    }
}
