using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.Commands;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Application.Service
{
    public class OffhireApplicationService : IOffhireApplicationService
    {
        private readonly IOffhireFactory offhireFactory;
        private readonly IOffhireRepository offhireRepository;
        private readonly IUnitOfWorkScope unitOfWorkScope;


        private readonly IOffhireDomainService offhireDomainService;
        private readonly IVesselInCompanyDomainService vesselDomianService;
        private readonly IVoyageDomainService voyageDomianService;
        private readonly ICompanyDomainService companyDomainService;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IGoodUnitDomainService goodUnitDomainService;
        private readonly ITankDomainService tankDomainService;
        private readonly IActivityLocationDomainService activityLocationDomainService;
        private readonly IOffhireManagementSystemDomainService offhireManagementSystemDomainService;

        public OffhireApplicationService(
            IOffhireFactory offhireFactory,
            IOffhireRepository offhireRepository,
            IUnitOfWorkScope unitOfWorkScope,
            IOffhireDomainService offhireDomainService,
            IEntityConfigurator<Offhire> offhireConfigurator,
            IVesselInCompanyDomainService vesselDomianService,
            IVoyageDomainService voyageDomianService,
            ICompanyDomainService companyDomainService,
            ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService,
            IGoodUnitDomainService goodUnitDomainService,
            ITankDomainService tankDomainService,
            IActivityLocationDomainService activityLocationDomainService,
            IOffhireManagementSystemDomainService offhireManagementSystemDomainService)
        {
            this.offhireFactory = offhireFactory;
            this.offhireRepository = offhireRepository;
            this.unitOfWorkScope = unitOfWorkScope;

            this.offhireDomainService = offhireDomainService;
            this.vesselDomianService = vesselDomianService;
            this.companyDomainService = companyDomainService;
            this.currencyDomainService = currencyDomainService;
            this.goodDomainService = goodDomainService;
            this.goodUnitDomainService = goodUnitDomainService;
            this.tankDomainService = tankDomainService;
            this.activityLocationDomainService = activityLocationDomainService;
            this.offhireManagementSystemDomainService = offhireManagementSystemDomainService;
            this.voyageDomianService = voyageDomianService;

            this.offhireDomainService.SetConfigurator(offhireConfigurator);
        }

        //================================================================================

        public Offhire AddOffhire(OffhireCommand command, long userId)
        {
            var vessel = vesselDomianService.Get(command.VesselInCompanyId);
            if (vessel == null)
                throw new ObjectNotFound("Vessel", command.VesselInCompanyId);

            var introducer = companyDomainService.Get(command.IntroducerId);
            if (introducer == null)
                throw new ObjectNotFound("Introducer", command.IntroducerId);

            Voyage voyage = null;
            if (command.VoyageId.HasValue)
            {
                voyage = voyageDomianService.Get(command.VoyageId.Value);
                if (voyage == null)
                    throw new ObjectNotFound("Voyage", command.VoyageId.Value);
            }

            var offhireLocation = activityLocationDomainService.Get(command.OffhireLocationId);
            if (offhireLocation == null)
                throw new ObjectNotFound("OffhireLocation", command.OffhireLocationId);

            var voucherCurrency = currencyDomainService.Get(command.VoucherCurrencyId);
            if (voucherCurrency == null)
                throw new ObjectNotFound("VoucherCurrency", command.VoucherCurrencyId);

            var offhire = offhireFactory.CreateOffhire(command.ReferenceNumber, command.StartDateTime, command.EndDateTime, introducer, vessel, voyage, offhireLocation, command.VoucherDate, voucherCurrency,userId);

            foreach (var commandDetail in command.OffhireDetails)
            {
                var good = goodDomainService.Get(commandDetail.GoodId);
                if (good == null)
                    throw new ObjectNotFound("Good", commandDetail.GoodId);

                var unit = goodUnitDomainService.Get(commandDetail.UnitId);
                if (unit == null)
                    throw new ObjectNotFound("Unit", commandDetail.UnitId);

                Tank tank = null;

                if (commandDetail.TankId.HasValue)
                {
                    tank = tankDomainService.Get(commandDetail.TankId.Value);
                    if (tank == null)
                        throw new ObjectNotFound("Tank", commandDetail.TankId.Value);
                }

                if (!commandDetail.FeeInVoucherCurrency.HasValue)
                    throw new InvalidArgument("FeeInVoucherCurrency For Good " + good.Name);

                if (!commandDetail.FeeInMainCurrency.HasValue)
                    throw new InvalidArgument("FeeInMainCurrency For Good " + good.Name);

                var detail = offhireFactory.CreateOffhireDetail(commandDetail.Quantity, commandDetail.FeeInVoucherCurrency.Value, commandDetail.FeeInMainCurrency.Value, good, unit, tank, offhire);

                offhire.AddDetail(detail);
            }

            offhireRepository.Add(offhire);

            unitOfWorkScope.Commit();

            return offhire;
        }

        //================================================================================

        public Offhire UpdateOffhire(OffhireCommand command)
        {
            var voucherCurrency = currencyDomainService.Get(command.VoucherCurrencyId);
            if (voucherCurrency == null)
                throw new ObjectNotFound("VoucherCurrency", command.VoucherCurrencyId);

            var offhireToUpdate = offhireDomainService.Get(command.Id);

            offhireToUpdate.Update(command.VoucherDate, voucherCurrency, currencyDomainService);

            //offhireRepository.Update(offhireToUpdate);

            //unitOfWorkScope.Commit();


            foreach (var commandDetail in command.OffhireDetails)
            {
                if (!commandDetail.FeeInVoucherCurrency.HasValue)
                    throw new InvalidArgument("FeeInVoucherCurrency");

                if (!commandDetail.FeeInMainCurrency.HasValue)
                    throw new InvalidArgument("FeeInMainCurrency");

                offhireToUpdate.UpdateDetail(commandDetail.Id, commandDetail.FeeInVoucherCurrency.Value, commandDetail.FeeInMainCurrency.Value, offhireDomainService,
                tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);
            }

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("UpdateOffhire");
            }

            return offhireToUpdate;
        }

        //================================================================================

        public void DeleteOffhire(long offhireId)
        {
            var offhireToDelete = offhireDomainService.Get(offhireId);

            offhireToDelete.Delete(offhireDomainService);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("DeleteOffhire");
            }
        }

        //================================================================================

        //public OffhireDetail AddOffhireDetail(long offhireId, decimal quantity, decimal feeInVoucherCurrency, decimal feeInMainCurrency, long goodId, long unitId, long tankId)
        //{
        //    var currency = currencyDomainService.Get(currencyId);
        //    if (currency == null)
        //        throw new ObjectNotFound("Currency");

        //    var good = goodDomainService.Get(goodId);
        //    if (good == null)
        //        throw new ObjectNotFound("Good");

        //    var unit = goodUnitDomainService.Get(unitId);
        //    if (unit == null)
        //        throw new ObjectNotFound("Unit");

        //    var tank = tankDomainService.Get(tankId);
        //    if (tank == null)
        //        throw new ObjectNotFound("Tank");



        //    var offhire = offhireDomainService.Get(offhireId);

        //    var offhireDetail = offhireFactory.CreateOffhireDetail(quantity, feeInVoucherCurrency, feeInMainCurrency, good, unit, tank, offhire);

        //    offhire.AddDetail(offhireDetail);

        //    //offhireRepository.Add();

        //    try
        //    {
        //        unitOfWorkScope.Commit();
        //    }
        //    catch (OptimisticConcurrencyException ex)
        //    {
        //        throw new ConcurencyException("AddOffhireDetail");
        //    }

        //    return offhireDetail;
        //}

        ////================================================================================

        //public OffhireDetail UpdateOffhireDetail(long offhireId, long offhireDetailId, double rob, double price, long currencyId, long goodId, long unitId, long tankId)
        //{
        //    var currency = currencyDomainService.Get(currencyId);
        //    if (currency == null)
        //        throw new ObjectNotFound("Currency");

        //    var good = goodDomainService.Get(goodId);
        //    if (good == null)
        //        throw new ObjectNotFound("Good");

        //    var unit = goodUnitDomainService.Get(unitId);
        //    if (unit == null)
        //        throw new ObjectNotFound("Unit");

        //    var tank = tankDomainService.Get(tankId);
        //    //if (tank == null)
        //    //    throw new ObjectNotFound("Tank");


        //    var offhire = offhireDomainService.Get(offhireId);

        //    var offhireDetail = offhire.UpdateDetail(offhireDetailId, rob, price, currency, good, unit, tank,
        //        this.tankDomainService, this.currencyDomainService, this.goodDomainService, this.goodUnitDomainService);

        //    try
        //    {
        //        unitOfWorkScope.Commit();
        //    }
        //    catch (OptimisticConcurrencyException ex)
        //    {
        //        throw new ConcurencyException("UpdateOffhireDetail");
        //    }

        //    return offhireDetail;
        //}

        ////================================================================================

        //public void DeleteOffhireDetail(long offhireId, long offhireDetailId)
        //{
        //    var offhire = offhireDomainService.Get(offhireId);

        //    offhire.DeleteDetail(offhireDetailId, this.offhireDomainService);

        //    try
        //    {
        //        unitOfWorkScope.Commit();
        //    }
        //    catch (OptimisticConcurrencyException ex)
        //    {
        //        throw new ConcurencyException("UpdateOffhireDetail");
        //    }
        //}

        ////================================================================================

        public OffhirePreparedData GetPreparedData(long referenceNumber, long introducerId)
        {
            var introducer = companyDomainService.Get(introducerId);
            if (introducer == null)
                throw new ObjectNotFound("Introducer", introducerId);

            var finalizedOffhire = offhireManagementSystemDomainService.GetFinalizedOffhire(referenceNumber, introducerId);
            if (finalizedOffhire == null)
                throw new ObjectNotFound("FinalizedOffhire", introducerId);

            var preparedData = convertToPreparedData(introducer, finalizedOffhire);

            return preparedData;
        }

        //================================================================================

        private OffhirePreparedData convertToPreparedData(Company introducer,  OffhireManagementSystemEntity finalizedOffhire)
        {
            return new OffhirePreparedData()
                   {
                       ReferenceNumber = finalizedOffhire.ReferenceNumber,
                       StartDateTime = finalizedOffhire.StartDateTime,
                       EndDateTime = finalizedOffhire.EndDateTime,
                       Introducer = introducer,
                       HasVoucher = finalizedOffhire.HasVoucher,
                       Location = finalizedOffhire.Location,
                       VesselInCompany = finalizedOffhire.VesselInCompany,
                       Voyage = voyageDomianService.GetVoyageContainingDuration(introducer, finalizedOffhire.VesselInCompany, finalizedOffhire.StartDateTime, finalizedOffhire.EndDateTime),
                       OffhireDetails = finalizedOffhire.OffhireDetails.Select(convertDetailToPreparedDataDetail).ToList()
                   };
        }

        //================================================================================

        private OffhirePreparedDataDetail convertDetailToPreparedDataDetail(OffhireManagementSystemEntityDetail finalizedOffhireDetail)
        {
            return new OffhirePreparedDataDetail()
            {
                Good = finalizedOffhireDetail.Good,
                Quantity = finalizedOffhireDetail.QuantityAmount,
                Unit = finalizedOffhireDetail.Unit,
                Tank = finalizedOffhireDetail.Tank
            };
        }

        //================================================================================

        public PricingValue GetOffhirePricingValueInVoucherCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long goodId, long voucherCurrencyId, DateTime voucherDate)
        {
            var introducer = companyDomainService.Get(introducerId);
            if (introducer == null)
                throw new ObjectNotFound("Introducer", introducerId);

            var vessel = vesselDomianService.Get(vesselInCompanyId);
            if (vessel == null)
                throw new ObjectNotFound("Vessel", vesselInCompanyId);

            var good = goodDomainService.Get(goodId);
            if (good == null)
                throw new ObjectNotFound("Good", introducerId);

            var voucherCurrency = currencyDomainService.Get(voucherCurrencyId);
            if (voucherCurrency == null)
                throw new ObjectNotFound("VoucherCurrency", voucherCurrencyId);

            return offhireDomainService.GetPricingValue(introducer, vessel, startDateTime, good, voucherCurrency, voucherDate);
        }

        //================================================================================

        public PricingValue GetOffhirePricingValueInMainCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long goodId)
        {
            var introducer = companyDomainService.Get(introducerId);
            if (introducer == null)
                throw new ObjectNotFound("Introducer", introducerId);

            var vessel = vesselDomianService.Get(vesselInCompanyId);
            if (vessel == null)
                throw new ObjectNotFound("Vessel", vesselInCompanyId);

            var good = goodDomainService.Get(goodId);
            if (good == null)
                throw new ObjectNotFound("Good", introducerId);

            var mainCurrency = currencyDomainService.GetMainCurrency();
            if (mainCurrency == null)
                throw new ObjectNotFound("SystemMainCurrency");

            return offhireDomainService.GetPricingValue(introducer, vessel, startDateTime, good, mainCurrency, startDateTime);
        }

        //================================================================================

        public List<PricingValue> GetOffhirePricingValuesInVoucherCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime, long voucherCurrencyId, DateTime voucherDate)
        {
            var introducer = companyDomainService.Get(introducerId);
            if (introducer == null)
                throw new ObjectNotFound("Introducer", introducerId);

            var vessel = vesselDomianService.Get(vesselInCompanyId);
            if (vessel == null)
                throw new ObjectNotFound("Vessel", vesselInCompanyId);

            var voucherCurrency = currencyDomainService.Get(voucherCurrencyId);
            if (voucherCurrency == null)
                throw new ObjectNotFound("VoucherCurrency", voucherCurrencyId);

            return offhireDomainService.GetPricingValues(introducer, vessel, startDateTime, voucherCurrency, voucherDate);
        }

        //================================================================================

        public List<PricingValue> GetOffhirePricingValuesInMainCurrency(long introducerId, long vesselInCompanyId, DateTime startDateTime)
        {
            var introducer = companyDomainService.Get(introducerId);
            if (introducer == null)
                throw new ObjectNotFound("Introducer", introducerId);

            var vessel = vesselDomianService.Get(vesselInCompanyId);
            if (vessel == null)
                throw new ObjectNotFound("Vessel", vesselInCompanyId);

            var mainCurrency = currencyDomainService.GetMainCurrency();
            if (mainCurrency == null)
                throw new ObjectNotFound("SystemMainCurrency");

            return offhireDomainService.GetPricingValues(introducer, vessel, startDateTime, mainCurrency, startDateTime);
        }

        //================================================================================
    }
}