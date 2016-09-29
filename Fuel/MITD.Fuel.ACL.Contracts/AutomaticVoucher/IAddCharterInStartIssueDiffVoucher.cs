using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.Contracts.AutomaticVoucher
{
   public interface IAddCharterInStartIssueDiffVoucher:IAutomaticVoucher
    {
       void Execute(CharterIn charterIn, List<Issue> issues,
         string issueWarehouseCode, string issueNumber, long userId
         , string lineCode, string voyageCode);
    }
}
