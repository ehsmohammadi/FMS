using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Logic.SL.Converters;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class OrderController : BaseController, IOrderController
    {
        public OrderController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {
        }

        public void Add(List<CompanyDto> dtos, List<VesselInCompanyDto> vesselInCompanyDtos)
        {
            var view = viewManager.ShowInDialog<IOrderView>();
            (view.ViewModel as OrderVM).AddNewOrder(dtos, vesselInCompanyDtos);
        }

        public void Edit(OrderDto dto, List<CompanyDto> dtos, List<VesselInCompanyDto> vesselInCompanyDtos)
        {
            var view = viewManager.ShowInDialog<IOrderView>();
            (view.ViewModel as OrderVM).Load(dto, dtos, vesselInCompanyDtos);
        }

        public void ShowList()
        {
            var view = this.viewManager.ShowInTabControl<IOrderListView>();
            (view.ViewModel as OrderListVM).Load();
        }

        public void ViewAssignedReferences(OrderAssignementReferenceTypeEnum destinationType, OrderDto orderDto)
        {
            var idsConverter = new OrderAssignementReferencesIdsConverter();

            switch (destinationType)
            {
                case OrderAssignementReferenceTypeEnum.FuelReportDetail:
                    var fuelReportDetailAssignmentReferences = idsConverter.Convert(orderDto, typeof(string), destinationType.ToString(), null) as string;

                    var fuelReportListViewForDetails = this.viewManager.ShowInDialog<IFuelReportListView>();
                        (fuelReportListViewForDetails.ViewModel as FuelReportListVM).LoadByFilter(string.Empty, fuelReportDetailAssignmentReferences);

                    break;
                case OrderAssignementReferenceTypeEnum.Invoice:
                    var invoiceAssignmentReferences = idsConverter.Convert(orderDto, typeof(string), destinationType.ToString(), null) as string;
            
                    var invoiceListViewForDetails = this.viewManager.ShowInDialog<IInvoiceListView>();
                        (invoiceListViewForDetails.ViewModel as InvoiceListVM).LoadByFilter(invoiceAssignmentReferences, string.Empty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("destinationType");
            }


        }
    }
}
