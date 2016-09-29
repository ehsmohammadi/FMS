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
    public class CharterInRepository : CharterRepository, ICharterInRepository
    {
         public CharterInRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {

        }

        public CharterInRepository(IUnitOfWorkScope unitOfWorkScope):base(unitOfWorkScope)
        {
            
        }


        public IQueryable<CharterIn> GetQueryInclude()
        {
            return this.Context.CreateObjectSet<Charter>()
                .Include("CharterItems")
                .Include("InventoryOperationItems").OfType<CharterIn>()
                .AsQueryable();
        }
        public IQueryable<CharterIn> GetQueryable()
        {
            return this.Context.CreateObjectSet<Charter>().OfType<CharterIn>()
                .AsQueryable();
        }


        public PageResult<CharterIn> GetByFilter(long vesselInCompanyId,long companyId, long id, DateTime? startdate, DateTime? enddate, int pageSize, int pageIndex)
        {
            var res = new PageResult<CharterIn>();
       
               var strategy=new ListFetchStrategy<Charter>(Enums.FetchInUnitOfWorkOption.NoTracking);

            if (id==0)
            {
                IQueryable<CharterIn> query = this.GetAll(strategy).OfType<CharterIn>().OrderByDescending(p => p.Id)
              .Where(c => ((c.ChartererId == companyId) &&
                          (c.ActionDate <= enddate || enddate == null) &&
                          (c.ActionDate >= startdate || startdate == null))&&
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
                res.Result =new List<CharterIn>(){GetById(id,companyId)};

                res.TotalCount = res.Result.Count;

                res.TotalPages = res.Result.Count;

                res.CurrentPage = res.Result.Count;
            }

          

            return res;

        }


        public CharterIn GetCharterStartById(long id)
        {

            var res = GetQueryInclude().Single(c => c.Id == id) as CharterIn;
            return res;
        }

        public CharterIn GetCharterEnd(long startId)
        {

             var sEntity = GetQueryInclude().SingleOrDefault(c => c.Id == startId);

            if (sEntity != null)
            {
                return GetQueryInclude().Where(c => c.VesselInCompanyId == sEntity.VesselInCompanyId &&
                                                    c.CharterType == CharterType.End &&
                                                    c.ChartererId == sEntity.ChartererId &&

                                                    c.ActionDate > sEntity.ActionDate)
                    .OrderBy(c => c.Id)
                    .FirstOrDefault();
            }
            return null;
        }

        public CharterIn GetCharterStart(long vesselInCompanyId, long chartererId)
        {


            var sEntity = GetQueryInclude().Where(c => c.VesselInCompanyId == vesselInCompanyId
                                              && c.ChartererId == chartererId
                                              && c.CharterType==CharterType.Start)
                                              .OrderByDescending(c=>c.Id).FirstOrDefault();
            return sEntity;

        }


        public CharterIn GetById(long id)
        {
            return this.FindByKey(id) as CharterIn;
        }

        private CharterIn GetById(long id,long charterid)
        {
            var r= this.FindByKey(id) as CharterIn;
            return (r.ChartererId == charterid) ? r : null;
        }
    }
}
