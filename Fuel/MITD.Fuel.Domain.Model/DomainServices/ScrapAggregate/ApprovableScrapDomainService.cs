using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ScrapStates;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public class ApprovableScrapDomainService : IApprovableScrapDomainService
    {
        private readonly IVesselInCompanyDomainService vesselDomainService;
        private readonly IInventoryOperationNotifier inventoryOperationNotifier;
        private readonly ITankDomainService tankDomainService;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IGoodUnitDomainService goodUnitDomainService;


        public ApprovableScrapDomainService(
            IVesselInCompanyDomainService vesselDomainService, IInventoryOperationNotifier inventoryOperationNotifier,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.vesselDomainService = vesselDomainService;
            this.inventoryOperationNotifier = inventoryOperationNotifier;
            this.tankDomainService = tankDomainService;
            this.currencyDomainService = currencyDomainService;
            this.goodDomainService = goodDomainService;
            this.goodUnitDomainService = goodUnitDomainService;
        }

        public void ValidateMiddleApprove(Scrap scrap)
        {
            scrap.ValidateMiddleApprove(vesselDomainService, tankDomainService, currencyDomainService,
                goodDomainService, goodUnitDomainService);
        }

        public void Submit(Scrap scrap, SubmittedState submittedState, long approverId)
        {
            scrap.Submit(submittedState, 
                vesselDomainService, inventoryOperationNotifier,
                tankDomainService, currencyDomainService,
                goodDomainService, goodUnitDomainService,
                approverId);
        }

        public void Cancel(Scrap scrap, CancelledState cancelledState, long approverId)
        {
            scrap.Cancel(cancelledState, inventoryOperationNotifier, approverId);
        }

        public void RejectSubmittedState(Scrap scrap, SubmitRejectedState submitRejectedState, long approverId)
        {
            scrap.RejectSubmittedState(submitRejectedState, approverId);
        }

        public void Close(Scrap scrap, ClosedState closedState, long approverId)
        {
            scrap.Close(closedState);
        }
    }
}