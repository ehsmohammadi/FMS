using System;
using System.Collections.Generic;
using System.Data;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Application.Service
{
    public class ScrapApplicationService : IScrapApplicationService
    {
        private readonly IScrapFactory scrapFactory;
        private readonly IScrapRepository scrapRepository;
        private readonly IUnitOfWorkScope unitOfWorkScope;


        private readonly IScrapDomainService scrapDomainService;
        private readonly IVesselInCompanyDomainService vesselDomianService;
        private readonly ICompanyDomainService companyDomainService;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IGoodUnitDomainService goodUnitDomainService;
        private readonly ITankDomainService tankDomainService;


        public ScrapApplicationService(
            IScrapFactory scrapFactory,
            IScrapRepository scrapRepository,
            IUnitOfWorkScope unitOfWorkScope,
            IScrapDomainService scrapDomainService,
            IVesselInCompanyDomainService vesselDomianService,
            ICompanyDomainService companyDomainService,
            ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService,
            IGoodUnitDomainService goodUnitDomainService,
            ITankDomainService tankDomainService)
        {
            this.scrapFactory = scrapFactory;
            this.scrapRepository = scrapRepository;
            this.unitOfWorkScope = unitOfWorkScope;

            this.scrapDomainService = scrapDomainService;
            this.vesselDomianService = vesselDomianService;
            this.companyDomainService = companyDomainService;
            this.currencyDomainService = currencyDomainService;
            this.goodDomainService = goodDomainService;
            this.goodUnitDomainService = goodUnitDomainService;
            this.tankDomainService = tankDomainService;
        }

        //================================================================================

        public Scrap ScrapVessel(long vesselInCompanyId, long secondPartyId, DateTime scrapDate, long userId)
        {
            var vessel = vesselDomianService.Get(vesselInCompanyId);
            if (vessel == null)
                throw new ObjectNotFound("Vessel", vesselInCompanyId);

            var secondParty = companyDomainService.Get(secondPartyId);
            if (secondParty == null)
                throw new ObjectNotFound("SecondParty", secondPartyId);

            var scrap = this.scrapFactory.CreateScrap(vessel, secondParty, scrapDate,userId);

            scrapRepository.Add(scrap);

            unitOfWorkScope.Commit();

            return scrap;
        }

        //================================================================================

        public Scrap UpdateScrap(long scrapId, long vesselInCompanyId, long secondPartyId, DateTime scrapDate)
        {
            var vessel = vesselDomianService.Get(vesselInCompanyId);
            if (vessel == null)
                throw new ObjectNotFound("Vessel", vesselInCompanyId);

            var secondParty = companyDomainService.Get(secondPartyId);
            if (secondParty == null)
                throw new ObjectNotFound("SecondParty", secondPartyId);

            var scrapToUpdate = scrapDomainService.Get(scrapId);

            scrapToUpdate.Update(vessel, secondParty, scrapDate, scrapDomainService, vesselDomianService, companyDomainService);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("UpdateScrap");
            }

            return scrapDomainService.Get(scrapId);
        }

        //================================================================================

        public void DeleteScrap(long scrapId)
        {
            var scrapToDelete = scrapDomainService.Get(scrapId);

            scrapToDelete.Delete(scrapDomainService);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("DeleteScrap");
            }
        }

        //================================================================================

        public ScrapDetail AddScrapDetail(long scrapId, double rob, double price, long currencyId, long goodId, long unitId, long tankId)
        {
            var currency = currencyDomainService.Get(currencyId);
            if (currency == null)
                throw new ObjectNotFound("Currency");

            var good = goodDomainService.Get(goodId);
            if (good == null)
                throw new ObjectNotFound("Good");

            var unit = goodUnitDomainService.Get(unitId);
            if (unit == null)
                throw new ObjectNotFound("Unit");

            var tank = tankDomainService.Get(tankId);
            if (tank == null)
                throw new ObjectNotFound("Tank");



            var scrap = scrapDomainService.Get(scrapId);

            var scrapDetail = scrapFactory.CreateScrapDetail(scrap, rob, price, currency, good, unit, tank);

            scrap.AddDetail(scrapDetail);

            //scrapRepository.Add();

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("AddScrapDetail");
            }

            return scrapDetail;
        }

        //================================================================================

        public ScrapDetail UpdateScrapDetail(long scrapId, long scrapDetailId, double rob, double price, long currencyId, long goodId, long unitId, long tankId)
        {
            var currency = currencyDomainService.Get(currencyId);
            if (currency == null)
                throw new ObjectNotFound("Currency");

            var good = goodDomainService.Get(goodId);
            if (good == null)
                throw new ObjectNotFound("Good");

            var unit = goodUnitDomainService.Get(unitId);
            if (unit == null)
                throw new ObjectNotFound("Unit");

            var tank = tankDomainService.Get(tankId);
            //if (tank == null)
            //    throw new ObjectNotFound("Tank");


            var scrap = scrapDomainService.Get(scrapId);

            var scrapDetail = scrap.UpdateDetail(scrapDetailId, rob, price, currency, good, unit, tank,
                this.tankDomainService, this.currencyDomainService, this.goodDomainService, this.goodUnitDomainService);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("UpdateScrapDetail");
            }

            return scrapDetail;
        }

        //================================================================================

        public void DeleteScrapDetail(long scrapId, long scrapDetailId)
        {
            var scrap = scrapDomainService.Get(scrapId);

            scrap.DeleteDetail(scrapDetailId, this.scrapDomainService);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("UpdateScrapDetail");
            }
        }

        //================================================================================
    }
}