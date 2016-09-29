using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IScrapDomainService : IDomainService
    {
        Scrap Get(long scrapId);
        PageResult<Scrap> GetPagedData(int pageSize, int pageIndex);
        PageResult<Scrap> GetPagedDataByFilter(long? companyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex);

        PageResult<ScrapDetail> GetPagedScrapDetailData(long scrapId, int pageSize, int pageIndex);
        ScrapDetail GetScrapDetail(long scrapId, long scrapDetailId);

        void Delete(Scrap scrap);

        void DeleteScrapDetail(ScrapDetail scrapDetail);

        List<Scrap> GetNotCancelledScrapsForVessel(VesselInCompany vesselInCompany, params long[] excludeIds);

        bool IsScrapEditPermitted(Scrap scrap);

        bool IsGoodEditable(Scrap scrap);

        bool IsTankEditable(Scrap scrap);

        bool IsScrapDeletePermitted(Scrap scrap);

        bool IsScrapDetailAddPermitted(Scrap scrap);

        bool IsScrapDetailEditPermitted(Scrap scrap);

        bool IsScrapDetailDeletePermitted(Scrap scrap);

        void SetInventoryResults(InventoryResultCommand resultCommand, Scrap scrap);
    }
}