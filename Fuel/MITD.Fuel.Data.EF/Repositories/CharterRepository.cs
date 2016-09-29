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
    public class CharterRepository :EFRepository<Charter>, ICharterRepository
    {
        public CharterRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {

        }

        public CharterRepository(IUnitOfWorkScope unitOfWorkScope):base(unitOfWorkScope)
        {
            
        }
    }
}
