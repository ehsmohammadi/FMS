using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IInvoiceController
    {
        void Add(List<CompanyDto> companiesFilter);
        void Edit(InvoiceDto dto,List<CompanyDto> companies);

        void ShowList();

        void ShowInvoiceReference(CompanyDto selectedCompany, InvoiceDto entity);
        void ShowOrderReference(CompanyDto selectedCompany, InvoiceDto invoiceDate);

        void EditItem(InvoiceItemDto selectedInvoiceItem, DivisionMethodEnum divistionMethod, decimal currentCurrency, InvoiceTypeEnum invoiceType);
        void EditAdditionalPrice(InvoiceAdditionalPriceDto selectedAdditionalPrice, ObservableCollection<EffectiveFactorDto> effectiveFactors, decimal currencyToMainCurrencyRate);
        void ManageAdditionalPrice(InvoiceDto invoice, decimal currencyToMainCurrencyRate, Guid uniqId);
        void AddAdditionalPrice(ObservableCollection<EffectiveFactorDto> effectiveFactors, decimal currencyId, Guid uniqId);
        void ViewOrdersReferences(string orderNumbers);
        void ViewFuelReportDetailsReferences(string fuelReportDetailIds);
    }
}
