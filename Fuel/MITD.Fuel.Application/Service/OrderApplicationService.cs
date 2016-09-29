#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.Repositories;

#endregion

namespace MITD.Fuel.Application.Service
{
    public class OrderApplicationService : IOrderApplicationService
    {
        private readonly ICompanyDomainService companyDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IOrderFactory iOrderFactory;
        private readonly IEntityConfigurator<Order> orderConfigurator;
        private readonly IOrderItemDomainService orderItemDomainService;
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWorkScope unitOfWorkScope;
        private readonly IVesselInCompanyDomainService vesselDomainService;
        //    private readonly IApprovableDomainService _approvableDomainService;
        //    private readonly IGoodPartyAssignmentDomainService _goodPartyAssignmentDomainService;


        public OrderApplicationService(IOrderRepository orderRepository,
                                       IUnitOfWorkScope unitOfWorkScope,
            //  IGoodPartyAssignmentDomainService goodPartyAssignmentDomainService,
                                       //IFuelUserRepository userRepository,
                                       IVesselInCompanyDomainService vesselDomainService,
                                       IGoodDomainService goodDomainService,
                                       IOrderFactory iOrderFactory,
                                       ICompanyDomainService companyDomainService,
                                       IOrderItemDomainService orderItemDomainService,
                                       IEntityConfigurator<Order> orderConfigurator
            //,IApprovableDomainService approvableDomainService
            )
        {
            this.orderRepository = orderRepository;
            this.vesselDomainService = vesselDomainService;
            this.goodDomainService = goodDomainService;
            this.iOrderFactory = iOrderFactory;
            this.unitOfWorkScope = unitOfWorkScope;
            this.companyDomainService = companyDomainService;

            this.orderItemDomainService = orderItemDomainService;
            this.orderConfigurator = orderConfigurator;
            // _approvableDomainService = approvableDomainService;
            //  _goodPartyAssignmentDomainService = goodPartyAssignmentDomainService;
        }

        #region Operations

        public Order Add(string description, long ownerId, long? transporter, long? supplier, long? receiver, OrderTypes orderType, long? fromVesselInCompanyId, long? toVesselInCompanyId, long userId, DateTime orderDate)
        {
            GetCompanies(ownerId, transporter, supplier, receiver);
            var vessels = GetVessels(fromVesselInCompanyId, toVesselInCompanyId);

            var order = iOrderFactory.CreateFactoryOrderObject(description, ownerId, transporter, supplier, receiver,
                                                                orderType, 
                                                                //DateTime.Now,
                                                                orderDate,
                                                                vessels.SingleOrDefault(c => c.Id == fromVesselInCompanyId),
                                                                vessels.SingleOrDefault(c => c.Id == toVesselInCompanyId), userId);

            orderRepository.Add(order);

            unitOfWorkScope.Commit();

            return order;
        }


        public Order Update(long id, string description, OrderTypes orderType, long ownerId, long? transporter, long? supplier, long? receiver, long? fromVesselInCompanyId, long? toVesselInCompanyId, DateTime orderDate)
        {
            var order = orderRepository.FindByKey(id);
            if (order == null)
                throw new ObjectNotFound("Order", id);


            GetCompanies(ownerId, transporter, supplier, receiver);
            var vessels = GetVessels(fromVesselInCompanyId, toVesselInCompanyId);

            order.Update(description, orderType,
                         ownerId,
                         transporter,
                         supplier,
                         receiver,
                         vessels.SingleOrDefault(c => c.Id == fromVesselInCompanyId),
                         vessels.SingleOrDefault(c => c.Id == toVesselInCompanyId), orderDate, this.orderConfigurator, this.orderItemDomainService);

            orderRepository.Update(order);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("Order");
            }
            return order;
        }

