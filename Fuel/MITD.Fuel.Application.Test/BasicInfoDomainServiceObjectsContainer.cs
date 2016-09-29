using System.Net.Http;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.Adapter;
using MITD.Fuel.ACL.StorageSpace.DomainServices;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events;
using MITD.Fuel.ACL.StorageSpace.Mapper;
using MITD.Fuel.ACL.StorageSpace.ServiceWrappers;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Infrastructure.Service;
using MITD.Fuel.Integration.Inventory;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.AntiCorruption;
using MITD.Services.Connectivity;
using MITD.StorageSpace.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Test
{
    public class BasicInfoDomainServiceObjectsContainer
    {
        public BasicInfoDomainServiceObjectsContainer(UnitOfWorkScope unitOfWorkScope)
        {
            this.UnitOfWorkScope = unitOfWorkScope;

            this.CurrencyRepository = new EFRepository<Currency>(this.UnitOfWorkScope);

            this.CurrencyDomainService = new CurrencyDomainService(this.CurrencyRepository, new EFRepository<CurrencyExchange>(this.UnitOfWorkScope), new EFRepository<Inventory_UnitConvert>(this.UnitOfWorkScope));

            this.VoyageDomainService = new VoyageDomainService(new VoyageRepository(this.UnitOfWorkScope), new FuelReportRepository(this.UnitOfWorkScope, new FuelReportConfigurator(new FuelReportStateFactory(),null,null)), new EFRepository<RotationVoyage>(this.UnitOfWorkScope));

            this.VoyageLogDomainService = new VoyageLogDomainService(new VoyageLogRepository(this.UnitOfWorkScope));

            this.ClientHelper = new WebClientHelper(new HttpClient());
            this.HostAddressHelper = new ExternalHostAddressHelper();


            this.WorkflowLogRepository = new WorkflowLogRepository(this.UnitOfWorkScope);

            //this.InventoryOperationNotifier = new InventoryOperationNotifier();
            this.InventoryOperationNotifier = null;

            this.VesselDomainService = new VesselInCompanyDomainService(
                new VesselInCompanyRepository(this.UnitOfWorkScope, null), new VoyageRepository(this.UnitOfWorkScope),
                new VesselRepository(this.UnitOfWorkScope), new InventoryOperationManager(), new CompanyRepository(this.UnitOfWorkScope));

            this.CompanyDomainService = new CompanyDomainService(
                new CompanyAntiCorruptionAdapter(
                    new CompanyAntiCorruptionServiceWrapper(this.ClientHelper, this.HostAddressHelper),
                    new CompanyAntiCorruptionMapper()),
                new CompanyRepository(this.UnitOfWorkScope));

            this.GoodDomainService = new GoodDomainService(
               new GoodAntiCorruptionAdapter(
                   new GoodAntiCorruptionServiceWrapper(this.ClientHelper, this.HostAddressHelper),
                   new GoodAntiCorruptionMapper()),
               new EFRepository<Good>(this.UnitOfWorkScope),
               this.CompanyDomainService, new EFRepository<GoodUnit>(this.UnitOfWorkScope));

            this.TankDomainService = new TankDomainService(new EFRepository<Tank>(this.UnitOfWorkScope));

            this.GoodUnitDomainService = new GoodUnitDomainService(
                new BaseAntiCorruptionAdapter<GoodUnit, GoodUnitDto>(
                    new BaseAntiCorruptionServiceWrapper<GoodUnitDto>(this.ClientHelper),
                    new BaseAntiCorruptionMapper<GoodUnit, GoodUnitDto>()),
                new EFRepository<GoodUnit>(this.UnitOfWorkScope));

            this.WorkflowStepRepository = new WorkflowStepRepository(this.UnitOfWorkScope);
        }

        public UnitOfWorkScope UnitOfWorkScope { get; private set; }

        public InventoryOperationNotifier InventoryOperationNotifier { get; private set; }

        public GoodDomainService GoodDomainService { get; private set; }

        public ExternalHostAddressHelper HostAddressHelper { get; private set; }

        public WebClientHelper ClientHelper { get; private set; }

        public CurrencyDomainService CurrencyDomainService { get; private set; }

        public EFRepository<Currency> CurrencyRepository { get; private set; }

        public WorkflowLogRepository WorkflowLogRepository { get; private set; }

        public VoyageDomainService VoyageDomainService { get; private set; }

        public VoyageLogDomainService VoyageLogDomainService { get; private set; }

        public VesselInCompanyDomainService VesselDomainService { get; private set; }

        public CompanyDomainService CompanyDomainService { get; private set; }

        public TankDomainService TankDomainService { get; private set; }

        public GoodUnitDomainService GoodUnitDomainService { get; private set; }

        public WorkflowStepRepository WorkflowStepRepository { get; private set; }

        public void RegenerateUnitOfWorkScope()
        {
            if (this.UnitOfWorkScope != null)
            {
                this.UnitOfWorkScope.CurrentUnitOfWork.Dispose();
            }
        }
    }
}