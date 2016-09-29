using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IInvoiceItemToDtoMapper : IFacadeMapper<InvoiceItem, InvoiceItemDto>
    {
        InvoiceItemDto MapEntityToDto(InvoiceItem orderItem);
        IEnumerable<InvoiceItemDto> MapEntityToDto(IEnumerable<InvoiceItem> entity);
        List<InvoiceItemCommand> MapModelToCommand(ObservableCollection<InvoiceItemDto> invoiceItems);
    }


}