        public void DeleteById(long id)
        {
            Order orderToDelete = orderRepository.FindByKey(id);
            if (orderToDelete == null)
                throw new ObjectNotFound("Order", id);

            orderToDelete.CheckDeleteRules();
            orderRepository.Delete(orderToDelete);
            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("Order");
            }
        }

        private List<VesselInCompany> GetVessels(long? fromVesselInCompanyId, long? toVesselInCompanyId)
        {
            var ids = new List<long>();
            if (fromVesselInCompanyId.HasValue)
                ids.Add(fromVesselInCompanyId.Value);

            if (toVesselInCompanyId.HasValue)
                ids.Add(toVesselInCompanyId.Value);

            var idList = ids.Distinct().ToList();

            var list = vesselDomainService.Get(idList);

            if (idList.Count != list.Count)
                throw new ObjectNotFound("Vessel");

            return list;
        }

        private List<Company> GetCompanies(long ownerId, long? transporter, long? supplier, long? receiver)
        {
            var ids = new List<long> { ownerId };

            if (transporter.HasValue)
                ids.Add(transporter.Value);

            if (supplier.HasValue)
                ids.Add(supplier.Value);

            if (receiver.HasValue)
                ids.Add(receiver.Value);

            var idList = ids.Distinct().ToList();

            var list = companyDomainService.Get(idList);

            if (idList.Count != list.Count)
                throw new ObjectNotFound("Company");

            return list;
        }

        #endregion

        #region OrderItem

        public void DeleteItem(long orderId, long orderItemId)
        {
            var order = orderRepository.FindByKey(orderId);
            if (order == null)
                throw new ObjectNotFound("order", orderId);

            var orderItem = order.OrderItems.SingleOrDefault(c => c.Id == orderItemId);
            if (orderItem == null)
                throw new ObjectNotFound("orderItem", orderItemId);
            order.DeleteItem(orderItem, orderItemDomainService);
            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("Order");
            }
            //            catch (Exception ex)
            //            {
            //                throw new UnHandleException(ex);
            //            }
        }


        public OrderItem AddItem(long orderId, string description, decimal quantity, long goodId, long unitId,
                                 long? assigneBuessinessPartyForGoodId)
        {
            var order = orderRepository.FindByKey(orderId);


            if (order == null)
                throw new ObjectNotFound("order", orderId);

            var goodDetails = GetGoodFromDomain(order.OwnerId, goodId);

            var orderItem = iOrderFactory.CreateFactoryOrderItemObject(order,
                                                                        description,
                                                                        quantity,
                                                                        goodId,
                                                                        unitId,
                                                                        goodDetails);

            order.AddItem(orderItem, goodDetails);
            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("Order");
            }
            //            catch (Exception ex)
            //            {
            //                throw new UnHandleException(ex);
            //            }
            var newOrderItem = orderRepository.SingleOrderItem(c => c.Id == orderItem.Id,
                                                                new SingleResultFetchStrategy<OrderItem>().Include(
                                                                    c => c.Good).Include(c => c.MeasuringUnit));

            return newOrderItem;
        }


        public OrderItem UpdateItem(long id, long orderId, string description, decimal quantity, long goodId, long unitId,
                                    long? assigneBuessinessPartyForGoodId)
        {
            var order = orderRepository.FindByKey(orderId);

            if (order == null)
                throw new ObjectNotFound("order", orderId);
            var goodDetails = GetGoodFromDomain(order.OwnerId, goodId);


            order.UpdateItem(id, description, quantity,
                             goodId,
                             unitId,
                             assigneBuessinessPartyForGoodId,
                             goodDetails
                );

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("Order");
            }
            return order.OrderItems.Single(c => c.Id == id);
        }

        private GoodFullInfo GetGoodFromDomain(long ownerId, long goodId)
        {
            var goodDetails = goodDomainService.GetGoodInfoes(ownerId, goodId);
            if (goodDetails == null)
                throw new ObjectNotFound("Good", goodId);
            return goodDetails;
        }


        #endregion

    }
}