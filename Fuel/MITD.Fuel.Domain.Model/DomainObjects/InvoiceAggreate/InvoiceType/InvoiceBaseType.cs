using System;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Specifications;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceType
{
    public abstract class InvoiceBaseType
    {
        public virtual void CheckRefrenceIsValid(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        //BR_IN4 ,BR_IN5 , BR_IN6 , BR_IN40
        public virtual void ValidateType(Invoice invoice)
        {
            if (invoice.Currency == null)
                throw new BusinessRuleException("BR_IN2", "Currency Must Be Selected");
            if (invoice.InvoiceDate == null || invoice.InvoiceDate == DateTime.MinValue)
                throw new BusinessRuleException("BR_IN2", "Invoice Date Must Have Value");

            if (string.IsNullOrEmpty(invoice.InvoiceNumber))
                throw new BusinessRuleException("BR_IN2", "Invoice Number Must Have Value");



            CheckAccountType(invoice);
            CheckRefrenceIsValid(invoice);

            ValidateSum(invoice);

            ValidateDivisionMethod(invoice);

            validateInvoiceDate(invoice);
        }

        private void validateInvoiceDate(Invoice invoice)
        {
            if (invoice.InvoiceDate > DateTime.Now)
                throw new BusinessRuleException("", "The Invoice Date is after today.");
        }

        private void ValidateSum(Invoice invoice)
        {
            if (invoice.DivisionMethod != DivisionMethods.Direct)
                return;

            var sumOfInvoiceItemDivision = invoice.InvoiceItems.Sum(c => c.DivisionPrice);
            var sumOfDivisionableAdditionalPrice = invoice.AdditionalPrices.Where(c => c.Divisionable).Sum(c => c.Price);
            if (sumOfInvoiceItemDivision != sumOfDivisionableAdditionalPrice)
            {
                throw new BusinessRuleException("BR_IN6", "Invoice Item Divistion Price Not Valid");
            }
        }

        //Br_IN19
        protected virtual void CheckAccountType(Invoice invoice)
        {

        }

        //Br_IN20
        protected virtual void ValidateOrderRefrences(Invoice invoice)
        {
            throw new NotImplementedException();
        }


        //BR_IN26
        protected static void CheckRefrencesOrderTypeHaveSameType(Invoice invoice)
        {
            if (invoice.OrderRefrences.GroupBy(c => c.OrderType).Count() > 1)
                throw new BusinessRuleException("BR_IN26", "Order Reference Must Have Same Type");

            if (invoice.OrderRefrences.GroupBy(c => c.TransporterId).Count() > 1)
                throw new BusinessRuleException("BR_IN100", "Order Reference Must Have Same Transporter");

            if (invoice.OrderRefrences.GroupBy(c => c.SupplierId).Count() > 1)
                throw new BusinessRuleException("BR_IN100", "Order Reference Must Have Same Supplier");

        }

        public bool CreditAccoutType(Invoice invoice)
        {
            return (invoice.AccountingType == AccountingTypes.Credit);
        }

        public bool DebitAccoutType(Invoice invoice)
        {
            return (invoice.AccountingType == AccountingTypes.Debit);
        }



        protected static bool CheckInvoiceRefrenceValid(Invoice invoice)
        {
            return (invoice.InvoiceRefrence != null);

        }


        protected static bool CheckOrderRefrenceValid(Invoice invoice)
        {
            return (invoice.OrderRefrences != null && invoice.OrderRefrences.Count > 0);

        }

        protected static bool IsSupplierValid(Invoice invoice)
        {
            return (invoice.Supplier != null && invoice.Supplier.Id > 0);

        }

        protected static bool IsTranspoterValid(Invoice invoice)
        {
            return (invoice.TransporterId.HasValue && invoice.TransporterId > 0);

        }

        public virtual void CheckInvoiceItemValidateQuantityAndRefrence(Invoice invoice, IInvoiceItemDomainService invoiceItemDomainService, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            throw new NotImplementedException();
        }




        private static void ValidateDivisionMethod(Invoice invoice)
        {
            if (invoice.TotalOfDivisionPrice > 0 && invoice.DivisionMethod == DivisionMethods.None)
                throw new BusinessRuleException("BR_IN1", "Must have Divide Mewthod ");
        }

        //protected void CheckInvoiceItemValidateQuantityAndRefrenceWithOrder(Invoice invoice, IInvoiceItemDomainService invoiceItemDomainService, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        //{

        //    var refrencedOrder = invoiceItemDomainService.GetRefrencedOrders(invoice.OrderRefrences.Select(c => c.Id).ToList()).ToList();

        //    //            foreach (var invoiceItem in invoice.InvoiceItems)
        //    //            {
        //    //                if (invoiceItem.Fee == 0)
        //    //                    throw new BusinessRuleException("Br_In13", "Fee can Not be Zero");
        //    //
        //    //                InvoiceItem item = invoiceItem;
        //    //                var allOrderItemsWithThisGoodId = refrencedOrder.SelectMany(c => c.OrderItems).Where(c => c.GoodId == item.GoodId).ToList();
        //    //                if (!allOrderItemsWithThisGoodId.Any())
        //    //                    throw new BusinessRuleException("Br_In10", "Invalid Invoice Item");
        //    //
        //    //                decimal totalOrder;
        //    //                decimal totalRecipt;
        //    //                decimal totalinvoice;
        //    //
        //    //                if (allOrderItemsWithThisGoodId.GroupBy(c => c.MeasuringUnitId).Count() > 1)
        //    //                {
        //    //                    totalOrder = allOrderItemsWithThisGoodId.Sum
        //    //                        (c => goodUnitConvertorDomainService.GetUnitValueInMainUnit(c.GoodId, c.MeasuringUnitId, c.Quantity).Value);
        //    //                    totalRecipt = allOrderItemsWithThisGoodId.SelectMany(c => c.OrderItemBalances).Sum
        //    //                        (
        //    //                            c =>
        //    //                                goodUnitConvertorDomainService.GetUnitValueInMainUnit
        //    //                                (c.OrderItem.GoodId, c.FuelReportDetail.MeasuringUnitId, c.FuelReportCount).Value);
        //    //                    
        //    //                    totalinvoice = allOrderItemsWithThisGoodId.SelectMany(c => c.OrderItemBalances).Sum
        //    //                        (
        //    //                            c =>
        //    //                                goodUnitConvertorDomainService.GetUnitValueInMainUnit
        //    //                                (c.OrderItem.GoodId, c.InvoiceItem.MeasuringUnitId, c.InvoiceItemCount).Value);
        //    //                
        //    //                }
        //    //                else
        //    //                {
        //    //                    totalOrder = allOrderItemsWithThisGoodId.Sum(c => c.Quantity);
        //    //                    totalRecipt = allOrderItemsWithThisGoodId.SelectMany(c => c.OrderItemBalances).Sum(c => c.FuelReportCount);
        //    //                    totalinvoice = allOrderItemsWithThisGoodId.SelectMany(c => c.OrderItemBalances).Sum(c => c.InvoiceItemCount);
        //    //                }
        //    //               // QuantityIsValidBalanceForInvoice(invoiceItem.Quantity, totalOrder, totalRecipt, totalinvoice);
        //    //            }

        //}




        public void QuantityIsValidBalanceForInvoice(decimal requested, decimal orderd, decimal recived, decimal invoiced)
        {
            if (requested > recived - invoiced)
                throw new BusinessRuleException("Br_In16", "Quantity Bigar than Recived Subtract Invoiced");
        }
    }
}
