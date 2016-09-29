using System.Linq;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceType
{
    public class SupplyForDeliveredVesselInvoice : InvoiceBaseType
    {
        public override void CheckRefrenceIsValid(Invoice invoice)
        {
            if (CheckInvoiceRefrenceValid(invoice))
                throw new BusinessRuleException("BR_IN1", "The selected reference is invalid.");

            if (!CheckOrderRefrenceValid(invoice))
                throw new BusinessRuleException("BR_IN1", "The order reference must be of 'Supplied for delivered Vessel'");

            CheckRefrencesOrderTypeHaveSameType(invoice);
        }

        protected override void ValidateOrderRefrences(Invoice invoice)
        {
            if (invoice.OrderRefrences.Any(c => c.OrderType != OrderTypes.SupplyForDeliveredVessel))
            {
                throw new BusinessRuleException("BR_IN1",
                                                "All the refrences should be 'Supplied for delivered Vessel'");
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

            if (IsTranspoterValid(invoice))
                throw new BusinessRuleException("BR_IN1", "Can not Have Transporter");

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