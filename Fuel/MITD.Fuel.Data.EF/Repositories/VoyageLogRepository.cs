using System.Collections.Generic;
using System.Linq;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class VoyageLogRepository : EFRepository<VoyageLog>, IVoyageLogRepository
    {
        public VoyageLogRepository(EFUnitOfWork unitofwork)
            : base(unitofwork)
        {
        }

        public VoyageLogRepository(IUnitOfWorkScope unitofworkscope)
            : base(unitofworkscope)
        {
        }
    }
}