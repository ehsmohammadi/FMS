using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class CharterItemHistoryRepository :EFRepository<CharterItemHistory>, ICharterItemHistoryRepository
    {
        public CharterItemHistoryRepository(EFUnitOfWork unitofwork) : base(unitofwork)
        {
        }

        public CharterItemHistoryRepository(IUnitOfWorkScope unitofworkscope)
            : base(unitofworkscope)
        {
        }
    }
}
