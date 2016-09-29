using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.IDomainServices;
using System.Linq.Expressions;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class VesselInCompanyRepository : EFRepository<VesselInCompany>, IVesselInCompanyRepository
    {
        private IEntityConfigurator<VesselInCompany> vesselInCompanyConfigurator;

        public VesselInCompanyRepository(EFUnitOfWork efUnitOfWork, IEntityConfigurator<VesselInCompany> vesselInCompanyConfigurator)
            : base(efUnitOfWork)
        {
            this.vesselInCompanyConfigurator = vesselInCompanyConfigurator;
        }

        public VesselInCompanyRepository(IUnitOfWorkScope iUnitOfWorkScope, IEntityConfigurator<VesselInCompany> vesselInCompanyConfigurator)
            : base(iUnitOfWorkScope)
        {
            this.vesselInCompanyConfigurator = vesselInCompanyConfigurator;
        }


        //================================================================================

        public new IList<VesselInCompany> Find(Expression<Func<VesselInCompany, bool>> where)
        {
            var result = base.Find(where).ToList();

            result.ForEach(e => vesselInCompanyConfigurator.Configure(e));

            return result;
        }

        //================================================================================

        public new IList<VesselInCompany> GetAll()
        {
            var result = base.GetAll().ToList();

            result.ForEach(e => vesselInCompanyConfigurator.Configure(e));

            return result;
        }

        //================================================================================

        public new IList<VesselInCompany> GetAll(IListFetchStrategy<VesselInCompany> fetchStrategy)
        {
            var result = base.GetAll(fetchStrategy).ToList();

            result.ForEach(e => vesselInCompanyConfigurator.Configure(e));

            return result;
        }

        //================================================================================

        public new IList<VesselInCompany> Find(Expression<Func<VesselInCompany, bool>> where, IListFetchStrategy<VesselInCompany> fetchStrategy)
        {
            var result = base.Find(where, fetchStrategy).ToList();

            result.ForEach(e => vesselInCompanyConfigurator.Configure(e));

            return result;
        }

        //================================================================================

        public new VesselInCompany FindByKey<Key>(Key keyValue)
        {
            var result = base.FindByKey<Key>(keyValue);

            return vesselInCompanyConfigurator.Configure(result);
        }

        //================================================================================

        public new VesselInCompany FindByKey<Key>(Key keyValue, IFetchStrategy<VesselInCompany> fetchStrategy)
        {
            var result = base.FindByKey<Key>(keyValue, fetchStrategy);

            return vesselInCompanyConfigurator.Configure(result);
        }

        //================================================================================

        public new VesselInCompany Single(Expression<Func<VesselInCompany, bool>> where, ISingleResultFetchStrategy<VesselInCompany> fetchStrategy)
        {
            var result = base.Single(where, fetchStrategy);

            return vesselInCompanyConfigurator.Configure(result);
        }

        //================================================================================

        public new VesselInCompany Single(Expression<Func<VesselInCompany, bool>> where)
        {
            var result = base.Single(where);

            return vesselInCompanyConfigurator.Configure(result);
        }

        //================================================================================

        public new VesselInCompany First(Expression<Func<VesselInCompany, bool>> where, ISingleResultFetchStrategy<VesselInCompany> fetchStrategy)
        {
            var result = base.First(where, fetchStrategy);

            return vesselInCompanyConfigurator.Configure(result);
        }

        //================================================================================

        public new VesselInCompany First(Expression<Func<VesselInCompany, bool>> where)
        {
            var result = base.First(where);

            return vesselInCompanyConfigurator.Configure(result);
        }

        //================================================================================
    }
}
