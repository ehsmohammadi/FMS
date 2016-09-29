#region

using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Specifications;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class TransferOrder : OrderTypeBase
    {
        protected internal override void Update(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isTransferValid = new IsTransferValid();
            if (!isTransferValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("BR_PO2", "transfer is not valid");

            CompanyHaveValidVessel(order, fromVesselInCompany, toVesselInCompany);
        }

        protected internal override void Add(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            var isTransferValid = new IsTransferValid();
            if (!isTransferValid.IsSatisfiedBy(order))
                throw new BusinessRuleException("BR_PO2", "transfer is not valid");

            CompanyHaveValidVessel(order, fromVesselInCompany, toVesselInCompany);
        }

        //BR_PO26  
        protected override void CompanyHaveValidVessel(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            if (fromVesselInCompany.CompanyId != order.OwnerId)
                throw new BusinessRuleException("", "Vessel Not In Enterprise Party");

            if (toVesselInCompany.CompanyId != order.ReceiverId)
                throw new BusinessRuleException("", "Vessel Not In Enterprise Party");
        }


        public override void ValidateGoodSuplierAndTransporter(Order order, GoodFullInfo goodFullInfo)
        {
           CompanyHaveValidTransporter(order, goodFullInfo);
        }
    }
}