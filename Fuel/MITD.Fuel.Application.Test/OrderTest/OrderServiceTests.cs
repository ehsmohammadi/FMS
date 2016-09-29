#region

using System;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.Adapter;
using MITD.Fuel.ACL.StorageSpace.DomainServices;
using MITD.Fuel.ACL.StorageSpace.Mapper;
using MITD.Fuel.ACL.StorageSpace.ServiceWrappers;
using MITD.Fuel.Application.Service;
using MITD.Fuel.Application.Test.ScrapTests;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.DomainService;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.FakeDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Infrastructure.Service;
using MITD.Fuel.Integration.Inventory;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.AntiCorruption;
using MITD.Services.Connectivity;
using MITD.StorageSpace.Presentation.Contracts.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MITD.Fuel.Application.Test.FuelReportTests;

#endregion

namespace MITD.Fuel.Application.Test
{
    [TestClass]
    public class OrderServiceTests
    {
        #region Initial

        private Good _good;
        private Good _good2;
        //private GoodPartyAssignment _goodPartAssignment;
        //private GoodPartyAssignment _goodPartAssignment2;
        private Company _hafiz;
        private VesselInCompany hafizVesselInCompany;
        private Company _imsemCo;
        private OrderRepository _orderRepository;
        private Company _sapid;
        private VesselInCompany sapidVesselInCompany;
        private VesselInCompany _sapidVessel2;
        private UnitOfWorkScope _scope;
        private OrderApplicationService _target;
        private TransactionScope _tr;
        private GoodUnit _unit;
        private GoodUnit _unit2;
        private GoodUnit _unit3;

        private BasicInfoDomainServiceObjectsContainer basicInfoDomainServiceObjects;
        private ScrapTestObjectsContainer scrapTestObjects;

        [TestInitialize]
        public void TestInitialize()
        {
            _scope = new UnitOfWorkScope(new EFUnitOfWorkFactory(() => new DataContainer()));

            basicInfoDomainServiceObjects = new BasicInfoDomainServiceObjectsContainer(_scope);
            scrapTestObjects = new ScrapTestObjectsContainer(_scope);

            var orderConfigurator = new OrderConfigurator
                (
                new OrderStateFactory
                    (
                    new InventoryOperationDomainService(new InventoryOperationRepository(_scope),
                        scrapTestObjects.ScrapDomainService)));


            var client = new WebClientHelper(new HttpClient());
            _orderRepository = new OrderRepository(_scope, orderConfigurator, new EFRepository<OrderItem>(_scope));
            _orderRepository.GetAll();
            _tr = new TransactionScope();

            var hostAdapter = new ExternalHostAddressHelper();
            var vesselService = new VesselInCompanyDomainService
                (new VesselInCompanyRepository(_scope, null), new VoyageRepository(_scope), new VesselRepository(this._scope), new InventoryOperationManager(), new CompanyRepository(this._scope));
            //var goodPartyService = new GoodPartyAssignmentDomainService
            //    (new GoodPartyAssignmentAntiCorruptionAdapter(new GoodAssignmentAntiCorruptionServiceWrapper(client, hostAdapter)));
            var goodService = new GoodDomainService
                (
                new GoodAntiCorruptionAdapter(new GoodAntiCorruptionServiceWrapper(client, hostAdapter), new GoodAntiCorruptionMapper()),
                new EFRepository<Good>(_scope), basicInfoDomainServiceObjects.CompanyDomainService, new EFRepository<GoodUnit>(_scope));

            var goodUnitService = new GoodUnitDomainService
                (
                new BaseAntiCorruptionAdapter<GoodUnit, GoodUnitDto>
                    (new BaseAntiCorruptionServiceWrapper<GoodUnitDto>(client), new BaseAntiCorruptionMapper<GoodUnit, GoodUnitDto>()),
                new EFRepository<GoodUnit>(_scope));

            var companyService = new CompanyDomainService
                (
                new CompanyAntiCorruptionAdapter(new CompanyAntiCorruptionServiceWrapper(client, hostAdapter), new CompanyAntiCorruptionMapper()),
                new CompanyRepository(_scope));

            //_target = new OrderApplicationService
            //    (
            //    _orderRepository, _scope, new UserRepository(_scope), vesselService, goodService,
            //    new OrderFactory(new OrderCodeGenerator(_orderRepository), orderConfigurator, new WorkflowRepository(_scope)), companyService,
            //    new OrderItemDomainService(new EFRepository<OrderItem>(_scope)), orderConfigurator);


            //_sapid = FakeDomainService.GetCompanies().Single(c => c.Id == 1);
            //_imsemCo = FakeDomainService.GetCompanies().Single(c => c.Id == 3);
            //_hafiz = FakeDomainService.GetCompanies().Single(c => c.Id == 2);

            this.sapidVesselInCompany = FakeDomainService.GetVesselsInCompanies().First(c => c.CompanyId == 1);
            _sapidVessel2 = FakeDomainService.GetVesselsInCompanies().Last(c => c.CompanyId == 1);
            this.hafizVesselInCompany = FakeDomainService.GetVesselsInCompanies().First(c => c.CompanyId == 2);
            _good = FakeDomainService.GetGoods().Single(c => c.Id == 1);
            _good2 = FakeDomainService.GetGoods().Single(c => c.Id == 2);
            _unit = FakeDomainService.GetCompanyGoodUnits().Single(c => c.Id == 1);
            _unit2 = FakeDomainService.GetCompanyGoodUnits().Single(c => c.Id == 2);
            _unit3 = FakeDomainService.GetCompanyGoodUnits().Single(c => c.Id == 4);

            //_goodPartAssignment = FakeDomainService.GetGoodPartyAssignments().Single(c => c.Id == 1);
            //_goodPartAssignment2 = FakeDomainService.GetGoodPartyAssignments().Single(c => c.Id == 2);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _tr.Dispose();
        }

