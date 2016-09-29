using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IInvoiceAdditionalPriceToDtoMapper : IFacadeMapper<InvoiceAdditionalPrice, InvoiceAdditionalPriceDto>
    {
        InvoiceAdditionalPriceDto MapEntityToDto(InvoiceAdditionalPrice orderItem);
        IEnumerable<InvoiceAdditionalPriceDto> MapEntityToDto(IEnumerable<InvoiceAdditionalPrice> entity);
        List<InvoiceAdditionalPriceCommand> MapModelToCommand(ObservableCollection<InvoiceAdditionalPriceDto> invoiceItems);
    }
}