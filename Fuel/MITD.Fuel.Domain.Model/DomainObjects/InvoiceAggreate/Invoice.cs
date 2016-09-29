#region

using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.DomainService;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceType;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Specifications;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.Repositories;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate
{
    public class Invoice
    {
        #region Properties

        private InvoiceBaseType invoiceBaseType;

        private IEntityConfigurator<Invoice> invoiceConfigurator;

        public long Id { get; set; }

        public DateTime InvoiceDate { get; set; }

        public long CurrencyId { get; set; }

        public States State { get; set; }

        public InvoiceState InvoiceState { get; set; }

        public string Description { get; private set; }

        public DivisionMethods DivisionMethod { get; private set; }

        public string InvoiceNumber { get; set; }

        public AccountingTypes AccountingType { get; set; }
        public long? InvoiceRefrenceId { get; set; }


        public InvoiceTypes InvoiceType { get; set; }

        public long? TransporterId { get; set; }

        public long? SupplierId { get; set; }

        public byte[] TimeStamp { get; set; }

        public long OwnerId { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual Company Owner { get; set; }

        public virtual Invoice InvoiceRefrence { get; set; }

        public virtual List<Order> OrderRefrences { get; set; }

        public virtual List<InvoiceItem> InvoiceItems { get; set; }

        public virtual List<InvoiceWorkflowLog> ApproveWorkFlows { get; private set; }

        public virtual Company Supplier { get; set; }

        public virtual Company Transporter { get; set; }

        public virtual List<InvoiceAdditionalPrice> AdditionalPrices { get; set; }

        public bool IsCreditor { get; set; }

        public decimal TotalOfDivisionPrice { get; set; }

        public virtual List<Invoice> Attachments { get; set; }

        #endregion

        #region Ctor

        public Invoice()
        {
            InvoiceItems = new List<InvoiceItem>();
            OrderRefrences = new List<Order>();
            AdditionalPrices = new List<InvoiceAdditionalPrice>();
            ApproveWorkFlows = new List<InvoiceWorkflowLog>();
        }


        public Invoice(InvoiceTypes invoiceType,
                       string invoiceNumber,
                       Company owner,
                       DateTime invoiceDate,
                       DivisionMethods divisionMethod,
                       AccountingTypes accountType,
                       Invoice invoiceRefrence,
                       List<Order> orderRefrences,
                       Currency currency,
                       bool isCreditor,
                       Company transporter,
                       Company supplier,
                       string description,
                       List<InvoiceItem> list,
                       List<InvoiceAdditionalPrice> invoiceAdditionalPriceList,
                       IEntityConfigurator<Invoice> invoiceConfigurator,
                       IInvoiceDomainService invoiceDomainService,
                       IInvoiceItemDomainService invoiceItemDomainService,
                       IGoodUnitConvertorDomainService goodUnitConvertorDomainService,
                       IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService,
                       IBalanceDomainService balanceDomainService)
            : this()
        {
            InvoiceType = invoiceType;
            InvoiceNumber = invoiceNumber;
            Owner = owner;
            InvoiceDate = invoiceDate;
            DivisionMethod = divisionMethod;
            AccountingType = accountType;
            InvoiceRefrence = invoiceRefrence;
            OrderRefrences = orderRefrences;
            checkOrederReferencesToBeTheSameType();

            Currency = currency;
            IsCreditor = isCreditor;
            Transporter = transporter;
            Supplier = supplier;


            TransporterId = Transporter == null ? (long?)null : Transporter.Id;
            SupplierId = Supplier == null ? (long?)null : Supplier.Id;
            InvoiceRefrenceId = InvoiceRefrence == null ? (long?)null : InvoiceRefrence.Id;
            Description = description;

            checkInvoiceAdditionalPrice(divisionMethod, invoiceAdditionalPriceList);

            UpdateInvoiceItems(list, null, balanceDomainService);
            UpdateInvoiceAdditionalPrice(invoiceAdditionalPriceList, null);

            this.invoiceConfigurator = invoiceConfigurator;
            invoiceConfigurator.Configure(this);

            invoiceAdditionalPriceDomainService.CalculateAdditionalPrice(this);

            invoiceBaseType.ValidateType(this);
            //checkInvoiceNumberToBeUnique(invoiceDomainService); //Moved to calling Add method in Application Service
            CheckInvoiceHaveInvoiceItem();
            invoiceBaseType.CheckInvoiceItemValidateQuantityAndRefrence(this, invoiceItemDomainService, goodUnitConvertorDomainService);
        }

        public void Update(string invoiceNumber,
                           DateTime invoiceDate,
                           DivisionMethods divisionMethod,
                           Invoice invoiceRefrence,
                           List<Order> orderRefrences,
                           Currency currency,
                           bool isCreditor,
                           Company transporter,
                           Company supplier,
                           string description,
                           List<InvoiceItem> invoiceItems,
                           List<InvoiceAdditionalPrice> invoiceAdditionalPriceList,
                           IInvoiceDomainService invoiceDomainService,
                           IInvoiceItemDomainService invoiceItemDomainService,
                           IGoodUnitConvertorDomainService goodUnitConvertorDomainService,
                           IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService,
                           IBalanceDomainService balanceDomainService)
        {
            if (!(this.State == States.Open || this.State == States.SubmitRejected))
                throw new BusinessRuleException("", "Invoice is not in open state.");

            InvoiceNumber = invoiceNumber;
            InvoiceDate = invoiceDate;
            DivisionMethod = divisionMethod;
            InvoiceRefrence = invoiceRefrence;
            OrderRefrences = orderRefrences;
            checkOrederReferencesToBeTheSameType();

            Currency = currency;
            Transporter = transporter;
            Supplier = supplier;
            Description = description;
            IsCreditor = isCreditor;

            checkInvoiceAdditionalPrice(divisionMethod, invoiceAdditionalPriceList);

            UpdateInvoiceItems(invoiceItems, invoiceItemDomainService, balanceDomainService);
            UpdateInvoiceAdditionalPrice(invoiceAdditionalPriceList, invoiceAdditionalPriceDomainService);

            TransporterId = Transporter == null ? (long?)null : Transporter.Id;
            SupplierId = Supplier == null ? (long?)null : Supplier.Id;
            InvoiceRefrenceId = InvoiceRefrence == null ? (long?)null : InvoiceRefrence.Id;

            // this.invoiceConfigurator = invoiceConfigurator;
            //                        invoiceConfigurator.Configure(this);

            invoiceAdditionalPriceDomainService.CalculateAdditionalPrice(this);

            invoiceBaseType.ValidateType(this);
            CheckInvoiceNumberToBeUnique(invoiceDomainService);
            CheckInvoiceHaveInvoiceItem();
            invoiceBaseType.CheckInvoiceItemValidateQuantityAndRefrence(this, invoiceItemDomainService, goodUnitConvertorDomainService);
        }

        private void UpdateInvoiceItems(List<InvoiceItem> invoiceItems,
                                        IInvoiceItemDomainService invoiceItemDomainService,
                                        IBalanceDomainService balanceDomainService)
        {
            if (InvoiceType == InvoiceTypes.Attach)
            {
                if (InvoiceRefrence == null)
                    throw new BusinessRuleException("", "Reference not Set");

                //<A.H> Moved to the outside of if block.
                //foreach (var invoiceItem in InvoiceItems.ToList())
                //{
                //    invoiceItemDomainService.DeleteInvoiceItem(invoiceItem);
                //}

                //<A.H> Moved to the outside of if block.
                //InvoiceItems = invoiceItems;
            }

            while (this.InvoiceItems.Count > 0)
            {
                invoiceItemDomainService.DeleteInvoiceItem(this.InvoiceItems[0]);
            }

            InvoiceItems = invoiceItems;

            //<A.H> : The implementation has been moved to Submit method.
            //else
            //{
            //    if (OrderRefrences == null)
            //        throw new BusinessRuleException("", "Reference not Set");
            //    var c = new CalculateChangeInOrderBlance(invoiceItemDomainService, balanceDomainService);
            //    InvoiceItems = c.Process(this, invoiceItems, OrderRefrences);
            //}
        }

        private void UpdateInvoiceAdditionalPrice(List<InvoiceAdditionalPrice> additionalPrice,
                                                  IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService)
        {
            if (AdditionalPrices != null)
            {
                while (this.AdditionalPrices.Count > 0)
                {
                    invoiceAdditionalPriceDomainService.DeleteInvoiceAdditionalPriceItem(this.AdditionalPrices[0]);
                }
            }

            if (additionalPrice.GroupBy(c => c.EffectiveFactorId).Count() != additionalPrice.Count)
                throw new BusinessRuleException("", "duplicate Additional Price Item Exception");

            AdditionalPrices = additionalPrice;
        }

        #endregion

        #region Bussiness Rules

        //BR_IN1
        public void CheckReferencesToBeValid()
        {
            invoiceBaseType.CheckRefrenceIsValid(this);
        }

        //BR_IN3
        public void CheckInvoiceNumberToBeUnique(IInvoiceDomainService invoiceDomainService)
        {
            if (!invoiceDomainService.IsInvoiceNumberUniqueForCompnay(Id, InvoiceNumber, SupplierId, TransporterId))
                throw new BusinessRuleException("BRIN3", "Invoice Number Must Be Unique For Company");
        }

        //BR_IN35
        public void CheckInvoiceHaveInvoiceItem()
        {
            if (InvoiceItems.Count == 0)
                throw new BusinessRuleException("Br_35", "Invoice Must Have Items");
        }

        void checkInvoiceAdditionalPrice(DivisionMethods divisionMethod, List<InvoiceAdditionalPrice> additionalPrice)
        {
            if (divisionMethod == DivisionMethods.None && (additionalPrice != null && additionalPrice.Count(ap => ap.Divisionable) > 0))
                throw new BusinessRuleException("", "Devision method must be specified.");

            if (divisionMethod != DivisionMethods.None && (additionalPrice != null && additionalPrice.Count(ap => ap.Divisionable) == 0))
                throw new BusinessRuleException("", "Devision method should be cleared.");

            if (additionalPrice.Count(ap => !ap.Divisionable && (ap.EffectiveFactor.Segments == null || ap.EffectiveFactor.Segments.Count == 0)) > 0)
                throw new BusinessRuleException("", "Selected Effective Factor(s) does not have any JournalEntry Segment settings.");
        }

        #endregion

        #region Methods

        #endregion

        #region InvoiceState

        public void SubmitInvoice(IInvoiceDomainService invoiceDomainService, IInvoiceItemDomainService invoiceItemDomainService, IBalanceDomainService balanceDomainService, IInventoryOperationNotifier inventoryOperationNotifier,
            IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService,
            IGoodUnitConvertorDomainService goodUnitConvertorDomainService,
            long approverId)
        {
            revertAllInvoiceItemsPricing(inventoryOperationNotifier, balanceDomainService, approverId);

            checkOrderReferencesToBeSet();

            checkInvoiceReferenceForAttachmentInvoice();

            checkOrederReferencesToBeTheSameType();

            checkInvoiceAdditionalPrice(this.DivisionMethod, this.AdditionalPrices);

            invoiceAdditionalPriceDomainService.CalculateAdditionalPrice(this);

            invoiceBaseType.ValidateType(this);
            CheckInvoiceNumberToBeUnique(invoiceDomainService);
            CheckInvoiceHaveInvoiceItem();
            checkInvoiceItemsPrices();
            invoiceBaseType.CheckInvoiceItemValidateQuantityAndRefrence(this, invoiceItemDomainService, goodUnitConvertorDomainService);

            State = States.Submitted;

            if (this.InvoiceType == InvoiceTypes.Attach)
            {
                var mainInvoice = findMainInvoice(this);

                if (mainInvoice.InvoiceType != InvoiceTypes.SupplyForDeliveredVessel)
                {
                    foreach (var orderItemBalance in mainInvoice.InvoiceItems.SelectMany(item => item.OrderItemBalances))
                    {
                        var inventoryResult = inventoryOperationNotifier.NotifySubmittingOrderItemBalance(orderItemBalance, approverId);

                        if (inventoryResult == null)
                            throw new InvalidOperation("RepriceByOrderItemBalance", "The pricing failled with an error.");

                        orderItemBalance.InventoryOperation = inventoryResult;
                    }
                }
            }
            else
            {
                Invoice foundPairedInvoice = null;

                if (this.OrderRefrences[0].OrderType == OrderTypes.PurchaseWithTransferOperations)
                {
                    foundPairedInvoice = findPairInvoiceForPurchaseWithTransferOperations(this);

                    if (foundPairedInvoice == null)
                    {
                        //State = States.Submitted;  
                        //The invoices for Purchases With Transfer Operations should be registered together, and their pricing will be effective when both of them are submitted.
                        return;
                    }
                }

                Invoice mainInvoice = this;
                Invoice pairedInvoice = null;

                if (this.InvoiceType == InvoiceTypes.Purchase) //If the current submitting invoice is Purchase, then...
                {
                    mainInvoice = this;  //The invoice of type Purchase includes main prices for pricing inventory receipt.
                    pairedInvoice = foundPairedInvoice;
                }
                else if (this.InvoiceType == InvoiceTypes.PurchaseOperations)  //If the current submitting invoice is PurchaseOperations, then...
                {
                    mainInvoice = foundPairedInvoice; //Found paired invoice is the main invoice with type of "Purchase",
                    pairedInvoice = this;       //and the current submitting invoice is the paired one for additional invoicing.
                }

                var orderItemsBalances = process(mainInvoice, pairedInvoice, this.OrderRefrences, invoiceItemDomainService, balanceDomainService);

                if (this.InvoiceType != InvoiceTypes.SupplyForDeliveredVessel)
                {
                    foreach (var orderItemBalance in orderItemsBalances)
                    {
                        var inventoryResult = inventoryOperationNotifier.NotifySubmittingOrderItemBalance(orderItemBalance, approverId);

                        if (inventoryResult == null)
                            throw new InvalidOperation("SubmitInvoiceOrderItemBalance", "Submit the OrderItemBalance of Invoice to Inventory resulted to an error.");

                        orderItemBalance.InventoryOperation = inventoryResult;
                    }
                }
            }
        }

        private void checkOrderReferencesToBeSet()
        {
            if (this.InvoiceType != InvoiceTypes.Attach && (this.OrderRefrences == null || this.OrderRefrences.Count == 0))
                throw new BusinessRuleException("", "Reference not Set");
        }

        private void checkInvoiceReferenceForAttachmentInvoice()
        {
            if(this.InvoiceType == InvoiceTypes.Attach && (this.InvoiceRefrence == null || this.InvoiceRefrence.State != States.Submitted))
                throw new BusinessRuleException("", "The attached invoice must be selected and should be final approved.");
        }

        private void checkInvoiceItemsPrices()
        {
            if (this.InvoiceItems.Any(item => item.Fee == 0 || item.Price == 0))
                throw new BusinessRuleException("", "Some Invoice items have ZERO price or fee.");
        }

        private Invoice findPairInvoiceForPurchaseWithTransferOperations(Invoice checkingInvoice)
        {
            if ((checkingInvoice.InvoiceType != InvoiceTypes.Purchase) && (checkingInvoice.InvoiceType != InvoiceTypes.PurchaseOperations))
                throw new InvalidOperation("FindPairInvoiceForTransferPurchaseInvoice", "The selected invoice is not proper type to find its pair invoice.");

            var invoiceDomainService = ServiceLocator.Current.GetInstance<IInvoiceDomainService>();
            try
            {
                return invoiceDomainService.FindPairTransferPurchaseInvoice(checkingInvoice);
            }
            finally
            {
                ServiceLocator.Current.Release(invoiceDomainService);
            }
        }

        private List<OrderItemBalance> process(Invoice invoice, Invoice pairInvoice, List<Order> orderRefrences, IInvoiceItemDomainService invoiceItemDomainService, IBalanceDomainService balanceDomainService)
        {
            var result = new List<OrderItemBalance>();

            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                InvoiceItem pairingInvoiceItem = null;

                if (pairInvoice != null)
                {
                    pairingInvoiceItem = pairInvoice.InvoiceItems.Single(pi => pi.GoodId == invoiceItem.GoodId);
                }

                result.AddRange(balanceDomainService.CreateBalanceRecordForInvoiceItem(invoiceItem, orderRefrences, pairingInvoiceItem));
            }

            return result;
        }

        public void CloseInvoice()
        {
            State = States.Closed;
        }

        private void checkOrederReferencesToBeTheSameType()
        {
            if (OrderRefrences != null && OrderRefrences.Count > 1)
            {
                OrderTypes orderType = OrderRefrences[0].OrderType;

                OrderRefrences.ForEach(o =>
                {
                    if (o.OrderType != orderType)
                        throw new BusinessRuleException("", "The selected Orders are not the same type.");
                });
            }
        }

        public void CancelInvoice(IInventoryOperationNotifier inventoryOperationNotifier, IBalanceDomainService balanceDomainService, long approverId)
        {
            State = States.Cancelled;

            if (this.InvoiceType == InvoiceTypes.Attach)
            {
                var mainInvoice = findMainInvoice(this);

                if (mainInvoice.InvoiceType != InvoiceTypes.SupplyForDeliveredVessel)
                {
                    foreach (var orderItemBalance in mainInvoice.InvoiceItems.SelectMany(item=>item.OrderItemBalances))
                    {
                        var inventoryResult = inventoryOperationNotifier.NotifySubmittingOrderItemBalance(orderItemBalance, approverId);

                        if (inventoryResult == null)
                            throw new InvalidOperation("RepriceByOrderItemBalance", "The pricing failled with an error.");

                        orderItemBalance.InventoryOperation = inventoryResult;
                    }
                }
            }
            else
            {
                if (this.InvoiceType == InvoiceTypes.Purchase)
                {
                    checkPariedInvoiceToBeCancelled();
                }

                checkAllAttachmentsToBeCancelled(this);

                revertAllInvoiceItemsPricing(inventoryOperationNotifier, balanceDomainService, approverId);
            }
        }

        private Invoice findMainInvoice(Invoice checkingInvoice)
        {
            if (checkingInvoice.InvoiceType == InvoiceTypes.Purchase || checkingInvoice.InvoiceType == InvoiceTypes.SupplyForDeliveredVessel)
                return checkingInvoice;

            if (checkingInvoice.InvoiceType == InvoiceTypes.PurchaseOperations)
                return findPairInvoiceForPurchaseWithTransferOperations(checkingInvoice);

            if (checkingInvoice.InvoiceType == InvoiceTypes.Attach)
                return findMainInvoice(checkingInvoice.InvoiceRefrence);

            return checkingInvoice;
        }

        private void checkPariedInvoiceToBeCancelled()
        {
            Invoice foundPairedInvoice = null;

            if (this.OrderRefrences[0].OrderType == OrderTypes.PurchaseWithTransferOperations)
            {
                foundPairedInvoice = findPairInvoiceForPurchaseWithTransferOperations(this);

                if (foundPairedInvoice != null)
                {
                    if (foundPairedInvoice.State != States.Cancelled)
                        throw new BusinessRuleException("", string.Format("The paring invoice with Id {0} is not cancelled.", foundPairedInvoice.Id));
                }
            }
        }

        private void checkAllAttachmentsToBeCancelled(Invoice invoice)
        {
            if (invoice.Attachments.Count(a => a.State != States.Cancelled) != 0)
                throw new BusinessRuleException("", "All attached invoices should be cancelled.\nThe attached invoices are:\n" + string.Join(", ", invoice.Attachments.Select(a => a.Id)));
        }

        private void revertAllInvoiceItemsPricing(IInventoryOperationNotifier inventoryOperationNotifier, IBalanceDomainService balanceDomainService, long approverId)
        {
            foreach (var invoiceItem in this.InvoiceItems)
            {
                foreach (var orderItemBalance in invoiceItem.OrderItemBalances)
                {
                    inventoryOperationNotifier.RevertOrderItemBalancePricing(orderItemBalance, approverId);
                }

                balanceDomainService.DeleteInvoiceItemRefrencesFromBalance(invoiceItem.Id);
            }
        }

        public void RejectSubmittedInvoice(IInventoryOperationNotifier inventoryOperationNotifier, IBalanceDomainService balanceDomainService,long approverId)
        {
            State = States.SubmitRejected;

            if (this.InvoiceType == InvoiceTypes.Attach)
            {
                var mainInvoice = findMainInvoice(this);

                if (mainInvoice.InvoiceType != InvoiceTypes.SupplyForDeliveredVessel)
                {
                    foreach (var orderItemBalance in mainInvoice.InvoiceItems.SelectMany(item => item.OrderItemBalances))
                    {
                        var inventoryResult = inventoryOperationNotifier.NotifySubmittingOrderItemBalance(orderItemBalance, approverId);

                        if (inventoryResult == null)
                            throw new InvalidOperation("RepriceByOrderItemBalance", "The pricing failled with an error.");

                        orderItemBalance.InventoryOperation = inventoryResult;
                    }
                }
            }
            else
            {
                revertAllInvoiceItemsPricing(inventoryOperationNotifier, balanceDomainService, approverId);
            }
        }

        public void SetInvoiceStateType(InvoiceState type)
        {
            InvoiceState = type;
        }

        internal void SetInvoiceType(InvoiceBaseType type)
        {
            invoiceBaseType = type;
        }

        #endregion

        //        public void DeleteAllItems(IInvoiceItemDomainService invoiceItemDomainService)
        //        {
        //            for (int index = 0; index < InvoiceItems.Count; index++)
        //            {
        //                
        //            }
        //        }

        private void checkDeleteRules()
        {
            if (!(State == States.Open /*|| State == States.SubmitRejected*/))
                throw new BusinessRuleException("BR_IN7", "Invoice Is not Open");
        }
        public void Delete(IInvoiceItemDomainService invoiceItemDomainService, IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService, IInvoiceRepository invoiceRepository)
        {
            checkDeleteRules();

            if (this.AdditionalPrices != null)
            {
                while (this.AdditionalPrices.Count > 0)
                {
                    invoiceAdditionalPriceDomainService.DeleteInvoiceAdditionalPriceItem(this.AdditionalPrices[0]);
                }
            }

            while (this.InvoiceItems.Count > 0)
            {
                invoiceItemDomainService.DeleteInvoiceItem(this.InvoiceItems[0]);
            }

            invoiceRepository.Delete(this);
        }
    }
}
