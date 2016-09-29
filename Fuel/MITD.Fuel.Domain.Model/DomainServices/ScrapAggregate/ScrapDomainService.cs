using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class ScrapDomainService : IScrapDomainService
    {
        private readonly IScrapRepository scrapRepository;
        private readonly IRepository<ScrapDetail> scrapDetailRepository;
        private IsScrapOpen isScrapOpen;
        private IsScrapSubmitted isScrapSubmitted;
        private IsScrapSubmitRejected isScrapSubmitRejected;


        public ScrapDomainService(IScrapRepository scrapRepository, IRepository<ScrapDetail> scrapDetailRepository)
        {
            this.scrapRepository = scrapRepository;
            this.scrapDetailRepository = scrapDetailRepository;


            this.isScrapOpen = new IsScrapOpen();
            this.isScrapSubmitted = new IsScrapSubmitted();
            this.isScrapSubmitRejected = new IsScrapSubmitRejected();

        }

        //================================================================================

        public Scrap Get(long scrapId)
        {
            var scrap = scrapRepository.First(e => e.Id == scrapId);

            if (scrap == null)
                throw new ObjectNotFound("Scrap", scrapId);

            return scrap;
        }

        //================================================================================

        public PageResult<Scrap> GetPagedData(int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Scrap>()
                .Include(s => s.VesselInCompany).Include(s => s.SecondParty)
                .WithPaging(pageSize, pageNumber);

            scrapRepository.GetAll(fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public PageResult<Scrap> GetPagedDataByFilter(long? companyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Scrap>()
                .Include(s => s.VesselInCompany).Include(s => s.SecondParty)
                .WithPaging(pageSize, pageNumber);

            scrapRepository.Find(
                e => (!companyId.HasValue || e.VesselInCompany.Company.Id == companyId) &&
                    (!fromDate.HasValue || e.ScrapDate >= fromDate) &&
                    (!toDate.HasValue || e.ScrapDate <= toDate),
                fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public PageResult<ScrapDetail> GetPagedScrapDetailData(long scrapId, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<ScrapDetail>()
                .WithPaging(pageSize, pageNumber);

            scrapDetailRepository.Find(sd => sd.Scrap.Id == scrapId, fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public ScrapDetail GetScrapDetail(long scrapId, long scrapDetailId)
        {
            var scrapDetail = scrapDetailRepository.Single(sd => sd.Scrap.Id == scrapId && sd.Id == scrapDetailId);

            if (scrapDetail == null)
                throw new ObjectNotFound("ScrapDetail", scrapDetailId);

            return scrapDetail;
        }

        //================================================================================

        public void Delete(Scrap scrap)
        {
            scrapRepository.Delete(scrap);
        }

        //================================================================================

        public void DeleteScrapDetail(ScrapDetail scrapDetail)
        {
            scrapDetailRepository.Delete(scrapDetail);
        }

        //================================================================================

        public List<Scrap> GetNotCancelledScrapsForVessel(VesselInCompany vesselInCompany, params long[] excludeIds)
        {
            IsScrapCancelled isScrapCancelled = new IsScrapCancelled();

            var result = scrapRepository.Find(isScrapCancelled.Predicate.Not().And(s => s.VesselInCompany.Id == vesselInCompany.Id));

            if (excludeIds != null)
            {
                result = result.Where(s => !excludeIds.Contains(s.Id)).ToList();
            }

            return result.ToList();
        }

        //================================================================================

        public bool IsScrapEditPermitted(Scrap scrap)
        {
            return isScrapOpen.IsSatisfiedBy(scrap);
        }

        //================================================================================

        public bool IsGoodEditable(Scrap scrap)
        {
            return isScrapOpen.IsSatisfiedBy(scrap);
        }

        //================================================================================

        public bool IsTankEditable(Scrap scrap)
        {
            return isScrapOpen.IsSatisfiedBy(scrap);
        }

        //================================================================================

        public bool IsScrapDeletePermitted(Scrap scrap)
        {
            return isScrapOpen.IsSatisfiedBy(scrap);
        }

        //================================================================================

        public bool IsScrapDetailAddPermitted(Scrap scrap)
        {
            return isScrapOpen.IsSatisfiedBy(scrap) || isScrapSubmitRejected.IsSatisfiedBy(scrap);
        }

        //================================================================================

        public bool IsScrapDetailEditPermitted(Scrap scrap)
        {
            return isScrapOpen.IsSatisfiedBy(scrap) || isScrapSubmitRejected.IsSatisfiedBy(scrap);
        }

        //================================================================================

        public bool IsScrapDetailDeletePermitted(Scrap scrap)
        {
            return isScrapOpen.IsSatisfiedBy(scrap);
        }

        //================================================================================

        public void SetInventoryResults(InventoryResultCommand resultCommand, Scrap scrap)
        {

        }

        //================================================================================
    }
}