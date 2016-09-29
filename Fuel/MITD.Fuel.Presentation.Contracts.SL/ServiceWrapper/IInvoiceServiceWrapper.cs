#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;

#endregion

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IInvoiceServiceWrapper : IServiceWrapper
    {
        void GetByFilter(Action<PageResultDto<InvoiceDto>, Exception> action, long? companyId, DateTime? fromDate, DateTime? toDate, string invoiceIds, string invoiceItemIds, string invoiceNumber, long? vesselInCompanyId, string orderNumbers, InvoiceTypeEnum? invoiceType, int pageSize, int pageIndex, bool submitedState);

        void GetAll(Action<PageResultDto<InvoiceDto>, Exception> action, string methodName, int pageSize,
                    int pageIndex);

        void GetById(Action<InvoiceDto, Exception> action, long id);

        void Add(Action<InvoiceDto, Exception> action, InvoiceDto ent);

        void Update(Action<InvoiceDto, Exception> action, InvoiceDto ent);

        void Delete(Action<string, Exception> action, long id);

        void UpdateItem(Action<InvoiceItemDto, Exception> action, InvoiceItemDto ent);

        void DeleteItem(Action<string, Exception> action, InvoiceItemDto ent);

        void GetItem(Action<InvoiceItemDto, Exception> action, long InvoiceId, long InvoiceItemId);

        void GetMainUnit(Action<MainUnitValueDto, Exception> action, long goodId, long goodUnitId, decimal value);

        void GenerateItemByFilter(Action<IEnumerable<InvoiceItemDto>, Exception> action, List<long> orderIdList);
        
        void CalculateAdditionalPrice(Action<InvoiceDto, Exception> action, InvoiceDto ent);
        void GetEffectiveFactors(Action<ObservableCollection<EffectiveFactorDto>, Exception> action);
    }
}