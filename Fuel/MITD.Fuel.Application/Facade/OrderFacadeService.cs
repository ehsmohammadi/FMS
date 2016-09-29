using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Castle.Core;
using Castle.Core.Internal;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Application.Service.Security;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public partial class OrderFacadeService : IOrderFacadeService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGoodUnitConvertorDomainService _goodUnitConvertorDomainService;
        private readonly IMainUnitVlaueTomainUnitVlaueDtoMapper _mainUnitVlaueTomainUnitVlaueDtoMapper;
        private readonly IUnitOfWorkScope _unitOfWorkScope;

        #region props
        private List<OrderDto> Data;
        private readonly IOrderApplicationService _orderAppService;
        private readonly ICompanyDomainService _companyDomainService;
        private readonly IOrderToDtoMapper _orderDtoMapper;
        private readonly IOrderItemToDtoMapper _itemToDtoMapper;
        private readonly IFuelUserRepository fuelUserRepository;

        #endregion

        #region ctor

        public OrderFacadeService(IOrderApplicationService orderAppService,
                                  ICompanyDomainService companyDomainService,
                                  IOrderToDtoMapper orderDtoMapper,
                                  IOrderItemToDtoMapper itemToDtoMapper,
                                  IOrderRepository orderRepository,
            IGoodUnitConvertorDomainService goodUnitConvertorDomainService,
            IMainUnitVlaueTomainUnitVlaueDtoMapper mainUnitVlaueTomainUnitVlaueDtoMapper
            , IUnitOfWorkScope unitOfWorkScope, IFuelUserRepository fuelUserRepository)
        {
            _orderRepository = orderRepository;
            _goodUnitConvertorDomainService = goodUnitConvertorDomainService;
            _mainUnitVlaueTomainUnitVlaueDtoMapper = mainUnitVlaueTomainUnitVlaueDtoMapper;
            _unitOfWorkScope = unitOfWorkScope;
            this.fuelUserRepository = fuelUserRepository;
            _orderAppService = orderAppService;
            _companyDomainService = companyDomainService;
            _orderDtoMapper = orderDtoMapper;
            _itemToDtoMapper = itemToDtoMapper;
        }

        #endregion

        #region methods

        public OrderDto Add(OrderDto data)
        {
            var updatedEnt = _orderAppService.Add(data.Description,
                                                    data.Owner.Id,
                                                    data.Transporter != null && data.Transporter.Id != 0 ? data.Transporter.Id : (long?)null,
                                                    data.Supplier != null && data.Supplier.Id != 0 ? data.Supplier.Id : (long?)null,
                                                    data.Receiver != null && data.Receiver.Id != 0 ? data.Receiver.Id : (long?)null,
                                                    (OrderTypes)(int)data.OrderType,
                                                    data.FromVesselInCompany != null && data.FromVesselInCompany.Id != 0 ? data.FromVesselInCompany.Id : (long?)null,
                                                    data.ToVesselInCompany != null && data.ToVesselInCompany.Id != 0 ? data.ToVesselInCompany.Id : (long?)null,
                                                    this.getFuelUserId(),
                                                    data.OrderDate);

            var result = _orderDtoMapper.MapToModel(updatedEnt);

            return result;
        }

        //TODO Sholde Check Type
        public OrderDto Update(OrderDto data)
        {
            var updatedEnt = _orderAppService.Update(data.Id, data.Description,
                                                     (OrderTypes)(int)data.OrderType,
                                                     data.Owner.Id,
                                                     data.Transporter != null && data.Transporter.Id != 0 ? data.Transporter.Id : (long?)null,
                                                     data.Supplier != null && data.Supplier.Id != 0 ? data.Supplier.Id : (long?)null,
                                                     data.Receiver != null && data.Receiver.Id != 0 ? data.Receiver.Id : (long?)null,
                                                     data.FromVesselInCompany != null && data.FromVesselInCompany.Id != 0 ? data.FromVesselInCompany.Id : (long?)null,
                                                     data.ToVesselInCompany != null && data.ToVesselInCompany.Id != 0 ? data.ToVesselInCompany.Id : (long?)null, 
                                                     data.OrderDate);

            var result = _orderDtoMapper.MapToModel(updatedEnt);

            return result;
        }

        public void Delete(OrderDto data)
        {
            _orderAppService.DeleteById(data.Id);
        }

        public OrderDto GetById(long id)
        {
            var fetch = new SingleResultFetchStrategy<Order>()
                .Include(o => o.FromVesselInCompany)
                .Include(o => o.ToVesselInCompany)
                .Include(o => o.OrderItems)
                .Include(o => o.Supplier)
                .Include(o => o.Receiver)
                .Include(o => o.Transporter)
                .Include(o => o.Owner)
                .Include(o => o.ApproveWorkFlows);

            var ordersPageResult = _orderRepository.Single(o => o.Id == id, fetch);

            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

            var orderDtos = _orderDtoMapper.MapToModelWithAllIncludes(ordersPageResult, (o, d) =>
            {
                var destinationIds = fuelReportDomainService.FindFuelReportDetailsWithReceiveByOrder(o.Id);
                d.DestinationReferences = new ObservableCollection<OrderAssignmentReferenceDto>(destinationIds.Select(i => new OrderAssignmentReferenceDto() { DestinationType = OrderAssignementReferenceTypeEnum.FuelReportDetail, DestinationId = i }));

                o.OrderItems.SelectMany(i => i.OrderItemBalances)
                    .Select(b =>
                        new OrderAssignmentReferenceDto()
                        {
                            DestinationType = OrderAssignementReferenceTypeEnum.Invoice,
                            DestinationId = b.InvoiceItem.InvoiceId
                        })
                    .ForEach(d.DestinationReferences.Add);

                o.OrderItems.SelectMany(i => i.OrderItemBalances)
                    .Where(b => b.PairingInvoiceItemId.HasValue)
                    .Select(b =>
                        new OrderAssignmentReferenceDto()
                        {
                            DestinationType = OrderAssignementReferenceTypeEnum.Invoice,
                            DestinationId = b.PairingInvoiceItem.InvoiceId
                        })
                    .ForEach(d.DestinationReferences.Add);
            });

            return orderDtos;
        }

        // todo: input parameters must be converted to IPageCritera
        public PageResultDto<OrderDto> GetAll(int pageSize, int pageIndex)
        {
            var fetch = new ListFetchStrategy<Order>()
                .Include(o => o.FromVesselInCompany)
                .Include(o => o.ToVesselInCompany)
                .Include(o => o.OrderItems)
                .Include(o => o.Supplier)
                .Include(o => o.Receiver)
                .Include(o => o.Transporter)
                .Include(o => o.Owner)
                .Include(o => o.ApproveWorkFlows)
                .OrderByDescending(o => o.OrderDate)
                .WithPaging(pageSize, pageIndex);

            _orderRepository.GetAll(fetch);

            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

            var finalResult = new PageResultDto<OrderDto>
                                  {
                                      CurrentPage = pageIndex,
                                      PageSize = pageSize,
                                      Result = _orderDtoMapper.MapToModel(fetch.PageCriteria.PageResult.Result.ToList(), (o, d) =>
                                      {
                                          var destinationIds = fuelReportDomainService.FindFuelReportDetailsWithReceiveByOrder(o.Id);
                                          d.DestinationReferences = new ObservableCollection<OrderAssignmentReferenceDto>(destinationIds.Select(i => new OrderAssignmentReferenceDto() { DestinationType = OrderAssignementReferenceTypeEnum.FuelReportDetail, DestinationId = i }));


                                          o.OrderItems.SelectMany(i => i.OrderItemBalances)
                                              .Select(b =>
                                                  new OrderAssignmentReferenceDto()
                                                  {
                                                      DestinationType = OrderAssignementReferenceTypeEnum.Invoice,
                                                      DestinationId = b.InvoiceItem.InvoiceId
                                                  })
                                              .ForEach(d.DestinationReferences.Add);

                                          o.OrderItems.SelectMany(i => i.OrderItemBalances)
                                              .Where(b => b.PairingInvoiceItemId.HasValue)
                                              .Select(b =>
                                                  new OrderAssignmentReferenceDto()
                                                  {
                                                      DestinationType = OrderAssignementReferenceTypeEnum.Invoice,
                                                      DestinationId = b.PairingInvoiceItem.InvoiceId
                                                  })
                                              .ForEach(d.DestinationReferences.Add);
                                      }).ToList(),
                                      TotalCount = fetch.PageCriteria.PageResult.TotalCount,
                                      TotalPages = fetch.PageCriteria.PageResult.TotalPages
                                  };

            return finalResult;
        }

        public void DeleteById(int id)
        {
            _orderAppService.DeleteById(id);
        }

        public PageResultDto<OrderDto> GetByFilter(long? companyId, DateTime? fromDate, DateTime? toDate, long? orderId, long? orderItemId, string orderNumber, string orderTypes, int pageSize, int pageIndex, long? vesselInCompanyId, long? supplierId, long? transporterId, bool includeOrderItem = false, string orderIdList = null, bool submitedState = false)
        {
            //var fromDateParam = fromDate.HasValue ? fromDate.Value.Date : DateTime.MinValue;
            var toDateParam = toDate.HasValue ? (DateTime?)toDate.Value.Date.AddDays(1) : null;
            var fetch =
                new ListFetchStrategy<Order>()
                    .Include(o => o.FromVesselInCompany)
                    .Include(o => o.ToVesselInCompany)
                    .Include(o => o.OrderItems)
                    .Include(o => o.Supplier)
                    .Include(o => o.Receiver)
                    .Include(o => o.Transporter)
                    .Include(o => o.Owner)
                    .Include(o => o.ApproveWorkFlows)
                    .OrderByDescending(o => o.OrderDate)
                    .WithPaging(pageSize, pageIndex + 1);

            var orderTypesCollection = new List<OrderTypes>();
            if (!string.IsNullOrEmpty(orderTypes))
            {
                var orderTypesParts = orderTypes.Split(',');
                orderTypesCollection.AddRange(orderTypesParts.Where(c => c != "0").Select(item => (OrderTypes)int.Parse(item)));
            }

            var orderIdListCollection = new List<long>();
            if (!string.IsNullOrEmpty(orderIdList))
            {
                var orderIdsParts = orderIdList.Split(',');
                orderIdListCollection.AddRange(orderIdsParts.Where(c => c != "0").Select(long.Parse));
                _orderRepository.Find(o => orderIdListCollection.Count == 0 || orderIdListCollection.Contains(o.Id), fetch);
            }
            else
            {
                var orderNumberListToSearch = string.IsNullOrEmpty(orderNumber) ? new List<string>() : orderNumber.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToList();
                _orderRepository.Find
                    (
                        o =>
                            (orderTypesCollection.Count == 0 || orderTypesCollection.Contains(o.OrderType))
                                && (orderId == null || o.Id == orderId)
                                && (orderItemId == null || o.OrderItems.Any(i => i.Id == orderItemId))
                                && (companyId == -1 || companyId == null || o.OwnerId == companyId)
                                && (!submitedState || (o.State == States.Submitted || o.State == States.Closed))
                                && (!fromDate.HasValue || o.OrderDate >= fromDate)
                                && (!toDateParam.HasValue || o.OrderDate <= toDateParam)
                                && (supplierId == 0 || supplierId == null || o.SupplierId == supplierId)
                                && (transporterId == 0 || transporterId == null || o.TransporterId == transporterId)
                                && (string.IsNullOrEmpty(orderNumber) || orderNumberListToSearch.Contains(o.Code))
                                && (!vesselInCompanyId.HasValue || o.ToVesselInCompanyId == vesselInCompanyId)
                                , fetch);

            }

            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

            var ordersPageResult = fetch.PageCriteria.PageResult;

            var result = new PageResultDto<OrderDto>
                             {
                                 CurrentPage = ordersPageResult.CurrentPage,
                                 PageSize = ordersPageResult.PageSize,
                                 Result = _orderDtoMapper.MapToModelWithAllIncludes(ordersPageResult.Result,
                                  (o, d) =>
                                  {
                                          //var destinationIds = fuelReportDomainService.FindFuelReportDetailsWithReceiveByOrder(o.Id);
                                          //d.DestinationReferences = new ObservableCollection<OrderAssignmentReferenceDto>(destinationIds.Select(i => new OrderAssignmentReferenceDto() { DestinationType = OrderAssignementReferenceTypeEnum.FuelReportDetail, DestinationId = i }));

                                          //o.OrderItems.SelectMany(i => i.OrderItemBalances)
                                          //    .Select(b =>
                                          //        new OrderAssignmentReferenceDto()
                                          //        {
                                          //            DestinationType = OrderAssignementReferenceTypeEnum.Invoice,
                                          //            DestinationId = b.InvoiceItem.InvoiceId
                                          //        })
                                          //    .ForEach(d.DestinationReferences.Add);

                                          //o.OrderItems.SelectMany(i => i.OrderItemBalances)
                                          //    .Where(b => b.PairingInvoiceItemId.HasValue)
                                          //    .Select(b =>
                                          //        new OrderAssignmentReferenceDto()
                                          //        {
                                          //            DestinationType = OrderAssignementReferenceTypeEnum.Invoice,
                                          //            DestinationId = b.PairingInvoiceItem.InvoiceId
                                          //        })
                                          //    .ForEach(d.DestinationReferences.Add);

                                          var operatedFuelReportDetailIds = o.OrderItems.SelectMany(i => i.OrderItemOperatedQuantities.Select(oq => oq.FuelReportDetailId)).Distinct().ToList();
                                          d.DestinationReferences = new ObservableCollection<OrderAssignmentReferenceDto>(operatedFuelReportDetailIds.Select(id => 
                                              new OrderAssignmentReferenceDto()
                                              {
                                                  DestinationType = OrderAssignementReferenceTypeEnum.FuelReportDetail, 
                                                  DestinationId = id
                                              }));

                                          var invoiceIds = o.OrderItems.SelectMany(i => i.OrderItemBalances.Select(ob => ob.InvoiceItem.InvoiceId)).ToList();
                                          invoiceIds.AddRange(o.OrderItems.SelectMany(i => i.OrderItemBalances.SelectMany(ob => ob.InvoiceItem.Invoice.Attachments.Select(a => a.Id))).ToList());
                                          invoiceIds.AddRange(o.OrderItems.SelectMany(i => i.OrderItemBalances.Where(ob => ob.PairingInvoiceItemId.HasValue).Select(ob => ob.PairingInvoiceItem.Invoice.Id)).ToList());
                                          invoiceIds.AddRange(o.OrderItems.SelectMany(i => i.OrderItemBalances.Where(ob => ob.PairingInvoiceItemId.HasValue).SelectMany(ob => ob.PairingInvoiceItem.Invoice.Attachments.Select(a => a.Id))).ToList());

                                          invoiceIds.Distinct().Select(id => 
                                              new OrderAssignmentReferenceDto()
                                              {
                                                  DestinationType = OrderAssignementReferenceTypeEnum.Invoice,
                                                            DestinationId = id
                                              })
                                          .ForEach(d.DestinationReferences.Add);
                                      }
                                  ).ToList(),
                                 TotalCount = ordersPageResult.TotalCount,
                                 TotalPages = ordersPageResult.TotalPages,
                             };
            return result;
        }

        private long getFuelUserId()
        {
            var currentUserId = SecurityApplicationService.GetCurrentUserId();
            var currentCompanyId = SecurityApplicationService.GetCurrentUserCompanyId();

            return fuelUserRepository.Single(fu => fu.IdentityId == currentUserId && fu.CompanyId == currentCompanyId).Id;
        }

        #endregion

        #region OrderItem
        public OrderItemDto AddItem(OrderItemDto data)
        {
            return _itemToDtoMapper.MapToModel(
                _orderAppService.AddItem(data.OrderId, data.Description, data.Quantity, data.Good.Id, data.Good.Unit.Id, data.AssigneBuessinessPartyForGoodId));
        }

        public OrderItemDto UpdateItem(OrderItemDto data)
        {
            return _itemToDtoMapper.MapToModel(
                _orderAppService.UpdateItem(
                data.Id, data.OrderId, data.Description, data.Quantity, data.Good.Id, data.Good.Unit.Id, 0));
        }

        public void DeleteItem(long id, long orderItemId)
        {
            this._orderAppService.DeleteItem(id, orderItemId);
        }

        public OrderItemDto GetOrderItemById(long orderId, long orderItemId)
        {
            var order = this._orderRepository.FindByKey(orderId);
            var orderItem = order.OrderItems.SingleOrDefault(c => c.Id == orderItemId);
            return _itemToDtoMapper.MapToModel(orderItem);
        }

        public MainUnitValueDto GetGoodMainUnit(long goodId, long goodUnitId, decimal value)
        {
            return _mainUnitVlaueTomainUnitVlaueDtoMapper.MapToModel(_goodUnitConvertorDomainService.GetUnitValueInMainUnit(goodId, goodUnitId, value));
        }

        public List<long> GetReferencedFuelReports(long orderId)
        {
            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

            return fuelReportDomainService.FindFuelReportDetailsWithReceiveByOrder(orderId);
        }

        #endregion
    }


}