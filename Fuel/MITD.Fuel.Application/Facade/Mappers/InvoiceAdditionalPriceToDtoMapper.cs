using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public class InvoiceAdditionalPriceToDtoMapper : BaseFacadeMapper<InvoiceAdditionalPrice, InvoiceAdditionalPriceDto>,
                                                     IInvoiceAdditionalPriceToDtoMapper
    {
        public InvoiceAdditionalPriceDto MapEntityToDto(InvoiceAdditionalPrice item)
        {
            return base.MapToModel(item);
        }

        public IEnumerable<InvoiceAdditionalPriceDto> MapEntityToDto(IEnumerable<InvoiceAdditionalPrice> entities)
        {
            return entities.Select(MapEntityToDto).ToList();
        }

        public InvoiceAdditionalPriceCommand MapModelToCommand(InvoiceAdditionalPriceDto invoiceItem)
        {
            return new InvoiceAdditionalPriceCommand
                (invoiceItem.EffectiveFactorId, invoiceItem.Price, invoiceItem.Description, invoiceItem.Divisionable);
        }

        public List<InvoiceAdditionalPriceCommand> MapModelToCommand(ObservableCollection<InvoiceAdditionalPriceDto> invoiceItems)
        {
            return invoiceItems.Select(MapModelToCommand).ToList();
        }
    }
}