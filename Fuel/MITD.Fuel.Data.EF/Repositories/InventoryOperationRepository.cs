using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class InventoryOperationRepository : EFRepository<InventoryOperation>, IInventoryOperationRepository
    {
        public InventoryOperationRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {

        }

        public InventoryOperationRepository(IUnitOfWorkScope iUnitOfWorkScope)
            : base(iUnitOfWorkScope)
        {

        }

    }
}
