using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Domain.Model.Repositories
{
    public interface IVoucherRepository:IRepository<Voucher>
    {
        long GetLocalVoucherNo();
        void Detach();
        List<Voucher> GetAllWithSegment();
    }
}
