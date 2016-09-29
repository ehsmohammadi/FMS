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
    public class TransactionRepository : EFRepository<Inventory_Transaction>, ITransactionRepository
    {
        public TransactionRepository(EFUnitOfWork unitofwork)
            : base(unitofwork)
        {
         
        }

        public TransactionRepository(IUnitOfWorkScope unitofworkscope)
            : base(unitofworkscope)
        {

        }

      
    }
}