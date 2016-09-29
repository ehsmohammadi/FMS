using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OffhireStates;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.FinanceOperations;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Offhire
    {
        private readonly IsOffhireCancelled isCancelled;
        private readonly IsOffhireClosed isClosed;
        private readonly IsOffhireOpen isOpen;
        private readonly IsOffhireSubmitRejected isSubmitRejected;
        private readonly IsOffhireSubmitted isSubmitted;

        public long Id { get; private set; }

        public long ReferenceNumber { get; private set; }

        public DateTime StartDateTime { get; private set; }

        public DateTime EndDateTime { get; private set; }

        public CharteringPartyType IntroducerType { get; private set; }

        public long IntroducerId { get; private set; }
        public virtual Company Introducer { get; private set; }

        public long VesselInCompanyId { get; private set; }
        public virtual VesselInCompany VesselInCompany { get; private set; }

        public long? VoyageId { get; private set; }
        public virtual Voyage Voyage { get; private set; }

        public long OffhireLocationId { get; private set; }
        public virtual ActivityLocation OffhireLocation { get; private set; }

        public DateTime VoucherDate { get; private set; }

        public long VoucherCurrencyId { get; private set; }
        public virtual Currency VoucherCurrency { get; private set; }

        public OffhirePricingReference PricingReference { get; private set; }

        public virtual List<OffhireDetail> OffhireDetails { get; private set; }


        public States State { get; private set; }

        public OffhireState EntityState { get; private set; }

        public virtual List<OffhireWorkflowLog> ApproveWorkflows { get; private set; }

        public byte[] TimeStamp { get; set; }

        //================================================================================

        private Offhire(
            long referenceNumber,
            DateTime startDateTime,
            DateTime endDateTime,
            CharteringPartyType introducerType,
            Company introducer,
            VesselInCompany vesselInCompany,
            Voyage voyage,
            ActivityLocation offhireLocation,
            DateTime voucherDate,
            Currency voucherCurrency,
            string pricingReferenceNumber,
            OffHirePricingType pricingReferenceType,
            List<OffhireDetail> offhireDetails)
        {
            //This is used to create instances by reflection to Insert Seed Data into DB.
            this.ReferenceNumber = referenceNumber;
            this.StartDateTime = startDateTime;
            this.EndDateTime = endDateTime;
            this.IntroducerType = introducerType;
            this.Introducer = introducer;
            this.VesselInCompany = vesselInCompany;
            this.Voyage = voyage;
            this.OffhireLocation = offhireLocation;
            this.VoucherDate = voucherDate;
            this.VoucherCurrencyId = voucherCurrency.Id;
            this.VoucherCurrency = voucherCurrency;
            this.PricingReference = new OffhirePricingReference() { Type = pricingReferenceType, Number = pricingReferenceNumber };


            this.OffhireDetails = offhireDetails;
            this.ApproveWorkflows = new List<OffhireWorkflowLog>();
        }

        public Offhire()
        {
            this.isSubmitted = new IsOffhireSubmitted();
            this.isClosed = new IsOffhireClosed();
            this.isOpen = new IsOffhireOpen();
            this.isSubmitRejected = new IsOffhireSubmitRejected();
            this.isCancelled = new IsOffhireCancelled();

            this.Id = -1;

            this.OffhireDetails = new List<OffhireDetail>();
            this.ApproveWorkflows = new List<OffhireWorkflowLog>();
        }

        internal Offhire(
            long referenceNumber,
            DateTime startDateTime,
            DateTime endDateTime,
            Company introducer,
            VesselInCompany vesselInCompany,
            Voyage voyage,
            ActivityLocation offhireLocation,
            DateTime voucherDate,
            Currency voucherCurrency,
            //string pricingReferenceNumber,
            //OffHirePricingType pricingReferenceType,
            IOffhireDomainService offhireDomainService,
            IOffhireManagementSystemDomainService offhireManagementSystemDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            IVoyageDomainService voyageDomainService,
            ICompanyDomainService companyDomainService,
            IActivityLocationDomainService activityLocationDomainService,
            ICurrencyDomainService currencyDomainService)
            : this()
        {
            //var pricingReference = new OffhirePricingReference() { Type = pricingReferenceType, Number = pricingReferenceNumber };
            this.validateCreation(
                referenceNumber,
                startDateTime,
                endDateTime,
                introducer,
                vesselInCompany,
                voyage,
                offhireLocation,
                voucherDate,
                voucherCurrency,
                //pricingReference,
                offhireDomainService,
                offhireManagementSystemDomainService,
                vesselDomainService,
                voyageDomainService,
                companyDomainService,
                activityLocationDomainService,
                currencyDomainService);


            this.ReferenceNumber = referenceNumber;
            this.StartDateTime = startDateTime;
            this.EndDateTime = endDateTime;
            this.IntroducerType = offhireDomainService.GetCharteringPartyType(vesselInCompany);
            this.Introducer = introducer;
            this.VesselInCompany = vesselInCompany;
            this.Voyage = voyage;
            this.OffhireLocation = offhireLocation;
            this.VoucherDate = voucherDate;
            this.VoucherCurrencyId = voucherCurrency.Id;
            this.VoucherCurrency = voucherCurrency;

            this.PricingReference = this.createPricingReference(introducer, vesselInCompany, this.IntroducerType, voyage, startDateTime, offhireDomainService);
        }

        //================================================================================

        private void validateCreation(
            long referenceNumber,
            DateTime startDateTime,
            DateTime endDateTime,
            Company introducer,
            VesselInCompany vesselInCompany,
            Voyage voyage,
            ActivityLocation offhireLocation,
            DateTime voucherDate,
            Currency voucherCurrency,
            //OffhirePricingReference pricingReference,
            IOffhireDomainService offhireDomainService,
            IOffhireManagementSystemDomainService offhireManagementSystemDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            IVoyageDomainService voyageDomainService,
            ICompanyDomainService companyDomainService,
            IActivityLocationDomainService activityLocationDomainService,
            ICurrencyDomainService currencyDomainService)
        {
            this.validateCompanyExistance(introducer, companyDomainService);

            this.validateOffhireReference(referenceNumber, introducer, offhireManagementSystemDomainService);

            this.validateOffhireUniquenessInFuelManagementDomain(referenceNumber, introducer, offhireDomainService);

            this.validateCurrency(voucherCurrency, currencyDomainService);
            this.validateVoucherDate(voucherDate);

            this.validateVesselExistance(vesselInCompany, vesselDomainService);
            this.validateVesselInCompanyExistance(vesselInCompany, introducer, companyDomainService);


            this.validateVoyageExistance(voyage, voyageDomainService);
            this.validateVoyageInVesselOfCompanyExistance(voyage, vesselInCompany);
            this.validateOffhireDurationInVoyage(startDateTime, endDateTime, voyage);


            this.validateActivityLocation(offhireLocation, activityLocationDomainService);

            CharteringPartyType charteringPartyType = offhireDomainService.GetCharteringPartyType(vesselInCompany);

            var offhirePricingType = offhireDomainService.GetPricingType(introducer, vesselInCompany, startDateTime);

            this.validateOffhirePricingType(introducer, vesselInCompany, voyage, startDateTime, charteringPartyType, offhirePricingType,
                offhireDomainService);
        }

        //================================================================================

        private void validateVoucherDate(DateTime voucherDate)
        {
            if (voucherDate == DateTime.MinValue || voucherDate == DateTime.MaxValue)
                throw new InvalidArgument("VoucherDate");
        }

        //================================================================================

        private void validateOffhirePricingType(
            Company introducer, VesselInCompany vesselInCompany, Voyage voyage, DateTime satrtDate, CharteringPartyType partyType,
            OffHirePricingType offhirePricingType, IOffhireDomainService offhireDomainService)
        {
            switch (partyType)
            {
                case CharteringPartyType.Charterer:

                    this.validateOffhirePricingTypeForCharteredInVessel(
                        voyage, offhirePricingType, offhireDomainService);

                    break;

                case CharteringPartyType.ShipOwner:

                    this.validateOffhirePricingTypeForCharteredOutVessel(
                        introducer, vesselInCompany, voyage, satrtDate, offhirePricingType, offhireDomainService);

                    break;
            }
        }

        //================================================================================

        private void validateOffhirePricingTypeForCharteredInVessel(
            Voyage voyage, OffHirePricingType offhirePricingType, IOffhireDomainService offhireDomainService)
        {
            switch (offhirePricingType)
            {
                case OffHirePricingType.CharterPartyBase:
                    break;
                case OffHirePricingType.IssueBase:
                    this.validateVoyagePricedIssueExistanceForCharterInIssueBasedPricingVessel(
                        voyage, offhireDomainService);
                    break;
                default:
                    throw new InvalidArgument("OffhirePricingTypeForCharteredIn");
            }
        }

        //================================================================================

        private void validateOffhirePricingTypeForCharteredOutVessel(
            Company introducer, VesselInCompany vesselInCompany, Voyage voyage, DateTime satrtDate, OffHirePricingType offhirePricingType,
            IOffhireDomainService offhireDomainService)
        {
            switch (offhirePricingType)
            {
                case OffHirePricingType.CharterPartyBase:
                    break;
                case OffHirePricingType.IssueBase:
                    this.validateRelevantCharterInOffhireExistanceInChartererCompanyForCharterOutVessel(
                        introducer, vesselInCompany, satrtDate, offhireDomainService);
                    break;
                default:
                    throw new InvalidArgument("OffhirePricingTypeForCharteredOut");
            }
        }

        //================================================================================

        private void validateRelevantCharterInOffhireExistanceInChartererCompanyForCharterOutVessel(
            Company shipOwner, VesselInCompany vesselInCompany, DateTime date,
            IOffhireDomainService offhireDomainService)
        {
            try
            {
                offhireDomainService.GetRelevantCharterInOffhire(shipOwner, vesselInCompany, date);
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException("", "Relevant CharterIn Offhire record for Charter Out Vessel not found.");
            }
        }

        //================================================================================

        private void validateVoyagePricedIssueExistanceForCharterInIssueBasedPricingVessel(
            Voyage voyage, IOffhireDomainService offhireDomainService)
        {
            var pricedIssueResult = offhireDomainService.GetVoyagePricedConsumptionIssue(voyage.CompanyId, voyage.Id);

            if (pricedIssueResult == null || pricedIssueResult.InventoryResultItems.Count == 0)
                throw new BusinessRuleException("", "The Consumption Issue is not priced yet in Inventory.");
        }

        //================================================================================

        private void validateVesselState(VesselStates vesselState)
        {
            if (vesselState != VesselStates.CharterIn && vesselState != VesselStates.CharterOut)
                throw new BusinessRuleException("", "Vessel is not in valid state.");
        }

        //================================================================================

        private void validateActivityLocation(ActivityLocation offhireLocation, IActivityLocationDomainService activityLocationDomainService)
        {
            if (offhireLocation == null)
                throw new BusinessRuleException("", "Offhire Location is empty.");

            if (activityLocationDomainService.Get(offhireLocation.Id) == null)
                throw new ObjectNotFound("OffhireLocation");
        }

        //================================================================================

        private void validateVoyageInVesselOfCompanyExistance(Voyage voyage, VesselInCompany vesselInCompany)
        {
            if (voyage.VesselInCompany == null || voyage.VesselInCompany.Id != vesselInCompany.Id)
                throw new BusinessRuleException("", "Voyage is not for selected Vessel.");
        }

        //================================================================================

        private void validateVoyageExistance(Voyage voyage, IVoyageDomainService voyageDomainService)
        {
            if (voyage == null || voyageDomainService.Get(voyage.Id) == null)
                throw new BusinessRuleException("", "The voyage not found.");
        }

        //================================================================================

        private void validateOffhireDurationInVoyage(DateTime startDateTime, DateTime endDateTime, Voyage voyage)
        {
            if (!(startDateTime <= endDateTime && startDateTime >= voyage.StartDate && endDateTime <= voyage.EndDate))
                throw new BusinessRuleException("", "The offhire period is not inside the voyage duration.");
        }

        //================================================================================

        private void validateVesselInCompanyExistance(DomainObjects.VesselInCompany vesselInCompany, Company introducer, ICompanyDomainService companyDomainService)
        {
            var vessels = companyDomainService.GetCompanyVessels(introducer.Id);

            if (vessels.Count(v => v.Id == vesselInCompany.Id) != 1)
                throw new BusinessRuleException("", "The vessel is not assigned to company.");
        }

        //================================================================================

        public void Configure(IOffhireStateFactory offhireStateFactory)
        {
            this.EntityState = offhireStateFactory.CreateState(this.State);
        }

        //================================================================================

        public OffhireDetail AddDetail(OffhireDetail offhireDetail)
        {
            this.validateNotToBeSubmitted();

            this.validateNotToBeCancelled();

            this.OffhireDetails.Add(offhireDetail);

            return offhireDetail;
        }

        //================================================================================

        public void Update(
            //string referenceNumber,
            //DateTime startDateTime,
            //DateTime endDateTime,
            //OffhireCharteringPartyType partyType,
            //Company introducer,
            //Vessel vessel,
            //Voyage voyage,
            //ActivityLocation offhireLocation,
            DateTime voucherDate,
            Currency voucherCurrency,
            //string pricingReferenceNumber,
            //OffHirePricingType pricingReferenceType,
            //IOffhireDomainService offhireDomainService,
            //IOffhireManagementSystemDomainService offhireManagementSystemDomainService,
            //IVesselDomainService vesselDomainService,
            //IVoyageDomainService voyageDomainService,
            //ICompanyDomainService companyDomainService,
            //IActivityLocationDomainService activityLocationDomainService,
            ICurrencyDomainService currencyDomainService)
        {
            validateUpdate(voucherCurrency, currencyDomainService);

            this.VoucherDate = voucherDate;

            this.VoucherCurrencyId = voucherCurrency.Id;
            this.VoucherCurrency = voucherCurrency;
        }

        //================================================================================

        private void validateUpdate(
            Currency voucherCurrency,
            ICurrencyDomainService currencyDomainService)
        {
            this.validateToBeOpenOrSubmitRejected();

            this.validateCurrency(voucherCurrency, currencyDomainService);
        }

        //================================================================================

        public void Delete(IOffhireDomainService offhireDomainService)
        {
            while (OffhireDetails.Count > 0)
            {

                this.DeleteDetail(OffhireDetails[0].Id, offhireDomainService);
            }

            this.validateDeletion();

            offhireDomainService.Delete(this);
        }

        //================================================================================

        private void validateDeletion()
        {
            this.validateToBeOpenOrSubmitRejected();

            this.validateNotToHaveDetails();
        }

        //================================================================================

        private void validateNotToHaveDetails()
        {
            if (this.OffhireDetails.Count != 0)
                throw new BusinessRuleException("", "The Offhire has some Details attached to.");
        }

        //================================================================================

        public OffhireDetail UpdateDetail(
            long offhireDetailId, decimal feeInVoucherCurrency, decimal feeInMainCurrency,
            IOffhireDomainService offhireDomainService, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            OffhireDetail offhireDetailToUpdate = this.findDetail(offhireDetailId);

            offhireDetailToUpdate.Update(feeInVoucherCurrency, feeInMainCurrency, offhireDomainService,
                                         tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);

            return offhireDetailToUpdate;
        }

        //================================================================================

        private void DeleteDetail(long offhireDetailId, IOffhireDomainService offhireDomainService)
        {
            this.validateDetailDeletion();

            var offhireDetailToDelete = this.findDetail(offhireDetailId);

            offhireDomainService.DeleteOffhireDetail(offhireDetailToDelete);
        }

        //================================================================================

        private void validateDetailDeletion()
        {
            this.validateToBeOpenOrSubmitRejected();
        }

        //================================================================================

        private OffhireDetail findDetail(long offhireDetailId)
        {
            OffhireDetail detail = this.OffhireDetails.SingleOrDefault(d => d.Id == offhireDetailId);

            if (detail == null)
            {
                throw new ObjectNotFound("OffhireDetail", offhireDetailId);
            }

            return detail;
        }

        //================================================================================

        private void validateNotToBeSubmitted()
        {
            if (this.isSubmitted.IsSatisfiedBy(this))
            {
                throw new BusinessRuleException("", "The Offhire is in Submitted state.");
            }
        }

        //================================================================================

        private void validateToBeSubmitted()
        {
            if (!this.isSubmitted.IsSatisfiedBy(this))
            {
                throw new BusinessRuleException("", "The Offhire is not in Submitted state.");
            }
        }

        //================================================================================

        //private void validateNotToBeSubmitRejected()
        //{
        //    if (this.isSubmitRejected.IsSatisfiedBy(this))
        //    {
        //        throw new BusinessRuleException("", "The Offhire is in SubmitRejected state.");
        //    }
        //}

        ////================================================================================
        //private void validateNotToBeClosed()
        //{
        //    if (this.isClosed.IsSatisfiedBy(this))
        //    {
        //        throw new BusinessRuleException("", "The Offhire is in Closed state.");
        //    }
        //}

        //================================================================================

        private void validateToBeOpen()
        {
            if (!this.isOpen.IsSatisfiedBy(this))
            {
                throw new BusinessRuleException("", "The Offhire is not in Open state.");
            }
        }

        //================================================================================

        private void validateNotToBeCancelled()
        {
            if (this.isCancelled.IsSatisfiedBy(this))
            {
                throw new BusinessRuleException("", "The Offhire is cancelled.");
            }
        }

        //================================================================================

        private void validateCompanyExistance(Company introducer, ICompanyDomainService companyDomainService)
        {
            if (introducer == null)
                throw new BusinessRuleException("", "The Offhire Registrar Company must be selected.");

            if (companyDomainService.Get(introducer.Id) == null)
            {
                throw new ObjectNotFound("Company Not Found");
            }
        }

        //================================================================================

        private void validateCurrency(Currency voucherCurrency, ICurrencyDomainService currencyDomainService)
        {
            if (voucherCurrency == null)
                throw new BusinessRuleException("", "VoucherCurrency must be selected.");

            if (currencyDomainService.Get(voucherCurrency.Id) == null)
                throw new ObjectNotFound("VoucherCurrency", voucherCurrency.Id);
        }

        //================================================================================

        private void validateVesselExistance(VesselInCompany vesselInCompany, IVesselInCompanyDomainService vesselDomainService)
        {
            if (vesselInCompany == null)
                throw new BusinessRuleException("", "Vessel must be selected.");

            if (vesselDomainService.Get(vesselInCompany.Id) == null)
            {
                throw new ObjectNotFound("Vessel Not Found");
            }
        }

        //================================================================================

        //private void validateVesselCharteringTypeInCompany(Vessel vessel, CharteringPartyType partyType, IVesselDomainService vesselDomainService)
        //{
        //    VesselStates vesselState = vesselDomainService.GetVesselCurrentState(vessel.Id);

        //    if (partyType == CharteringPartyType.Charterer && vesselState != VesselStates.CharterIn)
        //        throw new BusinessRuleException("", "Vessel is not in Charter-In state for the company.");

        //    if (partyType == CharteringPartyType.ShipOwner && vesselState != VesselStates.CharterOut)
        //        throw new BusinessRuleException("", "Vessel is not in Charter-Out state for the company.");
        //}

        //================================================================================

        private void validateOffhireReference(long referenceNumber, Company introducer, IOffhireManagementSystemDomainService offhireManagementSystemDomainService)
        {
            var finalizedOffhire = offhireManagementSystemDomainService.GetFinalizedOffhire(referenceNumber, introducer.Id);

            if (finalizedOffhire == null)
                throw new BusinessRuleException("", "The selected Offhire is not registered.");

            if (finalizedOffhire.HasVoucher)
                throw new BusinessRuleException("", "Offhire is already registered with voucher.");
        }

        //================================================================================

        private void validateOffhireUniquenessInFuelManagementDomain(long referenceNumber, Company introducer, IOffhireDomainService offhireDomainService)
        {
            if (offhireDomainService.IsOffhireRegistered(referenceNumber, introducer))
                throw new BusinessRuleException("", "The selected Offhire is already registered.");
        }

        //================================================================================

        private void validateDetailsExistance()
        {
            if (this.OffhireDetails.Count == 0)
            {
                throw new BusinessRuleException("", "No Details is defined for the Offhire.");
            }
        }

        //================================================================================

        private void validateDetails(IOffhireDomainService offhireDomainService,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.OffhireDetails.ForEach(sd => sd.Validate(offhireDomainService, tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService));
        }

        //================================================================================

        public void ValidateMiddleApprove(IOffhireDomainService offhireDomainService,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateToBeOpen();

            this.validateApproveAction(offhireDomainService, tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);
        }

        //================================================================================

        private void validateApproveAction(
            IOffhireDomainService offhireDomainService,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateDetailsExistance();

            this.validateDetails(offhireDomainService, tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);
        }

        //================================================================================

        public void Cancel(CancelledState cancelledState, IFinanceNotifier eventNotifier, long approverId)
        {
            this.setState(cancelledState);

            eventNotifier.NotifyOffhireCancelled(this, approverId);
        }

        //================================================================================

        public void RejectSubmittedState(SubmitRejectedState submitRejectedState)
        {
            this.validateToBeSubmitted();

            this.setState(submitRejectedState);
        }

        //================================================================================

        public void Close(ClosedState closedState)
        {
            this.validateToBeSubmitted();

            this.setState(closedState);
        }

        //================================================================================

        private void validateToBeOpenOrSubmitRejected()
        {
            if (!(this.isOpen.IsSatisfiedBy(this) || this.isSubmitRejected.IsSatisfiedBy(this)))
            {
                throw new BusinessRuleException("", "The Offhire is not in Open or Submit-Rejectted state.");
            }
        }

        //================================================================================

        public void Submit(SubmittedState submittedState, IOffhireDomainService offhireDomainService, IFinanceNotifier eventNotifier, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService, IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService, long approverId)
        {
            this.validateToBeOpenOrSubmitRejected();

            this.validateApproveAction(offhireDomainService, tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);


            switch (this.VesselInCompany.VesselStateCode)
            {
                case VesselStates.CharterIn:
                    eventNotifier.NotifySubmittingOffhire(this, approverId, VoucherDetailType.OffhireCharterIn);
                    break;
                case VesselStates.CharterOut:
                    eventNotifier.NotifySubmittingOffhire(this, approverId, VoucherDetailType.OffhireCharterOut);
                    break;
            }

            this.setState(submittedState);
        }

        //================================================================================

        private void setState(OffhireState offhireState)
        {
            this.EntityState = offhireState;
            this.State = offhireState.State;
        }

        //================================================================================

        private OffhirePricingReference createPricingReference(Company introducer, VesselInCompany vesselInCompany, CharteringPartyType partyType, Voyage voyage, DateTime startDateTime, IOffhireDomainService offhireDomainService)
        {
            var pricingType = offhireDomainService.GetPricingType(introducer, vesselInCompany, startDateTime);

            switch (pricingType)
            {
                case OffHirePricingType.CharterPartyBase:

                    var charteringReferenceNumber = offhireDomainService.GetCharteringReferenceNumber(introducer, vesselInCompany, partyType, startDateTime);

                    return new OffhirePricingReference() { Number = charteringReferenceNumber, Type = OffHirePricingType.CharterPartyBase };

                case OffHirePricingType.IssueBase:

                    var issueReferenceNumber = offhireDomainService.GetVoyageConsumptionIssueOperation(voyage.Id).ActionNumber;

                    return new OffhirePricingReference() { Number = issueReferenceNumber, Type = OffHirePricingType.IssueBase };

                default:
                    throw new InvalidArgument("Pricing Type could not be retrieved.", "PricingType");
            }
        }

        //================================================================================

        //internal List<PricingValue> GetCharterPartyBasedPricingValues(IOffhireDomainService offhireDomainService)
        //{
        //    return offhireDomainService.GetCharterPartyBasedPricingValues(this);
        //}

        //================================================================================

        //internal List<PricingValue> GetIssueBasedPricingValues(IOffhireDomainService offhireDomainService)
        //{
        //    if (this.Voyage == null)
        //        throw new BusinessRuleException("", "No Voyage is selected for vessel to fetch Consumption Issue Prices.");

        //    return offhireDomainService.GetIssueBasedPricingValues(this.Voyage);

        //}

        //================================================================================
    }

    public class OffhirePricingReference
    {
        public string Number { get; set; }
        public OffHirePricingType @Type { get; set; }
    }
}
