using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Repository;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class LogRepository : EFRepository<Log>, ILogRepository
    {
        public LogRepository(EFUnitOfWork efUnitOfWork)
            : base(efUnitOfWork)
        {
            
        }

        public LogRepository(IUnitOfWorkScope unitOfWorkScope)
            : base(unitOfWorkScope)
        {
            
        }

        
    }
}
