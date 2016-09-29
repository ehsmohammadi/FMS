using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public class ApprovableFuelReportDomainService : IApprovableFuelReportDomainService
    {
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IBalanceDomainService balanceDomainService;
        private readonly IInventoryManagementDomainService inventoryManagementDomainService;
        private readonly IFuelReportDomainService fuelReportDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
        private readonly IInventoryOperationNotifier inventoryOperationNotifier;
        private readonly IOrderDomainService orderDomainService;
        private readonly IVoyageDomainService voyageDomainService;

        public ApprovableFuelReportDomainService(IVoyageDomainService voyageDomainService, IFuelReportDomainService fuelReportDomainService, IInventoryOperationDomainService inventoryOperationDomainService, IGoodDomainService goodDomainService, IOrderDomainService orderDomainService, ICurrencyDomainService currencyDomainService, IBalanceDomainService balanceDomainService, IInventoryManagementDomainService inventoryManagementDomainService, IInventoryOperationNotifier inventoryOperationNotifier)
        {
            this.voyageDomainService = voyageDomainService;
            this.fuelReportDomainService = fuelReportDomainService;
            this.inventoryOperationDomainService = inventoryOperationDomainService;
            this.goodDomainService = goodDomainService;
            this.orderDomainService = orderDomainService;
            this.inventoryOperationNotifier = inventoryOperationNotifier;
            this.currencyDomainService = currencyDomainService;
            this.balanceDomainService = balanceDomainService;
            this.inventoryManagementDomainService = inventoryManagementDomainService;

           
        }

        public void Submit(FuelReport fuelReport, FuelReportState entityNewState, long approverId)
        {
            fuelReport.Submit(
                entityNewState,
                this.voyageDomainService,
                this.fuelReportDomainService,
                this.inventoryOperationDomainService,
                this.goodDomainService,
                this.orderDomainService,
                this.currencyDomainService,
                this.balanceDomainService,
                inventoryManagementDomainService,
                this.inventoryOperationNotifier,
                approverId);
        }
    }
}