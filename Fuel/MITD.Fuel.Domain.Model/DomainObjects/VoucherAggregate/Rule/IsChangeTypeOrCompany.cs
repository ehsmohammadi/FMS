using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate.Rule
{
    public class IsChangeTypeOrCompany:BusinessRuleBase<VoucherSeting>
    {
        private IVoucherSetingRepository _voucherSetingRepository;
      
        public IsChangeTypeOrCompany(Expression<Func<string, bool>> expression,VoucherSeting voucherSeting) : base(expression,voucherSeting)
        {
            _voucherSetingRepository = ServiceLocator.Current.GetInstance<IVoucherSetingRepository>();
         
        }


        public override void Validate(string typeAction)
        {
            if (IsValidExpression(typeAction))
            {
                var original= _voucherSetingRepository.First(c => c.Id == Entity.Id);
                if (original.CompanyId != Entity.CompanyId || original.VoucherDetailTypeId != Entity.VoucherDetailTypeId ||
                    original.VoucherTypeId == Entity.VoucherTypeId)
                {
                    throw new BusinessRuleException("Company Or Voucher Type Change");
                }
            }
        }
    }
}
