using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class OffhireDomainService : IOffhireDomainService
    {
        private readonly IOffhireRepository offhireRepository;
        private readonly IRepository<OffhireDetail> offhireDetailRepository;
        private readonly ICharteringDomainService charteringDomainService;
        private readonly IInventoryManagementDomainService inventoryManagementDomainService;
        private readonly IFuelReportDomainService fuelReportDomainService;
        private readonly IVesselInCompanyDomainService vesselDomainService;
        private readonly IVoyageDomainService voyageDomainService;
        private readonly ICurrencyDomainService currencyDomainService;

        private IEntityConfigurator<Offhire> offhireConfigurator;


        private readonly IsOffhireOpen isOffhireOpen;
        private readonly IsOffhireSubmitted isOffhireSubmitted;
        private readonly IsOffhireSubmitRejected isOffhireSubmitRejected;
        private readonly IsOffhireCancelled isOffhireCancelled;

        public OffhireDomainService(
            IOffhireRepository offhireRepository,
            IRepository<OffhireDetail> offhireDetailRepository,
            ICharteringDomainService charteringDomainService,
            IInventoryManagementDomainService inventoryManagementDomainService,
            IFuelReportDomainService fuelReportDomainService,
            IVesselInCompanyDomainService vesselDomainService, IVoyageDomainService voyageDomainService,
            ICurrencyDomainService currencyDomainService)
        {
            this.offhireRepository = offhireRepository;
            this.offhireDetailRepository = offhireDetailRepository;
            this.charteringDomainService = charteringDomainService;
            this.inventoryManagementDomainService = inventoryManagementDomainService;
            this.fuelReportDomainService = fuelReportDomainService;
            this.vesselDomainService = vesselDomainService;
            this.voyageDomainService = voyageDomainService;
            this.currencyDomainService = currencyDomainService;

            this.isOffhireOpen = new IsOffhireOpen();
            this.isOffhireSubmitted = new IsOffhireSubmitted();
            this.isOffhireSubmitRejected = new IsOffhireSubmitRejected();
            this.isOffhireCancelled = new IsOffhireCancelled();

        }

        //================================================================================

        public void SetConfigurator(IEntityConfigurator<Offhire> offhireConfigurator)
        {
            this.offhireConfigurator = offhireConfigurator;
            this.offhireRepository.SetConfigurator(this.offhireConfigurator);
        }

        //================================================================================

        public Offhire Get(long offhireId)
        {
            var offhire = offhireRepository.First(e => e.Id == offhireId);

            if (offhire == null)
                throw new ObjectNotFound("Offhire", offhireId);

            //this.offhireConfigurator.Configure(offhire);

            return offhire;
        }

        //================================================================================

        public PageResult<Offhire> GetPagedData(int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Offhire>()
                .Include(s => s.VesselInCompany).Include(s => s.Introducer)
                .WithPaging(pageSize, pageNumber);

            offhireRepository.GetAll(fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public PageResult<Offhire> GetPagedDataByFilter(long? companyId, long? vesselInCompanyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Offhire>()
                .Include(s => s.VesselInCompany).Include(s => s.Introducer)
                .WithPaging(pageSize, pageNumber);

            offhireRepository.Find(
                e => (!companyId.HasValue || e.VesselInCompany.Company.Id == companyId) &&
                    (!vesselInCompanyId.HasValue || e.VesselInCompany.Id == vesselInCompanyId) &&
                    (!fromDate.HasValue || e.StartDateTime >= fromDate) &&
                    (!toDate.HasValue || e.StartDateTime <= toDate),
                fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public PageResult<OffhireDetail> GetPagedOffhireDetailData(long offhireId, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<OffhireDetail>()
                .WithPaging(pageSize, pageNumber);

            offhireDetailRepository.Find(sd => sd.Offhire.Id == offhireId, fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public OffhireDetail GetOffhireDetail(long offhireId, long offhireDetailId)
        {
            var offhireDetail = offhireDetailRepository.Single(sd => sd.Offhire.Id == offhireId && sd.Id == offhireDetailId);

            if (offhireDetail == null)
                throw new ObjectNotFound("OffhireDetail", offhireDetailId);

            //this.offhireConfigurator.Configure(offhireDetail.Offhire);

            return offhireDetail;
        }

        //================================================================================

        public void Delete(Offhire offhire)
        {
            offhireRepository.Delete(offhire);
        }

        //================================================================================

        public void DeleteOffhireDetail(OffhireDetail offhireDetail)
        {
            offhireDetailRepository.Delete(offhireDetail);
        }

        //================================================================================

        public List<Offhire> GetNotCancelledOffhiresForVessel(VesselInCompany vesselInCompany, params long[] excludeIds)
        {
            var result = offhireRepository.Find(isOffhireCancelled.Predicate.Not().And(s => s.VesselInCompany.Id == vesselInCompany.Id));

            if (excludeIds != null)
            {
                result = result.Where(s => !excludeIds.Contains(s.Id)).ToList();
            }

            return result.ToList();
        }

        //================================================================================

        public bool IsOffhireEditPermitted(Offhire offhire)
        {
            return isOffhireOpen.IsSatisfiedBy(offhire) || isOffhireSubmitRejected.IsSatisfiedBy(offhire);
        }

        //================================================================================

        public bool IsOffhireDeletePermitted(Offhire offhire)
        {
            return isOffhireOpen.IsSatisfiedBy(offhire);
        }

        //================================================================================

        public bool IsOffhireRegistered(long referenceNumber, Company introducer)
        {
            return this.offhireRepository.Count(o => o.ReferenceNumber == referenceNumber && o.Introducer.Id == introducer.Id) > 0;
        }

        //================================================================================

        public Charter GetCharterContract(Company introducer, VesselInCompany vesselInCompany, CharteringPartyType introducerType, DateTime stratDate)
        {
            switch (introducerType)
            {
                case CharteringPartyType.ShipOwner:

                    //If the given company is in the role of ShipOwner, it will be inferred that 
                    //the pricing type must be fetched from Charter-Out contract.

                    var foundCharterOut = charteringDomainService.GetCharterOutStart(introducer, vesselInCompany, stratDate);

                    //TODO: State of CharterOut must be submitted.
                    if (foundCharterOut == null /*|| foundCharterOut.State != States.Submitted*/)
                        throw new BusinessRuleException("CharterOutForPricingType", "Charter-Out contract not found.");

                    return foundCharterOut;

                case CharteringPartyType.Charterer:

                    //If the given company is in the role of Charterer, it will be inferred that 
                    //the pricing type must be fetched from Charter-In contract.

                    var foundCharterIn = charteringDomainService.GetCharterInStart(introducer, vesselInCompany, stratDate);

                    if (foundCharterIn == null /*|| foundCharterIn.State != States.Submitted*/)
                        throw new BusinessRuleException("CharterInForPricingType", "Charter-In contract not found.");

                    return foundCharterIn;

                default:
                    throw new InvalidArgument("IntroducerType");
            }
        }

        //================================================================================

        public Offhire GetRelevantCharterInOffhire(Company shipOwner, VesselInCompany vesselInCompany, DateTime startDate)
        {
            var charterOutContract = GetCharterContract(shipOwner, vesselInCompany, CharteringPartyType.ShipOwner, startDate) as CharterOut;

            var charterInCompanyOffhire = GetCompanyValidOffhire(charterOutContract.Charterer, vesselInCompany, startDate);

            return charterInCompanyOffhire;
        }

        //================================================================================

        public Offhire GetCompanyValidOffhire(Company company, VesselInCompany vesselInCompany, DateTime startDate)
        {
            ISingleResultFetchStrategy<Offhire> fetchStrategy = new SingleResultFetchStrategy<Offhire>()
                .Include(o => o.OffhireDetails);

            var offhire = offhireRepository.Single(o => o.Introducer.Id == company.Id && o.VesselInCompany.Id == vesselInCompany.Id && o.StartDateTime == startDate, fetchStrategy);

            if (offhire == null)
                throw new ObjectNotFound("CompanyValidOffhire");

            //this.offhireConfigurator.Configure(offhire);

            if (!this.isOffhireSubmitted.IsSatisfiedBy(offhire))
                throw new BusinessRuleException("", "No valid Offhire for Company found.");

            return offhire;
        }

        //================================================================================

        public InventoryResult GetVoyagePricedConsumptionIssue(long companyId, long voyageId)
        {
            var voyageConsumptionIssueId = this.GetVoyageConsumptionIssueOperation(voyageId).InventoryOperationId;

            var pricedIssueResult = inventoryManagementDomainService.GetVoyageConsumptionResult(companyId, voyageConsumptionIssueId);

            return pricedIssueResult;
        }

        //================================================================================

        public List<PricingValue> GetCharterPartyBasedPricingValues(Offhire offhire)
        {
            return getCharterPartyBasedPricingValues(offhire.Introducer, offhire.VesselInCompany, offhire.IntroducerType, offhire.StartDateTime);
        }

        //================================================================================

        private List<PricingValue> getCharterPartyBasedPricingValues(Company introducer, VesselInCompany vesselInCompany, CharteringPartyType introducerType, DateTime stratDate)
        {
            var charterContract = this.GetCharterContract(introducer, vesselInCompany, introducerType, stratDate);
            //TODO: OffhireFee might not be set.
            return charterContract.CharterItems.Select(ci => new PricingValue()
                                                             {
                                                                 Good = ci.Good,
                                                                 Currency = ci.OffhireFee == 0 ? null : charterContract.Currency,
                                                                 Fee = ci.OffhireFee == 0 ? null : (decimal?)ci.OffhireFee
                                                             }).ToList();
        }

        //================================================================================

        public string GetCharteringReferenceNumber(Company company, VesselInCompany vesselInCompany, CharteringPartyType partyType, DateTime date)
        {
            var charterContract = this.GetCharterContract(company, vesselInCompany, partyType, date);

            switch (partyType)
            {
                case CharteringPartyType.ShipOwner:

                    //If the given company is in the role of ShipOwner, it will be inferred that 
                    //the pricing type must be fetched from Charter-Out contract.

                    return charterContract.Id.ToString();

                case CharteringPartyType.Charterer:

                    //If the given company is in the role of Charterer, it will be inferred that 
                    //the pricing type must be fetched from Charter-In contract.

                    return charterContract.Id.ToString();

                default:
                    throw new ArgumentOutOfRangeException("partyType");
            }
        }

        //================================================================================

        public InventoryOperation GetVoyageConsumptionIssueOperation(long voyageId)
        {
            return fuelReportDomainService.GetVoyageConsumptionIssueOperation(voyageId);
        }

        //================================================================================

        public PricingValue GetPricingValue(Company introducer, VesselInCompany vesselInCompany, DateTime startDate, Good good)
        {
            var partyType = this.GetCharteringPartyType(vesselInCompany);

            PricingValue result = null;

            switch (GetPricingType(introducer, vesselInCompany, startDate))
            {
                case OffHirePricingType.CharterPartyBase:

                    var charterPartyBasePricingValues = this.getCharterPartyBasedPricingValues(introducer, vesselInCompany, partyType, startDate);

                    if (charterPartyBasePricingValues != null && charterPartyBasePricingValues.Count != 0)
                    {
                        //The offhire pricing in Charty Party is not mandatory.
                        result = charterPartyBasePricingValues.SingleOrDefault(pv => pv.Good.Id == good.Id);
                    }

                    break;

                case OffHirePricingType.IssueBase:

                    switch (partyType)
                    {
                        case CharteringPartyType.ShipOwner:

                            var charterInOffhire = this.GetRelevantCharterInOffhire(introducer, vesselInCompany, startDate);

                            if (charterInOffhire == null)
                                throw new ObjectNotFound("RelevantCharterInOffhire");

                            var relevantCharterInOffhireDetail = charterInOffhire.OffhireDetails.FirstOrDefault(od => od.Good.Id == good.Id);

                            if (relevantCharterInOffhireDetail == null)
                                throw new ObjectNotFound("RelevantCharterInOffhireDetail");

                            result = new PricingValue()
                            {
                                Currency = relevantCharterInOffhireDetail.Offhire.VoucherCurrency,
                                Fee = relevantCharterInOffhireDetail.FeeInVoucherCurrency,
                                Good = good
                            };

                            break;

                        case CharteringPartyType.Charterer:
                            var voyage = voyageDomainService.GetVoyage(introducer, vesselInCompany, startDate);

                            var issueBasePricingValues = this.GetIssueBasedPricingValues(voyage);
                            if (issueBasePricingValues == null || issueBasePricingValues.Count == 0)
                                throw new ObjectNotFound("IssueBased Pricing for Vessel not found.");

                            var issueBasePricingValue = issueBasePricingValues.SingleOrDefault(pv => pv.Good.Id == good.Id);

                            if (issueBasePricingValue == null)
                                throw new ObjectNotFound("IssueBased Pricing for Good " + good.Name + " not found.");

                            result = issueBasePricingValue;
                            break;

                        default:
                            throw new InvalidArgument("PartyType");
                    }

                    break;

                default:
                    throw new InvalidArgument("PricingReferenceType");
            }

            if (result != null && result.Fee.HasValue && result.Currency == null)
                throw new InvalidArgument("Pricing Value has Fee but no currency.");

            return result;
        }

        //================================================================================

        public PricingValue GetPricingValue(Company introducer, VesselInCompany vesselInCompany, DateTime startDateTime, Good good, Currency currency, DateTime currencyDateTime)
        {
            decimal? feeInNewCurrency = null;

            var pricingValue = GetPricingValue(introducer, vesselInCompany, startDateTime, good);

            if (pricingValue.Fee.HasValue && pricingValue.Currency != null)
                feeInNewCurrency = currencyDomainService.ConvertPrice(pricingValue.Fee.Value, pricingValue.Currency, currency, currencyDateTime);

            return new PricingValue()
            {
                Good = good,
                Currency = currency,
                Fee = feeInNewCurrency
            };
        }

        //================================================================================

        public List<PricingValue> GetPricingValues(Company introducer, VesselInCompany vesselInCompany, DateTime startDateTime)
        {
            var partyType = this.GetCharteringPartyType(vesselInCompany);

            var result = new List<PricingValue>();

            switch (GetPricingType(introducer, vesselInCompany, startDateTime))
            {
                case OffHirePricingType.CharterPartyBase:

                    var charterPartyBasePricingValues = this.getCharterPartyBasedPricingValues(introducer, vesselInCompany, partyType, startDateTime);

                    if (charterPartyBasePricingValues != null && charterPartyBasePricingValues.Count != 0)
                    {
                        //The offhire pricing in Charty Party is not mandatory.
                        result.AddRange(charterPartyBasePricingValues);
                    }

                    break;

                case OffHirePricingType.IssueBase:

                    switch (partyType)
                    {
                        case CharteringPartyType.ShipOwner:

                            var charterInOffhire = this.GetRelevantCharterInOffhire(introducer, vesselInCompany, startDateTime);

                            if (charterInOffhire == null)
                                throw new ObjectNotFound("RelevantCharterInOffhire");

                            result.AddRange(charterInOffhire.OffhireDetails.Select(relOD => new PricingValue()
                                                                {
                                                                    Currency = relOD.Offhire.VoucherCurrency,
                                                                    Fee = relOD.FeeInVoucherCurrency,
                                                                    Good = relOD.Good
                                                                }));

                            break;

                        case CharteringPartyType.Charterer:
                            var voyage = voyageDomainService.GetVoyage(introducer, vesselInCompany, startDateTime);

                            var issueBasePricingValues = this.GetIssueBasedPricingValues(voyage);
                            if (issueBasePricingValues == null || issueBasePricingValues.Count == 0)
                                throw new ObjectNotFound("IssueBased Pricing for Vessel not found.");

                            result.AddRange(issueBasePricingValues);
                            break;

                        default:
                            throw new InvalidArgument("PartyType");
                    }

                    break;

                default:
                    throw new InvalidArgument("PricingReferenceType");
            }

            if (result.Any(pricing => pricing.Fee.HasValue && pricing.Currency == null))
                throw new InvalidArgument("Pricing Value has Fee but no currency.");

            return result;
        }

        //================================================================================

        public List<PricingValue> GetPricingValues(Company introducer, VesselInCompany vesselInCompany, DateTime startDateTime, Currency currency, DateTime currencyDateTime)
        {
            var pricingValues = GetPricingValues(introducer, vesselInCompany, startDateTime);

            var result = new List<PricingValue>();

            foreach (var pricingValue in pricingValues)
            {
                decimal? feeInVoucherCurrency = null;

                if (pricingValue.Fee.HasValue && pricingValue.Currency != null)
                    feeInVoucherCurrency = currencyDomainService.ConvertPrice(pricingValue.Fee.Value, pricingValue.Currency, currency, currencyDateTime);

                result.Add(new PricingValue()
                {
                    Currency = currency,
                    Good = pricingValue.Good,
                    Fee = feeInVoucherCurrency
                });
            }

            return result;
        }

        //================================================================================

        public List<PricingValue> GetIssueBasedPricingValues(Voyage voyage)
        {
            if (voyage == null)
                throw new BusinessRuleException("", "No Voyage is selected for vessel to fetch Consumption Issue Prices.");

            var pricedIssueResult = this.GetVoyagePricedConsumptionIssue(voyage.CompanyId, voyage.Id);

            return pricedIssueResult.InventoryResultItems.Select(invi => new PricingValue() { Good = invi.Good, Currency = invi.Currency, Fee = invi.Fee }).ToList();
        }

        //================================================================================

        public Charter GetOffhireCharterContract(Offhire offhire)
        {
            return GetCharterContract(offhire.Introducer, offhire.VesselInCompany, offhire.IntroducerType, offhire.StartDateTime);
        }

        //================================================================================

        public CharteringPartyType GetCharteringPartyType(VesselInCompany vesselInCompany)
        {
            //var vesselState = vesselDomainService.GetVesselCurrentState(vesselInCompany.Id);
            var vesselState = vesselInCompany.VesselStateCode;

            switch (vesselState)
            {
                case VesselStates.CharterIn:

                    return CharteringPartyType.Charterer;

                case VesselStates.CharterOut:

                    if (vesselInCompany.CompanyId == vesselInCompany.Vessel.OwnerId)
                        return CharteringPartyType.ShipOwner;
                    else
                        return CharteringPartyType.Charterer;

                default:
                    throw new BusinessRuleException("", "Invalid Vessel Status.");
            }
        }

        //================================================================================

        public OffHirePricingType GetPricingType(Company company, VesselInCompany vesselInCompany, DateTime date)
        {
            var partyType = this.GetCharteringPartyType(vesselInCompany);

            var charterContract = this.GetCharterContract(company, vesselInCompany, partyType, date);

            switch (partyType)
            {
                case CharteringPartyType.ShipOwner:

                    //If the given company is in the role of ShipOwner, it will be inferred that 
                    //the pricing type must be fetched from Charter-Out contract.

                    return ((CharterOut)charterContract).OffHirePricingType;

                case CharteringPartyType.Charterer:

                    //If the given company is in the role of Charterer, it will be inferred that 
                    //the pricing type must be fetched from Charter-In contract.

                    return ((CharterIn)charterContract).OffHirePricingType;

                default:
                    throw new ArgumentOutOfRangeException("PartyType");
            }
        }

        //================================================================================

        public bool IsOffhireRegisteredForVessel(long vesselInCompanyId, DateTime startDateTime, DateTime endDateTime)
        {
            return offhireRepository.Count(
                this.isOffhireCancelled.Not().Predicate.And(
                    o => o.VesselInCompany.Id == vesselInCompanyId &&
                        o.StartDateTime >= startDateTime &&
                        o.EndDateTime <= endDateTime)) != 0;
        }

        //================================================================================
    }
}