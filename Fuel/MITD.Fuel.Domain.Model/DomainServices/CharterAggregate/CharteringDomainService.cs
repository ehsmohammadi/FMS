using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainServices.CharterAggregate
{
    public class CharteringDomainService : ICharteringDomainService
    {
        private ICharterInRepository charterInRepository;
        private ICharterOutRepository charterOutRepository;

        public CharteringDomainService(ICharterInRepository charterInRepository, ICharterOutRepository charterOutRepository)
        {
            this.charterInRepository = charterInRepository;
            this.charterOutRepository = charterOutRepository;
        }



        public CharterOut GetCharterOutStart(Company company, VesselInCompany vesselInCompany, DateTime date)
        {
            IListFetchStrategy<Charter> fetchStrategy = new ListFetchStrategy<Charter>()
                .Include(c => c.CharterItems).OrderBy(c => c.ActionDate);

            var foundCharterOut = charterOutRepository
                .Find(cho => cho.CharterType == CharterType.Start && date >= cho.ActionDate &&
                    cho.OwnerId == company.Id && cho.VesselInCompanyId == vesselInCompany.Id, fetchStrategy).LastOrDefault() as CharterOut;

            return foundCharterOut;
        }

        public CharterIn GetCharterInStart(Company company, VesselInCompany vesselInCompany, DateTime date)
        {
            IListFetchStrategy<Charter> fetchStrategy = new ListFetchStrategy<Charter>()
                .Include(c => c.CharterItems).OrderBy(c => c.ActionDate);

            var foundCharterIn = charterInRepository
                .Find(chi => chi.CharterType == CharterType.Start && date >= chi.ActionDate &&
                    chi.ChartererId == company.Id && chi.VesselInCompanyId == vesselInCompany.Id, fetchStrategy).LastOrDefault() as CharterIn;

            return foundCharterIn;
        }

        //Added by Hatefi
        public CharterOut GetLastCharterOutEnd(Company company, VesselInCompany vesselInCompany, DateTime date)
        {
            IListFetchStrategy<Charter> fetchStrategy = new ListFetchStrategy<Charter>()
                .Include(c => c.CharterItems).OrderBy(c => c.ActionDate);

            var foundCharterOut = charterOutRepository
                .Find(cho => cho.CharterType == CharterType.End && date >= cho.ActionDate &&
                    cho.OwnerId == company.Id && cho.VesselInCompanyId == vesselInCompany.Id, fetchStrategy).LastOrDefault() as CharterOut;

            return foundCharterOut;
        }

        public Charter GetVesselActivationCharterContract(VesselInCompany vesselInCompany, DateTime comparingDateTime)
        {
            //Charter activationCharterContract = null;

            //switch (vesselInCompany.VesselStateCode)
            //{
            //    case VesselStates.Inactive:
            //    case VesselStates.CharterOut:
            //    case VesselStates.Scrapped:
            //        throw new BusinessRuleException("", "The vessel is in an incorrect state.");

            //    case VesselStates.CharterIn:

            //        var lastCharterInStart = this.GetCharterInStart(vesselInCompany.Company, vesselInCompany, comparingDateTime);
            //        var lastCharterOutEnd = this.GetLastCharterOutEnd(vesselInCompany.Company, vesselInCompany, comparingDateTime);


            //        if (lastCharterOutEnd == null && lastCharterInStart != null)
            //        {
            //            activationCharterContract = lastCharterInStart;
            //        }
            //        else if (lastCharterOutEnd != null && lastCharterInStart == null)
            //        {
            //            activationCharterContract = lastCharterOutEnd;
            //        }
            //        else if (lastCharterOutEnd != null && lastCharterInStart != null)
            //        {
            //            activationCharterContract = lastCharterOutEnd.ActionDate > lastCharterInStart.ActionDate ? (Charter)lastCharterOutEnd : (Charter)lastCharterInStart;
            //        }
            //        else
            //            throw new BusinessRuleException("", "The proper Charter-In Start record not found.");

            //        break;

            //    case VesselStates.Owned:

            //        var charterOutEnd = this.GetLastCharterOutEnd(vesselInCompany.Company, vesselInCompany, comparingDateTime);

            //        if (charterOutEnd != null)
            //            activationCharterContract = charterOutEnd;

            //        break;

            //    default:
            //        throw new InvalidArgument("VesselStateCode is invalid.", "VesselStateCode");
            //}
            //return activationCharterContract;

            var lastCharterContract = this.GetLastCharterContractForCompany(vesselInCompany.Code, vesselInCompany.CompanyId, comparingDateTime, true);

            if (lastCharterContract == null)  //This only happens for owned period of vessel.
                return null;

            if (lastCharterContract.CurrentState == States.SubmitRejected)
                throw new BusinessRuleException("", "Last Charter contract is reverted.");
            //todo:uncomment this .... realy ??? !!!!! what the faze 
            //if (!((lastCharterContract is CharterIn && lastCharterContract.CharterType == CharterType.Start) ||
            //    (lastCharterContract is CharterOut && lastCharterContract.CharterType == CharterType.End)))
            //    throw new BusinessRuleException("", "The vessel is in an incorrect state.");

            return lastCharterContract;

        }

        //public CharterOut GetLastCharterOutEnd(CharterOut comparingCharterOutStart)
        //{
        //    if (comparingCharterOutStart.CharterType != CharterType.Start)
        //        throw new BusinessRuleException("", "The given Charter Out is not of Start type.");

        //    var ownerId = comparingCharterOutStart.OwnerId.Value;

        //    var charterOutSingleFetchStrategy = new SingleResultFetchStrategy<Charter>().OrderByDescending(c => c.ActionDate);

        //    var lastCharterOutEnd = charterOutRepository.First(c => c.CharterType == CharterType.End && c.ActionDate < comparingCharterOutStart.ActionDate && c.OwnerId == ownerId, charterOutSingleFetchStrategy) as CharterOut;

        //    return lastCharterOutEnd;
        //}

        //<A.H> Hatefi added on 2016-05-29
        public Charter GetLastCharterContract(string vesselCode, DateTime fuelReportDateTime)
        {
            var lastCharterOutStartBeforeGivenDate = charterOutRepository.Find(
                c => c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.ActionDate <= fuelReportDateTime &&
                    c.CharterType == CharterType.Start &&
                    c.CurrentState == States.Submitted).OfType<CharterOut>().OrderBy(c => c.ActionDate).LastOrDefault();

            var lastCharterOutEndBeforeGivenDate = charterOutRepository.Find(
                c => c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.ActionDate <= fuelReportDateTime &&
                    c.CharterType == CharterType.End &&
                    c.CurrentState == States.Submitted).OfType<CharterOut>().OrderBy(c => c.ActionDate).LastOrDefault();

            var lastCharterInStartBeforeGivenDate = charterInRepository.Find(
                c => c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.ActionDate <= fuelReportDateTime &&
                    c.CharterType == CharterType.Start &&
                    c.CurrentState == States.Submitted).OfType<CharterIn>().OrderBy(c => c.ActionDate).LastOrDefault();

            var lastCharterInEndBeforeGivenDate = charterInRepository.Find(
                c => c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.ActionDate <= fuelReportDateTime &&
                    c.CharterType == CharterType.End &&
                    c.CurrentState == States.Submitted).OfType<CharterIn>().OrderBy(c => c.ActionDate).LastOrDefault();

            Charter lastCharterEnd = null;
            Charter lastCharterStart = null;

            if (lastCharterOutEndBeforeGivenDate != null)
            {
                if (lastCharterInEndBeforeGivenDate != null && lastCharterInEndBeforeGivenDate.ActionDate > lastCharterOutEndBeforeGivenDate.ActionDate)
                {
                    lastCharterEnd = null;

                    lastCharterStart = charterOutRepository.Find(c => c.VesselInCompany.Vessel.Code == vesselCode &&
                            c.OwnerId == lastCharterInEndBeforeGivenDate.OwnerId &&
                            c.CharterType == CharterType.Start).OrderBy(c => c.ActionDate).LastOrDefault();
                }
                else
                    lastCharterEnd = lastCharterOutEndBeforeGivenDate;
            }

            if (lastCharterInStartBeforeGivenDate != null)
                lastCharterStart = lastCharterInStartBeforeGivenDate;

            if (lastCharterOutStartBeforeGivenDate != null && (lastCharterInStartBeforeGivenDate == null || lastCharterOutStartBeforeGivenDate.ActionDate > lastCharterInStartBeforeGivenDate.ActionDate))
                lastCharterStart = lastCharterOutStartBeforeGivenDate;

            if (lastCharterEnd != null)
            {
                if (lastCharterStart != null && lastCharterStart.ActionDate > lastCharterEnd.ActionDate)
                    return lastCharterStart;
                else
                    return lastCharterEnd;
            }

            if (lastCharterStart != null)
                return lastCharterStart;
            else
                return null;

            if (lastCharterOutStartBeforeGivenDate == null && lastCharterOutEndBeforeGivenDate == null)
                return null;

            if (lastCharterOutStartBeforeGivenDate != null && lastCharterOutEndBeforeGivenDate == null)
                return lastCharterOutStartBeforeGivenDate;

            if (lastCharterOutStartBeforeGivenDate != null && lastCharterOutEndBeforeGivenDate != null)
                return null;

            if (lastCharterOutStartBeforeGivenDate == null && lastCharterOutEndBeforeGivenDate != null)
                throw new BusinessRuleException("", "Invalid vessel state for " + vesselCode);
        }

        public Charter GetLastCharterContractForCompany(string vesselCode, long companyId, DateTime comparingDateTime, bool includeSubmitRejected = false)
        {
            var lastCharterOutStartBeforeGivenDate = charterOutRepository.Find(
                c => c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.OwnerId == companyId &&
                    c.ActionDate <= comparingDateTime &&
                    c.CharterType == CharterType.Start &&
                    (c.CurrentState == States.Submitted || (includeSubmitRejected && c.CurrentState == States.SubmitRejected)))
                    .OfType<CharterOut>().OrderBy(c => c.ActionDate).LastOrDefault();

            var lastCharterOutEndBeforeGivenDate = charterOutRepository.Find(
                c => c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.OwnerId == companyId &&
                    c.ActionDate <= comparingDateTime &&
                    c.CharterType == CharterType.End &&
                    (c.CurrentState == States.Submitted || (includeSubmitRejected && c.CurrentState == States.SubmitRejected)))
                    .OfType<CharterOut>().OrderBy(c => c.ActionDate).LastOrDefault();

            var lastCharterInStartBeforeGivenDate = charterInRepository.Find(
                c => c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.ChartererId == companyId &&
                    c.ActionDate <= comparingDateTime &&
                    c.CharterType == CharterType.Start &&
                    (c.CurrentState == States.Submitted || (includeSubmitRejected && c.CurrentState == States.SubmitRejected)))
                    .OfType<CharterIn>().OrderBy(c => c.ActionDate).LastOrDefault();

            var lastCharterInEndBeforeGivenDate = charterInRepository.Find(
                c => c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.ChartererId == companyId &&
                    c.ActionDate <= comparingDateTime &&
                    c.CharterType == CharterType.End &&
                    (c.CurrentState == States.Submitted || (includeSubmitRejected && c.CurrentState == States.SubmitRejected)))
                    .OfType<CharterIn>().OrderBy(c => c.ActionDate).LastOrDefault();

            var foundCharterContracts = new List<Charter>();
            foundCharterContracts.Add(lastCharterInStartBeforeGivenDate);
            foundCharterContracts.Add(lastCharterInEndBeforeGivenDate);
            foundCharterContracts.Add(lastCharterOutStartBeforeGivenDate);
            foundCharterContracts.Add(lastCharterOutEndBeforeGivenDate);

            foundCharterContracts.RemoveAll(c => c == null);

            var lastCharterContract = foundCharterContracts.OrderBy(c => c.ActionDate).ThenBy(c =>
            {
                var orderValue = 0;

                if (c is CharterIn && c.CharterType == CharterType.Start)
                    orderValue = 1;

                if (c is CharterOut && c.CharterType == CharterType.Start)
                    orderValue = 2; //Charter Out Start has precedence over Charter In Start.

                if (c is CharterOut && c.CharterType == CharterType.End)
                    orderValue = 3;

                if (c is CharterIn && c.CharterType == CharterType.End)
                    orderValue = 4; //Charter In End has precedence over Charter Out End.

                return orderValue;
            }).LastOrDefault();

            return lastCharterContract;
        }

        public Charter GetNextCharterContractForCompany(string vesselCode, long companyId, DateTime comparingDateTime, long? charterIdToExclude, bool includeSubmitRejected = false)
        {
            var lastCharterOutStartBeforeGivenDate = charterOutRepository.Find(
                c => (!charterIdToExclude.HasValue || c.Id != charterIdToExclude) &&
                    c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.OwnerId == companyId &&
                    c.ActionDate >= comparingDateTime &&
                    c.CharterType == CharterType.Start &&
                    (c.CurrentState == States.Submitted || (includeSubmitRejected && c.CurrentState == States.SubmitRejected)))
                    .OfType<CharterOut>().OrderBy(c => c.ActionDate).FirstOrDefault();

            var lastCharterOutEndBeforeGivenDate = charterOutRepository.Find(
                c => (!charterIdToExclude.HasValue || c.Id != charterIdToExclude) &&
                    c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.OwnerId == companyId &&
                    c.ActionDate >= comparingDateTime &&
                    c.CharterType == CharterType.End &&
                    (c.CurrentState == States.Submitted || (includeSubmitRejected && c.CurrentState == States.SubmitRejected)))
                    .OfType<CharterOut>().OrderBy(c => c.ActionDate).FirstOrDefault();

            var lastCharterInStartBeforeGivenDate = charterInRepository.Find(
                c => (!charterIdToExclude.HasValue || c.Id != charterIdToExclude) &&
                    c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.ChartererId == companyId &&
                    c.ActionDate >= comparingDateTime &&
                    c.CharterType == CharterType.Start &&
                    (c.CurrentState == States.Submitted || (includeSubmitRejected && c.CurrentState == States.SubmitRejected)))
                    .OfType<CharterIn>().OrderBy(c => c.ActionDate).FirstOrDefault();

            var lastCharterInEndBeforeGivenDate = charterInRepository.Find(
                c => (!charterIdToExclude.HasValue || c.Id != charterIdToExclude) &&
                    c.VesselInCompany.Vessel.Code == vesselCode &&
                    c.ChartererId == companyId &&
                    c.ActionDate >= comparingDateTime &&
                    c.CharterType == CharterType.End &&
                    (c.CurrentState == States.Submitted || (includeSubmitRejected && c.CurrentState == States.SubmitRejected)))
                    .OfType<CharterIn>().OrderBy(c => c.ActionDate).FirstOrDefault();

            var foundCharterContracts = new List<Charter>();
            foundCharterContracts.Add(lastCharterInStartBeforeGivenDate);
            foundCharterContracts.Add(lastCharterInEndBeforeGivenDate);
            foundCharterContracts.Add(lastCharterOutStartBeforeGivenDate);
            foundCharterContracts.Add(lastCharterOutEndBeforeGivenDate);

            foundCharterContracts.RemoveAll(c => c == null);

            var nextCharterContract = foundCharterContracts.OrderBy(c => c.ActionDate).ThenBy(c =>
            {
                var orderValue = 0;

                if (c is CharterIn && c.CharterType == CharterType.Start)
                    orderValue = 1;

                if (c is CharterOut && c.CharterType == CharterType.Start)
                    orderValue = 2; //Charter Out Start has precedence over Charter In Start.

                if (c is CharterOut && c.CharterType == CharterType.End)
                    orderValue = 3;

                if (c is CharterIn && c.CharterType == CharterType.End)
                    orderValue = 4; //Charter In End has precedence over Charter Out End.

                return orderValue;
            }).FirstOrDefault();

            return nextCharterContract;
        }
    }
}