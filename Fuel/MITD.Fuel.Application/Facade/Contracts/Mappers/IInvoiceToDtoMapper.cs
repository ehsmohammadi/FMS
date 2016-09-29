#region

using System.Collections.Generic;
using MITD.Core;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Specifications;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

#endregion

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IInvoiceToDtoMapper : IFacadeMapper<Invoice, InvoiceDto>
    {
        
        InvoiceTypes MapInvoiceTypeDtoToInvoiceTypeEntity(InvoiceTypeEnum orderTypeEnum);
        InvoiceTypeEnum MapInvoiceTypeEntityToInvoiceTypeDto(InvoiceTypes orderTypes);
        IEnumerable<InvoiceDto> MapToModelWithAllIncludes(IEnumerable<Invoice> result);
        InvoiceDto MapToModelWithAllIncludes(Invoice order);


        InvoiceCommand MapModelToCommandWithAllIncludes(InvoiceDto invoiceDto);

    }
}