        private void RegenerateUow()
        {
            _scope.CurrentUnitOfWork.Dispose();
            _scope = new UnitOfWorkScope(new EFUnitOfWorkFactory(() => new DataContainer()));
            OrderConfigurator orderConfigurator = null;
            //   new OrderConfigurator(new OrderStateFactory(new FuelReportDomainService(new FuelReportRepository(_scope),
            //new VoyageDomainService(new VoyageRepository(_scope)), new InventoryOperationDomainService(new InventoryOperationRepository(_scope)),
            //new InventoryOperationRepository(_scope), new InventoryOperationFactory()), new InvoiceDomainService()));


            _orderRepository = new OrderRepository(_scope, orderConfigurator, new EFRepository<OrderItem>(_scope));
        }

        #endregion

        #region Order Add  Senario

        [TestMethod]
        public void AddPurchaseOrderTest()
        {
            #region Arrange

            #endregion

            #region Action

            var guid = Guid.NewGuid().ToString();
            _target.Add(guid, this._sapid.Id, null, this._imsemCo.Id, null, OrderTypes.Purchase, null, null, 201, DateTime.Now);
            _scope.Commit();

            #endregion

            #region Assert

            RegenerateUow();
            var order = _orderRepository.Single(c => c.Description == guid);
            Assert.IsNotNull(order);

            // Assert.IsTrue(order.CurrentApproveWorkFlowConfig.ApproveFlowState == ApproveFlowStates.Initial);

            #endregion
        }

        [TestMethod]
        public void AddPurchaseWithTransferOrderTest()
        {
            #region Arrange

            #endregion

            #region Action

            var guid = Guid.NewGuid().ToString();
            _target.Add(guid, this._sapid.Id, this._imsemCo.Id, this._hafiz.Id, null, OrderTypes.PurchaseWithTransferOperations, null, null,201, DateTime.Now);
            _scope.Commit();

            #endregion

            #region Assert

            RegenerateUow();
            Assert.IsNotNull(_orderRepository.Single(c => c.Description == guid));

            #endregion
        }

        [TestMethod]
        public void AddTransferOrderTest()
        {
            #region Arrange

            #endregion

            #region Action

            var guid = Guid.NewGuid().ToString();
            _target.Add(guid, this._hafiz.Id, this._imsemCo.Id, null, this._sapid.Id, OrderTypes.Transfer, this.hafizVesselInCompany.Id, this.sapidVesselInCompany.Id, 201, DateTime.Now);
            _scope.Commit();

            #endregion

            #region Assert

            RegenerateUow();

            Assert.IsNotNull(_orderRepository.Single(c => c.Description == guid));

            #endregion
        }


        [TestMethod]
        public void AddInternalTransferOrderTest()
        {
            #region Arrange

            #endregion

            #region Action

            var guid = Guid.NewGuid().ToString();
            _target.Add(guid, this._sapid.Id, null, null, null, OrderTypes.InternalTransfer, this.sapidVesselInCompany.Id, this._sapidVessel2.Id, 201, DateTime.Now);
            _scope.Commit();

            #endregion

            #region Assert

            RegenerateUow();

            Assert.IsNotNull(_orderRepository.Single(c => c.Description == guid));

            #endregion
        }

        #endregion

        #region Update Order Senario

