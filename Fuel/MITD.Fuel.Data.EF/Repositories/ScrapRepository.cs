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
    public class ScrapRepository : EFRepository<Scrap>, IScrapRepository
    {
        private readonly IEntityConfigurator<Scrap> scrapConfigurator;

        public ScrapRepository(EFUnitOfWork unitofwork, IEntityConfigurator<Scrap> scrapConfigurator)
            : base(unitofwork)
        {
            this.scrapConfigurator = scrapConfigurator;
        }

        public ScrapRepository(IUnitOfWorkScope unitofworkscope, IEntityConfigurator<Scrap> scrapConfigurator)
            : base(unitofworkscope)
        {
            this.scrapConfigurator = scrapConfigurator;
        }

        public new IList<Scrap> Find(Expression<Func<Scrap, bool>> where)
        {
            var result = base.Find(where).ToList();

            result.ForEach(e => scrapConfigurator.Configure(e));

            return result;
        }

        public new IList<Scrap> GetAll()
        {
            var result = base.GetAll().ToList();

            result.ForEach(e => scrapConfigurator.Configure(e));

            return result;
        }

        public new IList<Scrap> GetAll(IListFetchStrategy<Scrap> fetchStrategy)
        {
            var result = base.GetAll(fetchStrategy).ToList();

            result.ForEach(e => scrapConfigurator.Configure(e));

            return result;
        }

        public new IList<Scrap> Find(Expression<Func<Scrap, bool>> where, IListFetchStrategy<Scrap> fetchStrategy)
        {
            var result = base.Find(where, fetchStrategy).ToList();

            result.ForEach(e => scrapConfigurator.Configure(e));

            return result;
        }

        public new Scrap FindByKey<Key>(Key keyValue)
        {
            var result = base.FindByKey<Key>(keyValue);

            return scrapConfigurator.Configure(result);
        }

        public new Scrap FindByKey<Key>(Key keyValue, IFetchStrategy<Scrap> fetchStrategy)
        {
            var result = base.FindByKey<Key>(keyValue, fetchStrategy);

            return scrapConfigurator.Configure(result);
        }

        public new Scrap Single(Expression<Func<Scrap, bool>> where, ISingleResultFetchStrategy<Scrap> fetchStrategy)
        {
            var result = base.Single(where, fetchStrategy);

            return scrapConfigurator.Configure(result);
        }

        public new Scrap Single(Expression<Func<Scrap, bool>> where)
        {
            var result = base.Single(where);

            return scrapConfigurator.Configure(result);
        }

        public new Scrap First(Expression<Func<Scrap, bool>> where, ISingleResultFetchStrategy<Scrap> fetchStrategy)
        {
            var result = base.First(where, fetchStrategy);

            return scrapConfigurator.Configure(result);
        }

        public new Scrap First(Expression<Func<Scrap, bool>> where)
        {
            var result = base.First(where);

            return scrapConfigurator.Configure(result);
        }
    }
}