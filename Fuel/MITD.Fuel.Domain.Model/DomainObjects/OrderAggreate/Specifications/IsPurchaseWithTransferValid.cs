#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.Specifications
{
    //BR_PO14 , BR_PO15
    public class IsPurchaseWithTransferValid : SpecificationBase<Order>
    {
        public IsPurchaseWithTransferValid()
            : base(
                order => order.OrderType == OrderTypes.PurchaseWithTransferOperations &&
                         order.TransporterId.HasValue && order.TransporterId.Value > 0 &&
                         !order.ReceiverId.HasValue &&
                         order.OwnerId > 0 &&
                         order.SupplierId.HasValue && order.SupplierId.Value > 0 &&
                         !order.FromVesselInCompanyId.HasValue &&
                         !order.ToVesselInCompanyId.HasValue)
        {
        }
    }
}