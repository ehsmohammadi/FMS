using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
   public class OriginalAccountRepository:EFRepository<OriginalAccount>,IOriginalAccountRepository
    {
       public OriginalAccountRepository(EFUnitOfWork unitofwork) : base(unitofwork)
       {
       }

       public OriginalAccountRepository(IUnitOfWorkScope unitofworkscope)
           : base(unitofworkscope)
       {
       }
    }
}
