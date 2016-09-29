using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class OffhireRepository : EFRepository<Offhire>, IOffhireRepository
    {
        private IEntityConfigurator<Offhire> offhireConfigurator;

        public OffhireRepository(EFUnitOfWork unitofwork)
            : base(unitofwork)
        {

        }

        public OffhireRepository(IUnitOfWorkScope unitofworkscope)
            : base(unitofworkscope)
        {

        }

        public void SetConfigurator(IEntityConfigurator<Offhire> offhireConfigurator)
        {
            this.offhireConfigurator = offhireConfigurator;

        }
        

        public new IList<Offhire> GetAll()
        {
            var result = base.GetAll().ToList();

            result.ForEach(e => offhireConfigurator.Configure(e));

            return result;
        }

        public new IList<Offhire> GetAll(IListFetchStrategy<Offhire> fetchStrategy)
        {
            var result = base.GetAll(fetchStrategy).ToList();

            result.ForEach(e => offhireConfigurator.Configure(e));

            return result;
        }

        public new IList<Offhire> Find(Expression<Func<Offhire, bool>> where)
        {
            var result = base.Find(where).ToList();

            result.ForEach(e => offhireConfigurator.Configure(e));

            return result;
        }

        public new IList<Offhire> Find(Expression<Func<Offhire, bool>> where, IListFetchStrategy<Offhire> fetchStrategy)
        {
            var result = base.Find(where, fetchStrategy).ToList();

            result.ForEach(e => offhireConfigurator.Configure(e));

            return result;
        }

        public new Offhire FindByKey<Key>(Key keyValue)
        {
            var result = base.FindByKey<Key>(keyValue);

            return offhireConfigurator.Configure(result);
        }

        public new Offhire FindByKey<Key>(Key keyValue, IFetchStrategy<Offhire> fetchStrategy)
        {
            var result = base.FindByKey<Key>(keyValue, fetchStrategy);

            return offhireConfigurator.Configure(result);
        }

        public new Offhire Single(Expression<Func<Offhire, bool>> where, ISingleResultFetchStrategy<Offhire> fetchStrategy)
        {
            var result = base.Single(where, fetchStrategy);

            return offhireConfigurator.Configure(result);
        }

        public new Offhire Single(Expression<Func<Offhire, bool>> where)
        {
            var result = base.Single(where);

            return offhireConfigurator.Configure(result);
        }

        public new Offhire First(Expression<Func<Offhire, bool>> where, ISingleResultFetchStrategy<Offhire> fetchStrategy)
        {
            var result = base.First(where, fetchStrategy);

            return offhireConfigurator.Configure(result);
        }

        public new Offhire First(Expression<Func<Offhire, bool>> where)
        {
            var result = base.First(where);

            return offhireConfigurator.Configure(result);
        }

    }
}