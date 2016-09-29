using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Domain.Repository;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class VesselDomainService : IVesselDomainService
    {
        private IVesselRepository vesselRepository;

        public VesselDomainService(
            IVesselRepository vesselRepository
            )
        {
            this.vesselRepository = vesselRepository;
        }


        public Vessel Get(long id)
        {
            var fetchStrategy = new SingleResultFetchStrategy<Vessel>().Include(v=>v.Owner);

            return vesselRepository.First(
                e => (e.Id == id),
                fetchStrategy);


        }

        public List<Vessel> Get(List<long> IDs)
        {
            throw new NotImplementedException();
        }

        public List<Vessel> GetAll()
        {
            throw new NotImplementedException();
        }

        public PageResult<Vessel> GetPagedData(int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Vessel>()
                .WithPaging(pageSize, pageNumber);

            vesselRepository.GetAll(fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        public PageResult<Vessel> GetPagedDataByFilter(long? ownerCompanyId, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Vessel>()
                .WithPaging(pageSize, pageNumber);

            vesselRepository.Find(
                e => (e.OwnerId == ownerCompanyId),
                fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }
    }
}
