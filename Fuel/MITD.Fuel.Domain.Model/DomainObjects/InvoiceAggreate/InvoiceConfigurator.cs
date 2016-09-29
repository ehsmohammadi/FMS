#region

using System;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceType;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate
{
    public class InvoiceConfigurator : IEntityConfigurator<Invoice>
    {
        private readonly IInvoiceStateFactory invoiceStateFactory;

        #region IEntityConfigurator<Invoice> Members

        public InvoiceConfigurator(IInvoiceStateFactory invoiceStateFactory)
        {
            this.invoiceStateFactory = invoiceStateFactory;
        }

        public Invoice Configure(Invoice invoice)
        {
            ConfigureInvoiceTypes(invoice);
            ConfigureInvoiceStates(invoice);
            return invoice;
        }

        private void ConfigureInvoiceStates(Invoice invoice)
        {
            switch (invoice.State)
            {
                case States.Open:
                    invoice.SetInvoiceStateType(this.invoiceStateFactory.CreateOpenState());
                    break;
                case States.Submitted:
                    invoice.SetInvoiceStateType(this.invoiceStateFactory.CreateSubmitState());
                    break;
                case States.Closed:
                    invoice.SetInvoiceStateType(this.invoiceStateFactory.CreateCloseState());

                    break;
                case States.Cancelled:
                    invoice.SetInvoiceStateType(this.invoiceStateFactory.CreateCancelState());
                    break;
                case States.SubmitRejected:
                    invoice.SetInvoiceStateType(this.invoiceStateFactory.CreateSubmitRejectedState());
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ConfigureInvoiceTypes(Invoice Invoice)
        {
            switch (Invoice.InvoiceType)
            {
                case InvoiceTypes.Purchase:
                    Invoice.SetInvoiceType(new PurchaseInvoice());
                    break;
                case InvoiceTypes.PurchaseOperations:
                    Invoice.SetInvoiceType(new TransferInvoice());
                    break;
                case InvoiceTypes.Attach:
                    Invoice.SetInvoiceType(new AttachInvoice());
                    break;
                case InvoiceTypes.SupplyForDeliveredVessel:
                    Invoice.SetInvoiceType(new SupplyForDeliveredVesselInvoice());
                    break;
                default:
                    throw new InvalidArgument("InvoiceType");
            }
        }




        #endregion
    }
}