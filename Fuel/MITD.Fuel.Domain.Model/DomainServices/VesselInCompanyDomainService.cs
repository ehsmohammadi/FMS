using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Transactions;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.Enums.Inventory;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class VesselInCompanyDomainService : IVesselInCompanyDomainService
    {
        private ICompanyRepository companyRepository;
        private IVesselRepository vesselRepository;

        private readonly IVesselInCompanyRepository vesselInCompanyRepository;

        private readonly IVoyageRepository voyageRepository;

        private readonly IInventoryOperationManager inventoryOperationManager;

        public VesselInCompanyDomainService(
            IVesselInCompanyRepository vesselInCompanyRepository,
            IVoyageRepository voyageRepository, IVesselRepository vesselRepository, IInventoryOperationManager inventoryOperationManager, ICompanyRepository companyRepository)
        {
            this.vesselInCompanyRepository = vesselInCompanyRepository;
            this.voyageRepository = voyageRepository;
            this.vesselRepository = vesselRepository;
            this.companyRepository = companyRepository;
            this.inventoryOperationManager = inventoryOperationManager;
        }

        public List<VesselInCompany> Get(List<long> IDs)
        {
            return this.vesselInCompanyRepository.Find(c => IDs.Contains(c.Id)).ToList();
        }

        public List<VesselInCompany> GetCompanyVessels(long companyId)
        {
            return this.vesselInCompanyRepository.Find(v => v.CompanyId == companyId).ToList();
        }

        public VesselInCompany GetVesselInCompany(long companyId, string vesselCode)
        {
            return this.vesselInCompanyRepository.Single(v => v.CompanyId == companyId && v.Vessel.Code == vesselCode);
        }

        public List<VesselInCompany> GetVesselInCompanies(string vesselCode)
        {
            return this.vesselInCompanyRepository.Find(v => v.Vessel.Code == vesselCode).ToList();
        }

        public List<VesselInCompany> GetVesselInCompanies(long? companyId, string vesselStates)
        {
            var vesselStatesValues = vesselStates.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            
            var fetchStrategy = new ListFetchStrategy<VesselInCompany>().Include(p => p.Company).Include(p => p.Vessel);

            return this.vesselInCompanyRepository.Find(v => (!companyId.HasValue || v.CompanyId == companyId) &&
                (vesselStates == null || vesselStatesValues.Contains((int) v.VesselStateCode)), fetchStrategy).ToList();
        }

        public List<VesselInCompany> GetAll()
        {
            return this.vesselInCompanyRepository.GetAll().ToList();
        }

        public VesselInCompany Get(long id)
        {
            return this.vesselInCompanyRepository.Find(c => c.Id == id).FirstOrDefault();
        }

        public VesselInCompany GetWithTanks(long id)
        {
            return this.vesselInCompanyRepository.Find(c => c.Id == id).FirstOrDefault();
        }

        //public bool CompanyHaveValidVessel(long? vesselInCompanyId, long companyId)
        //{
        //    return vesselFakeRepository.Count(v => v.Id == vesselInCompanyId.Value && v.CompanyId == companyId) == 1;
        //}

        //public void IsValid(long value)
        //{
        //    vesselFakeRepository.Find(v => v.Id == value);
        //}

        public List<Voyage> Find(long companyId, long vesselInCompanyId)
        {
            return this.voyageRepository.Find(voy => voy.CompanyId == companyId && voy.VesselInCompanyId == vesselInCompanyId).ToList();
        }


        public List<VesselInCompany> GetInactiveVessels(long companyId)
        {
            return this.vesselInCompanyRepository.Find(v => v.CompanyId == companyId && v.VesselStateCode == VesselStates.Inactive).ToList();
        }

        public List<VesselInCompany> GetOwnedOrCharterInVessels(long companyId)
        {
            return this.vesselInCompanyRepository.Find(v => v.CompanyId == companyId &&
                                                 (v.VesselStateCode == VesselStates.CharterIn
                                                  || v.VesselStateCode == VesselStates.Owned)).ToList();
        }

        public List<VesselInCompany> GetOwnedVessels(long companyId)
        {
            return this.vesselInCompanyRepository.Find(v => v.CompanyId == companyId && v.VesselStateCode == VesselStates.Owned).ToList();
        }
        public List<VesselActivationItem> GetVesselActivationInfo(string vesselCode, out DateTime vesselActivationDate)
        {
            List<VesselActivationItem> vesselActivationItems = null;

            // Find corresponding inventory transaction
            IRepository<Inventory_Transaction> inventoryTransactionRepository =
                ServiceLocator.Current.GetInstance<IRepository<Inventory_Transaction>>();

            vesselActivationDate = DateTime.MinValue;
            try
            {
                var inventoryTransaction =
                    inventoryTransactionRepository.Single(
                        it => it.ReferenceType == InventoryOperationReferenceTypes.INVENTORY_INITIATION &&
                              it.ReferenceNo == vesselCode);

                if (inventoryTransaction != null)
                {
                    vesselActivationDate = inventoryTransaction.RegistrationDate.Value;
                    vesselActivationItems = new List<VesselActivationItem>();
                    var query =
                        from iti in inventoryTransaction.Inventory_TransactionItem
                        select new VesselActivationItem()
                        {
                            GoodId = iti.GoodId,
                            GoodName = iti.Inventory_Good.Name,
                            GoodCode = iti.Inventory_Good.Code,
                            UnitId = iti.Inventory_TransactionItemPrice.First().Inventory_Unit_QuantityUnit.Id,
                            UnitCode = iti.Inventory_TransactionItemPrice.First().Inventory_Unit_QuantityUnit.Abbreviation,
                            CurrencyId = iti.Inventory_TransactionItemPrice.First().Inventory_Unit_PriceUnit.Id,
                            CurrencyCode = iti.Inventory_TransactionItemPrice.First().Inventory_Unit_PriceUnit.Abbreviation,
                            Fee = iti.Inventory_TransactionItemPrice.First().Fee.GetValueOrDefault(),
                            Rob = iti.QuantityAmount.GetValueOrDefault(),
                        };
                    vesselActivationItems = query.ToList<VesselActivationItem>();
                }
            }
            catch
            {
                // Vessel has not been activated yet
            }
            return vesselActivationItems;
        }

        public VesselStates GetVesselCurrentState(long id)
        {
            return this.vesselInCompanyRepository.First(v => v.Id == id).VesselStateCode;
        }

        public List<VesselStates> GetVesselStatesLog(long id, DateTime? fromDate, DateTime? toDate)
        {
            return new List<VesselStates>() { VesselStates.Owned };
        }

        public void ScrapVessel(long vesselInCompanyId)
        {
            var vesselInCompany = this.vesselInCompanyRepository.First(v => v.Id == vesselInCompanyId);
            vesselInCompany.Scrap();
        }


        public void ActivateVessel(long vesselInCompanyId)
        {
            var vesselInCompany = this.vesselInCompanyRepository.First(v => v.Id == vesselInCompanyId);
            vesselInCompany.Activate();
        }

        public void DeactivateVessel(long vesselInCompanyId)
        {
            var vesselInCompany = this.vesselInCompanyRepository.First(v => v.Id == vesselInCompanyId);
            vesselInCompany.Deactivate();
        }

        public void BeginCharteringInVessel(long vesselInCompanyId, DateTime charterInStartDateTime, long userId)
        {
            var vesselInCompany = this.vesselInCompanyRepository.First(v => v.Id == vesselInCompanyId);
            vesselInCompany.StartCharterIn();

            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
            fuelReportDomainService.MoveFuelReportsToCompany(vesselInCompany.Code, vesselInCompany.CompanyId, charterInStartDateTime.AddSeconds(1),null, userId);
        }

        public void FinishCharteringInVessel(long vesselInCompanyId)
        {
            var vesselInCompany = this.vesselInCompanyRepository.First(v => v.Id == vesselInCompanyId);
            vesselInCompany.EndCharterIn();
        }

        public void BeginCharteringOutVessel(long vesselInCompanyId, DateTime charterOutstartDateTime, long userId)
        {
            var vesselInCompany = this.vesselInCompanyRepository.First(v => v.Id == vesselInCompanyId);
            vesselInCompany.StartCharterOut();

            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
            fuelReportDomainService.ChangeFuelReportsStateForCharteredOutVessel(vesselInCompany, charterOutstartDateTime, null, userId);
        }

        public void FinishCharteringOutVessel(long vesselInCompanyId, DateTime charterOutEndDateTime, long userId) 
        {
            var vesselInCompany = this.vesselInCompanyRepository.First(v => v.Id == vesselInCompanyId);
            vesselInCompany.EndCharterOut();

            //<A.H>
            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
            fuelReportDomainService.MoveFuelReportsToCompany(vesselInCompany.Code, vesselInCompany.CompanyId, charterOutEndDateTime.AddSeconds(1), null, userId);

        }

        public void RegisterNewVessel(long ownerId, string vesselCode, string name, string description, DateTime activationTime, int userId)
        {
            var ownerCompany = companyRepository.Single(c => c.Id == ownerId);

            if (ownerCompany == null)
                throw new ObjectNotFound("OwnerCompany");

            var vessel = new Vessel(vesselCode, ownerId);
            vesselRepository.Add(vessel);

            var companiesList = companyRepository.GetAll();

            long lastWarehouseId;

            var warehouseRepository = ServiceLocator.Current.GetInstance<IRepository<Inventory_Warehouse>>();

            lastWarehouseId = warehouseRepository.GetAll().OrderBy(w => w.Id).LastOrDefault().Id;

            foreach (var company in companiesList)
            {
                this.AssignVesselToCompany(vessel, company, name, description, userId, ++lastWarehouseId);
            }
        }

        public void AssignVesselToCompany(Vessel vessel, Company company, string name, string description, int userId, long warehouseId)
        {
            //var transactionOptions = new TransactionOptions();
            //transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;

            //using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            //{
                var vesselInCompany = new VesselInCompany(name, description, company.Id, vessel, VesselStates.Inactive, false);

                vesselInCompanyRepository.Add(vesselInCompany);
                inventoryOperationManager.RegisterInventory(warehouseId, company.Id, vessel.Code, name, description, userId);
                //tran.Complete();
            //}
        }
    }
}
