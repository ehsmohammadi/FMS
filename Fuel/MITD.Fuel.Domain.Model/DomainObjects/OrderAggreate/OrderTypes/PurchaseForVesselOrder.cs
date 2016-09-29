#region

using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Specifications;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class PurchaseForVesselOrder : OrderTypeBase
    {
        protected internal override void Update(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isPurchaseValid = new IsPurchaseForVesselValid();
            if (!isPurchaseValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("BR_PO3", "Purchase for Vessel is not valid");
        }

        protected internal override void Add(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isPurchaseValid = new IsPurchaseForVesselValid();
            if (!isPurchaseValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("BR_PO3", "Purchase is not valid");
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