using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IInventoryTransactionController
    {
        void ShowList();

        void ShowReference(string referenceType, string referenceNumber, long selectedCompanyId, bool isPricing = false);

        void ShowByFilter(string inventoryOperationReferenceCode);
    }
}
