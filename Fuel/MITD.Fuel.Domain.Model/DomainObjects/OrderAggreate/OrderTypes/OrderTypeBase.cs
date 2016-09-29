#region

using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class OrderTypeBase
    {
        protected internal virtual void Update(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
        }

        protected internal virtual void Add(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
        }

        //BR_PO26 
        protected virtual void CompanyHaveValidVessel(Order order, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany)
        {
            throw new NotImplementedException();
        }

        //BR_PO20
        public virtual void ValidateGoodSuplierAndTransporter(Order order, GoodFullInfo goodFullInfo)
        {
            throw new NotImplementedException();
        }


        protected  void CompanyHaveValidTransporter(Order order, GoodFullInfo goodFullInfo)
        {
            if (goodFullInfo.GoodTransporters.Count > 0 &&
                goodFullInfo.GoodTransporters.All(c => c.Id != order.TransporterId))
                throw new BusinessRuleException("BR_PO20", "This Good Must Be Buy From Valid Transporter ");
        }

        protected  void GoodHaveValidSupplier(Order order, GoodFullInfo goodFullInfo)
        {
            if (goodFullInfo.GoodSuppliers.Count > 0 && goodFullInfo.GoodSuppliers.All(c => c.Id != order.SupplierId))
                throw new BusinessRuleException("BR_PO20", "This Good Must Be Buy From Valid Supplier ");
        }

        protected  void GoodHaveValidSupplierAndTransporter(Order order, GoodFullInfo goodFullInfo)
        {
            if (goodFullInfo.GoodSuppliers.Count > 0 && goodFullInfo.GoodSuppliers.All(c => c.Id != order.SupplierId))
                throw new BusinessRuleException("BR_PO20", "This Good Must Be Buy From Valid Supplier ");

            if (goodFullInfo.GoodTransporters.Count > 0 &&
                goodFullInfo.GoodTransporters.All(c => c.Id != order.TransporterId))
                throw new BusinessRuleException("BR_PO20", "This Good Must Be Buy From Valid Transporter ");
        }
    }
}