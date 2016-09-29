#region

using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Specifications;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class PurchaseWithTransferOrder : OrderTypeBase
    {
        protected internal override void Update(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isPurchaseWithTransferValid = new IsPurchaseWithTransferValid();
            if (!isPurchaseWithTransferValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("", "purchase with transfer is not valid ");
        }

        protected internal override void Add(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isPurchaseWithTransferValid = new IsPurchaseWithTransferValid();
            if (!isPurchaseWithTransferValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("", "purchase with transfer is not valid ");
        }

        protected override void CompanyHaveValidVessel(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
        }

        public override void ValidateGoodSuplierAndTransporter(Order order, GoodFullInfo goodFullInfo)
        {
            GoodHaveValidSupplierAndTransporter(order, goodFullInfo);
        }
    }
}