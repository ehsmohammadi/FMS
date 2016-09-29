using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class VoyageLogDomainService : IVoyageLogDomainService
    {
        private readonly IVoyageLogRepository voyageLogRepository;

        public VoyageLogDomainService(IVoyageLogRepository voyageLogRepository)
        {
            this.voyageLogRepository = voyageLogRepository;
        }

        public PageResult<VoyageLog> GetPagedDataByFilter(long voyageId, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<VoyageLog>();
            fetchStrategy.Include(v => v.VesselInCompany).Include(v => v.Company);

            fetchStrategy.WithPaging(pageSize, pageNumber);

            this.voyageLogRepository.Find(v => v.ReferencedVoyageId == voyageId, fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }
    }
}