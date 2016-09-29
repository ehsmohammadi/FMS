using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class HasNotHeaderType : BusinessRuleBase<VoucherSeting>
    {
   
        private IVoucherSetingRepository _voucherSetingRepository;


        public HasNotHeaderType( Expression<Func<string, bool>> predicate,VoucherSeting voucherSeting)
            : base(predicate, voucherSeting)
        {
            
            _voucherSetingRepository = ServiceLocator.Current.GetInstance<IVoucherSetingRepository>();
         
        }

        public override void Validate(string typeAction)
        {
            if (IsValidExpression(typeAction))
            {
                var res = _voucherSetingRepository.Find(
                    c =>
                        c.Company.Id == Entity.CompanyId &&
                        c.VoucherDetailTypeId == Entity.VoucherDetailTypeId &&
                        c.VoucherTypeId == Entity.VoucherTypeId);

                //if (res != null)
                if (res != null && res.Count > 0)
                    throw new BusinessRuleException("0909", "Has Same Voucher Type");
            }
        }
    }
}
