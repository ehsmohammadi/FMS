#region

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.Repositories
{

    public interface IOrderRepository : IRepository<Order>
    {
        Order FindByKey(long id);

//        PageResult<Order> GetByFilter(int companyId, int orderCreatorId, OrderTypes orderType, DateTime fromDate,
//                                      DateTime toDate, int pageSize, int pageIndex);
        OrderItem SingleOrderItem(Expression<Func<OrderItem,bool>> where, ISingleResultFetchStrategy<OrderItem> fetch);
    }

}