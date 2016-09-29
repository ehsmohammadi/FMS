#region

using System;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.Factories
{
    public interface IOrderFactory : IFactory

    {
        Order CreateFactoryOrderObject(string description, long ownerId, long? transporter, long? supplier, long? receiver, OrderTypes orderType, DateTime orderDate, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany, long userId);


        OrderItem CreateFactoryOrderItemObject(Order order, string description, decimal quantity, long goodId,
                                               long goodUnitId, GoodFullInfo goodFullDetails);
    }
}