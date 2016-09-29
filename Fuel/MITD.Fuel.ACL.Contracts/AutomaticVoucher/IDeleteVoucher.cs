using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.ACL.Contracts.AutomaticVoucher
{
    public interface IDeleteVoucher:IAutomaticVoucher
    {
        void Done(long inventoryItemId, string headerCode);
    }
}
