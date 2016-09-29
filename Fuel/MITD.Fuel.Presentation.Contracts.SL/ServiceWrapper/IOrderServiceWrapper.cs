using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IOrderServiceWrapper : IServiceWrapper
    {
        void GetByFilter(Action<PageResultDto<OrderDto>, Exception> action, long? companyId, DateTime? fromDate, DateTime? toDate, long? orderId, long? orderItemId, string orderNumber, string orderTypes, int pageSize, int pageIndex, long? vesselInCompanyId, long? supplierId = null, long? transporterId = null, bool includeOrderItem = false, string orderIdList = null, bool submitedState = false);

         void GetAll(Action<PageResultDto<OrderDto>, Exception> action, string methodName, int pageSize,
                          int pageIndex);

         void GetById(Action<OrderDto, Exception> action, long id);

        void Add(Action<OrderDto, Exception> action, OrderDto ent);

        void Update(Action<OrderDto, Exception> action, OrderDto ent);

        void Delete(Action<string, Exception> action, long id);

        void AddItem(Action<OrderItemDto, Exception> action, OrderItemDto ent);

        void UpdateItem(Action<OrderItemDto, Exception> action, OrderItemDto ent);

        void DeleteItem(Action<string, Exception> action, OrderItemDto ent);

        void GetItem(Action<OrderItemDto, Exception> action, long orderId,long orderItemId);
      
        void GetMainUnit(Action<MainUnitValueDto, Exception> action, long goodId, long goodUnitId, decimal value);
    }
}
