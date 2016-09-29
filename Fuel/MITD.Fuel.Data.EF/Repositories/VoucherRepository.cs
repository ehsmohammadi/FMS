using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class VoucherRepository : EFRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(EFUnitOfWork unitofwork)
            : base(unitofwork)
        {
        }

        public VoucherRepository(IUnitOfWorkScope unitofworkscope)
            : base(unitofworkscope)
        {

        }


        public List<Voucher> GetAllWithSegment()
        {
            var fetchstrategy = new ListFetchStrategy<Voucher>(Enums.FetchInUnitOfWorkOption.NoTracking);


            fetchstrategy.Include(c => c.JournalEntrieses)
                .Include(c => c.JournalEntrieses.SelectMany(e => e.Segments));


          return  this.GetAll(fetchstrategy).ToList();

           
        }

        public void Detach()
        {

           var lstEntry = new List<ObjectStateEntry>(); 
                Context.ObjectStateManager.GetObjectStateEntries(EntityState.Added).ToList().ForEach(c=>lstEntry.Add(c));
            lstEntry.ForEach(c=>Context.Detach(c.Entity));
            
        }

        public long GetLocalVoucherNo()
        {
            return this.Context.ExecuteStoreQuery<long>("select next value for Fuel.LocalVoucherNoGenerator").First();
        }
    }
}
