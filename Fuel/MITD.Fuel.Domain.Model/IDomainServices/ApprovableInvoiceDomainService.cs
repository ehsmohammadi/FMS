//using MITD.Fuel.Domain.Model.DomainObjects;
//using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
//using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates;
//using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
//
//namespace MITD.Fuel.Domain.Model.IDomainServices
//{
//    public class ApprovableInvoiceDomainService : IApprovableInvoiceDomainService
//    {
//        private readonly IVesselDomainService vesselDomainService;
//        private readonly IInventoryOperationNotifier inventoryOperationNotifier;
//        private readonly ITankDomainService tankDomainService;
//        private readonly ICurrencyDomainService currencyDomainService;
//        private readonly IGoodDomainService goodDomainService;
//        private readonly IGoodUnitDomainService goodUnitDomainService;
//
//
//        public ApprovableInvoiceDomainService(IVesselDomainService vesselDomainService, IInventoryOperationNotifier inventoryOperationNotifier,
//            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
//            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
//        {
//            this.vesselDomainService = vesselDomainService;
//            this.inventoryOperationNotifier = inventoryOperationNotifier;
//            this.tankDomainService = tankDomainService;
//            this.currencyDomainService = currencyDomainService;
//            this.goodDomainService = goodDomainService;
//            this.goodUnitDomainService = goodUnitDomainService;
//        }
//
//        public void ValidateMiddleApprove(Invoice invoice)
//        {
////            invoice.ValidateMiddleApprove(vesselDomainService, tankDomainService, currencyDomainService,
////                goodDomainService, goodUnitDomainService);
//        }
//
//        public void Submit(Invoice invoice, SubmitState submittedState)
//        {
//            invoice.SubmiteInvoice(submittedState, vesselDomainService, inventoryOperationNotifier,
//                tankDomainService, currencyDomainService,
//                goodDomainService, goodUnitDomainService);
//        }
//
//        public void Cancel(Invoice invoice, CancelState cancelledState)
//        {
//            invoice.Cancel(cancelledState, inventoryOperationNotifier);
//        }
////
////        public void RejectSubmittedState(Invoice invoice, SubmitRejectedState submitRejectedState)
////        {
////            invoice.RejectSubmittedState(submitRejectedState);
////        }
////
//        public void Close(Invoice invoice, CloseState closedState)
//        {
//            invoice.Close(closedState);
//        }
//    }
//}