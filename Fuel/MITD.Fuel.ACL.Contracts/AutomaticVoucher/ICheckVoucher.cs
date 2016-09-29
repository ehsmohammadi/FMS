using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.Contracts.AutomaticVoucher
{
   public interface ICheckVoucher:IAutomaticVoucher
   {
       bool HasVoucher(string headerNo, long transactionId);
   }
}
