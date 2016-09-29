using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using System.Linq;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class OrderRepository : EFRepository<Order>, IOrderRepository
    {
        private readonly IEntityConfigurator<Order> _orderConfigurator;
        private readonly IRepository<OrderItem> _orderItemRepository;

        public OrderRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {
        }

        public OrderRepository(IUnitOfWorkScope iUnitOfWorkScope,
            IEntityConfigurator<Order> orderConfigurator,
            IRepository<OrderItem> orderItemRepository)
            : base(iUnitOfWorkScope)
        {
            _orderConfigurator = orderConfigurator;
            _orderItemRepository = orderItemRepository;
        }

        public Order FindByKey(long id)
        {
            var order = base.FindByKey(id);
            return _orderConfigurator.Configure(order);
        }

        public OrderItem SingleOrderItem(Expression<Func<OrderItem, bool>> where, ISingleResultFetchStrategy<OrderItem> fetch)
        {
            return _orderItemRepository.Single(where, fetch);
        }

        //        public PageResult<Order> GetByFilter(int companyId, int orderCreatorId, OrderTypes orderType, DateTime fromDate, DateTime toDate, int pageSize, int pageIndex)
        //        {
        //          
        //
        //
        //           /* var query = this.Context.CreateObjectSet<Order>()
        //                .Include(o=>o.FromVessel)
        //                .Include(o=>o.toVessel) 
        //                .Include(o=>o.OrderItems)
        //                .Include(o=>o.Supplier)
        //                .Include(o=>o.Receiver)
        //                .AsQueryable();
        //
        //            var countQuery = this.Context.CreateObjectSet<Order>().AsQueryable();
        //
        //            //orderType
        //            if (orderType == OrderTypes.None)
        //            {
        //                query = query.Where(o => o.OrderType == orderType);
        //                countQuery = countQuery.Where(o => o.OrderType == orderType);
        //            }
        //            //companyId
        //            if (companyId != -1)
        //            {
        //                query = query.Where(o => o.SupplierId == companyId ||
        //                                         o.ReceiverId == companyId);
        //
        //                countQuery = countQuery.Where(o => o.SupplierId == companyId ||
        //                                                   o.ReceiverId == companyId);
        //            }
        //            //orderCreator
        //            //if (orderCreatorId != -1)
        //            //    query = query.Where(o => o.CreatedByUserId == orderCreatorId);
        //            //fromDate
        //            if (fromDate != DateTime.MinValue)
        //            {
        //                var from = fromDate.Date;
        //                query = query.Where(o => o.OrderDate.HasValue &&
        //                                        o.OrderDate.Value > from);
        //
        //                countQuery = countQuery.Where(o => o.OrderDate.HasValue &&
        //                                                   o.OrderDate.Value > from);
        //            }
        //            //todate
        //            if (toDate != DateTime.MinValue)
        //            {
        //                var to = toDate.Date.AddDays(1);
        //                query = query.Where(o => o.OrderDate.HasValue &&
        //                                         o.OrderDate < to);
        //                countQuery = countQuery.Where(o => o.OrderDate.HasValue &&
        //                                                   o.OrderDate < to);
        //            }
        //
        //            var totalCount = countQuery.Count();
        //
        //            //pageing 
        //            int skipCount = pageSize * pageIndex;
        //            var pagedQuery = query.OrderByDescending(o => o.OrderDate)
        //                                  .Skip(skipCount)
        //                                  .Take(pageSize);
        //
        //            
        //
        //            var list = pagedQuery.ToList();
        //            var totalPages = Convert.ToInt32(Math.Ceiling(decimal.Divide(totalCount, pageSize)));
        //
        //            var result = new PageResult<Order>
        //            {
        //                CurrentPage = pageIndex + 1,
        //                PageSize = pageSize,
        //                Result = list,
        //                TotalCount = totalCount,
        //                TotalPages = totalPages
        //            };
        //            //result.Result.ToList().ForEach(c => c.OrderItems.Add(new OrderItem()
        //            //{
        //            //    Id = 1,
        //            //    GoodId = 1,
        //            //    GoodBerandId = 1,
        //            //    GoodUnitId = 1,
        //            //    OrderId = 3
        //
        //
        //
        //            //}));
        //            return result;*/
        //        }
    }
}