        [TestMethod]
        public void UpdatePurchaseOrderTest()
        {
            #region Arrange

            var guid = Guid.NewGuid().ToString();
            var order = _orderRepository.First(c => c.OrderType == OrderTypes.Purchase);

            #endregion

            #region Action

            _target.Update(order.Id, guid, order.OrderType, this._sapid.Id, null, this._hafiz.Id, null, null, null, DateTime.Now);
            _scope.Commit();

            #endregion

            #region Assert

            RegenerateUow();

            var updated = _orderRepository.Single(c => c.Description == guid);
            Assert.IsTrue(updated.SupplierId == _hafiz.Id);

            #endregion
        }

        [TestMethod]
        public void UpdatePurchaseOrderToTransportTest()
        {
            #region Arrange

            var guid = Guid.NewGuid().ToString();
            var order = _orderRepository.First(c => c.OrderType == OrderTypes.Transfer);

            #endregion

            #region Action

            _target.Update(order.Id, guid, order.OrderType, this._sapid.Id, this._imsemCo.Id, null, this._hafiz.Id, this._sapidVessel2.Id, this.hafizVesselInCompany.Id, DateTime.Now);
            _scope.Commit();

            #endregion

            #region Assert

            RegenerateUow();

            var updated = _orderRepository.Single(c => c.Description == guid);
            Assert.IsNotNull(updated);
            Assert.IsTrue(updated.ReceiverId == _hafiz.Id);

            #endregion
        }

        #endregion

        #region OrderItem  Manipulate Senario

        [TestMethod]
        public void AddOrderItemTest()
        {
            #region Arrange

            #endregion

            #region Action

            var guid = Guid.NewGuid().ToString();
            var order = _orderRepository.First(c => c.Id == 1);

            _target.AddItem(order.Id, guid, 5, _good2.Id, _unit3.Id, 0);

            #endregion

            #region Assert

            RegenerateUow();

            Assert.IsNotNull(_orderRepository.Single(c => c.Id == order.Id).OrderItems.Single(c => c.Description == guid));

            #endregion
        }

        [TestMethod]
        public void UpdateOrderItemTest()
        {
            #region Arrange

            #endregion

            #region Action

            var guid = Guid.NewGuid().ToString();
            var order = _orderRepository.First(c => c.Id == 1);
            var orderItem = order.OrderItems.First();
            _target.UpdateItem(orderItem.Id, order.Id, guid, 4, _good.Id, _unit2.Id, 0);

            #endregion

            #region Assert

            RegenerateUow();

            var updatedOrderItem = _orderRepository.Single(c => c.Id == order.Id).OrderItems.Single(c => c.Description == guid);

            Assert.AreEqual(updatedOrderItem.GoodId, _good.Id);
            Assert.AreEqual(updatedOrderItem.Quantity, 4);
            Assert.AreEqual(updatedOrderItem.MeasuringUnitId, _unit2.Id);
            Assert.AreNotEqual(updatedOrderItem.TimeStamp, orderItem.TimeStamp);

            #endregion
        }


        [TestMethod]
        public void DeleteOrderItemTest()
        {
            #region Arrange

            #endregion

            #region Action

            var guid = Guid.NewGuid().ToString();
            var order = _orderRepository.First(c => c.Id == 2);
            var orderItem = order.OrderItems.First();
            _target.DeleteItem(order.Id, orderItem.Id);

            #endregion

            #region Assert

            RegenerateUow();

            var deleted = _orderRepository.Single(c => c.Id == order.Id).OrderItems.SingleOrDefault(c => c.Description == guid);

            Assert.IsNull(deleted);

            #endregion
        }

        #endregion

        #region Approve Senarion

        [TestMethod]
        public void ApproveOrderTest()
        {
            //            #region Arrange
            //            var guid = Guid.NewGuid().ToString();
            //            var order = _orderRepository.First(c => c.Id == 1);
            //
            //            var nextApprove = order.CurrentApproveWorkFlowConfig.NextApproveWorkFlowConfig;
            //
            //            #endregion
            //
            //            #region Action
            //
            //
            //            _target.ValidateUpdateApproveFlowStage(nextApprove, ActionTypes.Approve, order.Id);
            //
            //            #endregion
            //
            //            #region Assert
            //            RegenerateUow();
            //
            //            var updatedOrderItem =
            //                _orderRepository.Single(c => c.Id == order.Id);
            //
            //            Assert.AreEqual(updatedOrderItem.CurrentApproveFlowId, nextApprove.Id);
            //            Assert.AreEqual(updatedOrderItem.CurrentApproveWorkFlowConfig.ApproveFlowState, nextApprove.ApproveFlowState);
            //
            //          #endregion
        }

        #endregion
    }
}