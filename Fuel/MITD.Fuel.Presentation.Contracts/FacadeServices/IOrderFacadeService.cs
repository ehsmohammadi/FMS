using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IOrderFacadeService : IFacadeService
    {
        PageResultDto<OrderDto> GetByFilter(long? companyId, DateTime? fromDate, DateTime? toDate, long? orderId, long? orderItemId, string orderNumber, string orderTypes, int pageSize, int pageIndex, long? vesselInCompanyId, long? supplierId, long? transporterId, bool includeOrderItem = false, string orderIdList = null, bool submitedState = false);

        OrderDto Add(OrderDto data);
        OrderDto Update(OrderDto data);
        void Delete(OrderDto data);
        OrderDto GetById(long id);
        PageResultDto<OrderDto> GetAll(int pageSize, int pageIndex);
        void DeleteById(int id);

        OrderItemDto AddItem(OrderItemDto data);
        OrderItemDto UpdateItem(OrderItemDto data);
        void DeleteItem(long id, long orderItemId);


        OrderItemDto GetOrderItemById(long orderId, long orderItemId);
        MainUnitValueDto GetGoodMainUnit(long goodId, long goodUnitId, decimal value);

        List<long> GetReferencedFuelReports(long orderId);
    }
}
