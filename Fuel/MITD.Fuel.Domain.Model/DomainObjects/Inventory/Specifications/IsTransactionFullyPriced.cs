#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.Specifications
{
    public class IsTransactionFullyPriced : SpecificationBase<Inventory_Transaction>
    {
        public IsTransactionFullyPriced() :
            base(t => (t.Status == (byte)TransactionState.FullPriced || t.Status == (byte)TransactionState.Vouchered))
        {
        }
    }
}