using System;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface ICharteringDomainService : IDomainService
    {
        CharterOut GetCharterOutStart(Company company, VesselInCompany vesselInCompany, DateTime date);
        CharterIn GetCharterInStart(Company company, VesselInCompany vesselInCompany, DateTime date);
        CharterOut GetLastCharterOutEnd(Company company, VesselInCompany vesselInCompany, DateTime date);

        Charter GetVesselActivationCharterContract(VesselInCompany vesselInCompany, DateTime comparingDateTime);

        Charter GetLastCharterContract(string vesselCode, DateTime fuelReportDateTime);

        Charter GetLastCharterContractForCompany(string vesselCode, long companyId, DateTime comparingDateTime, bool includeSubmitRejected = false);
        Charter GetNextCharterContractForCompany(string vesselCode, long companyId, DateTime comparingDateTime, long? charterIdToExclude, bool includeSubmitRejected = false);

        //CharterOut GetLastCharterOutEnd(CharterOut comparingCharterOutStart);
        //OffHirePricingType GetPricingType(Company company, Vessel vessel, CharteringPartyType partyType, DateTime date);
        //string GetCharteringReferenceNumber(Company company, Vessel vessel, CharteringPartyType partyType, DateTime date);
    }
}