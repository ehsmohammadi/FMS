using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IInvoiceFacadeService : IFacadeService
    {
        PageResultDto<InvoiceDto> GetByFilter(long? companyId, DateTime? fromDate, DateTime? toDate, string invoiceIds, string invoiceItemIds, string invoiceNumber, long? vesselInCompanyId, string orderNumbers, InvoiceTypeEnum? invoiceType, int pageSize, int pageIndex, bool submitedState);

        InvoiceDto Add(InvoiceDto data);
        InvoiceDto Update(InvoiceDto data);
        void Delete(InvoiceDto data);
        InvoiceDto GetById(long id);
        PageResultDto<InvoiceDto> GetAll(int pageSize, int pageIndex);
        void DeleteById(int id);

//        InvoiceItemDto UpdateItem(InvoiceItemDto data);
//        void DeleteItem(InvoiceItemDto data);

//        InvoiceItemDto GetInvoiceItemById(long InvoiceId, long InvoiceItemId);
      //  MainUnitValueDto GetGoodMainUnit(long goodId, long goodUnitId, decimal value);
        IEnumerable<InvoiceItemDto> GenerateInvoiceItemForOrders(string orderList);
        IEnumerable<EffectiveFactorDto> GetAllEffectiveFactors();
        InvoiceDto CalculateAdditionalPrice(InvoiceDto invoiceEntity);
    }
}
