using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class GoodRepository : EFRepository<Good>, IGoodRepository
    {
        public GoodRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {


        }

        public GoodRepository(IUnitOfWorkScope iUnitOfWorkScope)
            : base(iUnitOfWorkScope)
        {

        }
    }
}
