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
using MITD.Fuel.Application.Facade;
using MITD.Fuel.Application.Facade.Mappers;
using MITD.Fuel.Application.Service;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.FakeDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Infrastructure.Service;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Services.AntiCorruption;
using MITD.Services.Connectivity;
using MITD.Services.Facade;
using MITD.StorageSpace.Presentation.Contracts.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace MITD.Fuel.Application.Test
{
    [TestClass]
    public class GoodFacadeServiceTests
    {


        private OrderRepository _orderRepository;
        private UnitOfWorkScope _scope;

        private TransactionScope _tr;
        private IGoodFacadeService _target;

        private BasicInfoDomainServiceObjectsContainer basicInfoDomainServiceObjects;

        [TestInitialize]
        public void TestInitialize()
        {
            _scope = new UnitOfWorkScope(new EFUnitOfWorkFactory(() => new DataContainer()));

            basicInfoDomainServiceObjects = new BasicInfoDomainServiceObjectsContainer(_scope);

            OrderConfigurator orderConfigurator = null;
            //new OrderConfigurator(new OrderStateFactory(new FuelReportDomainService(new FuelReportRepository(_scope),
            //    new VoyageDomainService( new VoyageRepository(_scope)),new InventoryOperationDomainService(new InventoryOperationRepository(_scope)),
            //    new InventoryOperationRepository(_scope),new InventoryOperationFactory()   ),new InvoiceDomainService() ));

            var client = new WebClientHelper(new HttpClient());
            _orderRepository = new OrderRepository(_scope, orderConfigurator, new EFRepository<OrderItem>(_scope));
            _orderRepository.GetAll();
            _tr = new TransactionScope();

            var hostAdapter = new ExternalHostAddressHelper();

            _target = new GoodFacadeService(
                new GoodDomainService(
                    new GoodAntiCorruptionAdapter(
                        new GoodAntiCorruptionServiceWrapper
                    (client, hostAdapter),
                    new GoodAntiCorruptionMapper()),
                    new EFRepository<Good>(_scope),
                    basicInfoDomainServiceObjects.CompanyDomainService, new EFRepository<GoodUnit>(_scope)),
                    new GoodToGoodDtoMapper(new CompanyGoodUnitToGoodUnitDtoMapper()));

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _tr.Dispose();
        }
        [TestMethod]
        public void TestGetAll()
        {
            var g = _target.GetAll(1);
            Assert.AreEqual(g.Count, 2);
        }

    }
}