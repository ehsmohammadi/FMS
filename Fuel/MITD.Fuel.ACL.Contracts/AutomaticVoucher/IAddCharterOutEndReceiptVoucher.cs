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
    public interface IAddCharterOutEndReceiptVoucher : IAutomaticVoucher
    {
        void Execute(CharterOut charterOut, List<Receipt> receipts
            , string receiptWarehouseCode, string receiptNumber, long userId,
           string lineCode, string voyageCode, bool isReform = false);
    }
}
