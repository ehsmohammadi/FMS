using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class CharterOutRepository : CharterRepository, ICharterOutRepository
    {
        public CharterOutRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {

        }

        public CharterOutRepository(IUnitOfWorkScope unitOfWorkScope)
            : base(unitOfWorkScope)
        {

        }


        public IQueryable<CharterOut> GetQueryInclude()
        {
            return this.Context.CreateObjectSet<Charter>()
                .Include("CharterItems")
                .Include("InventoryOperationItems").OfType<CharterOut>()
                .AsQueryable();
           
        }
        public IQueryable<CharterOut> GetQueryable()
        {
            return this.Context.CreateObjectSet<Charter>().OfType<CharterOut>()
                .AsQueryable();
        }

        public PageResult<CharterOut> GetByFilter(long vesselInCompanyId,long companyId, long id, DateTime? startdate, DateTime? enddate, int pageSize, int pageIndex)
        {
            var res = new PageResult<CharterOut>();

            var strategy = new ListFetchStrategy<Charter>(Enums.FetchInUnitOfWorkOption.NoTracking);


            if (id == 0)
            {
                IQueryable<CharterOut> query = this.GetAll(strategy).OfType<CharterOut>().OrderByDescending(p => p.Id)
              .Where(c => ((c.OwnerId == companyId) &&
                          (c.ActionDate <= enddate || enddate == null) &&
                          (c.ActionDate >= startdate || startdate == null)) &&
                          (c.VesselInCompanyId == vesselInCompanyId || vesselInCompanyId == 0)

                        ).AsQueryable();


                res.Result = query.Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).ToList();

                res.TotalCount = query.Count();

                res.TotalPages = Convert.ToInt32(Math.Ceiling(decimal.Divide(res.TotalCount, pageSize)));

                res.CurrentPage = pageIndex; //+ 1;
            }
            else
            {
                res.Result = new List<CharterOut>() { GetById(id, companyId) };

                res.TotalCount = res.Result.Count;

                res.TotalPages = res.Result.Count;

                res.CurrentPage = res.Result.Count;
            }



            return res;


        }


        public CharterOut GetCharterStartById(long id)
        {

            var res = GetQueryInclude().Single(c => c.Id == id) as CharterOut;
            return res;
        }

        public CharterOut GetCharterEnd(long startId)
        {

            var sEntity = GetQueryInclude().SingleOrDefault(c => c.Id == startId);

            if (sEntity != null)
            {
                return GetQueryInclude().Where(c => c.VesselInCompanyId == sEntity.VesselInCompanyId &&
                                                    c.CharterType == CharterType.End &&
                                                    c.OwnerId == sEntity.OwnerId &&


                                                    c.ActionDate > sEntity.ActionDate)
                    .OrderBy(c => c.Id)
                    .FirstOrDefault();
            }
            return null;
        }

        public CharterOut GetCharterStart(long id,long vesselInCompanyId, long ownerId)
        {
            var sEntity = new CharterOut();
            if (id==0)
            {
                var x = GetQueryInclude().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                                               && c.OwnerId == ownerId
                                                               && c.CharterType == CharterType.Start
                    ).
                    OrderByDescending(c => c.Id).Select(d=>d);
                
                sEntity = GetQueryInclude().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                               && c.OwnerId == ownerId
                                               && c.CharterType == CharterType.Start
                                               ).
                                              OrderByDescending(c => c.Id).FirstOrDefault();
                if (sEntity != null)
                if (!(sEntity.CurrentState == States.Submitted))
                {
                    sEntity = null;
                }


            }
            else
            {
                sEntity = GetQueryInclude().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                             && c.OwnerId == ownerId
                                             && c.Id < id
                                              && c.CurrentState==States.Submitted
                                             && c.CharterType == CharterType.Start).
                                             OrderByDescending(c => c.Id).FirstOrDefault();
            }
            
            return sEntity;

        }

        public CharterOut GetById(long id)
        {
            return this.FindByKey(id) as CharterOut;
        }

        private CharterOut GetById(long id, long charterid)
        {
            var r = this.FindByKey(id) as CharterOut;
            return (r.OwnerId == charterid) ? r : null;
        }
    }
}
