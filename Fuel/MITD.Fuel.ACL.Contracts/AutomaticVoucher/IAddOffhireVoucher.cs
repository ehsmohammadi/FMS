using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.Contracts.AutomaticVoucher
{
    public interface IAddOffhireVoucher:IAutomaticVoucher
    {
        void Execute(Offhire offhire, long userId, VoucherDetailType voucherDetailType);
    }
}
