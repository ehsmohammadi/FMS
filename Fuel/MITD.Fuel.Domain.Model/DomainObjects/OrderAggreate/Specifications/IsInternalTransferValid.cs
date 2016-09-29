#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.Specifications
{
    //BR_PO24& BR_PO25
    public class IsInternalTransferValid : SpecificationBase<Order>
    {
        public IsInternalTransferValid()
            : base(order => order.OrderType == OrderTypes.InternalTransfer &&
                            order.TransporterId.HasValue && order.TransporterId > 0 &&
                            !order.ReceiverId.HasValue  &&
                            order.OwnerId > 0 &&
                            !order.SupplierId.HasValue &&
                            order.FromVesselInCompanyId.HasValue && order.FromVesselInCompanyId > 0 &&
                            order.ToVesselInCompanyId.HasValue && order.ToVesselInCompanyId > 0)
        {
        }
    }
}