using MITD.Domain.Repository;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Integration.Inventory.Data.Repositories;


namespace MITD.Fuel.Integration.Inventory
{
    public class InventoryOperationManager : IInventoryOperationManager
    {
        private readonly IRepository<Inventory_Company> companyRepository;

        public InventoryOperationManager(IRepository<Inventory_Company> companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public Domain.Model.DomainObjects.InventoryOperation ManageFuelReportConsumption(Domain.Model.DomainObjects.FuelReport fuelReport, int userId)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageFuelReportDetailReceive(Domain.Model.DomainObjects.FuelReportDetail fuelReportDetail, int userId)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageFuelReportDetailIncrementalCorrectionUsingPricingReference(Domain.Model.DomainObjects.FuelReportDetail fuelReportDetail, long pricingReferenceId, string pricingReferenceType, int userId)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageFuelReportDetailIncrementalCorrectionDirectPricing(Domain.Model.DomainObjects.FuelReportDetail fuelReportDetail, int userId)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageFuelReportDetailDecrementalCorrection(Domain.Model.DomainObjects.FuelReportDetail fuelReportDetail, int userId)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageFuelReportDetailTransfer(Domain.Model.DomainObjects.FuelReportDetail fuelReportDetail, long? pricingReferenceId, string pricingReferenceType, int userId)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageScrap(Domain.Model.DomainObjects.Scrap scrap, int userId)
        {
            throw new System.NotImplementedException();
        }

        public Domain.Model.DomainObjects.InventoryOperation ManageOrderItemBalance(Domain.Model.DomainObjects.OrderAggreate.OrderItemBalance orderItemBalance, int userId)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageCharterInStart(Domain.Model.DomainObjects.CharterAggregate.CharterIn charterInStart, int userId, System.Transactions.TransactionScope tran)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageCharterInEnd(Domain.Model.DomainObjects.CharterAggregate.CharterIn charterInEnd, int userId)
        {
            throw new System.NotImplementedException();
        }

        public Domain.Model.DomainObjects.InventoryOperation ManageCharterOutStart(Domain.Model.DomainObjects.CharterAggregate.CharterOut charterOutStart, int userId)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Domain.Model.DomainObjects.InventoryOperation> ManageCharterOutEnd(Domain.Model.DomainObjects.CharterAggregate.CharterOut charterOutEnd, int userId)
        {
            throw new System.NotImplementedException();
        }

        public Inventory_Transaction GetTransaction(long transactionId, InventoryOperationType operationType)
        {
            throw new System.NotImplementedException();
        }

        public Inventory_OperationReference GetFueReportDetailReceiveOperationReference(Domain.Model.DomainObjects.FuelReportDetail fuelReportDetail)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Inventory_OperationReference> GetFueReportDetailsReceiveOperationReference(System.Collections.Generic.List<Domain.Model.DomainObjects.FuelReportDetail> fuelReportDetails)
        {
            throw new System.NotImplementedException();
        }

        public decimal GetAveragePrice(long transactionId, MITD.Fuel.Integration.Inventory.TransactionActionType actionType, long goodId, long unitId)
        {
            throw new System.NotImplementedException();
        }
    }

    public enum TransactionActionType : byte
    {
        Receipt = 1,
        Issue = 2,
        SaleFactor = 3,
    }

    public enum TransactionStatus : byte
    {
        JustRegistered = 1,
        PartialPriced = 2,
        FullPriced = 3,
        Vouchered = 4
    }
}
