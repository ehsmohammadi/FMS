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
    public class VoucherSetingRepository : EFRepository<VoucherSeting>, IVoucherSetingRepository
    {
        public VoucherSetingRepository(EFUnitOfWork unitofwork) : base(unitofwork)
        {
        }

        public VoucherSetingRepository(IUnitOfWorkScope unitofworkscope) : base(unitofworkscope)
        {
        }

        public void Deletex(long id)
        {
            var asgndetail= this.Context.CreateObjectSet<AsgnSegmentTypeVoucherSetingDetail>().FirstOrDefault(c=>c.Id==id);
            
            this.Context.DeleteObject(asgndetail);
            

        }

    }
}
