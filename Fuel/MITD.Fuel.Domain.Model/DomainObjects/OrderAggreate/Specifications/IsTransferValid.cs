#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.Specifications
{
    //BR_PO2
    public class IsTransferValid : SpecificationBase<Order>
    {
        public IsTransferValid() :
            base(
            order => order.OrderType == OrderTypes.Transfer &&
                     order.TransporterId.HasValue && order.TransporterId > 0 &&
                     order.ReceiverId.HasValue && order.ReceiverId > 0 &&
                     order.OwnerId > 0 &&
                     !order.SupplierId.HasValue &&
                     order.FromVesselInCompanyId.HasValue && order.FromVesselInCompanyId > 0 &&
                     order.ToVesselInCompanyId.HasValue && order.ToVesselInCompanyId > 0)
        {
        }
    }
}