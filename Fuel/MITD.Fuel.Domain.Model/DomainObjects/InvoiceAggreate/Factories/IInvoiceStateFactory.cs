using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates;
using MITD.Fuel.Domain.Model.Factories;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate
{
    public interface IInvoiceStateFactory : IFactory
    {
        InvoiceState CreateSubmitState();
        InvoiceState CreateOpenState();
        InvoiceState CreateCloseState();
        InvoiceState CreateCancelState();
        InvoiceState CreateSubmitRejectedState();
    }
}