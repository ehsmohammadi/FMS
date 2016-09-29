using System;
using System.Transactions;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Application.Service
{
    public class VesselApplicationService : IVesselApplicationService
    {
        private readonly IVesselInCompanyDomainService vesselInCompanyDomainService;
        private readonly IFuelUserDomainService fuelUserDomainService;
        private readonly IUnitOfWorkScope unitOfWorkScope;


        public VesselApplicationService(
            IVesselInCompanyDomainService vesselInCompanyDomainService,
            IFuelUserDomainService fuelUserDomainService,
            IUnitOfWorkScope unitOfWorkScope)
        {
            this.vesselInCompanyDomainService = vesselInCompanyDomainService;
            this.fuelUserDomainService = fuelUserDomainService;
            this.unitOfWorkScope = unitOfWorkScope;
        }

        //================================================================================

        public void AddVessel(VesselDto vessel)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;

            using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                vesselInCompanyDomainService.RegisterNewVessel(vessel.OwnerId, vessel.Code, vessel.Name, vessel.Description, DateTime.Now, (int)fuelUserDomainService.GetCurrentFuelUserId()); // TODO: Temp long to int cast 
            
                unitOfWorkScope.Commit();

                tran.Complete();
            }
        }
    }
}