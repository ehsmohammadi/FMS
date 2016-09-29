using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceType
{
    public class AttachInvoice : InvoiceBaseType
    {
        public override void CheckRefrenceIsValid(Invoice invoice)
        {

            if (!CheckInvoiceRefrenceValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Invoice Must Refrence to Invoice");

            if (CheckOrderRefrenceValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Order  Can Not   Refrence to Invoice");

            CheckValidInvoiceTypeRefrenced(invoice);

        }



        public override void ValidateType(Invoice invoice)
        {
            if (IsSupplierValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Can not Have Supplier");

            if (IsTranspoterValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Can not Have Transporter");


            if (invoice.DivisionMethod != invoice.InvoiceRefrence.DivisionMethod)
                throw new BusinessRuleException("BR_IN6", "Invoice Divistion Method Must Like Refrence");


            base.ValidateType(invoice);


        }



        //Br_In32
        protected static void CheckValidInvoiceTypeRefrenced(Invoice invoice)
        {
            if (invoice.InvoiceRefrence.InvoiceType == InvoiceTypes.Attach)
                throw new BusinessRuleException("Br_In32", "Attach Invoice Cant Be refrence To Another Invoice ");
            if (invoice.InvoiceRefrence.State != States.Submitted)
                throw new BusinessRuleException("Br_In32", "Attach Invoice must be finall Approved");
        }



        protected override void CheckAccountType(Invoice invoice)
        {

        }

        public override void CheckInvoiceItemValidateQuantityAndRefrence(Invoice invoice,
                                                                         IInvoiceItemDomainService invoiceItemDomainService,
                                                                         IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                if (invoiceItem.Fee == 0)
                    throw new BusinessRuleException("Br_In13", "Fee can Not be Zero");

                if (invoice.InvoiceRefrence.InvoiceItems.All(c => c.GoodId != invoiceItem.GoodId))
                    throw new BusinessRuleException("", "invalid Invoice Item");
            }
        }
    }
}