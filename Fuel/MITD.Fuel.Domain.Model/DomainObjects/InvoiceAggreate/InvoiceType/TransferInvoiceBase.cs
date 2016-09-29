using System.Linq;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceType
{
    public class TransferInvoice : InvoiceBaseType
    {
        public override void CheckRefrenceIsValid(Invoice invoice)
        {

            if (CheckInvoiceRefrenceValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Invoice Can Not  Refrence to Invoice");

            if (!CheckOrderRefrenceValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Order  Must  Refrence to Invoice");


            CheckRefrencesOrderTypeHaveSameType(invoice);



        }

        protected override void ValidateOrderRefrences(Invoice invoice)
        {
            if (invoice.OrderRefrences.Any(c => !
                ((c.OrderType == OrderTypes.InternalTransfer || c.OrderType == OrderTypes.Transfer) &&
                c.State == States.Submitted)
                ))
            {
                throw new BusinessRuleException("BR_IN1", "Final Approved Transfer Or InternalTransfer Must Be refrence to Invoice");
            }

        }

        public override void CheckInvoiceItemValidateQuantityAndRefrence(Invoice invoice, IInvoiceItemDomainService invoiceItemDomainService, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            //CheckInvoiceItemValidateQuantityAndRefrenceWithOrder(invoice, invoiceItemDomainService,goodUnitConvertorDomainService);

        }
      
        
        public override void ValidateType(Invoice invoice)
        {
            if (!IsSupplierValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Must Have Supplier");

            if (!IsTranspoterValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Must Have Transporter");

      

            if (invoice.IsCreditor)
                throw new BusinessRuleException("BR_IN40", "Invalid Is Credit");


            base.ValidateType(invoice);

        }

        protected override void CheckAccountType(Invoice invoice)
        {
            if (!DebitAccoutType(invoice))
                throw new BusinessRuleException("Br_In19", "Accoutn Type Must Debit");

        }
    }
}