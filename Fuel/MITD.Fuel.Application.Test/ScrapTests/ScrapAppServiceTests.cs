using System;
using System.Linq;
using System.Runtime;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Application.Test.ScrapTests
{
    [TestClass]
    public class ScrapAppServiceTests
    {

        private ScrapTestObjectsContainer testObjects;
        private TransactionScope transactionScope;

        [TestInitialize]
        public void TestInitialize()
        {
            testObjects = new ScrapTestObjectsContainer();
            transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            transactionScope.Dispose();
        }

        [TestMethod]
        public void AddScrap()
        {
            #region Arrange

            var vessel = testObjects.BasicInfoDomainServiceObjects.VesselDomainService.GetAll().FirstOrDefault();

            var secondParty = testObjects.BasicInfoDomainServiceObjects.CompanyDomainService.GetAll().FirstOrDefault();

            #endregion

            #region Action

            Exception exception = null;

            Scrap addedScrap = null;

            try
            {
                addedScrap = this.testObjects.ScrapApplicationService.ScrapVessel(vessel.Id, secondParty.Id, DateTime.Now, 201);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            #endregion

            #region Assert

            Assert.IsNull(exception);

            Assert.IsNotNull(addedScrap);

            testObjects.RegenerateUnitOfWorkScope();

            var assertionRepository = testObjects.GenerateScrapRepository();

            var registered = assertionRepository.Find(s => s.Id == addedScrap.Id);

            Assert.IsNotNull(registered);

            #endregion
        }
    }

    public class ScrapTestObjectsContainer
    {
        public ScrapRepository ScrapRepository { get; private set; }

        public BasicInfoDomainServiceObjectsContainer BasicInfoDomainServiceObjects { get; private set; }

        public UnitOfWorkScope UnitOfWorkScope { get; private set; }

        public ScrapConfigurator ScrapConfigurator { get; private set; }

        public ScrapTestObjectsContainer()
            : this(new UnitOfWorkScope(new EFUnitOfWorkFactory(() => new DataContainer())))
        {

        }

        public ScrapTestObjectsContainer(UnitOfWorkScope unitOfWorkScope)
        {
            this.UnitOfWorkScope = unitOfWorkScope;

            this.BasicInfoDomainServiceObjects = new BasicInfoDomainServiceObjectsContainer(this.UnitOfWorkScope);


            var approvableScrapDomainService =
                new ApprovableScrapDomainService(
                    this.BasicInfoDomainServiceObjects.VesselDomainService,
                    this.BasicInfoDomainServiceObjects.InventoryOperationNotifier,
                    this.BasicInfoDomainServiceObjects.TankDomainService,
                    this.BasicInfoDomainServiceObjects.CurrencyDomainService,
                    this.BasicInfoDomainServiceObjects.GoodDomainService,
                    this.BasicInfoDomainServiceObjects.GoodUnitDomainService);

            this.ScrapConfigurator = new ScrapConfigurator(new ScrapStateFactory(approvableScrapDomainService), new VesselInInventoryRepository(this.UnitOfWorkScope), new VesselInCompanyStateFactory());

            this.ScrapRepository = GenerateScrapRepository();



            this.ScrapDomainService = new ScrapDomainService(this.ScrapRepository, new EFRepository<ScrapDetail>(this.UnitOfWorkScope));

            var scrapFactory = new ScrapFactory(
                this.ScrapConfigurator,
                this.BasicInfoDomainServiceObjects.WorkflowStepRepository,
                this.ScrapDomainService,
                this.BasicInfoDomainServiceObjects.VesselDomainService,
                this.BasicInfoDomainServiceObjects.CompanyDomainService,
                this.BasicInfoDomainServiceObjects.TankDomainService,
                this.BasicInfoDomainServiceObjects.CurrencyDomainService,
                this.BasicInfoDomainServiceObjects.GoodDomainService,
                this.BasicInfoDomainServiceObjects.GoodUnitDomainService);

            this.WorkflowLogRepository = new WorkflowLogRepository(this.UnitOfWorkScope);

            this.ScrapApplicationService = new ScrapApplicationService(
                scrapFactory,
                this.ScrapRepository,
                this.UnitOfWorkScope,
                this.ScrapDomainService,
                this.BasicInfoDomainServiceObjects.VesselDomainService,
                this.BasicInfoDomainServiceObjects.CompanyDomainService,
                this.BasicInfoDomainServiceObjects.CurrencyDomainService,
                this.BasicInfoDomainServiceObjects.GoodDomainService,
                this.BasicInfoDomainServiceObjects.GoodUnitDomainService,
                this.BasicInfoDomainServiceObjects.TankDomainService);
        }

        public ScrapApplicationService ScrapApplicationService { get; private set; }

        public ScrapDomainService ScrapDomainService { get; private set; }

        public WorkflowLogRepository WorkflowLogRepository { get; private set; }

        public void RegenerateUnitOfWorkScope()
        {
            if (this.UnitOfWorkScope != null)
            {
                this.UnitOfWorkScope.CurrentUnitOfWork.Dispose();
            }
        }

        internal ScrapRepository GenerateScrapRepository()
        {
            return new ScrapRepository(this.UnitOfWorkScope, this.ScrapConfigurator);
        }
    }
}
