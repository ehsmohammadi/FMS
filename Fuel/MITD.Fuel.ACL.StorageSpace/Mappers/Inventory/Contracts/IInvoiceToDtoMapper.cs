using System.Collections.Generic;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IInvoiceToDtoMapper : IFacadeMapper<Invoice, InvoiceDto>
    {
        
        //InvoiceTypes MapInvoiceTypeDtoToInvoiceTypeEntity(InvoiceTypeEnum orderTypeEnum);
        InvoiceTypeEnum MapInvoiceTypeEntityToInvoiceTypeDto(InvoiceTypes orderTypes);
        //IEnumerable<InvoiceDto> MapToModelWithAllIncludes(IEnumerable<Invoice> result);
        //InvoiceDto MapToModelWithAllIncludes(Invoice order);


        //InvoiceCommand MapModelToCommandWithAllIncludes(InvoiceDto invoiceDto);

    }
}