using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Logic.SL.ViewModels.Invoice;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class InvoiceController :BaseController, IInvoiceController
    {
        public InvoiceController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {
        }

        public void Add(List<CompanyDto> companiesFilter)
        {
            var view = viewManager.ShowInDialog<IInvoiceView>();
            (view.ViewModel as InvoiceVM).Add(companiesFilter);
        }

        public void Edit(InvoiceDto dto,List<CompanyDto> companies)
        {
            var view = viewManager.ShowInDialog<IInvoiceView>();
            (view.ViewModel as InvoiceVM).Edit(dto, companies);
        }

        public void ShowList()
        {
            var view = this.viewManager.ShowInTabControl<IInvoiceListView>();
            (view.ViewModel as InvoiceListVM).Load();
        }

        public void ShowInvoiceReference(CompanyDto selectedCompany, InvoiceDto invoice)
        {
            var view = this.viewManager.ShowInDialog<IInvoiceReferenceLookUp>();
            (view.ViewModel as InvoiceReferenceLookUpVM).Load(selectedCompany, invoice);
        }

        public void ShowOrderReference(CompanyDto selectedCompany, InvoiceDto invoice)
        {
            var view = this.viewManager.ShowInDialog<IOrderReferenceLookUp>();
            (view.ViewModel as OrderReferenceLookUpVM).Load(selectedCompany, invoice);
        }

        public void EditItem(InvoiceItemDto invoiceItem, DivisionMethodEnum divistionMethod, decimal currencyToMainCurrencyRate, InvoiceTypeEnum invoiceType)
        {
            var view = viewManager.ShowInDialog<IInvoiceItemView>();
            (view.ViewModel as InvoiceItemVM).Load(invoiceItem, divistionMethod, currencyToMainCurrencyRate,invoiceType);
        }

        public void EditAdditionalPrice(InvoiceAdditionalPriceDto selectedAdditionalPrice, ObservableCollection<EffectiveFactorDto> effectiveFactors, decimal currencyToMainCurrencyRate)
        {
            var view = viewManager.ShowInDialog<IInvoiceAdditionalPriceView>();
            (view.ViewModel as InvoiceAdditionalPriceVM).Load(selectedAdditionalPrice,effectiveFactors,currencyToMainCurrencyRate);
        }

        public void ManageAdditionalPrice(InvoiceDto invoice, decimal currencyToMainCurrencyRate, Guid uniqId)
        {
            var view = viewManager.ShowInDialog<IInvoiceAdditionalPriceListView>();
            (view.ViewModel as InvoiceAdditionalPriceListVM).Load(invoice, currencyToMainCurrencyRate,uniqId);
        }

        public void AddAdditionalPrice(ObservableCollection<EffectiveFactorDto> effectiveFactors, decimal currencyToMainCurrencyRate, Guid uniqId)
        {
            var view = viewManager.ShowInDialog<IInvoiceAdditionalPriceView>();
            (view.ViewModel as InvoiceAdditionalPriceVM).SetCollection(effectiveFactors, currencyToMainCurrencyRate,uniqId);
        }

        public void ViewOrdersReferences(string orderNumbers)
        {
            var orderListViewForDetails = this.viewManager.ShowInDialog<IOrderListView>();
            (orderListViewForDetails.ViewModel as OrderListVM).LoadByFilter(orderNumbers);
        }

        public void ViewFuelReportDetailsReferences(string fuelReportDetailIds)
        {
            var fuelReportListViewForDetails = this.viewManager.ShowInDialog<IFuelReportListView>();
            (fuelReportListViewForDetails.ViewModel as FuelReportListVM).LoadByFilter(string.Empty, fuelReportDetailIds);

        }
    }
}
