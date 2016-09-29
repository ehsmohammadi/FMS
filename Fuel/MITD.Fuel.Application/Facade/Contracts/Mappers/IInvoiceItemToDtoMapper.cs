using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IInvoiceItemToDtoMapper : IFacadeMapper<InvoiceItem,InvoiceItemDto>
    {
        InvoiceItemDto MapEntityToDto(InvoiceItem orderItem);
        IEnumerable<InvoiceItemDto> MapEntityToDto(IEnumerable<InvoiceItem> entity);
        List<InvoiceItemCommand> MapModelToCommand(ObservableCollection<InvoiceItemDto> invoiceItems);
    }

   
}