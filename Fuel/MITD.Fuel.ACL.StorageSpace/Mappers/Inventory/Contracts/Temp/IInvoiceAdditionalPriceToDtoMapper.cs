using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.InventoryServiceReference;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IInvoiceAdditionalPriceToDtoMapper : IFacadeMapper<InvoiceAdditionalPrice, InvoiceAdditionalPriceDto>
    {
        InvoiceAdditionalPriceDto MapEntityToDto(InvoiceAdditionalPrice orderItem);
        IEnumerable<InvoiceAdditionalPriceDto> MapEntityToDto(IEnumerable<InvoiceAdditionalPrice> entity);
        List<InvoiceAdditionalPriceCommand> MapModelToCommand(ObservableCollection<InvoiceAdditionalPriceDto> invoiceItems);
    }
}