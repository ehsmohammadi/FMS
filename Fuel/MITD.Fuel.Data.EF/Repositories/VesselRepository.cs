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
    public class VesselRepository : EFRepository<Vessel>, IVesselRepository
    {
        public VesselRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {

        }

        public VesselRepository(IUnitOfWorkScope iUnitOfWorkScope)
            : base(iUnitOfWorkScope)
        {

        }
    }
}
