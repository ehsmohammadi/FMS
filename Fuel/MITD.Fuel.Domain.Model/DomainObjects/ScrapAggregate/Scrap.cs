using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.ScrapStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Scrap
    {
        private readonly IsScrapClosed isClosed;
        private readonly IsScrapOpen isOpen;
        private readonly IsScrapCancelled isCancelled;
        private readonly IsScrapSubmitRejected isSubmitRejected;
        private readonly IsScrapSubmitted isSubmitted;

        public long Id { get; private set; }

        public DateTime ScrapDate { get; private set; }

        public long VesselInCompanyId { get; private set; }
        public virtual VesselInCompany VesselInCompany { get; private set; }

        public long SecondPartyId { get; private set; }
        public virtual Company SecondParty { get; private set; }

        public virtual List<ScrapDetail> ScrapDetails { get; private set; }

        public States State { get; private set; }

        public ScrapState EntityState { get; private set; }

        public virtual List<ScrapWorkflowLog> ApproveWorkflows { get; private set; }

        public virtual List<InventoryOperation> InventoryOperations { get; private set; }

        public byte[] TimeStamp { get; set; }

        //================================================================================

        private Scrap(
            VesselInCompany vesselInCompany,
            Company secondParty,
            DateTime scrapDate)
        {
            //This is used to create instances by reflection to Insert Seed Data into DB.
            this.VesselInCompany = vesselInCompany;
            this.SecondParty = secondParty;
            this.ScrapDate = scrapDate;
            this.ApproveWorkflows = new List<ScrapWorkflowLog>();
            this.ScrapDetails = new List<ScrapDetail>();

            this.InventoryOperations = new List<InventoryOperation>();
        }

        public Scrap()
        {
            this.isSubmitted = new IsScrapSubmitted();
            this.isClosed = new IsScrapClosed();
            this.isOpen = new IsScrapOpen();
            this.isSubmitRejected = new IsScrapSubmitRejected();
            this.isCancelled = new IsScrapCancelled();

            this.InventoryOperations = new List<InventoryOperation>();

            this.Id = -1;
        }

        internal Scrap(
            VesselInCompany vesselInCompany,
            Company secondParty,
            DateTime scrapDate,
            IScrapDomainService scrapDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            ICompanyDomainService companyDomainService)
            : this()
        {
            this.validateCreation(vesselInCompany, secondParty, scrapDate, scrapDomainService, vesselDomainService, companyDomainService);

            this.ScrapDate = scrapDate;
            this.VesselInCompany = vesselInCompany;
            this.SecondParty = secondParty;

            this.ScrapDetails = new List<ScrapDetail>();
            this.ApproveWorkflows = new List<ScrapWorkflowLog>();
        }

        //================================================================================

        private void validateCreation(
            VesselInCompany vesselInCompany,
            Company secondParty,
            DateTime scrapDate,
            IScrapDomainService scrapDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            ICompanyDomainService companyDomainService)
        {
            this.validateCompanyExistance(secondParty, companyDomainService);

            this.validateVesselExistance(vesselInCompany, vesselDomainService);
            this.validateVesselOwnedState(vesselInCompany, vesselDomainService);

            this.validateScrapDateToBeAfterTheLastVesselActivity(scrapDate, vesselInCompany, vesselDomainService);

            this.validateToBeTheOnlyScrapForVessel(vesselInCompany, scrapDomainService);
        }

        //================================================================================

        public void Configure(IScrapStateFactory scrapStateFactory)
        {
            this.EntityState = scrapStateFactory.CreateState(this.State);
        }

        //================================================================================

        public ScrapDetail AddDetail(ScrapDetail scrapDetail)
        {
            validateNotToBeSubmitted();

            validateNotToBeCancelled();

            this.ScrapDetails.Add(scrapDetail);

            return scrapDetail;
        }

        //================================================================================

        public void Update(
            VesselInCompany vesselInCompany, Company secondParty, DateTime scrapDate,
            IScrapDomainService scrapDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            ICompanyDomainService companyDomainService)
        {
            this.validateUpdate(vesselInCompany, secondParty, scrapDate, scrapDomainService, vesselDomainService, companyDomainService);

            this.VesselInCompany = vesselInCompany;
            this.SecondParty = secondParty;
            this.ScrapDate = scrapDate;
        }

        //================================================================================

        private void validateUpdate(
            VesselInCompany vesselInCompany, Company secondParty, DateTime scrapDate,
            IScrapDomainService scrapDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            ICompanyDomainService companyDomainService)
        {
            this.validateToBeOpen();

            this.validateCompanyExistance(secondParty, companyDomainService);

            this.validateVesselExistance(vesselInCompany, vesselDomainService);
            this.validateVesselOwnedState(vesselInCompany, vesselDomainService);

            this.validateScrapDateToBeAfterTheLastVesselActivity(scrapDate, vesselInCompany, vesselDomainService);

            this.validateToBeTheOnlyScrapForVessel(vesselInCompany, scrapDomainService);
        }

        //================================================================================

        public void Delete(IScrapDomainService scrapDomainService)
        {
            this.validateDeletion();

            scrapDomainService.Delete(this);
        }

        //================================================================================

        private void validateDeletion()
        {
            this.validateToBeOpen();

            this.validateNotToHaveDetails();
        }

        //================================================================================

        private void validateNotToHaveDetails()
        {
            if (this.ScrapDetails.Count != 0)
            {
                throw new BusinessRuleException("", "The Scrap has some Details attached to.");
            }
        }

        //================================================================================

        public ScrapDetail UpdateDetail(long scrapDetailId, double rob, double price, Currency currency, Good good, GoodUnit unit, Tank tank,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            var scrapDetailToUpdate = this.findDetail(scrapDetailId);

            scrapDetailToUpdate.Update(rob, price, currency, good, unit, tank,
                 tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);

            return scrapDetailToUpdate;
        }

        //================================================================================

        public void DeleteDetail(long scrapDetailId, IScrapDomainService scrapDomainService)
        {
            this.validateDetailDeletion(scrapDetailId);

            ScrapDetail scrapDetailToDelete = this.findDetail(scrapDetailId);

            scrapDomainService.DeleteScrapDetail(scrapDetailToDelete);
        }

        //================================================================================

        private void validateDetailDeletion(long scrapDetailId)
        {
            this.validateToBeOpen();

            this.findDetail(scrapDetailId);
        }

        //================================================================================

        private ScrapDetail findDetail(long scrapDetailId)
        {
            ScrapDetail detail = this.ScrapDetails.FirstOrDefault(d => d.Id == scrapDetailId);

            if (detail == null)
                throw new ObjectNotFound("ScrapDetail", scrapDetailId);

            return detail;
        }

        //================================================================================

        private void validateNotToBeSubmitted()
        {
            if (this.isSubmitted.IsSatisfiedBy(this))
            {
                throw new BusinessRuleException("", "The Scrap is in Submitted state.");
            }
        }

        //================================================================================

        private void validateToBeSubmitted()
        {
            if (!this.isSubmitted.IsSatisfiedBy(this))
            {
                throw new BusinessRuleException("", "The Scrap is not in Submitted state.");
            }
        }

        //================================================================================

        //private void validateNotToBeSubmitRejected()
        //{
        //    if (this.isSubmitRejected.IsSatisfiedBy(this))
        //    {
        //        throw new BusinessRuleException("", "The Scrap is in SubmitRejected state.");
        //    }
        //}

        ////================================================================================
        //private void validateNotToBeClosed()
        //{
        //    if (this.isClosed.IsSatisfiedBy(this))
        //    {
        //        throw new BusinessRuleException("", "The Scrap is in Closed state.");
        //    }
        //}

        //================================================================================

        private void validateToBeOpen()
        {
            if (!this.isOpen.IsSatisfiedBy(this))
            {
                throw new BusinessRuleException("", "The Scrap is not in Open state.");
            }
        }

        //================================================================================

        private void validateNotToBeCancelled()
        {
            if (this.isCancelled.IsSatisfiedBy(this))
            {
                throw new BusinessRuleException("", "The Scrap is cancelled.");
            }
        }

        //================================================================================

        private void validateCompanyExistance(Company secondParty, ICompanyDomainService companyDomainService)
        {
            Company fetchedCompany = companyDomainService.Get(secondParty.Id);

            if (fetchedCompany == null)
            {
                throw new ObjectNotFound("Company Not Found");
            }
        }

        //================================================================================

        private void validateVesselExistance(VesselInCompany vesselInCompany, IVesselInCompanyDomainService vesselDomainService)
        {
            VesselInCompany fetchedVesselInCompany = vesselDomainService.Get(vesselInCompany.Id);

            if (fetchedVesselInCompany == null)
            {
                throw new ObjectNotFound("Vessel Not Found");
            }
        }

        //================================================================================

        private void validateVesselOwnedState(VesselInCompany vesselInCompany, IVesselInCompanyDomainService vesselDomainService)
        {
            VesselStates vesselState = vesselDomainService.GetVesselCurrentState(vesselInCompany.Id);

            if (this.isOpen.IsSatisfiedBy(this) && vesselState != VesselStates.Owned)
            {
                throw new BusinessRuleException("", "Vessel is not in Owned state.");
            }
        }

        //================================================================================

        private void validateBeingSubmitRejectedIfIsInScrappedState(VesselInCompany vesselInCompany, IVesselInCompanyDomainService vesselDomainService)
        {
            VesselStates vesselState = vesselDomainService.GetVesselCurrentState(vesselInCompany.Id);

            if (vesselState == VesselStates.Scrapped)
                if (!this.isSubmitRejected.IsSatisfiedBy(this))
                {
                    throw new BusinessRuleException("", "The Scrap State is not rejected.");
                }
        }

        //================================================================================

        private void validateScrapDateToBeAfterTheLastVesselActivity(DateTime scrapDate, VesselInCompany vesselInCompany, IVesselInCompanyDomainService vesselDomainService)
        {
            List<VesselStates> vesselStatesLog = vesselDomainService.GetVesselStatesLog(vesselInCompany.Id, scrapDate, null);

            if (!(vesselStatesLog.Count == 1 && vesselStatesLog[0] == VesselStates.Owned))
            {
                throw new BusinessRuleException("", "The Scrap Date is an invalid date for scrapping vessel.");
            }
        }

        //================================================================================

        private void validateToBeTheOnlyScrapForVessel(VesselInCompany vesselInCompany, IScrapDomainService scrapDomainService)
        {
            List<Scrap> notCancelledScraps = scrapDomainService.GetNotCancelledScrapsForVessel(vesselInCompany, this.Id);

            if (notCancelledScraps.Count != 0)
            {
                throw new BusinessRuleException("", "There is already another Scrap defined for the Vessel.");
            }
        }

        //================================================================================

        private void validateDetailsExistance()
        {
            if (this.ScrapDetails.Count == 0)
            {
                throw new BusinessRuleException("", "No Details is defined for the Scrap.");
            }
        }

        //================================================================================

        private void validateDetails(ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.ScrapDetails.ForEach(sd => sd.Validate(tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService));
        }

        //================================================================================

        public void ValidateMiddleApprove(IVesselInCompanyDomainService vesselDomainService,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateToBeOpen();

            this.validateApproveAction(vesselDomainService, tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);
        }

        //================================================================================

        private void validateApproveAction(IVesselInCompanyDomainService vesselDomainService,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.validateVesselOwnedState(this.VesselInCompany, vesselDomainService);

            this.validateBeingSubmitRejectedIfIsInScrappedState(this.VesselInCompany, vesselDomainService);

            this.validateDetailsExistance();

            this.validateDetails(tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);
        }

        //================================================================================

        public void Cancel(CancelledState cancelledState, IInventoryOperationNotifier eventNotifier, long approverId)
        {
            //this.setState(cancelledState);

            //eventNotifier.NotifyScrapCancelled(this);
        }

        //================================================================================

        public void RejectSubmittedState(SubmitRejectedState submitRejectedState, long approverId)
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
                throw new BusinessRuleException("", "The Scrap is not in Open or Submit-Rejected state.");
            }
        }

        //================================================================================

        public void Submit(SubmittedState submittedState, IVesselInCompanyDomainService vesselDomainService, IInventoryOperationNotifier eventNotifier, ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService, IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService, long approverId)
        {
            this.validateToBeOpenOrSubmitRejected();

            this.validateApproveAction(vesselDomainService, tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);

            //Perform Submit operations.
            //foreach (var scrapDetail in this.ScrapDetails)
            //{
            //    scrapDetail.InventoryOperations.AddRange(eventNotifier.NotifySubmittingScrapDetail(scrapDetail));
            //}

            if (this.isOpen.IsSatisfiedBy(this))
                vesselDomainService.ScrapVessel(this.VesselInCompanyId);

            var invResult = eventNotifier.NotifySubmittingScrap(this, approverId);

            this.InventoryOperations.AddRange(invResult);

            this.setState(submittedState);
        }

        //================================================================================

        private void setState(ScrapState scrapState)
        {
            this.EntityState = scrapState;
            this.State = scrapState.State;
        }

        //================================================================================
    }
}
