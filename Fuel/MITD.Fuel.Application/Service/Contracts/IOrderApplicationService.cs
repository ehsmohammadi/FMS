using System;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface IOrderApplicationService : IApplicationService
    {

        void DeleteById(long id);

        Order Add(string description, long ownerId, long? transporter, long? supplier, long? receiver, OrderTypes orderType, long? fromVesselInCompanyId, long? toVesselInCompanyId, long userId, DateTime orderDate);

        Order Update(long id, string description, OrderTypes orderType, long ownerId, long? transporter, long? supplier, long? receiver, long? fromVesselInCompanyId, long? toVesselInCompanyId, DateTime orderDate);





        void DeleteItem(long orderId, long orderItemId);

        OrderItem AddItem(long orderId, string description, decimal quantity,  long goodId, long unitId, long? assigneBuessinessPartyForGoodId);

        OrderItem UpdateItem(long id, long orderId, string description, decimal quantity, long goodId, long unitId, long? assigneBuessinessPartyForGoodId);



    }
}