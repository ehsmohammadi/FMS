using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.AutomaticVoucher.Services
{
    public class CheckVoucher : ICheckVoucher
    {
        private IVoucherRepository _voucherRepository;
        private IUnitOfWorkScope _unitOfWorkScope;

        public CheckVoucher(IVoucherRepository voucherRepository, IUnitOfWorkScope unitOfWorkScope)
        {
            _unitOfWorkScope = unitOfWorkScope;
            _voucherRepository = voucherRepository;
        }
        
        public bool HasVoucher(string headerNo,long transactionId)
        {
            var res  = _voucherRepository.GetAll().Where(c => c.ReferenceNo == headerNo).ToList();
            if (!res.Any())
                return false;


            return res.Select(re => re.JournalEntrieses.Select(c => c.InventoryItemId == transactionId)).Any(res1 => (res1.Any()));
        }
    }
}
