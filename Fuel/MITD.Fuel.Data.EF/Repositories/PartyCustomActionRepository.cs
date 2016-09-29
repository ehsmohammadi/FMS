using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.DataAccess.EF;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Repository;

namespace MITD.Fuel.Data.EF.Repositories
{
    public class PartyCustomActionRepository :EFRepository<PartyCustomAction>, IPartyCustomActionRepository
    {
        public PartyCustomActionRepository(EFUnitOfWork unitofwork) : base(unitofwork)
        {
        }

        public PartyCustomActionRepository(IUnitOfWorkScope unitofworkscope) : base(unitofworkscope)
        {
        }

        public void DeleteAllByPartyId(long partyid)
        {
            var q= this.Context.CreateObjectSet<PartyCustomAction>();
            var lst= q.Where(c => c.PartyId == partyid).ToList();

           // var total = lst.Count();
            foreach (var partyCustomAction in lst)
            {
                this.Delete(partyCustomAction);
            }
            
            //for (int i = 0; i < total; i++)
            //{
            //   this.Delete(lst[i]);
            //    total--;
            //}

        }
    }
}
