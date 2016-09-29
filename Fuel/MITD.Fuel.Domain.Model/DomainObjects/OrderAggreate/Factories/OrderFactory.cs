#region

using System;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using System.Linq;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

#endregion

namespace MITD.Fuel.Domain.Model.Factories
{
    public class OrderFactory : IOrderFactory
    {
        private readonly IOrderCodeGenerator _iOrderCodeGenerator;
        private readonly IEntityConfigurator<Order> _orderConfigurator;
        private readonly IWorkflowStepRepository _workflowStepRepository;

        public OrderFactory(IOrderCodeGenerator iOrderCodeGenerator,
                            IEntityConfigurator<Order> orderConfigurator,
                            IWorkflowStepRepository _workflowStepRepository)
        {
            _iOrderCodeGenerator = iOrderCodeGenerator;
            _orderConfigurator = orderConfigurator;
            this._workflowStepRepository = _workflowStepRepository;
        }

        #region IOrderFactory Members

        public Order CreateFactoryOrderObject(string description, long ownerId, long? transporter, long? supplier, long? receiver, OrderTypes orderType, DateTime orderDate, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany, long userId)
        {
            var code = _iOrderCodeGenerator.GenerateNewCode();

            var order = new Order(
                code,
                description,
                ownerId,
                transporter,
                supplier,
                receiver,
                orderType,
                orderDate,
                fromVesselInCompany,
                toVesselInCompany,
                States.Open,
                _orderConfigurator
                );

            var initWorkflowStep = _workflowStepRepository.Single(
                c => c.Workflow.WorkflowEntity == WorkflowEntities.Order &&
                         c.CurrentWorkflowStage == WorkflowStages.Initial &&
                         c.Workflow.CompanyId == ownerId &&
                         c.Workflow.Name == Workflow.DEFAULT_NAME);
            var orderWorkflow = new OrderWorkflowLog(order.Id, WorkflowEntities.Order, DateTime.Now, WorkflowActions.Init,
                userId, "", initWorkflowStep.Id, true);

            order.ApproveWorkFlows.Add(orderWorkflow);

            return order;
        }

        public OrderItem CreateFactoryOrderItemObject(Order order, string description, decimal quantity, long goodId,
                                                      long unitId, GoodFullInfo goodFullDetails)
        {
            var orderItem = new OrderItem(description, quantity, goodId, unitId, goodFullDetails);

            if (order.OrderType == OrderTypes.SupplyForDeliveredVessel)
            {
                var goodUnitConvertorDomainService = ServiceLocator.Current.GetInstance<IGoodUnitConvertorDomainService>();

                var quantityInMainUnit = goodUnitConvertorDomainService.ConvertUnitValueToMainUnitValue(unitId, quantity);

                orderItem.AddSupplyForDeliveredVesselReceive(quantityInMainUnit, goodUnitConvertorDomainService);
            }

            return orderItem;
        }

        #endregion
    }
}