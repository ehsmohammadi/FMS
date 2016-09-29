using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;
using System.Linq;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class FuelReportRepository : EFRepository<FuelReport>, IFuelReportRepository
    {
        private readonly IEntityConfigurator<FuelReport> fuelReportConfigurator;

        public FuelReportRepository(EFUnitOfWork efUnitOfWork, IEntityConfigurator<FuelReport> fuelReportConfigurator)
            : base(efUnitOfWork)
        {
            this.fuelReportConfigurator = fuelReportConfigurator;
        }

        public FuelReportRepository(
            IUnitOfWorkScope iUnitOfWorkScope, IEntityConfigurator<FuelReport> fuelReportConfigurator)
            : base(iUnitOfWorkScope)
        {
            this.fuelReportConfigurator = fuelReportConfigurator;
        }

        //================================================================================

        public new IList<FuelReport> Find(Expression<Func<FuelReport, bool>> where)
        {
            var result = base.Find(where).ToList();

            result.ForEach(e => fuelReportConfigurator.Configure(e));

            return result;
        }

        //================================================================================

        public new IList<FuelReport> GetAll()
        {
            var result = base.GetAll().ToList();

            result.ForEach(e => fuelReportConfigurator.Configure(e));

            return result;
        }

        //================================================================================

        public new IList<FuelReport> GetAll(IListFetchStrategy<FuelReport> fetchStrategy)
        {
            var result = base.GetAll(fetchStrategy).ToList();

            result.ForEach(e => fuelReportConfigurator.Configure(e));

            return result;
        }

        //================================================================================

        public new IList<FuelReport> Find(Expression<Func<FuelReport, bool>> where, IListFetchStrategy<FuelReport> fetchStrategy)
        {
            var result = base.Find(where, fetchStrategy).ToList();

            result.ForEach(e => fuelReportConfigurator.Configure(e));

            return result;
        }

        //================================================================================

        public new FuelReport FindByKey<Key>(Key keyValue)
        {
            var result = base.FindByKey<Key>(keyValue);

            return fuelReportConfigurator.Configure(result);
        }

        //================================================================================

        public new FuelReport FindByKey<Key>(Key keyValue, IFetchStrategy<FuelReport> fetchStrategy)
        {
            var result = base.FindByKey<Key>(keyValue, fetchStrategy);

            return fuelReportConfigurator.Configure(result);
        }

        //================================================================================

        public new FuelReport Single(Expression<Func<FuelReport, bool>> where, ISingleResultFetchStrategy<FuelReport> fetchStrategy)
        {
            var result = base.Single(where, fetchStrategy);

            return fuelReportConfigurator.Configure(result);
        }

        //================================================================================

        public new FuelReport Single(Expression<Func<FuelReport, bool>> where)
        {
            var result = base.Single(where);

            return fuelReportConfigurator.Configure(result);
        }

        //================================================================================

        public new FuelReport First(Expression<Func<FuelReport, bool>> where, ISingleResultFetchStrategy<FuelReport> fetchStrategy)
        {
            var result = base.First(where, fetchStrategy);

            return fuelReportConfigurator.Configure(result);
        }

        //================================================================================

        public new FuelReport First(Expression<Func<FuelReport, bool>> where)
        {
            var result = base.First(where);

            return fuelReportConfigurator.Configure(result);
        }



        //================================================================================

        public List<long> FindOpenFuelReportIdByVesselInCompany(long? vesselInCompanyId)
        {
            var res = base.GetQuery().AsNoTracking();
            if (vesselInCompanyId.HasValue)
                res=res.Where(fr => fr.VesselInCompanyId == vesselInCompanyId);
            res = res.Where(fr => fr.State == States.SubmitRejected || fr.State == States.Open);
            return res.Select(fr => fr.Id).ToList();
        }
    }
}
