#region

using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Specifications;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class SupplyForDeliveredVesselOrder : OrderTypeBase
    {
        protected internal override void Update(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isPurchaseValid = new IsSupplyForDeliveredVesselOrderValid();
            if (!isPurchaseValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("BR_PO3", "Supply for Delivered Vessel is not valid");

            if (order.ToVesselInCompany.VesselStateCode != VesselStates.CharterOut)
                throw new BusinessRuleException("", "Vessel is not in Charter Out state.");
        }

        protected internal override void Add(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isPurchaseValid = new IsSupplyForDeliveredVesselOrderValid();
            if (!isPurchaseValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("BR_PO3", "Supply for Delivered Vessel is not valid");

            if (order.ToVesselInCompany.VesselStateCode != VesselStates.CharterOut)
                throw new BusinessRuleException("", "Vessel is not in Charter Out state.");
        }

        protected override void CompanyHaveValidVessel(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {

        }

        public override void ValidateGoodSuplierAndTransporter(Order order, GoodFullInfo goodFullInfo)
        {
            GoodHaveValidSupplier(order, goodFullInfo);
        }
    }
}