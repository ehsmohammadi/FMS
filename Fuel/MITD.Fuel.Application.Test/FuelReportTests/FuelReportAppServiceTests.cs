using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using MITD.Fuel.Application.Test.ScrapTests;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.DomainService;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.Adapter;
using MITD.Fuel.ACL.StorageSpace.DomainServices;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events;
using MITD.Fuel.ACL.StorageSpace.Mapper;
using MITD.Fuel.ACL.StorageSpace.ServiceWrappers;
using MITD.Fuel.Application.Facade;
using MITD.Fuel.Application.Facade.Mappers;
using MITD.Fuel.Application.Service;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.DomainServices.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Infrastructure.Service;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.AntiCorruption;
using MITD.Services.Connectivity;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.ReportObjects;
using MITD.Services.Facade;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Test.FuelReportTests
{
    [TestClass]
    public class FuelReportAppServiceTests
    {
        #region Test General Variables

        private FuelReportTestObjectsContainer testObjects;
        private TransactionScope transactionScope;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            this.testObjects = new FuelReportTestObjectsContainer();

            this.transactionScope = new TransactionScope();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.transactionScope.Dispose();
        }

        [TestMethod]
        public void UpdateVoyageId_InvalidFuelReport()
        {
            #region Arrange

            var fetchStategy = new ListFetchStrategy<FuelReport>();

            fetchStategy.OrderByDescending(en => en.Id);

            long invlaidFuelReportId = this.testObjects.FuelReportRepository.GetAll(fetchStategy).First().Id + 1;

            #endregion

            #region Action

            Exception thrownException = null;

            try
            {
                FuelReport result = this.testObjects.FuelReportApplicationService.UpdateVoyageId(invlaidFuelReportId, 5);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNotNull(thrownException);

            Assert.IsInstanceOfType(thrownException, typeof(ObjectNotFound));

            #endregion
        }

        [TestMethod]
        public void UpdateVoyageId_ValidFuelReport_InvalidVoyageId()
        {
            #region Arrange

            var fetchStategy = new ListFetchStrategy<FuelReport>();

            fetchStategy.OrderByDescending(en => en.Id);

            long validFuelReportId = this.testObjects.FuelReportRepository.GetAll(fetchStategy).First().Id;

            long invalidVoyageId = this.testObjects.VoyageDomainService.GetAll().OrderBy(v => v.Id).LastOrDefault().Id + 1;

            #endregion

            #region Action

            Exception thrownException = null;

            try
            {
                FuelReport result = this.testObjects.FuelReportApplicationService.UpdateVoyageId(validFuelReportId, invalidVoyageId);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNotNull(thrownException);

            Assert.IsInstanceOfType(thrownException, typeof(BusinessRuleException));

            Assert.AreEqual(((BusinessRuleException)thrownException).BusinessRuleCode, "BR_FR2");

            #endregion
        }

        [TestMethod]
        public void UpdateVoyageId_ValidFuelReport_ValidVoyageId()
        {
            #region Arrange

            var fetchStategy = new ListFetchStrategy<FuelReport>();

            fetchStategy.OrderByDescending(en => en.Id);

            long validFuelReportId = this.testObjects.FuelReportRepository.GetAll(fetchStategy).First().Id;

            long validVoyageId = this.testObjects.VoyageDomainService.GetAll().OrderBy(v => v.Id).LastOrDefault().Id;

            #endregion

            #region Action

            Exception thrownException = null;

            FuelReport result = null;

            try
            {
                result = this.testObjects.FuelReportApplicationService.UpdateVoyageId(validFuelReportId, validVoyageId);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNull(thrownException);

            Assert.IsNotNull(result);

            this.testObjects.RegenerateUnitOfWorkScope();

            var assertionRepository = new FuelReportRepository(this.testObjects.UnitOfWorkScope, testObjects.FuelReportConfigurator);

            FuelReport updatedFuelReport = assertionRepository.Find(fr => fr.Id == validFuelReportId).FirstOrDefault();

            Assert.IsNotNull(updatedFuelReport);

            Assert.IsNotNull(updatedFuelReport.Voyage);

            Assert.AreEqual(validVoyageId, updatedFuelReport.Voyage.Id);

            #endregion
        }

        [TestMethod]
        public void UpdateFuelReportDetail_InvlaidFuelReport()
        {
            #region Arrange

            var fetchStategy = new ListFetchStrategy<FuelReport>();

            fetchStategy.OrderByDescending(en => en.Id);

            long invalidFuelReportId = this.testObjects.FuelReportRepository.GetAll(fetchStategy).First().Id + 1;

            #endregion

            #region Action

            Exception thrownException = null;

            FuelReportDetail result = null;

            try
            {
                //result = testObjects.Target.UpdateFuelReportDetail(invalidFuelReportId, 1, 1, 1, 1, ReceiveTypes.InternalTransfer, 1, TransferTypes.InternalTransfer, 1, CorrectionTypes.Plus, 1, 1, null, null, null);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNotNull(thrownException);

            Assert.IsInstanceOfType(thrownException, typeof(ObjectNotFound));

            #endregion
        }

        [TestMethod]
        public void UpdateFuelReportDetail_InvalidFuelReportDetailId()
        {
            #region Arrange

            var fetchStategy = new ListFetchStrategy<FuelReport>();
            fetchStategy.Include(fr => fr.FuelReportDetails);

            fetchStategy.OrderByDescending(en => en.Id);

            long validFuelReportId = this.testObjects.FuelReportRepository.GetAll(fetchStategy).First().Id;

            FuelReport validFuelReport = this.testObjects.FuelReportRepository.Find(c => c.Id == validFuelReportId, fetchStategy).FirstOrDefault();

            FuelReportDetail lastFuelReportDetail = validFuelReport.FuelReportDetails.OrderByDescending(frd => frd.Id).FirstOrDefault();

            long invalidFuelReportDetailId = 0L;

            if (lastFuelReportDetail != null)
            {
                invalidFuelReportDetailId = lastFuelReportDetail.Id + 1;
            }

            #endregion

            #region Action

            Exception thrownException = null;

            FuelReportDetail result = null;

            try
            {
                result = this.testObjects.FuelReportApplicationService.UpdateFuelReportDetail(validFuelReportId, invalidFuelReportDetailId, 1, 1, 1, ReceiveTypes.InternalTransfer, 1, TransferTypes.InternalTransfer, 1, CorrectionTypes.Plus, CorrectionPricingTypes.Default, 1, 1, null, null, null,null);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNotNull(thrownException);

            Assert.IsInstanceOfType(thrownException, typeof(ObjectNotFound));

            Assert.AreEqual("FuelReportDetail", ((ObjectNotFound)thrownException).EntityName);

            #endregion
        }

        [TestMethod]
        public void UpdateFuelReportDetail_ValidFuelReportAndDetail_1()
        {
            #region Arrange

            FuelReport validFuelReport = this.testObjects.FuelReportRepository.First(fr => fr.FuelReportDetails.Count() > 0);

            #endregion

            #region Action

            Exception thrownException = null;

            if (validFuelReport == null)
            {
                return;
            }

            try
            {
                this.testObjects.FuelReportApplicationService.UpdateFuelReportDetail(validFuelReport.Id, validFuelReport.FuelReportDetails.First().Id, 234, 30, 50, ReceiveTypes.InternalTransfer, null, null, null, null, CorrectionPricingTypes.Default, null, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNull(thrownException);

            #endregion
        }

        [TestMethod]
        public void UpdateFuelReportDetail_ValidFuelReportAndDetail_2()
        {
            #region Arrange

            FuelReport validFuelReport = this.testObjects.FuelReportRepository.First(fr => fr.FuelReportDetails.Count() > 0);

            #endregion

            #region Action

            Exception thrownException = null;

            if (validFuelReport == null)
            {
                return;
            }

            try
            {
                this.testObjects.FuelReportApplicationService.UpdateFuelReportDetail(validFuelReport.Id, validFuelReport.FuelReportDetails.First().Id, 234, 30, null, null, 35, TransferTypes.InternalTransfer, null, null, CorrectionPricingTypes.Default, null, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNull(thrownException);

            #endregion
        }

        [TestMethod]
        public void MiddleApproveFuelReport()
        {
            #region Arrange

            //TODO: A.H Review
            //var initialFuelReprot = testObjects.FuelReportRepository.First(
            //    fr =>
            //        fr.CurrentApproveFlow.ApproveFlowState == ApproveFlowStates.Initial);

            #endregion

            #region Action

            Exception thrownException = null;

            try
            {
                //TODO: A.H Review
                //testObjects.Target.UpdateApproveFlowState(initialFuelReprot.CurrentApproveFlow.NextApproveFlow, ActionTypes.Approve, initialFuelReprot.Id);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNull(thrownException);

            #endregion
        }

        [TestMethod]
        public void FinalApproveFuelReport()
        {
            #region Arrange

            #endregion

            #region Action

            Exception thrownException = null;

            try
            {
                this.testObjects.FuelReportApplicationService.IsSetFuelReportInventoryResultPossible(1);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            #endregion

            #region Assert

            Assert.IsNull(thrownException);

            #endregion
        }

    }

    public class FuelReportTestObjectsContainer
    {
        public FuelReportTestObjectsContainer()
            : this(new UnitOfWorkScope(new EFUnitOfWorkFactory(() => new DataContainer())))
        {

        }

        public FuelReportTestObjectsContainer(UnitOfWorkScope unitOfWorkScope)
        {
            this.UnitOfWorkScope = unitOfWorkScope;

            var basicInfoDomainServiceObjects = new BasicInfoDomainServiceObjectsContainer(unitOfWorkScope);
            var scrapTestObjects = new ScrapTestObjectsContainer(this.UnitOfWorkScope);

            this.FuelReportConfigurator = new FuelReportConfigurator(new FuelReportStateFactory(), null, null);

            this.FuelReportRepository = new FuelReportRepository(this.UnitOfWorkScope, this.FuelReportConfigurator);

            var currencyRepository = new EFRepository<Currency>(this.UnitOfWorkScope);

            var currencyDomainService = new CurrencyDomainService(currencyRepository, new EFRepository<CurrencyExchange>(UnitOfWorkScope), new EFRepository<Inventory_UnitConvert>(UnitOfWorkScope));

            this.VoyageDomainService = new VoyageDomainService(new VoyageRepository(this.UnitOfWorkScope), FuelReportRepository, new EFRepository<RotationVoyage>(this.UnitOfWorkScope));

            this.VoyageLogDomainService = new VoyageLogDomainService(new VoyageLogRepository(this.UnitOfWorkScope));





            var inventoryOperationFactory = new InventoryOperationFactory();

            var inventoryOperationRepository = new InventoryOperationRepository(this.UnitOfWorkScope);
            var inventoryOperationDomainService = new InventoryOperationDomainService(inventoryOperationRepository, scrapTestObjects.ScrapDomainService);

            var orderConfigurator = new OrderConfigurator(
                new OrderStateFactory(
                    inventoryOperationDomainService
                    ));

            this.OrderDomainService = new OrderDomainService(new OrderRepository(this.UnitOfWorkScope, orderConfigurator, new EFRepository<OrderItem>(this.UnitOfWorkScope)));

            var client = new WebClientHelper(new HttpClient());
            var hostAdapter = new ExternalHostAddressHelper();

            var goodDomainService = new GoodDomainService(
                           new GoodAntiCorruptionAdapter(
                               new GoodAntiCorruptionServiceWrapper(client, hostAdapter),
                               new GoodAntiCorruptionMapper()),
                           new EFRepository<Good>(this.UnitOfWorkScope),
                           basicInfoDomainServiceObjects.CompanyDomainService, new EFRepository<GoodUnit>(this.UnitOfWorkScope));
            var inventoryManagementDomainService = new InventoryManagementDomainService(goodDomainService, currencyDomainService, null);

            this.FuelReportDomainService = new FuelReportDomainService(
                this.FuelReportRepository,
                this.VoyageDomainService,
                inventoryOperationDomainService,
                inventoryOperationRepository,
                inventoryOperationFactory,
                OrderDomainService,
                inventoryManagementDomainService,
                new CharteringDomainService(new CharterInRepository(this.UnitOfWorkScope), new CharterOutRepository(this.UnitOfWorkScope)),
                new EFRepository<FuelReportDetail>(this.UnitOfWorkScope),
                new VoyageRepository(this.UnitOfWorkScope),
                new GoodUnitConvertorDomainService(goodDomainService,
                    new GoodUnitDomainService(new BaseAntiCorruptionAdapter<GoodUnit, GoodUnitDto>(
                        new BaseAntiCorruptionServiceWrapper<GoodUnitDto>(client),
                        new BaseAntiCorruptionMapper<GoodUnit, GoodUnitDto>()),
                        new EFRepository<GoodUnit>(this.UnitOfWorkScope))),
                        new EFRepository<VesselEventReportsView>(this.UnitOfWorkScope),
                        WorkflowLogRepository);





            this.WorkflowLogRepository = new WorkflowLogRepository(this.UnitOfWorkScope);

            //var inventoryOperationNotifier = new InventoryOperationNotifier();

            this.FuelReportApplicationService = null;
            //this.FuelReportApplicationService = new FuelReportApplicationService(
            //    this.UnitOfWorkScope,
            //    currencyDomainService,
            //    this.VoyageDomainService,
            //    FuelReportDomainService,
            //    new ApproveFlowApplicationService(this.UnitOfWorkScope, new WorkflowLogRepository(this.UnitOfWorkScope), new WorkflowRepository(this.UnitOfWorkScope), new OrderConfigurator(new OrderStateFactory(new InventoryOperationDomainService(new InventoryOperationRepository(this.UnitOfWorkScope), new ScrapDomainService(new ScrapRepository(this.UnitOfWorkScope, new ScrapConfigurator(new ScrapStateFactory(new ApprovableScrapDomainService(new VesselDomainService(new BaseAntiCorruptionAdapter<Vessel, WarehouseDto>(), ), )))), )), )), ));


            var goodMapper = new GoodToGoodDtoMapper(new CompanyGoodUnitToGoodUnitDtoMapper());
            var currencyMapper = new CurrencyToCurrencyDtoMapper();

            var companyDtoMapper = new BaseFacadeMapper<Company, CompanyDto>();
            var vesselInCompanyDtoMapper = new VesselInCompanyToVesselInCompanyDtoMapper(companyDtoMapper);

            var fuelReportDetailMapper = new FuelReportDetailToFuelReportDetailDtoMapper(goodMapper, currencyMapper, new InventoryOperationToInventoryOperationDtoMapper());

            var fuelReportToFuelReportDtoMapper = new FuelReportToFuelReportDtoMapper(
                fuelReportDetailMapper, new InventoryOperationToInventoryOperationDtoMapper());

            this.VoyageFacadeService = new VoyageFacadeService(
                VoyageDomainService,
                new VoyageToVoyageDtoMapper(),
                FuelReportDomainService,
                fuelReportToFuelReportDtoMapper,
                inventoryOperationDomainService,
                new InventoryOperationToInventoryOperationDtoMapper(),
                this.VoyageLogDomainService,
                new VoyageLogToVoyageLogDtoMapper(companyDtoMapper, vesselInCompanyDtoMapper),
                goodMapper, inventoryManagementDomainService, new VoyageApplicationService(new VoyageRepository(this.UnitOfWorkScope)));
        }

        public FuelReportDomainService FuelReportDomainService { get; set; }

        public VoyageFacadeService VoyageFacadeService { get; private set; }

        public FuelReportApplicationService FuelReportApplicationService { get; private set; }

        public FuelReportRepository FuelReportRepository { get; private set; }

        public WorkflowLogRepository WorkflowLogRepository { get; private set; }

        public VoyageDomainService VoyageDomainService { get; private set; }

        public VoyageLogDomainService VoyageLogDomainService { get; private set; }

        public UnitOfWorkScope UnitOfWorkScope { get; private set; }

        public OrderDomainService OrderDomainService { get; private set; }

        public FuelReportConfigurator FuelReportConfigurator { get; private set; }

        public void RegenerateUnitOfWorkScope()
        {
            if (this.UnitOfWorkScope != null)
            {
                this.UnitOfWorkScope.CurrentUnitOfWork.Dispose();
            }
        }
    }
}
