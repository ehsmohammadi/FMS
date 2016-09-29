#region

using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Specifications;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class InternalTransferOrder : OrderTypeBase
    {
        protected internal override void Update(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            ValidateCommonRule(order, fromVesselInCompany, toVesselInCompany);
        }

        protected internal override void Add(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            ValidateCommonRule(order, fromVesselInCompany, toVesselInCompany);

        }

        void ValidateCommonRule(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isInternalTransferValid = new IsInternalTransferValid();
            if (!isInternalTransferValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("", "internal trasfer is not valid");


            CompanyHaveValidVessel(order, fromVesselInCompany, toVesselInCompany);
            TwoCompanyNotSame(fromVesselInCompany, toVesselInCompany);
        }
        //BR_PO26  
        protected override void CompanyHaveValidVessel(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            if (fromVesselInCompany.CompanyId != order.OwnerId)
                throw new BusinessRuleException("", "Vessel Not In Enterprise Party");

            if (toVesselInCompany.CompanyId != order.OwnerId)
                throw new BusinessRuleException("", "Vessel Not In Enterprise Party");
        }

        private void TwoCompanyNotSame(VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            if (fromVesselInCompany.Id == toVesselInCompany.Id)
                throw new BusinessRuleException("", "Twho Vessel Cannot Be One");
        }

        public override void ValidateGoodSuplierAndTransporter(Order order, GoodFullInfo goodFullInfo)
        {
        }
    }
}