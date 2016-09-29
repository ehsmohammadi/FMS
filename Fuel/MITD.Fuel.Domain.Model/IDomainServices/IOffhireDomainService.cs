using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IOffhireDomainService : IDomainService
    {
        void SetConfigurator(IEntityConfigurator<Offhire> offhireConfigurator);

        Offhire Get(long offhireId);
        PageResult<Offhire> GetPagedData(int pageSize, int pageIndex);
        PageResult<Offhire> GetPagedDataByFilter(long? companyId, long? vesselInCompanyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex);

        PageResult<OffhireDetail> GetPagedOffhireDetailData(long offhireId, int pageSize, int pageIndex);
        OffhireDetail GetOffhireDetail(long offhireId, long offhireDetailId);

        void Delete(Offhire offhire);

        void DeleteOffhireDetail(OffhireDetail offhireDetail);

        List<Offhire> GetNotCancelledOffhiresForVessel(VesselInCompany vesselInCompany, params long[] excludeIds);

        bool IsOffhireEditPermitted(Offhire offhire);

        bool IsOffhireDeletePermitted(Offhire offhire);

        bool IsOffhireRegistered(long referenceNumber, Company introducer);

        Charter GetCharterContract(Company introducer, VesselInCompany vesselInCompany, CharteringPartyType introducerType, DateTime stratDate);

        Offhire GetRelevantCharterInOffhire(Company shipOwner, VesselInCompany vesselInCompany, DateTime startDate);

        Offhire GetCompanyValidOffhire(Company company, VesselInCompany vesselInCompany, DateTime startDate);

        InventoryResult GetVoyagePricedConsumptionIssue(long companyId, long voyageId);

        List<PricingValue> GetIssueBasedPricingValues(Voyage voyage);
        Charter GetOffhireCharterContract(Offhire offhire);
        List<PricingValue> GetCharterPartyBasedPricingValues(Offhire offhire);

        string GetCharteringReferenceNumber(Company company, VesselInCompany vesselInCompany, CharteringPartyType partyType, DateTime date);

        InventoryOperation GetVoyageConsumptionIssueOperation(long voyageId);

        PricingValue GetPricingValue(Company introducer, VesselInCompany vesselInCompany, DateTime startDateTime, Good good);
        PricingValue GetPricingValue(Company introducer, VesselInCompany vesselInCompany, DateTime startDateTime, Good good, Currency currency, DateTime currencyDateTime);

        CharteringPartyType GetCharteringPartyType(VesselInCompany vesselInCompany);

        OffHirePricingType GetPricingType(Company company, VesselInCompany vesselInCompany, DateTime date);

        bool IsOffhireRegisteredForVessel(long vesselInCompanyId, DateTime startDateTime, DateTime endDateTime);

        List<PricingValue> GetPricingValues(Company introducer, VesselInCompany vesselInCompany, DateTime startDateTime);
        List<PricingValue> GetPricingValues(Company introducer, VesselInCompany vesselInCompany, DateTime startDateTime, Currency currency, DateTime currencyDateTime);
    }
}