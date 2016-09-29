using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class OrderDomainService : IOrderDomainService
    {
        private readonly IOrderRepository orderRepository;

        //TODO: A.H Huge Review

        public OrderDomainService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public IEnumerable<Order> GetFinalApprovedPrurchaseOrders(long companyId, long receiverVesselInCompanyId)
        {
            var finalApprovePurchaseSpecification = new IsFinalApprovedOrderTypeForOwners(OrderTypes.Purchase, companyId);
            var finalApprovePurchaseForVesselSpecification = new IsFinalApprovedOrderTypeForOwners(OrderTypes.PurchaseForVessel, companyId);

            return orderRepository.Find(finalApprovePurchaseSpecification.Predicate.Or(finalApprovePurchaseForVesselSpecification.Predicate.And(o=>o.ToVesselInCompanyId == receiverVesselInCompanyId)));
        }

        public IList<Order> GetFinalApprovedInternalTransferOrders(long companyId)
        {
            var finalApproveSpecification = new IsFinalApprovedOrderTypeForOwners(OrderTypes.InternalTransfer, companyId);

            return orderRepository.Find(finalApproveSpecification.Predicate);
        }

        public IList<Order> GetSellerFinalApprovedPurchaseTransferOrders(long companyId)
        {
            var finalApproveSpecification = new IsFinalApprovedOrderTypeForSuppliers(OrderTypes.PurchaseWithTransferOperations,
                                                                                     companyId);
            return orderRepository.Find(finalApproveSpecification.Predicate);
        }

        public IEnumerable<Order> GetBuyerFinalApprovedPurchaseTransferOrders(long companyId)
        {
            var finalApproveSpecification = new IsFinalApprovedOrderTypeForOwners(OrderTypes.PurchaseWithTransferOperations,
                                                                                  companyId);
            return orderRepository.Find(finalApproveSpecification.Predicate);
        }

        public IEnumerable<Order> GetFinalApprovedTransferOrders(long companyId)
        {
            var finalApproveSpecification = new IsFinalApprovedOrderTypeForOwners(OrderTypes.Transfer, companyId);

            return orderRepository.Find(finalApproveSpecification.Predicate);
        }

        public void UpdateOrderItemsFromInvoiceItem(Invoice invoice)
        {

        }

    }
}